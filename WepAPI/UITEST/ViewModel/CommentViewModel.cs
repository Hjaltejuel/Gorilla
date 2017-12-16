﻿using UITEST.Model;
using UITEST.Model.GorillaRestInterfaces;
using UITEST.Model.RedditRestInterfaces;

namespace UITEST.ViewModel
{
    public class CommentViewModel : BaseViewModel
    {
        IRedditApiConsumer _redditApiConsumer;
        IRestUserPreferenceRepository _restUserPreferenceRepository;
        IRestPostRepository _repository;
        public CommentViewModel(INavigationService service, IRestPostRepository repository, IRestUserPreferenceRepository restUserPreferenceRepository, IRedditApiConsumer redditApiConsumer) : base(service)
        {
            _repository = repository;
            _restUserPreferenceRepository = restUserPreferenceRepository;
            _redditApiConsumer = redditApiConsumer;
        }
    }
}
