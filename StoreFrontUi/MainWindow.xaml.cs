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
        public MainWindow()
        {
            InitializeComponent();
            MainFramePage.Navigate(new Pages.MainPage());
            Popup_Login.StaysOpen = false;
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
    }
}