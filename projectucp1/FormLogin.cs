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

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = TxtBoxUsername.Text.Trim();
            string password = TxtBoxPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username dan password wajib diisi.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();

                    string query = "SELECT role FROM login WHERE username = @username AND password = @password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        object result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Username atau password salah.");
                            return;
                        }

                        string role = result.ToString().Trim().ToLower();

                        if (role == "admin" || role == "produsen")
                        {
                            FormAdminMenu formAdmin = new FormAdminMenu(username, role);
                            formAdmin.Show();
                            this.Hide();
                        }
                        else if (role == "kasir")
                        {
                            FormKasirMenu kasir = new FormKasirMenu(username, role);
                            kasir.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Role tidak dikenali oleh sistem.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }
        }
    }
}