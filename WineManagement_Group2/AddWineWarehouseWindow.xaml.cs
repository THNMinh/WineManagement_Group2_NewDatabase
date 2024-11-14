using BusinessObjects.Entities;
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
    /// Interaction logic for AddWineWarehouseWindow.xaml
    /// </summary>
    public partial class AddWineWarehouseWindow : Window
    {
        private readonly IWareHouseRepository _wareHouseDAO;
        private readonly IWineRepository _wineDAO;
        private readonly IWarehouseWineRepository _repo;
        public AddWineWarehouseWindow()
        {
            InitializeComponent();
            _wareHouseDAO = new WareHouseDAO();
            _wineDAO = new WineDAO();
            _repo = new WarehouseWineDAO();
            DataContext = new AddWineWarehouseViewModel();
            LoadWareHouseList();
            LoadWinefList();
        }

        private void LoadWareHouseList()
        {
            WarehouseDataGrid.ItemsSource = _wareHouseDAO.GetAllWareHouses();
        }

        private void LoadWinefList()
        {
            // Loads only accounts with "Staff" role into the DataGrid
            WineDataGrid.ItemsSource = _wineDAO.GetAllWines();
        }

        public class AddWineWarehouseViewModel
        {
            public int? SelectedWineId { get; set; }
            public int? SelectedWareHouseId { get; set; }
            public int Quantity { get; set; } // Add a property for quantity
            public string Description { get; set; } // Add a property for description

            public AddWineWarehouseViewModel()
            {
                SelectedWineId = null;
                SelectedWareHouseId = null;
                Quantity = 0; // Set default quantity
                Description = ""; // Set default description
            }
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WineDataGrid.SelectedItem is Wine selectedWine)
            {

                WineIDTextBox.Text = selectedWine.WineId.ToString();
             }


        }

        private void dgData_SelectionChangedForWAREHOUSE(object sender, SelectionChangedEventArgs e)
        {
            if (WarehouseDataGrid.SelectedItem is WareHouse selectedWine)
            {

                WareHouseTextBox.Text = selectedWine.WareHouseId.ToString();
            }


        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

            var viewModel = (AddWineWarehouseViewModel)DataContext;
            viewModel.SelectedWineId = int.Parse(WineIDTextBox.Text);
            viewModel.SelectedWareHouseId = int.Parse(WareHouseTextBox.Text);
            // Check if both IDs are selected
            if (viewModel.SelectedWineId.HasValue && viewModel.SelectedWareHouseId.HasValue)
            {
                var newWarehouseWine = new WarehouseWine
                {
                    WareHouseId = viewModel.SelectedWareHouseId.Value,
                    WineId = viewModel.SelectedWineId.Value,
                    Quantity = int.Parse(QuantityBox.Text), // Access quantity from ViewModel
                    Description = DescriptionBox.Text // Access description from ViewModel
                };

                _repo.AddWarehouseWine(newWarehouseWine); // Add new WarehouseWine object to repository
                // Save changes to the database (assuming you have a SaveChanges method)

                // Clear selection and input fields (optional)
                viewModel.SelectedWineId = null;
                viewModel.SelectedWareHouseId = null;
                viewModel.Quantity = 0;
                viewModel.Description = "";
            }
            else
            {
                // Show error message if no selection is made
                MessageBox.Show("Please select a Wine and Warehouse from the DataGrids.");
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BacktoManagerHomePage_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
