using SimonsVossSearchPrototype.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Implementations
{
    public class RepositoryCollection<T> : IRepositoryCollection<T>
    {
        private readonly string _path;
        private readonly string _idField;
        private readonly Lazy<List<T>> _data;

        public RepositoryCollection(Lazy<List<T>> data, string path, string idField)
        {
            _path = path;
            _idField = idField;
            _data = data;
        }

        public int Count => _data.Value.Count;

        public IEnumerable<T> AsQueryable() => _data.Value.AsQueryable();

        public IEnumerable<T> Find(Predicate<T> query)
        {
            return _data.Value.Where(t => query(t));
        }

        public IEnumerable<T> Find(string text, bool caseSensitive = false)
        {
            if (string.IsNullOrWhiteSpace(text))
                return _data.Value;

            return _data.Value.Where(t => Extensions.FullTextSearch(t, text, caseSensitive));
        }
    }
}
