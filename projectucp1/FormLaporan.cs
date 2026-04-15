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
        private readonly string role;

        public FormLaporan(string user, string role)
        {
            InitializeComponent();
            this.username = user;
            this.role = role;
        }

        private void FormLaporan_Load(object sender, EventArgs e)
        {
            LoadLaporan();
        }

        /// <summary>
        /// Load laporan transaksi detail dari database dengan JOIN
        /// Query: transaksi JOIN detail_transaksi JOIN produk
        /// </summary>
        private void LoadLaporan()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();

                    // SQL Query dengan JOIN untuk detail transaksi
                    string query = @"
                        SELECT 
                            t.transaksiID AS 'Nomor Transaksi',
                            t.tanggal AS 'Tanggal',
                            t.username AS 'Kasir',
                            p.namaProduk AS 'Nama Produk',
                            dt.qty AS 'Jumlah',
                            p.harga AS 'Harga Satuan',
                            (p.harga * dt.qty) AS 'Total'
                        FROM transaksi t
                        INNER JOIN detail_transaksi dt ON t.transaksiID = dt.transaksiID
                        INNER JOIN produk p ON dt.produkID = p.produkID
                        ORDER BY t.tanggal DESC, t.transaksiID DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dataTable;
                        dataGridView1.AutoResizeColumns();
                        lblStatus.Text = $"Total: {dataTable.Rows.Count} item transaksi";
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                        lblStatus.Text = "Belum ada data transaksi";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        /// <summary>
        /// Tombol Refresh - reload laporan dari database
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLaporan();
        }
    }
}
