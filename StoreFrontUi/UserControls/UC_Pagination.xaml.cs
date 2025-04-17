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

    public partial class UC_Pagination : UserControl
    {
        public UC_Pagination()
        {
            InitializeComponent();
            UpdateButtons();
        }
        public static readonly DependencyProperty MinPageProperty = DependencyProperty.Register(
          nameof(MinPage),
          typeof(int),
          typeof(UC_Pagination),
          new PropertyMetadata(1, LimitChangedStatic));

        public static readonly DependencyProperty MaxPageProperty = DependencyProperty.Register(
            nameof(MaxPage),
            typeof(int),
            typeof(UC_Pagination),
            new PropertyMetadata(1, LimitChangedStatic));

        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            nameof(CurrentPage),
            typeof(int),
            typeof(UC_Pagination),
            new PropertyMetadata(1, PageChangedCallback));

        public static readonly DependencyProperty IsPreviousPageButtonEnabledProperty = DependencyProperty.Register(
            nameof(IsPreviousPageButtonEnabled),
            typeof(bool),
            typeof(UC_Pagination),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsNextPageButtonEnabledProperty = DependencyProperty.Register(
            nameof(IsNextPageButtonEnabled),
            typeof(bool),
            typeof(UC_Pagination),
            new PropertyMetadata(true));

        public event EventHandler PageChanged;

        public int MinPage
        {
            get => (int)GetValue(MinPageProperty);
            set => SetValue(MinPageProperty, value);
        }

        public int MaxPage
        {
            get => (int)GetValue(MaxPageProperty);
            set => SetValue(MaxPageProperty, value);
        }

        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public bool IsPreviousPageButtonEnabled
        {
            get => (bool)GetValue(IsPreviousPageButtonEnabledProperty);
            set => SetValue(IsPreviousPageButtonEnabledProperty, value);
        }

        public bool IsNextPageButtonEnabled
        {
            get => (bool)GetValue(IsNextPageButtonEnabledProperty);
            set => SetValue(IsNextPageButtonEnabledProperty, value);
        }

        private static void LimitChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as UC_Pagination)?.UpdateButtons();
        }

        private static void PageChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UC_Pagination control = d as UC_Pagination;
            control?.UpdateButtons();
            control?.PageChanged?.Invoke(control, EventArgs.Empty);
        }

        private void UpdateButtons()
        {
            PreviousPageButton.IsEnabled = CurrentPage > MinPage;
            NextPageButton.IsEnabled = CurrentPage < MaxPage;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > MinPage)
            {
                CurrentPage--;
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < MaxPage)
            {
                CurrentPage++;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse((sender as TextBox).Text, out int newPage))
            {
                if (newPage >= MinPage && newPage <= MaxPage)
                {
                    CurrentPage = newPage;
                }
            }
            UpdateButtons();
        }
    }
}
