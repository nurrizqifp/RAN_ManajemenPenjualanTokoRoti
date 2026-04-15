using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormProduk : Form
    {
        private readonly string con = "Data Source=MSI;Initial Catalog=TOKO_ROTI;Integrated Security=True";
        private readonly bool readOnly;
        private readonly string username;
        private readonly string role;

        public FormProduk(bool readOnly, string user, string role)
        {
            InitializeComponent();
            this.readOnly = readOnly;
            this.username = user;
            this.role = role;
        }

        private void FormProduk_Load(object sender, EventArgs e)
        {
            LoadData();

            if (readOnly)
            {
                btnTambah.Enabled = false;
                btnUpdate.Enabled = false;
                btnHapus.Enabled = false;
            }
        }

        void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                var da = new SqlDataAdapter("SELECT * FROM produk", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void BtnTambah_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    var cmd = new SqlCommand("INSERT INTO produk (namaProduk, harga, stok) VALUES (@n,@h,@s)", conn);
                    cmd.Parameters.AddWithValue("@n", txtNamaProduk.Text);
                    cmd.Parameters.AddWithValue("@h", decimal.Parse(txtHarga.Text));
                    cmd.Parameters.AddWithValue("@s", int.Parse(txtStok.Text));
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Produk berhasil ditambah", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtNamaProduk.Text = row.Cells["namaProduk"].Value?.ToString() ?? "";
                txtHarga.Text = row.Cells["harga"].Value?.ToString() ?? "";
                txtStok.Text = row.Cells["stok"].Value?.ToString() ?? "";
            }
        }

        private void TxtStok_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih satu produk yang akan diupdate", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput())
                return;

            try
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = (int)row.Cells[0].Value;

                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    var cmd = new SqlCommand("UPDATE produk SET namaProduk=@n, harga=@h, stok=@s WHERE produkID=@id", conn);
                    cmd.Parameters.AddWithValue("@n", txtNamaProduk.Text);
                    cmd.Parameters.AddWithValue("@h", decimal.Parse(txtHarga.Text));
                    cmd.Parameters.AddWithValue("@s", int.Parse(txtStok.Text));
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Produk berhasil diupdate", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHapus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih minimal 1 data untuk dihapus", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Konfirmasi sebelum hapus
            string message = dataGridView1.SelectedRows.Count == 1 
                ? "Yakin ingin menghapus produk ini?" 
                : $"Yakin ingin menghapus {dataGridView1.SelectedRows.Count} produk?";

            DialogResult result = MessageBox.Show(message, "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();

                    // Loop setiap selected row dan hapus satu per satu
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        if (row.IsNewRow) continue; // Skip row baru yang belum tersimpan

                        int id = (int)row.Cells[0].Value;

                        SqlCommand cmd = new SqlCommand("DELETE FROM produk WHERE produkID=@id", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                int deletedCount = dataGridView1.SelectedRows.Count;
                MessageBox.Show($"{deletedCount} produk berhasil dihapus", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearInputs();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtNamaProduk.Text))
            {
                MessageBox.Show("Nama produk tidak boleh kosong", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtHarga.Text, out decimal harga) || harga <= 0)
            {
                MessageBox.Show("Harga harus berupa angka positif", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtStok.Text, out int stok) || stok < 0)
            {
                MessageBox.Show("Stok harus berupa angka positif atau nol", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            txtNamaProduk.Clear();
            txtHarga.Clear();
            txtStok.Clear();
            dataGridView1.ClearSelection();
        }
    }
}