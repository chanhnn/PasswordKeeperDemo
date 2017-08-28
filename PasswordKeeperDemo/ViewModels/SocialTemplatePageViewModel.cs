using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;

namespace PasswordKeeperDemo.ViewModels
{
    public class SocialTemplatePageViewModel : BaseViewModel
    {
        public SocialTemplatePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            try
            {
                Title = "Social Security Number";
            }
            catch (Exception e)
            {
                DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }
    }
}
