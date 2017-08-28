using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordKeeperDemo.Helpers;
using PasswordKeeperDemo.Models;
using Prism.Navigation;

namespace PasswordKeeperDemo.ViewModels
{
    public class CategoryPageViewModel : BaseViewModel
    {
        public ObservableCollection<Cat> Cats { get; }

        private Cat _selectedItem;
        public Cat SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;

                if (_selectedItem != null)
                {
                    ItemSelected(_selectedItem);

                    _selectedItem = null;

                    RaisePropertyChanged();
                }
            }
        }

        public CategoryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            try
            {
                Title = "Category";
                Cats = new ObservableCollection<Cat>(CategoryData.CreateData());
            }
            catch (Exception e)
            {
                DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        private async void ItemSelected(object obj)
        {
            try
            {
                var cat = obj as Cat;
                if (cat != null)
                {
                    var pn = new PasswordNote
                    {
                        CategoryId = cat.CategoryId,
                        CategoryCode = cat.CategoryCode,
                        CategoryName = cat.CategoryName,
                        Icon = cat.Icon,
                        Page = cat.Page
                    };

                    await NavigationService.NavigateAsync(cat.Page, new NavigationParameters() { { "item", pn } });
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }
    }
}
