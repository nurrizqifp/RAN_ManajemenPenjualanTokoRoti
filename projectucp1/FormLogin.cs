using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormLogin : Form
    {
        private readonly string con = "Data Source=MSI;Initial Catalog=TOKO_ROTI;Integrated Security=True";

        public FormLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();

                string query = "SELECT role FROM login WHERE username=@u AND password=@p";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@u", TxtBoxUsername.Text);
                cmd.Parameters.AddWithValue("@p", TxtBoxPassword.Text);

                var role = cmd.ExecuteScalar();

                if (role == null)
                {
                    MessageBox.Show("Login gagal");
                    return;
                }

                string r = role.ToString().ToLower();

                if (r == "admin")
                {
                    new FormAdminMenu(TxtBoxUsername.Text, r).Show();
                }
                else
                {
                    new FormKasirMenu(TxtBoxUsername.Text, r).Show();
                }

                this.Hide();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Label click handler
        }
    }
}