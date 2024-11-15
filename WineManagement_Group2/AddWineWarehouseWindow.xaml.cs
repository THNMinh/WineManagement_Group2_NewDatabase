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
        private IEnumerable<dynamic> originalData;
        private bool isUpdating = false; // Flag to track if update is in progress
        private bool isDeleting = false; // Cờ để theo dõi trạng thái xóa


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
                // Lọc dữ liệu dựa trên vị trí được chọn
                var filteredDetails = originalData
                    .Where(ww => ww.Location.Equals(selectedLocation, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filteredDetails.Any())
                {
                    WareHousesDataGrid.ItemsSource = filteredDetails;
                }
                else
                {
                    MessageBox.Show("No records found for the selected location.");
                }

                // Giữ trạng thái của ComboBox
                LocationComboBox.SelectedValue = -1;
            }
            else
            {
                LoadWareHouseWineList();
            }
        }

        private void ResetSearch()
        {
            WareHousesDataGrid.ItemsSource = originalData; // Khôi phục dữ liệu gốc
        }




        //private void LoadWareHouseWineList()
        //{
        //    WareHousesDataGrid.ItemsSource = _wareHouseDAO.GetWareHouseWineDetails();
        //}
        private void LoadWareHouseWineList()
        {

            using (var db = new WineManagement2Context())
            {
                var wareHouseWineDetails = from ww in db.WarehouseWines
                                           join wh in db.WareHouses on ww.WareHouseId equals wh.WareHouseId
                                           join w in db.Wines on ww.WineId equals w.WineId
                                           where wh.Status == null  // Chỉ lấy các kho đang hoạt động
                                           select new
                                           {
                                               WarehouseWineId = ww.WarehouseWineId,
                                               WineId = ww.WineId,
                                               WareHouseId = ww.WareHouseId,
                                               WineName = w.Name,
                                               Address = wh.Address,
                                               ContactPerson = wh.ContactPerson,
                                               PhoneNumber = wh.PhoneNumber,
                                               Location = wh.Location,
                                               Quantity = ww.Quantity,
                                               Description = ww.Description
                                           };

                originalData = wareHouseWineDetails.ToList(); // Lưu trữ dữ liệu gốc
                WareHousesDataGrid.ItemsSource = originalData;
            }
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
            // Kiểm tra nếu có item được chọn
            var selectedItem = WareHousesDataGrid.SelectedItem;

            if (isDeleting)
            {
                isDeleting = false; // Tắt cờ sau khi hoàn tất xóa
                return;
            }
            if (isUpdating)
            {
                isUpdating = false; // T?t c? sau khi ho?n t?t cập nhật
                return;
            }

            if (selectedItem != null)
            {
                // Ép kiểu item được chọn về kiểu dữ liệu cụ thể
                dynamic selectedData = selectedItem;

                // Lấy giá trị từ các thuộc tính
                WineComboBox.SelectedValue = selectedData.WineId;
                WarehouseComboBox.SelectedValue = selectedData.WareHouseId;
                QuantityBox.Text = selectedData.Quantity?.ToString();
                DescriptionBox.Text = selectedData.Description;
            }
            else
            {
                // Xử lý khi không có item nào được chọn
                MessageBox.Show("Please select a valid record.");
            }
        }



        private bool DuplicateWarehouseWineExists(int wineId, int? warehouseWineId = null)
        {
            using (var db = new WineManagement2Context())
            {
                return db.WarehouseWines.Any(ww =>
                    ww.WineId == wineId &&
                    (!warehouseWineId.HasValue || ww.WarehouseWineId != warehouseWineId));
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
                        Quantity = int.Parse(QuantityBox.Text),
                        Description = DescriptionBox.Text
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

            // Lấy dữ liệu từ các ô nhập liệu
            if (WineComboBox.SelectedValue is int selectedWineId)
            {
                viewModel.SelectedWineId = selectedWineId;
            }

            if (WarehouseComboBox.SelectedValue is int selectedWarehouseId)
            {
                viewModel.SelectedWareHouseId = selectedWarehouseId;
            }

            if (int.TryParse(QuantityBox.Text, out int quantity))
            {
                viewModel.Quantity = quantity;
            }

            viewModel.Description = DescriptionBox.Text?.Trim();

            // Kiểm tra giá trị hợp lệ
            if (!viewModel.SelectedWineId.HasValue || !viewModel.SelectedWareHouseId.HasValue || viewModel.Quantity <= 0 || string.IsNullOrWhiteSpace(viewModel.Description))
            {
                MessageBox.Show("Please make sure no field is empty and quantity is greater than 0.");
                return;
            }

            // Kiểm tra xem một dòng đã được chọn trong DataGrid hay chưa
            if (WareHousesDataGrid.SelectedItem != null)
            {
                // Lấy bản ghi từ DataGrid
                var selectedData = WareHousesDataGrid.SelectedItem;

                // Kiểm tra xem selectedData có thuộc tính "WarehouseWineId" hay không
                var propertyInfo = selectedData.GetType().GetProperty("WarehouseWineId");

                if (propertyInfo != null)
                {
                    int warehouseWineId = (int)propertyInfo.GetValue(selectedData);

                    // Kiểm tra trùng lặp
                    if (!DuplicateWarehouseWineExists(viewModel.SelectedWineId.Value, warehouseWineId))
                    {
                        // Set the flag to skip selection changed validation during update
                        isUpdating = true;

                        // Cập nhật thông tin
                        using (var db = new WineManagement2Context())
                        {
                            var existingWarehouseWine = db.WarehouseWines.FirstOrDefault(ww => ww.WarehouseWineId == warehouseWineId);

                            if (existingWarehouseWine != null)
                            {
                                // Cập nhật thông tin
                                existingWarehouseWine.WineId = viewModel.SelectedWineId.Value;
                                existingWarehouseWine.WareHouseId = viewModel.SelectedWareHouseId.Value;
                                existingWarehouseWine.Quantity = viewModel.Quantity;
                                existingWarehouseWine.Description = viewModel.Description;

                                db.SaveChanges();
                            }
                        }

                        // Làm mới DataGrid
                        LoadWareHouseWineList();

                        // Xóa các trường nhập liệu sau khi cập nhật
                        WineComboBox.SelectedValue = null;
                        WarehouseComboBox.SelectedValue = null;
                        QuantityBox.Clear();
                        DescriptionBox.Clear();

                        MessageBox.Show("Warehouse-Wine record updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("This wine already exists in warehouse.");
                    }
                }
            }
        }








        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = WareHousesDataGrid.SelectedItem as dynamic;

            if (selectedRow == null)
            {
                MessageBox.Show("Please select a valid Warehouse-Wine record to delete.");
                return;
            }

            int warehouseWineId = selectedRow.WarehouseWineId;

            var result = MessageBox.Show(
                "Are you sure you want to delete this record?",
                "Delete Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Bật cờ xóa
                    isDeleting = true;

                    // Xóa bản ghi
                    _repo.DeleteWarehouseWine(warehouseWineId);

                    // Làm mới DataGrid
                    LoadWareHouseWineList();

                    MessageBox.Show("Warehouse-Wine record deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while deleting record: {ex.Message}");
                }
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
