-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Creato il: Nov 28, 2024 alle 20:14
-- Versione del server: 10.4.32-MariaDB
-- Versione PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `magazzino`
--

-- --------------------------------------------------------

--
-- Struttura della tabella `articoli`
--

CREATE TABLE `articoli` (
  `Id` int(11) NOT NULL,
  `Codice` varchar(50) NOT NULL,
  `Descrizione` varchar(255) NOT NULL,
  `DataArrivo` datetime NOT NULL,
  `DataUscita` datetime DEFAULT NULL,
  `Stato` varchar(50) NOT NULL,
  `Quantita` int(11) NOT NULL,
  `PosizioneId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dump dei dati per la tabella `articoli`
--

INSERT INTO `articoli` (`Id`, `Codice`, `Descrizione`, `DataArrivo`, `DataUscita`, `Stato`, `Quantita`, `PosizioneId`) VALUES
(1, 'ART001', 'Articolo 1', '2024-10-21 15:49:46', NULL, 'In Magazzino', 25, 26),
(2, 'ART002', 'Articolo 2', '2024-10-21 15:51:18', NULL, 'In Magazzino', 62, 14),
(3, 'ART003', 'Articolo 3', '2024-10-21 15:52:40', NULL, 'Reparto', 1, NULL),
(4, 'ART004', 'Articolo 4', '2024-10-21 15:53:01', NULL, 'In Magazzino', 18, 22),
(5, 'ART005', 'Articolo 5', '2024-10-21 15:53:49', NULL, 'In Magazzino', 3, 20),
(6, 'ART006', 'Articolo 6', '2024-10-21 16:36:22', NULL, 'In Magazzino', 1, 32),
(11, 'ART0038', 'Prova 2', '2024-11-26 17:16:57', NULL, 'Reparto', 3, NULL),
(28, 'ART007', 'Articolo 7', '2024-11-28 20:14:13', NULL, 'Difettoso', 34, 19);

-- --------------------------------------------------------

--
-- Struttura della tabella `movimenti`
--

CREATE TABLE `movimenti` (
  `Id` int(11) NOT NULL,
  `ArticoloId` int(11) DEFAULT NULL,
  `TipoMovimento` int(11) NOT NULL,
  `PosizioneInizialeId` int(11) DEFAULT NULL,
  `PosizioneFinaleId` int(11) DEFAULT NULL,
  `DataMovimento` datetime NOT NULL,
  `Quantita` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dump dei dati per la tabella `movimenti`
--

INSERT INTO `movimenti` (`Id`, `ArticoloId`, `TipoMovimento`, `PosizioneInizialeId`, `PosizioneFinaleId`, `DataMovimento`, `Quantita`) VALUES
(1, 1, 0, NULL, 7, '2024-10-21 15:49:46', 25),
(2, 2, 0, NULL, 18, '2024-10-21 15:51:18', 92),
(3, 3, 0, NULL, NULL, '2024-10-21 15:52:40', 1),
(4, 4, 0, NULL, 22, '2024-10-21 15:53:01', 21),
(5, 1, 1, 7, 21, '2024-10-21 15:53:14', 25),
(6, 5, 0, NULL, 20, '2024-10-21 15:53:49', 10),
(7, 5, 2, 20, NULL, '2024-10-21 15:53:55', 7),
(8, 6, 0, NULL, 26, '2024-10-21 16:36:22', 1),
(9, 7, 0, NULL, 23, '2024-10-21 17:22:42', 1),
(11, 8, 0, NULL, 17, '2024-10-21 17:26:02', 1),
(12, 9, 0, NULL, 24, '2024-10-21 17:52:40', 1),
(13, 6, 1, 26, 13, '2024-10-22 17:55:21', 1),
(14, 2, 1, 18, 14, '2024-10-23 12:01:57', 92),
(15, 2, 2, 14, NULL, '2024-10-23 12:02:09', 30),
(16, 2, 1, 14, 14, '2024-10-23 12:04:22', 62),
(17, 1, 1, 21, 26, '2024-10-23 12:12:02', 25),
(18, 10, 0, NULL, 7, '2024-11-26 17:15:31', 23),
(19, 10, 1, 7, 11, '2024-11-26 17:15:49', 23),
(20, 10, 2, 11, NULL, '2024-11-26 17:16:27', 18),
(21, 11, 0, NULL, NULL, '2024-11-26 17:16:57', 3),
(22, 12, 0, NULL, 23, '2024-11-26 17:17:22', 10),
(23, 13, 0, NULL, 30, '2024-11-26 17:33:37', 7),
(24, 14, 0, NULL, 31, '2024-11-26 17:38:36', 24),
(25, 15, 0, NULL, 28, '2024-11-26 17:39:19', 12),
(26, 16, 0, NULL, 29, '2024-11-26 17:42:53', 33),
(27, 17, 0, NULL, 27, '2024-11-26 17:45:48', 213),
(28, 18, 0, NULL, 41, '2024-11-26 17:50:55', 65),
(30, 19, 0, NULL, 32, '2024-11-26 17:54:31', 233),
(32, 20, 0, NULL, 34, '2024-11-26 18:02:00', 23),
(33, 21, 0, NULL, 25, '2024-11-26 18:04:06', 23),
(34, 22, 0, NULL, 33, '2024-11-26 18:06:06', 23),
(35, 23, 0, NULL, 35, '2024-11-26 18:14:10', 24),
(37, 24, 0, NULL, 36, '2024-11-26 18:16:54', 34),
(39, 25, 0, NULL, 35, '2024-11-26 18:26:14', 10),
(41, 6, 1, 13, 32, '2024-11-27 15:27:40', 1),
(42, 4, 2, 22, NULL, '2024-11-27 15:28:54', 3),
(43, 22, 1, 33, 19, '2024-11-27 15:29:48', 23),
(44, 26, 0, NULL, 9, '2024-11-27 15:31:37', 1),
(46, 20, 2, 34, NULL, '2024-11-27 15:32:43', 0),
(47, 27, 0, NULL, 35, '2024-11-27 15:35:05', 10),
(61, 28, 0, NULL, 19, '2024-11-28 20:14:13', 34);

-- --------------------------------------------------------

--
-- Struttura della tabella `posizioni`
--

CREATE TABLE `posizioni` (
  `Id` int(11) NOT NULL,
  `Occupata` tinyint(1) DEFAULT 0,
  `Quantita` int(11) DEFAULT 0,
  `CodicePosizione` varchar(3) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dump dei dati per la tabella `posizioni`
--

INSERT INTO `posizioni` (`Id`, `Occupata`, `Quantita`, `CodicePosizione`) VALUES
(1, 0, 0, 'A1'),
(2, 0, 0, 'B1'),
(3, 0, 0, 'C1'),
(4, 0, 0, 'D1'),
(5, 0, 0, 'E1'),
(6, 0, 0, 'F1'),
(7, 0, 0, 'G1'),
(8, 0, 0, 'H1'),
(9, 0, 0, 'I1'),
(10, 0, 0, 'J1'),
(11, 0, 0, 'A2'),
(12, 0, 0, 'B2'),
(13, 0, 0, 'C2'),
(14, 0, 0, 'D2'),
(15, 0, 0, 'E2'),
(16, 0, 0, 'F2'),
(17, 0, 0, 'G2'),
(18, 0, 0, 'H2'),
(19, 1, 34, 'I2'),
(20, 1, 3, 'J2'),
(21, 0, 0, 'A3'),
(22, 1, 18, 'B3'),
(23, 0, 0, 'C3'),
(24, 0, 0, 'D3'),
(25, 0, 0, 'E3'),
(26, 1, 25, 'F3'),
(27, 0, 0, 'G3'),
(28, 0, 0, 'H3'),
(29, 0, 0, 'I3'),
(30, 0, 0, 'J3'),
(31, 0, 0, 'A4'),
(32, 1, 1, 'B4'),
(33, 0, 0, 'C4'),
(34, 0, 0, 'D4'),
(35, 0, 0, 'E4'),
(36, 0, 0, 'F4'),
(37, 0, 0, 'G4'),
(38, 0, 0, 'H4'),
(39, 0, 0, 'I4'),
(40, 0, 0, 'J4'),
(41, 0, 0, 'A5'),
(42, 0, 0, 'B5'),
(43, 0, 0, 'C5'),
(44, 0, 0, 'D5'),
(45, 0, 0, 'E5'),
(46, 0, 0, 'F5'),
(47, 0, 0, 'G5'),
(48, 0, 0, 'H5'),
(49, 0, 0, 'I5'),
(50, 0, 0, 'J5'),
(51, 0, 0, 'A6'),
(52, 0, 0, 'B6'),
(53, 0, 0, 'C6'),
(54, 0, 0, 'D6'),
(55, 0, 0, 'E6'),
(56, 0, 0, 'F6'),
(57, 0, 0, 'G6'),
(58, 0, 0, 'H6'),
(59, 0, 0, 'I6'),
(60, 0, 0, 'J6'),
(61, 0, 0, 'A7'),
(62, 0, 0, 'B7'),
(63, 0, 0, 'C7'),
(64, 0, 0, 'D7'),
(65, 0, 0, 'E7'),
(66, 0, 0, 'F7'),
(67, 0, 0, 'G7'),
(68, 0, 0, 'H7'),
(69, 0, 0, 'I7'),
(70, 0, 0, 'J7'),
(71, 0, 0, 'A8'),
(72, 0, 0, 'B8'),
(73, 0, 0, 'C8'),
(74, 0, 0, 'D8'),
(75, 0, 0, 'E8'),
(76, 0, 0, 'F8'),
(77, 0, 0, 'G8'),
(78, 0, 0, 'H8'),
(79, 0, 0, 'I8'),
(80, 0, 0, 'J8'),
(81, 0, 0, 'A9'),
(82, 0, 0, 'B9'),
(83, 0, 0, 'C9'),
(84, 0, 0, 'D9'),
(85, 0, 0, 'E9'),
(86, 0, 0, 'F9'),
(87, 0, 0, 'G9'),
(88, 0, 0, 'H9'),
(89, 0, 0, 'I9'),
(90, 0, 0, 'J9'),
(91, 0, 0, 'A10'),
(92, 0, 0, 'B10'),
(93, 0, 0, 'C10'),
(94, 0, 0, 'D10'),
(95, 0, 0, 'E10'),
(96, 0, 0, 'F10'),
(97, 0, 0, 'G10'),
(98, 0, 0, 'H10'),
(99, 0, 0, 'I10'),
(100, 0, 0, 'J10');

-- --------------------------------------------------------

--
-- Struttura della tabella `utenti`
--

CREATE TABLE `utenti` (
  `Id` int(11) NOT NULL,
  `Nome` varchar(50) NOT NULL,
  `Cognome` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `DataRegistrazione` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dump dei dati per la tabella `utenti`
--

INSERT INTO `utenti` (`Id`, `Nome`, `Cognome`, `Email`, `Username`, `PasswordHash`, `DataRegistrazione`) VALUES
(1, 'Mario', 'Rossi', 'mario.rossi@example.com', 'mariorossi', 'passwordhash1', '2024-10-05 11:36:55'),
(2, 'Luca', 'Bianchi', 'luca.bianchi@example.com', 'lucabianchi', 'passwordhash2', '2024-10-05 11:36:55'),
(3, 'asdadas', 'asdasdas', 'aaa@gmail.com', 'aaa', '$2a$11$itOGr5doVr0GMpwkCGbDeOtjVe6O29sl83UIRNCylS.Mbp9ir/fVW', '2024-10-09 18:12:54'),
(5, 'Mario', 'Rddddossi', 'mario.asdadada@example.com', 'mario.ddddd', '$2a$11$CO6a.a2Wmxw.15UFOVFd3.kRf8qvAdtpo9P9XfvAyuQKQYFNRRxji', '2024-10-09 18:13:27'),
(6, 'asdadas', 'asdasdas', 'adadsadad@gmail.com', 'proravava', '$2a$11$msyLdWEuSktiEF5zP0XRWu.lJVKeN8Ucpd0Po0AdHT1cloUjyVw7O', '2024-10-09 18:16:26'),
(7, 'Francesco', 'Ridolfi', 'prova@gmail.com', 'frarido', '$2a$11$uMHIROcT4HvSWVxQKQD/bOmNojseI8Rxvg5An69NnmDERLxrGea.a', '2024-10-10 18:40:42'),
(8, 'dasdada', 'ddddd', 'rffrfasdca@gmail.com', 'dddddd111', '$2a$11$rJysM5Dn46zK8P91h72FOeD1Xd0.FgYB62ITFwoOrupvzjb9G.vBO', '2024-10-10 18:46:18'),
(9, 'asdasdaddddd', 'dasfasfa', 'hhhhhh@gmail.com', 'hhhh', '$2a$11$NGZgrDyDi1WQIXmspBjGz.K82nz//7GzUuKh3OTB1iGl8Wxuq3Rx.', '2024-10-10 18:56:34'),
(10, 'fffffff', 'rrrrrrr', 'frfsdas@gmail.com', 'ffff', '$2a$11$dkXJklGP8F.JV7Umq4Pzyenzy7cjBrkJgrkI8mzK9yo1sGD0z072i', '2024-10-11 18:46:41'),
(11, 'ddd', 'sadsa', 'dada@gmail.com', 'dada1', '$2a$11$Dh3afBQKUoNA3Ud5nodBcudX8EUQFY3SkYOgbMi.W5hXGPhyaqYaC', '2024-10-14 17:39:09'),
(12, 'ddd', 'sadsa', 'dadaddd@gmail.com', 'Lollo', '$2a$11$ZtX/HWlxiqBBCKLlq6siEeT75Y90vVzssuMJL3YKDHi2TGONT.61K', '2024-10-14 18:05:01'),
(13, 'ddd', 'sadsa', 'frasdad1231@gmail.com', 'Edgar', '$2a$11$0gzkyt0WcnI7ewFxX4aPT.1/kU5KRu8rIbTk6hd9r2i1ujCQB6/.i', '2024-10-14 18:23:13'),
(16, 'dasdada', 'dadsdad', 'asdadsada@gmail.com', 'Dada11', '$2a$11$YehQBxpenMoCel./znWoP.HtCJoIF.ihb8iKtDAvrhOtt84c7ZVVe', '2024-10-15 18:08:31'),
(19, 'dasdada', 'dadsdad', 'provaaaa@gmail.com', 'prova123123', '$2a$11$uTtEMLrnCG4yx1gdsq18lOQYYVjB2.44asEbABZcci2N7S7wtt8bu', '2024-10-15 18:11:58'),
(20, 'provafinale', 'provafinaleee', 'provafinale@gmail.com', 'ProvaFinale', '$2a$11$qFiso5UaVSeFZOruKTa9.eMzuvjFHbKlPSxLiNi3MER/eK14MRxDC', '2024-10-15 18:13:09'),
(21, 'Test', 'test', 'test@gmail.com', 'Test', '$2a$11$AStORkJqqv4JIUrlU9TZzuzVcy9LQla9erqwYUZ9vg6z/jBWXOKAq', '2024-10-15 18:19:17'),
(22, 'weffe', 'Adfaf', 'ggaag@gmail.com', 'Test12345', '$2a$11$efQqUScFGQeiF5BRuOherussUjyPQlKfoxyV/pONphBvK2DpMHFPG', '2024-11-26 18:27:50'),
(23, 'Aurora', 'D\'Orazio', 'aurordor@gmail.com', 'auro', '$2a$11$n86meuevsfdQJ.d7y6nLWuutXDXZ7RsORHubI0J4sLLBDSUHVz7EK', '2024-11-27 15:24:04');

-- --------------------------------------------------------

--
-- Struttura della tabella `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indici per le tabelle scaricate
--

--
-- Indici per le tabelle `articoli`
--
ALTER TABLE `articoli`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `PosizioneId` (`PosizioneId`);

--
-- Indici per le tabelle `movimenti`
--
ALTER TABLE `movimenti`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `ArticoloId` (`ArticoloId`),
  ADD KEY `PosizioneInizialeId` (`PosizioneInizialeId`),
  ADD KEY `movimenti_ibfk_3` (`PosizioneFinaleId`);

--
-- Indici per le tabelle `posizioni`
--
ALTER TABLE `posizioni`
  ADD PRIMARY KEY (`Id`);

--
-- Indici per le tabelle `utenti`
--
ALTER TABLE `utenti`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Username` (`Username`),
  ADD UNIQUE KEY `UC_Email` (`Email`),
  ADD KEY `idx_email` (`Email`),
  ADD KEY `idx_username` (`Username`);

--
-- Indici per le tabelle `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT per le tabelle scaricate
--

--
-- AUTO_INCREMENT per la tabella `articoli`
--
ALTER TABLE `articoli`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT per la tabella `movimenti`
--
ALTER TABLE `movimenti`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=62;

--
-- AUTO_INCREMENT per la tabella `posizioni`
--
ALTER TABLE `posizioni`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=101;

--
-- AUTO_INCREMENT per la tabella `utenti`
--
ALTER TABLE `utenti`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- Limiti per le tabelle scaricate
--

--
-- Limiti per la tabella `articoli`
--
ALTER TABLE `articoli`
  ADD CONSTRAINT `articoli_ibfk_1` FOREIGN KEY (`PosizioneId`) REFERENCES `posizioni` (`Id`);

--
-- Limiti per la tabella `movimenti`
--
ALTER TABLE `movimenti`
  ADD CONSTRAINT `movimenti_ibfk_2` FOREIGN KEY (`PosizioneInizialeId`) REFERENCES `posizioni` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
