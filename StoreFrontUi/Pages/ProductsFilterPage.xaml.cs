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
using StoreFrontDb;
using StoreFrontModel;
using StoreFrontRepository;

namespace StoreFrontUi.Pages
{
    public partial class ProductsFilterPage : Page
    {
        private IStoreFront storeFront;
        private MainWindow parentWindow;
        private string selectedCategory;
        private string currentSearchText = ""; 
        public ObservableCollection<Product> Shoes { get; set; }
        private List<Product> allShoes;

        public ProductsFilterPage(string category)
        {
            InitializeComponent();
            selectedCategory = category;

            storeFront = new StoreFrontRepository.StoreFrontRepository();
            parentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();



            if (parentWindow != null)
            {
         
                parentWindow.GlobalSearchChanged += async (searchText) =>
                {
                  
                    currentSearchText = searchText;
                    ApplyFilters();
                   // return Task.CompletedTask; 
                };
            }
            this.DataContext = this;

         
            AttachFilterEvents();

            Loaded += async (s, e) =>
            {
                await NavigatedFilter();
            };
        }

        private void AttachFilterEvents()
        {
          
            ckb_men.Checked += (s, e) => ApplyFilters();
            ckb_men.Unchecked += (s, e) => ApplyFilters();
            ckb_women.Checked += (s, e) => ApplyFilters();
            ckb_women.Unchecked += (s, e) => ApplyFilters();
            ckb_children.Checked += (s, e) => ApplyFilters();
            ckb_children.Unchecked += (s, e) => ApplyFilters();

         
            ckb_sport.Checked += (s, e) => ApplyFilters();
            ckb_sport.Unchecked += (s, e) => ApplyFilters();
            ckb_work.Checked += (s, e) => ApplyFilters();
            ckb_work.Unchecked += (s, e) => ApplyFilters();

            PriceRangeSlider.LowerValueChanged += (s, e) => ApplyFilters();
            PriceRangeSlider.HigherValueChanged += (s, e) => ApplyFilters();

          
            ckb_35.Checked += (s, e) => ApplyFilters();
            ckb_35.Unchecked += (s, e) => ApplyFilters();
            ckb_36.Checked += (s, e) => ApplyFilters();
            ckb_36.Unchecked += (s, e) => ApplyFilters();
            ckb_37.Checked += (s, e) => ApplyFilters();
            ckb_37.Unchecked += (s, e) => ApplyFilters();
            ckb_38.Checked += (s, e) => ApplyFilters();
            ckb_38.Unchecked += (s, e) => ApplyFilters();
            ckb_39.Checked += (s, e) => ApplyFilters();
            ckb_39.Unchecked += (s, e) => ApplyFilters();
            ckb_40.Checked += (s, e) => ApplyFilters();
            ckb_40.Unchecked += (s, e) => ApplyFilters();
            ckb_41.Checked += (s, e) => ApplyFilters();
            ckb_41.Unchecked += (s, e) => ApplyFilters();
            ckb_42.Checked += (s, e) => ApplyFilters();
            ckb_42.Unchecked += (s, e) => ApplyFilters();
            ckb_43.Checked += (s, e) => ApplyFilters();
            ckb_43.Unchecked += (s, e) => ApplyFilters();
            ckb_44.Checked += (s, e) => ApplyFilters();
            ckb_44.Unchecked += (s, e) => ApplyFilters();
            ckb_45.Checked += (s, e) => ApplyFilters();
            ckb_45.Unchecked += (s, e) => ApplyFilters();
            ckb_46.Checked += (s, e) => ApplyFilters();
            ckb_46.Unchecked += (s, e) => ApplyFilters();
            ckb_47.Checked += (s, e) => ApplyFilters();
            ckb_47.Unchecked += (s, e) => ApplyFilters();
            ckb_48.Checked += (s, e) => ApplyFilters();
            ckb_48.Unchecked += (s, e) => ApplyFilters();
            ckb_49.Checked += (s, e) => ApplyFilters();
            ckb_49.Unchecked += (s, e) => ApplyFilters();
            ckb_50.Checked += (s, e) => ApplyFilters();
            ckb_50.Unchecked += (s, e) => ApplyFilters();
        }

