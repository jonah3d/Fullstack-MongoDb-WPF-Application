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

          
            loadShippingMethods();

            this.DataContext = this;
        }

        public String SelectedShippingMethod { get; set; }

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

                            ucCartItem.RemoveItem -= UcCartItem_RemoveItem;
                            ucCartItem.RemoveItem += UcCartItem_RemoveItem;
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
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating cart: {ex.Message}", "Cart Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UcCartItem_RemoveItem(object sender, CartItem itemToRemove)
        {
            try
            {
              
                UserCart.Items.Remove(itemToRemove);

                
                UserCart.UpdatedAt = DateTime.UtcNow;
                await storeFront.AddOrUpdateCart(UserCart, parentWindow.CurrentUser.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing item from cart: {ex.Message}", "Cart Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        async  void loadShippingMethods()
        {
           
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
            
                decimal vatAmount = selected.BasePrice * (selected.VatPercentage / (decimal)100.00);
                decimal totalShipping = selected.BasePrice + vatAmount;

              //  SelectedShippingMethod = selected.Name;
                UserCart.ShippingMethod = selected.Name;

                if (UserCart.SubTotal >= selected.MinimumOrderForFreeShipping)
                {
                    totalShipping = 0;
                }

                UserCart.ShippingCost = (decimal)totalShipping;

      
                UserCart.RecalculateTotals();

             
                _ = storeFront.AddOrUpdateCart(UserCart, parentWindow.CurrentUser.Id);
            }
        }

        private void btn_checkout_Click(object sender, RoutedEventArgs e)
        {
            if(UserCart.Items.Count == 0)
            {
                MessageBox.Show("Your cart is empty. Please add items to your cart before checking out.", "Empty Cart", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(parentWindow.CurrentUser != null && UserCart != null)
            {
               var paymentPage = new PaymentPage(UserCart,parentWindow.CurrentUser);
                parentWindow.MainFramePage.Navigate(paymentPage);
            }
            else
            {
                MessageBox.Show("Please log in to proceed to checkout.", "Login Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                
            }
        }
    }



}
