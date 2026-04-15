using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormKasirManagement : Form
    {
        private readonly string con = "Data Source=MSI;Initial Catalog=TOKO_ROTI;Integrated Security=True";
        private readonly string adminUsername;

        public FormKasirManagement(string admin)
        {
            InitializeComponent();
            adminUsername = admin;
        }

        private void FormKasirManagement_Load(object sender, EventArgs e)
        {
            LoadKasir();
        }

        void LoadKasir()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    string query = "SELECT loginID, username, role FROM login WHERE role='kasir' ORDER BY username";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username tidak boleh kosong", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password tidak boleh kosong", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtUsername.Text.Length < 3)
            {
                MessageBox.Show("Username minimal 3 karakter", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtPassword.Text.Length < 3)
            {
                MessageBox.Show("Password minimal 3 karakter", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            dataGridView1.ClearSelection();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    
                    // Check if username already exists
                    SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM login WHERE username=@u", conn);
                    check.Parameters.AddWithValue("@u", txtUsername.Text);
                    int count = (int)check.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Username sudah terdaftar", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    SqlCommand cmd = new SqlCommand("INSERT INTO login (username, password, role) VALUES (@u,@p,'kasir')", conn);
                    cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@p", txtPassword.Text);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Kasir berhasil ditambah", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadKasir();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih kasir yang akan diupdate", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput())
                return;

            try
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE login SET username=@u, password=@p WHERE loginID=@id", conn);
                    cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@p", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Kasir berhasil diupdate", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadKasir();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih kasir yang akan dihapus", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Yakin ingin menghapus kasir ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;

                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM login WHERE loginID=@id AND role='kasir'", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Kasir berhasil dihapus", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadKasir();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtUsername.Text = row.Cells["username"].Value?.ToString() ?? "";
                txtPassword.Text = ""; // Don't show existing password
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadKasir();
            ClearInputs();
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormAdminMenu(adminUsername, "admin").Show();
        }
    }
}
