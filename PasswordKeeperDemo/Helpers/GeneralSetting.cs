using System.ComponentModel;
using System.Runtime.CompilerServices;
using PasswordKeeperDemo.Annotations;

namespace PasswordKeeperDemo.Helpers
{
    public class GeneralSetting : INotifyPropertyChanged
    {
        private static GeneralSetting _current;
        public static GeneralSetting Current => _current ?? (_current = new GeneralSetting());

        /**/
        public bool IsRequirePassword
        {
            get => Settings.IsRequirePasswordSetting;
            set
            {
                if (Settings.IsRequirePasswordSetting == value)
                    return;

                Settings.IsRequirePasswordSetting = value;

                OnPropertyChanged();
            }
        }

        /**/
        public string Password
        {
            get => Settings.PasswordSetting;
            set
            {
                if (Settings.PasswordSetting == value)
                    return;

                Settings.PasswordSetting = value;

                OnPropertyChanged();
            }
        }

        /**/
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
