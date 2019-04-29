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

        public int PageSize { get; set; }

        public int PageCount
        {
            get
            {
                return Total > 0 ? (int)Math.Ceiling(Total / 10f) : 0;
            }
        }

        public IEnumerable<T> Results { get; set; }
    }
}