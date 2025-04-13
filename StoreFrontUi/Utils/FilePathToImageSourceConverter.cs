using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace StoreFrontUi.Utils
{
    public class FilePathToImageSourceConverter : IValueConverter
    {
        private static readonly BitmapImage DefaultImage = new BitmapImage(
            new Uri("pack://application:,,,/Assets/test_adidas_.png"));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as string;

            if (string.IsNullOrWhiteSpace(path))
                return DefaultImage;

            path = path.Trim();

            if (!File.Exists(path))
                return DefaultImage;

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(path, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
            catch
            {
                return DefaultImage;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}