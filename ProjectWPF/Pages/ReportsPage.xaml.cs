using LiveCharts;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            InitializeComponent();
            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;
            // pobranie danych z bazy danych i obliczenie sumy oszczędności
            SQLiteConnection connection = new SQLiteConnection("Data Source=financeDB.sqlite3");
            connection.Open();
            string selectDataSQL = "SELECT SUM(SavingsAmount) FROM Savings";
            SQLiteCommand selectDataCommand = new SQLiteCommand(selectDataSQL, connection);
            double totalSavings = Convert.ToDouble(selectDataCommand.ExecuteScalar());
            TotalSavingsReports.Text = totalSavings.ToString("C");
            string selectIncomeSQL = "SELECT SUM(IncomeAmount) FROM Income";
            SQLiteCommand selectIncomeCommand = new SQLiteCommand(selectIncomeSQL, connection);
            double totalIncome = Convert.ToDouble(selectIncomeCommand.ExecuteScalar());
            TotalIncomeReports.Text = totalIncome.ToString("C");
            string selectExpensesSQL = "SELECT SUM(ExpenseAmount) FROM Expenses";
            SQLiteCommand selectExpensesCommand = new SQLiteCommand(selectExpensesSQL, connection);
            double totalExpenses = Convert.ToDouble(selectExpensesCommand.ExecuteScalar());
            TotalExpensesReports.Text = totalExpenses.ToString("C");
            double totalProfit = totalIncome - totalExpenses;
            TotalProfitReports.Text = totalProfit.ToString("C");
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
        public Func<ChartPoint, string> PointLabel { get; set; }

        private void YearlyChart_Button(object sender, RoutedEventArgs e)
        {
            if (IncomeYearlyChart.Visibility == Visibility.Hidden)
            {
                IncomeYearlyChart.Visibility = Visibility.Visible;
                IncomeMonthlyChart.Visibility = Visibility.Hidden;
                YearlyChart.Content = "Monthly";
            }
            else if (IncomeYearlyChart.Visibility == Visibility.Visible)
            {
                IncomeYearlyChart.Visibility = Visibility.Hidden;
                IncomeMonthlyChart.Visibility = Visibility.Visible;
                YearlyChart.Content = "Yearly";
            }
        }
    }
}
