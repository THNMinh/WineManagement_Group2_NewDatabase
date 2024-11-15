using BusinessObjects;
using BusinessObjects.Entities;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for ListRequestWindow.xaml
    /// </summary>
    public partial class ListRequestWindow : Window
    {

        private readonly IRequestDetailRepository _requestDetailDAO;
        private readonly IRequestRepository _requestDAO;
        private readonly IWareHouseRepository _wareHouseDAO;
        private WineManagement2Context _context;
        public ListRequestWindow()
        {
            InitializeComponent();
            _context = new WineManagement2Context();
            _requestDetailDAO = new RequestDetailDAO();
            _requestDAO = new RequestDAO();
            _wareHouseDAO = new WareHouseDAO();
            LoadRequest();
            LoadWareHouseList();
        }

        private void LoadWareHouseList()
        {
            WareHousesDataGrid.ItemsSource = _wareHouseDAO.GetWareHouseWineDetails();
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
            public int WineId { get; set; }
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
                    WineId = rd.WineId ?? 0,
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



        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
            {
                if (selectedRequest.Status != "Pending")
                {
                    MessageBox.Show("This request cannot Accept/Reject.");
                    return;
                }
                int requestDetailId = selectedRequest.RequestDetailId;
                int wineId = selectedRequest.WineId;
                bool isExport = selectedRequest.Export == "Import";
                int quantity = selectedRequest.Quantity;

                // Lấy RequestDetail từ repository
                var requestDetail = _requestDetailDAO.GetRequestDetailById(requestDetailId);
                if (requestDetail == null)
                {
                    MessageBox.Show("Request detail not found.");
                    return;
                }

                // Lấy WarehouseWine dựa trên WineId
                var warehouseWine = _context.WarehouseWines.FirstOrDefault(ww => ww.WineId == wineId);
                if (warehouseWine == null)
                {
                    MessageBox.Show("WineId does not exist in WarehouseWine. Update not allowed.");
                    return;
                }

                // Kiểm tra điều kiện Export
                if (isExport)
                {
                    // Nếu là Export, cộng Quantity vào WarehouseWine
                    warehouseWine.Quantity += quantity;

                    var request = _requestDAO.GetRequestById(requestDetailId);
                    if (request != null)
                    {
                        request.Status = "Rejected";


                        try
                        {
                            _requestDAO.UpdateRequest(request);
                            MessageBox.Show("Request detail updated successfully.");
                            LoadRequest(); // Refresh the request data grid

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
                    // Nếu là Import, kiểm tra Quantity trong WarehouseWine
                    if (warehouseWine.Quantity < quantity)
                    {
                        MessageBox.Show("Insufficient quantity in warehouse for this request.");
                        return;
                    }
                    warehouseWine.Quantity -= quantity;


                    var request = _requestDAO.GetRequestById(requestDetailId);
                    if (request != null)
                    {
                        request.Status = "Rejected";


                        try
                        {
                            _requestDAO.UpdateRequest(request);
                            MessageBox.Show("Request detail updated successfully.");
                            LoadRequest(); // Refresh the request data grid

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

                try
                {
                    // Cập nhật trong database
                    _requestDetailDAO.UpdateRequestDetail(requestDetail); // Cập nhật RequestDetail
                    _context.SaveChanges(); // Cập nhật WarehouseWine
                    MessageBox.Show("Request detail and warehouse updated successfully.");
                    LoadRequest(); // Tải lại danh sách request
                    LoadWareHouseList(); // Tải lại danh sách warehouse
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating request detail: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a request detail to update.");
            }
        }





        private void BacktoManagerHomePage_click(object sender, RoutedEventArgs e)
        {
            ManagerHomePageWindow managerWindow = new ManagerHomePageWindow();
            managerWindow.Show();
            this.Close();
        }




        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            {
                if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
                {
                    if (selectedRequest.Status != "Pending")
                    {
                        MessageBox.Show("This request cannot Accept/Reject.");
                        return;
                    }
                    int requestDetailId = selectedRequest.RequestDetailId; // Correct ID for RequestDetail


                    var requestDetail = _requestDAO.GetRequestById(requestDetailId);
                    if (requestDetail != null)
                    {
                        requestDetail.Status = "Rejected";


                        try
                        {
                            _requestDAO.UpdateRequest(requestDetail);
                            MessageBox.Show("Request detail updated successfully.");
                            LoadRequest(); // Refresh the request data grid

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
                    MessageBox.Show("Please select a request detail to update.");
                }

            }

        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
            {
                RequestIDBox.Text = selectedRequest.RequestDetailId.ToString();




            }


        }

        private void btn_Search(object sender, RoutedEventArgs e)
        {
            string searchKeyword = txtSearch.Text.Trim().ToLower();

            // Kiểm tra tab nào đang được chọn để tìm kiếm
            if (WareHouseTab.IsSelected) // Kiểm tra xem tab Wines có đang được chọn hay không
            {
                // Tìm kiếm theo Wine Name
                var result = _context.WarehouseWines.Where(w => w.Wine.Name.ToLower().Contains(searchKeyword)).Select(wine => new
                {
                    WineName = wine.Wine.Name,
                    wine.WareHouse.Address,
                    wine.WareHouse.ContactPerson,
                    wine.WareHouse.PhoneNumber,
                    wine.WareHouse.Location,
                    wine.Quantity,
                    wine.Description
                }).ToList();


                //var result = _context.Wines
                //    .Where(w => w.Name.ToLower().Contains(searchKeyword))
                //    .Select(wine => new
                //    {
                //        wine.Name,
                //        wine.VintageYear,
                //        wine.AlcoholContent,
                //        wine.Price,
                //        CategoryName = wine.Category != null ? wine.Category.CategoryName : "N/A"
                //    })
                //    .ToList();

                WareHousesDataGrid.ItemsSource = result;
            }
            else if (RequestTab.IsSelected) // Kiểm tra xem tab Requests có đang được chọn hay không
            {
                // Tìm kiếm theo Wine Name trong danh sách Requests
                var result = GetRequestData()
                .Where(r => r.WineName.ToLower().Contains(searchKeyword))
                .ToList();

                RequestsDataGrid.ItemsSource = result;
            }

        }
    }
}
