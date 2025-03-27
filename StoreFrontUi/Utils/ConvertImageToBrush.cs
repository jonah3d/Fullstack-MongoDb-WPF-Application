using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace StoreFrontUi.Utils
{
    public class ConvertImageToBrush
    {
        public static ImageBrush ConvertToImageBrush(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return new ImageBrush(); // Return a default brush
            }

            try
            {
                BitmapImage bitmap = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                }
                return new ImageBrush(bitmap);
            }
            catch
            {
                return new ImageBrush(); // Return default brush on error
            }
        }

    }
}
