use aurorabd;

DELIMITER $$
Drop PROCEDURE IF EXISTS PSCrearEmpresa $$
CREATE PROCEDURE PSCrearEmpresa(OUT xidEmpresa INT, xNombre VARCHAR(45))
BEGIN
    INSERT INTO Empresa (Nombre)
    VALUES (xNombre);
    set xidEmpresa = last_insert_id();
END $$

-- STORE PROCEDURE PARA ELIMINAR PARCIALMENTE UNA EMPRESA.
-- PRIMERO SE ELIMINA EL HISTORIAL DE PEDIDOS LIGADOS A LA EMPRESA.
-- SEGUNDO SE ELIMINA TODO PEDIDO QUE ENVIA LA EMPRESA.
-- TERCERO SE ELIMINA TODO ADMINISTRADOR LIGADO A LA EMPRESA.
-- CUARTO  SE ELIMINA LA EMPRESA.

DELIMITER $$
DROP PROCEDURE IF EXISTS SPDelEmpresa $$
CREATE PROCEDURE SPDelEmpresa (IN xidEmpresa INT)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    SET SQL_SAFE_UPDATES = 0; -- Desactiva modo seguro
    
    START TRANSACTION;
    
    DELETE FROM HistorialPedido
    WHERE idPedido IN (
        SELECT idPedido
        FROM Pedido p
        WHERE p.idEmpresa = xidEmpresa
           OR p.idAdministrador IN (
                SELECT idAdministrador
                FROM Administrador a
                WHERE a.idEmpresa = xidEmpresa
           )
    );
    
    DELETE FROM Pedido 
    WHERE idEmpresa = xidEmpresa
       OR idAdministrador IN (
            SELECT idAdministrador
            FROM Administrador
            WHERE idEmpresa = xidEmpresa
        );
    
    DELETE FROM Administrador 
    WHERE idEmpresa = xidEmpresa;
    
    DELETE FROM Empresa 
    WHERE idEmpresa = xidEmpresa;
    
    SET SQL_SAFE_UPDATES = 1;
    
    COMMIT;
END $$