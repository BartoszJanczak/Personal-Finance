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
            string expenseName = ExpensesNameTextBox.Text;
            int expenseAmount = 0;
            bool isExpenseAmountValid = int.TryParse(ExpensesAmountTextBox.Text, out expenseAmount);
            string expenseCategory = ExpensesCategoryComboBox.Text;
            DateOnly expenseDate = new DateOnly(ExpensesDatePicker.SelectedDate.Value.Year, ExpensesDatePicker.SelectedDate.Value.Month, ExpensesDatePicker.SelectedDate.Value.Day);

            if (string.IsNullOrWhiteSpace(expenseName) && !isExpenseAmountValid)
            {
                ExpensesNameTextBox.BorderBrush = Brushes.Red;
                ExpensesAmountTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Please enter a valid Expense name and Amount.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(expenseName))
            {
                ExpensesNameTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Please enter a valid Expense name.");
                return;
            }
            else if (!isExpenseAmountValid)
            {
                ExpensesAmountTextBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Please enter a valid Amount.");
                return;
            }

            SQLiteConnection connection = new SQLiteConnection("Data Source=financeDB.sqlite3");
            connection.Open();

            string query = "INSERT INTO Expenses (ExpenseName, ExpenseAmount, ExpenseCategory, ExpenseDate) VALUES (@ExpenseName, @ExpenseAmount, @ExpenseCategory, @ExpenseDate);";

            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@ExpenseName", expenseName);
            command.Parameters.AddWithValue("@ExpenseAmount", expenseAmount);
            command.Parameters.AddWithValue("@ExpenseCategory", expenseCategory);
            command.Parameters.AddWithValue("@ExpenseDate", expenseDate.ToString("yyyy-MM-dd"));

            command.ExecuteNonQuery();

            connection.Close();

            RefreshExpenseTable();
        }
        private void RefreshExpenseTable()
        {
            ExpensesList.Clear();
            SQLiteConnection connection = new SQLiteConnection("Data Source=financeDB.sqlite3");
            connection.Open();

            string query = "SELECT * FROM Expenses;";
            SQLiteCommand command = new SQLiteCommand(query, connection);

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Expenses expenses = new Expenses()
                {
                    ExpenseName = reader.GetString(1),
                    ExpenseAmount = reader.GetInt32(2),
                    ExpenseCategory = reader.GetString(3),
                    ExpenseDate = DateOnly.Parse(reader.GetString(4)),
                };

                ExpensesList.Add(expenses);
            }

            connection.Close();

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
            Expenses selectedExpense = ExpensesTable.SelectedItem as Expenses;
            if (selectedExpense != null)
            {
                SQLiteConnection connection = new SQLiteConnection("Data Source=financeDB.sqlite3");
                connection.Open();

                string query = "SELECT ID_Expense FROM Expenses WHERE ExpenseName = @ExpenseName AND ExpenseAmount = @ExpenseAmount AND ExpenseCategory = @ExpenseCategory AND ExpenseDate = @ExpenseDate ;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@ExpenseName", selectedExpense.ExpenseName);
                command.Parameters.AddWithValue("@ExpenseAmount", selectedExpense.ExpenseAmount);
                command.Parameters.AddWithValue("@ExpenseCategory", selectedExpense.ExpenseCategory);
                command.Parameters.AddWithValue("@ExpenseDate", selectedExpense.ExpenseDate.ToString("yyyy-MM-dd"));
                int ExpenseID = Convert.ToInt32(command.ExecuteScalar());
                
                string deleteQuery = "DELETE FROM Expenses WHERE ID_Expense = @ID;";

                SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@ID", ExpenseID);

                deleteCommand.ExecuteNonQuery();

                connection.Close();

                ExpensesList.Remove(selectedExpense);

                RefreshExpenseTable();

                FilterExpensesTextBox.Text = "";
                ICollectionView view = CollectionViewSource.GetDefaultView(ExpensesTable.ItemsSource);
                view.Filter = null;
            }
        }
    }
}
