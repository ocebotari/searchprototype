using SimonsVossSearchPrototype.DAL.Interfaces;
using SimonsVossSearchPrototype.DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            response.Buildings = await SearchBuildings(term);
            response.Locks = await SearchLocks(term);
            response.Groups = await SearchGroups(term);
            response.Medias = await SearchMedias(term);


            return response;
        }

        /// <summary>
        /// Search Buildings
        /// </summary>
        /// <param name="term">Matched text</param>
        /// <returns>Building Collection</returns>
        public async Task<IEnumerable<Building>> SearchBuildings(string term)
        {
            IEnumerable<Building> response = new List<Building>();

            await Task.Run(() =>
            {
                var collection = _dbStorage.GetCollection<Building>("buildings");
                var buildings = collection.AsQueryable();

                var lockCollection = _dbStorage.GetCollection<Lock>("locks");

                buildings.ToList().ForEach(b => b.CalculateWeight(term, lockCollection.AsQueryable()));

                var buildingsResult = buildings.Where(b => b.SumWeight > 0).OrderByDescending(b => b.SumWeight);

                if (!string.IsNullOrWhiteSpace(term))
                {
                    response = buildingsResult;
                }
                else
                {
                    response = buildings;
                }
            });

            return response;
        }

        /// <summary>
        /// Search Locks
        /// </summary>
        /// <param name="term">Matched text</param>
        /// <returns>Lock Collection</returns>
        public async Task<IEnumerable<Lock>> SearchLocks(string term)
        {
            IEnumerable<Lock> response = new List<Lock>();

            await Task.Run(() =>
            {
                var collection = _dbStorage.GetCollection<Building>("buildings");
                var lockCollection = _dbStorage.GetCollection<Lock>("locks");

                var buildings = collection.AsQueryable();
                var locks = lockCollection.AsQueryable();
                foreach (var item in locks)
                {
                    item.Building = buildings.FirstOrDefault(b => b.Id.Equals(item.BuildingId));
                }

                locks.ToList().ForEach(l => l.CalculateWeight(term));

                var locksResult = locks.Where(l => l.SumWeight > 0).OrderByDescending(l => l.SumWeight);

                response = locksResult;

                if (!string.IsNullOrWhiteSpace(term))
                {
                    response = locksResult;
                }
                else
                {
                    response = locks;
                }
            });

            return response;
        }

        /// <summary>
        /// Search Groups
        /// </summary>
        /// <param name="term">Matched text</param>
        /// <returns>Group Collection</returns>
        public async Task<IEnumerable<Group>> SearchGroups(string term)
        {
            IEnumerable<Group> response = new List<Group>();

            await Task.Run(() =>
            {
                var collection = _dbStorage.GetCollection<Group>("groups");
                var mediaCollection = _dbStorage.GetCollection<Media>("media");

                var groups = collection.AsQueryable();
                var medias = mediaCollection.AsQueryable();

                groups.ToList().ForEach(g => g.CalculateWeight(term, medias));

                var groupsResult = groups.Where(g => g.SumWeight > 0).OrderByDescending(g => g.SumWeight);

                if (string.IsNullOrWhiteSpace(term))
                    response = groups;
                else
                    response = groupsResult;
            });

            return response;
        }

        /// <summary>
        /// Search Medias
        /// </summary>
        /// <param name="term">Matched text</param>
        /// <returns>Media Collection</returns>
        public async Task<IEnumerable<Media>> SearchMedias(string term)
        {
            IEnumerable<Media> response = new List<Media>();

            await Task.Run(() =>
            {
                var collection = _dbStorage.GetCollection<Group>("groups");
                var mediaCollection = _dbStorage.GetCollection<Media>("media");

                var groups = collection.AsQueryable();
                var medias = mediaCollection.AsQueryable();

                foreach (var item in medias)
                {
                    item.Group = groups.FirstOrDefault(g => g.Id.Equals(item.GroupId));
                }

                medias.ToList().ForEach(m => m.CalculateWeight(term));
                var mediasResult = medias.Where(m => m.SumWeight > 0).OrderByDescending(m => m.SumWeight);

                if (!string.IsNullOrWhiteSpace(term))
                {
                    response = mediasResult;
                }
                else
                {
                    response = medias;
                }
            });

            return response;
        }
    }
}