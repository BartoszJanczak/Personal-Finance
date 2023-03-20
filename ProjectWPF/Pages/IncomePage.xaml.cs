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

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
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
                if (!int.TryParse(textBox.Text + e.Key.ToString().Last(), out value) || value < 0 || value > 100)
                {
                    e.Handled = true;
                }
            }
        }

    }
}
