using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Wpf.Ui.Controls;

namespace StoreFrontUi.Pages
{

    public partial class PaymentPage : Page
    {
        public Cart Cart { get; set; }
        public User User { get; set; }
        public PaymentPage(Cart cart, User user)
        {
            InitializeComponent();
            Cart = cart;
            User = user;
            this.DataContext = this;
        }

        private void txtCardNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string number = txtCardNumber.Text.Replace(" ", "");

            if (number.StartsWith("4"))
                imgCardType.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/visa.png"));
            else if (number.StartsWith("5"))
                imgCardType.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/mastercard.png"));
            else
                imgCardType.Source = null;
        }
      
        private void PayNow_Click(object sender, RoutedEventArgs e)
        {

            if (!ValidateCardDetails())
                return;


            if (!ValidateCardNumber(txtCardNumber.Text))
            {
                System.Windows.MessageBox.Show("Invalid card number.");
                return;
            }

        
       
        }

        private bool ValidateCardNumber(string number)
        {
            number = number.Replace(" ", "");
            int sum = 0;
            bool alternate = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(number[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }

            return (sum % 10 == 0);
        }

        private bool ValidateCardDetails()
        {
           
            string expDate = txtExpDate.Text.Trim();
            if (!Regex.IsMatch(expDate, @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                System.Windows.MessageBox.Show("Expiration date must be in MM/YY format.", "Invalid Input", System.Windows.MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

          
            var parts = expDate.Split('/');
            int month = int.Parse(parts[0]);
            int year = 2000 + int.Parse(parts[1]); 
            var exp = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            if (DateTime.Now > exp)
            {
                System.Windows.MessageBox.Show("Card is expired.", "Invalid Input", System.Windows.MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            string cvv = txtCVV.Text.Trim();
            if (!Regex.IsMatch(cvv, @"^\d{3,4}$"))
            {
                System.Windows.MessageBox.Show("CVV must be 3 or 4 digits.", "Invalid Input", System.Windows.MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtCardName.Text.Trim()))
            {
                System.Windows.MessageBox.Show("You Must Enter A Card Holder");
                return false;
            }

            if(string.IsNullOrEmpty(txtCardNumber.Text.Trim()))
            {
                System.Windows.MessageBox.Show("You Must Enter A Card Number");
                return false;
            }

            return true;
        }

    }
}
