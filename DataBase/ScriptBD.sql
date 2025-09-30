-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema aurorabd
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `aurorabd` ;


-- -----------------------------------------------------
-- Schema aurorabd
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `aurorabd` DEFAULT CHARACTER SET utf8 ;
ALTER DATABASE aurorabd CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci;
USE `aurorabd` ;

-- -----------------------------------------------------
-- Table `aurorabd`.`Empresa`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `aurorabd`.`Empresa` ;

CREATE TABLE IF NOT EXISTS `aurorabd`.`Empresa` (
  `idEmpresa` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NULL,
  `Contrasena` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`idEmpresa`),
  UNIQUE INDEX `Nombre_UNIQUE` (`Nombre` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Administrador`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `aurorabd`.`Administrador` ;

CREATE TABLE IF NOT EXISTS `aurorabd`.`Administrador` (
  `idAdministrador` INT NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NULL,
  `Contrasena` VARCHAR(45) NULL,
  `idEmpresa` INT NOT NULL,
  PRIMARY KEY (`idAdministrador`),
  INDEX `fk_Administrador_Empresa_idx` (`idEmpresa` ASC) VISIBLE,
  CONSTRAINT `fk_Administrador_Empresa`
    FOREIGN KEY (`idEmpresa`)
    REFERENCES `aurorabd`.`Empresa` (`idEmpresa`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Ruta`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `aurorabd`.`Ruta` ;

CREATE TABLE IF NOT EXISTS `aurorabd`.`Ruta` (
  `idRuta` INT NOT NULL AUTO_INCREMENT,
  `Origen` VARCHAR(45) NULL,
  `Destino` VARCHAR(45) NULL,
  PRIMARY KEY (`idRuta`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Vehiculo`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `aurorabd`.`Vehiculo` ;

CREATE TABLE IF NOT EXISTS `aurorabd`.`Vehiculo` (
  `idVehiculo` INT NOT NULL AUTO_INCREMENT,
  `Tipo` VARCHAR(45) NULL,
  `Matricula` VARCHAR(45) NULL,
  `CapacidadMax` DOUBLE NULL,
  `Estado` TINYINT NULL,
  PRIMARY KEY (`idVehiculo`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Pedido`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `aurorabd`.`Pedido` ;

CREATE TABLE IF NOT EXISTS `aurorabd`.`Pedido` (
  `idPedido` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NULL,
  `Volumen` double NOT NULL,
  `Peso` double NOT NULL,
  `EstadoPedido` VARCHAR(45) NULL,
  `FechaDespacho` DATE NULL,
  `idAdministrador` INT NOT NULL,
  `idRuta` INT NOT NULL,
  `idVehiculo` INT NOT NULL,
  `idEmpresa` INT NOT NULL,
  PRIMARY KEY (`idPedido`),
  INDEX `fk_Pedido_Administrador1_idx` (`idAdministrador` ASC) VISIBLE,
  INDEX `fk_Pedido_Ruta1_idx` (`idRuta` ASC) VISIBLE,
  INDEX `fk_Pedido_Vehiculo1_idx` (`idVehiculo` ASC) VISIBLE,
  INDEX `fk_Pedido_Empresa1_idx` (`idEmpresa` ASC) VISIBLE,
  CONSTRAINT `fk_Pedido_Administrador1`
    FOREIGN KEY (`idAdministrador`)
    REFERENCES `aurorabd`.`Administrador` (`idAdministrador`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pedido_Ruta1`
    FOREIGN KEY (`idRuta`)
    REFERENCES `aurorabd`.`Ruta` (`idRuta`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pedido_Vehiculo1`
    FOREIGN KEY (`idVehiculo`)
    REFERENCES `aurorabd`.`Vehiculo` (`idVehiculo`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pedido_Empresa1`
    FOREIGN KEY (`idEmpresa`)
    REFERENCES `aurorabd`.`Empresa` (`idEmpresa`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Conductor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `aurorabd`.`Conductor` (
  `idConductor` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  `Licencia` VARCHAR(45) NULL,
  `Disponibilidad` BOOLEAN NOT NULL DEFAULT 1,
  PRIMARY KEY (`idConductor`)) 
  ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Conductor_has_Vehiculo`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `aurorabd`.`Conductor_has_Vehiculo` ;

CREATE TABLE IF NOT EXISTS `aurorabd`.`Conductor_has_Vehiculo` (
  `idConductor` INT NOT NULL,
  `idVehiculo` INT NOT NULL,
  `FechaAsignado` DATE NULL,
  PRIMARY KEY (`idConductor`, `idVehiculo`),
  INDEX `fk_Conductor_has_Vehiculo_Vehiculo1_idx` (`idVehiculo` ASC) VISIBLE,
  INDEX `fk_Conductor_has_Vehiculo_Conductor1_idx` (`idConductor` ASC) VISIBLE,
  CONSTRAINT `fk_Conductor_has_Vehiculo_Conductor1`
    FOREIGN KEY (`idConductor`)
    REFERENCES `aurorabd`.`Conductor` (`idConductor`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Conductor_has_Vehiculo_Vehiculo1`
    FOREIGN KEY (`idVehiculo`)
    REFERENCES `aurorabd`.`Vehiculo` (`idVehiculo`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`HistorialPedido`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `aurorabd`.`HistorialPedido` ;

CREATE TABLE IF NOT EXISTS `aurorabd`.`HistorialPedido` (
  `idHistorialPedido` INT NOT NULL AUTO_INCREMENT,
  `EstadoAnterior` VARCHAR(45) NULL,
  `EstadoNuevo` VARCHAR(45) NULL,
  `FechaCambio` DATETIME NULL,
  `idPedido` INT NOT NULL,
  PRIMARY KEY (`idHistorialPedido`),
  INDEX `fk_HistorialPedido_Pedido1_idx` (`idPedido` ASC) VISIBLE,
  CONSTRAINT `fk_HistorialPedido_Pedido1`
    FOREIGN KEY (`idPedido`)
    REFERENCES `aurorabd`.`Pedido` (`idPedido`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
