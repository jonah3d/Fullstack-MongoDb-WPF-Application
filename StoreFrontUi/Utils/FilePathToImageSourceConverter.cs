using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StoreFrontUi.Utils
{
    public class FilePathToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is string filePath) || string.IsNullOrEmpty(filePath))
                return "/Assets/test_adidas_.png"; // Fallback image

            try
            {
                // Check if file exists
                if (File.Exists(filePath))
                {
                    // Create a URI from the file path
                    return new Uri(filePath);
                }
                return "/Assets/test_adidas_.png"; // Fallback if file doesn't exist
            }
            catch
            {
                return "/Assets/test_adidas_.png"; // Fallback for any errors
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
