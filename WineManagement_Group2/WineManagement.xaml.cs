using DataAccessLayer;
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
using System.Windows.Shapes;

namespace WineWarehouseManagement
{
    /// <summary>
    /// Interaction logic for WineManagement.xaml
    /// </summary>
    public partial class WineManagement : Window
    {

        private readonly IWineRepository _wineDAO;
        public WineManagement()
        {
            InitializeComponent();
            _wineDAO = new WineDAO();
            LoadWinefList();
        }

        private void LoadWinefList()
        {
            // Loads only accounts with "Staff" role into the DataGrid
            StaffDataGrid.ItemsSource = _wineDAO.GetAllWines();
        }

        private void ReadWineButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateWineButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateWineButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteWineButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReadManagerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateManagerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateManagerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteManagerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
