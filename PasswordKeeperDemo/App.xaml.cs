using PasswordKeeperDemo.Helpers;
using PasswordKeeperDemo.Views;
using Prism.DryIoc;
using SQLite;
using Xamarin.Forms;

namespace PasswordKeeperDemo
{
    public partial class App
    {
        public App()
        {
        }

        /**/
        public const string DbName = "PasswordKeeperDemo.db3";

        public static string FullPathDb = DependencyService.Get<IFileHelper>().GetLocalFilePath(DbName);

        private static SQLiteAsyncConnection _connection;

        public static SQLiteAsyncConnection GetCurrentConnection()
        {
            return _connection ?? (_connection = new SQLiteAsyncConnection(FullPathDb));
        }

        /**/
        static PasswordNoteDatabase _passwordNoteDatabase;
        public static PasswordNoteDatabase PasswordNoteDatabase => _passwordNoteDatabase ?? (_passwordNoteDatabase = new PasswordNoteDatabase());

        protected override async void OnInitialized()
        {
            InitializeComponent();

            if (GeneralSetting.Current.IsRequirePassword)
            {
                await NavigationService.NavigateAsync("/Login");
                return;
            }

            await NavigationService.NavigateAsync("/Navigation/Main");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>("Navigation");

            Container.RegisterTypeForNavigation<MainPage>("Main");
            Container.RegisterTypeForNavigation<LoginPage>("Login");

            Container.RegisterTypeForNavigation<EnablePasswordPage>("EnablePassword");
            Container.RegisterTypeForNavigation<DisablePasswordPage>("DisablePassword");
            Container.RegisterTypeForNavigation<ChangePasswordPage>("ChangePassword");

            Container.RegisterTypeForNavigation<CategoryPage>("Category");
            Container.RegisterTypeForNavigation<LoginTemplatePage>("LoginTemplate");
            Container.RegisterTypeForNavigation<PasswordTemplatePage>("PasswordTemplate");
            Container.RegisterTypeForNavigation<SecureTemplatePage>("SecureTemplate");
            Container.RegisterTypeForNavigation<LicenseTemplatePage>("LicenseTemplate");
            Container.RegisterTypeForNavigation<SocialTemplatePage>("SocialTemplate");
        }
    }
}
