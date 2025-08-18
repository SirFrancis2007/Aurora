-- Active: 1755368314772@@127.0.0.1@3306@aurorabd
-- =====================================================================
-- TRIGGERS
-- =====================================================================

-- Trigger para insertar un registro en el historial cuando se crea un pedido
DELIMITER $$

DROP TRIGGER IF EXISTS AftInsertPedido $$

CREATE TRIGGER AftInsertPedido AFTER INSERT ON Pedido
FOR EACH ROW
BEGIN
    INSERT INTO HistorialPedido (EstadoAnterior, EstadoNuevo, FechaCambio, Pedido_idPedido)
    VALUES (NULL, NEW.EstadoPedido, NOW(), NEW.idPedido);
END $$

-- Trigger para insertar un registro en el historial cuando cambia el estado de un pedido
DELIMITER $$

DROP TRIGGER IF EXISTS AftUpdatePedido $$

CREATE TRIGGER AftUpdatePedido AFTER UPDATE ON Pedido
FOR EACH ROW
BEGIN
    IF NEW.EstadoPedido != OLD.EstadoPedido THEN
        INSERT INTO HistorialPedido (EstadoAnterior, EstadoNuevo, FechaCambio, Pedido_idPedido)
        VALUES (OLD.EstadoPedido, NEW.EstadoPedido, NOW(), NEW.idPedido);
    END IF;
END $$

-- Trigger para verificar que un conductor tenga licencia válida antes de asignarle un vehículo
DELIMITER $$

DROP TRIGGER IF EXISTS BefInsertConductorVehiculo $$

CREATE TRIGGER BefInsertConductorVehiculo BEFORE INSERT ON Conductor_has_Vehiculo
FOR EACH ROW
BEGIN
    DECLARE licencia_valida BOOLEAN;

    SET licencia_valida = VerificarLicenciaValidaParaVehiculo(NEW.idConductor, NEW.idVehiculo);

    IF licencia_valida = FALSE THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El conductor no tiene una licencia válida para este tipo de vehículo';
    END IF;
END $$

-- Trigger para actualizar la disponibilidad del conductor al asignarle un vehículo
DELIMITER $$

DROP TRIGGER IF EXISTS AftInsertConductorVehiculo $$

CREATE TRIGGER AftInsertConductorVehiculo AFTER INSERT ON Conductor_has_Vehiculo
FOR EACH ROW
BEGIN
    -- Al asignar un vehículo, el conductor ya no está disponible para otros vehículos
    UPDATE Conductor
    SET Disponibilidad = 0
    WHERE idConductor = NEW.idConductor;
END $$


-- Trigger para verificar la capacidad del vehículo antes de asignarle un pedido
DELIMITER $$

DROP TRIGGER IF EXISTS VefInsPedido $$

CREATE TRIGGER VefInsPedido BEFORE INSERT ON Pedido
FOR EACH ROW
BEGIN
    DECLARE capacidad_vehiculo DOUBLE;
    DECLARE peso_pedido DOUBLE;
    DECLARE peso_pedidos_asignados DOUBLE;

    SELECT CapacidadMaz INTO capacidad_vehiculo
    FROM Vehiculo
    WHERE idVehiculo = NEW.idVehiculo;

    SELECT SUM(Peso)
    INTO peso_pedidos_asignados
    FROM Pedido
    WHERE idVehiculo = NEW.idVehiculo
    AND EstadoPedido NOT IN ('Entregado');

    IF (peso_pedidos_asignados + peso_pedido) > capacidad_vehiculo THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La asignación excede la capacidad máxima del vehículo';
    END IF;
END $$