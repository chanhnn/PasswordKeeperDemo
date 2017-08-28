using System;
using System.Globalization;
using Xamarin.Forms;

namespace PasswordKeeperDemo.Helpers
{
    public class IsFavouriteToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var isFavouriteValue = (bool)value;

                return isFavouriteValue ? "#EAFAEA" : "#eee";
            }
            catch (Exception e)
            {
                return "#eee";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
