using Entities.Exceptions;
using Entities.GorillaAPI.Interfaces;
using Entities.GorillaEntities;
using Gorilla.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Gorilla.Test
{
    public class PostControllerTest
    {
        [Fact(DisplayName = "Read returns Ok with posts")]
        public async Task Read_returns_Ok_with_posts()
        {

            var posts = new[] { new Post { Id = "test", username = "Hjalte" } };

            var repository = new Mock<IPostRepository>();
            repository.Setup(r => r.ReadAsync("Hjalte")).ReturnsAsync(posts);

            var controller = new PostController(repository.Object);

            if (await controller.ReadAsync("Hjalte") is OkObjectResult result) Assert.Equal(posts, result.Value);
        }

        [Fact(DisplayName = "Read returns NoContent with list of zero size")]
        public async Task Read_returns_NoContent_with_list_of_zero_size()
        {

            var posts = new Post[0];

            var repository = new Mock<IPostRepository>();
            repository.Setup(r => r.ReadAsync("Hjalte")).ReturnsAsync(posts);

            var controller = new PostController(repository.Object);

            var result = await controller.ReadAsync("Hjalte");

            Assert.IsType<NoContentResult>(result);
        }
        [Fact(DisplayName = "Post given invalid Post returns BadRequest")]
        public async Task Post_given_invalid_track_returns_BadRequest()
        {
            var repository = new Mock<IPostRepository>();

            var controller = new PostController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var post = new Post();
            var result = await controller.PostAsync(post);

            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact(DisplayName = "Post given AlreadyThereException returns conflict")]
        public async Task Post_given_AlreadyThereException_returns_Conflict()
        {
            var post = new Post();
            var repository = new Mock<IPostRepository>();
            repository.Setup(r => r.CreateAsync(post)).Throws(new AlreadyThereException(""));
            var controller = new PostController(repository.Object);


            var result = await controller.PostAsync(post);


            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact(DisplayName = "Post given invalid Post does not call CreateAsync")]
        public async Task Post_given_invalid_track_does_not_call_CreateAsync()
        {
            var repository = new Mock<IPostRepository>();

            var controller = new PostController(repository.Object);
            controller.ModelState.AddModelError(string.Empty, "Error");

            var post = new Post();
            await controller.PostAsync(post);

            repository.Verify(r => r.CreateAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact(DisplayName = "Post given valid Post calls CreateAsync")]
        public async Task Post_given_valid_track_calls_CreateAsync()
        {
            var repository = new Mock<IPostRepository>();

            var controller = new PostController(repository.Object);

            var post = new Post();
            await controller.PostAsync(post);

            repository.Verify(r => r.CreateAsync(post));
        }

        [Fact(DisplayName = "Post given valid Post returns CreatedAtAction")]
        public async Task Post_given_valid_track_returns_CreatedAtAction()
        {
            var repository = new Mock<IPostRepository>();
            repository.Setup(r => r.CreateAsync(It.IsAny<Post>())).ReturnsAsync(("test"));
            var controller = new PostController(repository.Object);

            var post = new Post();

            if (await controller.PostAsync(post) is CreatedAtActionResult result)
            {
                Assert.Equal(nameof(PostController.ReadAsync), result.ActionName);
                Assert.Equal(("test"), result.RouteValues["id"]);
            }
        }



    }
}
