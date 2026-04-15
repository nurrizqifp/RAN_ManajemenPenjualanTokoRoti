using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormTransaksi : Form
    {
        private readonly string con = "Data Source=MSI;Initial Catalog=TOKO_ROTI;Integrated Security=True";
        private readonly string username;

        DataTable keranjang = new DataTable();

        public FormTransaksi(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void FormTransaksi_Load(object sender, EventArgs e)
        {
            keranjang.Columns.Add("produkID");
            keranjang.Columns.Add("nama");
            keranjang.Columns.Add("harga");
            keranjang.Columns.Add("qty");

            dgvKeranjang.DataSource = keranjang;

            LoadProduk();
            UpdateTotal();
        }

        void LoadProduk()
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM produk WHERE stok > 0 ORDER BY namaProduk", conn);
                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    cmbProduk.Items.Add(r["produkID"] + " - " + r["namaProduk"]);
                }
            }
        }

        private int GetProductStock(int produkID)
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT stok FROM produk WHERE produkID=@id", conn);
                cmd.Parameters.AddWithValue("@id", produkID);
                var result = cmd.ExecuteScalar();
                return result != null ? (int)result : 0;
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (cmbProduk.SelectedIndex < 0)
            {
                MessageBox.Show("Pilih produk terlebih dahulu", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Jumlah harus berupa angka positif", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string[] split = cmbProduk.Text.Split('-');
                int id = int.Parse(split[0].Trim());

                int availableStock = GetProductStock(id);
                if (qty > availableStock)
                {
                    MessageBox.Show($"Stok tidak cukup. Stok tersedia: {availableStock}", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM produk WHERE produkID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    var r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        keranjang.Rows.Add(
                            id,
                            r["namaProduk"],
                            r["harga"],
                            qty
                        );
                    }
                }

                txtQty.Clear();
                cmbProduk.SelectedIndex = -1;
                UpdateTotal();
                MessageBox.Show("Produk berhasil ditambah ke keranjang", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (keranjang.Rows.Count == 0)
            {
                MessageBox.Show("Keranjang kosong, tambah produk terlebih dahulu", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtBayar.Text, out decimal bayar) || bayar <= 0)
            {
                MessageBox.Show("Jumlah pembayaran tidak valid", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal total = 0;
            foreach (DataRow row in keranjang.Rows)
            {
                total += decimal.Parse(row["harga"].ToString()) * int.Parse(row["qty"].ToString());
            }

            if (bayar < total)
            {
                MessageBox.Show($"Pembayaran kurang. Total: {total}, Bayar: {bayar}", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Lanjutkan transaksi?\nTotal: {total}\nBayar: {bayar}", "Konfirmasi Transaksi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO transaksi(tanggal, username) OUTPUT INSERTED.transaksiID VALUES(GETDATE(),@u)",
                        conn, trans);

                    cmd.Parameters.AddWithValue("@u", username);
                    int transaksiID = (int)cmd.ExecuteScalar();

                    foreach (DataRow row in keranjang.Rows)
                    {
                        SqlCommand d = new SqlCommand(
                            "INSERT INTO detail_transaksi (transaksiID, produkID, qty) VALUES (@tid,@pid,@qty)",
                            conn, trans);

                        d.Parameters.AddWithValue("@tid", transaksiID);
                        d.Parameters.AddWithValue("@pid", row["produkID"]);
                        d.Parameters.AddWithValue("@qty", row["qty"]);
                        d.ExecuteNonQuery();

                        SqlCommand u = new SqlCommand(
                            "UPDATE produk SET stok = stok - @q WHERE produkID=@id",
                            conn, trans);

                        u.Parameters.AddWithValue("@q", row["qty"]);
                        u.Parameters.AddWithValue("@id", row["produkID"]);
                        u.ExecuteNonQuery();
                    }

                    trans.Commit();
                    MessageBox.Show("Transaksi berhasil disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Gagal transaksi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbProduk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduk.SelectedIndex >= 0)
            {
                string[] split = cmbProduk.Text.Split('-');
                if (split.Length > 0 && int.TryParse(split[0].Trim(), out int id))
                {
                    using (SqlConnection conn = new SqlConnection(con))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("SELECT harga FROM produk WHERE produkID=@id", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        var harga = cmd.ExecuteScalar();
                        if (harga != null)
                        {
                            txtHarga.Text = harga.ToString();
                        }
                    }
                }
            }
        }

        private void txtBayar_TextChanged(object sender, EventArgs e)
        {
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = 0;

            foreach (DataRow row in keranjang.Rows)
            {
                decimal harga = decimal.Parse(row["harga"].ToString());
                int qty = int.Parse(row["qty"].ToString());
                total += harga * qty;
            }

            lblTotal.Text = $"Total: {total:C}";

            decimal bayar = 0;
            if (decimal.TryParse(txtBayar.Text, out bayar))
            {
                decimal kembalian = bayar - total;
                lblKembalian.Text = $"Kembalian: {kembalian:C}";
            }
        }

        private void ResetForm()
        {
            keranjang.Clear();
            cmbProduk.Items.Clear();
            cmbProduk.SelectedIndex = -1;
            txtHarga.Clear();
            txtQty.Clear();
            txtBayar.Clear();
            LoadProduk();
            UpdateTotal();
        }
    }
}