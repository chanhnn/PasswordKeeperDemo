using System;
using DryIoc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace PasswordKeeperDemo.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware
    {
        public string Title { get; set; }

        protected IPageDialogService DialogService { get; }

        protected INavigationService NavigationService { get; }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    RaisePropertyChanged(nameof(IsNotBusy));
                }
            }
        }

        public bool IsNotBusy => !IsBusy;

        protected BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            DialogService = ((App)Application.Current).Container.Resolve<IPageDialogService>();
        }

        public virtual async void OnNavigatedFrom(NavigationParameters parameters)
        {
            try
            {
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        public virtual async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        public virtual async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }
    }
}
