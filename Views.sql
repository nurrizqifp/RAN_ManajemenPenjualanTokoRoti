USE TOKO_ROTI;
GO

CREATE VIEW dbo.vw_produk
AS
SELECT produkID, namaProduk, harga, stok
FROM dbo.produk;
GO

CREATE VIEW dbo.vw_produk_tersedia
AS
SELECT produkID, namaProduk, harga, stok
FROM dbo.produk
WHERE stok > 0;
GO

CREATE VIEW dbo.vw_kasir
AS
SELECT k.kasirID, l.loginID, l.username, l.role
FROM dbo.login l
INNER JOIN dbo.kasirMenu k ON l.loginID = k.loginID
WHERE l.role = 'kasir';
GO

CREATE VIEW dbo.vw_transaksi_detail
AS
SELECT 
    t.transaksiID AS NomorTransaksi,
    t.tanggal AS Tanggal,
    t.totalHarga AS TotalTransaksi,
    p.namaProduk AS NamaProduk,
    dt.jumlah AS Jumlah,
    dt.hargaSatuan AS HargaSatuan,
    dt.total AS Subtotal
FROM dbo.transaksi t
INNER JOIN dbo.detailTransaksi dt ON dt.transaksiID = t.transaksiID
INNER JOIN dbo.produk p ON p.produkID = dt.produkID;
GO


select * from vw_kasir
INSERT INTO dbo.kasirMenu (loginID)
VALUES (5);