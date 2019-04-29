using SimonsVossSearchPrototype.DAL.Implementations;
using SimonsVossSearchPrototype.DAL.Interfaces;
using SimonsVossSearchPrototype.DAL.Models;
using SimonsVossSearchPrototype.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SimonsVossSearchPrototype.Controllers
{
    [RoutePrefix("api/search")]
    public class SearchController : ApiController
    {
        private readonly IDataStorage _dbStorage;
        private readonly ISearchService<Building> _buildingService;
        private readonly ISearchService<Lock> _lockService;
        private readonly ISearchService<Group> _groupService;
        private readonly ISearchService<Media> _mediaService;

        public SearchController(IDataStorage dbStorage,
            ISearchService<Building> buildingService,
            ISearchService<Lock> lockService,
            ISearchService<Group> groupService,
            ISearchService<Media> mediaService)
        {
            _dbStorage = dbStorage;
            _buildingService = buildingService;
            _lockService = lockService;
            _groupService = groupService;
            _mediaService = mediaService;
        }

        // GET api/search/locks/0a1e6f38-6076-4da8-8d6c-87356f975baf
        [Route("locks/{id:guid}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var collection = await Task.Run(() => _dbStorage.GetCollection<Lock>("locks"));

            var lockItem = collection.AsQueryable().FirstOrDefault(l => l.Id.Equals(id));

            return Ok(new { lockItem });
        }

        // GET api/search/locks/0a1e6f38-6076-4da8-8d6c-87356f975baf
        [Route("buildings/{id:guid}")]
        public async Task<IHttpActionResult> GetBuildings(Guid id)
        {
            var collection = await Task.Run(() => _dbStorage.GetCollection<Building>("buildings"));

            var building = collection.AsQueryable().FirstOrDefault(b => b.Id.Equals(id));

            return Ok(new { building });
        }

        // GET api/locks/search/term
        //[HttpGet]
        //[Route("search")]
        //public async Task<IHttpActionResult> Search([FromUri]string term)
        //{
        //    //var buildingCollection = await Task.Run(() => dbStorage.GetCollection<Building>("buildings"));            
        //    //var lockCollection = await Task.Run(() => dbStorage.GetCollection<Lock>("locks"));
        //    //var groupCollection = await Task.Run(() => dbStorage.GetCollection<Group>("groups"));
        //    //var mediaCollection = await Task.Run(() => dbStorage.GetCollection<Media>("medias"));

        //    ////building
        //    //var list = buildingCollection.AsQueryable().ToList();
        //    //list.ForEach(b => b.CalculateWeight(term, lockCollection.AsQueryable()));

        //    //var bsearch = list.Where(b => b.SumWeight > 0).OrderByDescending(b => b.SumWeight);

        //    ////locks
        //    //var locksQuery = lockCollection.AsQueryable();
        //    //locksQuery.ToList().ForEach(l => l.CalculateWeight(term));
        //    //var lsearch = locksQuery.Where(l => l.SumWeight > 0).OrderByDescending(l => l.SumWeight);

        //    ////groups
        //    //var groupsQuery = groupCollection.AsQueryable();
        //    //groupsQuery.ToList().ForEach(g => g.CalculateWeight(term, mediaCollection.AsQueryable()));
        //    //var gsearch = groupsQuery.Where(g => g.SumWeight > 0).OrderByDescending(g => g.SumWeight);


        //    ////medias
        //    //var mediasQuery = mediaCollection.AsQueryable();
        //    //mediasQuery.ToList().ForEach(m => m.CalculateWeight(term));
        //    //var msearch = mediasQuery.Where(m => m.SumWeight > 0).OrderByDescending(m => m.SumWeight);

        //    //foreach (var item in locks)
        //    //{
        //    //    var buildingCollection = dbStorage.GetCollection<Building>("buildings");
        //    //    if (buildingCollection != null)
        //    //    {
        //    //        item.Building = buildingCollection.AsQueryable().FirstOrDefault(b => b.Id.Equals(item.BuildingId));
        //    //    }
        //    //}

        //    var result = await service.Search(term);

        //    var bResult = new { count = result.Buildings.Count(), list = result.Buildings };
        //    var lResult = new { count = result.Locks.Count(), list = result.Locks };
        //    var gResult = new { count = result.Groups.Count(), list = result.Groups };
        //    var mResult = new { count = result.Medias.Count(), list = result.Medias };

        //    return Ok(new { buildings = bResult, locks = lResult, groups = gResult, medias = mResult });
        //}

        // GET api/locks/search/buildings/term
        [HttpGet]
        [Route("buildings", Name ="BuildingsSearchApi")]
        public async Task<IHttpActionResult> SearchBuildings([FromUri]string term, int page = 1)
        {
            var result = await _buildingService.SearchAsync(term, page, 10);

            var buildings = new { result };

            return Ok(buildings);
        }

        // GET api/locks/search/term
        [HttpGet]
        [Route("locks", Name = "LocksSearchApi")]
        public async Task<IHttpActionResult> SearchLocks([FromUri]string term, int page = 1)
        {
            var result = await _lockService.SearchAsync(term, page, 10);

            var locks = new { result };

            return Ok(locks);
        }

        // GET api/locks/search/term
        [HttpGet]
        [Route("groups", Name = "GroupsSearchApi")]
        public async Task<IHttpActionResult> SearchGroups([FromUri]string term, int page = 1)
        {
            var result = await _groupService.SearchAsync(term, page, 10);
            var groups = new { result };

            return Ok(groups);
        }

        // GET api/locks/search/term
        [HttpGet]
        [Route("medias", Name = "MediasSearchApi")]
        public async Task<IHttpActionResult> SearchMedias([FromUri]string term, int page = 1)
        {
            var result = await _mediaService.SearchAsync(term, page, 10);

            var medias = new { result };

            return Ok(medias);
        }
    }
}
