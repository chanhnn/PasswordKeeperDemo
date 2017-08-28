using System;
using System.Windows.Input;
using PasswordKeeperDemo.Helpers;
using Prism.Commands;
using Prism.Navigation;

namespace PasswordKeeperDemo.ViewModels
{
    public class EnablePasswordPageViewModel : BaseViewModel
    {
        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public ICommand DoneCommand { get; }

        public EnablePasswordPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            try
            {
                Title = "Enable Password";
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
                if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
                {
                    await DialogService.DisplayAlertAsync("Enable Password", "Bạn chưa nhập mật khẩu", "OK");
                    return;
                }
                else
                {
                    if (!Password.Equals(ConfirmPassword))
                    {
                        await DialogService.DisplayAlertAsync("Enable Password", "Mật khẩu nhập không chính xác", "OK");
                        return;
                    }
                }

                //
                GeneralSetting.Current.IsRequirePassword = true;

                //
                var encryptPassword = Crypto.EncryptHmacSha1(Password);
                GeneralSetting.Current.Password = encryptPassword;

                await NavigationService.NavigateAsync("/Navigation/Main");
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }
    }
}
