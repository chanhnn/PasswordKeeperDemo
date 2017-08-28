using System;
using Prism.Navigation;

namespace PasswordKeeperDemo.ViewModels
{
    public class LicenseTemplatePageViewModel : BaseViewModel
    {
        public LicenseTemplatePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            try
            {
                Title = "Software License";
            }
            catch (Exception e)
            {
                DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }
    }
}
