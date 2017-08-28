using System;
using System.Windows.Input;
using PasswordKeeperDemo.Helpers;
using Prism.Commands;
using Prism.Navigation;

namespace PasswordKeeperDemo.ViewModels
{
    public class ChangePasswordPageViewModel : BaseViewModel
    {
        private string _currentPassword;
        public string CurrentPassword
        {
            get => _currentPassword;
            set => SetProperty(ref _currentPassword, value);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        private string _confirmNewPassword;
        public string ConfirmNewPassword
        {
            get => _confirmNewPassword;
            set => SetProperty(ref _confirmNewPassword, value);
        }

        public ICommand DoneCommand { get; }

        public ChangePasswordPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            try
            {
                Title = "Change Password";
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
                var currentPassword = GeneralSetting.Current.Password;
                if (string.IsNullOrEmpty(CurrentPassword) ||
                    !Crypto.EncryptHmacSha1(CurrentPassword).Equals(currentPassword))
                {
                    await DialogService.DisplayAlertAsync("Change Password", "Mật khẩu hiện tại không đúng", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmNewPassword))
                {
                    await DialogService.DisplayAlertAsync("Change Password", "Bạn chưa nhập mật khẩu mới", "OK");
                    return;
                }
                else
                {
                    if (!NewPassword.Equals(ConfirmNewPassword))
                    {
                        await DialogService.DisplayAlertAsync("Change Password", "Mật khẩu mới nhập không chính xác", "OK");
                        return;
                    }
                }

                var encryptPassword = Crypto.EncryptHmacSha1(NewPassword);
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
