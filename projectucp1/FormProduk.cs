using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormProduk : Form
    {
        private readonly string con = "Data Source=MSI;Initial Catalog=TOKO_ROTI;Integrated Security=True";
        private readonly bool isReadOnly;
        private readonly string currentUsername;
        private readonly string currentRole;
        private int selectedProductId = -1;

        public FormProduk()
        {
            InitializeComponent();
        }

        public FormProduk(bool readOnly, string username, string role)
        {
            InitializeComponent();
            isReadOnly = readOnly;
            currentUsername = username;
            currentRole = role;
        }

        private void FormProduk_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            LoadData();

            if (isReadOnly)
            {
                btnTambah.Enabled = false;
                btnUpdate.Enabled = false;
                btnHapus.Enabled = false;

                txtNamaProduk.ReadOnly = true;
                txtHarga.ReadOnly = true;
                txtStok.ReadOnly = true;
            }
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                string query = "SELECT produkID, namaProduk, harga, stok FROM produk";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            selectedProductId = Convert.ToInt32(row.Cells["produkID"].Value);

            txtNamaProduk.Text = row.Cells["namaProduk"].Value?.ToString();
            txtHarga.Text = row.Cells["harga"].Value?.ToString();
            txtStok.Text = row.Cells["stok"].Value?.ToString();
        }

        private void BtnTambah_Click(object sender, EventArgs e)
        {
            if (isReadOnly)
            {
                MessageBox.Show("Kasir tidak memiliki akses tambah produk.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNamaProduk.Text))
            {
                MessageBox.Show("Nama produk wajib diisi.");
                return;
            }

            if (!decimal.TryParse(txtHarga.Text, out decimal harga))
            {
                MessageBox.Show("Harga tidak valid.");
                return;
            }

            if (!int.TryParse(txtStok.Text, out int stok) || stok < 0)
            {
                MessageBox.Show("Stok tidak valid atau tidak boleh negatif.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                string query = "INSERT INTO produk (namaProduk, harga, stok) VALUES (@nama, @harga, @stok)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nama", txtNamaProduk.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", harga);
                    cmd.Parameters.AddWithValue("@stok", stok);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Data berhasil ditambahkan.");
            LoadData();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (isReadOnly)
            {
                MessageBox.Show("Kasir tidak memiliki akses update produk.");
                return;
            }

            if (selectedProductId == -1 && dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Pilih data terlebih dahulu.");
                return;
            }

            if (!decimal.TryParse(txtHarga.Text, out decimal harga))
            {
                MessageBox.Show("Harga tidak valid.");
                return;
            }

            if (!int.TryParse(txtStok.Text, out int stok) || stok < 0)
            {
                MessageBox.Show("Stok tidak valid atau tidak boleh negatif.");
                return;
            }

            int id = selectedProductId != -1
                ? selectedProductId
                : Convert.ToInt32(dataGridView1.CurrentRow.Cells["produkID"].Value);

            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                string query = @"UPDATE produk
                                 SET namaProduk=@nama, harga=@harga, stok=@stok
                                 WHERE produkID=@id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@nama", txtNamaProduk.Text.Trim());
                    cmd.Parameters.AddWithValue("@harga", harga);
                    cmd.Parameters.AddWithValue("@stok", stok);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Data berhasil diupdate.");
            LoadData();
        }

        private void BtnHapus_Click(object sender, EventArgs e)
        {
            if (isReadOnly)
            {
                MessageBox.Show("Kasir tidak memiliki akses hapus produk.");
                return;
            }

            if (selectedProductId == -1 && dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Pilih data terlebih dahulu.");
                return;
            }

            int id = selectedProductId != -1
                ? selectedProductId
                : Convert.ToInt32(dataGridView1.CurrentRow.Cells["produkID"].Value);

            var confirm = MessageBox.Show("Yakin hapus data?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                string query = "DELETE FROM produk WHERE produkID=@id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Data berhasil dihapus.");
            LoadData();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}