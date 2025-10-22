DELIMITER $$
DROP PROCEDURE IF EXISTS SPCrearPedido $$
CREATE PROCEDURE SPCrearPedido(
    OUT xidPedido INT,
    xName VARCHAR(45),
    xVolumen VARCHAR(45),
    xPeso VARCHAR(45),
    xEstadoPedido VARCHAR(45),
    xFechaDespacho DATE,
    xAdministrador_idAdministrador INT,
    xEmpresaDestino INT,
    xRuta_idRuta INT,
    xidVehiculo INT
)
BEGIN
    INSERT INTO Pedido (
        Name, Volumen, Peso, EstadoPedido, FechaDespacho,
        idAdministrador, idEmpresa,
        idRuta, idVehiculo
    )
    VALUES (
        xName, xVolumen, xPeso, xEstadoPedido, xFechaDespacho,
        xAdministrador_idAdministrador, xEmpresaDestino,
        xRuta_idRuta, xidVehiculo
    );
    
    SET xidPedido = LAST_INSERT_ID();
    
    UPDATE Vehiculo
    SET Estado = 1
    WHERE idVehiculo = xidVehiculo;
    -- Aclaracion: El trigger InsertHistorialPedido se encargará de crear el registro en el historial
END $$
DELIMITER ;

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

DROP PROCEDURE IF EXISTS SPActualizarEstadoPedidosPorVehiculo;
DELIMITER $$
CREATE PROCEDURE SPActualizarEstadoPedidosPorVehiculo(
    IN xIdVehiculo INT,
    IN xNuevoEstado VARCHAR(45)
)
BEGIN
    UPDATE Pedido
    SET EstadoPedido = xNuevoEstado
    WHERE idVehiculo = xIdVehiculo;
END $$
DELIMITER ;

DROP PROCEDURE IF EXISTS SPActualizarEstadoPedidoIndividual;
DELIMITER $$
CREATE PROCEDURE SPActualizarEstadoPedidoIndividual(
    IN xIdPedido INT,
    IN xNuevoEstado VARCHAR(45)
)
BEGIN
    UPDATE Pedido
    SET EstadoPedido = xNuevoEstado
    WHERE idPedido = xIdPedido;
END $$
DELIMITER ;

