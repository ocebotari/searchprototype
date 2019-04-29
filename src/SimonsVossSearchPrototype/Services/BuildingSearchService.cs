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

            var pageNumber = page > 0 ? page - 1 : page;
            IEnumerable<Building> result = buildings;
            if (!string.IsNullOrWhiteSpace(query))
            {
                buildings.ToList().ForEach(b => b.CalculateWeight(query, lockCollection.AsQueryable()));

                result = buildings.Where(b => b.SumWeight > 0).OrderByDescending(b => b.SumWeight);
            }

            return new SearchResult<Building>
            {
                Total = (int)result.Count(),
                Page = page,
                PageSize = pageSize,
                Results = result.Skip(pageNumber * pageSize).Take(pageSize)
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