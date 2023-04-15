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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.contratos: ~9 rows (aproximadamente)
INSERT INTO `contratos` (`Id`, `FechaInicio`, `FechaFin`, `Precio`, `InquilinoId`, `InmuebleId`) VALUES
	(20, '2023-04-15 20:15:00', '2023-04-15 20:17:24', 2000.000000, 7, 32),
	(21, '2023-04-22 20:17:00', '2023-08-16 20:17:00', 1500.000000, 7, 32);

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
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.inmuebles: ~12 rows (aproximadamente)
INSERT INTO `inmuebles` (`Id`, `Direccion`, `Ambientes`, `Superficie`, `Latitud`, `Longitud`, `PropietarioId`, `Tipo`, `Precio`, `Estado`) VALUES
	(31, 'Juana Koslay 22', 3, 1255, 2222.000000, 3355.000000, 28, 'Comercial', 1258.000000, 0),
	(32, 'Colon 1233', 2, 345, 3322.000000, 4444.000000, 29, 'Residencial', 2000.000000, 1),
	(33, 'Rivadavia 2300', 1, 200, 3322.000000, 2222.000000, 29, 'Comercial', 1847.000000, 1);

-- Volcando estructura para tabla inmobiliaria.inquilinos
CREATE TABLE IF NOT EXISTS `inquilinos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DNI` varchar(50) NOT NULL DEFAULT '',
  `Nombre` varchar(50) NOT NULL DEFAULT '',
  `Apellido` varchar(50) NOT NULL DEFAULT '',
  `Telefono` varchar(50) NOT NULL DEFAULT '',
  `Email` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.inquilinos: ~2 rows (aproximadamente)
INSERT INTO `inquilinos` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`) VALUES
	(7, '212333322', 'Inquilino 1', 'Apellido i1', '02664477889', 'mail@gmail.com.ar'),
	(8, '5588779', 'Inquilino 2 ', 'Ape2', '477887', 'este@mail.com.ar');

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
) ENGINE=InnoDB AUTO_INCREMENT=62 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.pagos: ~42 rows (aproximadamente)
INSERT INTO `pagos` (`Id`, `Mes`, `FechaPagado`, `ContratoId`, `Importe`) VALUES
	(52, 0, '2023-04-15 20:17:24', 20, NULL),
	(53, 1, '2023-04-15 20:17:24', 20, NULL),
	(54, 2, '2023-04-15 20:17:24', 20, NULL),
	(55, 3, '2023-04-15 20:17:24', 20, NULL),
	(56, 4, '2023-04-15 20:17:24', 20, NULL),
	(57, 0, '2023-04-15 20:17:24', 20, NULL),
	(58, 0, NULL, 21, NULL),
	(59, 1, NULL, 21, NULL),
	(60, 2, NULL, 21, NULL),
	(61, 10, '2023-04-15 20:18:00', 21, NULL);

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
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.propietarios: ~4 rows (aproximadamente)
INSERT INTO `propietarios` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`, `Clave`) VALUES
	(28, '3312222', 'Propietario 1', 'Apellido 1', '4455897', 'prop@gmail.com', NULL),
	(29, '43222111', 'Propietario 2', 'Apellido 2', '4778899', 'prop2@gmail.com2', NULL);

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

-- Volcando datos para la tabla inmobiliaria.usuarios: ~6 rows (aproximadamente)
INSERT INTO `usuarios` (`Id`, `Rol`, `Nombre`, `Apellido`, `Email`, `Clave`, `Avatar`) VALUES
	(4, 0, 'Lucas', 'Echevarne', 'empleado@empleado.com', '42cBSXesh0vNWSwhCy236yHd+V/P6mzEsNpySeUgShk=', '/Uploads/avatar_418c9950d-69aa-4b01-9e53-c57318114197.webp'),
	(11, 1, 'admin', 'admin', 'admin@admin.com', 'eia65qLim9UsFSfCqeMfSxBpLUb6Ec2hfdBC6NwSTIc=', '/Uploads/avatar_11.jpg');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
