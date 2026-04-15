using System;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormAdminMenu : Form
    {
        private readonly string username;
        private readonly string role;

        public FormAdminMenu(string user, string role)
        {
            InitializeComponent();
            username = user;
            this.role = role;
        }

        private void FormAdminMenu_Load(object sender, EventArgs e)
        {
            LblUser.Text = $"{username} ({role})";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            FormAdminMenu_Load(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new FormProduk(false, username, role).Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new FormLogin().Show();
            this.Hide();
        }
    }
}