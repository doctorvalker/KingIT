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
        private string FCD = "SELECT Surname, Name, MiddleName, Login, Password, Role, PhoneNumber FROM Employees";

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
            //NavigationService.Nagigate(new InterfaceAdmin(null, 1));
        }

        private void ChangeE(object sender, RoutedEventArgs e)
        {
            if (EN.Text != null)
            {
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand CE = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE Surname = @SN", CN);
                    CE.Parameters.AddWithValue("@SN",EN.Text);
                    int EC = (int)CE.ExecuteScalar();
                    if (EC > 0)
                    {

                    }

                    CN.Close();
                }
            }
        }

        private void DeleteE(object sender, RoutedEventArgs e)
        {

        }

        private void ViewE(object sender, RoutedEventArgs e)
        {

        }
    }
}
