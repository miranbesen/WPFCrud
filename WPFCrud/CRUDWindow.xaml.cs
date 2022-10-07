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
using System.Windows.Shapes;

namespace WPFCrud
{
    /// <summary>
    /// Interaction logic for CRUDWindow.xaml
    /// </summary>
    public partial class CRUDWindow : Window
    {
        public CRUDWindow()
        {
            InitializeComponent();
            LoadGrid();
        }
        SqlConnection con = new SqlConnection(@"Data Source=KEGM_HARITA;Initial Catalog=New_DB;Integrated Security=True");

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select * from FirsT", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;


        }

        public void clearData()
        {
            name_txt.Clear();
            age_txt.Clear();
            gender_txt.Clear();
            city_txt.Clear();
            search_txt.Clear();
        }
        private void ClearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        public bool isValid()
        {
            if (name_txt.Text == String.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (age_txt.Text == String.Empty)
            {
                MessageBox.Show("Age is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (gender_txt.Text == String.Empty)
            {
                MessageBox.Show("Gender is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (city_txt.Text == String.Empty)
            {
                MessageBox.Show("City is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO FirsT VALUES (@Name, @Age, @Gender, @City)");
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", name_txt.Text);
                    cmd.Parameters.AddWithValue("@Age", age_txt.Text);
                    cmd.Parameters.AddWithValue("@Gender", gender_txt.Text);
                    cmd.Parameters.AddWithValue("@City", city_txt.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadGrid();
                    MessageBox.Show("Successfully registered", "Saved", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    clearData();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void DeeteBtn_Click_1(object sender, RoutedEventArgs e)
        {
            //Silinecek Id eger yok ise hata mesajı ekrana basması için yazıldı.
            SqlCommand cmd = new SqlCommand("select * from FirsT", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;
            var TableList = new List<string>();
            bool bayrak = false;

            foreach (DataColumn dc in dt.Columns)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dc.ColumnName == "Id")
                    {
                        TableList.Add(dr[dc].ToString());
                    }

                }
            }
            //------------------------------------------------------------------------------

            con.Open();
            SqlCommand cmd2 = new SqlCommand("delete from FirsT where ID=" + search_txt.Text + " ", con);
            try
            {
                if (TableList.Count > 0)
                {
                    for (int i = 0; i < TableList.Count(); i++)
                    {
                        if (TableList[i] == search_txt.Text)
                        {
                            bayrak = true;
                            i = TableList.Count();
                        }
                        else
                        {
                            bayrak = false;
                        }

                    }
                }
                else
                {
                    MessageBox.Show("silinecek eleman yok", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (bayrak == true)
                {
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("Record has been delete successfully", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show("silinecek eleman yok", "Failed", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                clearData();
                LoadGrid();
            }

        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update FirsT set Name='" + name_txt.Text + "',Age='" + age_txt.Text + "',Gender='" + gender_txt.Text + "',City='" + city_txt.Text + "' WHERE ID='" + search_txt.Text + "'", con);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record has been updated successfully", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                clearData();
                LoadGrid();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow dashboard = new MainWindow();
            dashboard.Show();
            this.Close();
        }
    }
}
