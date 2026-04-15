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
            // 
            // cmbProduk
            // 
            this.cmbProduk.Location = new System.Drawing.Point(30, 30);
            this.cmbProduk.Name = "cmbProduk";
            this.cmbProduk.Size = new System.Drawing.Size(121, 21);
            this.cmbProduk.TabIndex = 0;
            this.cmbProduk.SelectedIndexChanged += new System.EventHandler(this.cmbProduk_SelectedIndexChanged);
            // 
            // txtHarga
            // 
            this.txtHarga.Location = new System.Drawing.Point(200, 30);
            this.txtHarga.Name = "txtHarga";
            this.txtHarga.ReadOnly = true;
            this.txtHarga.Size = new System.Drawing.Size(100, 20);
            this.txtHarga.TabIndex = 1;
            // 
            // txtQty
            // 
            this.txtQty.Location = new System.Drawing.Point(350, 30);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(100, 20);
            this.txtQty.TabIndex = 2;
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(500, 30);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(75, 23);
            this.btnTambah.TabIndex = 3;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // dgvKeranjang
            // 
            this.dgvKeranjang.Location = new System.Drawing.Point(30, 80);
            this.dgvKeranjang.Name = "dgvKeranjang";
            this.dgvKeranjang.Size = new System.Drawing.Size(600, 200);
            this.dgvKeranjang.TabIndex = 4;
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(30, 300);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(100, 23);
            this.lblTotal.TabIndex = 5;
            this.lblTotal.Text = "Total: 0";
            // 
            // txtBayar
            // 
            this.txtBayar.Location = new System.Drawing.Point(30, 330);
            this.txtBayar.Name = "txtBayar";
            this.txtBayar.Size = new System.Drawing.Size(100, 20);
            this.txtBayar.TabIndex = 6;
            this.txtBayar.TextChanged += new System.EventHandler(this.txtBayar_TextChanged);
            // 
            // lblKembalian
            // 
            this.lblKembalian.Location = new System.Drawing.Point(30, 360);
            this.lblKembalian.Name = "lblKembalian";
            this.lblKembalian.Size = new System.Drawing.Size(100, 23);
            this.lblKembalian.TabIndex = 7;
            this.lblKembalian.Text = "Kembalian: 0";
            // 
            // btnSimpan
            // 
            this.btnSimpan.Location = new System.Drawing.Point(30, 400);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 23);
            this.btnSimpan.TabIndex = 8;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // FormTransaksi
            // 
            this.ClientSize = new System.Drawing.Size(683, 451);
            this.Controls.Add(this.cmbProduk);
            this.Controls.Add(this.txtHarga);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.dgvKeranjang);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.txtBayar);
            this.Controls.Add(this.lblKembalian);
            this.Controls.Add(this.btnSimpan);
            this.Name = "FormTransaksi";
            this.Text = "FormTransaksi";
            this.Load += new System.EventHandler(this.FormTransaksi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKeranjang)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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