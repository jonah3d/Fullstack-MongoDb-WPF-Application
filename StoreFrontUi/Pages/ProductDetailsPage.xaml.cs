using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StoreFrontModel;

namespace StoreFrontUi.Pages
{
    /// <summary>
    /// Interaction logic for ProductDetailsPage.xaml
    /// </summary>
    public partial class ProductDetailsPage : Page
    {
        public ObservableCollection<ProductVariant> Variant { get; set; }
        public ProductDetailsPage(Product product)
        {
            InitializeComponent();
            StoreProduct = product;
            Variant = new ObservableCollection<ProductVariant>(product.Variants);

            this.DataContext = this;
        }



        public Product StoreProduct
        {
            get { return (Product)GetValue(StoreProductProperty); }
            set { SetValue(StoreProductProperty, value); }
        }

      
        public static readonly DependencyProperty StoreProductProperty =
            DependencyProperty.Register("StoreProduct", typeof(Product), typeof(ProductDetailsPage), new PropertyMetadata(null));

        private void LB_ProductVariants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Tb_color.Text = ((ProductVariant)LB_ProductVariants.SelectedItem).Color;
           // Tb_Price.Text = ((ProductVariant)LB_ProductVariants.SelectedItem).Price.ToString();
        }
    }
}
