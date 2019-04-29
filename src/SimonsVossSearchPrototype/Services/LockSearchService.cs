using SimonsVossSearchPrototype.DAL.Interfaces;
using SimonsVossSearchPrototype.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SimonsVossSearchPrototype.Services
{
    public class LockSearchService : BaseSearchService, ISearchService<Lock>
    {
        public LockSearchService(IDataStorage dbStorage) : base(dbStorage)
        {}

        public SearchResult<Lock> Search(string query, int page, int pageSize)
        {

            var collection = _dbStorage.GetCollection<Building>("buildings");
            var lockCollection = _dbStorage.GetCollection<Lock>("locks");

            var buildings = collection.AsQueryable();
            var locks = lockCollection.AsQueryable();
            foreach (var item in locks)
            {
                item.Building = buildings.FirstOrDefault(b => b.Id.Equals(item.BuildingId));
            }

            var pageNumber = page > 0 ? page - 1 : page;
            IEnumerable<Lock> result = locks;
            if (!string.IsNullOrWhiteSpace(query))
            {
                locks.ToList().ForEach(l => l.CalculateWeight(query));

                result = locks.Where(l => l.SumWeight > 0).OrderByDescending(l => l.SumWeight);
            }

            return new SearchResult<Lock>
            {
                Total = (int)result.Count(),
                Page = page,
                PageSize = pageSize,
                Results = result.Skip(pageNumber * pageSize).Take(pageSize)
            };
        }

        public async Task<SearchResult<Lock>> SearchAsync(string query, int page, int pageSize)
        {
            var result = await Task.Run(() =>
            {
                return Search(query, page, pageSize); 
            });

            return result;
        }
    }
}