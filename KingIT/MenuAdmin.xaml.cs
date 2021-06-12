using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace KingIT
{
    /// <summary>
    /// Логика взаимодействия для MenuAdmin.xaml
    /// </summary>
    public partial class MenuAdmin : Page
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KingIT; Integrated Security=True;";
        private string FCD = "SELECT * FROM Employees";

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

        public MenuAdmin()
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

        private void AddE(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InterfaceAdmin(0, 1));
        }

        private void ChangeE(object sender, RoutedEventArgs e)
        {
            if (EN.Text != null)
            {
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand CE = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE Surname = @SN", CN);
                    CE.Parameters.AddWithValue("@SN", Convert.ToInt32(EN.Text));
                    int EC = (int)CE.ExecuteScalar();
                    if (EC > 0)
                    {
                        SqlCommand EID = new SqlCommand("SELECT * FROM Employees WHERE EmployeeID = @EID", CN);
                        EID.Parameters.AddWithValue("@EID", Convert.ToInt32(EN.Text));
                        SqlDataReader EDR = EID.ExecuteReader();
                        EDR.Read();
                        NavigationService.Navigate(new InterfaceAdmin(Convert.ToInt32(EDR["EmployeeID"]), 2));
                        EDR.Close();
                    }
                    else MessageBox.Show("Такого сотрудника не существует");
                    CN.Close();
                }
            }
            else MessageBox.Show("Вы не ввели код сотрудника");
        }

        private void DeleteE(object sender, RoutedEventArgs e)
        {
            if (EN.Text != null)
            {
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand CE = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE Surname = @SN", CN);
                    CE.Parameters.AddWithValue("@SN", Convert.ToInt32(EN.Text));
                    int EC = (int)CE.ExecuteScalar();
                    if (EC > 0)
                    {
                        SqlCommand DE = new SqlCommand("UPDATE Employees SET Role = 'Удален' WHERE Surname = @SN", CN);
                        DE.Parameters.AddWithValue("@SN", Convert.ToInt32(EN.Text));
                        DE.ExecuteNonQuery();
                        NavigationService.Refresh();
                    }
                    else MessageBox.Show("Такого сотрудника не существует");
                    CN.Close();
                }
            }
            else MessageBox.Show("Вы не ввели код сотрудника");
        }

        private void ViewE(object sender, RoutedEventArgs e)
        {
            if (EN.Text != null)
            {
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand CE = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE EmployeeID = @EID", CN);
                    CE.Parameters.AddWithValue("@EID", Convert.ToInt32(EN.Text));
                    int EC = (int)CE.ExecuteScalar();
                    if (EC > 0)
                    {
                        SqlCommand EID = new SqlCommand("SELECT * FROM Employees WHERE EmployeeID = @EID", CN);
                        EID.Parameters.AddWithValue("@EID", Convert.ToInt32(EN.Text));
                        SqlDataReader EDR = EID.ExecuteReader();
                        EDR.Read();
                        NavigationService.Navigate(new InterfaceAdmin(Convert.ToInt32(EDR["EmployeeID"]), 3));
                        EDR.Close();
                    }
                    else MessageBox.Show("Такого сотрудника не существует");
                    CN.Close();
                }
            }
            else MessageBox.Show("Вы не ввели код сотрудника");
        }
    }
}
