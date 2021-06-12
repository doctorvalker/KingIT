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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace KingIT
{
    /// <summary>
    /// Логика взаимодействия для MenuRents.xaml
    /// </summary>
    public partial class MenuTenants : Page
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KingIT; Integrated Security=True;";
        private string FCD = "SELECT * FROM Tenants";

        private void FillDG()
        {
            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                SqlCommand FDG = new SqlCommand(FCD, CN);
                SqlDataAdapter FDA = new SqlDataAdapter(FDG);
                DataTable FDT = new DataTable();
                FDA.Fill(FDT);
                DG.ItemsSource = FDT.DefaultView;
                CN.Close();
            }
        }

        public MenuTenants()
        {
            InitializeComponent();
            FillDG();
        }

        private void Backward(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autoriz());
        }

        private void SPSOpen(object sender, MouseEventArgs e)
        {
            SPS.Opacity = 1;
        }

        private void SPSClose(object sender, MouseEventArgs e)
        {
            SPS.Opacity = 0;
        }

        private void AddT(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InterfaceTenants(0, 1));
        }

        private void ChangeT(object sender, RoutedEventArgs e)
        {
            if (TN.Text != null)
            {
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand CE = new SqlCommand("SELECT COUNT(*) FROM Tenants WHERE TenantID = @TID", CN);
                    CE.Parameters.AddWithValue("@TID", Convert.ToInt32(TN.Text));
                    int EC = (int)CE.ExecuteScalar();
                    if (EC > 0)
                    {
                        SqlCommand EID = new SqlCommand("SELECT * FROM Tenants WHERE TenantID = @TID", CN);
                        EID.Parameters.AddWithValue("@TID", Convert.ToInt32(TN.Text));
                        SqlDataReader EDR = EID.ExecuteReader();
                        EDR.Read();
                        NavigationService.Navigate(new InterfaceTenants(Convert.ToInt32(EDR["TenantID"]), 3));
                        EDR.Close();
                    }
                    else MessageBox.Show("Такой компании не существует");
                    CN.Close();
                }
            }
            else MessageBox.Show("Вы не ввели код компании");
        }

        private void ViewT(object sender, RoutedEventArgs e)
        {
            if (TN.Text != null)
            {
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand CE = new SqlCommand("SELECT COUNT(*) FROM Tenants WHERE TenantID = @TID", CN);
                    CE.Parameters.AddWithValue("@TID", Convert.ToInt32(TN.Text));
                    int EC = (int)CE.ExecuteScalar();
                    if (EC > 0)
                    {
                        SqlCommand EID = new SqlCommand("SELECT * FROM Tenants WHERE TenantID = @TID", CN);
                        EID.Parameters.AddWithValue("@TID", Convert.ToInt32(TN.Text));
                        SqlDataReader EDR = EID.ExecuteReader();
                        EDR.Read();
                        NavigationService.Navigate(new InterfaceTenants(Convert.ToInt32(EDR["TenantID"]), 3));
                        EDR.Close();
                    }
                    else MessageBox.Show("Такой компании не существует");
                    CN.Close();
                }
            }
            else MessageBox.Show("Вы не ввели код компании");
        }
    }
}
