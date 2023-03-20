using LiveCharts;
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
