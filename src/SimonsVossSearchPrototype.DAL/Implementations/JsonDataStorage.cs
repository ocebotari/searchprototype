using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SimonsVossSearchPrototype.DAL.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Implementations
{
    public class JsonDataStorage : IDataStorage
    {
        private readonly string _filePath;
        private readonly string _keyProperty;
        private readonly bool _reloadBeforeGetCollection;
        private readonly Func<JObject, string> _toJsonFunc;
        private readonly Func<string, string> _convertPathToCorrectCamelCase;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly ExpandoObjectConverter _converter = new ExpandoObjectConverter();
        private readonly JsonSerializerSettings _serializerSettings;

        private JObject _jsonData;

        public JsonDataStorage(string path, bool useLowerCamelCase = true, string keyProperty = null, bool reloadBeforeGetCollection = false)
        {
            _filePath = path;
            _serializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            _toJsonFunc = useLowerCamelCase
                        ? new Func<JObject, string>(data =>
                        {
                            // Serializing JObject ignores SerializerSettings, so we have to first deserialize to ExpandoObject and then serialize
                            // http://json.codeplex.com/workitem/23853
                            var jObject = JsonConvert.DeserializeObject<ExpandoObject>(data.ToString());
                            return JsonConvert.SerializeObject(jObject, Formatting.Indented, _serializerSettings);
                        })
                        : new Func<JObject, string>(s => s.ToString());

            _convertPathToCorrectCamelCase = useLowerCamelCase
                                ? new Func<string, string>(s => string.Concat(s.Select((x, i) => i == 0 ? char.ToLower(x).ToString() : x.ToString())))
                                : new Func<string, string>(s => s);

            _keyProperty = keyProperty ?? (useLowerCamelCase ? "id" : "Id");

            _reloadBeforeGetCollection = reloadBeforeGetCollection;

            _jsonData = JObject.Parse(ReadJsonFromFile(path));
        }

        public void Dispose()
        {
            if (_cts.IsCancellationRequested == false)
            {
                _cts.Cancel();
            }
        }

        public IRepositoryCollection<T> GetCollection<T>(string name = null) where T : class
        {
            // NOTE 27.6.2017: Should this be new Func<JToken, T>(e => e.ToObject<T>())?
            var readConvert = new Func<JToken, T>(e => JsonConvert.DeserializeObject<T>(e.ToString()));
            var insertConvert = new Func<T, T>(e => e);
            var createNewInstance = new Func<T>(() => Activator.CreateInstance<T>());

            return GetCollection(name ?? _convertPathToCorrectCamelCase(typeof(T).Name), readConvert);
        }

        public T GetItem<T>(string key)
        {
            if (_reloadBeforeGetCollection)
            {
                // This might be a bad idea especially if the file is in use, as this can take a long time
                _jsonData = JObject.Parse(ReadJsonFromFile(_filePath));
            }

            var token = _jsonData[key];

            if (token == null)
            {
                if (Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    return default(T);
                }

                throw new KeyNotFoundException();
            }

            return token.ToObject<T>();
        }

        private IRepositoryCollection<T> GetCollection<T>(string path, Func<JToken, T> readConvert)
        {
            var data = new Lazy<List<T>>(() =>
            {
                lock (_jsonData)
                {
                    if (_reloadBeforeGetCollection)
                    {
                        // This might be a bad idea especially if the file is in use, as this can take a long time
                        _jsonData = JObject.Parse(ReadJsonFromFile(_filePath));
                    }

                    return _jsonData[path]?
                                .Children()
                                .Select(e => readConvert(e))
                                .ToList()
                                ?? new List<T>();
                }
            });

            return new RepositoryCollection<T>(data, path, _keyProperty);
        }

        private string ReadJsonFromFile(string path)
        {
            Stopwatch sw = null;
            string json = "{}";

            while (true)
            {
                try
                {
                    json = File.ReadAllText(path);
                    break;
                }
                catch (FileNotFoundException)
                {
                    File.WriteAllText(path, json);
                    break;
                }
                catch (IOException e) when (e.Message.Contains("because it is being used by another process"))
                {
                    // If some other process is using this file, retry operation unless elapsed times is greater than 10sec
                    sw = sw ?? Stopwatch.StartNew();
                    if (sw.ElapsedMilliseconds > 10000)
                        throw;
                }
            }

            return json;
        }
    }
}
