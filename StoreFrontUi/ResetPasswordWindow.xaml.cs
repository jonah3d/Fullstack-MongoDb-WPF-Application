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
using System.Windows.Shapes;
using StoreFrontRepository;

namespace StoreFrontUi
{

    public partial class ResetPasswordWindow : Window
    {
        private IStoreFront storeFront;

        public ResetPasswordWindow()
        {
            InitializeComponent();
            storeFront = new StoreFrontRepository.StoreFrontRepository();
        }

        private async void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string username = Txt_Username.Text.Trim();
            string email = Txt_Email.Text.Trim();
            string newPassword = Txt_NewPassword.Password;
            string confirmPassword = Txt_ConfirmPassword.Password;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                bool success = await storeFront.ResetPassword(email, username, newPassword);
                if (success)
                {
                    MessageBox.Show("Password has been reset.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No user found with provided username and email.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
