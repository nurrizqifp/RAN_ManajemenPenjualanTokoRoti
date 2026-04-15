using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormTransaksi : Form
    {
        string con = "Data Source=MSI;Initial Catalog=TOKO_ROTI;Integrated Security=True";

        DataTable keranjang = new DataTable();
        decimal totalHarga = 0;
        int kasirID;

        public FormTransaksi(int kasirID)
        {
            InitializeComponent();
            this.kasirID = kasirID;
        }

        private void FormTransaksi_Load(object sender, EventArgs e)
        {
            LoadProduk();
            InitKeranjang();
        }

        void LoadProduk()
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT produkID, namaProduk, harga FROM produk", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbProduk.DataSource = dt;
                cmbProduk.DisplayMember = "namaProduk";
                cmbProduk.ValueMember = "produkID";
            }
        }

        void InitKeranjang()
        {
            keranjang.Columns.Add("produkID", typeof(int));
            keranjang.Columns.Add("namaProduk", typeof(string));
            keranjang.Columns.Add("harga", typeof(decimal));
            keranjang.Columns.Add("qty", typeof(int));
            keranjang.Columns.Add("subtotal", typeof(decimal));

            dgvKeranjang.DataSource = keranjang;
        }

        private void cmbProduk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduk.SelectedItem is DataRowView row)
                txtHarga.Text = row["harga"].ToString();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Qty tidak valid");
                return;
            }

            int produkID = Convert.ToInt32(cmbProduk.SelectedValue);
            string nama = cmbProduk.Text;
            decimal harga = decimal.Parse(txtHarga.Text);

            decimal subtotal = harga * qty;

            keranjang.Rows.Add(produkID, nama, harga, qty, subtotal);

            totalHarga += subtotal;
            lblTotal.Text = "Total: " + totalHarga.ToString("N0");
        }

        private void txtBayar_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtBayar.Text, out decimal bayar))
            {
                decimal kembali = bayar - totalHarga;
                lblKembalian.Text = "Kembalian: " + kembali.ToString("N0");
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
                        "INSERT INTO transaksi (tanggal, totalHarga, kasirID) OUTPUT INSERTED.transaksiID VALUES (GETDATE(), @total, @kasir)",
                        conn, trans);

                    cmd.Parameters.AddWithValue("@total", totalHarga);
                    cmd.Parameters.AddWithValue("@kasir", kasirID);

                    int transaksiID = (int)cmd.ExecuteScalar();

                    foreach (DataRow row in keranjang.Rows)
                    {
                        SqlCommand detail = new SqlCommand(
                            "INSERT INTO detailTransaksi (transaksiID, produkID, jumlah, hargaSatuan, total) VALUES (@tid,@pid,@qty,@harga,@total)",
                            conn, trans);

                        detail.Parameters.AddWithValue("@tid", transaksiID);
                        detail.Parameters.AddWithValue("@pid", row["produkID"]);
                        detail.Parameters.AddWithValue("@qty", row["qty"]);
                        detail.Parameters.AddWithValue("@harga", row["harga"]);
                        detail.Parameters.AddWithValue("@total", row["subtotal"]);
                        detail.ExecuteNonQuery();

                        SqlCommand stok = new SqlCommand(
                            "UPDATE produk SET stok = stok - @qty WHERE produkID=@pid",
                            conn, trans);

                        stok.Parameters.AddWithValue("@qty", row["qty"]);
                        stok.Parameters.AddWithValue("@pid", row["produkID"]);
                        stok.ExecuteNonQuery();
                    }

                    trans.Commit();
                    MessageBox.Show("Transaksi berhasil");

                    keranjang.Rows.Clear();
                    totalHarga = 0;
                    lblTotal.Text = "Total: 0";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}