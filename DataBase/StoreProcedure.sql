USE aurorabd;

DELIMITER $$
CREATE PROCEDURE PSCrearEmpresa(OUT xidEmpresa INT, IN xNombre VARCHAR(45))
BEGIN
    INSERT INTO Empresa (Nombre)
    VALUES (xNombre);
    set xidEmpresa = last_insert_id();
END $$

DELIMITER $$
CREATE PROCEDURE SPNuevoAdministrador(	out xidAdministrador INT, xName VARCHAR(45), xPassword VARCHAR(45), xEmpresa_idEmpresa INT)
BEGIN
    INSERT INTO Administrador (Name, Passworld, Empresa_idEmpresa)
    VALUES (xName, xPassword, xEmpresa_idEmpresa);
    set xidAdministrador = last_insert_id();
END $$

DELIMITER $$
CREATE PROCEDURE SPDelEmpresa (IN xidEmpresa INT)
BEGIN
    START TRANSACTION;
    
    DELETE hp FROM HistorialPedido hp
    INNER JOIN Pedido p ON hp.Pedido_idPedido = p.idPedido
    WHERE p.EmpresaDestino = xidEmpresa OR p.Administrador_idAdministrador IN (
        SELECT idAdministrador FROM Administrador WHERE Empresa_idEmpresa = xidEmpresa
    );
    
    DELETE vp FROM Vehiculo_has_Pedido vp
    INNER JOIN Pedido p ON vp.Pedido_idPedido = p.xidEmpresa
    WHERE p.EmpresaDestino = xidEmpresa OR p.Administrador_idAdministrador IN (
        SELECT idAdministrador FROM Administrador WHERE Empresa_idEmpresa = xidEmpresa
    );
    
    DELETE FROM Pedido 
    WHERE EmpresaDestino = p_idEmpresa OR Administrador_idAdministrador IN (
        SELECT idAdministrador FROM Administrador WHERE Empresa_idEmpresa = xidEmpresa
    );
    
    DELETE FROM Administrador 
    WHERE Empresa_idEmpresa = xidEmpresa;
    
    DELETE FROM Empresa 
    WHERE idEmpresa = xidEmpresa;
    
    COMMIT;
END $$

DELIMITER $$
CREATE PROCEDURE SPDelAdministrador( IN p_idAdministrador INT)
BEGIN
    -- Se verifica que el administrador no tenga paquetes asignados todavia
    DECLARE pedidos_count INT;
    
    SELECT COUNT(*) INTO pedidos_count 
    FROM Pedido 
    WHERE Administrador_idAdministrador = p_idAdministrador;
    
    IF pedidos_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede eliminar el administrador porque tiene pedidos asociados';
    ELSE
        DELETE FROM Administrador 
        WHERE idAdministrador = p_idAdministrador;
    END IF;
END $$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE SPActAdmi(
    IN p_idAdministrador INT,
    IN p_Name VARCHAR(45),
    IN p_Password VARCHAR(45)
)
BEGIN
    UPDATE Administrador
    SET Name = p_Name,
        Passworld = p_Password
    WHERE idAdministrador = p_idAdministrador;
END $$
DELIMITER ;


-- ======================================
-- Conductor
-- ======================================
DELIMITER $$
CREATE PROCEDURE SPNewConductor(
    OUT p_idConductor INT,
    IN p_Name VARCHAR(45),
    IN p_Licencia VARCHAR(45),
    IN p_Disponibilidad TINYINT
)
BEGIN
    INSERT INTO Conductor (Name, Licencia, Disponibilidad)
    VALUES (p_Name, p_Licencia, p_Disponibilidad);
    
    set p_idConductor = last_insert_id();
END $$
DELIMITER ;

-- Procedimiento para eliminar un conductor
DELIMITER $$
CREATE PROCEDURE SPDelConductor( xidConductor INT)
BEGIN
    -- Se verifica si el conductor tiene asignaciones de vehículos
    DECLARE asignaciones_count INT;
    
    SELECT COUNT(*) INTO asignaciones_count 
    FROM Conductor_has_Vehiculo 
    WHERE Conductor_idConductor = xidConductor;
    
    IF asignaciones_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede eliminar el conductor porque tiene vehículos asignados';
    ELSE
        DELETE FROM Conductor WHERE idConductor = xidConductor;
    END IF;
END $$
DELIMITER ;

