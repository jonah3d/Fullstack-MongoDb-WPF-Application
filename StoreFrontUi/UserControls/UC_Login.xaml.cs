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

    public partial class UC_Login : UserControl
    {
        IStoreFront storeFront;

        public UC_Login()
        {
            InitializeComponent();
            storeFront = new StoreFrontRepository.StoreFrontRepository();

            Txt_Username.GotFocus += (s, e) => Placeholder_Username.Visibility = Visibility.Collapsed;
            Txt_Username.LostFocus += (s, e) => Placeholder_Username.Visibility = string.IsNullOrEmpty(Txt_Username.Text) ? Visibility.Visible : Visibility.Collapsed;

            Txt_Password.GotFocus += (s, e) => Placeholder_Password.Visibility = Visibility.Collapsed;
            Txt_Password.LostFocus += (s, e) => Placeholder_Password.Visibility = string.IsNullOrEmpty(Txt_Password.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string username = Txt_Username.Text;
            string password = Txt_Password.Password;
            if(username == "" || password == "")
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }
            bool ans = storeFront.LoginUser(username, password);
            if (ans)
            {
                MessageBox.Show("Login Successful");
            }
            else
            {
                MessageBox.Show("Login Failed");
            }

                
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Redirecting to Sign Up page...");
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Redirecting to Forgot Password page...");
        }
    }
}
