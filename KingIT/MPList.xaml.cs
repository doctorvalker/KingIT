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
    /// Логика взаимодействия для MallsList.xaml
    /// </summary>
    public partial class MPList : Page
    {
        private string ConStr = @"Data Source=DESKTOP-FBTM7V3\SQLEXPRESS01; Initial Catalog=KingIT; Integrated Security=True;";
        public int MenuID;

        private void FDG(int CheckID)
        {
            string FillMall = "SELECT * FROM Malls";
            string FillPavilions = "SELECT * FROM Pavilions";

            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                if (CheckID == 1)
                {
                    SqlCommand FMV = new SqlCommand(FillMall, CN);
                    SqlDataAdapter FV = new SqlDataAdapter(FMV);
                    DataTable DS = new DataTable();
                    FV.Fill(DS);
                    DG.ItemsSource = DS.DefaultView;
                    POR.Content = "План";
                    POR.Tag = "План";
                    POR.Checked += new RoutedEventHandler(this.Filter);
                    ROB.Content = "Реализация";
                    ROB.Tag = "Реализация";
                    ROB.Checked += new RoutedEventHandler(this.Filter);
                    BOF.Content = "Строительство";
                    BOF.Tag = "Строительство";
                    BOF.Checked += new RoutedEventHandler(this.Filter);
                    FillWP();
                }
                else
                {
                    SqlCommand FPV = new SqlCommand(FillPavilions, CN);
                    SqlDataAdapter FV = new SqlDataAdapter(FPV);
                    DataTable DT = new DataTable("List");
                    FV.Fill(DT);
                    DG.ItemsSource = DT.DefaultView;
                    SC.Visibility = Visibility.Collapsed;
                    STB.Visibility = Visibility.Collapsed;
                    POR.Content = "Арендован";
                    POR.Tag = "Арендован";
                    POR.Checked += new RoutedEventHandler(this.Filter);
                    ROB.Content = "Забронировано";
                    ROB.Tag = "Забронировано";
                    ROB.Checked += new RoutedEventHandler(this.Filter);
                    BOF.Content = "Свободен";
                    BOF.Tag = "Свободен";
                    BOF.Checked += new RoutedEventHandler(this.Filter);
                }
                CN.Close();
            }
        }

        private void Filter (object sender, RoutedEventArgs e)
        {
            string FM = "SELECT * FROM Malls WHERE [Status] = @SID";
            string FP = "SELECT * FROM Pavilions WHERE [Status] = @SID";
            string Check = ((Control)sender).Tag.ToString();
            
            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                if (Check == "План" || Check == "Реализация" || Check == "Строительство")
                {
                    SqlCommand FMDG = new SqlCommand(FM, CN);
                    FMDG.Parameters.AddWithValue("@SID", Check);
                    SqlDataAdapter FMDA = new SqlDataAdapter(FMDG);
                    DataTable FMDT = new DataTable();
                    FMDA.Fill(FMDT);
                    DG.ItemsSource = FMDT.DefaultView;
                }
                else
                {
                    SqlCommand FPDG = new SqlCommand(FP, CN);
                    FPDG.Parameters.AddWithValue("@SID", Check);
                    SqlDataAdapter FPDA = new SqlDataAdapter(FPDG);
                    DataTable FPDT = new DataTable();
                    FPDA.Fill(FPDT);
                    DG.ItemsSource = FPDT.DefaultView;
                }
                CN.Close();
            }
        }

        private void FillWP()
        {
            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                SqlCommand FWP = new SqlCommand("SELECT DISTINCT City FROM Malls WHERE [Status] <> 'Удален'", CN);
                SqlDataReader FWPDR = FWP.ExecuteReader();
                if (FWPDR.HasRows)
                {
                    while (FWPDR.Read())
                    {
                        Button CB = new Button();
                        CB.FontFamily = new FontFamily("Arial");
                        CB.FontSize = 20;
                        CB.Content = FWPDR["City"].ToString();
                        CB.Tag = FWPDR["City"].ToString();
                        CB.Click += new RoutedEventHandler(this.ButCity);
                        Cities.Children.Add(CB);
                    }
                }
                FWPDR.Close();
                CN.Close();
            }
        }

        private void ButCity (object sender, RoutedEventArgs e)
        {
            string check = ((Control)sender).Tag.ToString();
            using (SqlConnection CN = new SqlConnection(ConStr))
            {
                CN.Open();
                SqlCommand CityName = new SqlCommand("SELECT * FROM Malls WHERE City = @City", CN);
                CityName.Parameters.AddWithValue("@City", check);
                SqlDataAdapter CNDA = new SqlDataAdapter(CityName);
                DataTable CNDT = new DataTable();
                CNDA.Fill(CNDT);
                DG.ItemsSource = CNDT.DefaultView;
                CN.Close();
            }
        }

        public MPList(int CheckMenu)
        {
            InitializeComponent();
            MenuID = CheckMenu;
            if (MenuID == 1)
            {
                NP.Opacity = 0;
                PN.Opacity = 0;
            }
            FDG(MenuID);

            BL(MenuID);
        }

        private void Backward(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autoriz());
        }

        private void BL(int CheckID)
        {
            if (CheckID == 1)
            {
                CF.FontFamily = new FontFamily("Arial");
                CF.FontSize = 20;
                CF.Content = "Открыть Павилионы";
                CF.Click += new RoutedEventHandler(this.CPV);
            }
            else
            {
                CF.FontFamily = new FontFamily("Arial");
                CF.FontSize = 20;
                CF.Content = "Открыть ТЦ";
                CF.Click += new RoutedEventHandler(this.CMV);
            }
        }

        private void CPV(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MPList(2));
        }

        private void CMV(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MPList(1));
        }

        private void SPSOpen(object sender, MouseEventArgs e)
        {
            SPS.Opacity = 1;
        }

        private void SPSClose(object sender, MouseEventArgs e)
        {
            SPS.Opacity = 0;
        }

        private void AddMP(object sender, RoutedEventArgs e)
        {
            if (MenuID == 1) NavigationService.Navigate(new InterfaceMalls(null, 1));
            else
            {
                if (MN.Text != null)
                {
                    using (SqlConnection CN = new SqlConnection(ConStr))
                    {
                        CN.Open();
                        SqlCommand CheckMall = new SqlCommand("SELECT COUNT(*) FROM Malls WHERE MallName = @MN", CN);
                        CheckMall.Parameters.AddWithValue("@MN", MN.Text);
                        int Check = (int)CheckMall.ExecuteScalar();
                        if (Check > 0)
                        {
                            NavigationService.Navigate(new InterfacePavilions(MN.Text, null, 1));
                        }
                        else
                        {
                            MessageBox.Show("Такого ТЦ не существует");
                        }
                        CN.Close();
                    }
                }
                else MessageBox.Show("Вы не ввели название ТЦ");
            }
        }

        private void ChangeMP(object sender, RoutedEventArgs e)
        {
            if (MN.Text != null)
            {
                if (MenuID == 1)
                {
                    using (SqlConnection CN = new SqlConnection(ConStr))
                    {
                        CN.Open();
                        SqlCommand CheckMall = new SqlCommand("SELECT COUNT(*) FROM Malls WHERE MallName = @MN", CN);
                        CheckMall.Parameters.AddWithValue("@MN", MN.Text);
                        int Check = (int)CheckMall.ExecuteScalar();
                        if (Check > 0)
                        {
                            NavigationService.Navigate(new InterfaceMalls(MN.Text, 2));
                        }
                        else
                        {
                            MessageBox.Show("Такого ТЦ не существует");
                        }
                        CN.Close();
                    }
                }
                else
                {
                    if (PN.Text != null)
                    {
                        using (SqlConnection CN = new SqlConnection(ConStr))
                        {
                            CN.Open();
                            SqlCommand CheckPav = new SqlCommand("SELECT COUNT(*) FROM Pavilions WHERE MallName = @PN AND PavilionID = @PID", CN);
                            CheckPav.Parameters.AddWithValue("@PN", MN.Text);
                            CheckPav.Parameters.AddWithValue("@PID", PN.Text);
                            int Check = (int)CheckPav.ExecuteScalar();
                            if (Check > 0)
                            {
                                NavigationService.Navigate(new InterfacePavilions(MN.Text, PN.Text, 2));
                            }
                            else
                            {
                                MessageBox.Show("Такого ТЦ/павильона не существует");
                            }
                            CN.Close();
                        }
                    }
                    else MessageBox.Show("Вы не ввели название павильона");
                }
            }
            else MessageBox.Show("Вы не ввели название ТЦ");
        }

        private void DeleteMP(object sender, RoutedEventArgs e)
        {
            if (MN.Text != null)
            {
                if (MenuID == 1)
                {
                    using (SqlConnection CN = new SqlConnection(ConStr))
                    {
                        CN.Open();
                        SqlCommand CheckMall = new SqlCommand("SELECT COUNT(*) FROM Malls WHERE MallName = @MN", CN);
                        CheckMall.Parameters.AddWithValue("@MN", MN.Text);
                        int Check = (int)CheckMall.ExecuteScalar();
                        if (Check > 0)
                        {
                            SqlCommand DeletePav = new SqlCommand("UPDATE Malls SET Status = 'Удален' WHERE MallName = @MN", CN);
                            DeletePav.Parameters.AddWithValue("@MN", MN.Text);
                            DeletePav.ExecuteNonQuery();
                            NavigationService.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("Такого ТЦ не существует");
                        }
                        CN.Close();
                    }
                }
                else 
                {
                    if (PN.Text != null)
                    {
                        using (SqlConnection CN = new SqlConnection(ConStr))
                        {
                            CN.Open();
                            SqlCommand CheckPav = new SqlCommand("SELECT COUNT(*) FROM Malls WHERE MallName = @MN AND PavilionID = @PID", CN);
                            CheckPav.Parameters.AddWithValue("@MN", MN.Text);
                            CheckPav.Parameters.AddWithValue("@PID", PN.Text);
                            int Check = (int)CheckPav.ExecuteScalar();
                            if (Check > 0)
                            {
                                SqlCommand DeletePav = new SqlCommand("UPDATE Pavilions SET Status = 'Удален' WHERE MallName = @MN AND PavilionID = @PID", CN);
                                DeletePav.Parameters.AddWithValue("@MN", MN.Text);
                                DeletePav.Parameters.AddWithValue("@PID", PN.Text);
                                DeletePav.ExecuteNonQuery();
                                NavigationService.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("Такого ТЦ/павильона не существует");
                            }
                            CN.Close();
                        }
                    }
                    else MessageBox.Show("Вы не ввели название павильона");
                }
            }
            else MessageBox.Show("Вы не ввели название ТЦ");
        }

        private void ViewMP(object sender, RoutedEventArgs e)
        {
            if (MN.Text != null)
            {
                if (MenuID == 1)
                {
                    using (SqlConnection CN = new SqlConnection(ConStr))
                    {
                        CN.Open();
                        SqlCommand CheckMall = new SqlCommand("SELECT COUNT(*) FROM Malls WHERE MallName = @MN", CN);
                        CheckMall.Parameters.AddWithValue("@MN", MN.Text);
                        int Check = (int)CheckMall.ExecuteScalar();
                        if (Check > 0)
                        {
                            NavigationService.Navigate(new InterfaceMalls(MN.Text, 3));
                        }
                        else
                        {
                            MessageBox.Show("Такого ТЦ/павильона не существует");
                        }
                        CN.Close();
                    }
                }
                else
                {
                    if (PN.Text != null)
                    {
                        using (SqlConnection CN = new SqlConnection(ConStr))
                        {
                            CN.Open();
                            SqlCommand CheckPav = new SqlCommand("SELECT COUNT(*) FROM Malls WHERE MallName = @MN AND PavilionID = @ PN", CN);
                            CheckPav.Parameters.AddWithValue("@MN", MN.Text);
                            CheckPav.Parameters.AddWithValue("@PID", PN.Text);
                            int Check = (int)CheckPav.ExecuteScalar();
                            if (Check > 0)
                            {
                                NavigationService.Navigate(new InterfacePavilions(MN.Text, PN.Text, 3));
                            }
                            else
                            {
                                MessageBox.Show("Такого ТЦ/павильона не существует");
                            }
                            CN.Close();
                        }
                    }
                    else MessageBox.Show("Вы не ввели название павильона");
                }
            }
            else MessageBox.Show("Вы не ввели название ТЦ");
        }
    }
}