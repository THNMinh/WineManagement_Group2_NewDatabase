using BusinessObjects;
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

        public AdminWindow()
        {
            InitializeComponent();
            _accountDAO = new AccountDAO();
            LoadStaffList();
            LoadManagerList();
        }

        private void LoadStaffList()
        {
            // Loads only accounts with "Staff" role into the DataGrid
            StaffDataGrid.ItemsSource = _accountDAO.GetAccountsByRole("Staff");
        }

        private void ReadStaffButton_Click(object sender, RoutedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem is Account selectedStaff)
            {
                // Populate text fields with the selected staff information
                StaffNameTextBox.Text = selectedStaff.Username;
                StaffPasswordBox.Password = selectedStaff.PasswordHash;
                StaffEmailTextBox.Text = selectedStaff.Email;

                foreach (ComboBoxItem item in StaffRoleComboBox.Items)
                {
                    if (item.Content.ToString() == selectedStaff.Role)
                    {
                        StaffRoleComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a staff member to read.");
            }
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
                    selectedStaff.Role = (StaffRoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

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


        private void ClearStaffFields()
        {
            // Clears the text fields after creating, updating, or deleting
            StaffNameTextBox.Text = string.Empty;
            StaffPasswordBox.Password = string.Empty;
            StaffEmailTextBox.Text = string.Empty;
            StaffRoleComboBox.Text = string.Empty;
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
                string.IsNullOrWhiteSpace(ManagerPasswordTextBox.Text))
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
                PasswordHash = ManagerPasswordTextBox.Text,
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
                ManagerPasswordTextBox.Text = selectedManager.PasswordHash;

                // Set the ComboBox to the selected role
                foreach (ComboBoxItem item in ManagerRoleComboBox.Items)
                {
                    if (item.Content.ToString() == selectedManager.Role)
                    {
                        ManagerRoleComboBox.SelectedItem = item;
                        break;
                    }
                }
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
                    string.IsNullOrWhiteSpace(ManagerPasswordTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields (Name, Email, and Password).", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ManagerDataGrid.SelectedItem is Account selectedManager && ManagerRoleComboBox.SelectedItem is ComboBoxItem selectedRoleItem)
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
                    managerToUpdate.PasswordHash = ManagerPasswordTextBox.Text;
                    managerToUpdate.Role = selectedRoleItem.Content.ToString();

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

        // Clear manager input fields
        private void ClearManagerFields()
        {
            ManagerNameTextBox.Clear();
            ManagerEmailTextBox.Clear();
            ManagerPasswordTextBox.Clear();
            ManagerRoleComboBox.SelectedIndex = -1;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
