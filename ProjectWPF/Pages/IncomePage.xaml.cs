using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            IncomeDatePicker.SelectedDate = DateTime.Now.Date;
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
            else if (textBox == SavingsPercentageComboBox)
            {
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
        private void SaveIncome_Click(object sender, RoutedEventArgs e)
        {
            string incomeName = IncomeNameTextBox.Text;
            int incomeAmount = 0;
            bool isIncomeAmountValid = int.TryParse(IncomeAmountTextBox.Text, out incomeAmount);
            string incomeCategory = IncomeCategoryComboBox.Text;
            DateOnly incomeDate = new DateOnly(IncomeDatePicker.SelectedDate.Value.Year, IncomeDatePicker.SelectedDate.Value.Month, IncomeDatePicker.SelectedDate.Value.Day);
            int incomeSavings = SavingsCheckBox.IsChecked == true ? int.Parse(SavingsPercentageComboBox.Text) : 0;
            
            if (string.IsNullOrWhiteSpace(incomeName) && !isIncomeAmountValid)
            {
                IncomeNameTextBox.BorderBrush = Brushes.Red;
                IncomeAmountTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Please enter a valid Income name and Amount.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(incomeName))
            {
                IncomeNameTextBox.BorderBrush = Brushes.Red;
                IncomeAmountTextBox.BorderBrush = Brushes.Gray;
                MessageBox.Show("Please enter a valid Income name.");
                return;
            }
            else if (!isIncomeAmountValid)
            {
                IncomeAmountTextBox.BorderBrush = Brushes.Red;
                IncomeNameTextBox.BorderBrush = Brushes.Gray;
                MessageBox.Show("Please enter a valid Amount.");
                return;
            }
            Income newIncome = new Income()
            {
                IncomeName = incomeName,
                IncomeAmount = incomeAmount,
                IncomeCategory = incomeCategory,
                IncomeDate = incomeDate,
                IncomeSavings = incomeSavings
            };

            IncomeList.Add(newIncome);

            IncomeTable.ItemsSource = null;
            IncomeTable.ItemsSource = IncomeList;

            IncomeNameTextBox.BorderBrush = Brushes.Gray;
            IncomeAmountTextBox.BorderBrush = Brushes.Gray;
            IncomeNameTextBox.Text = "";
            IncomeAmountTextBox.Text = "";
            IncomeCategoryComboBox.SelectedIndex = 0;
            IncomeDatePicker.SelectedDate = DateTime.Now.Date;
            SavingsCheckBox.IsChecked = false;

            SavingsPage savingsPage = new SavingsPage(incomeAmount, incomeCategory, incomeName, incomeDate, incomeSavings);
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(IncomeTable.ItemsSource);
            view.Filter = item =>
            {
                ResetButton.Visibility = Visibility.Visible;
                if (FilterTextBox.Text == "") return true;
                Income income = item as Income;
                return income.IncomeCategory.ToLower().Contains(FilterTextBox.Text.ToLower());
            };
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(IncomeTable.ItemsSource);
            view.Filter = null;
            ResetButton.Visibility = Visibility.Hidden;
        }
        private void DeleteSelectedRow_Click(object sender, RoutedEventArgs e)
        {
            Income selectedIncome = IncomeTable.SelectedItem as Income;
            if (selectedIncome != null)
            {
                int index = IncomeList.IndexOf(selectedIncome);
                IncomeList.RemoveAt(index);
                ICollectionView view = CollectionViewSource.GetDefaultView(IncomeTable.ItemsSource);
                view.Refresh();
            }
        }
    }
}
