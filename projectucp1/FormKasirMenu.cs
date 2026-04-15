using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormKasirMenu : Form
    {
        private string currentUsername;
        private string currentRole;
        private string con = "Data Source=MSI;Initial Catalog=TOKO_ROTI;Integrated Security=True";

        public FormKasirMenu(string username, string role)
        {
            InitializeComponent();
            currentUsername = username;
            currentRole = role;
        }

        private void FormKasirMenu_Load(object sender, EventArgs e)
        {
            LblUser.Text = $"Kasir: {currentUsername}";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FormProduk produk = new FormProduk(true, currentUsername, currentRole);
            produk.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            int kasirID = GetKasirID(currentUsername);

            FormTransaksi trx = new FormTransaksi(kasirID);
            trx.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Close();
        }

        private int GetKasirID(string username)
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();

                string query = @"SELECT k.kasirID
                                 FROM kasirMenu k
                                 JOIN login l ON k.loginID = l.loginID
                                 WHERE l.username = @user";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", username);

                object result = cmd.ExecuteScalar();

                if (result == null)
                    throw new Exception("Kasir tidak ditemukan");

                return Convert.ToInt32(result);
            }
        }
    }
}