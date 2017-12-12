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
 

        [Fact(DisplayName = "Read returns Ok with users")]
        public async Task Get_returns_Ok_with_tracks()
        {
            var users = new User[1] { new User { Username = "test" } };

            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.ReadAsync()).ReturnsAsync(users);

            var controller = new UserController(repository.Object);

            var result = await controller.ReadAsync() as OkObjectResult;

            Assert.Equal(users, result.Value);
        }
        [Fact(DisplayName = "Read returns noContent")]
        public async Task Get_returns_NoContent()
        {
            var users = new User[0];

            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.ReadAsync()).ReturnsAsync(users);

            var controller = new UserController(repository.Object);

            var result = await controller.ReadAsync();

            Assert.IsType<NoContentResult>(result); 
        }

        [Fact(DisplayName = "Get given existing username returns Ok with user")]
        public async Task Get_given_existing_id_returns_Ok_with_track()
        {
            var user = new User();

            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.FindAsync("test")).ReturnsAsync(user);

            var controller = new UserController(repository.Object);

            var result = await controller.GetAsync("test") as OkObjectResult;

            Assert.Equal(user, result.Value);
        }

        [Fact(DisplayName = "Get given non-existing username returns NotFound")]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.FindAsync("test")).ReturnsAsync(default(User));

            var controller = new UserController(repository.Object);

            var result = await controller.GetAsync("test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Post given invalid user returns BadRequest")]
        public async Task Post_given_invalid_track_returns_BadRequest()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var user = new User();
            var result = await controller.PostAsync(user);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact(DisplayName = "Post given AlreadyThereException returns conflict")]
        public async Task Post_given_AlreadyThereException_returns_Conflict()
        {
            var user = new User();
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.CreateAsync(user)).Throws(new AlreadyThereException(""));
            var controller = new UserController(repository.Object);


            var result = await controller.PostAsync(user);


            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact(DisplayName = "Post given invalid user does not call CreateAsync")]
        public async Task Post_given_invalid_track_does_not_call_CreateAsync()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var user = new User();
            await controller.PostAsync(user);

            repository.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Post given valid user calls CreateAsync")]
        public async Task Post_given_valid_track_calls_CreateAsync()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);

            var user = new User();
            await controller.PostAsync(user);

            repository.Verify(r => r.CreateAsync(user));
        }

        [Fact(DisplayName = "Post given valid user returns CreatedAtAction")]
        public async Task Post_given_valid_track_returns_CreatedAtAction()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.CreateAsync(It.IsAny<User>())).ReturnsAsync("test");
            var controller = new UserController(repository.Object);

            var user = new User();
            var result = await controller.PostAsync(user) as CreatedAtActionResult;

            Assert.Equal(nameof(UserController.GetAsync), result.ActionName);
            Assert.Equal("test", result.RouteValues["username"]);
        }

        [Fact(DisplayName = "Put given invalid user returns BadRequest")]
        public async Task Put_given_invalid_track_returns_BadRequest()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var user = new User { Username = "test" };
            var result = await controller.PutAsync( user);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        

        [Fact(DisplayName = "Put given invalid user does not call UpdateAsync")]
        public async Task Put_given_invalid_track_does_not_call_UpdateAsync()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var user = new User();
            await controller.PutAsync( user);

            repository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Put given valid user calls UpdateAsync")]
        public async Task Put_given_valid_track_calls_UpdateAsync()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);

            var user = new User { Username = "test" };
            await controller.PutAsync( user);

            repository.Verify(r => r.UpdateAsync(user));
        }

        [Fact(DisplayName = "Put given non-existing user returns NotFound")]
        public async Task Put_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(false);

            var controller = new UserController(repository.Object);

            var user = new User { Username = "test" };
            var result = await controller.PutAsync( user);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Put given valid user returns NoContent")]
        public async Task Put_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);

            var controller = new UserController(repository.Object);

            var user = new User { Username = "test" };
            var result = await controller.PutAsync( user);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given non-existing user returns NotFound")]
        public async Task Delete_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.DeleteAsync("test")).ReturnsAsync(false);

            var controller = new UserController(repository.Object);

            var user = new User();
            var result = await controller.DeleteAsync("test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Delete given valid user returns NoContent")]
        public async Task Delete_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<IUserRepository>();
            repository.Setup(r => r.DeleteAsync("test")).ReturnsAsync(true);

            var controller = new UserController(repository.Object);

            var user = new User();
            var result = await controller.DeleteAsync("test");

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given username calls DeleteAsync")]
        public async Task Delete_given_id_calls_DeleteAsync()
        {
            var repository = new Mock<IUserRepository>();

            var controller = new UserController(repository.Object);

            await controller.DeleteAsync("test");

            repository.Verify(r => r.DeleteAsync("test"));
        }
    }
}
