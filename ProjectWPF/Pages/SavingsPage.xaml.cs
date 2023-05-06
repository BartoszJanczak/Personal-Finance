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
