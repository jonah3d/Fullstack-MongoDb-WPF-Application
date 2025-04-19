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

namespace StoreFrontUi.Pages
{

    public partial class CartPage : Page
    {
        private IStoreFront storeFront;
        private MainWindow parentWindow;
      

        public CartPage()
        {
            InitializeComponent();
            storeFront = new StoreFrontRepository.StoreFrontRepository();
            parentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();


            Lv_CartItems.ItemsSource = parentWindow.StoreCart.Items;
            this.DataContext = this;

        }

      
    }
}
