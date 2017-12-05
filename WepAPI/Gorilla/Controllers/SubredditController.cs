using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Model;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace WepAPI.Controllers
{
    
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SubredditController : Controller
    {
        private readonly ISubredditRepository repository;
        public SubredditController(ISubredditRepository _repository)
        {
            repository = _repository;
        }
        // GET api
        [HttpGet]
        public async Task<IActionResult> ReadAsync()
        {

            var result = await repository.ReadAsync();
            if(result == null)
            {
                return NotFound();
            } else if(result.Count() == 0)
            {
                return NoContent();
            }
            return Ok(result);

        }
        // GET api/Reddit/5
        [HttpGet("{name}", Name = "GetSubreddit")]
        public async Task<IActionResult> GetAsync(string name)
        {
            {
                var result = await repository.FindAsync(name);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
        }

        // POST api/Reddit
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Subreddit subreddit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            try
            {
                var id = await repository.CreateAsync(subreddit);
                return CreatedAtAction(nameof(GetAsync), new { id }, null);
            } catch (AlreadyThereException)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            } 
            

        }


        // DELETE api/Reddit/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteAsync(string name)
        {
            {
                var deleted = await repository.DeleteAsync(name);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
        }
    }
}
