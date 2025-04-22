using StoreFrontModel;
using StoreFrontRepository;
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
namespace StoreFrontUi.UserControls
{
    public partial class UC_LoggedInUser : UserControl
    {
        private MainWindow parentWindow;

        public UC_LoggedInUser()
        {
            InitializeComponent();
            parentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        public User LoginUser
        {
            get { return (User)GetValue(LoginUserProperty); }
            set { SetValue(LoginUserProperty, value); }
        }

        public static readonly DependencyProperty LoginUserProperty =
            DependencyProperty.Register("LoginUser", typeof(User), typeof(UC_LoggedInUser), new PropertyMetadata(null, OnLoginUserChanged));

        private static void OnLoginUserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is User user)
            {
                var control = (UC_LoggedInUser)d;
              
            }
        }

       
        private void Btn_SignOut_Click(object sender, RoutedEventArgs e)
        {
            
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.CurrentUser = null;  
            mainWindow.SetUpCurrentUser();
          
        }

        private void Btn_userdownloadinvoices_Click(object sender, RoutedEventArgs e)
        {
            var userInvoicesWindow = new UserInvoicesWindow(LoginUser);
            userInvoicesWindow.Owner = parentWindow;
            userInvoicesWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            userInvoicesWindow.ShowDialog();
        }
    }
}
