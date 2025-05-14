use aurorabd;

-- =====================================================================
-- TRIGGERS
-- =====================================================================

-- Trigger para insertar un registro en el historial cuando se crea un pedido
DELIMITER //
CREATE TRIGGER InsertHistorialPedido AFTER INSERT ON Pedido
FOR EACH ROW
BEGIN
	DECLARE new_id INT;
    SELECT IFNULL(MAX(idHistorialPedido), 0) + 1 INTO new_id FROM HistorialPedido;
    
    -- Insertamos el registro en el historial
    INSERT INTO HistorialPedido (idHistorialPedido, EstadoAnterior, EstadoNuevo, FechaCambio, Pedido_idPedido)
    VALUES (new_id, NULL, NEW.EstadoPedido, NOW(), NEW.idPedido);
END //

-- Trigger para insertar un registro en el historial cuando cambia el estado de un pedido
DELIMITER //
CREATE TRIGGER UpdateHistorialPedido AFTER UPDATE ON Pedido
FOR EACH ROW
BEGIN
	DECLARE new_id INT;
    -- Solo si cambia el estado
    IF NEW.EstadoPedido != OLD.EstadoPedido THEN
        -- Generamos un ID para el historial (en producción, podría ser auto-incremento)
        SELECT IFNULL(MAX(idHistorialPedido), 0) + 1 INTO new_id FROM HistorialPedido;
        
        -- Insertamos el registro en el historial
        INSERT INTO HistorialPedido (idHistorialPedido, EstadoAnterior, EstadoNuevo, FechaCambio, Pedido_idPedido)
        VALUES (new_id, OLD.EstadoPedido, NEW.EstadoPedido, NOW(), NEW.idPedido);
    END IF;
END //
DELIMITER ;

-- Trigger para verificar que un conductor tenga licencia válida antes de asignarle un vehículo
DELIMITER //
CREATE TRIGGER ValidarLicenciaAnteAsignacion
BEFORE INSERT ON Conductor_has_Vehiculo
FOR EACH ROW
BEGIN
    DECLARE licencia_valida BOOLEAN;
    
    SET licencia_valida = VerificarLicenciaValidaParaVehiculo(NEW.Conductor_idConductor, NEW.Vehiculo_idVehiculo);
    
    IF licencia_valida = FALSE THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El conductor no tiene una licencia válida para este tipo de vehículo';
    END IF;
END //
DELIMITER ;

-- Trigger para actualizar la disponibilidad del conductor al asignarle un vehículo
DELIMITER //
CREATE TRIGGER ActualizarDisponibilidadConductor AFTER INSERT ON Conductor_has_Vehiculo
FOR EACH ROW
BEGIN
    -- Al asignar un vehículo, el conductor ya no está disponible para otros vehículos
    UPDATE Conductor
    SET Disponibilidad = 0
    WHERE idConductor = NEW.Conductor_idConductor;
END //
DELIMITER ;

-- Trigger para restaurar la disponibilidad del conductor al desasignarle un vehículo
DELIMITER //
CREATE TRIGGER RestaurarDisponibilidadConductor AFTER DELETE ON Conductor_has_Vehiculo
FOR EACH ROW
BEGIN
    -- Al desasignar un vehículo, el conductor vuelve a estar disponible
    UPDATE Conductor
    SET Disponibilidad = 1
    WHERE idConductor = OLD.Conductor_idConductor;
END //
DELIMITER ;

-- Trigger para verificar la capacidad del vehículo antes de asignarle un pedido
DELIMITER //
CREATE TRIGGER VerificarCapacidadVehiculo BEFORE INSERT ON Vehiculo_has_Pedido
FOR EACH ROW
BEGIN
    DECLARE capacidad_vehiculo DOUBLE;
    DECLARE peso_pedido DOUBLE;
    DECLARE peso_pedidos_asignados DOUBLE;
    
    -- Obtenemos la capacidad del vehículo
    SELECT CapacidadMaz INTO capacidad_vehiculo
    FROM Vehiculo
    WHERE idVehiculo = NEW.Vehiculo_idVehiculo;
    
    -- Obtenemos el peso del pedido (convertimos de VARCHAR a DOUBLE)
    SELECT Peso INTO peso_pedido
    FROM Pedido
    WHERE idPedido = NEW.Pedido_idPedido;
    
    -- Calculamos el peso total de los pedidos ya asignados al vehículo
    SELECT SUM (p.Peso) INTO peso_pedidos_asignados
    FROM Vehiculo_has_Pedido vp
    JOIN Pedido p ON vp.Pedido_idPedido = p.idPedido
    WHERE vp.Vehiculo_idVehiculo = NEW.Vehiculo_idVehiculo
    AND p.EstadoPedido NOT IN ('Entregado', 'Cancelado');
    
    -- Verificamos si excede la capacidad
    IF (peso_pedidos_asignados + peso_pedido) > capacidad_vehiculo THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La asignación excede la capacidad máxima del vehículo';
    END IF;
END //
DELIMITER ;