-- Active: 1748344015396@@127.0.0.1@3308

/*Store Procedures para EMPRESA*/
DELIMITER $$
Drop PROCEDURE IF EXISTS  PSCrearEmpresa $$
CREATE PROCEDURE PSCrearEmpresa(OUT xidEmpresa INT, xNombre VARCHAR(45))
BEGIN
    INSERT INTO Empresa (Nombre)
    VALUES (xNombre);
    set xidEmpresa = last_insert_id();
END $$

DELIMITER $$
Drop PROCEDURE IF EXISTS  SPDelEmpresa $$
CREATE PROCEDURE SPDelEmpresa (IN xidEmpresa INT)
BEGIN
    START TRANSACTION;
    
    DELETE hp FROM HistorialPedido hp
    INNER JOIN Pedido p ON hp.Pedido_idPedido = p.idPedido
    WHERE p.EmpresaDestino = xidEmpresa OR p.Administrador_idAdministrador IN (
        SELECT idAdministrador FROM Administrador WHERE Empresa_idEmpresa = xidEmpresa
    );
    
    DELETE FROM Pedido 
    WHERE EmpresaDestino = xidEmpresa OR Administrador_idAdministrador IN (
        SELECT idAdministrador FROM Administrador WHERE Empresa_idEmpresa = xidEmpresa
    );
    
    DELETE FROM Administrador 
    WHERE Empresa_idEmpresa = xidEmpresa;
    
    DELETE FROM Empresa 
    WHERE idEmpresa = xidEmpresa;
    
    COMMIT;
END $$

/*Stores procedures de ADMINISTRADORES*/
DELIMITER $$
Drop PROCEDURE IF EXISTS  SPNuevoAdministrador $$

CREATE PROCEDURE SPNuevoAdministrador(out xidAdministrador INT, xName VARCHAR(45), xPassword VARCHAR(45), xEmpresa_idEmpresa INT)
BEGIN
    INSERT INTO Administrador (Name, Passworld, Empresa_idEmpresa)
    VALUES (xName, xPassword, xEmpresa_idEmpresa);
    set xidAdministrador = last_insert_id();
END $$

DELIMITER $$
Drop PROCEDURE IF EXISTS  SPDelAdministrador $$

CREATE PROCEDURE SPDelAdministrador(xidAdministrador INT)
BEGIN
    -- Se verifica que el administrador no tenga paquetes asignados todavia
    DECLARE pedidos_count INT;
    
    SELECT COUNT(*) INTO pedidos_count 
    FROM Pedido 
    WHERE Administrador_idAdministrador = xidAdministrador;
    
    IF pedidos_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede eliminar el administrador porque tiene pedidos asociados';
    ELSE
        DELETE FROM Administrador 
        WHERE idAdministrador = xidAdministrador;
    END IF;
END $$
DELIMITER ;

/*En la logica del negocio no iria esto pero para la bd si*/
DELIMITER $$
Drop PROCEDURE IF EXISTS  SPActAdmi $$

CREATE PROCEDURE SPActAdmi(
    xidAdministrador INT,
    xName VARCHAR(45),
    xPassword VARCHAR(45)
)
BEGIN
    UPDATE Administrador
    SET Name = xName, Passworld = xPassword
    WHERE idAdministrador = xidAdministrador;
END $$
DELIMITER ;

-- ======================================
-- Stores procedures Conductor
-- ======================================
DELIMITER $$
Drop PROCEDURE IF EXISTS  SPNewConductor $$

CREATE PROCEDURE SPNewConductor(
    OUT xidConductor INT,
    xName VARCHAR(45),
    xLicencia VARCHAR(45),
    xDisponibilidad TINYINT
)
BEGIN
    INSERT INTO Conductor (Name, Licencia, Disponibilidad)
		VALUES (xName, xLicencia, xDisponibilidad);
    
    set xidConductor = last_insert_id();
END $$
DELIMITER ;

-- Procedimiento para eliminar un conductor
DELIMITER $$
Drop PROCEDURE IF EXISTS  SPDelConductor $$

CREATE PROCEDURE SPDelConductor(xidConductor INT)
BEGIN
    -- Se verifica si el conductor tiene asignaciones de vehículos
    DECLARE cantPedidos INT;
    
    SELECT COUNT(*) INTO cantPedidos 
    FROM Conductor_has_Vehiculo 
    WHERE Conductor_idConductor = xidConductor;
    
    IF cantPedidos > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede eliminar el conductor porque tiene vehículos asignados';
    ELSE
        DELETE FROM Conductor WHERE idConductor = xidConductor;
    END IF;
END $$
DELIMITER ;

-- Procedimiento para actualizar datos de un conductor
DELIMITER $$
Drop PROCEDURE IF EXISTS  UpdateConductor $$

