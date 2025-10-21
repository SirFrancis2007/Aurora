DELIMITER $$
Drop PROCEDURE IF EXISTS  SPCrearVehiculo $$
CREATE PROCEDURE SPCrearVehiculo(
    OUT xidVehiculo INT,
    xTipo VARCHAR(45),
    xMatricula VARCHAR(45),
    xCapacidadMax DOUBLE,
    xEstado TINYINT
)
BEGIN
    INSERT INTO Vehiculo (Tipo, Matricula, CapacidadMax, Estado)
    VALUES (xTipo, xMatricula, xCapacidadMax, xEstado);
    
    set xidVehiculo = last_insert_id();
END $$
DELIMITER ;

DELIMITER $$
Drop PROCEDURE IF EXISTS  SPDelVehiculo $$
CREATE PROCEDURE SPDelVehiculo (xidVehiculo INT)
BEGIN
    -- Aca Verificamos si el vehículo tiene pedidos asignados
    DECLARE asignaciones_pedidos INT;
    
    SELECT COUNT(*) INTO asignaciones_pedidos 
    FROM pedido 
    WHERE pedido.idVehiculo = xidVehiculo;
    
    IF asignaciones_pedidos > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede eliminar el vehículo porque tiene pedidos asignados';
    ELSE
        DELETE FROM Vehiculo WHERE Vehiculo.idVehiculo = xidVehiculo;
    END IF;
END $$
DELIMITER ;

DELIMITER $$
Drop PROCEDURE IF EXISTS  UpdateVehiculo $$
CREATE PROCEDURE UpdateVehiculo(xidVehiculo INT, xMatricula VARCHAR(45)
)
BEGIN
    UPDATE Vehiculo
    SET Matricula = xMatricula
    WHERE idVehiculo = xidVehiculo;
END $$
DELIMITER $$

DELIMITER $$
Drop PROCEDURE IF EXISTS  SPActualizarEstadoVehiculo $$
CREATE PROCEDURE SPActualizarEstadoVehiculo(xidVehiculo INT, xdisponible BOOLEAN)
BEGIN
    UPDATE Vehiculo
    SET Estado = xdisponible 
    WHERE idVehiculo = xidVehiculo;
END $$

DELIMITER $$

DROP PROCEDURE IF EXISTS SPRestaurarPesoVehiculo $$

CREATE PROCEDURE SPRestaurarPesoVehiculo (xidVehiculo INT, xidpedido INT)
BEGIN
    DECLARE v_peso DECIMAL;
    DECLARE v_capacidadActual DECIMAL;

    SELECT CapacidadMax INTO v_capacidadActual
    FROM Vehiculo
    WHERE IdVehiculo = xidVehiculo;

    SELECT Peso INTO v_peso
    FROM Pedido
    WHERE IdPedido = xidpedido;

    IF v_peso IS NOT NULL AND v_capacidadActual IS NOT NULL THEN
        UPDATE Vehiculo
        SET CapacidadMax = v_capacidadActual + v_peso
        WHERE IdVehiculo = xidVehiculo;
    END IF;
END $$
DELIMITER ;

DELIMITER $$

DROP PROCEDURE IF EXISTS SPRestarPesoVehiculo $$

CREATE PROCEDURE SPRestarPesoVehiculo (IN xidVehiculo INT,IN xidpedido INT)
BEGIN
    DECLARE v_peso DECIMAL(10,2);
    DECLARE v_capacidadMax DECIMAL(10,2);

    SELECT CapacidadMax INTO v_capacidadMax
    FROM Vehiculo
    WHERE IdVehiculo = xidVehiculo;

    SELECT Peso INTO v_peso
    FROM Pedido
    WHERE IdPedido = xidpedido;

    IF v_peso IS NOT NULL AND v_capacidadMax IS NOT NULL THEN
        UPDATE Vehiculo
        SET CapacidadMax = v_capacidadMax - v_peso
        WHERE IdVehiculo = xidVehiculo;
    END IF;
END $$
DELIMITER ;


-- --------------------------------------------------------------------------------------------

-- =====================================================================
-- PROCEDIMIENTOS PARA ASIGNACIÓN DE VEHÍCULOS A CONDUCTORES
-- =====================================================================

-- Procedimiento para asignar un vehículo a un conductor
DELIMITER $$
DROP PROCEDURE IF EXISTS AsignarVehiculoAConductor $$
CREATE PROCEDURE AsignarVehiculoAConductor(
    xidConductor INT,
    xidVehiculo INT
)
BEGIN
    DECLARE conductor_disponible TINYINT;
    DECLARE vehiculo_disponible TINYINT;
    
    -- Verificar disponibilidad del conductor
    SELECT Disponibilidad INTO conductor_disponible
    FROM Conductor
    WHERE idConductor = xidConductor;
    
    -- Verificar estado del vehículo
    SELECT Estado INTO vehiculo_disponible
    FROM Vehiculo
    WHERE idVehiculo = xidVehiculo;
    
    IF conductor_disponible != 1 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El conductor no está disponible para asignaciones';
    ELSEIF vehiculo_disponible != 1 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El vehículo no está disponible para asignaciones';
    ELSE
        INSERT INTO Conductor_has_Vehiculo (
            idConductor,
            idVehiculo,
            FechaAsignado
        )
        VALUES (xidConductor, xidVehiculo, CURDATE());
    END IF;
END $$
DELIMITER ;

-- Procedimiento para desasignar un vehículo de un conductor
DELIMITER $$
DROP PROCEDURE IF EXISTS SPDesasignarVehiculoAConductor $$
CREATE PROCEDURE SPDesasignarVehiculoAConductor(
    xidConductor INT,
    xidVehiculo INT
)
BEGIN
    DECLARE esDisponible INT;

    SELECT v.Estado INTO esDisponible
    FROM vehiculo v
    WHERE idVehiculo = xidVehiculo;

    IF esDisponible = 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede desasignar el vehículo porque tiene pedidos activos';
    ELSE
        DELETE FROM Conductor_has_Vehiculo
        WHERE idConductor = xidConductor
        AND idVehiculo = xidVehiculo;
        
        UPDATE Conductor
        SET Disponibilidad = 1
        WHERE idConductor = xidConductor;

        UPDATE Vehiculo
        SET Estado = 0
        WHERE idVehiculo = xidVehiculo;
    END IF;
END $$
DELIMITER ;