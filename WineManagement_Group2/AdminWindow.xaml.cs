using BusinessObjects;
using BusinessObjects.Entities;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WineWarehouseManagement
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly IAccountRepository _accountDAO;
        private readonly IWareHouseRepository _wareHouseDAO;

        public AdminWindow()
        {
            InitializeComponent();
            _accountDAO = new AccountDAO();
            _wareHouseDAO = new WareHouseDAO();
            LoadStaffList();
            LoadManagerList();
            LoadWareHouseList();
        }

        private void LoadStaffList()
        {
            // Loads only accounts with "Staff" role into the DataGrid
            StaffDataGrid.ItemsSource = _accountDAO.GetAccountsByRole("Staff");
        }



        private bool IsValidGmail(string email)
        {
            var gmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@gmail\.com$");
            return gmailRegex.IsMatch(email);
        }

        private bool IsEmailUnique(string email)
        {
            return !_accountDAO.GetAllAccounts().Any(account => account.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }


        private void CreateStaffButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StaffNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(StaffEmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(StaffPasswordBox.Password))
            {
                MessageBox.Show("Please fill in all required fields (Name, Email, and Password).", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidGmail(StaffEmailTextBox.Text))
            {
                MessageBox.Show("Please enter a valid Gmail address (e.g., example@gmail.com).", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsEmailUnique(StaffEmailTextBox.Text))
            {
                MessageBox.Show("This email is already in use. Please use a different email address.", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Account newStaff = new Account
            {
                Username = StaffNameTextBox.Text,
                Email = StaffEmailTextBox.Text,
                PasswordHash = StaffPasswordBox.Password,
                Role = "Staff"
            };


            _accountDAO.AddAccount(newStaff);
            LoadStaffList();
            ClearStaffFields();
        }

        private void UpdateStaffButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StaffNameTextBox.Text) ||
            string.IsNullOrWhiteSpace(StaffEmailTextBox.Text) ||
            string.IsNullOrWhiteSpace(StaffPasswordBox.Password))
            {
                MessageBox.Show("Please fill in all required fields (Name, Email, and Password).", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedStaff = StaffDataGrid.SelectedItem as Account;

            if (selectedStaff != null)
            {
                if (!IsValidGmail(StaffEmailTextBox.Text))
                {
                    MessageBox.Show("Please enter a valid Gmail address (e.g., example@gmail.com).", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!IsEmailUnique(StaffEmailTextBox.Text) && !selectedStaff.Email.Equals(StaffEmailTextBox.Text, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("This email is already in use. Please use a different email address.", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    selectedStaff.Username = StaffNameTextBox.Text;
                    selectedStaff.Email = StaffEmailTextBox.Text;
                    selectedStaff.PasswordHash = StaffPasswordBox.Password;
                    selectedStaff.Role = StaffRole.Text;

                    _accountDAO.UpdateAccount(selectedStaff);
                    LoadStaffList();
                    ClearStaffFields();
                    MessageBox.Show("Staff updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating staff: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a staff member to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void DeleteStaffButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this staff?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (StaffDataGrid.SelectedItem is Account selectedStaff)
                {
                    _accountDAO.DeleteAccount(selectedStaff.AccountId);
                    LoadStaffList();
                    ClearStaffFields();
                }
                else
                {
                    MessageBox.Show("Please select a valid staff member to delete.");
                }
            }
        }


        private void ClearStaffFields()
        {
            // Clears the text fields after creating, updating, or deleting
            StaffNameTextBox.Text = string.Empty;
            StaffPasswordBox.Password = string.Empty;
            StaffEmailTextBox.Text = string.Empty;
        }



        ///////////////////////////////////// Manager
        // Load manager list into DataGrid
        private void LoadManagerList()
        {
            ManagerDataGrid.ItemsSource = _accountDAO.GetAccountsByRole("Manager");
        }

        // Create a new manager
        private void CreateManagerButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ManagerNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ManagerEmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(ManagerPasswordTextBox.Password))
            {
                MessageBox.Show("Please fill in all required fields (Name, Email, and Password).", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!IsValidGmail(ManagerEmailTextBox.Text))
            {
                MessageBox.Show("Please enter a valid Gmail address (e.g., example@gmail.com).", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsEmailUnique(ManagerEmailTextBox.Text))
            {
                MessageBox.Show("This email is already in use. Please use a different email address.", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Account newManager = new Account
            {
                Username = ManagerNameTextBox.Text,
                Email = ManagerEmailTextBox.Text,
                PasswordHash = ManagerPasswordTextBox.Password,
                Role = "Manager"
            };

            _accountDAO.AddAccount(newManager);
            LoadManagerList();
            ClearManagerFields();
        }

        // Load selected manager information
        private void ReadManagerButton_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerDataGrid.SelectedItem is Account selectedManager)
            {
                ManagerNameTextBox.Text = selectedManager.Username;
                ManagerEmailTextBox.Text = selectedManager.Email;
                ManagerPasswordTextBox.Password = selectedManager.PasswordHash;
            }
            else
            {
                MessageBox.Show("Please select a manager to view.");
            }
        }

        // Update selected manager
        private void UpdateManagerButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ManagerNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(ManagerEmailTextBox.Text) ||
                    string.IsNullOrWhiteSpace(ManagerPasswordTextBox.Password))
            {
                MessageBox.Show("Please fill in all required fields (Name, Email, and Password).", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ManagerDataGrid.SelectedItem is Account selectedManager)
            {
                if (!IsValidGmail(ManagerEmailTextBox.Text))
                {
                    MessageBox.Show("Please enter a valid Gmail address (e.g., example@gmail.com).", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!IsEmailUnique(ManagerEmailTextBox.Text) && !selectedManager.Email.Equals(ManagerEmailTextBox.Text, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("This email is already in use. Please use a different email address.", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var managerToUpdate = _accountDAO.GetAccountById(selectedManager.AccountId);
                if (managerToUpdate != null)
                {
                    managerToUpdate.Username = ManagerNameTextBox.Text;
                    managerToUpdate.Email = ManagerEmailTextBox.Text;
                    managerToUpdate.PasswordHash = ManagerPasswordTextBox.Password;
                    managerToUpdate.Role = ManagerRole.Text.ToString();

                    _accountDAO.UpdateAccount(managerToUpdate);
                    LoadManagerList();
                    ClearManagerFields();
                }
            }
            else
            {
                MessageBox.Show("Please select a valid manager to update and ensure all fields are filled.");
            }
        }

        // Delete selected manager
        private void DeleteManagerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this Account?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (ManagerDataGrid.SelectedItem is Account selectedManager)
                {
                    _accountDAO.DeleteAccount(selectedManager.AccountId);
                    LoadManagerList();
                    ClearManagerFields();
                }
                else
                {
                    MessageBox.Show("Please select a manager to delete.");
                }
            }
        }

        // Clear manager input fields
        private void ClearManagerFields()
        {
            ManagerNameTextBox.Clear();
            ManagerEmailTextBox.Clear();
            ManagerPasswordTextBox.Clear();
        }

        // WareHouse
        private void ClearWareHouseFields()
        {
            WareHouseAddressTextBox.Clear();
            LocationTextBox.Clear();
            ContactPersonTextBox.Clear();
            PhoneNumberTextBox.Clear();
        }

        private void LoadWareHouseList()
        {
            WareHouseDataGrid.ItemsSource = _wareHouseDAO.GetAllWareHouses();
        }
        private bool IsLocationUnique(string Location)
        {
            return !_wareHouseDAO.GetAllWareHouses().Any(wareHouse => wareHouse.Location.Equals(Location, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsValidPhoneNumber(string PhoneNumber)
        {
            var PhoneNumberRegex = new Regex(@"^\d{10}$");
            return PhoneNumberRegex.IsMatch(PhoneNumber);
        }

        // Create a new WareHouse
        private void CreateWareHouseButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(WareHouseAddressTextBox.Text) ||
                string.IsNullOrWhiteSpace(LocationTextBox.Text) ||
                string.IsNullOrWhiteSpace(ContactPersonTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsLocationUnique(LocationTextBox.Text))
            {
                MessageBox.Show("This location is already in have. Please use a different location.", "Duplicate Location", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidPhoneNumber(PhoneNumberTextBox.Text))
            {
                MessageBox.Show("Please enter a valid Phone Number (e.g., 0525462579).", "Invalid Phone", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            WareHouse newWareHouse = new WareHouse
            {
                Address = WareHouseAddressTextBox.Text,
                Status = "True",
                ContactPerson = ContactPersonTextBox.Text,
                PhoneNumber = PhoneNumberTextBox.Text,
                Location = LocationTextBox.Text
            };

            _wareHouseDAO.AddWareHouse(newWareHouse);
            LoadWareHouseList();
            ClearWareHouseFields();
        }

        private void UpdateWareHouseButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(WareHouseAddressTextBox.Text) ||
                string.IsNullOrWhiteSpace(LocationTextBox.Text) ||
                string.IsNullOrWhiteSpace(ContactPersonTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (WareHouseDataGrid.SelectedItem is WareHouse selectedWareHouse)
            {
                if (!IsLocationUnique(LocationTextBox.Text))
                {
                    MessageBox.Show("This location is already in have. Please use a different location.", "Duplicate Location", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!IsValidPhoneNumber(PhoneNumberTextBox.Text))
                {
                    MessageBox.Show("Please enter a valid Phone Number (e.g., 0525462579).", "Invalid Phone", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var WareHouseToUpdate = _wareHouseDAO.GetWareHouseById(selectedWareHouse.WareHouseId);
                if (WareHouseToUpdate != null)
                {
                    WareHouseToUpdate.Address = WareHouseAddressTextBox.Text;
                    WareHouseToUpdate.ContactPerson = ContactPersonTextBox.Text;
                    WareHouseToUpdate.PhoneNumber = PhoneNumberTextBox.Text;
                    WareHouseToUpdate.Location = LocationTextBox.Text;

                    _wareHouseDAO.UpdateWareHouse(WareHouseToUpdate);
                    LoadWareHouseList();
                    ClearWareHouseFields();
                }
            }
            else
            {
                MessageBox.Show("Please select a valid WareHouse to update and ensure all fields are filled.");
            }
        }

        private void DeleteWareHouseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this WareHouse?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (WareHouseDataGrid.SelectedItem is WareHouse selectedWareHouse)
                {
                    var WareHouseToUpdate = _wareHouseDAO.GetWareHouseById(selectedWareHouse.WareHouseId);
                    if (WareHouseToUpdate != null)
                    {
                        WareHouseToUpdate.Status = "False";

                        _wareHouseDAO.UpdateWareHouse(WareHouseToUpdate);
                        LoadWareHouseList();
                        ClearWareHouseFields();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a manager to delete.");
                }
            }
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem is Account selectedStaff)
            {

                StaffNameTextBox.Text = selectedStaff.Username.ToString();
                StaffEmailTextBox.Text = selectedStaff.Email;
                StaffPasswordBox.Password = selectedStaff.PasswordHash; // Assuming you want to display this as well
                StaffRole.Text = selectedStaff.Role.ToString(); // Convert boolean to string


            }


        }

        private void dgData_SelectionChangedForMANAGER(object sender, SelectionChangedEventArgs e)
        {
            if (ManagerDataGrid.SelectedItem is Account selectedStaff)
            {

                ManagerNameTextBox.Text = selectedStaff.Username.ToString();
                ManagerEmailTextBox.Text = selectedStaff.Email;
                ManagerPasswordTextBox.Password = selectedStaff.PasswordHash; // Assuming you want to display this as well
                ManagerRole.Text = selectedStaff.Role.ToString(); // Convert boolean to string


            }


        }

        private void dgData_SelectionChangedForWareHouse(object sender, SelectionChangedEventArgs e)
        {
            if (WareHouseDataGrid.SelectedItem is WareHouse selectedWareHouse)
            {
                WareHouseAddressTextBox.Text = selectedWareHouse.Address.ToString();
                LocationTextBox.Text = selectedWareHouse.Location.ToString();
                ContactPersonTextBox.Text = selectedWareHouse.ContactPerson.ToString();
                PhoneNumberTextBox.Text = selectedWareHouse.PhoneNumber.ToString();
            }
        }

        private void btn_Search(object sender, RoutedEventArgs e)
        {
            string searchKeyword = string.Empty;

            // Kiểm tra xem tab nào đang được chọn và lấy giá trị tìm kiếm tương ứng
            if (StaffTab.IsSelected) // Nếu đang ở tab Staff
            {
                searchKeyword = txtSearch.Text.Trim().ToLower();  // Tìm kiếm cho Staff
            }
            else if (ManagerTab.IsSelected) // Nếu đang ở tab Manager
            {
                searchKeyword = txtSearchManager.Text.Trim().ToLower();  // Tìm kiếm cho Manager
            }
            else if (WareHouseTab.IsSelected)
            {
                searchKeyword = txtSearchWareHouse.Text.Trim().ToLower();
            }

            // Nếu không có từ khóa tìm kiếm, tải lại danh sách tương ứng
            if (string.IsNullOrWhiteSpace(searchKeyword))
            {
                if (StaffTab.IsSelected)
                {
                    LoadStaffList();
                }
                else if (ManagerTab.IsSelected)
                {
                    LoadManagerList();
                }
                else if (WareHouseTab.IsSelected)
                {
                    LoadWareHouseList();
                }
                return;
            }

            // Kiểm tra xem tab nào đang được chọn
            if (StaffTab.IsSelected) // Nếu đang ở tab Staff
            {
                var result = _accountDAO.GetAccountsByRole("Staff")
                                        .Where(account => account.Username.ToLower().Contains(searchKeyword))
                                        .ToList();

                if (result.Any())
                {
                    StaffDataGrid.ItemsSource = result; // Hiển thị kết quả vào DataGrid
                }
                else
                {
                    MessageBox.Show("No staff found with the specified name.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    //StaffDataGrid.ItemsSource = null; // Nếu không tìm thấy, không hiển thị kết quả
                }
            }
            else if (ManagerTab.IsSelected) // Nếu đang ở tab Manager
            {
                var result = _accountDAO.GetAccountsByRole("Manager")
                                        .Where(account => account.Username.ToLower().Contains(searchKeyword))
                                        .ToList();

                if (result.Any())
                {
                    ManagerDataGrid.ItemsSource = result; // Hiển thị kết quả vào DataGrid
                }
                else
                {
                    MessageBox.Show("No managers found with the specified name.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    //ManagerDataGrid.ItemsSource = null; // Nếu không tìm thấy, không hiển thị kết quả
                }
            }
            else if (WareHouseTab.IsSelected) // Nếu đang ở tab Manager
            {
                var result = _wareHouseDAO.GetAllWareHouses()
                                        .Where(WareHouse => WareHouse.Address.ToLower().Contains(searchKeyword))
                                        .ToList();

                if (result.Any())
                {
                    WareHouseDataGrid.ItemsSource = result; // Hiển thị kết quả vào DataGrid
                }
                else
                {
                    MessageBox.Show("No WareHouses found with the specified address.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                    //ManagerDataGrid.ItemsSource = null; // Nếu không tìm thấy, không hiển thị kết quả
                }
            }
        }
    }
}
