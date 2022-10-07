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
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=KEGM_HARITA;Initial Catalog=New_DB;Integrated Security=True");

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                String query = "SELECT COUNT(1) FROM LoginUser WHERE UserName=@UserName AND Password=@Password";
                SqlCommand cmd= new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Username", Username_txt.Text);
                cmd.Parameters.AddWithValue("@Password", Password_txt.Password);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 1)
                {
                    MainWindow dashboard =new MainWindow();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username or password is incorrect");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
