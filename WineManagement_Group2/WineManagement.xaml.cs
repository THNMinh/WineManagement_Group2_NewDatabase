﻿using BusinessObjects;
using BusinessObjects.Entities;
using DataAccessLayer;
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
    /// Interaction logic for WineManagement.xaml
    /// </summary>
    public partial class WineManagement : Window
    {

        private readonly IWineRepository _wineDAO;
        private readonly ISupplierRepository _supplierDAO;
        private readonly ICategoryRepository _categoryDAO;
        
        public WineManagement()
        {
            InitializeComponent();
            _wineDAO = new WineDAO();
            _supplierDAO = new SupplierDAO();
            _categoryDAO = new CategoryDAO();
            LoadWinefList();
            LoadSupplierList();
            LoadCategoryList();
            LoadComboList();
        }

        private void LoadWinefList()
        {
            // Loads only accounts with "Staff" role into the DataGrid
            StaffDataGrid.ItemsSource = _wineDAO.GetAllWines();
        }

        public void LoadComboList()
        {
            
            var listSupplier = _supplierDAO.GetAllSuppliers();
            SupplierComboBox.ItemsSource = listSupplier;
            SupplierComboBox.DisplayMemberPath = "Name";
            SupplierComboBox.SelectedValuePath = "SupplierId";


            var list = _categoryDAO.GetAllCategories();
            CategoryComboBox.ItemsSource = list;
            CategoryComboBox.DisplayMemberPath = "CategoryName";
            CategoryComboBox.SelectedValuePath = "CategoryId";
        }

        private void ReadWineButton_Click(object sender, RoutedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem is Wine selectedWine)
            {
                WineNameTextBox.Text = selectedWine.Name;
                VintageYearTextBox.Text = selectedWine.VintageYear.ToString();
                PriceBox.Text = selectedWine.Price.ToString();
                AlcoholContentBox.Text = selectedWine.AlcoholContent.ToString();

                // Fetch category name using CategoryId
                Category category = _categoryDAO.GetCategoryById((int)selectedWine.CategoryId);
                Supplier supplier = _supplierDAO.GetSupplierById((int)selectedWine.SupplierId);
                if (category != null)
                {
                    CategoryComboBox.Text = category.CategoryName; // Set the displayed text
                   
                }
                if (supplier != null)
                {
                    // Set the displayed text
                    SupplierComboBox.Text = supplier.Name;
                }
                else
                {
                    // Handle potential case where category is not found
                    CategoryComboBox.Text = "Category Not Found";
                    SupplierComboBox.Text = "Supplier Not Found";
                }
            }
            else
            {
                MessageBox.Show("Please select a wine to read.");
            }
        }
        //private void ReadWineButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (StaffDataGrid.SelectedItem is Wine selectedWine)
        //    {
        //        WineNameTextBox.Text = selectedWine.Name;
        //        VintageYearTextBox.Text = selectedWine.VintageYear.ToString();
        //        PriceBox.Text = selectedWine.Price.ToString();
        //        AlcoholContentBox.Text = selectedWine.AlcoholContent.ToString();

        //        ReadCategoryName((int)selectedWine.CategoryId); // Call separate function
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select a wine to read.");
        //    }
        //}

        //private void ReadCategoryName(int categoryId)
        //{
        //    Category category = _categoryDAO.GetCategoryById(categoryId);
        //    if (category != null)
        //    {
        //        CategoryComboBox.Text = category.CategoryName;
        //    }
        //    else
        //    {
        //        // Handle potential case where category is not found
        //        CategoryComboBox.Text = "Category Not Found";
        //    }
        //}

        private void CreateWineButton_Click(object sender, RoutedEventArgs e)
        {

            // Validate input fields
            if (string.IsNullOrWhiteSpace(WineNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(VintageYearTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceBox.Text) ||
                string.IsNullOrWhiteSpace(AlcoholContentBox.Text) ||
                string.IsNullOrWhiteSpace(CategoryComboBox.Text) ||
                string.IsNullOrWhiteSpace(SupplierComboBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate integers (VintageYear, Price, AlcoholContent)
            if (!int.TryParse(VintageYearTextBox.Text, out int vintageYear) ||
                !decimal.TryParse(PriceBox.Text, out decimal price) ||
                !decimal.TryParse(AlcoholContentBox.Text, out decimal alcoholContent))
            {
                MessageBox.Show("Vintage Year, Price, and Alcohol Content must be numbers.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate VintageYear <= 2024
            if (vintageYear > 2024)
            {
                MessageBox.Show("Vintage Year cannot be greater than the current year (2024).", "Invalid Vintage Year", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create a new Wine object with the entered data
            Wine newWine = new Wine
            {
                Name = WineNameTextBox.Text,
                VintageYear = vintageYear,
                Price = price,
                AlcoholContent = alcoholContent,
                CategoryId = (int)CategoryComboBox.SelectedValue,
                SupplierId = (int)SupplierComboBox.SelectedValue
            };

            // Insert the new wine into the database
            _wineDAO.AddWine(newWine);

            // Refresh the wine list
            LoadWinefList();

            // Clear the input fields
            ClearInputFields();

        }

        private void ClearInputFields()
        {
            // Clears the text fields after creating, updating, or deleting
            WineNameTextBox.Text = string.Empty;
            VintageYearTextBox.Text = string.Empty;
            PriceBox.Text = string.Empty;
            AlcoholContentBox.Text = string.Empty;
            CategoryComboBox.Text = string.Empty;
            SupplierComboBox.Text = string.Empty;

        }

        private void UpdateWineButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input fields
            if (string.IsNullOrWhiteSpace(WineNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(VintageYearTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceBox.Text) ||
                string.IsNullOrWhiteSpace(AlcoholContentBox.Text) ||
                string.IsNullOrWhiteSpace(CategoryComboBox.Text) ||
                string.IsNullOrWhiteSpace(SupplierComboBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate integers (VintageYear, Price, AlcoholContent)
            if (!int.TryParse(VintageYearTextBox.Text, out int vintageYear) ||
                !decimal.TryParse(PriceBox.Text, out decimal price) ||
                !decimal.TryParse(AlcoholContentBox.Text, out decimal alcoholContent))
            {
                MessageBox.Show("Vintage Year, Price, and Alcohol Content must be numbers.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate VintageYear <= 2024
            if (vintageYear > 2024)
            {
                MessageBox.Show("Vintage Year cannot be greater than the current year (2024).", "Invalid Vintage Year", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (StaffDataGrid.SelectedItem is Wine selectedWine)
            {
                // Update the wine's properties with the new values
                selectedWine.Name = WineNameTextBox.Text;
                selectedWine.VintageYear = int.Parse(VintageYearTextBox.Text);
                selectedWine.Price = decimal.Parse(PriceBox.Text);
                selectedWine.AlcoholContent = decimal.Parse(AlcoholContentBox.Text);
                selectedWine.CategoryId = (int)CategoryComboBox.SelectedValue;
                selectedWine.SupplierId = (int)SupplierComboBox.SelectedValue;

                // Update the wine in the database
                _wineDAO.UpdateWine(selectedWine);

                // Refresh the wine list
                LoadWinefList();
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Please select a wine to update.");
            }

        }

        private void DeleteWineButton_Click(object sender, RoutedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem is Wine selectedWine)
            {
                // Confirm deletion with the user
                if (MessageBox.Show("Are you sure you want to delete this wine?", "Confirm Deletion", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // Delete the wine from the database
                    _wineDAO.DeleteWine(selectedWine.WineId);

                    // Refresh the wine list
                    LoadWinefList();

                    // Clear the input fields
                    ClearInputFields();
                }
            }
            else
            {
                MessageBox.Show("Please select a wine to delete.");
            }

        }

        private void LoadSupplierList()
        {
            // Loads only accounts with "Staff" role into the DataGrid
            ManagerDataGrid.ItemsSource = _supplierDAO.GetAllSuppliers();
        }

       

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();

        }

        private void BacktoManagerHomePage_click(object sender, RoutedEventArgs e)
        {
            ManagerHomePageWindow managerWindow = new ManagerHomePageWindow();
            managerWindow.Show();
            this.Close();
        }

        private void ReadSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerDataGrid.SelectedItem is Supplier selectedSupplier)
            {
                SupplierNameTextBox.Text = selectedSupplier.Name;
                ContactPersonTextBox.Text = selectedSupplier.ContactPerson;
                PhoneTextBox.Text = selectedSupplier.Phone;
                EmailTextBox.Text = selectedSupplier.Email;
                AddressTextBox.Text = selectedSupplier.Address;
            }
            else
            {
                MessageBox.Show("Please select a supplier to read.");
            }

        }

        private bool IsEmailUnique(string email)
        {
            return !_supplierDAO.GetAllSuppliers().Any(a => a.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsPhoneUnique(string email)
        {
            return !_supplierDAO.GetAllSuppliers().Any(a => a.Phone.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            return phoneNumber.All(char.IsDigit);
        }

        private void CreateSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SupplierNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ContactPersonTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddressTextBox.Text) )
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (!IsEmailUnique(EmailTextBox.Text))
            {
                MessageBox.Show("This email is already in use. Please use a different email address.", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Validate phone number
            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text) || !IsPhoneNumberValid(PhoneTextBox.Text) || PhoneTextBox.Text.Length != 10)
            {
                MessageBox.Show("Phone number must be filled, only contain digits, and be 10 digits long.", "Invalid Phone Number", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

            if (!IsPhoneUnique(PhoneTextBox.Text))
            {
                MessageBox.Show("This phone number is already in use. Please use a different please.", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            Supplier newSupplier = new Supplier
            {
                Name = SupplierNameTextBox.Text,
                ContactPerson = ContactPersonTextBox.Text,
                Phone = PhoneTextBox.Text,
                Email = EmailTextBox.Text,
                Address = AddressTextBox.Text
            };

            // Insert the new supplier into the database
            _supplierDAO.AddSupplier(newSupplier);

            // Refresh the supplier list
            LoadSupplierList();

            // Clear the input fields
            ClearInputFields2();


        }

        private void ClearInputFields2()
        {

            SupplierNameTextBox.Text = string.Empty;
                ContactPersonTextBox.Text = string.Empty;
            PhoneTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            AddressTextBox.Text = string.Empty;


        }

        private bool IsEmailUniqueForUpdate(string email, int supplierId)
        {
            return !_supplierDAO.GetAllSuppliers()
                .Any(s => s.SupplierId != supplierId && s.Email != null && s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsPhoneUniqueForUpdate(string phone, int supplierId)
        {
            return !_supplierDAO.GetAllSuppliers()
                .Any(s => s.SupplierId != supplierId && s.Phone != null && s.Phone.Equals(phone, StringComparison.OrdinalIgnoreCase));
        }

        private void UpdateSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SupplierNameTextBox.Text) ||
               string.IsNullOrWhiteSpace(ContactPersonTextBox.Text) ||
               string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
               string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
               string.IsNullOrWhiteSpace(AddressTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate phone number
            if (!IsPhoneNumberValid(PhoneTextBox.Text) || PhoneTextBox.Text.Length != 10)
            {
                MessageBox.Show("Phone number must only contain digits and be 10 digits long.", "Invalid Phone Number", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ManagerDataGrid.SelectedItem is Supplier selectedSupplier)
            {
                // Check if the email has been changed
                if (!selectedSupplier.Email.Equals(EmailTextBox.Text, StringComparison.OrdinalIgnoreCase))
                {
                    // If the email is changed, check if it already exists
                    if (!IsEmailUniqueForUpdate(EmailTextBox.Text, selectedSupplier.SupplierId))
                    {
                        MessageBox.Show("This email is already in use by another supplier. Please use a different email address.", "Duplicate Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Check if the phone number has been changed
                if (!selectedSupplier.Phone.Equals(PhoneTextBox.Text, StringComparison.OrdinalIgnoreCase))
                {
                    // If the phone number is changed, check if it already exists
                    if (!IsPhoneUniqueForUpdate(PhoneTextBox.Text, selectedSupplier.SupplierId))
                    {
                        MessageBox.Show("This phone number is already in use by another supplier. Please use a different phone number.", "Duplicate Phone Number", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Update the supplier's properties with the new values
                selectedSupplier.Name = SupplierNameTextBox.Text;
                selectedSupplier.ContactPerson = ContactPersonTextBox.Text;
                selectedSupplier.Phone = PhoneTextBox.Text;
                selectedSupplier.Email = EmailTextBox.Text;
                selectedSupplier.Address = AddressTextBox.Text;

                // Update the supplier in the database
                _supplierDAO.UpdateSupplier(selectedSupplier);

                // Refresh the supplier list
                LoadSupplierList();
                ClearInputFields2();
            }
            else
            {
                MessageBox.Show("Please select a supplier to update.");
            }
        }


        private void DeleteSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerDataGrid.SelectedItem is Supplier selectedSupplier)
            {
                // Confirm deletion with the user
                if (MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Deletion", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // Delete the supplier from the database
                    _supplierDAO.DeleteSupplier(selectedSupplier.SupplierId);

                    // Refresh the supplier list
                    LoadSupplierList();

                    // Clear the input fields
                    ClearInputFields2();
                }
            }
            else
            {
                MessageBox.Show("Please select a supplier to delete.");
            }

        }

        private void ClearInputFields3()
        {

            CategoryNameTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            


        }

        private void LoadCategoryList()
        {
            // Loads only accounts with "Staff" role into the DataGrid
            CategoryDataGrid.ItemsSource = _categoryDAO.GetAllCategories();
        }

        private void ReadCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryDataGrid.SelectedItem is Category selectedCategory)
            {
                CategoryNameTextBox.Text = selectedCategory.CategoryName;
                DescriptionTextBox.Text = selectedCategory.Description;
            }
            else
            {
                MessageBox.Show("Please select a category to read.");
            }

        }

        private void CreateCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if the CategoryName already exists
            if (_categoryDAO.GetAllCategories().Any(c => c.CategoryName.Equals(CategoryNameTextBox.Text, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Category name already exists. Please use a different name.", "Duplicate Category", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create and add a new category
            Category newCategory = new Category
            {
                CategoryName = CategoryNameTextBox.Text,
                Description = DescriptionTextBox.Text,
            };

            _categoryDAO.AddCategory(newCategory);

            // Refresh the category list
            LoadCategoryList();

            // Clear the input fields
            ClearInputFields3();
        }

        private void UpdateCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text) ||
               string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CategoryDataGrid.SelectedItem is Category selectedCategory)
            {
                // Check if the updated CategoryName is already in use by another category
                if (_categoryDAO.GetAllCategories()
                    .Any(c => c.CategoryId != selectedCategory.CategoryId &&
                              c.CategoryName.Equals(CategoryNameTextBox.Text, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Category name already exists. Please use a different name.", "Duplicate Category", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Update the category's properties with the new values
                selectedCategory.CategoryName = CategoryNameTextBox.Text;
                selectedCategory.Description = DescriptionTextBox.Text;

                // Update the category in the database
                _categoryDAO.UpdateCategory(selectedCategory);

                // Refresh the category list
                LoadCategoryList();
                ClearInputFields3();
            }
            else
            {
                MessageBox.Show("Please select a category to update.");
            }
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {

            if (CategoryDataGrid.SelectedItem is Category selectedCategory)
            {
                // Confirm deletion with the user
                if (MessageBox.Show("Are you sure you want to delete this category?", "Confirm Deletion", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // Delete the category from the database
                    _categoryDAO.DeleteCategory(selectedCategory.CategoryId);

                    // Refresh the category list
                    LoadCategoryList();

                    // Clear the input fields
                    ClearInputFields3();
                }
            }
            else
            {
                MessageBox.Show("Please select a category to delete.");
            }

        }


        private void StaffDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
