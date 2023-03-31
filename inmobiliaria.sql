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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.contratos: ~2 rows (aproximadamente)
INSERT INTO `contratos` (`Id`, `FechaInicio`, `FechaFin`, `Precio`, `InquilinoId`, `InmuebleId`) VALUES
	(7, '2023-03-04 10:55:00', '2023-04-08 10:55:00', 800.000000, 4, 16),
	(9, '2023-03-24 01:10:00', '2023-04-08 01:10:00', 222.000000, 3, 1);

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
  PRIMARY KEY (`Id`),
  KEY `FK_inmuebles_propietarios` (`PropietarioId`),
  CONSTRAINT `FK_inmuebles_propietarios` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.inmuebles: ~13 rows (aproximadamente)
INSERT INTO `inmuebles` (`Id`, `Direccion`, `Ambientes`, `Superficie`, `Latitud`, `Longitud`, `PropietarioId`, `Tipo`) VALUES
	(1, 'lalal', 1, 12, 22.000000, 33.000000, 23, 'Comercial'),
	(2, 'potre', 2, 32, 32.000000, 33.000000, 23, 'Comercial'),
	(3, 'Juana Koslay', 333, 333, 33.000000, 33.000000, 24, 'Residencial'),
	(16, 'cerro de oro', 2, 23, 23.000000, 333.000000, 24, 'Residencial'),
	(18, 'cerros 2', 3, 3333, 222.000000, 22.000000, 23, 'Residencial'),
	(19, 'asdg', 123, 123, 123.000000, 123.000000, 25, 'Residencial'),
	(20, 'asdg', 123, 123, 123.000000, 123.000000, 23, 'Comercial'),
	(22, 'Juana Koslay', 123, 123, 123.000000, 123.000000, 23, 'Residencial'),
	(24, 'asgard 224', 1, 2000, 23.000000, 33.000000, 23, 'Residencial'),
	(25, 'Juana Koslay comercial 1', 123, 123, 123.000000, 321.000000, 23, 'Comercial'),
	(26, 'cerro de la gloria 3', 123, 1231, 2222.000000, 223.000000, 23, 'Comercial');

-- Volcando estructura para tabla inmobiliaria.inquilinos
CREATE TABLE IF NOT EXISTS `inquilinos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DNI` varchar(50) NOT NULL DEFAULT '',
  `Nombre` varchar(50) NOT NULL DEFAULT '',
  `Apellido` varchar(50) NOT NULL DEFAULT '',
  `Telefono` varchar(50) NOT NULL DEFAULT '',
  `Email` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.inquilinos: ~2 rows (aproximadamente)
INSERT INTO `inquilinos` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`) VALUES
	(3, '123123', 'Franco', 'Torres', '4556655', 'la2@llelo.com'),
	(4, '3123123', 'Luciano222', 'Echevarne22', '02664259847', 'echevarneluciano@gmail.com222');

-- Volcando estructura para tabla inmobiliaria.pagos
CREATE TABLE IF NOT EXISTS `pagos` (
  `Id` int(11) NOT NULL,
  `Mes` int(11) NOT NULL,
  `FechaPagado` datetime DEFAULT NULL,
  `ContratoId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_pagos_contratos` (`ContratoId`),
  CONSTRAINT `FK_pagos_contratos` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.pagos: ~0 rows (aproximadamente)

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
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.propietarios: ~3 rows (aproximadamente)
INSERT INTO `propietarios` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`, `Clave`) VALUES
	(23, '1232', 'Franco', 'Echevarne', '026642598472', 'pro2p@prop.com', ''),
	(24, '22233', 'Luciano', 'Echevarne', '02664259847', 'echevarneluciano@gmail.com', NULL),
	(25, '223311', 'Jorge', 'Reta', '123123', 'reta@gmail.com', NULL);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
