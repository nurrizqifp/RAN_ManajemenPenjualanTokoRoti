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
        }

        void LoadProduk()
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM produk", conn);
                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    cmbProduk.Items.Add(r["produkID"] + " - " + r["namaProduk"]);
                }
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            string[] split = cmbProduk.Text.Split('-');
            int id = int.Parse(split[0]);

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
                        int.Parse(txtQty.Text)
                    );
                }
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
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
                            "INSERT INTO detail_transaksi VALUES(@tid,@pid,@qty)",
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
                    MessageBox.Show("Transaksi berhasil");
                }
                catch
                {
                    trans.Rollback();
                    MessageBox.Show("Gagal transaksi");
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
            decimal total = 0;
            decimal bayar = 0;

            foreach (DataRow row in keranjang.Rows)
            {
                decimal harga = decimal.Parse(row["harga"].ToString());
                int qty = int.Parse(row["qty"].ToString());
                total += harga * qty;
            }

            if (decimal.TryParse(txtBayar.Text, out bayar))
            {
                decimal kembalian = bayar - total;
                lblKembalian.Text = $"Kembalian: {kembalian}";
            }

            lblTotal.Text = $"Total: {total}";
        }
    }
}