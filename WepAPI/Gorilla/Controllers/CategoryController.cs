using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.GorillaAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.GorillaEntities;

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
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] CategoryObject categoryObject  )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _repository.UpdateAsync(categoryObject._username,categoryObject._names);
            
            return Ok(result);
        }

    }
}
