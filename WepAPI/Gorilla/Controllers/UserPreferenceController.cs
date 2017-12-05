using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Entities;
using Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace WepAPI.Controllers
{
   // [Authorize]
    [Produces("application/json")]
    [Route("api/UserPreference")]
    public class UserPreferenceController : Controller
    {
        private readonly IUserPreferenceRepository repository;
        public UserPreferenceController(IUserPreferenceRepository _repository)
        {
            repository = _repository;
        }

        // GET: api/UserPreference/5
        [HttpGet("{username}", Name = "FindUserPreference")]
        public async Task<IActionResult> Find(string username)
        {
            var result = await repository.FindAll(username);
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

        // POST: api/UserPreference
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserPreference userPreference)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var usernameAndSub = await repository.Create(userPreference);
                return CreatedAtAction("Find", new { usernameAndSub }, null);
            } catch (AlreadyThereException)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            } catch (NotFoundException)
            {
                return NotFound();
            }
        } 

        // PUT: api/UserPreference/
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UserPreference userPreference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await repository.Update(userPreference);
            if (!updated)
            {
                return NotFound();
            }
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string username, string subreddit)
        {
            var deleted = await repository.Delete(username, subreddit);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
