using SimonsVossSearchPrototype.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.Services
{
    public interface ISearchService<T>
    {
        /// <summary>
        /// Async Search method
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<SearchResult<T>> SearchAsync(string query, int page, int pageSize);

        /// <summary>
        /// Search method
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        SearchResult<T> Search(string query, int page, int pageSize);
    }
}
