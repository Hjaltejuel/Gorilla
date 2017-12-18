using Entities.GorillaAPI.Interfaces;
using Entities.GorillaEntities;
using Gorilla.Controllers;
using Model.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Gorilla.Test
{
    public class CategoryControllerTest
    {
        [Fact(DisplayName = "Put given invalid CategorySubreddit does not call UpdateAsync")]
        public async Task Put_given_invalid_track_does_not_call_UpdateAsync()
        {
            var repository = new Mock<ICategoryRepository>();

            var controller = new CategoryController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var categorySubreddit = new CategoryObject { _username = "Test", _names = new string[] { "Science", "Food" } };
            await controller.PutAsync(categorySubreddit);

            repository.Verify(r => r.UpdateAsync(categorySubreddit._username, categorySubreddit._names), Times.Never);
        }

        [Fact(DisplayName = "Put given valid CategorySubreddit calls UpdateAsync")]
        public async Task Put_given_valid_track_calls_UpdateAsync()
        {
            var repository = new Mock<ICategoryRepository>();

            
            var controller = new CategoryController(repository.Object);

            var categorySubreddit = new CategoryObject { _username = "Test", _names = new string[] { "Science", "Food" } };
            await controller.PutAsync(categorySubreddit);

            repository.Verify(r => r.UpdateAsync(categorySubreddit._username, categorySubreddit._names));
        }


    }
}
