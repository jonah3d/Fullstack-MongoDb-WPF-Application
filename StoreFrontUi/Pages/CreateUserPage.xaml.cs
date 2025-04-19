using Microsoft.Win32;
using StoreFrontModel;
using StoreFrontRepository;
using System;
using System.Collections.Generic;
using System.IO;
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

    public partial class CreateUserPage : Page
    {
        private byte[] selectedImageBytes;
        private IStoreFront storeFront;
        private MainWindow parentWindow;
        public CreateUserPage()
        {
            InitializeComponent();

            parentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            Img_profileImage.Cursor = Cursors.Hand;
            Img_profileImage.MouseLeftButtonDown += Img_profileImage_MouseLeftButtonDown;
            storeFront = new StoreFrontRepository.StoreFrontRepository();
        }

        private void Img_profileImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Profile Image",
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.gif)|*.png;*.jpeg;*.jpg;*.gif|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                  
                    selectedImageBytes = File.ReadAllBytes(openFileDialog.FileName);

                    
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze(); 

                   
                    Img_profileImage.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error selecting image: {ex.Message}", "Image Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btn_registerUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs())
                {
                   
                    User newUser = new User
                    {
                        FirstName = Tb_Name.Text,
                        LastName = Tb_LastName.Text,
                        Email = Tb_Email.Text,
                        Password = Tb_password.Password,
                        Username = Tb_username.Text,
                        Phone = Tb_phone.Text,
                       
                        ProfileImage = selectedImageBytes,
              
                        Addresses = new List<Address>
                    {
                        new Address
                        {
                            Street = Tb_street.Text,
                            City = Tb_city.Text,
                            Provincia = Tb_province.Text,
                            PostalCode = Tb_postalcode.Text,
                             Country = Tb_country.Text
                        }
                    }
                    };

                    if (storeFront.CreateUser(newUser))
                    {
                        MessageBox.Show("User created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

               
                        NavigateToMainPage();
                    }
                    else
                    {
                        MessageBox.Show("Failed to create user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
              
                System.Diagnostics.Debug.WriteLine($"Full Exception: {ex}");
            }
        }
        private void NavigateToMainPage()
        {
        
            if (parentWindow != null)
            {
                parentWindow.NavigateToMainPage();
            }
            else
            {
            
                MessageBox.Show("Unable to navigate to main page.", "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private bool ValidateInputs()
        {
 
            if (string.IsNullOrWhiteSpace(Tb_Name.Text))
            {
                MessageBox.Show("Please enter a first name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Tb_LastName.Text))
            {
                MessageBox.Show("Please enter a last name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Tb_Email.Text))
            {
                MessageBox.Show("Please enter an email address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Tb_password.Password != Tb_confpassword.Password)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

           if(string.IsNullOrEmpty(Tb_password.Password) || string.IsNullOrEmpty(Tb_confpassword.Password))
            {
                MessageBox.Show("Please enter a password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

           if(string.IsNullOrEmpty(Tb_Email.Text))
            {
                MessageBox.Show("Please enter an email address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

           if(string.IsNullOrWhiteSpace(Tb_username.Text))
            {
                MessageBox.Show("Please enter a username.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

           if(Tb_username.Text.Length < 5)
            {
                MessageBox.Show("Username must be at least 5 characters long.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Tb_phone.Text))
            {
                MessageBox.Show("Please enter a phone number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if(Tb_phone.Text.Length < 9)
            {
                MessageBox.Show("Phone number must be at least 10 characters long.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (selectedImageBytes == null)
            {
                MessageBox.Show("Please select a profile image.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if(string.IsNullOrWhiteSpace(Tb_street.Text))
            {
                MessageBox.Show("Please enter a street address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if(string.IsNullOrEmpty(Tb_city.Text))
            {
                MessageBox.Show("Please enter a city.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(Tb_province.Text))
            {
                MessageBox.Show("Please enter a province.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if(string.IsNullOrEmpty(Tb_postalcode.Text))
            {
                MessageBox.Show("Please enter a postal code.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(Tb_country.Text))
            {
                MessageBox.Show("Please enter a country.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
