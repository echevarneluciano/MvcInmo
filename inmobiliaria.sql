-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         10.4.24-MariaDB - mariadb.org binary distribution
-- SO del servidor:              Win64
-- HeidiSQL Versión:             12.2.0.6576
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para inmobiliaria
CREATE DATABASE IF NOT EXISTS `inmobiliaria` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `inmobiliaria`;

-- Volcando estructura para tabla inmobiliaria.contratos
CREATE TABLE IF NOT EXISTS `contratos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FechaInicio` datetime NOT NULL,
  `FechaFin` datetime DEFAULT NULL,
  `Precio` decimal(20,6) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_contratos_inquilinos` (`InquilinoId`),
  KEY `FK_contratos_inmuebles` (`InmuebleId`),
  CONSTRAINT `FK_contratos_inmuebles` FOREIGN KEY (`InmuebleId`) REFERENCES `inmuebles` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_contratos_inquilinos` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.contratos: ~9 rows (aproximadamente)
INSERT INTO `contratos` (`Id`, `FechaInicio`, `FechaFin`, `Precio`, `InquilinoId`, `InmuebleId`) VALUES
	(7, '2023-03-04 10:55:00', '2023-04-08 10:55:00', 1000.000000, 4, 16),
	(9, '2023-03-01 01:10:00', '2023-03-03 01:10:00', 222.000000, 5, 1),
	(10, '2023-03-03 01:36:00', '2023-03-05 01:36:00', 78.000000, 3, 1),
	(11, '2023-04-06 23:46:00', '2023-11-09 23:47:00', 200.000000, 3, 2),
	(12, '2023-04-12 11:41:00', '2024-06-11 11:41:00', 950.000000, 6, 30),
	(14, '2023-02-01 08:08:00', '2023-02-13 08:08:00', 200.000000, 3, 1),
	(15, '2023-03-01 08:31:00', '2023-03-02 08:31:00', 23.000000, 3, 1),
	(16, '2023-03-02 09:12:00', '2023-03-03 09:12:00', 222.000000, 3, 2),
	(19, '2023-04-06 09:32:00', '2023-04-22 09:32:00', 666.000000, 4, 18);

-- Volcando estructura para tabla inmobiliaria.inmuebles
CREATE TABLE IF NOT EXISTS `inmuebles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Direccion` varchar(50) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Superficie` int(11) NOT NULL,
  `Latitud` decimal(20,6) NOT NULL,
  `Longitud` decimal(20,6) NOT NULL,
  `PropietarioId` int(11) NOT NULL,
  `Tipo` varchar(50) DEFAULT NULL,
  `Precio` decimal(20,6) DEFAULT NULL,
  `Estado` int(11) NOT NULL DEFAULT 1,
  PRIMARY KEY (`Id`),
  KEY `FK_inmuebles_propietarios` (`PropietarioId`),
  CONSTRAINT `FK_inmuebles_propietarios` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.inmuebles: ~12 rows (aproximadamente)
INSERT INTO `inmuebles` (`Id`, `Direccion`, `Ambientes`, `Superficie`, `Latitud`, `Longitud`, `PropietarioId`, `Tipo`, `Precio`, `Estado`) VALUES
	(1, 'puertas del sol 2', 3, 15, 33.000000, 45.000000, 25, 'Residencial', 500.000000, 1),
	(2, 'potrero 12', 2, 32, 32.000000, 33.000000, 23, 'Residencial', NULL, 1),
	(16, 'cerro de oro', 2, 23, 23.000000, 333.000000, 24, 'Residencial', NULL, 1),
	(18, 'cerros 2', 3, 3333, 222.000000, 22.000000, 23, 'Residencial', NULL, 1),
	(20, 'colon 33', 123, 123, 123.000000, 123.000000, 23, 'Comercial', NULL, 1),
	(22, 'Juana Koslay', 123, 123, 123.000000, 123.000000, 23, 'Residencial', NULL, 1),
	(24, 'asgard 224', 1, 2000, 23.000000, 33.000000, 23, 'Residencial', NULL, 1),
	(25, 'Juana Koslay comercial 1', 123, 123, 123.000000, 321.000000, 23, 'Comercial', NULL, 1),
	(26, 'cerro de la gloria 3', 123, 1231, 2222.000000, 223.000000, 23, 'Comercial', NULL, 1),
	(28, 'Juana Koslay 55', 12, 12, 12.000000, 12.000000, 23, 'Comercial', NULL, 1),
	(29, 'puertas del sol 5', 123, 123, 123.000000, 123.000000, 23, 'Residencial', 500.000000, 0),
	(30, 'Barrio sur', 2, 1, 2233.000000, 232.000000, 26, 'Comercial', 800.000000, 1);

-- Volcando estructura para tabla inmobiliaria.inquilinos
CREATE TABLE IF NOT EXISTS `inquilinos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DNI` varchar(50) NOT NULL DEFAULT '',
  `Nombre` varchar(50) NOT NULL DEFAULT '',
  `Apellido` varchar(50) NOT NULL DEFAULT '',
  `Telefono` varchar(50) NOT NULL DEFAULT '',
  `Email` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.inquilinos: ~3 rows (aproximadamente)
INSERT INTO `inquilinos` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`) VALUES
	(3, '123123', 'Franco', 'Torres2', '4556655', 'la2@llelo.com'),
	(4, '3123123', 'Luciano222', 'Echevarne22', '02664259847', 'echevarneluciano@gmail.com222'),
	(5, '221', 'Morti', 'Martinez', '443322', 'morti@marti.com'),
	(6, '568887', 'Morta ', 'Dellas', '445577', 'mort@ade.com');

