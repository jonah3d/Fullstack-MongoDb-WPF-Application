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

    public partial class ProductsFilterPage : Page
    {
        private IStoreFront storeFront;
        private MainWindow parentWindow;
        private string selectedCategory;
        public ObservableCollection<Product> Shoes { get; set; }

        public ProductsFilterPage(string category)
        {
            InitializeComponent();
            selectedCategory = category;
            parentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            storeFront = new StoreFrontRepository.StoreFrontRepository();
            this.DataContext = this;

       
            Loaded += async (s, e) => {
                await NavigatedFilter();
            };
        }

        public async Task NavigatedFilter()
        {

            switch (selectedCategory)
            {
                case "men_cat":
                    ckb_men.IsChecked = true;
                    await LoadMenShoes();
                    break;
                case "women_cat":
                    ckb_women.IsChecked = true;
                    await LoadWomenShoes();
                    break;
                case "children_cat":
                    ckb_children.IsChecked = true;
                    await LoadChildrenShoes();
                    break;
                case "sports_cat":
                    ckb_sport.IsChecked = true;
                    await LoadSportsShoes();
                    break;

            }
        }

        public async Task LoadMenShoes()
        {
            try
            {
                LoadingProgressBar.Visibility = Visibility.Visible;
                LoadingProgressBar.IsIndeterminate = true;

                var menShoes = await storeFront.GetAllMenProduct();

                Shoes = new ObservableCollection<Product>(menShoes);
                ProductsList.ItemsSource = Shoes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading shoes: {ex.Message}");
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        public async Task LoadWomenShoes()
        {
            try
            {
                LoadingProgressBar.Visibility = Visibility.Visible;
                LoadingProgressBar.IsIndeterminate = true;
                var womenShoes = await storeFront.GetAllWomenProduct();
                Shoes = new ObservableCollection<Product>(womenShoes);
                ProductsList.ItemsSource = Shoes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading shoes: {ex.Message}");
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }


        public async Task LoadSportsShoes()
        {
            try
            {
                LoadingProgressBar.Visibility = Visibility.Visible;
                LoadingProgressBar.IsIndeterminate = true;
                var sportsShoes = await storeFront.GetAllSportsProduct();
                Shoes = new ObservableCollection<Product>(sportsShoes);
                ProductsList.ItemsSource = Shoes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading shoes: {ex.Message}");
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }


        public async Task LoadChildrenShoes()
        {
            try
            {
                LoadingProgressBar.Visibility = Visibility.Visible;
                LoadingProgressBar.IsIndeterminate = true;
                var childrenShoes = await storeFront.GetAllChildrenProduct();
                Shoes = new ObservableCollection<Product>(childrenShoes);
                ProductsList.ItemsSource = Shoes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading shoes: {ex.Message}");
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ProductsList.SelectedItem is Product selectedProduct)
            {
                var productDetailsPage = new ProductDetailsPage(selectedProduct);
                parentWindow.MainFramePage.Navigate(productDetailsPage);
            }
        }
    }
}

