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
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserRepository repository;
        public UserController(IUserRepository _repository)
        {
            repository = _repository;
        }
        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> ReadAsync()
        {
            var result = await repository.ReadAsync();
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

        // GET: api/User/5
        [HttpGet("{username}", Name = "Get")]
        public async Task<IActionResult> GetAsync(string username)
        {
            var result = await repository.FindAsync(username);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]User user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var username = await repository.CreateAsync(user);
                return CreatedAtAction(nameof(GetAsync), new { username }, null);
            } catch (AlreadyThereException)
            {
               return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteAsync(string username)
        {
            
                var deleted = await repository.DeleteAsync(username);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody]User user)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await repository.UpdateAsync(user);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
