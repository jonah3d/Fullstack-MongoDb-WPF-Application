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

namespace StoreFrontUi.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string categoryName)
            {
            
                if (btn.Content is Grid grid &&
                    grid.Children.OfType<Image>().FirstOrDefault() is Image img)
                {
                    string colorImagePath = $"pack://application:,,,/Assets/{categoryName}.jpg";
                    img.Source = new BitmapImage(new Uri(colorImagePath));
                }
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string categoryName)
            {
               
                if (btn.Content is Grid grid &&
                    grid.Children.OfType<Image>().FirstOrDefault() is Image img)
                {
                    string bwImagePath = $"pack://application:,,,/Assets/bw_{categoryName}.jpg";
                    img.Source = new BitmapImage(new Uri(bwImagePath));
                }
            }
        }

        private void Btn_men_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string men_cat)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFramePage.Navigate(new ProductsFilterPage(men_cat));
            }
        }

        private void Btn_women_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void Btn_children_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Btn_sports_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}