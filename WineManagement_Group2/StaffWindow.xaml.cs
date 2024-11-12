using BusinessObjects;
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
    /// Interaction logic for StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {
        private WineManagement2Context _context;
        int accountId;
        IRequestRepository _requestRepository;
        public StaffWindow(int AccountId)
        {
            InitializeComponent();
            _context = new WineManagement2Context(); // Initialize the context
            LoadWines(); // Load wine data into the DataGrid
            accountId = AccountId;
            _requestRepository = new RequestDAO();
            LoadRequest(); // Load y?u c?u t? co s? d? li?u v?i c?c thu?c t?nh trong DataGrid
        }

        public class RequestData()
        {
            public int RequestId { get; set; }
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
                WineDataGrid.ItemsSource = wineData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading wine data: " + ex.Message);
            }
        }

        private void CreateRequest(object sender, RoutedEventArgs e)
        {
            try
            {
                // Tạo yêu cầu mới
                //var request = new Request
                //{
                //    AccountId = accountId/* Đặt AccountId nếu có */,
                //    Status = "Pending" // Trạng thái mặc định
                //};

                //// Thêm yêu cầu vào cơ sở dữ liệu
                //_context.Requests.Add(request);
                //_context.SaveChanges();



                new RequestDetailWindow(accountId).Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating request: " + ex.Message);
            }
        }

        private void btn_Search(object sender, RoutedEventArgs e)
        {
            string searchKeyword = txtSearch.Text.Trim().ToLower();

            // Kiểm tra tab nào đang được chọn để tìm kiếm
            if (WineTab.IsSelected) // Kiểm tra xem tab Wines có đang được chọn hay không
            {
                // Tìm kiếm theo Wine Name
                var result = _context.Wines
                    .Where(w => w.Name.ToLower().Contains(searchKeyword))
                    .Select(wine => new
                    {
                        wine.Name,
                        wine.VintageYear,
                        wine.AlcoholContent,
                        wine.Price,
                        CategoryName = wine.Category != null ? wine.Category.CategoryName : "N/A"
                    })
                    .ToList();

                WineDataGrid.ItemsSource = result;
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

        protected override void OnClosed(EventArgs e)
        {
            // Đóng context khi cửa sổ bị đóng
            _context.Dispose();
            base.OnClosed(e);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

    }
}
