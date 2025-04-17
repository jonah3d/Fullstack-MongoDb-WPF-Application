using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    public partial class UC_ImageCarousel : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty ProductsProperty =
         DependencyProperty.Register("Products", typeof(IEnumerable<Product>), typeof(UC_ImageCarousel),
             new PropertyMetadata(null, OnProductsChanged));




        private static void OnProductsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var carousel = d as UC_ImageCarousel;
            if (carousel != null)
            {
                carousel.LoadImagesFromProducts();
            }
        }

        private void LoadImagesFromProducts()
        {
            try
            {
                _imagePaths.Clear();
                Indicators.Clear();
                _currentIndex = 0;

                if (Products != null)
                {
                    foreach (var product in Products)
                    {
                        if (product.Variants != null)
                        {
                            foreach (var variant in product.Variants)
                            {
                                if (variant.Photos != null && variant.Photos.Count > 0)
                                {
                                    string url = variant.Photos[0].Url;
                                    if (!string.IsNullOrEmpty(url) && !_imagePaths.Contains(url))
                                    {
                                        _imagePaths.Add(url);
                                    }
                                }
                            }
                        }
                    }

                    for (int i = 0; i < _imagePaths.Count; i++)
                    {
                        Indicators.Add(new ImageIndicator { Index = i, IsActive = i == 0 });
                    }

                    if (_imagePaths.Count > 0)
                    {
                        LoadImageAtIndex(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images from products: {ex.Message}");
            }
        }

        public IEnumerable<Product> Products
        {
            get { return (IEnumerable<Product>)GetValue(ProductsProperty); }
            set { SetValue(ProductsProperty, value); }
        }


        // Current image to display
        private BitmapImage _currentImage;
        public BitmapImage CurrentImage
        {
            get { return _currentImage; }
            set
            {
                _currentImage = value;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        // Collection of image indicators
        private ObservableCollection<ImageIndicator> _indicators;
        public ObservableCollection<ImageIndicator> Indicators
        {
            get { return _indicators; }
            set
            {
                _indicators = value;
                OnPropertyChanged(nameof(Indicators));
            }
        }

        // Current image index
        private int _currentIndex = 0;
        private List<string> _imagePaths;

        public UC_ImageCarousel()
        {
            InitializeComponent();
            DataContext = this;
            _indicators = new ObservableCollection<ImageIndicator>();
            _imagePaths = new List<string>();
        }



 


        private void LoadImageAtIndex(int index)
        {
            if (index >= 0 && index < _imagePaths.Count)
            {
                try
                {
                    _currentIndex = index;

                  
                    for (int i = 0; i < Indicators.Count; i++)
                    {
                        Indicators[i].IsActive = i == index;
                    }

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(_imagePaths[index]);
                    bitmap.EndInit();
                    CurrentImage = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}");
                }
            }
        }

 
        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_imagePaths.Count > 0)
            {
                int newIndex = (_currentIndex - 1 + _imagePaths.Count) % _imagePaths.Count;
                LoadImageAtIndex(newIndex);
            }
        }

    
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_imagePaths.Count > 0)
            {
                int newIndex = (_currentIndex + 1) % _imagePaths.Count;
                LoadImageAtIndex(newIndex);
            }
        }

        
        private void IndicatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag is int index)
            {
                LoadImageAtIndex(index);
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

 
    public class ImageIndicator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
