using System;
using System.Windows.Input;
using PasswordKeeperDemo.Helpers;
using PasswordKeeperDemo.Models;
using Prism.Commands;
using Prism.Navigation;

namespace PasswordKeeperDemo.ViewModels
{
    public class LoginTemplatePageViewModel : BaseViewModel
    {
        private PasswordNote _passwordNote;

        public PasswordNote PasswordNote
        {
            get => _passwordNote;
            set => SetProperty(ref _passwordNote, value);
        }

        public DelegateCommand ViewPasswordCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        public LoginTemplatePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            try
            {
                Title = "Login";
                ViewPasswordCommand = new DelegateCommand(ViewPassword);
                SaveCommand = new DelegateCommand(Save);
                DeleteCommand = new DelegateCommand(Delete);
            }
            catch (Exception e)
            {
                DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        private async void ViewPassword()
        {
            try
            {
                if (PasswordNote != null)
                {
                    await DialogService.DisplayAlertAsync("", PasswordNote.Password, "OK");
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        private async void Save()
        {
            try
            {
                if (PasswordNote != null)
                {
                    if (PasswordNote.ID != 0)
                    {
                        var pwd = Crypto.EncryptAesCbcPkcs7(PasswordNote.Password, PasswordNote.IvKey);

                        PasswordNote.Password = pwd;

                        PasswordNote.ModifyDateTime = DateTime.Now;

                        await App.PasswordNoteDatabase.UpdateItemAsync(PasswordNote);
                        await NavigationService.NavigateAsync("/Navigation/Main");
                    }
                    else
                    {
                        string ivKey = string.Empty;
                        var pwd = Crypto.EncryptAesCbcPkcs7(PasswordNote.Password, ref ivKey);

                        PasswordNote.Password = pwd;
                        PasswordNote.IvKey = ivKey;

                        PasswordNote.CreateDateTime = DateTime.Now;

                        await App.PasswordNoteDatabase.InsertItemAsync(PasswordNote);
                        await NavigationService.NavigateAsync("/Navigation/Main");
                    }
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        private async void Delete()
        {
            try
            {
                if (PasswordNote != null && PasswordNote.ID != 0)
                {
                    await App.PasswordNoteDatabase.DeleteItemAsync(PasswordNote);
                    await NavigationService.NavigateAsync("/Navigation/Main");
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                var n = parameters["item"];
                if (n != null && (n is PasswordNote))
                {
                    PasswordNote = n as PasswordNote;

                    if (PasswordNote.ID != 0)
                    {
                        var pwd = PasswordNote.Password;
                        var ivKey = PasswordNote.IvKey;

                        var dPwd = Crypto.DecryptAesCbcPkcs7(pwd, ivKey);
                        PasswordNote.Password = dPwd;
                    }
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }
    }
}
