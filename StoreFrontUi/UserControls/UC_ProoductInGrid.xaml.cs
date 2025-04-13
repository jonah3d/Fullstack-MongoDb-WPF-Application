using System.Diagnostics;
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
          
        }

 

        public Product GridProducts
        {
            get { return (Product)GetValue(GridProductsProperty); }
            set { SetValue(GridProductsProperty, value); }
        }

        public static readonly DependencyProperty GridProductsProperty =
            DependencyProperty.Register("GridProducts", typeof(Product), typeof(UC_ProoductInGrid),
            new PropertyMetadata(null));

   
        }
    }

