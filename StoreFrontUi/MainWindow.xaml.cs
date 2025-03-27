using StoreFrontModel;
using StoreFrontUi.Utils;
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

        public User CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MainFramePage.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            NavigateToMainPage();
            Popup_Login.StaysOpen = false;
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
        }
        public void NavigateToCreateUserPage()
        {
            MainFramePage.Navigate(new Pages.CreateUserPage());
        }

        private void Btn_UserAcc_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup_Login.IsOpen = true;
        }

        private void Btn_UserAcc_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!Popup_Login.IsMouseOver && !Btn_UserAcc.IsMouseOver)
            {
                Popup_Login.IsOpen = false;
            }
        }

        private void Popup_Login_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!Btn_UserAcc.IsMouseOver)
            {
                Popup_Login.IsOpen = false;
            }
        }

   

        public void SetUpCurrentUser()
        {
            if (CurrentUser != null && CurrentUser.ProfileImage != null)
            {
                Btn_UserAcc.Background = ConvertImageToBrush.ConvertToImageBrush(CurrentUser.ProfileImage);
                Btn_Tb_Icon.Visibility = Visibility.Collapsed;
            }

            if(CurrentUser.ProfileImage == null)
            {
                Btn_UserAcc.Background = new SolidColorBrush(Colors.Transparent);
                Btn_Tb_Icon.Visibility = Visibility.Visible;
            }
        }
        }
}