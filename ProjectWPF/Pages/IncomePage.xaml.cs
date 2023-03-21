using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for IncomePage.xaml
    /// </summary>
    public partial class IncomePage : Page
    {
        public IncomePage()
        {
            InitializeComponent();
            IncomeTable.ItemsSource = IncomeList;
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

        private void SavingsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SavingsPercentageComboBox.Visibility = Visibility.Visible;
            SavingsPercentageLabel.Visibility = Visibility.Visible;
        }
        private void SavingsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SavingsPercentageComboBox.Visibility = Visibility.Hidden;
            SavingsPercentageLabel.Visibility = Visibility.Hidden;
        }

        private void AmountIncome_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
                {
                    if (e.Key != Key.Back && e.Key != Key.Delete && e.Key != Key.Left && e.Key != Key.Right)
                    {
                        e.Handled = true;
                    }
                }
            }
            else
            {
                // Sprawdzenie, czy wprowadzona wartość jest liczbą z zakresu 0-100
                int value;
                if (!int.TryParse(SavingsPercentageComboBox.Text + e.Key.ToString().Last(), out value) || value < 0 || value > 100)
                {
                    e.Handled = true;
                }
            }
        }
        public class Income
        {
            public string IncomeName { get; set; }
            public int IncomeAmount { get; set; }
            public string IncomeCategory { get; set; }
            public DateOnly IncomeDate { get; set; }
            public int IncomeSavings { get; set; }
        }
        List<Income> IncomeList = new List<Income>()
        {
            new Income() { IncomeName = "Work", IncomeAmount = 3800, IncomeCategory = "Salary", IncomeDate = new DateOnly(2023, 2, 1), IncomeSavings = 10 },
            new Income() { IncomeName = "Work", IncomeAmount = 4000, IncomeCategory = "Salary", IncomeDate = new DateOnly(2023, 3, 1), IncomeSavings = 10 },
            new Income() { IncomeName = "Bitcoin", IncomeAmount = 1000, IncomeCategory = "Investment", IncomeDate = new DateOnly(2023, 3, 12), IncomeSavings = 50 },
            new Income() { IncomeName = "Flat", IncomeAmount = 2000, IncomeCategory = "Rental", IncomeDate = new DateOnly(2023, 3, 15), IncomeSavings = 40 },
            new Income() { IncomeName = "Gift", IncomeAmount = 200, IncomeCategory = "Other", IncomeDate = new DateOnly(2023, 3, 17), IncomeSavings = 80 },
        };
    }
}
