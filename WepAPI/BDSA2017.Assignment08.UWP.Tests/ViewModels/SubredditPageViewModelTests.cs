using Entities.GorillaEntities;
using Entities.RedditEntities;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Model.RedditRestInterfaces;
using UI.Lib.ViewModel;
using Xunit;

namespace UI.Tests.ViewModels
{
    public class SubredditPageViewModelTests
    {
        private readonly Mock<INavigationService> service;
        private readonly Mock<IAuthenticationHelper> helper;
        private readonly Mock<IRestUserPreferenceRepository> repository;
        private readonly Mock<IRedditApiConsumer> consumer;

        public SubredditPageViewModelTests()
        {
            service = new Mock<INavigationService>();
            helper = new Mock<IAuthenticationHelper>();
            repository = new Mock<IRestUserPreferenceRepository>();
            consumer = new Mock<IRedditApiConsumer>();

        }
        [Fact(DisplayName = "GeneratePost Test given subredditName PUBG sets _Subreddit with display name pubg")]
        public async void GeneratePostsTest()
        {
            //Arrange
            var returnResult = Task.FromResult((HttpStatusCode.OK, new Entities.RedditEntities.Subreddit() {
                    display_name = "Pubg",
                    posts = new ObservableCollection<Entities.RedditEntities.Post>() { new Entities.RedditEntities.Post() { title = "GOTY"} }
            }));
            consumer.Setup(o => o.GetSubredditPostsAsync(It.IsAny<Entities.RedditEntities.Subreddit>(), "hot"))
                                    .Returns(returnResult);
            var vm = new SubredditPageViewModel(helper.Object, service.Object, consumer.Object, repository.Object);

            //Act
            await vm.GeneratePosts("Pubg");

            //Assert
            var expected = "Pubg";
            var actual = vm._Subreddit.display_name;
            Assert.Equal(expected, actual);
            Assert.NotEmpty(vm._Subreddit.posts);
        }

        [Fact(DisplayName = "GeneratePost Test given subredditName PUBG sets _Subreddit is null")]
        public async void GeneratePostsTestWhenSubredditBecomesNull()
        {
            //Arrange
            var returnResult = Task.FromResult((HttpStatusCode.OK, new Entities.RedditEntities.Subreddit()
            {
                display_name = null
            }));
            consumer.Setup(o => o.GetSubredditPostsAsync(It.IsAny<Entities.RedditEntities.Subreddit>(), "hot"))
                                     .Returns(returnResult);

            var vm = new SubredditPageViewModel(helper.Object, service.Object, consumer.Object, repository.Object);

            //Act
            await vm.GeneratePosts("Pubg");

            //Assert
            var actual = vm._Subreddit.display_name;
            Assert.Null(actual);
        }
    }
}