CREATE PROCEDURE UpdateConductor(xidConductor INT,xLicencia VARCHAR(45))
BEGIN
    UPDATE Conductor
    SET Licencia = xLicencia
    WHERE idConductor = xidConductor;
END $$
DELIMITER ;

-- =====================================================================
-- PROCEDIMIENTOS PARA GESTIÓN DE VEHÍCULOS
-- =====================================================================

-- Procedimiento para crear un nuevo vehículo (por administrador)
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
    INSERT INTO Vehiculo (Tipo, Matricula, CapacidadMaz, Estado)
    VALUES (xTipo, xMatricula, xCapacidadMax, xEstado);
    
    set xidVehiculo = last_insert_id();
END $$
DELIMITER ;

-- Procedimiento para eliminar un vehículo
DELIMITER $$
Drop PROCEDURE IF EXISTS  SPDelVehiculo $$
CREATE PROCEDURE SPDelVehiculo(xidVehiculo INT)
BEGIN
    -- Aca Verificamos si el vehículo tiene pedidos asignados
    DECLARE asignaciones_pedidos INT;
    
    SELECT COUNT(*) INTO asignaciones_pedidos 
    FROM pedido 
    WHERE pedido.Vehiculo_idVehiculo = xidVehiculo;
    
    IF asignaciones_pedidos > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede eliminar el vehículo porque tiene pedidos asignados';
    ELSE
        DELETE FROM Vehiculo WHERE `Vehiculo`.`idVehiculo` = xidVehiculo;
    END IF;
END $$
DELIMITER ;

-- Procedimiento para actualizar datos de un vehículo
DELIMITER $$
Drop PROCEDURE IF EXISTS  UpdateVehiculo $$
CREATE PROCEDURE UpdateVehiculo(xidVehiculo INT,xMatricula VARCHAR(45)
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
	-- Ojo que esto vendria aser un trigger ya qeu al asignarle almenos un pedido/conductor ya se cambia la disponibilidad
    UPDATE Vehiculo
    SET Estado = CASE
        WHEN xdisponible THEN 0
        ELSE 1
    END
    WHERE idVehiculo = xidVehiculo;
END $$

-- =====================================================================
-- PROCEDIMIENTOS PARA ASIGNACIÓN DE VEHÍCULOS A CONDUCTORES
-- =====================================================================

-- Procedimiento para asignar un vehículo a un conductor
DELIMITER $$
Drop PROCEDURE IF EXISTS  AsignarVehiculoAConductor $$
CREATE PROCEDURE AsignarVehiculoAConductor(xidConductor INT,xidVehiculo INT)
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
DELIMITER $$

-- Procedimiento para desasignar un vehículo de un conductor
DELIMITER $$
Drop PROCEDURE IF EXISTS  SPDesasignarVehiculoAConductor $$

CREATE PROCEDURE SPDesasignarVehiculoAConductor(
    IN xidConductor INT,
    IN xidVehiculo INT
)
BEGIN
    DECLARE pedidosEnVehiculo INT;
    
    SELECT COUNT(*) INTO pedidosEnVehiculo 
    FROM vehiculo  
    JOIN Pedido using (idvehiculo)
    WHERE Vehiculo_idVehiculo = xidVehiculo;
    
    IF pedidosEnVehiculo > 0 THEN
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
Drop PROCEDURE IF EXISTS  SPCrearRuta $$

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
Drop PROCEDURE IF EXISTS  SPCrearPedido $$

CREATE PROCEDURE SPCrearPedido(
    OUT xidPedido INT,
    IN xName VARCHAR(45),
    IN xVolumen VARCHAR(45),
    IN xPeso VARCHAR(45),
    IN xEstadoPedido VARCHAR(45),
    IN xFechaDespacho DATE,
    IN xAdministrador_idAdministrador INT,
    IN xEmpresaDestino INT,
    IN xRuta_idRuta INT,
    IN xidVehiculo INT
)
BEGIN
    -- Creamos el pedido con estado inicial
    INSERT INTO Pedido (
        Name, Volumen, Peso, EstadoPedido, FechaDespacho,
        Administrador_idAdministrador, EmpresaDestino, Ruta_idRuta, Vehiculo_idVehiculo
    )
    VALUES (
        xName, xVolumen, xPeso, xEstadoPedido, xFechaDespacho,
        xAdministrador_idAdministrador, xEmpresaDestino, xRuta_idRuta, xidVehiculo
    );
    
    set xidPedido = last_insert_id();
    
    -- Aclaracion: El trigger InsertHistorialPedido se encargará de crear el registro en el historial
END $$
DELIMITER ;

-- Procedimiento para actualizar el estado de un pedido
DELIMITER $$
Drop PROCEDURE IF EXISTS  SPUpdateEstadoPedido $$

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