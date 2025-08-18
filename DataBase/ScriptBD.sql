SET @OLD_UNIQUE_CHECKS = @@UNIQUE_CHECKS, UNIQUE_CHECKS = 0;

SET
    @OLD_FOREIGN_KEY_CHECKS = @@FOREIGN_KEY_CHECKS,
    FOREIGN_KEY_CHECKS = 0;

SET
    @OLD_SQL_MODE = @@SQL_MODE,
    SQL_MODE = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema AuroraBD
-- -----------------------------------------------------
DROP DATABASE IF EXISTS `AuroraBD`;

CREATE SCHEMA IF NOT EXISTS `AuroraBD` DEFAULT CHARACTER SET utf8;

USE `AuroraBD`;

-- -----------------------------------------------------
-- Table Empresa
-- -----------------------------------------------------
CREATE TABLE Empresa (
    idEmpresa INT NOT NULL AUTO_INCREMENT,
    Nombre VARCHAR(45) NULL,
    PRIMARY KEY (idEmpresa),
    UNIQUE INDEX Nombre_UNIQUE (Nombre ASC) VISIBLE
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table Administrador
-- -----------------------------------------------------
CREATE TABLE Administrador (
    idAdministrador INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(45) NULL,
    Passworld VARCHAR(45) NULL,
    idEmpresa INT NOT NULL,
    PRIMARY KEY (idAdministrador),
    INDEX fk_Administrador_Empresa_idx (idEmpresa ASC) VISIBLE,
    CONSTRAINT fk_Administrador_Empresa FOREIGN KEY (idEmpresa) REFERENCES Empresa (idEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table Ruta
-- -----------------------------------------------------
CREATE TABLE Ruta (
    idRuta INT NOT NULL AUTO_INCREMENT,
    Origen VARCHAR(45) NULL,
    Destino VARCHAR(45) NULL,
    PRIMARY KEY (idRuta)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table Vehiculo
-- -----------------------------------------------------
CREATE TABLE Vehiculo (
    idVehiculo INT NOT NULL AUTO_INCREMENT,
    Tipo VARCHAR(45) NULL,
    Matricula VARCHAR(45) NULL,
    CapacidadMaz DOUBLE NULL,
    Estado TINYINT NULL,
    PRIMARY KEY (idVehiculo)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table Pedido
-- -----------------------------------------------------
CREATE TABLE Pedido (
    idPedido INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(45) NULL,
    Volumen VARCHAR(45) NULL,
    Peso VARCHAR(45) NULL,
    EstadoPedido VARCHAR(45) NULL,
    FechaDespacho DATE NULL,
    idAdministrador INT NOT NULL,
    idEmpresa INT NOT NULL,
    idRuta INT NOT NULL,
    idVehiculo INT NOT NULL,
    PRIMARY KEY (idPedido),
    INDEX fk_Pedido_Administrador_idx (idAdministrador ASC) VISIBLE,
    INDEX fk_Pedido_Empresa_idx (idEmpresa ASC) VISIBLE,
    INDEX fk_Pedido_Ruta_idx (idRuta ASC) VISIBLE,
    INDEX fk_Pedido_Vehiculo_idx (idVehiculo ASC) VISIBLE,
    CONSTRAINT fk_Pedido_Administrador FOREIGN KEY (idAdministrador) REFERENCES Administrador (idAdministrador),
    CONSTRAINT fk_Pedido_Empresa FOREIGN KEY (idEmpresa) REFERENCES Empresa (idEmpresa),
    CONSTRAINT fk_Pedido_Ruta FOREIGN KEY (idRuta) REFERENCES Ruta (idRuta),
    CONSTRAINT fk_Pedido_Vehiculo FOREIGN KEY (idVehiculo) REFERENCES Vehiculo (idVehiculo)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table Conductor
-- -----------------------------------------------------
CREATE TABLE Conductor (
    idConductor INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(45) NULL,
    Licencia VARCHAR(45) NULL,
    Disponibilidad TINYINT NULL,
    PRIMARY KEY (idConductor)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table Conductor_has_Vehiculo
-- -----------------------------------------------------
CREATE TABLE Conductor_has_Vehiculo (
    idConductor INT NOT NULL,
    idVehiculo INT NOT NULL,
    FechaAsignado DATE NULL,
    PRIMARY KEY (idConductor, idVehiculo),
    INDEX fk_Conductor_has_Vehiculo_Vehiculo_idx (idVehiculo ASC) VISIBLE,
    INDEX fk_Conductor_has_Vehiculo_Conductor_idx (idConductor ASC) VISIBLE,
    CONSTRAINT fk_Conductor_has_Vehiculo_Conductor FOREIGN KEY (idConductor) REFERENCES Conductor (idConductor),
    CONSTRAINT fk_Conductor_has_Vehiculo_Vehiculo FOREIGN KEY (idVehiculo) REFERENCES Vehiculo (idVehiculo)
) ENGINE = InnoDB;

-- -----------------------------------------------------
-- Table HistorialPedido
-- -----------------------------------------------------
CREATE TABLE HistorialPedido (
    idHistorialPedido INT NOT NULL AUTO_INCREMENT,
    EstadoAnterior VARCHAR(45) NULL,
    EstadoNuevo VARCHAR(45) NULL,
    FechaCambio DATETIME NULL,
    idPedido INT NOT NULL,
    PRIMARY KEY (idHistorialPedido),
    INDEX fk_HistorialPedido_Pedido_idx (idPedido ASC) VISIBLE,
    CONSTRAINT fk_HistorialPedido_Pedido FOREIGN KEY (idPedido) REFERENCES Pedido (idPedido)
) ENGINE = InnoDB;

SET SQL_MODE = @OLD_SQL_MODE;

SET FOREIGN_KEY_CHECKS = @OLD_FOREIGN_KEY_CHECKS;

SET UNIQUE_CHECKS = @OLD_UNIQUE_CHECKS;