-- Volcando estructura para tabla inmobiliaria.pagos
CREATE TABLE IF NOT EXISTS `pagos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Mes` int(11) NOT NULL,
  `FechaPagado` datetime DEFAULT NULL,
  `ContratoId` int(11) NOT NULL,
  `Importe` decimal(20,6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_pagos_contratos` (`ContratoId`),
  CONSTRAINT `FK_pagos_contratos` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.pagos: ~41 rows (aproximadamente)
INSERT INTO `pagos` (`Id`, `Mes`, `FechaPagado`, `ContratoId`, `Importe`) VALUES
	(3, 3, '2023-04-13 22:32:00', 9, 2555.000000),
	(4, 1, '2023-04-14 22:42:00', 10, 0.010000),
	(5, 2, NULL, 10, 0.000000),
	(6, 0, '2023-04-10 22:59:00', 11, 0.000000),
	(7, 1, NULL, 11, 0.000000),
	(8, 2, NULL, 11, 0.000000),
	(9, 3, NULL, 11, 0.000000),
	(10, 4, NULL, 11, 0.000000),
	(11, 5, NULL, 11, 0.000000),
	(12, 6, NULL, 11, 0.000000),
	(14, 1, NULL, 12, NULL),
	(15, 2, NULL, 12, NULL),
	(16, 3, NULL, 12, NULL),
	(17, 4, NULL, 12, NULL),
	(18, 5, NULL, 12, NULL),
	(19, 6, NULL, 12, NULL),
	(20, 7, NULL, 12, NULL),
	(21, 8, NULL, 12, NULL),
	(22, 9, NULL, 12, NULL),
	(23, 10, NULL, 12, NULL),
	(24, 11, NULL, 12, NULL),
	(25, 12, NULL, 12, NULL),
	(26, 13, NULL, 12, NULL),
	(27, 0, NULL, 14, NULL),
	(28, 1, NULL, 14, NULL),
	(29, 2, NULL, 14, NULL),
	(30, 3, NULL, 14, NULL),
	(31, 4, NULL, 14, NULL),
	(32, 5, NULL, 14, NULL),
	(33, 6, NULL, 14, NULL),
	(34, 7, NULL, 14, NULL),
	(35, 8, NULL, 14, NULL),
	(36, 9, NULL, 14, NULL),
	(37, 10, NULL, 14, NULL),
	(38, 11, NULL, 14, NULL),
	(39, 12, NULL, 14, NULL),
	(40, 13, NULL, 14, NULL),
	(41, 14, NULL, 14, NULL),
	(42, 15, NULL, 14, NULL),
	(43, 16, NULL, 14, NULL),
	(45, 0, NULL, 19, NULL);

-- Volcando estructura para tabla inmobiliaria.propietarios
CREATE TABLE IF NOT EXISTS `propietarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DNI` varchar(50) NOT NULL DEFAULT '0',
  `Nombre` varchar(50) NOT NULL DEFAULT '0',
  `Apellido` varchar(50) NOT NULL DEFAULT '0',
  `Telefono` varchar(50) NOT NULL DEFAULT '0',
  `Email` varchar(50) NOT NULL,
  `Clave` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.propietarios: ~4 rows (aproximadamente)
INSERT INTO `propietarios` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`, `Clave`) VALUES
	(23, '12324', 'Franco', 'Echevarnes', '026642598472', 'pro2p@prop.com', ''),
	(24, '22233', 'Luciano', 'Echevarne', '02664259847', 'echevarneluciano@gmail.com', NULL),
	(25, '223311', 'Jorge', 'Reta', '123123', 'reta@gmail.com', NULL),
	(26, '1235554', 'Rodrigo', 'Vallses', '4556699', 'rodri@go.com', NULL);

-- Volcando estructura para tabla inmobiliaria.usuarios
CREATE TABLE IF NOT EXISTS `usuarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Rol` int(11) NOT NULL,
  `Nombre` varchar(50) DEFAULT NULL,
  `Apellido` varchar(50) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Clave` varchar(50) DEFAULT NULL,
  `Avatar` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.usuarios: ~5 rows (aproximadamente)
INSERT INTO `usuarios` (`Id`, `Rol`, `Nombre`, `Apellido`, `Email`, `Clave`, `Avatar`) VALUES
	(4, 0, 'Lucas', 'Echevarne', 'empleado@empleado.com', '42cBSXesh0vNWSwhCy236yHd+V/P6mzEsNpySeUgShk=', '/Uploads/avatar_418c9950d-69aa-4b01-9e53-c57318114197.webp'),
	(7, 2, 'asd', 'ddd', 'dd@sad', '5p4fC86R2Gq92YE9kLJcYRtaotnsJUIyeUDGBN0i/aU=', '/Uploads/avatar_7.jpg'),
	(8, 2, 'asd', 'asddd', 'ddsa@asddd', 'rJ7VmKtwO9IoN9E3E1AzhSjcKSbFQ48nzaIUiLlUtZw=', 'Uploads\\avatar_8.jpg'),
	(9, 2, 'dfsd', 'fsdf', 'fsdf@sdf', 'CD5aextJBNL+c0kB2aGR3rO9BA8xsiCKhxF2QxBigO0=', 'Uploads\\avatar_9.jpg'),
	(11, 1, 'admin', 'admin', 'admin@admin.com', 'eia65qLim9UsFSfCqeMfSxBpLUb6Ec2hfdBC6NwSTIc=', '/Uploads/avatar_11.jpg');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
