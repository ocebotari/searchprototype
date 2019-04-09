using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonsVossSearchPrototype.Services
{
    public class SearchResult<T>
    {
        public int Total { get; set; }

        public int Page { get; set; }

        public IEnumerable<T> Results { get; set; }
    }
}