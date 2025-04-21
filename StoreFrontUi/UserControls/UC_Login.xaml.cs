using StoreFrontModel;
using StoreFrontRepository;
using StoreFrontUi.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoreFrontUi.UserControls
{
    public partial class UC_Login : UserControl
    {
        private IStoreFront storeFront;
        private MainWindow parentWindow;

        public UC_Login()
        {
            InitializeComponent();
            storeFront = new StoreFrontRepository.StoreFrontRepository();
            parentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            SetupPlaceholderHandlers();
        }

        private void SetupPlaceholderHandlers()
        {
            Txt_Username.GotFocus += (s, e) => Placeholder_Username.Visibility = Visibility.Collapsed;
            Txt_Username.LostFocus += (s, e) => Placeholder_Username.Visibility =
                string.IsNullOrEmpty(Txt_Username.Text) ? Visibility.Visible : Visibility.Collapsed;

            Txt_Password.GotFocus += (s, e) => Placeholder_Password.Visibility = Visibility.Collapsed;
            Txt_Password.LostFocus += (s, e) => Placeholder_Password.Visibility =
                string.IsNullOrEmpty(Txt_Password.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string username = Txt_Username.Text;
            string password = Txt_Password.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }
            try
            {
                User loggedUser = storeFront.LoginUser(username, password);

                if (loggedUser != null)
                {
                    MessageBox.Show("Login Successful");
                    parentWindow.CurrentUser = loggedUser;
                    parentWindow.SetUpCurrentUser();
                   await parentWindow.loadusercart();
                    Txt_Password.Password = string.Empty;
                    Txt_Username.Text = string.Empty;

                }
                else
                {
                    MessageBox.Show("Login Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            parentWindow?.NavigateToCreateUserPage();

           
            var popup = this.Parent as Popup;
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetWindow = new ResetPasswordWindow
            {
                Owner = Window.GetWindow(this)
            };
            resetWindow.ShowDialog(); 



        }
    }
}
