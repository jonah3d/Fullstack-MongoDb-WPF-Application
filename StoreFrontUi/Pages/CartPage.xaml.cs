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
using StoreFrontRepository;
using StoreFrontUi.UserControls;

namespace StoreFrontUi.Pages
{

    public partial class CartPage : Page
    {
        private IStoreFront storeFront;
        private MainWindow parentWindow;

        public Cart UserCart { get; set; }

        public CartPage()
        {
            InitializeComponent();
            storeFront = new StoreFrontRepository.StoreFrontRepository();
            parentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            

            Lv_CartItems.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            Lv_CartItems.ItemsSource = parentWindow.StoreCart.Items;
            UserCart = parentWindow.StoreCart;

            // Load shipping methods
            loadShippingMethods();

            this.DataContext = this;
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (Lv_CartItems.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                foreach (var item in Lv_CartItems.Items)
                {
                    var container = Lv_CartItems.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                    if (container != null)
                    {
                        var ucCartItem = FindVisualChild<UC_CartItem>(container);
                        if (ucCartItem != null)
                        {
                            ucCartItem.QuantityChanged -= UcCartItem_QuantityChanged; // avoid duplicates
                            ucCartItem.QuantityChanged += UcCartItem_QuantityChanged;
                        }
                    }
                }
            }
        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T correctlyTyped)
                    return correctlyTyped;

                T descendant = FindVisualChild<T>(child);
                if (descendant != null)
                    return descendant;
            }
            return null;
        }
        private async void UcCartItem_QuantityChanged(object sender, EventArgs e)
        {
            try
            {
                parentWindow.StoreCart.UpdatedAt = DateTime.UtcNow;
                await storeFront.AddOrUpdateCart(parentWindow.StoreCart, parentWindow.CurrentUser.Id);
                // Optionally: refresh checkout totals here
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating cart: {ex.Message}", "Cart Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      async  void loadShippingMethods()
        {
            // Load shipping methods from the repository
            var shippingMethods = await storeFront.GetShippingMethods();
            if (shippingMethods != null)
            {
                cmbshippingcost.DisplayMemberPath = "Name";
                cmbshippingcost.ItemsSource = shippingMethods;
               
            }
        }

        private void cmbshippingcost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbshippingcost.SelectedItem is ShippingMethod selected)
            {
                // Calculate cost including VAT
                decimal vatAmount = selected.BasePrice * (selected.VatPercentage / (decimal)100.00);
                decimal totalShipping = selected.BasePrice + vatAmount;

                // Check for free shipping
                if (UserCart.SubTotal >= selected.MinimumOrderForFreeShipping)
                {
                    totalShipping = 0;
                }

                UserCart.ShippingCost = (decimal)totalShipping;

                // Recalculate other totals
                UserCart.RecalculateTotals();

                // Optional: update to database
                _ = storeFront.AddOrUpdateCart(UserCart, parentWindow.CurrentUser.Id);
            }
        }

    }



}
