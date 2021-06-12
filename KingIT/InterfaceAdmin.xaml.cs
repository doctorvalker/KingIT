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
using System.IO;

namespace KingIT
{
    /// <summary>
    /// Логика взаимодействия для InterfaceAdmin.xaml
    /// </summary>
    public partial class InterfaceAdmin : Page
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KingIT; Integrated Security=True;";
        public int Emp;
        public int FID;

        private void FillInterface(int FuncID)
        {
            if (FuncID == 1)
            {
                ChangeE.Visibility = Visibility.Collapsed;
                EmPh.Visibility = Visibility.Collapsed;
            }
            else
            {
                switch (FuncID)
                {
                    case 2:
                        using (SqlConnection CN = new SqlConnection(ConStr))
                        {
                            CN.Open();
                            SqlCommand CCMMND = new SqlCommand("SELECT * FROM Employees WHERE EmployeeID = @EID", CN);
                            CCMMND.Parameters.AddWithValue("@EID", Emp);
                            SqlDataReader CDR = CCMMND.ExecuteReader();
                            CDR.Read();
                            SuNa.Text = CDR["Surname"].ToString();
                            Na.Text = CDR["Name"].ToString();
                            Log.Text = CDR["Login"].ToString();
                            Pass.Text = CDR["Password"].ToString();
                            Role.Text = CDR["Role"].ToString();
                            Gen.Text = CDR["Gender"].ToString();
                            PhNu.Text = CDR["PhoneNumber"].ToString();
                            var IB = CDR["Photo"] as byte[];
                            MemoryStream MS = new MemoryStream();
                            MS.Write(IB, 0, IB.Length);
                            BitmapImage BMI = new BitmapImage();
                            BMI.BeginInit();
                            BMI.StreamSource = MS;
                            BMI.EndInit();
                            EmPh.Source = BMI;
                            CDR.Close();
                            CN.Close();
                        }
                        AddE.Visibility = Visibility.Collapsed;
                        break;

                    case 3:
                        using (SqlConnection CN = new SqlConnection(ConStr))
                        {
                            CN.Open();
                            SqlCommand CCMMND = new SqlCommand("SELECT * FROM Employees WHERE EmployeeID = @EID", CN);
                            CCMMND.Parameters.AddWithValue("@EID", Emp);
                            SqlDataReader CDR = CCMMND.ExecuteReader();
                            CDR.Read();
                            SuNa.Text = CDR["Surname"].ToString();
                            Na.Text = CDR["Name"].ToString();
                            Log.Text = CDR["Login"].ToString();
                            Pass.Text = CDR["Password"].ToString();
                            Role.Text = CDR["Role"].ToString();
                            Gen.Text = CDR["Gender"].ToString();
                            PhNu.Text = CDR["PhoneNumber"].ToString();
                            var IB = CDR["Photo"] as byte[];
                            MemoryStream MS = new MemoryStream();
                            MS.Write(IB, 0, IB.Length);
                            BitmapImage BMI = new BitmapImage();
                            BMI.BeginInit();
                            BMI.StreamSource = MS;
                            BMI.EndInit();
                            EmPh.Source = BMI;
                            CDR.Close();
                            CN.Close();
                        }
                        SuNa.IsReadOnly = true;
                        Na.IsReadOnly = true;
                        Log.IsReadOnly = true;
                        Pass.IsReadOnly = true;
                        Role.IsReadOnly = true;
                        Gen.IsReadOnly = true;
                        PhNu.IsReadOnly = true;
                        PathToPhoto.Visibility = Visibility.Collapsed;
                        AddE.Visibility = Visibility.Collapsed;
                        ChangeE.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        public InterfaceAdmin(int Employee, int AID)
        {
            InitializeComponent();
            Emp = Employee;
            FID = AID;
            FillInterface(FID);
        }

        private void Backward(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuAdmin());
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

        private void AddEmp(object sender, RoutedEventArgs e)
        {
            if (SuNa.Text != null && Na.Text != null && Log.Text != null && Pass.Text != null 
                && Role.Text != null &&  Gen.Text != null && PhNu.Text != null && PathToPhoto.Text != null)
            {
                byte[] photo = GetPhoto(PathToPhoto.Text);
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand ACMMND = new SqlCommand("INSERT INTO Employees (Surname, Login, Password, Role, PhoneNumver, Gender, Photo)"
                        + "VALUES (@SN, @Na, @Log, @Pass, @Role, @PN, @Gen, @PH)", CN);
                    ACMMND.Parameters.Add("@SN", SqlDbType.NVarChar, 30).Value = SuNa.Text;
                    ACMMND.Parameters.Add("@Na", SqlDbType.NVarChar, 20).Value = Na.Text;
                    ACMMND.Parameters.Add("@Log", SqlDbType.NVarChar, 50).Value = Log.Text;
                    ACMMND.Parameters.Add("@Pass", SqlDbType.NVarChar, 20).Value = Pass.Text;
                    ACMMND.Parameters.Add("@Role", SqlDbType.NVarChar, 20).Value = Role.Text;
                    ACMMND.Parameters.Add("@PH", SqlDbType.NVarChar, 15).Value = PhNu.Text;
                    ACMMND.Parameters.Add("@Gen", SqlDbType.NVarChar, 1).Value = Gen.Text;
                    ACMMND.Parameters.Add("@PH", SqlDbType.Image, photo.Length).Value = photo;
                    ACMMND.ExecuteNonQuery();
                    CN.Close();
                }
                NavigationService.Navigate(new MenuAdmin());
            }
            else MessageBox.Show("Не все поля заполненны");
        }

        private void ChangeEmp(object sender, RoutedEventArgs e)
        {
            if (SuNa.Text != null && Na.Text != null && Log.Text != null && Pass.Text != null
                && Role.Text != null && Gen.Text != null && PhNu.Text != null && PathToPhoto.Text != null)
            {
                byte[] photo = GetPhoto(PathToPhoto.Text);
                using (SqlConnection CN = new SqlConnection(ConStr))
                {
                    CN.Open();
                    SqlCommand ACMMND = new SqlCommand("INSERT INTO Employees (Surname, Login, Password, Role, PhoneNumver, Gender, Photo)"
                        + "VALUES (@SN, @Na, @Log, @Pass, @Role, @PN, @Gen, @PH)", CN);
                    ACMMND.Parameters.Add("@SN", SqlDbType.NVarChar, 30).Value = SuNa.Text;
                    ACMMND.Parameters.Add("@Na", SqlDbType.NVarChar, 20).Value = Na.Text;
                    ACMMND.Parameters.Add("@Log", SqlDbType.NVarChar, 50).Value = Log.Text;
                    ACMMND.Parameters.Add("@Pass", SqlDbType.NVarChar, 20).Value = Pass.Text;
                    ACMMND.Parameters.Add("@Role", SqlDbType.NVarChar, 20).Value = Role.Text;
                    ACMMND.Parameters.Add("@PH", SqlDbType.NVarChar, 15).Value = PhNu.Text;
                    ACMMND.Parameters.Add("@Gen", SqlDbType.NVarChar, 1).Value = Gen.Text;
                    ACMMND.Parameters.Add("@PH", SqlDbType.Image, photo.Length).Value = photo;
                    ACMMND.ExecuteNonQuery();
                    CN.Close();
                }
                NavigationService.Navigate(new MenuAdmin());
            }
            else MessageBox.Show("Не все поля заполненны");
        }
    }
}
