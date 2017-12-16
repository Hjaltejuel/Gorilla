
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
    [Route("api/[controller]")]
    public class SubredditConnectionController : Controller
    {
        private readonly ISubredditConnectionRepository repository;

        public SubredditConnectionController(ISubredditConnectionRepository _repository)
        {
            repository = _repository;
        }

        // GET: api/UserPreference/5
        [HttpGet("{subredditFromName}", Name = "Find")]
        public async Task<IActionResult> FindAsync(string subredditFromName)
        {
            var result = await repository.FindAsync(subredditFromName);
            if (result == null)
            {
                return NotFound();
            } else if (result.Count() == 0)
            {
                return NoContent();
            }

            return Ok(result);
        }
        [HttpGet("GetAllPrefs")]
        public async Task<IActionResult> GetAllPrefs([FromBody]string[] ListOfPrefs)
            {
            var result = await repository.GetAllPrefs(ListOfPrefs);
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
        [HttpGet(Name = "Read")]
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

        [HttpGet("{subredditFromName}, {subredditToName}", Name = "GetSubredditConnection")]
        public async Task<IActionResult> GetAsync(string subredditFromName, string subredditToName)
        {
            var result = await repository.GetAsync(subredditFromName,subredditToName);
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }

        // POST: api/UserPreference
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]SubredditConnection subredditConnection)
        {
          

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await repository.CreateAsync(subredditConnection);
                return CreatedAtAction(nameof(GetAsync), new { result }, null);
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
        public async Task<IActionResult> PutAsync([FromBody]SubredditConnection subredditConnection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await repository.UpdateAsync(subredditConnection);
            if (!updated)
            {
                return NotFound();
            }
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{subredditFromName},{subredditToName}")]
        public async Task<IActionResult> DeleteAsync(string subredditFromName, string subredditToName)
        {
            var deleted = await repository.DeleteAsync(subredditFromName, subredditToName);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
