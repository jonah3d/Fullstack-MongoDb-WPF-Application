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
    public partial class MainPage : Page
    {

        public ObservableCollection<Product> NewProducts { get; set; }
        private IStoreFront storeFront;
        public MainPage()
        {
            InitializeComponent();
            storeFront = new StoreFrontRepository.StoreFrontRepository();


            Loaded += async (s, e) =>
            {
                await LoadNewProducts();
            };

            this.DataContext = this;
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

        public async Task LoadNewProducts()
        {
            try
            {
                LoadingProgressBar.Visibility = Visibility.Visible;
                LoadingProgressBar.IsIndeterminate = true;
                var newShoes = await storeFront.GetNewProducts();
                if (newShoes != null)
                {
                    NewProducts = new ObservableCollection<Product>(newShoes);
                    LB_NewProducts.ItemsSource = NewProducts;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading new products: {ex.Message}");
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
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
            if (sender is Button btn && btn.Tag is string categoryName)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFramePage.Navigate(new ProductsFilterPage(categoryName));
            }
        }

        private void Btn_women_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string categoryName)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFramePage.Navigate(new ProductsFilterPage(categoryName));
            }
        }

        private void Btn_children_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string categoryName)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFramePage.Navigate(new ProductsFilterPage(categoryName));
            }
        }

        private void Btn_sports_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string categoryName)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFramePage.Navigate(new ProductsFilterPage(categoryName));
            }
        }

        private void LB_NewProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LB_NewProducts.SelectedItem is Product selectedProduct)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFramePage.Navigate(new ProductDetailsPage(selectedProduct));
            }
        }
    }
}