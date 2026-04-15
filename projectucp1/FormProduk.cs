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
                conn.Open();
                string query = "SELECT produkID, namaProduk, harga, stok FROM produk";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            txtNamaProduk.Text = row.Cells["namaProduk"].Value?.ToString();
            txtHarga.Text = row.Cells["harga"].Value?.ToString();
            txtStok.Text = row.Cells["stok"].Value?.ToString();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (txtNamaProduk.Text == "" || txtHarga.Text == "" || txtStok.Text == "")
            {
                MessageBox.Show("Semua field harus diisi");
                return;
            }

            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();

                string query = "INSERT INTO produk (namaProduk, harga, stok) VALUES (@nama, @harga, @stok)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nama", txtNamaProduk.Text);
                cmd.Parameters.AddWithValue("@harga", decimal.Parse(txtHarga.Text));
                cmd.Parameters.AddWithValue("@stok", int.Parse(txtStok.Text));

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Data berhasil ditambahkan");
            LoadData();
            ClearForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Pilih data terlebih dahulu");
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
                cmd.Parameters.AddWithValue("@harga", decimal.Parse(txtHarga.Text));
                cmd.Parameters.AddWithValue("@stok", int.Parse(txtStok.Text));

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Data berhasil diupdate");
            LoadData();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Pilih data dulu");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["produkID"].Value);

            DialogResult confirm = MessageBox.Show("Yakin hapus data?", "Konfirmasi", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.No) return;

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

        private void ClearForm()
        {
            txtNamaProduk.Clear();
            txtHarga.Clear();
            txtStok.Clear();
        }
    }
}