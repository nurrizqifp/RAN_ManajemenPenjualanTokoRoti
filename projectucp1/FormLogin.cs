using System;
using System.Data;
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
            if (string.IsNullOrWhiteSpace(TxtBoxUsername.Text) || string.IsNullOrWhiteSpace(TxtBoxPassword.Text))
            {
                MessageBox.Show("Username dan password tidak boleh kosong", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    SqlCommand cmd = new SqlCommand("dbo.sp_Login_Safe", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter pUsername = new SqlParameter("@username", SqlDbType.VarChar, 50);
                    pUsername.Value = TxtBoxUsername.Text;
                    cmd.Parameters.Add(pUsername);

                    SqlParameter pPassword = new SqlParameter("@password", SqlDbType.VarChar, 50);
                    pPassword.Value = TxtBoxPassword.Text;
                    cmd.Parameters.Add(pPassword);

                    SqlParameter pRole = new SqlParameter("@outRole", SqlDbType.VarChar, 20);
                    pRole.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pRole);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    object roleObj = pRole.Value;
                    if (roleObj == null || roleObj is DBNull)
                    {
                        MessageBox.Show("Username atau password salah", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TxtBoxPassword.Clear();
                        TxtBoxUsername.Focus();
                        return;
                    }

                    string r = roleObj.ToString().ToLower();

                    if (r == "admin")
                    {
                        new FormAdminMenu(TxtBoxUsername.Text, r).Show();
                    }
                    else if (r == "kasir")
                    {
                        new FormKasirMenu(TxtBoxUsername.Text, r).Show();
                    }
                    else
                    {
                        MessageBox.Show("Role tidak dikenali", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show("Yakin ingin keluar dari aplikasi?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void LblWelcome_Click(object sender, EventArgs e)
        {
        }
    }
}