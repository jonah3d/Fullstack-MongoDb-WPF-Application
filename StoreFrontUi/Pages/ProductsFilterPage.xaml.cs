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
using StoreFrontRepository;

namespace StoreFrontUi.Pages
{

    public partial class ProductsFilterPage : Page
    {
        private IStoreFront storeFront;
        private MainWindow parentWindow;
        private string selectedCategory;


        public ProductsFilterPage(string category)
        {
            InitializeComponent();

            selectedCategory = category;
            parentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            storeFront = new StoreFrontRepository.StoreFrontRepository();

        
        }


    }
}

