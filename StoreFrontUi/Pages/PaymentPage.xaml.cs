using System;
using System.Collections.Generic;
using System.IO;
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
using QuestPDF.Fluent;
using StoreFrontModel;
using StoreFrontRepository;
using StoreFrontUi.Document;
using StoreFrontUi.Utils;


namespace StoreFrontUi.Pages
{

    public partial class PaymentPage : Page
    {
        public Cart Cart { get; set; }
        public User User { get; set; }
        public IStoreFront StoreFront { get; set; }

        private MainWindow parentWindow;
        public PaymentPage(Cart cart, User user)
        {
            InitializeComponent();
            Cart = cart;
            User = user;

            StoreFront = new StoreFrontRepository.StoreFrontRepository();

            parentWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

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
        private async void PayNow_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateCardDetails())
                return;

            if (!ValidateCardNumber(txtCardNumber.Text))
            {
                System.Windows.MessageBox.Show("Invalid card number.");
                return;
            }

            var result = MessageBox.Show("Do you want to confirm payment?", "Confirm Payment", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                MessageBox.Show("Payment cancelled.", "Cancelled", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            PayNowButton.Visibility = Visibility.Collapsed;
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.Value = 10;

            try
            {
                // STEP 1: Decrease stock for each cart item
                foreach (var item in Cart.Items)
                {
                    bool stockUpdated = await StoreFront.DeleteStock(item.ProductId, item.VariantId, item.Size, item.Quantity);
                    if (!stockUpdated)
                    {
                        MessageBox.Show($"Not enough stock for {item.ProductName} ({item.Color}, Size: {item.Size})", "Stock Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        ProgressBar.Visibility = Visibility.Collapsed;
                        PayNowButton.Visibility = Visibility.Visible;
                        return;
                    }
                }

                ProgressBar.Value = 50;

                // STEP 2: Save the invoice (includes file writing)
                bool invoiceSaved = await saveInvoiceWithProgress();
               
                if (!invoiceSaved)
                {
                    ProgressBar.Visibility = Visibility.Collapsed;
                    PayNowButton.Visibility = Visibility.Visible;
                    return;
                }

                // STEP 3: Delete the cart
               // await StoreFront.DeleteCart(User.Id,Cart.CartId);

                parentWindow.StoreCart.Purchased = true;
                parentWindow.StoreCart.Items.Clear();
                parentWindow.StoreCart.UpdatedAt = DateTime.Now;
                parentWindow.StoreCart.ShippingCost = 0;
                parentWindow.CartStatus();


                await StoreFront.AddOrUpdateCart(parentWindow.StoreCart, User.Id);

                ProgressBar.Value = 100;
                await Task.Delay(200);

                MessageBox.Show("Payment successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // Optionally navigate to another page
                parentWindow.MainFramePage.Navigate(new Pages.MainPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ProgressBar.Visibility = Visibility.Collapsed;
                PayNowButton.Visibility = Visibility.Visible;
            }
        }



        private async Task<bool> saveInvoiceWithProgress()
        {
            try
            {
                ProgressBar.Value = 10;
                await Task.Delay(200); // Simulate startup

                var invoiceNumber = await StoreFront.GenerateInvoiceNumberAsync();
                ProgressBar.Value = 30;
                await Task.Delay(100);

                var companyDetails = await StoreFront.GetCompanyInfo();
                ProgressBar.Value = 50;
                await Task.Delay(100);

                var invoice = new Invoice
                {
                    InvoiceNumber = invoiceNumber,
                    InvoiceDate = DateTime.Now,
                    CompanyName = companyDetails.Name,
                    CompanyNIF = companyDetails.Nif,
                    CompanyRegistry = companyDetails.RegistroMercantil,
                    CompanyAddress = companyDetails.Addresses.FirstOrDefault(),
                    CompanyPhone = companyDetails.Phone,
                    CompanyEmail = companyDetails.Email,
                    CustomerNIF = User.Nif,
                    CustomerName = $"{User.FirstName} {User.LastName}",
                    CustomerAddress = User.Addresses.FirstOrDefault(),
                    CustomerPhone = User.Phone,
                    CustomerEmail = User.Email,

                    Items = Cart.Items.Select(item => new InvoiceItems
                    {
                        Description = $"{item.ProductName} ({item.Color}, Size: {item.Size})",
                        Quantity = item.Quantity,
                        Price = item.BasePrice,
                        LineAmount = item.LineAmount,
                        DiscountPercent = item.DiscountPercentage,
                        DiscountAmount = item.DiscountAmount,
                        VatPercentage = item.VatPercentage,
                        NetAmount = item.NetAmount,
                        VatAmount = item.VatAmount,
                    }).ToList(),

                    SubTotal = Cart.SubTotal,
                    ShippingMethod = Cart.ShippingMethod,
                    ShippingCost = Cart.ShippingCost,
                    TotalVAT = Cart.TotalVat,
                    Total = Cart.Total,
                };

                ProgressBar.Value = 75;
                await Task.Delay(200); // simulate processing




                // Save to file
            /*    File.AppendAllText("invoices.txt", GenerateInvoiceText(invoice));*/
                await StoreFront.SaveInvoice(invoice);


                var downloader = new InvoiceDownloader(
                        "http://10.2.124.70:8080/jasperserver",
                        "jasperadmin",
                        "bitnami");

                // When calling the method:
                bool reportDownloaded = await downloader.DownloadReport(
      reportUri: "/StoreFrontReports/storefrontInvoice", // Fixed case
      outputPath: @$"C:\Users\Public\Documents\{invoice.InvoiceNumber}.pdf",
      paramName: "invoiceNum", // Using the ID from your parameter control
      paramValue: invoice.InvoiceNumber
  );
                if (reportDownloaded)
                {
                    Console.WriteLine("PDF was successfully created");
                    // Maybe even try to open it automatically to verify
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = @$"C:\Users\Public\Documents\{invoice.InvoiceNumber}.pdf",
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Could not open PDF: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("PDF download failed");
                }

                /*
                string pdfPath = System.IO.Path.Combine("Invoices", $"{invoice.InvoiceNumber}.pdf");
                Directory.CreateDirectory("Invoices"); 
                var document = new InvoiceDocument(invoice);
                document.GeneratePdf(pdfPath);*/


                ProgressBar.Value = 100;

                await Task.Delay(300); 
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error saving invoice: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
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
        private string GenerateInvoiceText(Invoice invoice)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Invoice Number: {invoice.InvoiceNumber}");
            sb.AppendLine($"Date: {invoice.InvoiceDate:dd/MM/yyyy}");
            sb.AppendLine($"Company: {invoice.CompanyName} ({invoice.CompanyNIF})");
            sb.AppendLine($"Customer: {invoice.CustomerName} ({invoice.CustomerNIF})");
            sb.AppendLine($"-------------------------------------------");
            foreach (var item in invoice.Items)
            {
                sb.AppendLine($"{item.Description}");
                sb.AppendLine($"Qty: {item.Quantity}, Unit: {item.Price:C}, Disc: {item.DiscountAmount:C}, VAT: {item.VatAmount:C}, VAT %: {item.VatPercentage}");
                sb.AppendLine($"→ Line Total: {item.LineAmount:C}");
                sb.AppendLine();
            }
            sb.AppendLine($"Shipping: {invoice.ShippingCost:C} ({invoice.ShippingMethod})");
            sb.AppendLine($"VAT: {invoice.TotalVAT:C}");
            sb.AppendLine($"TOTAL: {invoice.Total:C}");
            sb.AppendLine($"===========================================");
            sb.AppendLine();

            return sb.ToString();
        }



    }
}
