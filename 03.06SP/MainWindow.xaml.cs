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

namespace _03._06SP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ExecuteAssemblyAsync(string fileName, string domainName)
        {
            SetLoading(true);
            SaveButton.IsEnabled = false;
            PerformingCalculationsButton.IsEnabled = false;

            var currentDomain = AppDomain.CurrentDomain;
            var secondDomain = AppDomain.CreateDomain(domainName);
            var appPath = currentDomain.BaseDirectory + fileName;

            try
            {
                await Task.Run(() => secondDomain.ExecuteAssembly(appPath));
            }

            catch (Exception exception)
            {
                SetLoading(false);
                MessageBox.Show(exception.Message);
                AppDomain.Unload(secondDomain);
                return;
            }

            SaveButton.IsEnabled = true;
            PerformingCalculationsButton.IsEnabled = true;
            SetLoading(false);

            MessageBox.Show("Task completed!");
            AppDomain.Unload(secondDomain);
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            ExecuteAssemblyAsync("SaveFile.exe", "SaveFile");
        }

        private void PerformingCalculationsButtonClick(object sender, RoutedEventArgs e)
        {
            ExecuteAssemblyAsync("PerformingCalculation.exe", "PerformingCalculationApp");
        }

        private void SetLoading(bool isLoading)
        {
            if (isLoading)
            {
                progressBar.Visibility = Visibility.Visible;
                progressLabel.Content = "Please wait...";
            }
            else
            {
                progressBar.Visibility = Visibility.Collapsed;
                progressLabel.Content = "Done";
            }
        }
    }
}
