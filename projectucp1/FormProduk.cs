using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormProduk : Form
    {
        readonly string con = "Data Source=MSI;Initial Catalog=TOKO_ROTI;Integrated Security=True";

        public FormProduk()
        {
            InitializeComponent();
        }

        private void FormProduk_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            LoadData();
        }

        void LoadData()
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

            txtNamaProduk.Text = row.Cells["namaProduk"].Value?.ToString();
            txtHarga.Text = row.Cells["harga"].Value?.ToString();
            txtStok.Text = row.Cells["stok"].Value?.ToString();
        }

        private void BtnTambah_Click(object sender, EventArgs e)
        {
            if (txtNamaProduk.Text == "" || txtHarga.Text == "" || txtStok.Text == "")
            {
                MessageBox.Show("Semua field harus diisi");
                return;
            }

            if (!decimal.TryParse(txtHarga.Text, out decimal harga) ||
                !int.TryParse(txtStok.Text, out int stok) || stok < 0)
            {
                MessageBox.Show("Input tidak valid / stok tidak boleh negatif");
                return;
            }

            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();

                string query = "INSERT INTO produk (namaProduk, harga, stok) VALUES (@nama, @harga, @stok)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@nama", txtNamaProduk.Text);
                cmd.Parameters.AddWithValue("@harga", harga);
                cmd.Parameters.AddWithValue("@stok", stok);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Data berhasil ditambahkan");
            LoadData();
            ClearForm();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Pilih data terlebih dahulu");
                return;
            }

            if (!decimal.TryParse(txtHarga.Text, out decimal harga) ||
                !int.TryParse(txtStok.Text, out int stok) || stok < 0)
            {
                MessageBox.Show("Input tidak valid / stok tidak boleh negatif");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["produkID"].Value);

            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();

                string query = @"UPDATE produk 
                                 SET namaProduk=@nama, harga=@harga, stok=@stok 
                                 WHERE produkID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nama", txtNamaProduk.Text);
                cmd.Parameters.AddWithValue("@harga", harga);
                cmd.Parameters.AddWithValue("@stok", stok);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Data berhasil diupdate");
            LoadData();
        }

        private void BtnHapus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Pilih data terlebih dahulu");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["produkID"].Value);

            if (MessageBox.Show("Yakin hapus data?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();

                string query = "DELETE FROM produk WHERE produkID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Data berhasil dihapus");
            LoadData();
            ClearForm();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        void ClearForm()
        {
            txtNamaProduk.Clear();
            txtHarga.Clear();
            txtStok.Clear();
        }

        private void TxtStok_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}