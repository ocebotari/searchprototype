using SimonsVossSearchPrototype.DAL.Interfaces;
using SimonsVossSearchPrototype.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SimonsVossSearchPrototype.Services
{
    public class GroupSearchService : BaseSearchService, ISearchService<Group>
    {
        public GroupSearchService(IDataStorage dbStorage) : base(dbStorage)
        { }

        public SearchResult<Group> Search(string query, int page, int pageSize)
        {
            var collection = _dbStorage.GetCollection<Group>("groups");
            var mediaCollection = _dbStorage.GetCollection<Media>("media");

            var groups = collection.AsQueryable();
            var medias = mediaCollection.AsQueryable();

            groups.ToList().ForEach(g => g.CalculateWeight(query, medias));

            var groupsResult = groups.Where(g => g.SumWeight > 0).OrderByDescending(g => g.SumWeight);

            IEnumerable<Group> result = new List<Group>();
            if (string.IsNullOrWhiteSpace(query))
                result = groups;
            else
                result = groupsResult;

            return new SearchResult<Group>
            {
                Total = (int)result.Count(),
                Page = page,
                Results = result
            };
        }

        public async Task<SearchResult<Group>> SearchAsync(string query, int page, int pageSize)
        {
            var result = await Task.Run(() =>
            {
                return Search(query, page, pageSize);
            });

            return result;
        }
    }
}