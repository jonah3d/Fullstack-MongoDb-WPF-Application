using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreFrontUi.Utils
{
    public static class NavigationHelper
    {
        public static void NavigateWithoutChrome(this Frame frame, Page page)
        {
         
            frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            frame.Navigate(page);
        }

  
        public static void GoBackCustom(this Frame frame)
        {
            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }
    }
}
