using Entities;
using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using WepAPI.Controllers;
using Xunit;

namespace WebApi.Test
{
    public class UserControllerTest
    {
        [Fact(DisplayName = "Controller has AuthorizeAttribute")]
        public void Controller_has_AuthorizeAttribute()
        {
            var type = typeof(UserController);

            var authorizeAttribute = type.CustomAttributes.FirstOrDefault(c => c.AttributeType == typeof(AuthorizeAttribute));

            Assert.NotNull(authorizeAttribute);
        }

        [Fact(DisplayName = "Read returns Ok with users")]
        public async Task Get_returns_Ok_with_tracks()
        {
            var users = new User[1] { new User { Username = "test" } };

            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.Read()).ReturnsAsync(users);

            var controller = new UserController(repository.Object);

            var result = await controller.Read() as OkObjectResult;

            Assert.Equal(users, result.Value);
        }
        [Fact(DisplayName = "Read returns noContent")]
        public async Task Get_returns_NoContent()
        {
            var users = new User[0];

            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.Read()).ReturnsAsync(users);

            var controller = new UserController(repository.Object);

            var result = await controller.Read();

            Assert.IsType<NoContentResult>(result); 
        }

        [Fact(DisplayName = "Get given existing username returns Ok with user")]
        public async Task Get_given_existing_id_returns_Ok_with_track()
        {
            var user = new User();

            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.Find("test")).ReturnsAsync(user);

            var controller = new UserController(repository.Object);

            var result = await controller.Get("test") as OkObjectResult;

            Assert.Equal(user, result.Value);
        }

        [Fact(DisplayName = "Get given non-existing username returns NotFound")]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.Find("test")).ReturnsAsync(default(User));

            var controller = new UserController(repository.Object);

            var result = await controller.Get("test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Post given invalid user returns BadRequest")]
        public async Task Post_given_invalid_track_returns_BadRequest()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var user = new User();
            var result = await controller.Post(user);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact(DisplayName = "Post given AlreadyThereException returns conflict")]
        public async Task Post_given_AlreadyThereException_returns_Conflict()
        {
            var user = new User();
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.Create(user)).Throws(new AlreadyThereException(""));
            var controller = new UserController(repository.Object);


            var result = await controller.Post(user);


            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact(DisplayName = "Post given invalid user does not call CreateAsync")]
        public async Task Post_given_invalid_track_does_not_call_CreateAsync()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var user = new User();
            await controller.Post(user);

            repository.Verify(r => r.Create(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Post given valid user calls CreateAsync")]
        public async Task Post_given_valid_track_calls_CreateAsync()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);

            var user = new User();
            await controller.Post(user);

            repository.Verify(r => r.Create(user));
        }

        [Fact(DisplayName = "Post given valid user returns CreatedAtAction")]
        public async Task Post_given_valid_track_returns_CreatedAtAction()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.Create(It.IsAny<User>())).ReturnsAsync("test");
            var controller = new UserController(repository.Object);

            var user = new User();
            var result = await controller.Post(user) as CreatedAtActionResult;

            Assert.Equal(nameof(UserController.Get), result.ActionName);
            Assert.Equal("test", result.RouteValues["username"]);
        }

        [Fact(DisplayName = "Put given invalid user returns BadRequest")]
        public async Task Put_given_invalid_track_returns_BadRequest()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var user = new User { Username = "test" };
            var result = await controller.Put( user);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        

        [Fact(DisplayName = "Put given invalid user does not call UpdateAsync")]
        public async Task Put_given_invalid_track_does_not_call_UpdateAsync()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var user = new User();
            await controller.Put( user);

            repository.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Put given valid user calls UpdateAsync")]
        public async Task Put_given_valid_track_calls_UpdateAsync()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);

            var user = new User { Username = "test" };
            await controller.Put( user);

            repository.Verify(r => r.Update(user));
        }

        [Fact(DisplayName = "Put given non-existing user returns NotFound")]
        public async Task Put_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.Update(It.IsAny<User>())).ReturnsAsync(false);

            var controller = new UserController(repository.Object);

            var user = new User { Username = "test" };
            var result = await controller.Put( user);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Put given valid user returns NoContent")]
        public async Task Put_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.Update(It.IsAny<User>())).ReturnsAsync(true);

            var controller = new UserController(repository.Object);

            var user = new User { Username = "test" };
            var result = await controller.Put( user);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given non-existing user returns NotFound")]
        public async Task Delete_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.Delete("test")).ReturnsAsync(false);

            var controller = new UserController(repository.Object);

            var user = new User();
            var result = await controller.Delete("test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Delete given valid user returns NoContent")]
        public async Task Delete_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.Delete("test")).ReturnsAsync(true);

            var controller = new UserController(repository.Object);

            var user = new User();
            var result = await controller.Delete("test");

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given username calls DeleteAsync")]
        public async Task Delete_given_id_calls_DeleteAsync()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);

            await controller.Delete("test");

            repository.Verify(r => r.Delete("test"));
        }
    }
}
