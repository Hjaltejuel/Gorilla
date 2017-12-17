using System.Linq;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaAPI.Interfaces;
using Entities.GorillaEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gorilla.Controllers
{
    
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserPreferenceController : Controller
    {
        private readonly IUserPreferenceRepository _repository;
        public UserPreferenceController(IUserPreferenceRepository repository)
        {
            _repository = repository;
        }

        // GET: api/UserPreference/5
        [HttpGet("{username}", Name = "FindUserPreference")]
        public async Task<IActionResult> FindAsync(string username)
        {
            var result = await _repository.FindAsync(username);
            if (result == null)
            {
                return NotFound();
            }
            else if (!result.Any())
            {
                return NoContent();
            }
            return Ok(result);
        }

        // POST: api/UserPreference
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]UserPreference userPreference)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var usernameAndSub = await _repository.CreateAsync(userPreference);
                return CreatedAtAction(nameof(FindAsync), new { usernameAndSub }, null);
            } catch (AlreadyThereException)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            } catch (NotFoundException)
            {
                return BadRequest();
            }
        } 

        // PUT: api/UserPreference/
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody]UserPreference userPreference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _repository.UpdateAsync(userPreference);
            if (!updated)
            {
                return NotFound();
            }
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string username, string subreddit)
        {
            var deleted = await _repository.DeleteAsync(username, subreddit);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
