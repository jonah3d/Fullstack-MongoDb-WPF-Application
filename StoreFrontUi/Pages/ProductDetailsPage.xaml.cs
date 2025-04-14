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

   
        public partial class ProductDetailsPage : Page
        {
            public ObservableCollection<ProductVariant> Variant { get; set; }

            public ProductDetailsPage(Product product)
            {
                InitializeComponent();
                StoreProduct = product;
                Variant = new ObservableCollection<ProductVariant>(product.Variants);


                if (product.Variants != null && product.Variants.Count > 0)
                {
                    SelectedVariant = product.Variants[0];
                }

                this.DataContext = this;
            }

            public Product StoreProduct
            {
                get { return (Product)GetValue(StoreProductProperty); }
                set { SetValue(StoreProductProperty, value); }
            }

            public static readonly DependencyProperty StoreProductProperty =
                DependencyProperty.Register("StoreProduct", typeof(Product), typeof(ProductDetailsPage), new PropertyMetadata(null));

            public ProductVariant SelectedVariant
            {
                get { return (ProductVariant)GetValue(SelectedVariantProperty); }
                set { SetValue(SelectedVariantProperty, value); }
            }

            public static readonly DependencyProperty SelectedVariantProperty =
                DependencyProperty.Register("SelectedVariant", typeof(ProductVariant), typeof(ProductDetailsPage), new PropertyMetadata(null));

            private void LB_ProductVariants_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
   
                if (LB_ProductVariants.SelectedItem is ProductVariant selectedVariant)
                {
                    SelectedVariant = selectedVariant;
                }
            }
        }
    }
