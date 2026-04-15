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

        public FormProduk(bool readOnly, string user, string role)
        {
            InitializeComponent();
            this.readOnly = readOnly;
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
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO produk VALUES (@n,@h,@s)", conn);
                cmd.Parameters.AddWithValue("@n", txtNamaProduk.Text);
                cmd.Parameters.AddWithValue("@h", decimal.Parse(txtHarga.Text));
                cmd.Parameters.AddWithValue("@s", int.Parse(txtStok.Text));
                cmd.ExecuteNonQuery();
            }
            LoadData();
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
            if (dataGridView1.SelectedRows.Count > 0)
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
                LoadData();
            }
        }

        private void BtnHapus_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = (int)row.Cells[0].Value;

                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    var cmd = new SqlCommand("DELETE FROM produk WHERE produkID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                LoadData();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            txtNamaProduk.Clear();
            txtHarga.Clear();
            txtStok.Clear();
        }
    }
}