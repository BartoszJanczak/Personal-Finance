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

namespace ProjectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.GetPosition(this).Y < 20)
                DragMove();
        }
        private void IncomeButton_Checked(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Pages/IncomePage.xaml", UriKind.Relative));
        }
        private void ExpensesButton_Checked(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Pages/ExpensesPage.xaml", UriKind.Relative));
        }
        private void SavingsButton_Checked(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Pages/SavingsPage.xaml", UriKind.Relative));
        }
        private void ReportsButton_Checked(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Pages/ReportsPage.xaml", UriKind.Relative));
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Pages/IncomePage.xaml", UriKind.Relative));
        }
    }
}