-- Procedimiento para actualizar datos de un conductor
DELIMITER $$
CREATE PROCEDURE UpdateConductor( 	IN p_idConductor INT,
									IN p_Name VARCHAR(45),
									IN p_Licencia VARCHAR(45),
									IN p_Disponibilidad TINYINT
)
BEGIN
    UPDATE Conductor
    SET Name = p_Name,
        Licencia = p_Licencia,
        Disponibilidad = p_Disponibilidad
    WHERE idConductor = p_idConductor;
END $$
DELIMITER ;

-- =====================================================================
-- PROCEDIMIENTOS PARA GESTIÓN DE VEHÍCULOS
-- =====================================================================

-- Procedimiento para crear un nuevo vehículo (por administrador)
DELIMITER $$
CREATE PROCEDURE SPCrearVehiculo(
    OUT xidVehiculo INT,
    IN xTipo VARCHAR(45),
    IN xMatricula VARCHAR(45),
    IN xCapacidadMax DOUBLE,
    IN xEstado TINYINT
)
BEGIN
    INSERT INTO Vehiculo (Tipo, Matricula, CapacidadMaz, Estado)
    VALUES (xTipo, xMatricula, xCapacidadMax, xEstado);
    
    set xidVehiculo = last_insert_id();
END $$
DELIMITER ;

-- Procedimiento para eliminar un vehículo
DELIMITER $$
CREATE PROCEDURE SPDelVehiculo(IN xidVehiculo INT)
BEGIN
    -- Se Verifica que el vehiculo no posea actualmente un conductor asignado
    DECLARE asignaciones_conductores INT;
    
    -- Aca Verificamos si el vehículo tiene pedidos asignados
    DECLARE asignaciones_pedidos INT;
    
    SELECT COUNT(*) INTO asignaciones_conductores 
    FROM Conductor_has_Vehiculo 
    WHERE Vehiculo_idVehiculo = xidVehiculo;
    
    SELECT COUNT(*) INTO asignaciones_pedidos 
    FROM Vehiculo_has_Pedido 
    WHERE Vehiculo_idVehiculo = xidVehiculo;
    
    IF asignaciones_conductores > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede eliminar el vehículo porque está asignado a uno o más conductores';
    ELSEIF asignaciones_pedidos > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede eliminar el vehículo porque tiene pedidos asignados';
    ELSE
        DELETE FROM Vehiculo WHERE idVehiculo = xidVehiculo;
    END IF;
END $$
DELIMITER ;

-- Procedimiento para actualizar datos de un vehículo
DELIMITER $$
CREATE PROCEDURE UpdateVehiculo(IN p_idVehiculo INT,
								IN p_Tipo VARCHAR(45),
								IN p_Matricula VARCHAR(45),
								IN p_CapacidadMax DOUBLE,
								IN p_Estado TINYINT
)
BEGIN
    UPDATE Vehiculo
    SET Tipo = p_Tipo,
        Matricula = p_Matricula,
        CapacidadMaz = p_CapacidadMax,
        Estado = p_Estado
    WHERE idVehiculo = p_idVehiculo;
END $$
DELIMITER ;

DELIMITER //

CREATE PROCEDURE SPActualizarEstadoVehiculo(xidVehiculo INT, xdisponible BOOLEAN)
BEGIN
    UPDATE Vehiculo
    SET Estado = CASE
        WHEN xdisponible THEN 0
        ELSE 1
    END
    WHERE idVehiculo = xidVehiculo;
END //

-- =====================================================================
-- PROCEDIMIENTOS PARA ASIGNACIÓN DE VEHÍCULOS A CONDUCTORES
-- =====================================================================

-- Procedimiento para asignar un vehículo a un conductor
DELIMITER $$
CREATE PROCEDURE AsignarVehiculoAConductor(
    IN xidConductor INT,
    IN xidVehiculo INT
)
BEGIN
    -- Verificamos disponibilidad del conductor
    DECLARE conductor_disponible TINYINT;
    
    -- Verificamos estado del vehículo
    DECLARE vehiculo_disponible TINYINT;
    
    SELECT Disponibilidad INTO conductor_disponible 
    FROM Conductor 
    WHERE idConductor = xidConductor;
    
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
        INSERT INTO Conductor_has_Vehiculo (Conductor_idConductor, Vehiculo_idVehiculo, FechaAsignado)
        VALUES (xidConductor, xidVehiculo, CURDATE());
    END IF;
END $$
DELIMITER ;

