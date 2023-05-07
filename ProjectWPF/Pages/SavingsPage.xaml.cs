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
    /// Interaction logic for SavingsPage.xaml
    /// </summary>
    public partial class SavingsPage : Page
    {
        public SavingsPage()
        {
            InitializeComponent();

            SQLiteConnection connection = new SQLiteConnection("Data Source=financeDB.sqlite3");
            connection.Open();

            // pobranie danych z bazy danych i dodanie ich do listy
            string selectDataSQL = "SELECT * FROM Savings";
            SQLiteCommand selectDataCommand = new SQLiteCommand(selectDataSQL, connection);
            SQLiteDataReader dataReader = selectDataCommand.ExecuteReader();
            SavingsList = new List<Savings>();
            while (dataReader.Read())
            {
                Savings savings = new Savings()
                {
                    AmountSavings = double.Parse(dataReader["SavingsAmount"].ToString()),
                    CategorySavings = dataReader["SavingsCategory"].ToString(),
                    SourceSavings = dataReader["SavingsSource"].ToString(),
                    DateSavings = DateOnly.Parse(dataReader["SavingsDate"].ToString()),
                };
                SavingsList.Add(savings);
            }
            dataReader.Close();
            SavingsTable.ItemsSource = SavingsList;
            double totalSavings = SavingsList.Sum(s => s.AmountSavings);
            TotalSavings.Content = totalSavings.ToString("C");
            EstimatedSavings();
            RefreshSavingsTable();
            connection.Close();
        }
        private void EstimatedSavings()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=financeDB.sqlite3");
            connection.Open();
            string selectMonthlySavingsSQL = "SELECT SUM(SavingsAmount), strftime('%Y-%m', SavingsDate) AS MonthYear " +
                                 "FROM Savings " +
                                 "GROUP BY MonthYear";
            SQLiteCommand selectMonthlySavingsCommand = new SQLiteCommand(selectMonthlySavingsSQL, connection);
            SQLiteDataReader monthlySavingsReader = selectMonthlySavingsCommand.ExecuteReader();
            //tworze pustą listę, która będzie przechowywać sumy oszczędności dla każdego miesiąca.
            List<double> monthlySavingsList = new List<double>();
            while (monthlySavingsReader.Read())
            {
                double monthlySavings = monthlySavingsReader.GetDouble(0);
                monthlySavingsList.Add(monthlySavings);
            }
            monthlySavingsReader.Close();
            double averageMonthlySavings = monthlySavingsList.Average();
            double estimatedSavings = averageMonthlySavings * 12;
            EstimatedSavingsLabel.Content = estimatedSavings.ToString("C");
            connection.Close();
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
            public double AmountSavings { get; set; }
            public string CategorySavings { get; set; }
            public string SourceSavings { get; set; }
            public DateOnly DateSavings { get; set; }
        }
        List<Savings> SavingsList = new List<Savings>()
        {
            
        };
        private void RefreshSavingsTable()
        {
            SavingsList.Clear();
            SQLiteConnection connection = new SQLiteConnection("Data Source=financeDB.sqlite3");
            connection.Open();

            string query = "SELECT * FROM Savings;";
            SQLiteCommand command = new SQLiteCommand(query, connection);

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Savings savings = new Savings()
                {
                    AmountSavings = reader.GetDouble(1),
                    CategorySavings = reader.GetString(2),
                    SourceSavings = reader.GetString(3),
                    DateSavings = DateOnly.Parse(reader.GetString(4))
                };
                SavingsList.Add(savings);
            }

            connection.Close();

            SavingsTable.ItemsSource = null;
            SavingsTable.ItemsSource = SavingsList;
        }
        private void DeleteSelectedSavingsRow_Click(object sender, RoutedEventArgs e)
        {
            Savings selectedIncome = SavingsTable.SelectedItem as Savings;
            if (selectedIncome != null)
            {
                int index = SavingsList.IndexOf(selectedIncome);
                SavingsList.RemoveAt(index);
                ICollectionView view = CollectionViewSource.GetDefaultView(SavingsTable.ItemsSource);
                view.Refresh();
            }
        }
    }
}
