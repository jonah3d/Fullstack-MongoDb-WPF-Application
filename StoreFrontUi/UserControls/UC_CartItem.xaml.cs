using System;
using System.Collections.Generic;
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

namespace StoreFrontUi.UserControls
{
    public partial class UC_CartItem : UserControl
    {
        public UC_CartItem()
        {
            InitializeComponent();
        }

        public CartItem Item
        {
            get => (CartItem)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(CartItem), typeof(UC_CartItem), new PropertyMetadata(null));

        public event EventHandler QuantityChanged;

        private void BtnIncrease_Click(object sender, RoutedEventArgs e)
        {
            if (Item != null)
            {
                Item.Quantity++;
                QuantityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void BtnDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (Item != null && Item.Quantity > 1)
            {
                Item.Quantity--;
                QuantityChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
