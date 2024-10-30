-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 28, 2024 at 02:33 PM
-- Server version: 10.4.25-MariaDB
-- PHP Version: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_penjualan`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_admin`
--

CREATE TABLE `tbl_admin` (
  `kodeadmin` varchar(6) NOT NULL,
  `namaadmin` varchar(50) NOT NULL,
  `passwordadmin` varchar(30) NOT NULL,
  `leveladmin` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_admin`
--

INSERT INTO `tbl_admin` (`kodeadmin`, `namaadmin`, `passwordadmin`, `leveladmin`) VALUES
('ADM001', 'lucy', '123', 'ADMIN'),
('ADM002', 'ambatukam', '123', 'USER');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_barang`
--

CREATE TABLE `tbl_barang` (
  `kodebarang` varchar(6) NOT NULL,
  `namabarang` varchar(50) DEFAULT NULL,
  `hargabarang` int(11) DEFAULT NULL,
  `jumlahbarang` int(11) DEFAULT NULL,
  `satuanbarang` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_barang`
--

INSERT INTO `tbl_barang` (`kodebarang`, `namabarang`, `hargabarang`, `jumlahbarang`, `satuanbarang`) VALUES
('BRG001', 'Maag Salam 1 Botol', 140000, 109, 'PCS'),
('BRG002', 'Maag Salam 2 Botol', 240000, 100, 'PCS'),
('BRG003', 'Maag Salam 3 Botol', 340000, 100, 'PCS'),
('BRG004', 'Madu Anak', 80000, 162, 'PCS');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_beli`
--

CREATE TABLE `tbl_beli` (
  `nobeli` varchar(10) NOT NULL,
  `tglbeli` date NOT NULL,
  `jambeli` time NOT NULL,
  `itembeli` int(11) NOT NULL,
  `totalbeli` int(11) NOT NULL,
  `bayar` int(11) NOT NULL,
  `kembali` int(11) NOT NULL,
  `kodesupplier` varchar(6) NOT NULL,
  `kodeadmin` varchar(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_beli`
--

INSERT INTO `tbl_beli` (`nobeli`, `tglbeli`, `jambeli`, `itembeli`, `totalbeli`, `bayar`, `kembali`, `kodesupplier`, `kodeadmin`) VALUES
('B240525001', '2024-05-25', '01:25:57', 10, 1400000, 1500000, 100000, 'SUP001', 'ADM001'),
('B240525002', '2024-05-25', '02:04:10', 50, 4000000, 5000000, 1000000, 'SUP002', 'ADM001'),
('B240527003', '2024-05-27', '10:02:08', 15, 1200000, 1500000, 300000, 'SUP002', 'ADM001');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_detailbeli`
--

CREATE TABLE `tbl_detailbeli` (
  `nobeli` varchar(10) NOT NULL,
  `kodebarang` varchar(6) NOT NULL,
  `namabarang` varchar(50) NOT NULL,
  `hargabeli` int(11) NOT NULL,
  `jumlahbeli` int(11) NOT NULL,
  `subtotal` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_detailbeli`
--

INSERT INTO `tbl_detailbeli` (`nobeli`, `kodebarang`, `namabarang`, `hargabeli`, `jumlahbeli`, `subtotal`) VALUES
('B240525001', 'BRG001', 'Maag Salam 1 Botol', 140000, 10, 1400000),
('B240525002', 'BRG004', 'Madu Anak', 80000, 50, 4000000),
('B240527003', 'BRG004', 'Madu Anak', 80000, 10, 800000),
('B240527003', 'BRG004', 'Madu Anak', 80000, 5, 400000);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_detailjual`
--

CREATE TABLE `tbl_detailjual` (
  `nojual` varchar(10) NOT NULL,
  `kodebarang` varchar(6) NOT NULL,
  `namabarang` varchar(50) NOT NULL,
  `hargajual` int(11) NOT NULL,
  `jumlahjual` int(11) NOT NULL,
  `subtotal` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_detailjual`
--

INSERT INTO `tbl_detailjual` (`nojual`, `kodebarang`, `namabarang`, `hargajual`, `jumlahjual`, `subtotal`) VALUES
('J240525001', 'BRG001', 'Maag Salam 1 Botol', 140000, 1, 140000),
('J240528002', 'BRG004', 'Madu Anak', 80000, 3, 240000);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_jual`
--

CREATE TABLE `tbl_jual` (
  `nojual` varchar(10) NOT NULL,
  `tgljual` date DEFAULT NULL,
  `jamjual` time DEFAULT NULL,
  `itemjual` int(11) DEFAULT NULL,
  `totaljual` int(11) DEFAULT NULL,
  `dibayar` int(11) DEFAULT NULL,
  `kembali` int(11) DEFAULT NULL,
  `kodepelanggan` varchar(6) DEFAULT NULL,
  `kodeadmin` varchar(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_jual`
--

INSERT INTO `tbl_jual` (`nojual`, `tgljual`, `jamjual`, `itemjual`, `totaljual`, `dibayar`, `kembali`, `kodepelanggan`, `kodeadmin`) VALUES
('J240525001', '2024-05-25', '01:21:26', 1, 140000, 150000, 10000, 'PLG001', 'ADM001'),
('J240528002', '2024-05-28', '07:26:40', 3, 240000, 250000, 10000, 'PLG001', 'ADM001');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_pelanggan`
--

CREATE TABLE `tbl_pelanggan` (
  `kodepelanggan` varchar(6) NOT NULL,
  `namapelanggan` varchar(50) DEFAULT NULL,
  `alamatpelanggan` varchar(100) DEFAULT NULL,
  `telppelanggan` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_pelanggan`
--

INSERT INTO `tbl_pelanggan` (`kodepelanggan`, `namapelanggan`, `alamatpelanggan`, `telppelanggan`) VALUES
('PLG001', 'Ambatron', 'Ngawi', '555'),
('PLG002', 'Rusdi', 'Nganjuk', '000');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_supplier`
--

CREATE TABLE `tbl_supplier` (
  `kodesupplier` varchar(6) NOT NULL,
  `namasupplier` varchar(50) NOT NULL,
  `alamatsupplier` varchar(100) NOT NULL,
  `telpsupplier` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_supplier`
--

INSERT INTO `tbl_supplier` (`kodesupplier`, `namasupplier`, `alamatsupplier`, `telpsupplier`) VALUES
('SUP001', 'Ironi', 'Cibaduyut', '505'),
('SUP002', 'Ambatusyam', 'Gotham', '050');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_admin`
--
ALTER TABLE `tbl_admin`
  ADD PRIMARY KEY (`kodeadmin`);

--
-- Indexes for table `tbl_barang`
--
ALTER TABLE `tbl_barang`
  ADD PRIMARY KEY (`kodebarang`);

--
-- Indexes for table `tbl_beli`
--
ALTER TABLE `tbl_beli`
  ADD PRIMARY KEY (`nobeli`);

--
-- Indexes for table `tbl_jual`
--
ALTER TABLE `tbl_jual`
  ADD PRIMARY KEY (`nojual`);

--
-- Indexes for table `tbl_pelanggan`
--
ALTER TABLE `tbl_pelanggan`
  ADD PRIMARY KEY (`kodepelanggan`);

--
-- Indexes for table `tbl_supplier`
--
ALTER TABLE `tbl_supplier`
  ADD PRIMARY KEY (`kodesupplier`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
