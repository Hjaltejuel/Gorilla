using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UI.Lib.Model;
using UI.Lib.ViewModel;

namespace BDSA2017.Assignment08.UWP.ViewModel
{
    public class ChooseYourCategoriesViewModel : BaseViewModel
    {
        HashSet<string> names = new HashSet<string>(); 
        public bool? _Science;
        public bool? Science { get => _Science; set { _Science = value; if (_Science == true) { names.Add(nameof(Science)); } else { names.Remove(nameof(Science)); } } }
        public bool? _Cars;
        public bool? Cars { get => _Cars; set { _Cars = value; if (_Cars == true) { names.Add(nameof(Cars)); } else { names.Remove(nameof(Cars)); } } }
        public bool? _Sport;
        public bool? Sport { get => _Sport; set { _Sport = value; if (_Sport == true) { names.Add(nameof(Sport)); } else { names.Remove(nameof(Sport)); } } }
        public bool? _Games;
        public bool? Games { get => _Games; set { _Games = value; if (_Games == true) { names.Add(nameof(Games)); } else { names.Remove(nameof(Games)); } } }
        public bool? _News;
        public bool? News { get => _News; set { _News = value; if (_News == true) { names.Add(nameof(News)); } else { names.Remove(nameof(News)); } } }
        public bool? _Travel;
        public bool? Travel { get => _Travel; set { _Travel = value; if (_Travel == true) { names.Add(nameof(Travel)); } else { names.Remove(nameof(Travel)); } } }
        public bool? _Photography;
        public bool? Photography { get => _Photography; set { _Photography = value; if (_Photography == true) { names.Add(nameof(Photography)); } else { names.Remove(nameof(Photography)); } } }
        public bool? _Food;
        public bool? Food { get => _Food; set { _Food = value; if (_Food == true) { names.Add(nameof(Food)); } else { names.Remove(nameof(Food)); } } }
        public bool? _Technology;
        public bool? Technology { get => _Technology; set { _Technology = value; if (_Technology == true) { names.Add(nameof(Technology)); } else { names.Remove(nameof(Technology)); } } }


        public ChooseYourCategoriesViewModel(INavigationService service) : base(service)
        {
           
        }
        public void add(string name)
        {
            names.Add(name);
        }
        public void remove(string name)
        {
            names.Remove(name);
        }
        public void GoToLoading()
        {
            
            Service.Navigate(ThankYouForChoosing, names.ToArray());
        }

    }
}
