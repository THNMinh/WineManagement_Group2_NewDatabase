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
    /// Interaction logic for ListRequestWindow.xaml
    /// </summary>
    public partial class ListRequestWindow : Window
    {

        private readonly IRequestDetailRepository _requestDetailDAO;
        private readonly IRequestRepository _requestDAO;
        private WineManagement2Context _context;
        public ListRequestWindow()
        {
            InitializeComponent();
            _context = new WineManagement2Context();
            _requestDetailDAO = new RequestDetailDAO();
            _requestDAO = new RequestDAO();
            LoadRequest();
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


        public class RequestData()
        {
            public int RequestId { get; set; }
            public int RequestDetailId { get; set; }
            public int AccountId { get; set; }
            public string WineName { get; set; }
            public int Quantity { get; set; }
            public string Status { get; set; }
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


        //public void LoadRequest()
        //{
        //    RequestsDataGrid.ItemsSource = _requestDetailDAO.GetAllRequestDetails();
        //    RequestsDataGrid.ItemsSource = _requestDAO.GetAllRequests();
        //}

        //private void ReadButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (RequestsDataGrid.SelectedItem is RequestData selectedStaff)
        //    {
        //        // Populate text fields with the selected staff information
        //        RequestIDBox.Text = selectedStaff.RequestId.ToString();
                
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select a staff member to read.");
        //    }
        //}

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            {
                if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
                {
                    
                        int requestDetailId = selectedRequest.RequestDetailId; // Correct ID for RequestDetail


                        var requestDetail = _requestDAO.GetRequestById(requestDetailId);
                        if (requestDetail != null)
                        {
                            requestDetail.Status = "Accepted";
                            

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

        

        private void BacktoManagerHomePage_click(object sender, RoutedEventArgs e)
        {
            ManagerHomePageWindow managerWindow = new ManagerHomePageWindow();
            managerWindow.Show();
            this.Close();
        }


        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
            {
                RequestIDBox.Text = selectedRequest.RequestDetailId.ToString();

             


            }


        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            {
                if (RequestsDataGrid.SelectedItem is RequestData selectedRequest)
                {

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
    }
}
