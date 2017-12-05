using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Entities;
using Exceptions;

namespace Gorilla.Controllers
{
    [Produces("application/json")]
    [Route("api/SubredditConnection")]
    public class SubredditConnectionController : Controller
    {
        private readonly ISubredditConnectionRepository repository;

        public SubredditConnectionController(ISubredditConnectionRepository _repository)
        {
            repository = _repository;
        }

        // GET: api/UserPreference/5
        [HttpGet("{subredditFromName}", Name = "Find")]
        public async Task<IActionResult> Find(string subredditFromName)
        {
            var result = await repository.Find(subredditFromName);
            if (result == null)
            {
                return NotFound();
            } else if (result.Count() == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet(Name = "Read")]
        public async Task<IActionResult> Read()
        {
            var result = await repository.Read();
            if (result == null)
            {
                return NotFound();
            }
            else if (result.Count() == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet("{subredditFromName}, {subredditToName}", Name = "GetSubredditConnection")]
        public async Task<IActionResult> Get(string subredditFromName, string subredditToName)
        {
            var result = await repository.Get(subredditFromName,subredditToName);
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }

        // POST: api/UserPreference
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SubredditConnection subredditConnection)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await repository.Create(subredditConnection);
                return CreatedAtAction("GetSubredditConnection", new { result }, null);
            }
            catch (AlreadyThereException)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // PUT: api/UserPreference/
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]SubredditConnection subredditConnection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await repository.Update(subredditConnection);
            if (!updated)
            {
                return NotFound();
            }
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string subredditFromName, string subredditToName)
        {
            var deleted = await repository.Delete(subredditFromName, subredditToName);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
