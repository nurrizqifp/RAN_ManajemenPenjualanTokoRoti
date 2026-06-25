# 🍞 RAN - Manajemen Penjualan Toko Roti

Aplikasi desktop untuk manajemen penjualan toko roti dengan dua peran pengguna: Admin dan Kasir. Mendukung pengelolaan produk, transaksi, stok, import data Excel, dan laporan penjualan. Dibangun dengan arsitektur client-server menggunakan SQL Server sebagai database terpusat.

**Link Repository:** [https://github.com/nisrinahnfhh/TokoRoti_UCP3_PABD.git](https://github.com/nisrinahnfhh/TokoRoti_UCP3_PABD.git)

---

## Daftar Isi

- [Fitur Aplikasi](#fitur-aplikasi)
- [Teknologi](#teknologi)
- [Prasyarat Sistem](#prasyarat-sistem)
- [Panduan Instalasi](#panduan-instalasi)
  - [Langkah 1 - Konfigurasi SQL Server Authentication](#langkah-1---konfigurasi-sql-server-authentication)
  - [Langkah 2 - Mengetahui Port TCP yang Digunakan](#langkah-2---mengetahui-port-tcp-yang-digunakan)
  - [Langkah 3 - Konfigurasi Firewall](#langkah-3---konfigurasi-firewall)
  - [Langkah 4 - Membuat Installer dengan Inno Setup](#langkah-4---membuat-installer-dengan-inno-setup)
  - [Langkah 5 - Instalasi di Laptop Client](#langkah-5---instalasi-di-laptop-client)
- [Panduan Penggunaan](#panduan-penggunaan)
- [Catatan Penting](#catatan-penting)

---

## Fitur Aplikasi

**Admin:**
- Mengelola data produk (tambah, update, hapus)
- Import data produk melalui file Excel
- Mengelola akun kasir (tambah, update, hapus)
- Melihat dan mencetak laporan penjualan

**Kasir:**
- Melihat stok produk
- Menginput dan mengelola transaksi penjualan

**Umum:**
- Login dengan username dan password
- Logout dari aplikasi

---

## Teknologi

| Komponen | Teknologi |
|---|---|
| Bahasa Pemrograman | C# / VB.NET |
| Framework UI | Windows Forms (.NET Framework) |
| Database | Microsoft SQL Server |
| Installer Builder | Inno Setup Compiler |
| Platform | Windows |

---

## Prasyarat Sistem

**Laptop Server:**
- Windows 10 atau lebih baru
- Microsoft SQL Server (Express / Developer / Standard)
- SQL Server Management Studio (SSMS)
- SQL Server Configuration Manager
- Inno Setup Compiler (untuk membuat file installer)
- Terhubung ke jaringan WiFi lokal

**Laptop Client:**
- Windows 10 atau lebih baru
- .NET Framework (sesuai versi aplikasi)
- Terhubung ke jaringan WiFi yang sama dengan server

---

## Panduan Instalasi

Semua langkah 1 sampai 4 dilakukan di **laptop server**. Langkah 5 dilakukan di **laptop client**.

---

### Langkah 1 - Konfigurasi SQL Server Authentication

1. Buka **SQL Server Management Studio (SSMS)**.
2. Di Object Explorer, klik kanan nama server lalu pilih **Properties**.
3. Pilih halaman **Security** di panel kiri.
4. Pada bagian Server Authentication, pilih **"SQL Server and Windows Authentication mode"**.
5. Klik **OK**.

---

### Langkah 2 - Mengetahui Port TCP yang Digunakan

> Langkah ini wajib dilakukan sebelum konfigurasi firewall karena nomor port berbeda di setiap komputer.

1. Buka **SQL Server Configuration Manager**.
2. Di panel kiri, klik **SQL Server Network Configuration**, lalu klik **Protocols**.
3. Pastikan status **TCP/IP** adalah **Enabled**.
4. Double klik **TCP/IP**, lalu pilih tab **IP Addresses**.
5. Scroll ke paling bawah hingga menemukan bagian **IPAll**.
6. Catat nilai **TCP Dynamic Ports** karena angka ini akan digunakan pada konfigurasi firewall.
7. Klik **OK**.

---

### Langkah 3 - Konfigurasi Firewall

#### Membuat Rule untuk Port TCP (Port SQL Server)

1. Buka **Windows Defender Firewall with Advanced Security**.
2. Klik **Inbound Rules** di panel kiri, lalu klik **New Rule** di panel kanan.
3. Pilih **Port**, klik **Next**.
4. Pilih **TCP**, pilih **Specific local ports**, lalu ketik angka port yang dicatat pada Langkah 2 (contoh: 27144). Klik **Next**.
5. Pilih **Allow the connection**, klik **Next**.
6. Centang semua: **Domain**, **Private**, **Public**. Klik **Next**.
7. Isi Name: `SQL Server NANA 27144`, lalu klik **Finish**.

#### Membuat Rule untuk Port UDP 1434 (SQL Server Browser)

1. Buka **Windows Defender Firewall with Advanced Security**.
2. Klik **Inbound Rules** di panel kiri, lalu klik **New Rule** di panel kanan.
3. Pilih **Port**, klik **Next**.
4. Pilih **UDP**, pilih **Specific local ports**, ketik `1434`, lalu klik **Next**.
5. Pilih **Allow the connection**, klik **Next**.
6. Centang semua: **Domain**, **Private**, **Public**. Klik **Next**.
7. Isi Name: `SQL Browser 1434`, lalu klik **Finish**.

Pastikan rule untuk Port TCP dan Port UDP sudah berhasil dibuat.

---

### Langkah 4 - Membuat Installer dengan Inno Setup

1. Buka **Inno Setup Compiler**.
2. Pilih **"Create a new script file using the Script Wizard"**, klik **OK**.
3. Klik **Next** pada halaman Welcome.
4. Isi informasi aplikasi sesuai kebutuhan.
5. Biarkan pengaturan folder instalasi default, klik **Next**.
6. Pada halaman **Application Files**:
   - Klik **Browse** di Application main executable file, masuk ke folder `bin\Release\`, pilih `projectucp1.exe`, klik **Open**.
   - Klik **Add folder**, pilih seluruh folder `bin\Release\`, klik **OK**.
   - Klik **Next**.
7. Pada halaman **Application File Association**, ubah extension menjadi `.exe`, klik **Next**.
8. Klik **Next** pada halaman Shortcuts, Documentation, Install Mode, dan Languages.
9. Pada halaman **Compiler Settings**, ubah nilai **compiler output base file name** dari `mysetup` menjadi `TokoRotiSetup`. Klik **Next**.
10. Klik **Next**, **Next**, lalu **Finish**.

File `TokoRotiSetup.exe` siap untuk didistribusikan ke laptop client.

---

### Langkah 5 - Instalasi di Laptop Client

1. Copy file `TokoRotiSetup.exe` dari laptop server ke laptop client menggunakan flashdisk.
2. Di laptop client, double klik `TokoRotiSetup.exe` untuk memulai instalasi.
3. Klik **Next** pada halaman Welcome.
4. Pilih lokasi instalasi, biarkan default, klik **Next**.
5. Klik **Install** dan tunggu hingga proses selesai.
6. Klik **Finish**.
7. Buka aplikasi dari shortcut Desktop atau Start Menu.
8. Masukkan username dan password, lalu klik **Login**.

---

## Panduan Penggunaan

### Login sebagai Admin

1. Masukkan username dan password Admin, klik **Login**.
2. Setelah masuk, Admin dapat:
   - Menambah, mengupdate, dan menghapus data produk
   - Mengimport data produk melalui file Excel
   - Menambah, mengupdate, dan menghapus akun kasir
   - Melihat dan mencetak laporan penjualan

### Login sebagai Kasir

1. Masukkan username dan password Kasir, klik **Login**.
2. Setelah masuk, Kasir dapat:
   - Melihat stok produk
   - Menginput dan mengelola transaksi penjualan

### Logout

Baik Admin maupun Kasir dapat melakukan logout dari dalam aplikasi.

---
