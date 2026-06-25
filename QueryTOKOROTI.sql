CREATE DATABASE TOKO_ROTI;
GO

USE TOKO_ROTI;
GO

-- 1. Tabel login
CREATE TABLE login (
    loginID INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(50) NOT NULL,
    role VARCHAR(20) NOT NULL,
    CONSTRAINT CHK_Login_Role CHECK (role IN ('admin', 'kasir'))
);
GO

-- 2. Tabel adminMenu
CREATE TABLE adminMenu (
    adminID INT IDENTITY(1,1) PRIMARY KEY,
    loginID INT NOT NULL UNIQUE,
    CONSTRAINT FK_adminMenu_login
        FOREIGN KEY (loginID) REFERENCES login(loginID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
GO

-- 3. Tabel kasirMenu
CREATE TABLE kasirMenu (
    kasirID INT IDENTITY(1,1) PRIMARY KEY,
    loginID INT NOT NULL UNIQUE,
    CONSTRAINT FK_kasirMenu_login
        FOREIGN KEY (loginID) REFERENCES login(loginID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
GO

-- 4. Tabel pelanggan
CREATE TABLE pelanggan (
    pelangganID INT IDENTITY(1,1) PRIMARY KEY,
    nama VARCHAR(100) NOT NULL,
    telepon VARCHAR(15) NULL
);
GO

-- 5. Tabel produk
CREATE TABLE produk (
    produkID INT IDENTITY(1,1) PRIMARY KEY,
    namaProduk VARCHAR(100) NOT NULL,
    harga DECIMAL(18,2) NOT NULL,
    stok INT NOT NULL,
    CONSTRAINT CHK_Produk_Stok CHECK (stok >= 0),
    CONSTRAINT CHK_Produk_Harga CHECK (harga >= 0)
);
GO

-- 6. Tabel transaksi
CREATE TABLE transaksi (
    transaksiID INT IDENTITY(1,1) PRIMARY KEY,
    tanggal DATETIME NOT NULL DEFAULT GETDATE(),
    totalHarga DECIMAL(18,2) NOT NULL,
    kasirID INT NOT NULL,
    pelangganID INT NULL,
    CONSTRAINT CHK_Transaksi_Total CHECK (totalHarga >= 0),
    CONSTRAINT FK_Transaksi_Kasir
        FOREIGN KEY (kasirID) REFERENCES kasirMenu(kasirID)
        ON UPDATE CASCADE,
    CONSTRAINT FK_Transaksi_Pelanggan
        FOREIGN KEY (pelangganID) REFERENCES pelanggan(pelangganID)
        ON DELETE SET NULL
        ON UPDATE CASCADE
);
GO

-- 7. Tabel detailTransaksi
CREATE TABLE detailTransaksi (
    detailID INT IDENTITY(1,1) PRIMARY KEY,
    transaksiID INT NOT NULL,
    produkID INT NOT NULL,
    jumlah INT NOT NULL,
    hargaSatuan DECIMAL(18,2) NOT NULL,
    total DECIMAL(18,2) NOT NULL,
    CONSTRAINT CHK_DetailTransaksi_Jumlah CHECK (jumlah > 0),
    CONSTRAINT CHK_DetailTransaksi_HargaSatuan CHECK (hargaSatuan >= 0),
    CONSTRAINT CHK_DetailTransaksi_Total CHECK (total >= 0),
    CONSTRAINT FK_DetailTransaksi_Transaksi
        FOREIGN KEY (transaksiID) REFERENCES transaksi(transaksiID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT FK_DetailTransaksi_Produk
        FOREIGN KEY (produkID) REFERENCES produk(produkID)
        ON UPDATE CASCADE
);
GO


/*
TESTING
*/

INSERT INTO login (username, password, role)
VALUES 
('nana', '123', 'admin'),
('apip', '123', 'kasir');

INSERT INTO adminMenu (loginID)
SELECT loginID FROM login WHERE username = 'nana';

INSERT INTO kasirMenu (loginID)
SELECT loginID FROM login WHERE username = 'apip';

INSERT INTO pelanggan (nama, telepon)
VALUES
('Budi', '08123456789'),
('Siti', '08129876543');

INSERT INTO produk (namaProduk, harga, stok)
VALUES
('Roti Coklat', 8000, 50),
('Roti Keju', 9000, 30),
('Roti Tawar', 12000, 20);
GO

SELECT * FROM login;
SELECT * FROM adminMenu;
SELECT * FROM kasirMenu;
SELECT * FROM pelanggan;
SELECT * FROM produk;
SELECT * FROM transaksi;
SELECT * FROM detailTransaksi;