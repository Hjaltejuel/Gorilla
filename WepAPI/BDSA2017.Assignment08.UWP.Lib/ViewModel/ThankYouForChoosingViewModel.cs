
using BDSA2017.Assignment08.UWP.Model.GorillaRestInterfaces;
using Entities.GorillaEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Lib.Model;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.ViewModel;

namespace BDSA2017.Assignment08.UWP.ViewModel
{
    public class ThankYouForChoosingViewModel : BaseViewModel
    {
        private readonly IRestCategoryRepository _repository;
        private readonly IRestUserRepository _restUserRepository;
        private readonly IUserHandler _userHandler;
        public delegate void ChoosingReady();
        public event ChoosingReady ChoosingReadyEvent;
        public ThankYouForChoosingViewModel(INavigationService service, IRestCategoryRepository repository, IRestUserRepository restUserRepository, IUserHandler userHandler) : base(service)
        {
            _userHandler = userHandler;
            _restUserRepository = restUserRepository;
            _repository = repository;
        }

        public async Task load(string[] names)
        {
            await _repository.UpdateAsync(new CategoryObject { _names = names, _username = _userHandler.GetUserName()});
            await _restUserRepository.UpdateAsync(new User { Username = _userHandler.GetUserName(), StartUpQuestionAnswered = 1 });
            ChoosingReadyEvent.Invoke();
            Service.Navigate(MainPage, null);
        }
    }
}
