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
    /// Interaction logic for ManagerHomePageWindow.xaml
    /// </summary>
    public partial class ManagerHomePageWindow : Window
    {
        public ManagerHomePageWindow()
        {
            InitializeComponent();
        }

        private void btnRequstLisr(object sender, RoutedEventArgs e)
        {
            new ListRequestWindow().Show();
            this.Close();

        }

        private void btnWine_Supplier(object sender, RoutedEventArgs e)
        {
            new WineManagement().Show();
            this.Close();
        }

        private void btnStaff(object sender, RoutedEventArgs e)
        {
            new ManagerWindow().Show();
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void btnAddWineWarehouse(object sender, RoutedEventArgs e)
        {
            new AddWineWarehouseWindow().Show();
            this.Close();
        }
    }
}
