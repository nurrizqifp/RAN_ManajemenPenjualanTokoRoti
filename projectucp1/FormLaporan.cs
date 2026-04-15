using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace projectucp1
{
    public partial class FormLaporan : Form
    {
        private readonly string con = "Data Source=MSI;Initial Catalog=TOKO_ROTI;Integrated Security=True";
        private readonly string username;

        public FormLaporan(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void FormLaporan_Load(object sender, EventArgs e)
        {
            LoadTransaksi();
        }

        void LoadTransaksi()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            t.transaksiID,
                            t.tanggal,
                            t.username,
                            COUNT(dt.produkID) AS jumlahProduk,
                            SUM(p.harga * dt.qty) AS total
                        FROM transaksi t
                        LEFT JOIN detail_transaksi dt ON t.transaksiID = dt.transaksiID
                        LEFT JOIN produk p ON dt.produkID = p.produkID
                        GROUP BY t.transaksiID, t.tanggal, t.username
                        ORDER BY t.tanggal DESC";

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

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih transaksi untuk melihat detail", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int transaksiID = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            dt.transaksiID,
                            dt.produkID,
                            p.namaProduk,
                            p.harga,
                            dt.qty,
                            (p.harga * dt.qty) AS subtotal
                        FROM detail_transaksi dt
                        JOIN produk p ON dt.produkID = p.produkID
                        WHERE dt.transaksiID = @tid
                        ORDER BY dt.produkID";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@tid", transaksiID);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    Form detailForm = new Form();
                    detailForm.Text = $"Detail Transaksi #{transaksiID}";
                    detailForm.Width = 600;
                    detailForm.Height = 400;
                    detailForm.StartPosition = FormStartPosition.CenterParent;

                    DataGridView dgv = new DataGridView();
                    dgv.DataSource = dt;
                    dgv.Dock = DockStyle.Fill;
                    dgv.AutoResizeColumns();
                    detailForm.Controls.Add(dgv);

                    detailForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTransaksi();
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            new FormAdminMenu(username, "admin").Show();
        }
    }
}
