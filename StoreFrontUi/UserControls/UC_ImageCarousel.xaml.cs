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

namespace StoreFrontUi.UserControls
{
  
    public partial class UC_ImageCarousel : UserControl, INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

  
        public static readonly DependencyProperty ProductProperty =
            DependencyProperty.Register("Product", typeof(Product), typeof(UC_ImageCarousel),
                new PropertyMetadata(null, OnProductChanged));

        public Product Product
        {
            get { return (Product)GetValue(ProductProperty); }
            set { SetValue(ProductProperty, value); }
        }


        private ObservableCollection<ImageSource> _images = new ObservableCollection<ImageSource>();
        public ObservableCollection<ImageSource> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged(nameof(Images));
                CurrentIndex = 0;
                UpdateIndicators();
            }
        }

     
        private int _currentIndex = 0;
        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                if (value < 0)
                    _currentIndex = Images.Count - 1;
                else if (value >= Images.Count)
                    _currentIndex = 0;
                else
                    _currentIndex = value;

                OnPropertyChanged(nameof(CurrentIndex));
                OnPropertyChanged(nameof(CurrentImage));
                UpdateIndicators();
            }
        }

       
        public ImageSource CurrentImage
        {
            get
            {
                if (Images.Count > 0 && CurrentIndex >= 0 && CurrentIndex < Images.Count)
                    return Images[CurrentIndex];
                return null;
            }
        }

  
        private ObservableCollection<IndicatorModel> _indicators = new ObservableCollection<IndicatorModel>();
        public ObservableCollection<IndicatorModel> Indicators
        {
            get { return _indicators; }
            set
            {
                _indicators = value;
                OnPropertyChanged(nameof(Indicators));
            }
        }

        public UC_ImageCarousel()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private static void OnProductChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UC_ImageCarousel carousel && e.NewValue is Product product)
            {
                carousel.LoadImagesFromProduct(product);
            }
        }

        private void LoadImagesFromProduct(Product product)
        {
            Images.Clear();

 
            if (product != null && product.ImagePaths != null)
            {
                foreach (string imagePath in product.ImagePaths)
                {
                    try
                    {
                        var image = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                        Images.Add(image);
                    }
                    catch (Exception ex)
                    {
                      
                        Console.WriteLine($"Error loading image: {ex.Message}");
                    }
                }
            }

     
            if (Images.Count == 0)
            {
                var defaultImage = new BitmapImage(new Uri("pack://application:,,,/Assets/placeholder_image.jpg", UriKind.Absolute));
                Images.Add(defaultImage);
            }

            CurrentIndex = 0;
            UpdateIndicators();
        }

        private void UpdateIndicators()
        {
            Indicators.Clear();
            for (int i = 0; i < Images.Count; i++)
            {
                Indicators.Add(new IndicatorModel
                {
                    Index = i,
                    IsActive = i == CurrentIndex
                });
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentIndex--;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentIndex++;
        }

        private void IndicatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int index)
            {
                CurrentIndex = index;
            }
        }
    }


    public class IndicatorModel : INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<string> ImagePaths { get; set; } = new List<string>();
    }
}
