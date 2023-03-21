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
    /// Interaction logic for SavingsPage.xaml
    /// </summary>
    public partial class SavingsPage : Page
    {
        public SavingsPage()
        {
            InitializeComponent();
            SavingsTable.ItemsSource = SavingsList;
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
        public class Savings
        {
            public string AmountSavings { get; set; }
            public string CategorySavings { get; set; }
            public string SourceSavings { get; set; }
            public DateOnly DateSavings { get; set; }
        }
        List<Savings> SavingsList = new List<Savings>()
        {
            new Savings() { AmountSavings = "380", CategorySavings = "Salary", SourceSavings = "Work", DateSavings = new DateOnly(2021, 2, 1) },
            new Savings() { AmountSavings = "400", CategorySavings = "Salary", SourceSavings = "Work", DateSavings = new DateOnly(2023, 3, 1) },
            new Savings() { AmountSavings = "500", CategorySavings = "Investment", SourceSavings = "Bitcoin", DateSavings = new DateOnly(2023, 3, 12) },
            new Savings() { AmountSavings = "800", CategorySavings = "Rental", SourceSavings = "Flat", DateSavings = new DateOnly(2023, 3, 15) },
            new Savings() { AmountSavings = "160", CategorySavings = "Other", SourceSavings = "Gift", DateSavings = new DateOnly(2023, 3, 17) },
        };
    }
}
