using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using StoreFrontModel;

namespace StoreFrontUi.UserControls
{

    public partial class UC_ProoductInGrid : UserControl
    {
        public UC_ProoductInGrid()
        {
            InitializeComponent();
            this.Loaded += UC_ProoductInGrid_Loaded;
        }

        private void UC_ProoductInGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var product = this.DataContext as StoreFrontModel.Product;
                if (product != null &&
                    product.Variants != null &&
                    product.Variants.Count > 0 &&
                    product.Variants[0].Photos != null &&
                    product.Variants[0].Photos.Count > 0)
                {
                    string imagePath = product.Variants[0].Photos[0].Url;

                    if (File.Exists(imagePath))
                    {
                        // Create BitmapImage
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad; // Load the image right away
                        bitmap.UriSource = new Uri(imagePath);
                        bitmap.EndInit();

                        // Set image source
                        Img_newitem.Source = bitmap;
                    }
                    else
                    {
                        // Fallback to default image
                        Img_newitem.Source = new BitmapImage(new Uri("/Assets/test_adidas_.png", UriKind.Relative));
                    }
                }
            }
            catch
            {
                // Fallback to default image on any error
                Img_newitem.Source = new BitmapImage(new Uri("/Assets/test_adidas_.png", UriKind.Relative));
            }
        }

        public Product GridProducts
        {
            get { return (Product)GetValue(GridProductsProperty); }
            set { SetValue(GridProductsProperty, value); }
        }

        public static readonly DependencyProperty GridProductsProperty =
            DependencyProperty.Register("GridProducts", typeof(Product), typeof(UC_ProoductInGrid),
            new PropertyMetadata(null, OnGridProductsChanged));

        private static void OnGridProductsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UC_ProoductInGrid control && e.NewValue is Product product)
            {
                control.DataContext = product;
            }
        }
    }
}
