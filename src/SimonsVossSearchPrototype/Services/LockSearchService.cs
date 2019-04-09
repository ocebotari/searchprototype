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

            locks.ToList().ForEach(l => l.CalculateWeight(query));

            var locksResult = locks.Where(l => l.SumWeight > 0).OrderByDescending(l => l.SumWeight);

            IEnumerable<Lock> result = new List<Lock>();
            if (!string.IsNullOrWhiteSpace(query))
            {
                result = locksResult;
            }
            else
            {
                result = locks;
            }

            return new SearchResult<Lock>
            {
                Total = (int)result.Count(),
                Page = page,
                Results = result
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