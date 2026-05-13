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
        private readonly string username;
        private readonly string role;
        private bool updatingSelection = false;
        private System.Threading.CancellationTokenSource loadCts;


        public FormProduk(bool readOnly, string user, string role)
        {
            InitializeComponent();
            this.readOnly = readOnly;
            this.username = user;
            this.role = role;
            
        }

        private async void FormProduk_Load(object sender, EventArgs e)
        {
            SetupBinding();
            loadCts?.Cancel(); loadCts?.Dispose();
            loadCts = new System.Threading.CancellationTokenSource();
            try
            {
                await LoadDataAsync(loadCts.Token);
            }
            catch (OperationCanceledException) { }

            if (readOnly)
            {
                btnTambah.Enabled = false;
                btnUpdate.Enabled = false;
                btnHapus.Enabled = false;
            }
            this.FormClosed -= FormProduk_FormClosed;
            this.FormClosed += FormProduk_FormClosed;
        }

        private void FormProduk_FormClosed(object sender, FormClosedEventArgs e)
        {
            try { loadCts?.Cancel(); } catch { }
            try { loadCts?.Dispose(); } catch { }
        }

        private void SetupBinding()
        {
            dataGridView1.DataSource = vwprodukBindingSource;
            bindingNavigator1.BindingSource = vwprodukBindingSource;

            txtNamaProduk.DataBindings.Clear();
            txtHarga.DataBindings.Clear();
            txtStok.DataBindings.Clear();
            txtNamaProduk.DataBindings.Add("Text", vwprodukBindingSource, "namaProduk", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged);
            txtHarga.DataBindings.Add("Text", vwprodukBindingSource, "harga", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged);
            txtStok.DataBindings.Add("Text", vwprodukBindingSource, "stok", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged);

            vwprodukBindingSource.PositionChanged -= VwprodukBindingSource_PositionChanged;
            vwprodukBindingSource.PositionChanged += VwprodukBindingSource_PositionChanged;
        }

        private void VwprodukBindingSource_PositionChanged(object sender, EventArgs e)
        {
            UpdateGridSelection();
        }

        private void UpdateGridSelection()
        {
            if (vwprodukBindingSource == null || vwprodukBindingSource.Position < 0)
            {
                dataGridView1.ClearSelection();
                return;
            }

            if (updatingSelection) return;

            try
            {
                updatingSelection = true;
                int pos = vwprodukBindingSource.Position;
                if (dataGridView1.Rows.Count > 0 && pos >= 0 && pos < dataGridView1.Rows.Count)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[pos].Selected = true;
                    if (dataGridView1.Rows[pos].Cells.Count > 0 &&
                        (dataGridView1.CurrentCell == null || dataGridView1.CurrentCell.RowIndex != pos))
                    {
                        this.BeginInvoke(new Action(() =>
                            dataGridView1.CurrentCell = dataGridView1.Rows[pos].Cells[0]));
                    }
                }
                else
                {
                    dataGridView1.ClearSelection();
                }
            }
            finally
            {
                updatingSelection = false;
            }
        }

        // async wrapper to perform DB fill off UI thread
        private async System.Threading.Tasks.Task LoadDataAsync(System.Threading.CancellationToken ct)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    int prevPos = vwprodukBindingSource != null ? vwprodukBindingSource.Position : -1;
                    var dt = await System.Threading.Tasks.Task.Run(() =>
                    {
                        ct.ThrowIfCancellationRequested();
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM vw_produk ORDER BY produkID", conn);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        ct.ThrowIfCancellationRequested();
                        return dataTable;
                    }, ct);

                    // marshal to UI thread to assign datasource
                    this.BeginInvoke(new Action(() =>
                    {
                        vwprodukBindingSource.DataSource = dt;
                        vwprodukBindingSource.ResetBindings(false);
                        if (dt.Rows.Count > 0)
                        {
                            if (prevPos >= 0 && prevPos < vwprodukBindingSource.Count)
                                vwprodukBindingSource.Position = prevPos;
                            else
                                vwprodukBindingSource.Position = 0;
                        }
                        else
                        {
                            dataGridView1.ClearSelection();
                        }
                    }));
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTambah_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    SqlCommand cmd = new SqlCommand("dbo.sp_InsertProduk", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter pName = new SqlParameter("@namaProduk", SqlDbType.VarChar, 100);
                    pName.Value = txtNamaProduk.Text;
                    cmd.Parameters.Add(pName);

                    SqlParameter pHarga = new SqlParameter("@harga", SqlDbType.Decimal);
                    pHarga.Value = decimal.Parse(txtHarga.Text);
                    cmd.Parameters.Add(pHarga);

                    SqlParameter pStok = new SqlParameter("@stok", SqlDbType.Int);
                    pStok.Value = int.Parse(txtStok.Text);
                    cmd.Parameters.Add(pStok);

                    SqlParameter pOutID = new SqlParameter("@outProdukID", SqlDbType.Int);
                    pOutID.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pOutID);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Produk berhasil ditambah", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    try { loadCts?.Cancel(); } catch { }
                    try { loadCts?.Dispose(); } catch { }
                    loadCts = new System.Threading.CancellationTokenSource();
                    _ = LoadDataAsync(loadCts.Token);
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                vwprodukBindingSource.Position = e.RowIndex;
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
            if (vwprodukBindingSource.Current == null)
            {
                MessageBox.Show("Pilih satu produk yang akan diupdate", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput())
                return;

            try
            {
                DataRowView rowView = (DataRowView)vwprodukBindingSource.Current;
                int id = (int)rowView["produkID"];

                using (SqlConnection conn = new SqlConnection(con))
                {
                    SqlCommand cmd = new SqlCommand("dbo.sp_UpdateProduk", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter pID = new SqlParameter("@produkID", SqlDbType.Int);
                    pID.Value = id;
                    cmd.Parameters.Add(pID);

                    SqlParameter pName = new SqlParameter("@namaProduk", SqlDbType.VarChar, 100);
                    pName.Value = txtNamaProduk.Text;
                    cmd.Parameters.Add(pName);

                    SqlParameter pHarga = new SqlParameter("@harga", SqlDbType.Decimal);
                    pHarga.Value = decimal.Parse(txtHarga.Text);
                    cmd.Parameters.Add(pHarga);

                    SqlParameter pStok = new SqlParameter("@stok", SqlDbType.Int);
                    pStok.Value = int.Parse(txtStok.Text);
                    cmd.Parameters.Add(pStok);

                    SqlParameter pOutRows = new SqlParameter("@outRows", SqlDbType.Int);
                    pOutRows.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pOutRows);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Produk berhasil diupdate", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    try { loadCts?.Cancel(); } catch { }
                    try { loadCts?.Dispose(); } catch { }
                    loadCts = new System.Threading.CancellationTokenSource();
                    _ = LoadDataAsync(loadCts.Token);
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHapus_Click(object sender, EventArgs e)
        {
            if (vwprodukBindingSource.Current == null)
            {
                MessageBox.Show("Pilih minimal 1 data untuk dihapus", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string message = "Yakin ingin menghapus produk ini?";

            DialogResult result = MessageBox.Show(message, "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                DataRowView rowView = (DataRowView)vwprodukBindingSource.Current;
                int id = (int)rowView["produkID"];

                using (SqlConnection conn = new SqlConnection(con))
                {
                    SqlCommand cmd = new SqlCommand("dbo.sp_DeleteProduk", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter pID = new SqlParameter("@produkID", SqlDbType.Int);
                    pID.Value = id;
                    cmd.Parameters.Add(pID);

                    SqlParameter pOutRows = new SqlParameter("@outRows", SqlDbType.Int);
                    pOutRows.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pOutRows);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Produk berhasil dihapus", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    try { loadCts?.Cancel(); } catch { }
                    try { loadCts?.Dispose(); } catch { }
                    loadCts = new System.Threading.CancellationTokenSource();
                    _ = LoadDataAsync(loadCts.Token);
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            try { loadCts?.Cancel(); } catch { }
            try { loadCts?.Dispose(); } catch { }
            loadCts = new System.Threading.CancellationTokenSource();
            _ = LoadDataAsync(loadCts.Token);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtNamaProduk.Text))
            {
                MessageBox.Show("Nama produk tidak boleh kosong", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtHarga.Text, out decimal harga) || harga <= 0)
            {
                MessageBox.Show("Harga harus berupa angka positif", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtStok.Text, out int stok) || stok < 0)
            {
                MessageBox.Show("Stok harus berupa angka positif atau nol", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            txtNamaProduk.Clear();
            txtHarga.Clear();
            txtStok.Clear();
            if (vwprodukBindingSource.Count > 0)
                vwprodukBindingSource.Position = 0;
        }

        private void BtnKembali_Click(object sender, EventArgs e)
        {
            this.Close();
            if (role == "admin")
            {
                new FormAdminMenu(username, role).Show();
            }
            else if (role == "kasir")
            {
                new FormKasirMenu(username, role).Show();
            }
        }
    }
}