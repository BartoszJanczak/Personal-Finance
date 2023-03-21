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
    /// Interaction logic for ExpensesPage.xaml
    /// </summary>
    public partial class ExpensesPage : Page
    {
        public ExpensesPage()
        {
            InitializeComponent();
            ExpensesTable.ItemsSource = ExpensesList;
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
            public string NameExpense { get; set; }
            public int AmountExpense { get; set; }
            public string CategoryExpense { get; set; }
            public DateOnly DateExpense { get; set; }
        }
        List<Expenses> ExpensesList = new List<Expenses>()
        {
            new Expenses() { NameExpense = "Restaurant", AmountExpense = 200, CategoryExpense = "Food", DateExpense = new DateOnly(2023, 3, 11) },
            new Expenses() { NameExpense = "McDonald's", AmountExpense = 40, CategoryExpense = "Food", DateExpense = new DateOnly(2023, 3, 15) },
            new Expenses() { NameExpense = "Electricity", AmountExpense = 200, CategoryExpense = "Bills", DateExpense = new DateOnly(2023, 3, 17) },
            new Expenses() { NameExpense = "Fuel", AmountExpense = 180, CategoryExpense = "Transport", DateExpense = new DateOnly(2023, 3, 18) },
            new Expenses() { NameExpense = "Party", AmountExpense = 100, CategoryExpense = "Entertainment", DateExpense = new DateOnly(2023, 3, 20) },

        };
    }
}
