using System;
using System.Windows.Input;
using PasswordKeeperDemo.Helpers;
using Prism.Commands;
using Prism.Navigation;

namespace PasswordKeeperDemo.ViewModels
{
    public class DisablePasswordPageViewModel : BaseViewModel
    {
        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand DoneCommand { get; }

        public DisablePasswordPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            try
            {
                Title = "Disable Password";
                DoneCommand = new DelegateCommand(Done);
            }
            catch (Exception e)
            {
                DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        private async void Done()
        {
            try
            {
                if (string.IsNullOrEmpty(Password))
                {
                    await DialogService.DisplayAlertAsync("Disable Password", "Bạn chưa nhập mật khẩu", "OK");
                    return;
                }
                else
                {
                    var encryptPassword = Crypto.EncryptHmacSha1(Password);
                    if (!encryptPassword.Equals(GeneralSetting.Current.Password))
                    {
                        await DialogService.DisplayAlertAsync("Disable Password", "Mật khẩu không đúng", "OK");
                        return;
                    }
                }

                GeneralSetting.Current.IsRequirePassword = false;

                GeneralSetting.Current.Password = string.Empty;

                await NavigationService.NavigateAsync("/Navigation/Main");
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }
    }
}