-- Procedimiento para desasignar un vehículo de un conductor
DELIMITER $$
CREATE PROCEDURE SPDesasignarVehiculoAConductor(
    IN xidConductor INT,
    IN xidVehiculo INT
)
BEGIN
    -- Verificamos si el vehículo tiene pedidos asignados actualmente
    DECLARE pedidos_activos INT;
    
    SELECT COUNT(*) INTO pedidos_activos 
    FROM Vehiculo_has_Pedido vp
    JOIN Pedido p ON vp.Pedido_idPedido = p.idPedido
    WHERE vp.Vehiculo_idVehiculo = xidVehiculo
    AND p.EstadoPedido NOT IN ('Entregado', 'Cancelado');
    
    IF pedidos_activos > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede desasignar el vehículo porque tiene pedidos activos';
    ELSE
        DELETE FROM Conductor_has_Vehiculo 
        WHERE Conductor_idConductor = xidConductor 
        AND Vehiculo_idVehiculo = xidVehiculo;
    END IF;
END $$
DELIMITER ;

-- =====================================================================
-- PROCEDIMIENTOS PARA GESTIÓN DE RUTAS
-- =====================================================================

-- Procedimiento para crear una nueva ruta
DELIMITER $$
CREATE PROCEDURE SPCrearRuta(
    OUT xidRuta INT,
    IN xOrigen VARCHAR(45),
    IN xDestino VARCHAR(45)
)
BEGIN
    INSERT INTO Ruta (Origen, Destino)
    VALUES (xOrigen, xDestino);
    
    set xidRuta = last_insert_id();
END $$
DELIMITER ;

-- =====================================================================
-- PROCEDIMIENTOS PARA GESTIÓN DE PEDIDOS
-- =====================================================================

-- Procedimiento para crear un nuevo pedido
DELIMITER $$
CREATE PROCEDURE SPCrearPedido(
    OUT xidPedido INT,
    IN xName VARCHAR(45),
    IN xVolumen VARCHAR(45),
    IN xPeso VARCHAR(45),
    IN xEstadoPedido VARCHAR(45),
    IN xFechaDespacho DATE,
    IN xAdministrador_idAdministrador INT,
    IN xEmpresaDestino INT,
    IN xRuta_idRuta INT
)
BEGIN
    -- Creamos el pedido con estado inicial
    INSERT INTO Pedido (
        Name, Volumen, Peso, EstadoPedido, FechaDespacho,
        Administrador_idAdministrador, EmpresaDestino, Ruta_idRuta
    )
    VALUES (
        xName, xVolumen, xPeso, xEstadoPedido, xFechaDespacho,
        xAdministrador_idAdministrador, xEmpresaDestino, xRuta_idRuta
    );
    
    set xidPedido = last_insert_id();
    
    -- Aclaracion: El trigger InsertHistorialPedido se encargará de crear el registro en el historial
END $$
DELIMITER ;

-- Procedimiento para actualizar el estado de un pedido
DELIMITER $$
CREATE PROCEDURE SPUpdateEstadoPedido(
    IN xidPedido INT,
    IN xNuevoEstado VARCHAR(45)
)
BEGIN
    -- Obtenemos el estado actual
    DECLARE estado_actual VARCHAR(45);
    
    SELECT EstadoPedido INTO estado_actual 
    FROM Pedido 
    WHERE idPedido = xidPedido;
    
    -- Actualizamos el estado
    UPDATE Pedido
    SET EstadoPedido = xNuevoEstado
    WHERE idPedido = xidPedido;
    
    -- Aclaracion: El trigger UpdateHistorialPedido se encargará de crear el registro en el historial
END $$
DELIMITER ;

-- Procedimiento para asignar un pedido a un vehículo
DELIMITER $$
CREATE PROCEDURE SPAsignarPedidoAVehiculo(
    IN xidPedido INT,
    IN xidVehiculo INT
)
BEGIN
    -- Verificamos si el vehículo está operativo
    DECLARE estado_vehiculo TINYINT;
    
    -- Verificamos si el pedido ya está asignado
    DECLARE pedido_asignado INT;
    
    SELECT Estado INTO estado_vehiculo 
    FROM Vehiculo 
    WHERE idVehiculo = xidVehiculo;
    
    SELECT COUNT(*) INTO pedido_asignado 
    FROM Vehiculo_has_Pedido 
    WHERE Pedido_idPedido = xidPedido;
    
    IF estado_vehiculo != 1 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El vehículo no está operativo';
    ELSEIF pedido_asignado > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El pedido ya está asignado a otro vehículo';
    ELSE
        -- Asignamos el pedido al vehículo
        INSERT INTO Vehiculo_has_Pedido (Vehiculo_idVehiculo, Pedido_idPedido, FechaAsignacion)
        VALUES (xidVehiculo, xidPedido, CURDATE());
        
        CALL UpdateEstadoPedido(xidPedido, 'En proceso');
    END IF;
END $$
DELIMITER ;