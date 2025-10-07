-- Función para calcular el total de pedidos por estado
DELIMITER $$
DROP FUNCTION if EXISTS TotalPedidosPorEstado $$
CREATE FUNCTION TotalPedidosPorEstado(xEstado VARCHAR(45)) 
RETURNS INT READS SQL Data
BEGIN
    DECLARE total INT;
    
    SELECT COUNT(*) INTO total
    FROM Pedido
    WHERE EstadoPedido = xEstado;
    
    RETURN total;
END $$
DELIMITER ;

-- funcion para verificar si la licencia es valida

DELIMITER $$
DROP FUNCTION if EXISTS VerificarLicenciaValidaParaVehiculo $$
CREATE FUNCTION VerificarLicenciaValidaParaVehiculo(p_idConductor INT, p_idVehiculo INT) 
RETURNS BOOLEAN READS SQL Data
BEGIN
    DECLARE licencia_conductor VARCHAR(45);
    DECLARE tipo_vehiculo VARCHAR(45);
    DECLARE es_valida BOOLEAN;
    
    SELECT Licencia INTO licencia_conductor
    FROM Conductor
    WHERE idConductor = p_idConductor;
    
    SELECT Tipo INTO tipo_vehiculo
    FROM Vehiculo
    WHERE idVehiculo = p_idVehiculo;
    
	IF licencia_conductor is not null then
		set es_valida = true;
	else
		set es_valida = false;
    END IF;
    
    RETURN es_valida;
END $$

DELIMITER $$

DROP FUNCTION IF EXISTS FLoginEmpresa $$
CREATE FUNCTION FLoginEmpresa(xNombre VARCHAR(100), xContrasena VARCHAR(100))
RETURNS TINYINT(1)
READS SQL DATA
BEGIN
    DECLARE existe TINYINT(1) DEFAULT 0;

    SELECT COUNT(*) > 0
    INTO existe
    FROM Empresa e
    WHERE e.Nombre = xNombre
      AND e.Contrasena = xContrasena;

    RETURN existe;
END $$


DELIMITER $$
DROP FUNCTION IF EXISTS FLoginAdministrador $$

CREATE FUNCTION FLoginAdministrador(xNombre VARCHAR(100), xPassword VARCHAR(100))
RETURNS BOOLEAN
READS SQL DATA
BEGIN
    DECLARE existe BOOLEAN DEFAULT FALSE;

    IF EXISTS (SELECT 1 FROM Administrador  WHERE Nombre = xNombre AND Contrasena = xPassword) THEN
        SET existe = TRUE;
    ELSE
        SET existe = FALSE;
    END IF;

    RETURN existe;
END $$

DELIMITER ;

DELIMITER $$
DROP FUNCTION IF EXISTS FLoginConductor $$
CREATE FUNCTION FLoginConductor(xNombre VARCHAR(45), xLicencia VARCHAR(45))
RETURNS BOOLEAN
READS SQL DATA
BEGIN
    DECLARE existe BOOLEAN DEFAULT FALSE;

    IF EXISTS (SELECT 1 FROM conductor WHERE Name = xNombre and Licencia = xLicencia) THEN
        SET existe = TRUE;
    ELSE 
        SET existe = FALSE;
    END IF;

    RETURN existe;
END $$

DELIMITER $$
DROP FUNCTION IF EXISTS FncActualizarEstadoConductor $$
CREATE FUNCTION FncActualizarEstadoConductor(xidconductor TINYINT)
RETURNS BOOLEAN
READS SQL DATA
BEGIN
    UPDATE conductor c
    SET Disponibilidad = 0 -- Significa que esta en viaje.
    WHERE c.idConductor = xidconductor;
END $$

DELIMITER $$
DROP FUNCTION IF EXISTS FncActualizarEstadoVehiculo $$
CREATE FUNCTION FncActualizarEstadoVehiculo(xidVehiculo TINYINT)
RETURNS BOOLEAN
READS SQL DATA
BEGIN
    UPDATE vehiculo
    SET Estado = 0 -- Significa que esta en viaje.
    WHERE idVehiculo = xidVehiculo;
END $$