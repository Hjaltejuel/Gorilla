using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaAPI.Interfaces;
using Entities.GorillaEntities;
using Gorilla.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Gorilla.Test
{
    public class SubredditConnectionControllerTest
    {
        [Fact(DisplayName = "FindAsync returns Ok with SubredditConnections")]
        public async Task Find_returns_Ok_with_tracks()
        {
            var subredditConnections = new[] { new SubredditConnection { SubredditFromName = "test", SubredditToName = "name" } };

            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.FindAsync("test")).ReturnsAsync(subredditConnections);

            var controller = new SubredditConnectionController(repository.Object);

            if (await controller.FindAsync("test") is OkObjectResult result) Assert.Equal(subredditConnections, result.Value);
        }

        [Fact(DisplayName = "FindAsync returns NoContent")]
        public async Task Find_returns_NoContent()
        {
            var subredditConnections = new SubredditConnection[0];

            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.FindAsync("test")).ReturnsAsync(subredditConnections);

            var controller = new SubredditConnectionController(repository.Object);

            var result = await controller.FindAsync("test");

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "FindAsync given non-existing SubredditFromName returns NotFound")]
        public async Task Find_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.FindAsync("test")).ReturnsAsync(default(SubredditConnection[]));

            var controller = new SubredditConnectionController(repository.Object);

            var result = await controller.FindAsync("test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "GetAsync given existing keys returns subredditConnection")]
        public async Task Get_Given_existing_keys_returns_SubredditConnection()
        {
            var subredditConnection = new SubredditConnection { SubredditFromName = "test", SubredditToName = "test2" };
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.GetAsync("test","test2")).ReturnsAsync(subredditConnection);

            var controller = new SubredditConnectionController(repository.Object);

            if (await controller.GetAsync("test","test2") is OkObjectResult result) Assert.Equal(subredditConnection, result.Value);
        }
        [Fact(DisplayName = "GetAsync given null returns NotFound")]
        public async Task Get_Given_null_returns_NotFound()
        {
        
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.GetAsync("test", "test2")).ReturnsAsync(default(SubredditConnection));

            var controller = new SubredditConnectionController(repository.Object);

            var result = await controller.GetAsync("test", "test2");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Read returns SubredditConnections returns ok")]
        public async Task Read_returns_SubredditConnections_returns_ok()
        {
            var subredditConnection = new[] { new SubredditConnection { SubredditFromName = "test", SubredditToName = "test2" } };
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.ReadAsync()).ReturnsAsync(subredditConnection);

            var controller = new SubredditConnectionController(repository.Object);

            if (await controller.ReadAsync() is OkObjectResult result) Assert.Equal(subredditConnection, result.Value);
        }
        [Fact(DisplayName = "Read returns empty returns NoContent")]
        public async Task Read_returns_empty_returns_NoContent()
        {
            var subredditConnection = new SubredditConnection[0];
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.ReadAsync()).ReturnsAsync(subredditConnection);

            var controller = new SubredditConnectionController(repository.Object);

            var result = await controller.ReadAsync();

            Assert.IsType<NoContentResult>(result);
        }


        [Fact(DisplayName = "Post given invalid SubredditConnection returns BadRequest")]
        public async Task Post_given_invalid_track_returns_BadRequest()
        {
            var repository = new Mock<ISubredditConnectionRepository>();

            var controller = new SubredditConnectionController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var subredditConnection = new SubredditConnection();
            var result = await controller.PostAsync(subredditConnection);

            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact(DisplayName = "Post given AlreadyThereException returns conflict")]
        public async Task Post_given_AlreadyThereException_returns_Conflict()
        {
            var subredditConnection = new SubredditConnection();
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.CreateAsync(subredditConnection)).Throws(new AlreadyThereException(""));
            var controller = new SubredditConnectionController(repository.Object);


            var result = await controller.PostAsync(subredditConnection);


            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact(DisplayName = "Post given invalid SubredditConnection does not call CreateAsync")]
        public async Task Post_given_invalid_track_does_not_call_CreateAsync()
        {
            var repository = new Mock<ISubredditConnectionRepository>();

            var controller = new SubredditConnectionController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var subredditConnection = new SubredditConnection();
            await controller.PostAsync(subredditConnection);

            repository.Verify(r => r.CreateAsync(It.IsAny<SubredditConnection>()), Times.Never);
        }

        [Fact(DisplayName = "Post given valid SubredditConnection calls CreateAsync")]
        public async Task Post_given_valid_track_calls_CreateAsync()
        {
            var repository = new Mock<ISubredditConnectionRepository>();

            var controller = new SubredditConnectionController(repository.Object);

            var subredditConnection = new SubredditConnection();
            await controller.PostAsync(subredditConnection);

            repository.Verify(r => r.CreateAsync(subredditConnection));
        }

        [Fact(DisplayName = "Post given valid SubredditConnection returns CreatedAtAction")]
        public async Task Post_given_valid_track_returns_CreatedAtAction()
        {
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.CreateAsync(It.IsAny<SubredditConnection>())).ReturnsAsync(("test", "test"));
            var controller = new SubredditConnectionController(repository.Object);

            var subredditConnection = new SubredditConnection();

            if (await controller.PostAsync(subredditConnection) is CreatedAtActionResult result)
            {
                Assert.Equal(nameof(SubredditConnectionController.GetAsync), result.ActionName);
                Assert.Equal(("test", "test"), result.RouteValues["result"]);
            }
        }



        [Fact(DisplayName = "Put given invalid SubredditConnection does not call UpdateAsync")]
        public async Task Put_given_invalid_track_does_not_call_UpdateAsync()
        {
            var repository = new Mock<ISubredditConnectionRepository>();

            var controller = new SubredditConnectionController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var subredditConnection = new SubredditConnection();
            await controller.PutAsync(subredditConnection);

            repository.Verify(r => r.UpdateAsync(It.IsAny<SubredditConnection>()), Times.Never);
        }

        [Fact(DisplayName = "Put given valid SubredditConnection calls UpdateAsync")]
        public async Task Put_given_valid_track_calls_UpdateAsync()
        {
            var repository = new Mock<ISubredditConnectionRepository>();

            var controller = new SubredditConnectionController(repository.Object);

            var subredditConnection = new SubredditConnection { SubredditFromName = "test" };
            await controller.PutAsync(subredditConnection);

            repository.Verify(r => r.UpdateAsync(subredditConnection));
        }

        [Fact(DisplayName = "Put given non-existing SubredditConnection returns NotFound")]
        public async Task Put_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.UpdateAsync(It.IsAny<SubredditConnection>())).ReturnsAsync(false);

            var controller = new SubredditConnectionController(repository.Object);

            var subredditConnection = new SubredditConnection { SubredditFromName = "test" };
            var result = await controller.PutAsync(subredditConnection);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Put given valid SubredditConnection returns NoContent")]
        public async Task Put_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.UpdateAsync(It.IsAny<SubredditConnection>())).ReturnsAsync(true);

            var controller = new SubredditConnectionController(repository.Object);

            var subredditConnection = new SubredditConnection { SubredditFromName = "test" };
            var result = await controller.PutAsync(subredditConnection);

            Assert.IsType<OkResult>(result);
        }

        [Fact(DisplayName = "Delete given non-existing SubredditConnection returns NotFound")]
        public async Task Delete_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.DeleteAsync("test", "test")).ReturnsAsync(false);

            var controller = new SubredditConnectionController(repository.Object);
            
            var result = await controller.DeleteAsync("test", "test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Delete given valid SubredditConnection returns NoContent")]
        public async Task Delete_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<ISubredditConnectionRepository>();
            repository.Setup(r => r.DeleteAsync("test", "test")).ReturnsAsync(true);

            var controller = new SubredditConnectionController(repository.Object);
            
            var result = await controller.DeleteAsync("test", "test");

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given SubredditFromName calls DeleteAsync")]
        public async Task Delete_given_id_calls_DeleteAsync()
        {
            var repository = new Mock<ISubredditConnectionRepository>();

            var controller = new SubredditConnectionController(repository.Object);

            await controller.DeleteAsync("test", "test");

            repository.Verify(r => r.DeleteAsync("test", "test"));
        }
}
}
