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
    /// Логика взаимодействия для InterfacePavilions.xaml
    /// </summary>
    public partial class InterfacePavilions : Page
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KingIT; Integrated Security=True;";
        public string MN;
        public string PN;
        public int FID;

        private void FillInterface(int FuncID)
        {
            if (FuncID == 1)
            {
                ChangeP.Visibility = Visibility.Collapsed;
            }
            else
            {
                switch (FuncID)
                {
                    case 2:
                        using (SqlConnection CN = new SqlConnection(ConStr))
                        {
                            CN.Open();
                            SqlCommand CCMMND = new SqlCommand("SELECT * FROM Malls WHERE MallName = @MN", CN);
                            SqlCommand VCMMND = new SqlCommand("SELECT * FROM Pavilions WHERE MallName = @MN PavilionID = @PN", CN);
                            VCMMND.Parameters.AddWithValue("@MN", MN);
                            VCMMND.Parameters.AddWithValue("@PN", PN);
                            SqlDataReader VDR = VCMMND.ExecuteReader();
                            PavID.Text = VDR["PavilionID"].ToString();
                            Floor.Text = VDR["Floor"].ToString();
                            SquareP.Text = VDR["Square"].ToString();
                            Stat.Text = VDR["Status"].ToString();
                            ARVP.Text = VDR["AVR"].ToString();
                            CostP.Text = VDR["CPMS"].ToString();
                            CN.Close();
                        }
                        AddP.Visibility = Visibility.Collapsed;
                        break;

                    case 3:
                        using (SqlConnection CN = new SqlConnection(ConStr))
                        {
                            CN.Open();
                            SqlCommand VCMMND = new SqlCommand("SELECT * FROM Pavilions WHERE MallName = @MN PavilionID = @PN", CN);
                            VCMMND.Parameters.AddWithValue("@MN", MN);
                            VCMMND.Parameters.AddWithValue("@PN", PN);
                            SqlDataReader VDR = VCMMND.ExecuteReader();
                            PavID.Text = VDR["PavilionID"].ToString();
                            Floor.Text = VDR["Floor"].ToString();
                            SquareP.Text = VDR["Square"].ToString();
                            Stat.Text = VDR["Status"].ToString();
                            ARVP.Text = VDR["AVR"].ToString();
                            CostP.Text = VDR["CPMS"].ToString();
                            CN.Close();
                        }
                        AddP.Visibility = Visibility.Collapsed;
                        ChangeP.Visibility = Visibility.Collapsed;
                        PavID.IsReadOnly = true;
                        Floor.IsReadOnly = true;
                        SquareP.IsReadOnly = true;
                        Stat.IsReadOnly = true;
                        ARVP.IsReadOnly = true;
                        CostP.IsReadOnly = true;
                        break;
                }
            }
        }

        public InterfacePavilions(string MallName, string PavName, int ID)
        {
            InitializeComponent();
            MN = MallName;
            PN = PavName;
            FID = ID;
            FillInterface(FID);
        }

        private void Backward(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MPList(2));
        }

        private void AddPav(object sender, RoutedEventArgs e)
        {
            if (PavID.Text != null && Floor.Text != null && SquareP.Text != null && Stat.Text != null && ARVP.Text != null && CostP.Text != null)
            {
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand AddPavilion = new SqlCommand("INSERT INTO Pavilions (MallName, PavilionID, Floor, Status, Square, CPMS, ARV)" + 
                        "VALUES (@MN, @PID, @FL, @ST, @SQ, @CPSM, @ARV)", CN);
                    AddPavilion.Parameters.Add("@MN", SqlDbType.NVarChar, 100).Value = MN;
                    AddPavilion.Parameters.Add("@PID", SqlDbType.NVarChar, 5).Value = PavID.Text;
                    AddPavilion.Parameters.Add("@FL", SqlDbType.Int).Value = Convert.ToInt32(Floor.Text);
                    AddPavilion.Parameters.Add("@ST", SqlDbType.NVarChar, 15).Value = Stat.Text;
                    AddPavilion.Parameters.Add("@SQ", SqlDbType.Float).Value = float.Parse(SquareP.Text);
                    AddPavilion.Parameters.Add("@CPSM", SqlDbType.Float).Value = float.Parse(CostP.Text);
                    AddPavilion.Parameters.Add("@ARV", SqlDbType.Float).Value = float.Parse(ARVP.Text);
                    AddPavilion.ExecuteNonQuery();
                    CN.Close();
                }
                NavigationService.Navigate(new MPList(2));
            }
            else MessageBox.Show("Не все поля заполненны");
        }

        private void ChangePav(object sender, RoutedEventArgs e)
        {
            if (PavID.Text != null && Floor.Text != null && SquareP.Text != null && Stat.Text != null && ARVP.Text != null && CostP.Text != null)
            {
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand AddPavilion = new SqlCommand("UPDATE Pavilions SET PavilionID = @PID, Floor = @FL," + 
                        "Status = @ST, Square = @SQ, CPMS = @CPSM, ARV = @ARV", CN);
                    AddPavilion.Parameters.Add("@PID", SqlDbType.NVarChar, 5).Value = PavID.Text;
                    AddPavilion.Parameters.Add("@FL", SqlDbType.Int).Value = Convert.ToInt32(Floor.Text);
                    AddPavilion.Parameters.Add("@ST", SqlDbType.NVarChar, 15).Value = Stat.Text;
                    AddPavilion.Parameters.Add("@SQ", SqlDbType.Float).Value = float.Parse(SquareP.Text);
                    AddPavilion.Parameters.Add("@CPSM", SqlDbType.Float).Value = float.Parse(CostP.Text);
                    AddPavilion.Parameters.Add("@ARV", SqlDbType.Float).Value = float.Parse(ARVP.Text);
                    AddPavilion.ExecuteNonQuery();
                    CN.Close();
                }
                NavigationService.Navigate(new MPList(2));
            }
            else MessageBox.Show("Не все поля заполненны");
        }
    }
}