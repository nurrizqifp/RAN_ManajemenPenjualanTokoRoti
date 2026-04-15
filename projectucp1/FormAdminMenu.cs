using System;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormAdminMenu : Form
    {
        private readonly string currentUsername;
        private readonly string currentRole;

        public FormAdminMenu(string username, string role)
        {
            InitializeComponent();
            currentUsername = username;
            currentRole = role;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LblUser.Text = $"User: {currentUsername} ({currentRole})";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FormProduk formProduk = new FormProduk(false, currentUsername, currentRole);
            formProduk.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Close();
        }
    }
}