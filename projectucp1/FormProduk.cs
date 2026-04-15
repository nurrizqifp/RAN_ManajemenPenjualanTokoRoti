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
        private int selectedprodukId = -1;

        public FormProduk(bool readOnly, string username, string role)
        {
            InitializeComponent();
            isReadOnly = readOnly;
            currentUsername = username;
            currentRole = role;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
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
                try
                {
                    conn.Open();
                    string query = "SELECT produkID, namaProduk, harga, stok FROM produk";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat data produk: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            if (row.Cells[0].Value != null)
                selectedprodukId = Convert.ToInt32(row.Cells[0].Value);

            txtNamaProduk.Text = row.Cells[1].Value?.ToString();
            txtHarga.Text = row.Cells[2].Value?.ToString();
            txtStok.Text = row.Cells[3].Value?.ToString();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (isReadOnly)
            {
                MessageBox.Show("Kasir tidak memiliki akses tambah produk.");
                return;
            }

            string nama = txtNamaProduk.Text.Trim();

            if (string.IsNullOrWhiteSpace(nama))
            {
                MessageBox.Show("Nama produk wajib diisi.");
                return;
            }

            if (!decimal.TryParse(txtHarga.Text.Trim(), out decimal harga))
            {
                MessageBox.Show("Harga tidak valid.");
                return;
            }

            if (!int.TryParse(txtStok.Text.Trim(), out int stok))
            {
                MessageBox.Show("Stok tidak valid.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(con))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO produk (namaProduk, harga, stok) VALUES (@nama, @harga, @stok)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama", nama);
                        cmd.Parameters.AddWithValue("@harga", harga);
                        cmd.Parameters.AddWithValue("@stok", stok);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Produk berhasil ditambahkan.");
                    LoadData();
                    ClearInput();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menambah produk: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (isReadOnly)
            {
                MessageBox.Show("Kasir tidak memiliki akses update produk.");
                return;
            }

            if (selectedprodukId == -1)
            {
                MessageBox.Show("Pilih produk yang akan diupdate.");
                return;
            }

            string nama = txtNamaProduk.Text.Trim();

            if (string.IsNullOrWhiteSpace(nama))
            {
                MessageBox.Show("Nama produk wajib diisi.");
                return;
            }

            if (!decimal.TryParse(txtHarga.Text.Trim(), out decimal harga))
            {
                MessageBox.Show("Harga tidak valid.");
                return;
            }

            if (!int.TryParse(txtStok.Text.Trim(), out int stok))
            {
                MessageBox.Show("Stok tidak valid.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(con))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE produk
                                     SET namaProduk = @nama,
                                         harga = @harga,
                                         stok = @stok
                                     WHERE produkID = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedprodukId);
                        cmd.Parameters.AddWithValue("@nama", nama);
                        cmd.Parameters.AddWithValue("@harga", harga);
                        cmd.Parameters.AddWithValue("@stok", stok);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Produk berhasil diupdate.");
                    LoadData();
                    ClearInput();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal update produk: " + ex.Message);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (isReadOnly)
            {
                MessageBox.Show("Kasir tidak memiliki akses hapus produk.");
                return;
            }

            if (selectedprodukId == -1)
            {
                MessageBox.Show("Pilih produk yang akan dihapus.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Yakin ingin menghapus produk ini?",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(con))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM produk WHERE produkID = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedprodukId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Produk berhasil dihapus.");
                    LoadData();
                    ClearInput();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menghapus produk: " + ex.Message);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (isReadOnly)
            {
                FormLogin login = new FormLogin();
                login.Show();
                this.Close();
                return;
            }

            FormAdminMenu adminMenu = new FormAdminMenu(currentUsername, currentRole);
            adminMenu.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void ClearInput()
        {
            selectedprodukId = -1;
            txtNamaProduk.Clear();
            txtHarga.Clear();
            txtStok.Clear();
        }
    }
}