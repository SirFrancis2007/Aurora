use aurorabd;

-- Trigger para insertar un registro en el historial despues de crear cada pedido
DELIMITER $$
CREATE TRIGGER BefInsHistoriPedido AFTER INSERT ON Pedido
FOR EACH ROW
BEGIN
	DECLARE new_id INT;
    SELECT IFNULL(MAX(idHistorialPedido), 0) + 1 INTO new_id FROM HistorialPedido;
    
    -- Insertamos el registro en el historial
    INSERT INTO HistorialPedido (idHistorialPedido, EstadoAnterior, EstadoNuevo, FechaCambio, Pedido_idPedido)
    VALUES (new_id, NULL, NEW.EstadoPedido, NOW(), NEW.idPedido);
END $$

-- Por cada actulizacion en el estado se registra en el historial del paquete
-- Trigger para insertar un registro en el historial cuando cambia el estado de un pedido
DELIMITER $$
CREATE TRIGGER AftUpdateHistorialPedido AFTER UPDATE ON Pedido
FOR EACH ROW
BEGIN
	DECLARE new_id INT;
    IF NEW.EstadoPedido != OLD.EstadoPedido THEN        
        INSERT INTO HistorialPedido (idHistorialPedido, EstadoAnterior, EstadoNuevo, FechaCambio, Pedido_idPedido)
        VALUES (new_id, OLD.EstadoPedido, NEW.EstadoPedido, NOW(), NEW.idPedido);
    END IF;
END $$

-- Trigger para verificar que un conductor tenga licencia válida antes de asignarle un vehículo
DELIMITER $$
CREATE TRIGGER BefValidarConductor
BEFORE INSERT ON Conductor_has_Vehiculo
FOR EACH ROW
BEGIN
    DECLARE licencia_valida BOOLEAN;
    
    SET licencia_valida = VerificarLicenciaValidaParaVehiculo(NEW.Conductor_idConductor, NEW.Vehiculo_idVehiculo);
    
    IF licencia_valida = FALSE THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El conductor no tiene una licencia';
    END IF;
END $$

-- Trigger para actualizar la disponibilidad del conductor al asignarle un vehículo
DELIMITER $$
CREATE TRIGGER ActualizarDisponibilidadConductor AFTER INSERT ON Conductor_has_Vehiculo
FOR EACH ROW
BEGIN
    UPDATE Conductor
    SET Disponibilidad = 0
    WHERE idConductor = NEW.Conductor_idConductor;
END $$
DELIMITER ;

-- Trigger para restaurar la disponibilidad del conductor al desasignarle un vehículo
DELIMITER $$
CREATE TRIGGER RestaurarDisponibilidadConductor AFTER DELETE ON Conductor_has_Vehiculo
FOR EACH ROW
BEGIN
    UPDATE Conductor
    SET Disponibilidad = 1
    WHERE idConductor = OLD.Conductor_idConductor;
END $$
DELIMITER ;

-- Trigger para verificar la capacidad del vehículo antes de asignarle un pedido
DELIMITER $$
CREATE TRIGGER VerificarCapacidadVehiculo BEFORE INSERT ON Vehiculo_has_Pedido
FOR EACH ROW
BEGIN
    DECLARE capacidad_vehiculo DOUBLE;
    DECLARE peso_pedido DOUBLE;
    DECLARE peso_pedidos_asignados DOUBLE;
    
    -- Se obtine la capacidad maxima del vehiculo
    SELECT CapacidadMaz INTO capacidad_vehiculo
    FROM Vehiculo
    WHERE idVehiculo = NEW.Vehiculo_idVehiculo;
    
    -- Peso del pedido
    SELECT Peso INTO peso_pedido
    FROM Pedido
    WHERE idPedido = NEW.Pedido_idPedido;
    
    -- Suma de los pedidos que ya se encuentra en el vehiculo
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
END $$
DELIMITER ;