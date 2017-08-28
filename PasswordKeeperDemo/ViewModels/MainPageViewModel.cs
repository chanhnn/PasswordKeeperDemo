using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordKeeperDemo.Helpers;
using PasswordKeeperDemo.Models;
using PasswordKeeperDemo.Services;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace PasswordKeeperDemo.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        #region Main 

        #endregion

        #region List

        public ICommand AddCommand { get; set; }

        public string ListTitle { get; set; }

        private ObservableCollection<PasswordNote> _passwordNotes;
        public ObservableCollection<PasswordNote> PasswordNotes
        {
            get => _passwordNotes;
            set => SetProperty(ref _passwordNotes, value);
        }

        private PasswordNote _selectedItem;
        public PasswordNote SelectedItem
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

        #endregion

        #region Favourite

        public string FavouriteTitle { get; set; }

        private ObservableCollection<PasswordNote> _passwordNoteFavourites;
        public ObservableCollection<PasswordNote> PasswordNoteFavourites
        {
            get => _passwordNoteFavourites;
            set => SetProperty(ref _passwordNoteFavourites, value);
        }

        private PasswordNote _selectedFavouriteItem;
        public PasswordNote SelectedFavouriteNoteItem
        {
            get => _selectedFavouriteItem;
            set
            {
                _selectedFavouriteItem = value;

                if (_selectedFavouriteItem != null)
                {
                    FavouriteItemSelected(_selectedFavouriteItem);

                    _selectedFavouriteItem = null;

                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Settings

        //
        public string SettingTitle { get; set; }

        //
        public GeneralSetting GeneralSetting => GeneralSetting.Current;

        //
        private bool _isRequirePassword;

        public bool IsRequirePassword
        {
            get
            {
                _isRequirePassword = GeneralSetting.IsRequirePassword;
                return _isRequirePassword;
            }
            set
            {
                if (_isRequirePassword == value)
                    return;

                if (value)
                {
                    // TODO
                    NavigationService.NavigateAsync("EnablePassword");
                    return;
                }
                else
                {
                    // TODO
                    NavigationService.NavigateAsync("DisablePassword");
                    return;
                }

                //if (SetProperty(ref _isRequirePassword, value))
                //{
                //    GeneralSetting.IsRequirePassword = _isRequirePassword;
                //    RaisePropertyChanged(nameof(HasRequirePassword));
                //}
            }
        }

        public bool HasRequirePassword => _isRequirePassword;

        public ICommand ChangePasswordCommand { get; set; }

        private readonly DropBoxService _dropBoxService = new DropBoxService();

        public ICommand DropboxCommand { get; set; }

        private string _dropboxDisplayName;

        public string DropboxDisplayName
        {
            get => _dropboxDisplayName;
            set => SetProperty(ref _dropboxDisplayName, value);
        }

        private string _dropboxEmail;

        public string DropboxEmail
        {
            get => _dropboxEmail;
            set => SetProperty(ref _dropboxEmail, value);
        }

        //
        private bool _isVerifyDropbox;

        public bool IsVerifyDropbox
        {
            get => _isVerifyDropbox;
            set => SetProperty(ref _isVerifyDropbox, value);
        }

        public ICommand BackupCommand { get; set; }
        public ICommand RestoreCommand { get; set; }

        #endregion

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            try
            {
                #region Main

                Title = "Password Keeper";

                #endregion

                #region Categories

                ListTitle = "Categories";
                AddCommand = new DelegateCommand(Add);

                #endregion

                #region Favourites

                FavouriteTitle = "Favourites";

                #endregion

                #region Settings

                SettingTitle = "Settings";
                ChangePasswordCommand = new DelegateCommand(ChangePassword);

                DropboxCommand = new DelegateCommand(Dropbox);

                BackupCommand = new DelegateCommand(Backup);
                RestoreCommand = new DelegateCommand(Restore);

                #endregion
            }
            catch (Exception e)
            {
                DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                PasswordNotes = new ObservableCollection<PasswordNote>(await App.PasswordNoteDatabase.GetItemsAsync());
                PasswordNoteFavourites = new ObservableCollection<PasswordNote>(await App.PasswordNoteDatabase.GetFavouriteItemsAsync());

                await OnDropboxLoadInfo();
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        #region Events - List

        private async void Add()
        {
            try
            {
                await NavigationService.NavigateAsync("Category");
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        private async void ItemSelected(object obj)
        {
            try
            {
                var pn = obj as PasswordNote;
                if (pn != null)
                {
                    await NavigationService.NavigateAsync(pn.Page, new NavigationParameters() { { "item", pn } });
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        #endregion

        #region Favourites

        private async void FavouriteItemSelected(object obj)
        {
            try
            {
                var pn = obj as PasswordNote;
                if (pn != null)
                {
                    await NavigationService.NavigateAsync(pn.Page, new NavigationParameters() { { "item", pn } });
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        #endregion

        #region Events - Settings

        private async void ChangePassword()
        {
            try
            {
                if (GeneralSetting.IsRequirePassword)
                {
                    await NavigationService.NavigateAsync("ChangePassword");
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        private async void Dropbox()
        {
            try
            {
                // load info
                await this._dropBoxService.Authorize();
                this._dropBoxService.OnAuthenticated += this.LoadDropboxInfo;
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        private async Task OnDropboxLoadInfo()
        {
            try
            {
                IsVerifyDropbox = false;

                // load info
                this._dropBoxService.OnAuthenticated += this.LoadDropboxInfo;
                await this._dropBoxService.VerifyAuthorize();
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        private async void LoadDropboxInfo()
        {
            try
            {
                var di = await _dropBoxService.GetDropboxInfo();
                DropboxDisplayName = di.DisplayName;
                DropboxEmail = di.Email;

                IsVerifyDropbox = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void Backup()
        {
            try
            {
                if (IsVerifyDropbox)
                {
                    IsBusy = true;

                    // Read the database from app storage
                    var dbContent = DependencyService.Get<IFileUtils>().GetFileContent(App.FullPathDb);
                    if (dbContent != null)
                    {
                        // Write the database to DropBox folder
                        var metadata = await this._dropBoxService.WriteFile(dbContent, App.FullPathDb);
                        await DialogService.DisplayAlertAsync("Backup", "Backup thành công", "OK");
                    }
                    else
                    {
                        // TODO
                        await DialogService.DisplayAlertAsync("Backup", "Không lấy được dữ liệu backup", "OK");
                    }
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void Restore()
        {
            try
            {
                if (IsVerifyDropbox)
                {
                    IsBusy = true;

                    // Read the database from DropBox folder
                    var db = await this._dropBoxService.ReadFile(App.FullPathDb);

                    if (db == null)
                    {
                        await DialogService.DisplayAlertAsync("Restore", "Không lấy được dữ liệu từ Dropbox", "OK");
                    }
                    else
                    {
                        // Write the database to storage
                        DependencyService.Get<IFileUtils>().SaveFileContent(App.FullPathDb, db);

                        await DialogService.DisplayAlertAsync("Restore", "Restore thành công", "OK");
                        await NavigationService.NavigateAsync("/Navigation/Main");
                    }
                }
            }
            catch (Exception e)
            {
                await DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
