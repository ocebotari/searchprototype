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
            var collection = await Task.Run(() => dbStorage.GetCollection<Lock>());

            return Ok(collection);
        }

        // GET api/locks/0a1e6f38-6076-4da8-8d6c-87356f975baf
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var collection = await Task.Run(() => dbStorage.GetCollection<Lock>());

            var lockItem = dbStorage.GetItem<Lock>(id.ToString());
            var lockItem1 = collection.AsQueryable().FirstOrDefault(l => l.Id.Equals(id));

            return Ok(new { lockItem, lockItem1});
        }

        // GET api/locks/search/term
        [Route("search/{term}")]
        public async Task<IHttpActionResult> Search(string term)
        {
            var collection = await Task.Run(() => dbStorage.GetCollection<Lock>());
            var matched = collection.Find(term, true);

            //await Task.Delay(100);
            return Ok(matched);
        }

    }
}
