-- =========================================
-- PRODUK
-- =========================================
CREATE PROCEDURE dbo.sp_InsertProduk
    @namaProduk VARCHAR(100),
    @harga DECIMAL(18,2),
    @stok INT,
    @outProdukID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF LEN(RTRIM(ISNULL(@namaProduk,''))) = 0
    BEGIN
        RAISERROR('Nama produk tidak boleh kosong',16,1);
        RETURN;
    END

    IF @harga < 0 OR @stok < 0
    BEGIN
        RAISERROR('Harga atau stok tidak valid',16,1);
        RETURN;
    END

    BEGIN TRY
        INSERT INTO dbo.produk (namaProduk, harga, stok)
        VALUES (@namaProduk, @harga, @stok);

        SET @outProdukID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        DECLARE @Err VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
        RETURN;
    END CATCH
END
GO

CREATE PROCEDURE dbo.sp_UpdateProduk
    @produkID INT,
    @namaProduk VARCHAR(100),
    @harga DECIMAL(18,2),
    @stok INT,
    @outRows INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF @produkID IS NULL OR @produkID <= 0
    BEGIN
        RAISERROR('produkID tidak valid',16,1);
        RETURN;
    END

    BEGIN TRY
        UPDATE dbo.produk
        SET namaProduk = @namaProduk,
            harga = @harga,
            stok = @stok
        WHERE produkID = @produkID;

        SET @outRows = @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        DECLARE @Err VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
        RETURN;
    END CATCH
END
GO

CREATE PROCEDURE dbo.sp_DeleteProduk
    @produkID INT,
    @outRows INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM dbo.produk
        WHERE produkID = @produkID;
        SET @outRows = @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        DECLARE @Err VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END
GO

CREATE PROCEDURE dbo.sp_SearchProduk
    @query VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    IF @query IS NULL SET @query = '';

    SELECT produkID, namaProduk, harga, stok
    FROM dbo.vw_produk
    WHERE namaProduk LIKE '%' + @query + '%';
END
GO

-- =========================================
-- KASIR & LOGIN
-- =========================================
CREATE PROCEDURE dbo.sp_InsertKasir
    @username VARCHAR(50),
    @password VARCHAR(50),
    @outLoginID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF LEN(RTRIM(ISNULL(@username,''))) < 3 OR LEN(RTRIM(ISNULL(@password,''))) < 3
    BEGIN
        RAISERROR('Panjang username/password minimal 3 karakter',16,1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM dbo.login WHERE username = @username)
    BEGIN
        RAISERROR('Username sudah ada',16,1);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Insert ke tabel login
        INSERT INTO dbo.login (username, password, role)
        VALUES (@username, @password, 'kasir');

        SET @outLoginID = SCOPE_IDENTITY();

        -- Insert ke tabel kasirMenu karena kasir terikat ke login
        INSERT INTO dbo.kasirMenu (loginID)
        VALUES (@outLoginID);
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @Err VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END
GO

CREATE PROCEDURE dbo.sp_UpdateKasir
    @loginID INT,
    @username VARCHAR(50),
    @password VARCHAR(50),
    @outRows INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.login
        SET username = @username,
            password = @password
        WHERE loginID = @loginID AND role = 'kasir';

        SET @outRows = @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        DECLARE @Err VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END
GO

CREATE PROCEDURE dbo.sp_DeleteKasir
    @loginID INT,
    @outRows INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Karena ada ON DELETE CASCADE, menghapus dari login otomatis menghapus data di kasirMenu
        DELETE FROM dbo.login
        WHERE loginID = @loginID AND role = 'kasir';

        SET @outRows = @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        DECLARE @Err VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END
GO

CREATE PROCEDURE dbo.sp_SearchKasir
    @query VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF @query IS NULL SET @query = '';

    SELECT kasirID, loginID, username, role
    FROM dbo.vw_kasir
    WHERE username LIKE '%' + @query + '%';
END
GO

CREATE PROCEDURE dbo.sp_Login_Safe
    @username VARCHAR(50),
    @password VARCHAR(50),
    @outRole VARCHAR(20) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @outRole = role
    FROM dbo.login
    WHERE username = @username AND password = @password;
END
GO

CREATE PROCEDURE dbo.sp_Login_Insecure
    @username VARCHAR(50),
    @password VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @sql VARCHAR(MAX);
    SET @sql = '
        SELECT role
        FROM dbo.login
        WHERE username = ''' + ISNULL(@username,'') + '''
          AND password = ''' + ISNULL(@password,'') + '''';
    EXEC(@sql);
END
GO

-- =========================================
-- TRANSAKSI
-- =========================================
CREATE PROCEDURE dbo.sp_CreateTransaction
    @kasirID INT,
    @pelangganID INT = NULL,
    @outTransaksiID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.transaksi (tanggal, totalHarga, kasirID, pelangganID)
        VALUES (GETDATE(), 0, @kasirID, @pelangganID);

        SET @outTransaksiID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        DECLARE @Err VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END
GO

CREATE PROCEDURE dbo.sp_InsertDetailTransaksi
    @transaksiID INT,
    @produkID INT,
    @jumlah INT,
    @hargaSatuan DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO dbo.detailTransaksi (transaksiID, produkID, jumlah, hargaSatuan, total)
        VALUES (@transaksiID, @produkID, @jumlah, @hargaSatuan, @jumlah * @hargaSatuan);
    END TRY
    BEGIN CATCH
        DECLARE @Err VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END
GO

CREATE PROCEDURE dbo.sp_UpdateTransaksi
    @transaksiID INT,
    @tanggal DATETIME,
    @totalHarga DECIMAL(18,2),
    @kasirID INT,
    @pelangganID INT = NULL,
    @outRows INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.transaksi
        SET tanggal = @tanggal,
            totalHarga = @totalHarga,
            kasirID = @kasirID,
            pelangganID = @pelangganID
        WHERE transaksiID = @transaksiID;

        SET @outRows = @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        DECLARE @Err VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END
GO

CREATE PROCEDURE dbo.sp_DeleteTransaksi
    @transaksiID INT,
    @outRows INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Karena ada ON DELETE CASCADE pada tabel detailTransaksi, 
        -- cukup delete di tabel transaksi saja
        DELETE FROM dbo.transaksi
        WHERE transaksiID = @transaksiID;

        SET @outRows = @@ROWCOUNT;
    END TRY
    BEGIN CATCH
        DECLARE @Err VARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END
GO

CREATE PROCEDURE dbo.sp_SearchTransaksi
    @fromDate DATETIME = NULL,
    @toDate DATETIME = NULL,
    @transaksiID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @fromDate IS NULL SET @fromDate = '1900-01-01';
    IF @toDate IS NULL SET @toDate = GETDATE();

    SELECT *
    FROM dbo.vw_transaksi_detail
    WHERE (@transaksiID IS NULL OR NomorTransaksi = @transaksiID)
      AND (Tanggal >= @fromDate AND Tanggal <= @toDate);
END
GO
