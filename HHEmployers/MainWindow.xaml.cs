using System;
using System.Windows;
using System.Windows.Documents;
using HH_API;

namespace HHEmployers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            dataGridEmployers.ItemsSource = null;
            dataGridEmployers.Items.Clear();
            dataGridEmployers.Items.Refresh();

            try
            {
                var result = await HeadhunterClient.GetEmployersAsync(textBoxName.Text);
                if (result != null)
                {
                    dataGridEmployers.ItemsSource = result.Items;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            try
            {
                var result = await HeadhunterClient.GetEmployerInfoAsync(link.NavigateUri.OriginalString);
                if (result != null)
                {
                    MessageBox.Show(result.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
