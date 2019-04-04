using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.DAL.Interfaces
{
    /// <summary>
    /// Collection of items
    /// </summary>
    /// <typeparam name="T">Type of item</typeparam>
    public interface IRepositoryCollection<T>
    {
        /// <summary>
        /// Collection as queryable
        /// </summary>
        /// <returns>All items in queryable collection</returns>
        IEnumerable<T> AsQueryable();

        /// <summary>
        /// Find all items matching the query
        /// </summary>
        /// <param name="query">Filter predicate</param>
        /// <returns>Items matching the query</returns>
        IEnumerable<T> Find(Predicate<T> query);

        /// <summary>
        /// Full-text search
        /// </summary>
        /// <param name="text">Search text</param>
        /// <param name="caseSensitive">Is the search case sensitive</param>
        /// <returns>Items mathcing the search text</returns>
        IEnumerable<T> Find(string text, bool caseSensitive = false);

        /// <summary>
        /// Number of items in the collection
        /// </summary>
        int Count { get; }
    }
}
