using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
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
    /// Interaction logic for ExpensesPage.xaml
    /// </summary>
    public partial class ExpensesPage : Page
    {
        public ExpensesPage()
        {
            InitializeComponent();
            // nawiązanie połączenia z bazą danych
            SQLiteConnection connection = new SQLiteConnection("Data Source=financeDB.sqlite3");
            connection.Open();

            // pobranie danych z bazy danych i dodanie ich do listy
            string selectDataSQL = "SELECT * FROM Expenses";
            SQLiteCommand selectDataCommand = new SQLiteCommand(selectDataSQL, connection);
            SQLiteDataReader dataReader = selectDataCommand.ExecuteReader();
            ExpensesList = new List<Expenses>();
            while (dataReader.Read())
            {
                Expenses expenses = new Expenses()
                {
                    ExpenseName = dataReader["ExpenseName"].ToString(),
                    ExpenseAmount = int.Parse(dataReader["ExpenseAmount"].ToString()),
                    ExpenseCategory = dataReader["ExpenseCategory"].ToString(),
                    ExpenseDate = DateOnly.Parse(dataReader["ExpenseDate"].ToString()),
                };
                ExpensesList.Add(expenses);
            }
            dataReader.Close();
            ExpensesTable.ItemsSource = ExpensesList;
            ExpensesDatePicker.SelectedDate = DateTime.Now.Date;
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
        private void AmountExpenses_PreviewKeyDown(object sender, KeyEventArgs e)
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
        }
        public class Expenses
        {
            public string ExpenseName { get; set; }
            public int ExpenseAmount { get; set; }
            public string ExpenseCategory { get; set; }
            public DateOnly ExpenseDate { get; set; }
        }
        List<Expenses> ExpensesList = new List<Expenses>()
        {

        };
        private void SaveExpenses_Click(object sender, RoutedEventArgs e)
        {
            string expensesName = ExpensesNameTextBox.Text;
            int expensesAmount = 0;
            bool isExpensesAmountValid = int.TryParse(ExpensesAmountTextBox.Text, out expensesAmount);
            string expensesCategory = ExpensesCategoryComboBox.Text;
            DateOnly expensesDate = new DateOnly(ExpensesDatePicker.SelectedDate.Value.Year, ExpensesDatePicker.SelectedDate.Value.Month, ExpensesDatePicker.SelectedDate.Value.Day);

            if (string.IsNullOrWhiteSpace(expensesName) && !isExpensesAmountValid)
            {
                ExpensesNameTextBox.BorderBrush = Brushes.Red;
                ExpensesAmountTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Please enter a valid Expense name and Amount.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(expensesName))
            {
                ExpensesNameTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Please enter a valid Expense name.");
                return;
            }
            else if (!isExpensesAmountValid)
            {
                ExpensesAmountTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Please enter a valid Amount.");
                return;
            }
            Expenses newExpenses = new Expenses()
            {
                ExpenseName = expensesName,
                ExpenseAmount = expensesAmount,
                ExpenseCategory = expensesCategory,
                ExpenseDate = expensesDate,
            };

            ExpensesList.Add(newExpenses);

            ExpensesTable.ItemsSource = null;
            ExpensesTable.ItemsSource = ExpensesList;

            ExpensesNameTextBox.BorderBrush = Brushes.Gray;
            ExpensesAmountTextBox.BorderBrush = Brushes.Gray;
            ExpensesNameTextBox.Text = "";
            ExpensesAmountTextBox.Text = "";
            ExpensesCategoryComboBox.SelectedIndex = 0;
            ExpensesDatePicker.SelectedDate = DateTime.Now.Date;
        }
        private void FilterExpensesButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ExpensesTable.ItemsSource);
            view.Filter = item =>
            {
                ResetExpensesButton.Visibility = Visibility.Visible;
                if (FilterExpensesTextBox.Text == "") return true;
                Expenses expenses = item as Expenses;
                return expenses.ExpenseCategory.ToLower().Contains(FilterExpensesTextBox.Text.ToLower());
            };
        }
        private void ResetExpensesButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ExpensesTable.ItemsSource);
            view.Filter = null;
            ResetExpensesButton.Visibility = Visibility.Hidden;
        }
        private void DeleteSelectedExpensesRow_Click(object sender, RoutedEventArgs e)
        {
            Expenses selectedIncome = ExpensesTable.SelectedItem as Expenses;
            if (selectedIncome != null)
            {
                int index = ExpensesList.IndexOf(selectedIncome);
                ExpensesList.RemoveAt(index);
                ICollectionView view = CollectionViewSource.GetDefaultView(ExpensesTable.ItemsSource);
                view.Refresh();
            }
        }
    }
}
