using SimonsVossSearchPrototype.DAL.Interfaces;
using SimonsVossSearchPrototype.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SimonsVossSearchPrototype.Services
{
    public class BuildingSearchService : BaseSearchService, ISearchService<Building>
    {
        public BuildingSearchService(IDataStorage dbStorage): base(dbStorage)
        {
        }

        public SearchResult<Building> Search(string query, int page, int pageSize)
        {
            var collection = _dbStorage.GetCollection<Building>("buildings");
            var buildings = collection.AsQueryable();

            var lockCollection = _dbStorage.GetCollection<Lock>("locks");

            buildings.ToList().ForEach(b => b.CalculateWeight(query, lockCollection.AsQueryable()));

            var buildingsResult = buildings.Where(b => b.SumWeight > 0).OrderByDescending(b => b.SumWeight);

            IEnumerable<Building> result = new List<Building>();
            if (!string.IsNullOrWhiteSpace(query))
            {
                result = buildingsResult;
            }
            else
            {
                result = buildings;
            }

            return new SearchResult<Building>
            {
                Total = (int)result.Count(),
                Page = page,
                Results = result
            };
        }

        public async Task<SearchResult<Building>> SearchAsync(string query, int page, int pageSize)
        {
            var result = await Task.Run(() =>
            {
                return Search(query, page, pageSize);
            });

            return result;
        }
    }
}