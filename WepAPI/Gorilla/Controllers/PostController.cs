using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Exceptions;
using Entities.GorillaAPI.Interfaces;
using Entities.GorillaEntities;

namespace Gorilla.Controllers
{
    [Produces("application/json")]
    [Route("api/Post")]
    public class PostController : Controller
    {

        private readonly IPostRepository _repository;
        public PostController(IPostRepository repository)
        {
            _repository = repository;
        }
        // GET: api/Post
        [HttpGet("{username}")]
        public async Task<IActionResult> ReadAsync(string username)
        {

            var result = await _repository.ReadAsync(username);
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



        // POST: api/Post
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var id = await _repository.CreateAsync(post);
                return CreatedAtAction(nameof(ReadAsync), new { id }, null);
            }
            catch ( AlreadyThereException )
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            


        }


    }
}
