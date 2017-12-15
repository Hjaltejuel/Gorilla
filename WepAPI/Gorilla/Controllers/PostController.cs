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
using Entities.GorillaAPI.Interfaces;

namespace Gorilla.Controllers
{
    [Produces("application/json")]
    [Route("api/Post")]
    public class PostController : Controller
    {

        private readonly IPostRepository repository;
        public PostController(IPostRepository _repository)
        {
            repository = _repository;
        }
        // GET: api/Post
        [HttpGet("{username}")]
        public async Task<IActionResult> ReadAsync(string username)
        {

            var result = await repository.ReadAsync(username);
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
                var id = await repository.CreateAsync(post);
                return CreatedAtAction(nameof(ReadAsync), new { id }, null);
            }
            catch ( AlreadyThereException )
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            


        }


    }
}
