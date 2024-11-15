using BusinessObjects;
using BusinessObjects.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WineWarehouseManagement
{
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
            LoadWareHouseWineList();
            LoadWineList();
            LoadWarehouseList();
            LoadLocationList();
        }
        private void LoadLocationList()
        {
            LocationComboBox.ItemsSource = _wareHouseDAO.GetAllWareHouses();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (LocationComboBox.SelectedValue is string selectedLocation)
            {
                // Fetch all details and then filter by the selected location
                var allDetails = _wareHouseDAO.GetWareHouseWineDetails();

                // Apply the location filter
                var filteredDetails = allDetails
                    .Where(ww => ww.GetType().GetProperty("Location")?.GetValue(ww)?.ToString() == selectedLocation)
                    .ToList();

                // Bind the filtered data to the DataGrid
                WareHousesDataGrid.ItemsSource = filteredDetails;
            }
            else
            {
                MessageBox.Show("Please select a location to search.");
            }
        }



        private void LoadWareHouseWineList()
        {
            WareHousesDataGrid.ItemsSource = _wareHouseDAO.GetWareHouseWineDetails();
        }

        private void LoadWineList()
        {
            WineComboBox.ItemsSource = _wineDAO.GetAllWines();
        }

        private void LoadWarehouseList()
        {
            WarehouseComboBox.ItemsSource = _wareHouseDAO.GetAllWareHouses();
        }

        public class AddWineWarehouseViewModel
        {
            public int? SelectedWineId { get; set; }
            public int? SelectedWareHouseId { get; set; }
            public int Quantity { get; set; }
            public string Description { get; set; }

            public AddWineWarehouseViewModel()
            {
                SelectedWineId = null;
                SelectedWareHouseId = null;
                Quantity = 0;
                Description = "";
            }
        }

        private void WareHousesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WareHousesDataGrid.SelectedItem is WarehouseWine selectedWarehouseWine)
            {
                var viewModel = (AddWineWarehouseViewModel)DataContext;
                viewModel.SelectedWineId = selectedWarehouseWine.WineId;
                viewModel.SelectedWareHouseId = selectedWarehouseWine.WareHouseId;
                viewModel.Quantity = selectedWarehouseWine.Quantity.Value;
                viewModel.Description = selectedWarehouseWine.Description;

                WineComboBox.SelectedValue = viewModel.SelectedWineId;
                WarehouseComboBox.SelectedValue = viewModel.SelectedWareHouseId;
                QuantityBox.Text = viewModel.Quantity.ToString();
                DescriptionBox.Text = viewModel.Description;
            }
        }



        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (AddWineWarehouseViewModel)DataContext;

            // Check if both IDs are selected
            if (viewModel.SelectedWineId.HasValue && viewModel.SelectedWareHouseId.HasValue)
            {
                // Check for duplicate entry before adding
                if (!DuplicateWarehouseWineExists(viewModel.SelectedWineId.Value))
                {
                    var newWarehouseWine = new WarehouseWine
                    {
                        WareHouseId = viewModel.SelectedWareHouseId.Value,
                        WineId = viewModel.SelectedWineId.Value,
                        Quantity = viewModel.Quantity,
                        Description = viewModel.Description
                    };

                    _repo.AddWarehouseWine(newWarehouseWine);
                    LoadWareHouseWineList(); // Refresh the list after adding

                    // Clear selection and input fields
                    viewModel.SelectedWineId = null;
                    viewModel.SelectedWareHouseId = null;
                    viewModel.Quantity = 0;
                    viewModel.Description = "";
                }
                else
                {
                    MessageBox.Show("This wine already exists in warehouse.");
                }
            }
            else
            {
                MessageBox.Show("Please select a Wine and Warehouse from the dropdowns.");
            }
        }

        private bool DuplicateWarehouseWineExists(int wineId)
        {
            using (var db = new WineManagement2Context())
            {
                return db.WarehouseWines.Any(ww => ww.WineId == wineId);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (AddWineWarehouseViewModel)DataContext;

            // Check if all required fields have values
            if (!viewModel.SelectedWineId.HasValue || !viewModel.SelectedWareHouseId.HasValue || viewModel.Quantity <= 0 || string.IsNullOrWhiteSpace(viewModel.Description))
            {
                MessageBox.Show("Please make sure no field is empty and quantity is greater than 0.");
                return;
            }

            // Ensure an item is selected in the DataGrid for update
            if (WareHousesDataGrid.SelectedItem is WarehouseWine selectedWarehouseWine)
            {
                if (!DuplicateWarehouseWineExists(viewModel.SelectedWineId.Value))
                {
                    // Update the selected WarehouseWine details with values from the view model
                    selectedWarehouseWine.WineId = viewModel.SelectedWineId.Value;
                    selectedWarehouseWine.WareHouseId = viewModel.SelectedWareHouseId.Value;
                    selectedWarehouseWine.Quantity = viewModel.Quantity;
                    selectedWarehouseWine.Description = viewModel.Description;

                    // Call repository update method
                    _repo.UpdateWarehouseWine(selectedWarehouseWine);

                    // Refresh the data grid to show updated information
                    LoadWareHouseWineList();

                    // Clear the fields after updating
                    viewModel.SelectedWineId = null;
                    viewModel.SelectedWareHouseId = null;
                    viewModel.Quantity = 0;
                    viewModel.Description = "";

                    MessageBox.Show("Warehouse-Wine record updated successfully.");
                }
                else
                {
                    MessageBox.Show("This wine already exists in warehouse.");
                }
            }
            else
            {
                MessageBox.Show("Please select a Warehouse-Wine record to update.");
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (WareHousesDataGrid.SelectedItem is WarehouseWine selectedWarehouseWine)
            {
                _repo.DeleteWarehouseWine(selectedWarehouseWine.WarehouseWineId);
                LoadWareHouseWineList(); // Refresh the list after deleting
            }
            else
            {
                MessageBox.Show("Please select a Warehouse-Wine record to delete.");
            }
        }

        private void BacktoManagerHomePage_click(object sender, RoutedEventArgs e)
        {
            ManagerHomePageWindow managerWindow = new ManagerHomePageWindow();
            managerWindow.Show();
            this.Close();
        }
    }
}
