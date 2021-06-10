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
using System.IO;
using System.Data.SqlTypes;
using System.Data;

namespace KingIT
{
    /// <summary>
    /// Логика взаимодействия для InterfaceMalls.xaml
    /// </summary>
    public partial class InterfaceMalls : Page
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KingIT; Integrated Security=True;";
        public string MN;
        public int FID;

        private void FillInterface(int FuncID)
        {
            if (FuncID == 1)
            {
                ChangeM.Visibility = Visibility.Collapsed;
                PTPV.Visibility = Visibility.Visible;
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
                            CCMMND.Parameters.AddWithValue("@MN", MN);
                            SqlDataReader CDR = CCMMND.ExecuteReader();
                            CDR.Read();
                            MallN.Text = CDR["MallName"].ToString();
                            ARV.Text = CDR["ARV"].ToString();
                            Stat.Text = CDR["Status"].ToString();
                            CostM.Text = CDR["Cost"].ToString();
                            CityM.Text = CDR["City"].ToString();
                            PC.Text = CDR["PavilionCount"].ToString();
                            Floor.Text = CDR["NoS"].ToString();
                            var IB = CDR["MallPhoto"] as byte[];
                            MemoryStream MS = new MemoryStream();
                            MS.Write(IB, 0, IB.Length);
                            BitmapImage BMI = new BitmapImage();
                            BMI.BeginInit();
                            BMI.StreamSource = MS;
                            BMI.EndInit();
                            IM.Source = BMI;
                            CDR.Close();
                            CN.Close();
                        }
                        AddM.Visibility = Visibility.Collapsed;
                        PTPV.Visibility = Visibility.Visible;
                        break;

                case 3:
                        using (SqlConnection CN = new SqlConnection(ConStr))
                        {
                            CN.Open();
                            SqlCommand VCMMND = new SqlCommand("SELECT * FROM Malls WHERE MallName = @MN", CN);
                            VCMMND.Parameters.AddWithValue("@MN", MN);
                            SqlDataReader VDR = VCMMND.ExecuteReader();
                            VDR.Read();
                            MallN.Text = VDR["MallName"].ToString();
                            ARV.Text = VDR["ARV"].ToString();
                            Stat.Text = VDR["Status"].ToString();
                            CostM.Text = VDR["Cost"].ToString();
                            CityM.Text = VDR["City"].ToString();
                            PC.Text = VDR["PavilionCount"].ToString();
                            Floor.Text = VDR["NoS"].ToString();
                            var IB = VDR["MallPhoto"] as byte[];
                            MemoryStream MS = new MemoryStream();
                            MS.Write(IB, 0, IB.Length);
                            BitmapImage BMI = new BitmapImage();
                            BMI.BeginInit();
                            BMI.StreamSource = MS;
                            BMI.EndInit();
                            IM.Source = BMI;
                            VDR.Close();
                            CN.Close();
                        }
                        AddM.Visibility = Visibility.Collapsed;
                        ChangeM.Visibility = Visibility.Collapsed;
                        PTP.Visibility = Visibility.Collapsed;
                        MallN.IsReadOnly = true;
                        ARV.IsReadOnly = true;
                        Stat.IsReadOnly = true;
                        CostM.IsReadOnly = true;
                        CityM.IsReadOnly = true;
                        PC.IsReadOnly = true;
                        Floor.IsReadOnly = true;
                        break;
                }
            }
        }

        public InterfaceMalls(string MallName, int ID)
        {
            InitializeComponent();
            MN = MallName;
            FID = ID;
            FillInterface(FID);
        }

        private void Backward(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MPList(1));
        }

        private static byte[] GetPhoto(string filePath)
        {
            try
            {
                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader RD = new BinaryReader(stream);
                byte[] photo = RD.ReadBytes((int)stream.Length);
                RD.Close();
                stream.Close();
                return photo;
            }
            catch (Exception)
            {
            }
            return null;
        }

        private void AddMall(object sender, RoutedEventArgs e)
        {
            if (MallN.Text != null && ARV.Text != null && Stat.Text != null && CostM.Text != null 
                && CityM.Text != null && PC.Text != null && Floor.Text != null && PTP.Text != null)
                {
                byte[] photo = GetPhoto(PTP.Text); 
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand ACMMND = new SqlCommand("INSERT INTO Malls (MallName, [Status], PavilionCount, City, Cost, ARV, NoS, MallPhoto)"
                        + "VALUES (@MN, @St, @PC, @City, @Cost, @ARV, @NS, @MP)", CN);
                    ACMMND.Parameters.Add("@MN", SqlDbType.NVarChar, 100).Value = MallN.Text;
                    ACMMND.Parameters.Add("@St", SqlDbType.NVarChar, 15).Value = ARV.Text;
                    ACMMND.Parameters.Add("@PC", SqlDbType.Int).Value = Convert.ToInt32(PC.Text);
                    ACMMND.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = CityM.Text;
                    ACMMND.Parameters.Add("@Cost", SqlDbType.Int).Value = Convert.ToInt32(CostM.Text);
                    ACMMND.Parameters.Add("@ARV", SqlDbType.Float).Value = float.Parse(ARV.Text);
                    ACMMND.Parameters.Add("@NS", SqlDbType.Int).Value = Convert.ToInt32(Floor.Text);
                    ACMMND.Parameters.Add("@MP", SqlDbType.Image, photo.Length).Value = photo;
                    ACMMND.ExecuteNonQuery();
                    CN.Close();
                }
                NavigationService.Navigate(new MPList(1));
            }
            else MessageBox.Show("Не все поля заполненны");
        }

        private void ChangeMall(object sender, RoutedEventArgs e)
        {
            if (MallN.Text != null && ARV.Text != null && Stat.Text != null && CostM.Text != null 
                && CityM.Text != null && PC.Text != null && Floor.Text != null && PTP.Text != null)
                {
                byte[] photo = GetPhoto(PTP.Text); 
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand ACMMND = new SqlCommand("UPDATE Malls Set MallName = @MN, [Status] = @St, PavilionCount = @PC"
                        + "City = @City, Cost = @Cost, ARV = @ARV, NoS = @NS, MallPhoto = @MP", CN);
                    ACMMND.Parameters.Add("@MN", SqlDbType.NVarChar, 100).Value = MallN.Text;
                    ACMMND.Parameters.Add("@St", SqlDbType.NVarChar, 15).Value = ARV.Text;
                    ACMMND.Parameters.Add("@PC", SqlDbType.Int).Value = Convert.ToInt32(PC.Text);
                    ACMMND.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = CityM.Text;
                    ACMMND.Parameters.Add("@Cost", SqlDbType.Int).Value = Convert.ToInt32(CostM.Text);
                    ACMMND.Parameters.Add("@ARV", SqlDbType.Float).Value = float.Parse(ARV.Text);
                    ACMMND.Parameters.Add("@NS", SqlDbType.Int).Value = Convert.ToInt32(Floor.Text);
                    ACMMND.Parameters.Add("@MP", SqlDbType.Image, photo.Length).Value = photo;
                    ACMMND.ExecuteNonQuery();
                    CN.Close();
                }
                NavigationService.Navigate(new MPList(1));
            }
            else MessageBox.Show("Не все поля заполненны");
        }
    }
}