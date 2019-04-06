using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Models
{
    public class Group: Entity<Guid>
    {
        public Group()
        {
            Id = Guid.NewGuid();
            WeightList = new List<WeightValue>()
            {
                new WeightValue { Name = "name", W = 9, WT = 8 },
                new WeightValue { Name = "description", W = 5, WT = 0 }
            };
        }

        [JsonIgnore]
        public List<WeightValue> WeightList { get; set; }
        [JsonProperty(Order = 3)]
        public int SumWeight { get; set; }
        [JsonProperty(Order = 1)]
        public string Name { get; set; }
        [JsonProperty(Order = 2)]
        public string Description { get; set; }

        public void CalculateWeight(string term, IEnumerable<Media> medias)
        {
            var regex = new Regex(term, RegexOptions.IgnoreCase);
            if (!string.IsNullOrWhiteSpace(Name) && regex.IsMatch(Name))
            {
                SumWeight += WeightList.Where(w => w.Name == "name").Select(w => w.W).FirstOrDefault();
                if (medias.Count(l => l.GroupId.Equals(Id)) > 0)
                {
                    SumWeight += WeightList.Where(w => w.Name == "name").Select(w => w.WT).FirstOrDefault();
                }
            }

            if (!string.IsNullOrWhiteSpace(Description) && regex.IsMatch(Description))
            {
                SumWeight += WeightList.Where(w => w.Name == "description").Select(w => w.W).FirstOrDefault();
                if (medias.Count(l => l.GroupId.Equals(Id)) > 0)
                {
                    SumWeight += WeightList.Where(w => w.Name == "description").Select(w => w.WT).FirstOrDefault();
                }
            }
        }
    }
}
