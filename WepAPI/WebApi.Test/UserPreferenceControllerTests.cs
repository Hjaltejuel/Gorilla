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
            repository.Setup(r => r.FindAll("test")).ReturnsAsync(SubredditConnections);

            var controller = new UserPreferenceController(repository.Object);

            var result = await controller.Find("test") as OkObjectResult;

            Assert.Equal(SubredditConnections, result.Value);
        }

        [Fact(DisplayName = "Find returns NoContent")]
        public async Task Find_returns_NoContent()
        {
            var SubredditConnections = new UserPreference[0];

            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.FindAll("test")).ReturnsAsync(SubredditConnections);

            var controller = new UserPreferenceController(repository.Object);

            var result = await controller.Find("test");

            Assert.IsType<NoContentResult>(result) ;
        }

        [Fact(DisplayName = "Find given non-existing username returns NotFound")]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.FindAll("test")).ReturnsAsync(default(UserPreference[]));

            var controller = new UserPreferenceController(repository.Object);

            var result = await controller.Find("test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Post given invalid userPreference returns BadRequest")]
        public async Task Post_given_invalid_track_returns_BadRequest()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var userPreference = new UserPreference();
            var result = await controller.Post(userPreference);

            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact(DisplayName = "Post given AlreadyThereException returns conflict")]
        public async Task Post_given_AlreadyThereException_returns_Conflict()
        {
            var userPreference = new UserPreference();
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.Create(userPreference)).Throws(new AlreadyThereException(""));
            var controller = new UserPreferenceController(repository.Object);

            
            var result = await controller.Post(userPreference);
 

            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact(DisplayName = "Post given invalid userPreference does not call CreateAsync")]
        public async Task Post_given_invalid_track_does_not_call_CreateAsync()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var userPreference = new UserPreference();
            await controller.Post(userPreference);

            repository.Verify(r => r.Create(It.IsAny<UserPreference>()), Times.Never);
        }

        [Fact(DisplayName = "Post given valid userPreference calls CreateAsync")]
        public async Task Post_given_valid_track_calls_CreateAsync()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference();
            await controller.Post(userPreference);

            repository.Verify(r => r.Create(userPreference));
        }

        [Fact(DisplayName = "Post given valid userPreference returns CreatedAtAction")]
        public async Task Post_given_valid_track_returns_CreatedAtAction()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.Create(It.IsAny<UserPreference>())).ReturnsAsync(("test","test"));
            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference();
            var result = await controller.Post(userPreference) as CreatedAtActionResult;

            Assert.Equal(nameof(UserPreferenceController.Find), result.ActionName);
            Assert.Equal(("test","test"), result.RouteValues["usernameAndSub"]);
        }



        [Fact(DisplayName = "Put given invalid userPreference does not call UpdateAsync")]
        public async Task Put_given_invalid_track_does_not_call_UpdateAsync()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var userPreference = new UserPreference();
            await controller.Put( userPreference);

            repository.Verify(r => r.Update(It.IsAny<UserPreference>()), Times.Never);
        }

        [Fact(DisplayName = "Put given valid userPreference calls UpdateAsync")]
        public async Task Put_given_valid_track_calls_UpdateAsync()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference { Username = "test" };
            await controller.Put( userPreference);

            repository.Verify(r => r.Update(userPreference));
        }

        [Fact(DisplayName = "Put given non-existing userPreference returns NotFound")]
        public async Task Put_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.Update(It.IsAny<UserPreference>())).ReturnsAsync(false);

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference { Username = "test" };
            var result = await controller.Put(userPreference);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Put given valid userPreference returns NoContent")]
        public async Task Put_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.Update(It.IsAny<UserPreference>())).ReturnsAsync(true);

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference { Username = "test" };
            var result = await controller.Put(userPreference);

            Assert.IsType<OkResult>(result);
        }

        [Fact(DisplayName = "Delete given non-existing userPreference returns NotFound")]
        public async Task Delete_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.Delete("test","test")).ReturnsAsync(false);

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference();
            var result = await controller.Delete("test","test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Delete given valid userPreference returns NoContent")]
        public async Task Delete_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<IUserPreferenceRepository>();
            repository.Setup(r => r.Delete("test","test")).ReturnsAsync(true);

            var controller = new UserPreferenceController(repository.Object);

            var userPreference = new UserPreference();
            var result = await controller.Delete("test","test");

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given username calls DeleteAsync")]
        public async Task Delete_given_id_calls_DeleteAsync()
        {
            var repository = new Mock<IUserPreferenceRepository>();

            var controller = new UserPreferenceController(repository.Object);

            await controller.Delete("test","test");

            repository.Verify(r => r.Delete("test","test"));
        }
    }
}
