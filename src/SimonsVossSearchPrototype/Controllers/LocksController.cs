using SimonsVossSearchPrototype.DAL.Implementations;
using SimonsVossSearchPrototype.DAL.Interfaces;
using SimonsVossSearchPrototype.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SimonsVossSearchPrototype.Controllers
{
    [RoutePrefix("api/locks")]
    public class LocksController : ApiController
    {
        IDataStorage dbStorage;
        
        public LocksController()
        {
            var path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["json-data-file"]);
            dbStorage = new JsonDataStorage(path);
        }

        // GET api/locks
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var collection = await Task.Run(() => dbStorage.GetCollection<Lock>("locks"));

            var locks = collection.AsQueryable();
            foreach (var item in locks)
            {
                var buildingCollection = dbStorage.GetCollection<Building>("buildings");
                if(buildingCollection != null)
                {
                    item.Building = buildingCollection.AsQueryable().FirstOrDefault(b => b.Id.Equals(item.BuildingId));
                }
            }

            var count = collection.Count;

            return Ok(new { count, locks });
        }

        // GET api/locks/0a1e6f38-6076-4da8-8d6c-87356f975baf
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var collection = await Task.Run(() => dbStorage.GetCollection<Lock>("locks"));

            var lockItem = collection.AsQueryable().FirstOrDefault(l => l.Id.Equals(id));

            return Ok(new { lockItem });
        }

        // GET api/locks/search/term
        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search([FromUri]string term)
        {
            var collection = await Task.Run(() => dbStorage.GetCollection<Lock>("locks"));
            var locks = collection.Find(term, true);

            foreach (var item in locks)
            {
                var buildingCollection = dbStorage.GetCollection<Building>("buildings");
                if (buildingCollection != null)
                {
                    item.Building = buildingCollection.AsQueryable().FirstOrDefault(b => b.Id.Equals(item.BuildingId));
                }
            }

            //await Task.Delay(100);
            return Ok(new { count = locks.Count(), locks });
        }

    }
}
