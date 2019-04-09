using SimonsVossSearchPrototype.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.Services
{
    public interface ISearchService
    {
        /// <summary>
        /// Search from all entities
        /// </summary>
        /// <param name="term">Matched text</param>
        /// <returns>Response with Entity Collection</returns>
        Task<SearchResponse> Search(string term);
        /// <summary>
        /// Search Buildings
        /// </summary>
        /// <param name="term">Matched text</param>
        /// <returns>Building Collection</returns>
        Task<IEnumerable<Building>> SearchBuildings(string term);
        /// <summary>
        /// Search Locks
        /// </summary>
        /// <param name="term">Matched text</param>
        /// <returns>Lock Collection</returns>
        Task<IEnumerable<Lock>> SearchLocks(string term);
        /// <summary>
        /// Search Groups
        /// </summary>
        /// <param name="term">Matched text</param>
        /// <returns>Group Collection</returns>
        Task<IEnumerable<Group>> SearchGroups(string term);
        /// <summary>
        /// Search Medias
        /// </summary>
        /// <param name="term">Matched text</param>
        /// <returns>Media Collection</returns>
        Task<IEnumerable<Media>> SearchMedias(string term);
    }
}
