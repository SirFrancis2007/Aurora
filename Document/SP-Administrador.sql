DELIMITER $$
DROP PROCEDURE IF EXISTS SPNuevoAdministrador $$
CREATE PROCEDURE SPNuevoAdministrador(out xidAdministrador INT, xName VARCHAR(45), xPassword VARCHAR(45), xidEmpresa INT)
BEGIN
    INSERT INTO Administrador (Nombre, contrasena, idEmpresa)
    VALUES (xName, xPassword, xidEmpresa);
    set xidAdministrador = last_insert_id();
END $$

-- STORE PROCEDURE PARA ELIMINAR PARCIALMENTE UN ADMINISTRADOR.

DELIMITER $$
Drop PROCEDURE IF EXISTS SPDelAdministrador $$
CREATE PROCEDURE SPDelAdministrador(xidAdministrador INT)
BEGIN
    -- Se verifica que el administrador no tenga paquetes asignados todavia
    DECLARE pedidos_count INT;
    
    SELECT COUNT(*) INTO pedidos_count 
    FROM Pedido 
    join administrador using (idAdministrador)
    WHERE idAdministrador = xidAdministrador;
    
    IF pedidos_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede eliminar el administrador porque tiene pedidos asociados';
    ELSE
        DELETE FROM Administrador 
        WHERE idAdministrador = xidAdministrador;
    END IF;
END $$

-- STORE PROCEDURE PARA ACTUALIZAR TODAS LAS TUPLAS DE LA ENTIDAD ADMINISTRADOR

DELIMITER $$
Drop PROCEDURE IF EXISTS SPUpdateAdmi $$
CREATE PROCEDURE SPUpdateAdmi(
    xidAdministrador INT,
    xName VARCHAR(45),
    xPassword VARCHAR(45)
)
BEGIN
    UPDATE Administrador
    SET Nombre = xName, Contrasena = xPassword
    WHERE idAdministrador = xidAdministrador;
END $$