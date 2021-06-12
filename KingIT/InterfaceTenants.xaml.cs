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
using System.Data;
using System.Data.SqlClient;

namespace KingIT
{
    /// <summary>
    /// Логика взаимодействия для InterfaceTenants.xaml
    /// </summary>
    public partial class InterfaceTenants : Page
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KingIT; Integrated Security=True;";
        public int Ten;
        public int FID;

        private void FillInterface(int FuncID)
        {
            if (FuncID == 1)
            {
                ChangeT.Visibility = Visibility.Collapsed;
            }
            else
            {
                switch (FuncID)
                {
                    case 2:
                        using (SqlConnection CN = new SqlConnection(ConStr))
                        {
                            CN.Open();
                            SqlCommand CCMMND = new SqlCommand("SELECT * FROM Tenants WHERE TenantID = @TID", CN);
                            CCMMND.Parameters.AddWithValue("@TID", Ten);
                            SqlDataReader CDR = CCMMND.ExecuteReader();
                            CDR.Read();
                            CoNa.Text = CDR["Name"].ToString();
                            Ph.Text = CDR["Phone"].ToString();
                            Ads.Text = CDR["Address"].ToString();
                            CDR.Close();
                            CN.Close();
                        }
                        AddT.Visibility = Visibility.Collapsed;
                        break;

                    case 3:
                        using (SqlConnection CN = new SqlConnection(ConStr))
                        {
                            CN.Open();
                            SqlCommand CCMMND = new SqlCommand("SELECT * FROM Tenants WHERE TenantID = @TID", CN);
                            CCMMND.Parameters.AddWithValue("@TID", Ten);
                            SqlDataReader CDR = CCMMND.ExecuteReader();
                            CDR.Read();
                            CoNa.Text = CDR["Name"].ToString();
                            Ph.Text = CDR["Phone"].ToString();
                            Ads.Text = CDR["Address"].ToString();
                            CDR.Close();
                            CN.Close();
                        }
                        CoNa.IsReadOnly = true;
                        Ph.IsReadOnly = true;
                        Ads.IsReadOnly = true;
                        AddT.Visibility = Visibility.Collapsed;
                        ChangeT.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        public InterfaceTenants(int TID, int CID)
        {
            InitializeComponent();
            Ten = TID;
            FID = CID;
            FillInterface(FID);
        }

        private void Backward(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuTenants());
        }

        private void AddTen(object sender, RoutedEventArgs e)
        {
            if (CoNa.Text != null && Ph.Text != null && Ads.Text != null)
            {
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand ACMMND = new SqlCommand("INSERT INTO Tenants (Name, Phone, Address) VALUES @CN, @PN, @ADS", CN);
                    ACMMND.Parameters.Add("@CN", SqlDbType.NVarChar, 30).Value = CoNa.Text;
                    ACMMND.Parameters.Add("@PN", SqlDbType.NVarChar, 20).Value = Ph.Text;
                    ACMMND.Parameters.Add("@ADS", SqlDbType.NVarChar, 100).Value = Ads.Text;
                    ACMMND.ExecuteNonQuery();
                    CN.Close();
                }
                NavigationService.Navigate(new MenuTenants());
            }
            else MessageBox.Show("Не все поля заполненны");
        }

        private void ChangeTen(object sender, RoutedEventArgs e)
        {
            if (CoNa.Text != null && Ph.Text != null && Ads.Text != null)
            {
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand ACMMND = new SqlCommand("UPDATE Tenants SET Name = @CN, Phone = @PN, Address = @ADS", CN);
                    ACMMND.Parameters.Add("@CN", SqlDbType.NVarChar, 30).Value = CoNa.Text;
                    ACMMND.Parameters.Add("@PN", SqlDbType.NVarChar, 20).Value = Ph.Text;
                    ACMMND.Parameters.Add("@ADS", SqlDbType.NVarChar, 100).Value = Ads.Text;
                    ACMMND.ExecuteNonQuery();
                    CN.Close();
                }
                NavigationService.Navigate(new MenuTenants());
            }
            else MessageBox.Show("Не все поля заполненны");
        }
    }
}
