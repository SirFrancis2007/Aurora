-- Trigger para insertar un registro en el historial cuando se crea un pedido
DELIMITER $$

DROP TRIGGER IF EXISTS AftInsertPedido $$

CREATE TRIGGER AftInsertPedido AFTER INSERT ON Pedido
FOR EACH ROW
BEGIN
    INSERT INTO HistorialPedido (EstadoAnterior, EstadoNuevo, FechaCambio, idPedido)
    VALUES (NULL, NEW.EstadoPedido, NOW(), NEW.idPedido);
END $$

-- Trigger para insertar un registro en el historial cuando cambia el estado de un pedido
DELIMITER $$

DROP TRIGGER IF EXISTS AftUpdatePedido $$

CREATE TRIGGER AftUpdatePedido AFTER UPDATE ON Pedido
FOR EACH ROW
BEGIN
    IF NEW.EstadoPedido != OLD.EstadoPedido THEN
        INSERT INTO HistorialPedido (EstadoAnterior, EstadoNuevo, FechaCambio, idPedido)
        VALUES (OLD.EstadoPedido, NEW.EstadoPedido, NOW(), NEW.idPedido);
    END IF;
END $$

-- Trigger para verificar que un conductor tenga licencia válida antes de asignarle un vehículo
/*DELIMITER $$

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
END $$*/

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


DELIMITER $$

DROP TRIGGER IF EXISTS VefInsPedido $$

CREATE TRIGGER VefInsPedido BEFORE INSERT ON Pedido
FOR EACH ROW
BEGIN
    DECLARE capacidad_vehiculo DOUBLE;
    DECLARE peso_pedido DOUBLE;
    DECLARE peso_pedidos_asignados DOUBLE;
    DECLARE volumen_pedido DOUBLE;
    DECLARE volumen_pedidos_asignados DOUBLE;

    -- Obtener capacidad del vehículo
    SELECT CapacidadMax INTO capacidad_vehiculo
    FROM Vehiculo
    WHERE idVehiculo = NEW.idVehiculo;

    -- Convertir los valores VARCHAR a DOUBLE para cálculo
    SET peso_pedido = CAST(NEW.Peso AS DECIMAL(10,2));
    SET volumen_pedido = CAST(NEW.Volumen AS DECIMAL(10,2));

    -- Calcular peso total de pedidos asignados (excluyendo entregados)
    SELECT IFNULL(SUM(CAST(Peso AS DECIMAL(10,2))), 0)
    INTO peso_pedidos_asignados
    FROM Pedido
    WHERE idVehiculo = NEW.idVehiculo
    AND EstadoPedido NOT IN ('Entregado', 'Cancelado');

    -- Verificar capacidad de peso
    IF (peso_pedidos_asignados + peso_pedido) > capacidad_vehiculo THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La asignación excede la capacidad máxima de peso del vehículo';
    END IF;
END $$
DELIMITER ;