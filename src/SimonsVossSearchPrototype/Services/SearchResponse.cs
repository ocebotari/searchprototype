using Newtonsoft.Json;
using SimonsVossSearchPrototype.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonsVossSearchPrototype.Services
{
    public class SearchResponse
    {
        public IEnumerable<Building> Buildings { get; set; }
        public IEnumerable<Lock> Locks { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<Media> Medias { get; set; }
    }
}