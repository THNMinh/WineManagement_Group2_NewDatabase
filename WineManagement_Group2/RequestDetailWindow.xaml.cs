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
            var list = _wineRepository.GetAllWines();
            WineNameComboBox.ItemsSource = list;
            WineNameComboBox.DisplayMemberPath = "Name";
            WineNameComboBox.SelectedValuePath = "WineId";
        }

        public class RequestData()
        {
            public int RequestId { get; set; }
            public int RequestDetailId { get; set; }
            public int AccountId { get; set; }
            public string WineName { get; set; }
            public int Quantity { get; set; }
            public string Status { get; set; }
        }
        private void LoadRequest()
        {
            try
            {
                RequestsDataGrid.ItemsSource = GetRequestData();  // Bind data to RequestsDataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading request data: " + ex.Message);
            }
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
                    Status = rd.Request != null ? rd.Request.Status ?? "Unknown" : "Unknown"
                })
                .ToList();

            return requestData;
        }


        private void LoadWines()
        {
            try
            {
                // Lấy dữ liệu rượu vang từ cơ sở dữ liệu và ánh xạ với các thuộc tính trong DataGrid
                var wineData = _context.Wines.Select(wine => new
                {
                    wine.Name,
                    wine.VintageYear,
                    wine.AlcoholContent,
                    wine.Price,
                    CategoryName = wine.Category != null ? wine.Category.CategoryName : "N/A"
                }).ToList();

                // Gán dữ liệu vào WineDataGrid
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

                // First, create a new request and capture the RequestId
                int requestId = CreateRequest(); // Update CreateRequest to return the new RequestId

                if (requestId > 0) // Ensure a valid RequestId was returned
                {
                    var requestDetail = new RequestDetail
                    {
                        RequestId = requestId, // Use the newly created RequestId
                        WineId = wineId,
                        Quantity = quantity
                    };

                    try
                    {
                        _requestDetailRepository.AddRequestDetail(requestDetail);
                        MessageBox.Show("Request detail created successfully!");
                        LoadRequest(); // Refresh the request data grid
                        ClearRequestDetailFields();
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

        private int CreateRequest()
        {
            try
            {
                // Create a new request
                var request = new BusinessObjects.Entities.Request
                {
                    AccountId = accountId, // Set AccountId
                    Status = "Pending" // Default status
                };

                // Add the request to the database and save changes
                _context.Requests.Add(request);
                _context.SaveChanges();

                // Return the newly created RequestId
                return request.RequestId; // Ensure this property exists in your Request entity
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating request: " + ex.Message);
                return 0; // Return 0 if creation fails
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
            {
                if (WineNameComboBox.SelectedValue != null && !string.IsNullOrWhiteSpace(QuantityTextBox.Text))
                {
                    int requestDetailId = selectedRequest.RequestDetailId; // Correct ID for RequestDetail
                    if (!int.TryParse(WineNameComboBox.SelectedValue.ToString(), out int wineId))
                    {
                        MessageBox.Show("Invalid wine selection.");
                        return;
                    }

                    if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
                    {
                        MessageBox.Show("Quantity must be a positive number.");
                        return;
                    }

                    var requestDetail = _requestDetailRepository.GetRequestDetailById(requestDetailId);
                    if (requestDetail != null)
                    {
                        requestDetail.WineId = wineId;
                        requestDetail.Quantity = quantity;

                        try
                        {
                            _requestDetailRepository.UpdateRequestDetail(requestDetail);
                            MessageBox.Show("Request detail updated successfully.");
                            LoadRequest(); // Refresh the request data grid
                            ClearRequestDetailFields();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error updating request detail: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Request detail not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Wine Name and Quantity must not be null!");
                }
            }
            else
            {
                MessageBox.Show("Please select a request detail to update.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure a request detail is selected
            if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
            {
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
        }
    }
}