        public async Task NavigatedFilter()
        {
            try
            {
                LoadingProgressBar.Visibility = Visibility.Visible;

          
                allShoes = await storeFront.GetAllProducts();

     
                switch (selectedCategory)
                {
                    case "men_cat":
                        ckb_men.IsChecked = true;
                        break;
                    case "women_cat":
                        ckb_women.IsChecked = true;
                        break;
                    case "children_cat":
                        ckb_children.IsChecked = true;
                        break;
                    case "sports_cat":
                        ckb_sport.IsChecked = true;
                        break;
                }

              
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}");
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void ApplyFilters()
        {
            if (allShoes == null || allShoes.Count == 0)
                return;

            try
            {
                LoadingProgressBar.Visibility = Visibility.Visible;

                IEnumerable<Product> filteredShoes = allShoes;

                if (!string.IsNullOrWhiteSpace(currentSearchText))
                {
                    filteredShoes = filteredShoes.Where(p =>
                        p.Name.IndexOf(currentSearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (p.Description != null && p.Description.IndexOf(currentSearchText, StringComparison.OrdinalIgnoreCase) >= 0));
                }

                var genderFilters = new List<Func<Product, bool>>();
                if (ckb_men.IsChecked == true)
                    genderFilters.Add(p => IsMenProduct(p));
                if (ckb_women.IsChecked == true)
                    genderFilters.Add(p => IsWomenProduct(p));
                if (ckb_children.IsChecked == true)
                    genderFilters.Add(p => IsChildrenProduct(p));

                if (genderFilters.Any())
                {
                    filteredShoes = filteredShoes.Where(p => genderFilters.Any(filter => filter(p)));
                }

             
                var activityFilters = new List<Func<Product, bool>>();
                if (ckb_sport.IsChecked == true)
                    activityFilters.Add(p => IsSportProduct(p));
                if (ckb_work.IsChecked == true)
                    activityFilters.Add(p => IsWorkProduct(p));

                if (activityFilters.Any())
                {
                    filteredShoes = filteredShoes.Where(p => activityFilters.Any(filter => filter(p)));
                }

               
                decimal minPrice = (decimal)PriceRangeSlider.LowerValue;
                decimal maxPrice = (decimal)PriceRangeSlider.HigherValue;
                filteredShoes = filteredShoes.Where(p => p.Price >= minPrice && p.Price <= maxPrice);

                var selectedSizes = GetSelectedSizes();
                if (selectedSizes.Any())
                {
                    filteredShoes = filteredShoes.Where(p =>
                        p.Variants.Any(v =>
                            v.Sizes.Any(s => selectedSizes.Contains(s.Size))
                        )
                    );
                }

                Shoes = new ObservableCollection<Product>(filteredShoes);
                ProductsList.ItemsSource = Shoes;

                UpdatePagination(Shoes.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filters: {ex.Message}");
            }
            finally
            {
                LoadingProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private List<string> GetSelectedSizes()
        {
            var sizes = new List<string>();

            if (ckb_35.IsChecked == true) sizes.Add("35");
            if (ckb_36.IsChecked == true) sizes.Add("36");
            if (ckb_37.IsChecked == true) sizes.Add("37");
            if (ckb_38.IsChecked == true) sizes.Add("38");
            if (ckb_39.IsChecked == true) sizes.Add("39");
            if (ckb_40.IsChecked == true) sizes.Add("40");
            if (ckb_41.IsChecked == true) sizes.Add("41");
            if (ckb_42.IsChecked == true) sizes.Add("42");
            if (ckb_43.IsChecked == true) sizes.Add("43");
            if (ckb_44.IsChecked == true) sizes.Add("44");
            if (ckb_45.IsChecked == true) sizes.Add("45");
            if (ckb_46.IsChecked == true) sizes.Add("46");
            if (ckb_47.IsChecked == true) sizes.Add("47");
            if (ckb_48.IsChecked == true) sizes.Add("48");
            if (ckb_49.IsChecked == true) sizes.Add("49");
            if (ckb_50.IsChecked == true) sizes.Add("50");

            return sizes;
        }

        private void UpdatePagination(int totalItems)
        {
      
            int itemsPerPage = 12;
            int maxPage = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            PaginationControl.MaxPage = maxPage > 0 ? maxPage : 1;

      
            PaginationControl.CurrentPage = 1;

          
            ApplyPagination();
        }

        private void ApplyPagination()
        {
            int currentPage = PaginationControl.CurrentPage;
            int itemsPerPage = 12;

            if (Shoes != null)
            {
                var displayedShoes = Shoes.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage);
                ProductsList.ItemsSource = new ObservableCollection<Product>(displayedShoes);
            }
        }


        private bool IsMenProduct(Product product)
        {
    
            return product.Categories?.Any(c => c.Name.Contains("Men") || c.Name.Contains("men")) ?? false;
        }

        private bool IsWomenProduct(Product product)
        {
            return product.Categories?.Any(c => c.Name.Contains("Women") || c.Name.Contains("women")) ?? false;
        }

        private bool IsChildrenProduct(Product product)
        {
            return product.Categories?.Any(c => c.Name.Contains("Children") || c.Name.Contains("children") || c.Name.Contains("Kids") || c.Name.Contains("kids")) ?? false;
        }

        private bool IsSportProduct(Product product)
        {
            return product.Categories?.Any(c => c.Name.Contains("Sport") || c.Name.Contains("sport") || c.Name.Contains("Athletic") || c.Name.Contains("athletic")) ?? false;
        }

        private bool IsWorkProduct(Product product)
        {
            return product.Categories?.Any(c => c.Name.Contains("Work") || c.Name.Contains("work") || c.Name.Contains("Formal") || c.Name.Contains("formal") || c.Name.Contains("Business") || c.Name.Contains("business")) ?? false;
        }

        private void ProductsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsList.SelectedItem is Product selectedProduct)
            {
                var productDetailsPage = new ProductDetailsPage(selectedProduct);
                parentWindow.MainFramePage.Navigate(productDetailsPage);
            }
        }

        private void UC_Pagination_PageChanged(object sender, EventArgs e)
        {
            ApplyPagination();
        }
    }
    }

