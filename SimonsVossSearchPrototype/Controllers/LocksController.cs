using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimonsVossSearchPrototype.Controllers
{
    [RoutePrefix("api/locks")]
    public class LocksController : ApiController
    {
        // GET api/locks
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            await Task.Delay(100);

            return Ok();
        }

        // GET api/locks/0a1e6f38-6076-4da8-8d6c-87356f975baf
        public IHttpActionResult Get(Guid id)
        {
            return Ok();
        }

        // GET api/locks/search/term
        [Route("search/{term}")]
        public async Task<IHttpActionResult> Search(string term)
        {
            await Task.Delay(100);
            return Ok();
        }

    }
}
