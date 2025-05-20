-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`Empresa`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Empresa` (
  `idEmpresa` INT NOT NULL,
  `Nombre` VARCHAR(45) NULL,
  PRIMARY KEY (`idEmpresa`),
  UNIQUE INDEX `Nombre_UNIQUE` (`Nombre` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Administrador`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Administrador` (
  `idAdministrador` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  `Passworld` VARCHAR(45) NULL,
  `Empresa_idEmpresa` INT NOT NULL,
  PRIMARY KEY (`idAdministrador`),
  INDEX `fk_Administrador_Empresa_idx` (`Empresa_idEmpresa` ASC) VISIBLE,
  CONSTRAINT `fk_Administrador_Empresa`
    FOREIGN KEY (`Empresa_idEmpresa`)
    REFERENCES `mydb`.`Empresa` (`idEmpresa`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Ruta`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Ruta` (
  `idRuta` INT NOT NULL,
  `Origen` VARCHAR(45) NULL,
  `Destino` VARCHAR(45) NULL,
  PRIMARY KEY (`idRuta`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Vehiculo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Vehiculo` (
  `idVehiculo` INT NOT NULL,
  `Tipo` VARCHAR(45) NULL,
  `Matricula` VARCHAR(45) NULL,
  `CapacidadMaz` DOUBLE NULL,
  `Estado` TINYINT NULL,
  PRIMARY KEY (`idVehiculo`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Pedido`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Pedido` (
  `idPedido` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  `Volumen` VARCHAR(45) NULL,
  `Peso` VARCHAR(45) NULL,
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
    REFERENCES `mydb`.`Administrador` (`idAdministrador`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pedido_Pedido2`
    FOREIGN KEY (`EmpresaDestino`)
    REFERENCES `mydb`.`Pedido` (`idPedido`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pedido_Ruta1`
    FOREIGN KEY (`Ruta_idRuta`)
    REFERENCES `mydb`.`Ruta` (`idRuta`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Pedido_Vehiculo1`
    FOREIGN KEY (`Vehiculo_idVehiculo`)
    REFERENCES `mydb`.`Vehiculo` (`idVehiculo`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Conductor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Conductor` (
  `idConductor` INT NOT NULL,
  `Name` VARCHAR(45) NULL,
  `Licencia` VARCHAR(45) NULL,
  `Disponibilidad` TINYINT NULL,
  PRIMARY KEY (`idConductor`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Conductor_has_Vehiculo`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Conductor_has_Vehiculo` (
  `Conductor_idConductor` INT NOT NULL,
  `Vehiculo_idVehiculo` INT NOT NULL,
  `FechaAsignado` DATE NULL,
  PRIMARY KEY (`Conductor_idConductor`, `Vehiculo_idVehiculo`),
  INDEX `fk_Conductor_has_Vehiculo_Vehiculo1_idx` (`Vehiculo_idVehiculo` ASC) VISIBLE,
  INDEX `fk_Conductor_has_Vehiculo_Conductor1_idx` (`Conductor_idConductor` ASC) VISIBLE,
  CONSTRAINT `fk_Conductor_has_Vehiculo_Conductor1`
    FOREIGN KEY (`Conductor_idConductor`)
    REFERENCES `mydb`.`Conductor` (`idConductor`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Conductor_has_Vehiculo_Vehiculo1`
    FOREIGN KEY (`Vehiculo_idVehiculo`)
    REFERENCES `mydb`.`Vehiculo` (`idVehiculo`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`HistorialPedido`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`HistorialPedido` (
  `idHistorialPedido` INT NOT NULL,
  `EstadoAnterior` VARCHAR(45) NULL,
  `EstadoNuevo` VARCHAR(45) NULL,
  `FechaCambio` DATETIME NULL,
  `Pedido_idPedido` INT NOT NULL,
  PRIMARY KEY (`idHistorialPedido`),
  INDEX `fk_HistorialPedido_Pedido1_idx` (`Pedido_idPedido` ASC) VISIBLE,
  CONSTRAINT `fk_HistorialPedido_Pedido1`
    FOREIGN KEY (`Pedido_idPedido`)
    REFERENCES `mydb`.`Pedido` (`idPedido`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
