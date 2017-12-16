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
    public class SubredditControllerTests
    {
        [Fact(DisplayName = "Read returns Ok with subreddits")]
        public async Task Read_returns_Ok_with_subreddits()
        {
            
            var subreddits = new[] { new Subreddit { SubredditName = "Hello" } };

            var repository = new Mock<ISubredditRepository>();
            repository.Setup(r => r.ReadAsync()).ReturnsAsync(subreddits);

            var controller = new SubredditController(repository.Object);

            if (await controller.ReadAsync() is OkObjectResult result) Assert.Equal(subreddits, result.Value);
        }

        [Fact(DisplayName = "Read returns NoContent with list of zero size")]
        public async Task Read_returns_NoContent_with_list_of_zero_size()
        {

            var subreddits = new Subreddit[0];

            var repository = new Mock<ISubredditRepository>();
            repository.Setup(r => r.ReadAsync()).ReturnsAsync(subreddits);

            var controller = new SubredditController(repository.Object);

            var result = await controller.ReadAsync();

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Get given existing SubredditName returns Ok with Subreddit")]
        public async Task Get_given_existing_id_returns_Ok_with_track()
        {
            var subreddit = new Subreddit();

            var repository = new Mock<ISubredditRepository>();
            repository.Setup(r => r.FindAsync("test")).ReturnsAsync(subreddit);

            var controller = new SubredditController(repository.Object);

            if (await controller.GetAsync("test") is OkObjectResult result) Assert.Equal(subreddit, result.Value);
        }

        [Fact(DisplayName = "Get given non-existing SubredditName returns NotFound")]
        public async Task Get_given_non_existing_id_returns_NotFound()
        {
            var repository = new Mock<ISubredditRepository>();
            repository.Setup(r => r.FindAsync("test")).ReturnsAsync(default(Subreddit));

            var controller = new SubredditController(repository.Object);

            var result = await controller.GetAsync("test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Post given invalid Subreddit returns BadRequest")]
        public async Task Post_given_invalid_track_returns_BadRequest()
        {
            var repository = new Mock<ISubredditRepository>();

            var controller = new SubredditController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var subreddit = new Subreddit();
            var result = await controller.PostAsync(subreddit);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact(DisplayName = "Post given invalid Subreddit does not call CreateAsync")]
        public async Task Post_given_invalid_track_does_not_call_CreateAsync()
        {
            var repository = new Mock<ISubredditRepository>();

            var controller = new SubredditController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var subreddit = new Subreddit();
            await controller.PostAsync(subreddit);

            repository.Verify(r => r.CreateAsync(It.IsAny<Subreddit>()), Times.Never);
        }

        [Fact(DisplayName = "Post given valid Subreddit calls CreateAsync")]
        public async Task Post_given_valid_track_calls_CreateAsync()
        {
            var repository = new Mock<ISubredditRepository>();

            var controller = new SubredditController(repository.Object);

            var subreddit = new Subreddit();
            await controller.PostAsync(subreddit);

            repository.Verify(r => r.CreateAsync(subreddit));
        }

        [Fact(DisplayName = "Post given valid Subreddit returns CreatedAtAction")]
        public async Task Post_given_valid_track_returns_CreatedAtAction()
        {
            var repository = new Mock<ISubredditRepository>();
            repository.Setup(r => r.CreateAsync(It.IsAny<Subreddit>())).ReturnsAsync("test");
            var controller = new SubredditController(repository.Object);

            var subreddit = new Subreddit();
            var result = await controller.PostAsync(subreddit) as CreatedAtActionResult;

            Assert.Equal(nameof(SubredditController.GetAsync), result.ActionName);
            Assert.Equal("test", result.RouteValues["id"]);
        }

        [Fact(DisplayName = "Delete given non-existing Subreddit returns NotFound")]
        public async Task Delete_given_non_existing_track_returns_NotFound()
        {
            var repository = new Mock<ISubredditRepository>();
            repository.Setup(r => r.DeleteAsync("test")).ReturnsAsync(false);

            var controller = new SubredditController(repository.Object);
            
            var result = await controller.DeleteAsync("test");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Delete given valid Subreddit returns NoContent")]
        public async Task Delete_given_valid_track_returns_NoContent()
        {
            var repository = new Mock<ISubredditRepository>();
            repository.Setup(r => r.DeleteAsync("test")).ReturnsAsync(true);

            var controller = new SubredditController(repository.Object);
            
            var result = await controller.DeleteAsync("test");

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "Delete given SubredditName calls DeleteAsync")]
        public async Task Delete_given_id_calls_DeleteAsync()
        {
            var repository = new Mock<ISubredditRepository>();

            var controller = new SubredditController(repository.Object);

            await controller.DeleteAsync("test");

            repository.Verify(r => r.DeleteAsync("test"));
        }

        [Fact(DisplayName = "Throws AlreadyThereException when trying to create duplicate subreddit")]
        public async Task Throw_AlreadyThereException_When_Creating_Duplicate_Subreddit()
        {
            var repository = new Mock<ISubredditRepository>();

            var subreddit = new Subreddit();
            repository.Setup(
                    r => r.CreateAsync(subreddit)).Throws(new AlreadyThereException("")
                    );
            var controller = new SubredditController(repository.Object);
            var result = await controller.PostAsync(subreddit);
            Assert.IsType<StatusCodeResult>(result);
        }
    }
}

