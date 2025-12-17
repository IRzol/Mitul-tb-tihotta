-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Dec 15. 19:59
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `mitulatbatihotta`
--
CREATE DATABASE IF NOT EXISTS `mitulatbatihotta` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `mitulatbatihotta`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `versenyzok`
--

CREATE TABLE `versenyzok` (
  `id` int(11) NOT NULL,
  `nev` varchar(100) NOT NULL,
  `ido1` double NOT NULL,
  `ido2` double NOT NULL,
  `ido3` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `versenyzok`
--

INSERT INTO `versenyzok` (`id`, `nev`, `ido1`, `ido2`, `ido3`) VALUES
(1, 'Kovacs Bela', 4.12, 2.01, 7.02),
(2, 'Nagy Arpad', 1.11, 5.02, 3.44),
(3, 'Szabo Tamas', 6.22, 10, 8.31),
(4, 'Varga Imre', 3.21, 4.09, 1.553),
(5, 'Toth Gergely', 7.01, 6.12, 10),
(6, 'Kis Balint', 2.44, 1.87, 4.55),
(7, 'Horvath Levente', 5.11, 7.44, 6.01),
(8, 'Farkas Dominik', 8.13, 8.99, 7.44),
(9, 'Lakatos Daniel', 0.55, 2.12, 3.01),
(10, 'Molnar Lajos', 4.77, 3.11, 2.02);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
