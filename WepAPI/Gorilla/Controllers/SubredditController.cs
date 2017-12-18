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
    public class SubredditController : Controller
    {
        private readonly ISubredditRepository _repository;
        public SubredditController(ISubredditRepository repository)
        {
            _repository = repository;
        }
        // GET api
        [HttpGet]
        public async Task<IActionResult> ReadAsync()
        {

            var result = await _repository.ReadAsync();
            if(result == null)
            {
                return NotFound();
            } else if(!result.Any())
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
                var result = await _repository.FindAsync(name);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
        }

        // GET api/Reddit/5
        [HttpGet("like/{like}")]
        public async Task<IActionResult> GetLike(string like)
        {
            {
                var result = await _repository.GetLikeAsync(like);
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
                var id = await _repository.CreateAsync(subreddit);
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
                var deleted = await _repository.DeleteAsync(name);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
        }
    }
}
