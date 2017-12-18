using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.GorillaAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gorilla.Controllers
{
    [Produces("application/json")]
    [Route("api/Category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repository;
        public CategoryController (ICategoryRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> PutAsync([FromBody] )
        {

            var result = await _repository.;
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

    }
}
