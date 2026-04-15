using System;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormKasirMenu : Form
    {
        private string currentUsername;
        private string currentRole;

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

        // ===============================
        // BUTTON LIHAT PRODUK (READ ONLY)
        // ===============================
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FormProduk produk = new FormProduk(true, currentUsername, currentRole);
            produk.Show();
            this.Hide();
        }

        // ===============================
        // BUTTON TRANSAKSI
        // ===============================
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // sementara placeholder dulu (karena FormTransaksi belum dibuat)
            MessageBox.Show("Form Transaksi belum dibuat.");

            // nanti ganti dengan:
            // FormTransaksi trx = new FormTransaksi(currentUsername);
            // trx.Show();
            // this.Hide();
        }

        // ===============================
        // BUTTON LOGOUT
        // ===============================
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Close();
        }
    }
}