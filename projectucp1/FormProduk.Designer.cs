namespace projectucp1
{
    partial class FormProduk
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtNamaProduk = new System.Windows.Forms.TextBox();
            this.txtHarga = new System.Windows.Forms.TextBox();
            this.txtStok = new System.Windows.Forms.TextBox();
            this.btnTambah = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Location = new System.Drawing.Point(30, 30);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(500, 200);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellClick);
            // 
            // txtNamaProduk
            // 
            this.txtNamaProduk.Location = new System.Drawing.Point(30, 250);
            this.txtNamaProduk.Name = "txtNamaProduk";
            this.txtNamaProduk.Size = new System.Drawing.Size(397, 20);
            this.txtNamaProduk.TabIndex = 1;
            // 
            // txtHarga
            // 
            this.txtHarga.Location = new System.Drawing.Point(30, 290);
            this.txtHarga.Name = "txtHarga";
            this.txtHarga.Size = new System.Drawing.Size(397, 20);
            this.txtHarga.TabIndex = 2;
            // 
            // txtStok
            // 
            this.txtStok.Location = new System.Drawing.Point(30, 330);
            this.txtStok.Name = "txtStok";
            this.txtStok.Size = new System.Drawing.Size(397, 20);
            this.txtStok.TabIndex = 3;
            this.txtStok.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtStok_KeyPress);
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(455, 250);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(75, 23);
            this.btnTambah.TabIndex = 4;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.Click += new System.EventHandler(this.BtnTambah_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(455, 290);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(455, 330);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(75, 23);
            this.btnHapus.TabIndex = 6;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.Click += new System.EventHandler(this.BtnHapus_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(431, 371);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(99, 32);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // FormProduk
            // 
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtNamaProduk);
            this.Controls.Add(this.txtHarga);
            this.Controls.Add(this.txtStok);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnRefresh);
            this.Name = "FormProduk";
            this.Text = "FormProduk";
            this.Load += new System.EventHandler(this.FormProduk_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtNamaProduk;
        private System.Windows.Forms.TextBox txtHarga;
        private System.Windows.Forms.TextBox txtStok;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnRefresh;
    }
}