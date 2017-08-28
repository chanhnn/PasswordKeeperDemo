using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasswordKeeperDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
        }
    }
}