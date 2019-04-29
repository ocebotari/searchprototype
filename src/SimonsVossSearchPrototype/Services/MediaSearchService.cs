using SimonsVossSearchPrototype.DAL.Interfaces;
using SimonsVossSearchPrototype.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SimonsVossSearchPrototype.Services
{
    public class MediaSearchService : BaseSearchService, ISearchService<Media>
    {
        public MediaSearchService(IDataStorage dbStorage) : base(dbStorage)
        { }

        public SearchResult<Media> Search(string query, int page, int pageSize)
        {
            var collection = _dbStorage.GetCollection<Group>("groups");
            var mediaCollection = _dbStorage.GetCollection<Media>("media");

            var groups = collection.AsQueryable();
            var medias = mediaCollection.AsQueryable();

            foreach (var item in medias)
            {
                item.Group = groups.FirstOrDefault(g => g.Id.Equals(item.GroupId));
            }

            var pageNumber = page > 0 ? page - 1 : page;
            IEnumerable<Media> result = medias;
            if (!string.IsNullOrWhiteSpace(query))
            {
                medias.ToList().ForEach(m => m.CalculateWeight(query));
                result = medias.Where(m => m.SumWeight > 0).OrderByDescending(m => m.SumWeight);
            }

            return new SearchResult<Media>
            {
                Total = (int)result.Count(),
                Page = page,
                PageSize = pageSize,
                Results = result.Skip(pageNumber * pageSize).Take(pageSize)
            };
        }

        public async Task<SearchResult<Media>> SearchAsync(string query, int page, int pageSize)
        {
            var result = await Task.Run(() =>
            {
                return Search(query, page, pageSize);
            });

            return result;
        }
    }
}