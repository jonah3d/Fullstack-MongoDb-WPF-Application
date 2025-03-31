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

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Image img && img.Tag is string imageName)
            {
                string colorImagePath = $"pack://application:,,,/Assets/{imageName}.jpg";
                img.Source = new BitmapImage(new Uri(colorImagePath));
            }
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Image img && img.Tag is string imageName)
            {
                string bwImagePath = $"pack://application:,,,/Assets/bw_{imageName}.jpg";
                img.Source = new BitmapImage(new Uri(bwImagePath));
            }
        }
    }
}