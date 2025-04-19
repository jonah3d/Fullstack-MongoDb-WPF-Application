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
            get { return (CartItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); 
            
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register("Item", typeof(CartItem), typeof(UC_CartItem), new PropertyMetadata(null));


    }
}
