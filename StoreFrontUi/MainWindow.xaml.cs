using StoreFrontModel;
using StoreFrontRepository;
using StoreFrontUi.Pages;
using StoreFrontUi.Utils;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace StoreFrontUi
{
    public partial class MainWindow : Window
    {


        public User CurrentUser
        {
            get { return (User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }



        public Cart StoreCart
        {
            get { return (Cart)GetValue(StoreCartProperty); }
            set { SetValue(StoreCartProperty, value); }
        }

       
        public static readonly DependencyProperty StoreCartProperty =
            DependencyProperty.Register("StoreCart", typeof(Cart), typeof(MainWindow), new PropertyMetadata(null));




        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(User), typeof(MainWindow), new PropertyMetadata(null));


        public IStoreFront storeFront;
        public ObservableCollection<CartItem> CartItems { get; set; } = new ObservableCollection<CartItem>();

        public MainWindow()
        {
            InitializeComponent();
            MainFramePage.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            NavigateToMainPage();
            Popup_Login.StaysOpen = false;

            storeFront = new StoreFrontRepository.StoreFrontRepository();
            StoreCart = new Cart();
           
              

            StoreCart.Items.CollectionChanged += (s, e) => CartStatus();


            MainFramePage.Navigated += MainFramePage_Navigated;


            this.DataContext = this;
        }

        public async Task loadusercart()
        {
            try
            {
                if (CurrentUser != null)
                {
                    var loadedCart = await storeFront.GetCartByUserId(CurrentUser.Id);

                    if (loadedCart != null)
                    {
                        StoreCart.UserId = loadedCart.UserId;
                        StoreCart.CartId = loadedCart.CartId;
                        StoreCart.CreatedAt = loadedCart.CreatedAt;
                        StoreCart.UpdatedAt = loadedCart.UpdatedAt;
                        StoreCart.Purchased = loadedCart.Purchased;

                        StoreCart.Items.Clear();
                        foreach (var item in loadedCart.Items)
                        {
                            StoreCart.Items.Add(item);
                        }
                    }
                    else
                    {
                        StoreCart = new Cart { UserId = CurrentUser.Id };
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading cart: " + ex.Message);
            }
        }


        public void NavigateToMainPage()
        {
            MainFramePage.Navigate(new Pages.MainPage());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFramePage.CanGoBack)
            {
                MainFramePage.GoBack();
            }
        }

        private void MainFramePage_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            BackButton.GetBindingExpression(Button.VisibilityProperty)?.UpdateTarget();

            Tb_search.Visibility = Visibility.Collapsed;
            Tb_search.IsEnabled = false;

            // If the new page is ProductsFilterPage, show it
            if (e.Content is ProductsFilterPage)
            {
                Tb_search.Visibility = Visibility.Visible;
                Tb_search.IsEnabled = true;
            }
        }

        public void NavigateToCreateUserPage()
        {
            MainFramePage.Navigate(new Pages.CreateUserPage());


        }


        private void Btn_UserAcc_MouseEnter(object sender, MouseEventArgs e)
        {
            if (CurrentUser != null && CurrentUser.ProfileImage != null)
            {
             
                Popup_loggedInUser.IsOpen = true;
                Popup_Login.IsOpen = false;
            }
            else
            {
                Popup_Login.IsOpen = true;
                Popup_loggedInUser.IsOpen = false;
            }
        }


        private void Btn_UserAcc_MouseLeave(object sender, MouseEventArgs e)
        {
            if (CurrentUser != null)
            {
               
                if (!Popup_loggedInUser.IsMouseOver && !Btn_UserAcc.IsMouseOver)
                {
                    Popup_loggedInUser.IsOpen = false;
                }
            }
            else
            {
             
                if (!Popup_Login.IsMouseOver && !Btn_UserAcc.IsMouseOver)
                {
                    Popup_Login.IsOpen = false;
                }
            }
        }

        private void Popup_Login_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!Btn_UserAcc.IsMouseOver && !Popup_Login.IsMouseOver)
            {
                Popup_Login.IsOpen = false;
            }
        }

        private void Popup_loggedInUser_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!Btn_UserAcc.IsMouseOver && !Popup_loggedInUser.IsMouseOver)
            {
                Popup_loggedInUser.IsOpen = false;
            }
        }

        public void SetUpCurrentUser()
        {
            if (CurrentUser != null && CurrentUser.ProfileImage != null)
            {
                Btn_UserAcc.Background = ConvertImageToBrush.ConvertToImageBrush(CurrentUser.ProfileImage);
                Btn_Tb_Icon.Visibility = Visibility.Collapsed;
            }
            else
            {
                Btn_UserAcc.Background = new SolidColorBrush(Colors.Transparent);
                Btn_Tb_Icon.Visibility = Visibility.Visible;
            }

            
        }

        public event Func<string, Task> GlobalSearchChanged;

        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = Tb_search.Text.Trim();

            if (GlobalSearchChanged != null)
            {
                await GlobalSearchChanged.Invoke(search);
            }

          
        }

    /*    public void addCartToDb()
        {
            if (StoreCart != null)
            {
                StoreCart.UserId = CurrentUser.Id;
                StoreCart.CreatedAt = DateTime.UtcNow;
                StoreCart.UpdatedAt = DateTime.UtcNow;

                // storeFront.AddCartToDb(StoreCart);
            }
        }*/


        public void CartStatus()
        {
            if (StoreCart?.Items?.Count > 0)
            {
                CartEclipse.Visibility = Visibility.Visible;
                Tb_QtItems.Text = StoreCart.Items.Count.ToString();
            }
            else
            {
                CartEclipse.Visibility = Visibility.Collapsed;
              
            }

        }

        private void Btn_Cart_Click(object sender, RoutedEventArgs e)
        {
            MainFramePage.Navigate(new Pages.CartPage());
        }
    }
}
