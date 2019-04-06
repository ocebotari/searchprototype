using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Models
{
    public class Lock: Entity<Guid>
    {
        public Lock()
        {
            Id = Guid.NewGuid();
            WeightList = new List<WeightValue>()
            {
                new WeightValue { Name = "type", W = 3 },
                new WeightValue { Name = "name", W = 10 },
                new WeightValue { Name = "serialNumber", W = 8 },
                new WeightValue { Name = "floor", W = 6 },
                new WeightValue { Name = "roomNumber", W = 6 },
                new WeightValue { Name = "description", W = 6 }
            };
        }

        [JsonIgnore]
        public List<WeightValue> WeightList { get; set; }
        
        [JsonProperty(Order = 1)]
        public Guid? BuildingId { get; set; }
        [JsonProperty(Order = 2)]
        public string Type { get; set; }
        [JsonProperty(Order = 3)]
        public string Name { get; set; }
        [JsonProperty(Order = 4)]
        public string Description { get; set; }
        [JsonProperty(Order = 5)]
        public string SerialNumber { get; set; }
        [JsonProperty(Order = 6)]
        public string Floor { get; set; }
        [JsonProperty(Order = 7)]
        public string RoomNumber { get; set; }
        [JsonProperty(Order = 8)]
        public Building Building { get; set; }
        [JsonProperty(Order = 9)]
        public int SumWeight { get; set; }

        public void CalculateWeight(string term)
        {
            var regex = new Regex(term, RegexOptions.IgnoreCase);

            if (!string.IsNullOrWhiteSpace(Name) && regex.IsMatch(Name))
                SumWeight += WeightList.Where(w => w.Name == "name").Select(w => w.W).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(Type) && regex.IsMatch(Type))
                SumWeight += WeightList.Where(w => w.Name == "type").Select(w => w.W).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(SerialNumber) && regex.IsMatch(SerialNumber))
                SumWeight += WeightList.Where(w => w.Name == "serialNumber").Select(w => w.W).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(Floor) && regex.IsMatch(Floor))
                SumWeight += WeightList.Where(w => w.Name == "floor").Select(w => w.W).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(RoomNumber) && regex.IsMatch(RoomNumber))
                SumWeight += WeightList.Where(w => w.Name == "roomNumber").Select(w => w.W).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(Description) && regex.IsMatch(Description))
                SumWeight += WeightList.Where(w => w.Name == "description").Select(w => w.W).FirstOrDefault();
        }
    }
}
