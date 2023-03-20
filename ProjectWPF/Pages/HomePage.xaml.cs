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

namespace ProjectWPF.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Close();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.WindowState = WindowState.Minimized;
        }
        private void ManageIncome_Button(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.IncomeRadioButton.IsChecked = true;
            mainWindow.HomeRadioButton.IsChecked = false;
            MainFrame.Navigate(new Uri("Pages/IncomePage.xaml", UriKind.Relative));
        }
        private void ManageExpenses_Button(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ExpensesRadioButton.IsChecked = true;
            MainFrame.Navigate(new Uri("Pages/ExpensesPage.xaml", UriKind.Relative));
        }
        private void ManageSavings_Button(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.SavingsRadioButton.IsChecked = true;
            MainFrame.Navigate(new Uri("Pages/SavingsPage.xaml", UriKind.Relative));
        }
        private void ShowProfits_Button(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ReportsRadioButton.IsChecked = true;
            MainFrame.Navigate(new Uri("Pages/ReportsPage.xaml", UriKind.Relative));
        }
    }
}
