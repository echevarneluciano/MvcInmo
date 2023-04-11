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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.contratos: ~4 rows (aproximadamente)
INSERT INTO `contratos` (`Id`, `FechaInicio`, `FechaFin`, `Precio`, `InquilinoId`, `InmuebleId`) VALUES
	(7, '2023-03-04 10:55:00', '2023-04-08 10:55:00', 92.000000, 4, 16),
	(9, '2023-03-24 01:10:00', '2023-04-08 01:10:00', 222.000000, 5, 1),
	(10, '2023-03-15 01:36:00', '2023-03-15 01:36:00', 78.000000, 3, 1),
	(11, '2023-04-06 23:46:00', '2023-11-09 23:47:00', 200.000000, 3, 2);

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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.inmuebles: ~11 rows (aproximadamente)
INSERT INTO `inmuebles` (`Id`, `Direccion`, `Ambientes`, `Superficie`, `Latitud`, `Longitud`, `PropietarioId`, `Tipo`) VALUES
	(1, 'puertas del sol 2', 3, 15, 33.000000, 45.000000, 25, 'Residencial'),
	(2, 'potrero 12', 2, 32, 32.000000, 33.000000, 23, 'Residencial'),
	(16, 'cerro de oro', 2, 23, 23.000000, 333.000000, 24, 'Residencial'),
	(18, 'cerros 2', 3, 3333, 222.000000, 22.000000, 23, 'Residencial'),
	(20, 'colon 33', 123, 123, 123.000000, 123.000000, 23, 'Comercial'),
	(22, 'Juana Koslay', 123, 123, 123.000000, 123.000000, 23, 'Residencial'),
	(24, 'asgard 224', 1, 2000, 23.000000, 33.000000, 23, 'Residencial'),
	(25, 'Juana Koslay comercial 1', 123, 123, 123.000000, 321.000000, 23, 'Comercial'),
	(26, 'cerro de la gloria 3', 123, 1231, 2222.000000, 223.000000, 23, 'Comercial'),
	(28, 'Juana Koslay 55', 12, 12, 12.000000, 12.000000, 23, 'Comercial'),
	(29, 'puertas del sol 5', 123, 123, 123.000000, 123.000000, 23, 'Comercial');

-- Volcando estructura para tabla inmobiliaria.inquilinos
CREATE TABLE IF NOT EXISTS `inquilinos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DNI` varchar(50) NOT NULL DEFAULT '',
  `Nombre` varchar(50) NOT NULL DEFAULT '',
  `Apellido` varchar(50) NOT NULL DEFAULT '',
  `Telefono` varchar(50) NOT NULL DEFAULT '',
  `Email` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.inquilinos: ~3 rows (aproximadamente)
INSERT INTO `inquilinos` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`) VALUES
	(3, '123123', 'Franco', 'Torres', '4556655', 'la2@llelo.com'),
	(4, '3123123', 'Luciano222', 'Echevarne22', '02664259847', 'echevarneluciano@gmail.com222'),
	(5, '221', 'Morti', 'Martinez', '443322', 'morti@marti.com');

-- Volcando estructura para tabla inmobiliaria.pagos
CREATE TABLE IF NOT EXISTS `pagos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Mes` int(11) NOT NULL,
  `FechaPagado` datetime DEFAULT NULL,
  `ContratoId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_pagos_contratos` (`ContratoId`),
  CONSTRAINT `FK_pagos_contratos` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.pagos: ~11 rows (aproximadamente)
INSERT INTO `pagos` (`Id`, `Mes`, `FechaPagado`, `ContratoId`) VALUES
	(2, 2, '2023-04-11 00:40:00', 9),
	(3, 3, NULL, 9),
	(4, 1, NULL, 10),
	(5, 2, NULL, 10),
	(6, 0, NULL, 11),
	(7, 1, NULL, 11),
	(8, 2, NULL, 11),
	(9, 3, NULL, 11),
	(10, 4, NULL, 11),
	(11, 5, NULL, 11),
	(12, 6, NULL, 11);

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
	(23, '1232', 'Franco', 'Echevarnes', '026642598472', 'pro2p@prop.com', ''),
	(24, '22233', 'Luciano', 'Echevarne', '02664259847', 'echevarneluciano@gmail.com', NULL),
	(25, '223311', 'Jorge', 'Reta', '123123', 'reta@gmail.com', NULL);

-- Volcando estructura para tabla inmobiliaria.usuarios
CREATE TABLE IF NOT EXISTS `usuarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Rol` int(11) NOT NULL,
  `Nombre` varchar(50) DEFAULT NULL,
  `Apellido` varchar(50) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Clave` varchar(50) DEFAULT NULL,
  `Avatar` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.usuarios: ~7 rows (aproximadamente)
INSERT INTO `usuarios` (`Id`, `Rol`, `Nombre`, `Apellido`, `Email`, `Clave`, `Avatar`) VALUES
	(4, 0, 'Franco', 'Echevarne', 'empleado@empleado.com', '42cBSXesh0vNWSwhCy236yHd+V/P6mzEsNpySeUgShk=', '/Uploads/avatar_4.jpeg'),
	(5, 2, 'este', 'otro ', 'este@gmail', 'dA2ltKnw21R1Ej7YVuIbbRibx5I9qJJkRM8EYqaIbv0=', NULL),
	(7, 2, 'asd', 'ddd', 'dd@sad', '5p4fC86R2Gq92YE9kLJcYRtaotnsJUIyeUDGBN0i/aU=', '/Uploads/avatar_7.jpg'),
	(8, 2, 'asd', 'asddd', 'ddsa@asddd', 'rJ7VmKtwO9IoN9E3E1AzhSjcKSbFQ48nzaIUiLlUtZw=', 'Uploads\\avatar_8.jpg'),
	(9, 2, 'dfsd', 'fsdf', 'fsdf@sdf', 'CD5aextJBNL+c0kB2aGR3rO9BA8xsiCKhxF2QxBigO0=', 'Uploads\\avatar_9.jpg'),
	(10, 2, 'dasd', 'asd', 'asd@asd', '7/Mhqv1axK60qVQB2iTmyxTUHoakip1s+vWLTrGJE/4=', '/Uploads/avatar_10.jpeg'),
	(11, 1, 'admin', 'admin', 'admin@admin.com', 'eia65qLim9UsFSfCqeMfSxBpLUb6Ec2hfdBC6NwSTIc=', '/Uploads/avatar_11.jpg');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
