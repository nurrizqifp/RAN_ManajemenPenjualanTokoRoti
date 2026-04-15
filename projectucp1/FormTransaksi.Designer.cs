namespace projectucp1
{
    partial class FormTransaksi
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbProduk = new System.Windows.Forms.ComboBox();
            this.txtHarga = new System.Windows.Forms.TextBox();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.btnTambah = new System.Windows.Forms.Button();
            this.dgvKeranjang = new System.Windows.Forms.DataGridView();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtBayar = new System.Windows.Forms.TextBox();
            this.lblKembalian = new System.Windows.Forms.Label();
            this.btnSimpan = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvKeranjang)).BeginInit();
            this.SuspendLayout();

            cmbProduk.Location = new System.Drawing.Point(30, 30);
            cmbProduk.SelectedIndexChanged += new System.EventHandler(this.cmbProduk_SelectedIndexChanged);

            txtHarga.Location = new System.Drawing.Point(200, 30);
            txtHarga.ReadOnly = true;

            txtQty.Location = new System.Drawing.Point(350, 30);

            btnTambah.Location = new System.Drawing.Point(500, 30);
            btnTambah.Text = "Tambah";
            btnTambah.Click += new System.EventHandler(this.btnTambah_Click);

            dgvKeranjang.Location = new System.Drawing.Point(30, 80);
            dgvKeranjang.Size = new System.Drawing.Size(600, 200);

            lblTotal.Location = new System.Drawing.Point(30, 300);
            lblTotal.Text = "Total: 0";

            txtBayar.Location = new System.Drawing.Point(30, 330);
            txtBayar.TextChanged += new System.EventHandler(this.txtBayar_TextChanged);

            lblKembalian.Location = new System.Drawing.Point(30, 360);
            lblKembalian.Text = "Kembalian: 0";

            btnSimpan.Location = new System.Drawing.Point(30, 400);
            btnSimpan.Text = "Simpan";
            btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);

            this.Controls.Add(cmbProduk);
            this.Controls.Add(txtHarga);
            this.Controls.Add(txtQty);
            this.Controls.Add(btnTambah);
            this.Controls.Add(dgvKeranjang);
            this.Controls.Add(lblTotal);
            this.Controls.Add(txtBayar);
            this.Controls.Add(lblKembalian);
            this.Controls.Add(btnSimpan);

            this.Text = "FormTransaksi";
            this.Load += new System.EventHandler(this.FormTransaksi_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvKeranjang)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ComboBox cmbProduk;
        private System.Windows.Forms.TextBox txtHarga;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.DataGridView dgvKeranjang;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtBayar;
        private System.Windows.Forms.Label lblKembalian;
        private System.Windows.Forms.Button btnSimpan;
    }
}