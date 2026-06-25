# 🍞 RAN - Manajemen Penjualan Toko Roti

---

## 📋 Daftar Isi

- [Fitur Utama](#fitur-utama)
- [Teknologi](#teknologi)
- [Prasyarat Sistem](#prasyarat-sistem)
- [Instalasi](#instalasi)
  - [A. Persiapan Laptop Server](#a-persiapan-laptop-server)
  - [B. Konfigurasi SQL Server](#b-konfigurasi-sql-server)
  - [C. Instalasi di Laptop Client](#c-instalasi-di-laptop-client)
- [Cara Penggunaan](#cara-penggunaan)
- [Catatan Penting](#catatan-penting)
- [Tim Pengembang](#tim-pengembang)

---

## Fitur Utama

- Manajemen data produk roti (tambah, ubah, hapus)
- Pencatatan transaksi penjualan harian
- Pengelolaan stok bahan baku dan produk
- Laporan penjualan harian, mingguan, dan bulanan
- Manajemen pengguna dengan autentikasi login
- Dukungan multi-client melalui jaringan WiFi lokal

---

## Teknologi

| Komponen | Teknologi |
|---|---|
| Bahasa Pemrograman | C# / VB.NET |
| Framework UI | Windows Forms (.NET Framework) |
| Database | Microsoft SQL Server |
| Platform | Windows |
| Distribusi | Installer (.exe) |

---

## Prasyarat Sistem

### Laptop Server
- Windows 10 atau lebih baru
- Microsoft SQL Server (Express / Developer / Standard)
- SQL Server Management Studio (SSMS) - opsional
- Terhubung ke jaringan WiFi lokal

### Laptop Client
- Windows 10 atau lebih baru
- .NET Framework (sesuai versi aplikasi)
- Terhubung ke jaringan WiFi yang **sama** dengan server

---

## Instalasi

### A. Persiapan Laptop Server

1. Pastikan **SQL Server** sudah terinstal dan service-nya dalam kondisi **Running**.
2. Buka **SQL Server Configuration Manager**.
3. Aktifkan protokol **TCP/IP** pada SQL Server Network Configuration.
4. Catat nomor **port TCP** yang digunakan (default: 1433 atau TCP Dynamic).
5. Buka **Windows Firewall** dan tambahkan **Inbound Rule** untuk port SQL Server tersebut.
6. Pastikan database aplikasi toko roti sudah di-restore atau sudah tersedia di SQL Server.

### B. Konfigurasi SQL Server

1. Buka **SQL Server Management Studio (SSMS)**.
2. Pastikan login SQL Server Authentication sudah dikonfigurasi dengan benar.
3. Aktifkan opsi **Mixed Mode Authentication** jika belum aktif (Properties > Security).
4. Restart SQL Server service setelah perubahan konfigurasi.

### C. Instalasi di Laptop Client

1. Copy file `TokoRotiSetup.exe` dari laptop server ke laptop client menggunakan flashdisk.
2. Di laptop client, double klik `TokoRotiSetup.exe` untuk memulai instalasi.
3. Klik **Next** pada halaman Welcome.
4. Pilih lokasi instalasi, biarkan default, lalu klik **Next**.
5. Klik **Install** dan tunggu hingga proses selesai.
6. Klik **Finish** untuk menutup wizard instalasi.
7. Buka aplikasi melalui shortcut di Desktop atau Start Menu.
8. Masukkan username dan password, lalu klik **Login**.

---

## Cara Penggunaan

1. Pastikan laptop server sudah menyala dan SQL Server service sedang berjalan.
2. Pastikan laptop client dan server terhubung ke jaringan WiFi yang sama.
3. Buka aplikasi dari shortcut Desktop atau Start Menu.
4. Login menggunakan akun yang sudah terdaftar.
5. Gunakan menu navigasi untuk mengakses fitur yang tersedia.
