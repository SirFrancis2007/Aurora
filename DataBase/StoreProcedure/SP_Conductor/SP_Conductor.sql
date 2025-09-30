DELIMITER $$
Drop PROCEDURE IF EXISTS  SPNuevoConductor $$
CREATE PROCEDURE SPNuevoConductor(
    OUT xidConductor INT,
    xName VARCHAR(45),
    xLicencia VARCHAR(45),
    xDisponibilidad TINYINT
)
BEGIN
    INSERT INTO Conductor (Name, Licencia, Disponibilidad)
		VALUES (xName, xLicencia, 1); -- Predeterminadamente se da 1 que significa DISPONIBLE
    
    SET xidConductor = last_insert_id();
END $$


DELIMITER $$
Drop PROCEDURE IF EXISTS  SPDelConductor $$
CREATE PROCEDURE SPDelConductor(xidConductor INT)
BEGIN
    -- Se verifica si el conductor tiene asignaciones de vehículos
    DECLARE cantPedidos INT;
    
    SELECT COUNT(*) INTO cantPedidos 
    FROM Conductor_has_Vehiculo 
    WHERE idConductor = xidConductor;
    
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
Drop PROCEDURE IF EXISTS  SPUpdateConductor $$

CREATE PROCEDURE SPUpdateConductor(xidConductor INT, xnombre VARCHAR(50),xLicencia VARCHAR(45), xDisponibilidad BOOL)
BEGIN
    UPDATE Conductor
    SET Licencia = xLicencia, Name = xnombre, Disponibilidad = xDisponibilidad
    WHERE idConductor = xidConductor;
END $$
DELIMITER ;