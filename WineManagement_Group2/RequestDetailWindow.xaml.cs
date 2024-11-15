using Azure.Core;
using BusinessObjects;
using BusinessObjects.Entities;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for RequestDetailWindow.xaml
    /// </summary>
    public partial class RequestDetailWindow : Window
    {
        IRequestDetailRepository _requestDetailRepository;
        IRequestRepository _requestRepository;
        IWineRepository _wineRepository;
        private WineManagement2Context _context;
        int accountId;
        public RequestDetailWindow(int AccountId)
        {
            InitializeComponent();
            accountId = AccountId;
            _context = new WineManagement2Context(); // Initialize the context
            _requestRepository = new RequestDAO();
            _requestDetailRepository = new RequestDetailDAO();
            _wineRepository = new WineDAO();
            LoadWines(); // Load wine data into the DataGrid
            LoadComboList(); // Load d? li?u v?n h?ng vào ComboBox
            LoadRequest(); // Load d? li?u y?u cầu t? co s? d? li?u v? ?nh x? v?i c?c thu?c t?nh trong DataGrid
        }
        public void LoadComboList()
        {
            // Get all wines that have an entry in WarehouseWine
            var list = _context.WarehouseWines
                .Select(ww => ww.Wine)
                .Distinct() // Ensure unique wines are listed
                .ToList();

            // Bind the filtered list to the WineNameComboBox
            WineNameComboBox.ItemsSource = list;
            WineNameComboBox.DisplayMemberPath = "Name";
            WineNameComboBox.SelectedValuePath = "WineId";
        }


        private void LoadRequest()
        {
            try
            {
                var requestData = GetRequestData();
                if (requestData != null && requestData.Count > 0)
                {
                    RequestsDataGrid.ItemsSource = requestData;
                }
                else
                {
                    MessageBox.Show("No data found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading request data: " + ex.Message);
            }
        }



        public class RequestData()
        {
            public int RequestId { get; set; }
            public int RequestDetailId { get; set; }
            public int AccountId { get; set; }
            public string WineName { get; set; }
            public int Quantity { get; set; }
            public string Status { get; set; }
            public string Export { get; set; }
        }

        private List<RequestData> GetRequestData()
        {
            var requestData = _context.RequestDetails
                .Include(rd => rd.Request)  // Include related Request entity
                .Include(rd => rd.Wine)     // Include related Wine entity
                .Select(rd => new RequestData
                {
                    RequestId = rd.RequestId ?? 0,
                    RequestDetailId = rd.RequestDetailId,
                    AccountId = rd.Request != null ? rd.Request.AccountId ?? 0 : 0,
                    WineName = rd.Wine != null ? rd.Wine.Name ?? "Unknown" : "Unknown",
                    Quantity = rd.Quantity,
                    Status = rd.Request != null ? rd.Request.Status ?? "Unknown" : "Unknown",
                    Export = rd.Request != null
                        ? (rd.Request.Export.HasValue
                            ? (rd.Request.Export.Value == true ? "Import" : "Export")
                            : "Not Exported")
                        : "Not Exported"
                })
                .ToList();

            return requestData;
        }


        private void LoadWines()
        {
            try
            {
                // Get data of wines that are present in WarehouseWine, including their quantities in each warehouse
                var wineData = _context.WarehouseWines // Optional: filter to show only wines with positive quantities
                    .Select(ww => new
                    {
                        ww.Wine.Name,
                        ww.Wine.VintageYear,
                        ww.Wine.AlcoholContent,
                        ww.Wine.Price,
                        CategoryName = ww.Wine.Category != null ? ww.Wine.Category.CategoryName : "N/A",
                        Quantity = ww.Quantity
                    })
                    .ToList();

                // Bind data to WinesDataGrid
                WinesDataGrid.ItemsSource = wineData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading wine data: " + ex.Message);
            }
        }



        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new StaffWindow(accountId).Show();
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (WineNameComboBox.SelectedValue != null && int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                int wineId = (int)WineNameComboBox.SelectedValue;
                bool isExport = ExpportComboBox.SelectedValue.ToString() == "true";

                // If export is false (Import), check quantity availability
                if (!isExport)
                {
                    var warehouseWine = _context.WarehouseWines.FirstOrDefault(ww => ww.WineId == wineId);
                    if (warehouseWine != null && (warehouseWine.Quantity - quantity < 0))
                    {
                        MessageBox.Show("Insufficient stock for the requested quantity.");
                        return;
                    }
                }

                // Create a new request and add request details
                int requestId = CreateRequest(isExport);
                if (requestId > 0)
                {
                    var requestDetail = new RequestDetail
                    {
                        RequestId = requestId,
                        WineId = wineId,
                        Quantity = quantity
                    };

                    try
                    {
                        _requestDetailRepository.AddRequestDetail(requestDetail);
                        MessageBox.Show("Request detail created successfully!");
                        LoadRequest(); // Refresh the request data grid
                        ClearRequestDetailFields();

                        //// If export is false, reduce stock quantity
                        //if (!isExport)
                        //{
                        //    var warehouseWine = _context.WarehouseWines.FirstOrDefault(ww => ww.WineId == wineId);
                        //    if (warehouseWine != null)
                        //    {
                        //        warehouseWine.Quantity -= quantity;
                        //        _context.SaveChanges();
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding request detail: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to create request.");
                }
            }
            else
            {
                MessageBox.Show("Please select a wine and enter a valid quantity.");
            }
        }

        private int CreateRequest(bool isExport)
        {
            try
            {
                var request = new BusinessObjects.Entities.Request
                {
                    AccountId = accountId,
                    Status = "Pending",
                    Export = isExport // Set export status based on selection
                };

                _context.Requests.Add(request);
                _context.SaveChanges();
                return request.RequestId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating request: " + ex.Message);
                return 0;
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
            {
                if (WineNameComboBox.SelectedValue != null && int.TryParse(QuantityTextBox.Text, out int quantity))
                {
                    if (selectedRequest.Status != "Pending")
                    {
                        MessageBox.Show("This request cannot update.");
                        return;
                    }

                    int requestDetailId = selectedRequest.RequestDetailId;
                    int requestId = selectedRequest.RequestId;
                    int wineId = (int)WineNameComboBox.SelectedValue;
                    bool isExport = ExpportComboBox.SelectedValue.ToString() == "true";

                    // Lấy Request và RequestDetail từ cơ sở dữ liệu
                    var request = _context.Requests.FirstOrDefault(r => r.RequestId == requestId);
                    var requestDetail = _context.RequestDetails.FirstOrDefault(rd => rd.RequestDetailId == requestDetailId);

                    if (request != null && requestDetail != null)
                    {


                        // Kiểm tra kho nếu là nhập (Import)
                        if (!isExport)
                        {
                            var warehouseWine = _context.WarehouseWines.FirstOrDefault(ww => ww.WineId == wineId);
                            if (warehouseWine != null && (warehouseWine.Quantity - quantity < 0))
                            {
                                MessageBox.Show("Insufficient stock for the requested quantity.");
                                return;
                            }
                        }
                        // Cập nhật thông tin Request
                        request.Export = isExport;
                        // Cập nhật thông tin RequestDetail
                        requestDetail.WineId = wineId;
                        requestDetail.Quantity = quantity;

                        try
                        {
                            // Lưu thay đổi trong database
                            _context.SaveChanges();
                            MessageBox.Show("Request detail created successfully!");
                            LoadRequest(); // Tải lại danh sách yêu cầu
                            ClearRequestDetailFields();

                            // Nếu là nhập (Import), giảm số lượng trong kho
                            //if (!isExport)
                            //{
                            //    var warehouseWine = _context.WarehouseWines.FirstOrDefault(ww => ww.WineId == wineId);
                            //    if (warehouseWine != null)
                            //    {
                            //        warehouseWine.Quantity -= quantity;
                            //        _context.SaveChanges();
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating Request or RequestDetail: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Request or RequestDetail not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select product and enter valid quantity.");
                }
            }
            else
            {
                MessageBox.Show("Please select a Request to update.");
            }
        }




        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure a request detail is selected
            if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
            {
                if (selectedRequest.Status != "Pending")
                {
                    MessageBox.Show("This request cannot delete.");
                    return;
                }

                int requestDetailId = selectedRequest.RequestDetailId; // Get the ID of the selected request detail

                // Confirm deletion with the user
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this request detail?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Find and delete the request detail
                        var requestDetail = _requestDetailRepository.GetRequestDetailById(requestDetailId);
                        if (requestDetail != null)
                        {
                            _requestDetailRepository.DeleteRequestDetail(requestDetail);
                            MessageBox.Show("Request detail deleted successfully.");

                            // Refresh the data grid
                            LoadRequest();
                        }
                        else
                        {
                            MessageBox.Show("Request detail not found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting request detail: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a request detail to delete.");
            }
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
            {
                WineNameComboBox.Text = selectedRequest.WineName.ToString();
                QuantityTextBox.Text = selectedRequest.Quantity.ToString();
                ExpportComboBox.Text = selectedRequest.Export.ToString();
                
            }
        }



        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void ClearRequestDetailFields()
        {
            QuantityTextBox.Clear();
            WineNameComboBox.SelectedIndex = -1;
            ExpportComboBox.SelectedIndex = -1;
        }
    }
}
