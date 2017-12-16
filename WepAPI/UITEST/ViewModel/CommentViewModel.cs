using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gorilla.Model;
using Gorilla.Model.GorillaRestInterfaces;
using Model;
using UITEST.RedditInterfaces;
using UITEST.ViewModel;

namespace Gorilla.ViewModel
{
    public class CommentViewModel : BaseViewModel
    {
        IRedditAPIConsumer _redditAPIConsumer;
        IRestUserPreferenceRepository _restUserPreferenceRepository;
        IRestPostRepository _repository;
        public CommentViewModel(INavigationService service, IRestPostRepository repository, IRestUserPreferenceRepository restUserPreferenceRepository, IRedditAPIConsumer redditApiConsumer) : base(service)
        {
            _repository = repository;
            _restUserPreferenceRepository = restUserPreferenceRepository;
            _redditAPIConsumer = redditApiConsumer;
        }
    }
}
