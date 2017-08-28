using System;
using PasswordKeeperDemo.Helpers;
using PasswordKeeperDemo.Models;
using Prism.Commands;
using Prism.Navigation;

namespace PasswordKeeperDemo.ViewModels
{
    public class SecureTemplatePageViewModel : BaseViewModel
    {
        private PasswordNote _passwordNote;

        public PasswordNote PasswordNote
        {
            get => _passwordNote;
            set => SetProperty(ref _passwordNote, value);
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        public SecureTemplatePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            try
            {
                Title = "Secure Note";
                SaveCommand = new DelegateCommand(Save);
                DeleteCommand = new DelegateCommand(Delete);
            }
            catch (Exception e)
            {
                DialogService.DisplayAlertAsync("Error", e.Message, "OK");
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
                        PasswordNote.ModifyDateTime = DateTime.Now;

                        await App.PasswordNoteDatabase.UpdateItemAsync(PasswordNote);
                        await NavigationService.NavigateAsync("/Navigation/Main");
                    }
                    else
                    {
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
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }
    }
}
