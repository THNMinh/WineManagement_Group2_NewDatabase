using DataAccessLayer;
using System;
using System.Windows;
using WineWarehouseManagement; // Update to include other window namespaces if needed

namespace WineWarehouseManagement
{
    public partial class LoginWindow : Window
    {
        private readonly IAccountRepository _accountRepository;

        public LoginWindow()
        {
            InitializeComponent();
            _accountRepository = new AccountDAO();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            // Retrieve account details based on username
            var account = _accountRepository.GetAccountMember(email);

            if (account != null && account.PasswordHash == password)
            {
                // Check the role and navigate to the corresponding window
                switch (account.Role)
                {
                    case "Admin":
                        new AdminWindow().Show();
                        break;
                    case "Manager":
                        new ManagerHomePageWindow().Show();
                        break;
                    case "Staff":
                        new StaffWindow(account.AccountId).Show();
                        break;
                    default:
                        MessageBox.Show("Role is undefined.");
                        return;
                }

                // Close the login window after successful login
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Email or password.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
