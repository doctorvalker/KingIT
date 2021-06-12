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
using System.Collections;

namespace KingIT
{
    /// <summary>
    /// Логика взаимодействия для Autoriz.xaml
    /// </summary>
    public partial class Autoriz : Page
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KingIT; Integrated Security=True;";
        private int CapCount = 0;

        private bool NickCheck()
        {
            string LogComm = "SELECT count(*) FROM Employees WHERE Login = @log";

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();
                SqlCommand CMMND = new SqlCommand(LogComm, Con);
                CMMND.Parameters.AddWithValue("@log", Login.Text);
                int NC = (int)CMMND.ExecuteScalar();
                if (NC > 0)
                {
                    Con.Close();
                    return true;
                }
                else
                {
                    Login.Text = String.Empty;
                    Password.Password = String.Empty;
                    MessageBox.Show ("Неверный логин");
                }
                Con.Close();
                return false;
            }
        }

        private bool PassCheck()
        {
            string PassComm = "SELECT count(*) FROM Employees WHERE Login = @log AND Password = @pass";

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();
                SqlCommand CMMND = new SqlCommand(PassComm, Con);
                CMMND.Parameters.AddWithValue("@log", Login.Text);
                CMMND.Parameters.AddWithValue("@pass", Password.Password);
                int PC = (int)CMMND.ExecuteScalar();
                if (PC > 0)
                {
                    Con.Close();
                    return true;
                }
                else
                {
                    Login.Text = String.Empty;
                    Password.Password = String.Empty;
                    MessageBox.Show("Неверный пароль");
                }
                return false;
            }
        }

        private void StatusCheck()
        {
            string StCh = "SELECT Role FROM Employees WHERE Login = @log AND Password = @pass";
            string Stat = "";

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();
                SqlCommand SC = new SqlCommand(StCh, Con);
                SC.Parameters.AddWithValue("@log", Login.Text);
                SC.Parameters.AddWithValue("@pass", Password.Password);
                SqlDataReader SCDR = SC.ExecuteReader();
                SCDR.Read();
                Stat = SCDR["Role"].ToString();
                SCDR.Close();
                Con.Close();
            }
            switch (Stat)
            {
                case "Администратор":
                    NavigationService.Navigate(new MenuAdmin());
                    break;
                case "Менеджер А":
                    NavigationService.Navigate(new MenuTenants());
                    break;
                case "Менеджер С":
                    NavigationService.Navigate(new MainMenu());
                    break;
                case "Удален":
                    MessageBox.Show("Данный пользователь удалён");
                    break;
            }
        }


        public Autoriz()
        {
            InitializeComponent();
        }

        private void EntMenu(object sender, RoutedEventArgs e)
        {
            if (CPTCHT.Text == CPTCHIT.Text)
            {
                if (NickCheck() == true && PassCheck() == true)
                {
                    StatusCheck();
                }
                else CapCount++;
                if (CapCount >= 3)
                {
                    CPTCHG.Visibility = Visibility.Visible;
                    CPTCHT.Visibility = Visibility.Visible;
                    CPTCHIT.Visibility = Visibility.Visible;
                    RNDCPTCH();
                }
            }
            else
            {
                RNDCPTCH();
                Login.Text = String.Empty;
                Password.Password = String.Empty;
                CPTCHIT.Text = String.Empty;
            }
        }

        private void RNDCPTCH()
        {
            string CPTCHSTR = "";
            Random RNDSTR = new Random();
            for (int k = 0; k < 7; k++)
            {
                char CPTCHCH = (char)(RNDSTR.Next(48, 57) + RNDSTR.Next(8, 25) + RNDSTR.Next(7, 25));
                CPTCHSTR += CPTCHCH;
            }
            CPTCHT.Text = CPTCHSTR;
        }
    }
}
