using SimonsVossSearchPrototype.DAL.Interfaces;
using SimonsVossSearchPrototype.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SimonsVossSearchPrototype.Services
{
    public class SearchService : ISearchService
    {
        private readonly IDataStorage _dbStorage;

        public SearchService(IDataStorage dbStorage)
        {
            _dbStorage = dbStorage;
        }

        public async Task<SearchResponse> Search(string term)
        {
            var response = new SearchResponse();

            await Task.Run(() =>
            {
                var buildingCollection = _dbStorage.GetCollection<Building>("buildings");
                var lockCollection = _dbStorage.GetCollection<Lock>("locks");
                var groupCollection = _dbStorage.GetCollection<Group>("groups");
                var mediaCollection = _dbStorage.GetCollection<Media>("media");

                var buildings = buildingCollection.AsQueryable();

                var locks = lockCollection.AsQueryable();
                foreach (var item in locks)
                {
                    item.Building = buildings.FirstOrDefault(b => b.Id.Equals(item.BuildingId));
                }

                var groups = groupCollection.AsQueryable();

                var medias = mediaCollection.AsQueryable();
                foreach (var item in medias)
                {
                    item.Group = groups.FirstOrDefault(g => g.Id.Equals(item.GroupId));
                }

                if (!string.IsNullOrWhiteSpace(term))
                {
                    buildings.ToList().ForEach(b => b.CalculateWeight(term, lockCollection.AsQueryable()));
                    locks.ToList().ForEach(l => l.CalculateWeight(term));
                    groups.ToList().ForEach(g => g.CalculateWeight(term, mediaCollection.AsQueryable()));
                    medias.ToList().ForEach(m => m.CalculateWeight(term));


                    var buildingsResult = buildings.Where(b => b.SumWeight > 0).OrderByDescending(b => b.SumWeight);
                    var locksResult = locks.Where(l => l.SumWeight > 0).OrderByDescending(l => l.SumWeight);
                    var groupsResult = groups.Where(g => g.SumWeight > 0).OrderByDescending(g => g.SumWeight);
                    var mediasResult = medias.Where(m => m.SumWeight > 0).OrderByDescending(m => m.SumWeight);

                    response.Buildings = buildingsResult;
                    response.Locks = locksResult;
                    response.Groups = groupsResult;
                    response.Medias = mediasResult;
                }
                else
                {
                    response.Buildings = buildings;
                    response.Locks = locks;
                    response.Groups = groups;
                    response.Medias = medias;
                }
            });

            return response;
        }
    }
}