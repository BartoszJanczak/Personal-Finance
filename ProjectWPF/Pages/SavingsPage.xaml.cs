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
        public SavingsPage() : this(0, "", "", new DateOnly(), 0) { }

        private string incomeName;
        private int incomeAmount;
        private string incomeCategory;
        private DateOnly incomeDate;
        private int incomeSavings;
        public SavingsPage(int incomeAmount, string incomeCategory, string incomeName, DateOnly incomeDate, int incomeSavings)
        {
            InitializeComponent();

            this.incomeAmount = incomeAmount;
            this.incomeCategory = incomeCategory;
            this.incomeName = incomeName;
            this.incomeDate = incomeDate;
            this.incomeSavings = incomeSavings;

            double savingsPercentage = double.Parse(incomeSavings.ToString());
            double savingsMultiplier = savingsPercentage / 100.0;
            double savingsAmount = incomeAmount * savingsMultiplier;
            if (savingsAmount > 0)
            {
                SavingsList.Add(new Savings() { AmountSavings = savingsAmount.ToString("N2"), CategorySavings = incomeCategory, SourceSavings = incomeName, DateSavings = incomeDate });
            }
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
