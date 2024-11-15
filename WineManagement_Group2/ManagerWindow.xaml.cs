using BusinessObjects;
using BusinessObjects.Entities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private readonly IAccountRepository _accountDAO;

        public ManagerWindow()
        {
            InitializeComponent();
            _accountDAO = new AccountDAO();
            LoadStaffList();
        }

        private void LoadStaffList()
        {
            // Loads only accounts with "Staff" role into the DataGrid
            StaffDataGrid.ItemsSource = _accountDAO.GetAccountsByRole("Staff");
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
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

        private bool IsValidGmail(string email)
        {
            var gmailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@gmail\.com$");
            return gmailRegex.IsMatch(email);
        }

        private bool IsEmailUnique(string email)
        {
            return !_accountDAO.GetAllAccounts().Any(account => account.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        private void ClearStaffFields()
        {
            // Clears the text fields after creating, updating, or deleting
            StaffNameTextBox.Text = string.Empty;
            StaffPasswordBox.Password = string.Empty;
            StaffEmailTextBox.Text = string.Empty;
            StaffRoleComboBox.Text = string.Empty;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
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


        private void ReadButton_Click(object sender, RoutedEventArgs e)
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


        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem is Account selectedStaff)
            {

                StaffNameTextBox.Text = selectedStaff.Username.ToString();
                StaffEmailTextBox.Text = selectedStaff.Email;
                StaffPasswordBox.Password = selectedStaff.PasswordHash; // Assuming you want to display this as well
                StaffRoleComboBox.Text = selectedStaff.Role.ToString(); // Convert boolean to string


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
