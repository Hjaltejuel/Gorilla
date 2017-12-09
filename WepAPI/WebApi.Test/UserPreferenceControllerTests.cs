using Entities;
using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WepAPI.Controllers;
using Xunit;

namespace WebApi.Test
{
    public class UserPreferenceControllerTest
    {
        [Fact(DisplayName = "Controller has AuthorizeAttribute")]
        public void Controller_has_AuthorizeAttribute()
        {
            var type = typeof(UserPreferenceController);

            var authorizeAttribute = type.CustomAttributes.FirstOrDefault(c => c.AttributeType == typeof(AuthorizeAttribute));

            Assert.NotNull(authorizeAttribute);
        }

        [Fact(DisplayName = "Find returns Ok with UserPreference")]
        public async Task Find_returns_Ok_with_tracks()
        {
            var SubredditConnections = new UserPreference[1] { new UserPreference {Username = "test", SubredditName = "name" } };

            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.FindAsync("test")).ReturnsAsync(SubredditConnections);

            var controller = new UserPreferenceController(repository.Object);

            var result = await controller.FindAsync("test") as OkObjectResult;

            Assert.Equal(SubredditConnections, result.Value);
        }

        [Fact(DisplayName = "Find returns NoContent")]
        public async Task Find_returns_NoContent()
        {
            var SubredditConnections = new UserPreference[0];

            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.FindAsync("test")).ReturnsAsync(SubredditConnections);

            var controller = new UserPreferenceController(repository.Object);

            var result = await controller.FindAsync("test");

            Assert.IsType<NoContentResult>(result) ;
        }

        [Fact(DisplayName = "Find given non-existing username returns NotFound")]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.FindAsync("test")).ReturnsAsync(default(UserPreference[]));

            var controller = new UserPreferenceController(repository.Object);

            var result = await controller.FindAsync("test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Post given invalid userPreference returns BadRequest")]
        public async Task Post_given_invalid_track_returns_BadRequest()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var userPreference = new UserPreference();
            var result = await controller.PostAsync(userPreference);

            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact(DisplayName = "Post given AlreadyThereException returns conflict")]
        public async Task Post_given_AlreadyThereException_returns_Conflict()
        {
            var userPreference = new UserPreference();
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.CreateAsync(userPreference)).Throws(new AlreadyThereException(""));
            var controller = new UserPreferenceController(repository.Object);

            
            var result = await controller.PostAsync(userPreference);
 

            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact(DisplayName = "Post given invalid userPreference does not call CreateAsync")]
        public async Task Post_given_invalid_track_does_not_call_CreateAsync()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var userPreference = new UserPreference();
            await controller.PostAsync(userPreference);

            repository.Verify(r => r.CreateAsync(It.IsAny<UserPreference>()), Times.Never);
        }

        [Fact(DisplayName = "Post given valid userPreference calls CreateAsync")]
        public async Task Post_given_valid_track_calls_CreateAsync()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference();
            await controller.PostAsync(userPreference);

            repository.Verify(r => r.CreateAsync(userPreference));
        }

        [Fact(DisplayName = "Post given valid userPreference returns CreatedAtAction")]
        public async Task Post_given_valid_track_returns_CreatedAtAction()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.CreateAsync(It.IsAny<UserPreference>())).ReturnsAsync(("test","test"));
            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference();
            var result = await controller.PostAsync(userPreference) as CreatedAtActionResult;
            var strid = nameof(UserPreferenceController.FindAsync);
            Assert.Equal(nameof(UserPreferenceController.FindAsync), result.ActionName);
            Assert.Equal(("test","test"), result.RouteValues["usernameAndSub"]);
        }



        [Fact(DisplayName = "Put given invalid userPreference does not call UpdateAsync")]
        public async Task Put_given_invalid_track_does_not_call_UpdateAsync()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var userPreference = new UserPreference();
            await controller.PutAsync( userPreference);

            repository.Verify(r => r.UpdateAsync(It.IsAny<UserPreference>()), Times.Never);
        }

        [Fact(DisplayName = "Put given valid userPreference calls UpdateAsync")]
        public async Task Put_given_valid_track_calls_UpdateAsync()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference { Username = "test" };
            await controller.PutAsync( userPreference);

            repository.Verify(r => r.UpdateAsync(userPreference));
        }

        [Fact(DisplayName = "Put given non-existing userPreference returns NotFound")]
        public async Task Put_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.UpdateAsync(It.IsAny<UserPreference>())).ReturnsAsync(false);

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference { Username = "test" };
            var result = await controller.PutAsync(userPreference);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Put given valid userPreference returns NoContent")]
        public async Task Put_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.UpdateAsync(It.IsAny<UserPreference>())).ReturnsAsync(true);

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference { Username = "test" };
            var result = await controller.PutAsync(userPreference);

            Assert.IsType<OkResult>(result);
        }

        [Fact(DisplayName = "Delete given non-existing userPreference returns NotFound")]
        public async Task Delete_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.DeleteAsync("test","test")).ReturnsAsync(false);

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference();
            var result = await controller.DeleteAsync("test","test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Delete given valid userPreference returns NoContent")]
        public async Task Delete_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.DeleteAsync("test","test")).ReturnsAsync(true);

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference();
            var result = await controller.DeleteAsync("test","test");

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given username calls DeleteAsync")]
        public async Task Delete_given_id_calls_DeleteAsync()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);

            await controller.DeleteAsync("test","test");

            repository.Verify(r => r.DeleteAsync("test","test"));
        }
    }
}
