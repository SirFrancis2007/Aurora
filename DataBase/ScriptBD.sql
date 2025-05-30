-- Active: 1748614287392@@127.0.0.1@3306@aurorabd
-- MySQL Workbench Forward Engineering
/*
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';
*/
-- ------------------------------------------------------ñ-
-- Schema aurorabd
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema aurorabd
-- -----------------------------------------------------
DROP DATABASE IF EXISTS `aurorabd` ;
CREATE SCHEMA `aurorabd` DEFAULT CHARACTER SET utf8 ;
USE `aurorabd` ;

-- -----------------------------------------------------
-- Table `aurorabd`.`Empresa`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `aurorabd`.`Empresa` (
  `idEmpresa` INT auto_increment,
  `Nombre` VARCHAR(45) NULL,
  PRIMARY KEY (`idEmpresa`),
  UNIQUE INDEX `Nombre_UNIQUE` (`Nombre` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Administrador`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `aurorabd`.`Administrador` (
  `idAdministrador` INT NOT NULL auto_increment,
  `Name` VARCHAR(45) NULL,
  `Passworld` VARCHAR(45) NULL,
  `Empresa_idEmpresa` INT NOT NULL,
  PRIMARY KEY (`idAdministrador`),
  INDEX `fk_Administrador_Empresa_idx` (`Empresa_idEmpresa` ASC) VISIBLE,
  CONSTRAINT `fk_Administrador_Empresa`
    FOREIGN KEY (`Empresa_idEmpresa`)
    REFERENCES `aurorabd`.`Empresa` (`idEmpresa`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Ruta`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `aurorabd`.`Ruta` (
  `idRuta` INT NOT NULL auto_increment,
  `Origen` VARCHAR(45) NULL,
  `Destino` VARCHAR(45) NULL,
  PRIMARY KEY (`idRuta`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Vehiculo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `aurorabd`.`Vehiculo` (
  `idVehiculo` INT NOT NULL auto_increment,
  `Tipo` VARCHAR(45) NULL,
  `Matricula` VARCHAR(45) NULL,
  `CapacidadMaz` DOUBLE NULL,
  `Estado` TINYINT NULL,
  PRIMARY KEY (`idVehiculo`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Pedido`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `aurorabd`.`Pedido` (
  `idPedido` INT NOT NULL auto_increment,
  `Name` VARCHAR(45) NULL,
  `Volumen` DOUBLE NULL,
  `Peso` DOUBLE NULL,
  `EstadoPedido` VARCHAR(45) NULL,
  `FechaDespacho` DATE NULL,
  `Administrador_idAdministrador` INT NOT NULL,
  `EmpresaDestino` INT NOT NULL,
  `Ruta_idRuta` INT NOT NULL,
  `Vehiculo_idVehiculo` INT NOT NULL,
  PRIMARY KEY (`idPedido`),
  INDEX `fk_Pedido_Administrador1_idx` (`Administrador_idAdministrador` ASC) VISIBLE,
  INDEX `fk_Pedido_Pedido2_idx` (`EmpresaDestino` ASC) VISIBLE,
  INDEX `fk_Pedido_Ruta1_idx` (`Ruta_idRuta` ASC) VISIBLE,
  INDEX `fk_Pedido_Vehiculo1_idx` (`Vehiculo_idVehiculo` ASC) VISIBLE,
  CONSTRAINT `fk_Pedido_Administrador1`
    FOREIGN KEY (`Administrador_idAdministrador`)
    REFERENCES `aurorabd`.`Administrador` (`idAdministrador`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pedido_Pedido2`
    FOREIGN KEY (`EmpresaDestino`)
    REFERENCES `aurorabd`.`Pedido` (`idPedido`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pedido_Ruta1`
    FOREIGN KEY (`Ruta_idRuta`)
    REFERENCES `aurorabd`.`Ruta` (`idRuta`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pedido_Vehiculo1`
    FOREIGN KEY (`Vehiculo_idVehiculo`)
    REFERENCES `aurorabd`.`Vehiculo` (`idVehiculo`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Conductor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `aurorabd`.`Conductor` (
  `idConductor` INT NOT NULL auto_increment,
  `Name` VARCHAR(45) NULL,
  `Licencia` VARCHAR(45) NULL,
  `Disponibilidad` TINYINT NULL,
  PRIMARY KEY (`idConductor`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`Conductor_has_Vehiculo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `aurorabd`.`Conductor_has_Vehiculo` (
  `Conductor_idConductor` INT NOT NULL,
  `Vehiculo_idVehiculo` INT NOT NULL,
  `FechaAsignado` DATE NULL,
  PRIMARY KEY (`Conductor_idConductor`, `Vehiculo_idVehiculo`),
  INDEX `fk_Conductor_has_Vehiculo_Vehiculo1_idx` (`Vehiculo_idVehiculo` ASC) VISIBLE,
  INDEX `fk_Conductor_has_Vehiculo_Conductor1_idx` (`Conductor_idConductor` ASC) VISIBLE,
  CONSTRAINT `fk_Conductor_has_Vehiculo_Conductor1`
    FOREIGN KEY (`Conductor_idConductor`)
    REFERENCES `aurorabd`.`Conductor` (`idConductor`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Conductor_has_Vehiculo_Vehiculo1`
    FOREIGN KEY (`Vehiculo_idVehiculo`)
    REFERENCES `aurorabd`.`Vehiculo` (`idVehiculo`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `aurorabd`.`HistorialPedido`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `aurorabd`.`HistorialPedido` (
  `idHistorialPedido` INT NOT NULL auto_increment,
  `EstadoAnterior` VARCHAR(45) NULL,
  `EstadoNuevo` VARCHAR(45) NULL,
  `FechaCambio` DATETIME NULL,
  `Pedido_idPedido` INT NOT NULL,
  PRIMARY KEY (`idHistorialPedido`),
  INDEX `fk_HistorialPedido_Pedido1_idx` (`Pedido_idPedido` ASC) VISIBLE,
  CONSTRAINT `fk_HistorialPedido_Pedido1`
    FOREIGN KEY (`Pedido_idPedido`)
    REFERENCES `aurorabd`.`Pedido` (`idPedido`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

/*
SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
*/