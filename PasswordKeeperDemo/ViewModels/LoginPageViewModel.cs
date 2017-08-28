using System;
using System.Windows.Input;
using PasswordKeeperDemo.Helpers;
using Prism.Commands;
using Prism.Navigation;

namespace PasswordKeeperDemo.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }

        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            try
            {
                Title = "Login";
                LoginCommand = new DelegateCommand(Login);
            }
            catch (Exception e)
            {
                DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        private async void Login()
        {
            try
            {
                if (string.IsNullOrEmpty(Password))
                {
                    await DialogService.DisplayAlertAsync("Login", "Bạn chưa nhập mật khẩu", "OK");
                    return;
                }
                else
                {
                    var encryptPassword = Crypto.EncryptHmacSha1(Password);
                    if (!encryptPassword.Equals(GeneralSetting.Current.Password))
                    {
                        await DialogService.DisplayAlertAsync("Login", "Mật khẩu không đúng", "OK");
                        return;
                    }
                }
                await NavigationService.NavigateAsync("/Navigation/Main");
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }
    }
}
