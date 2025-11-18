use aurorabd;

DELIMITER $$
Drop PROCEDURE IF EXISTS PSCrearEmpresa $$
CREATE PROCEDURE PSCrearEmpresa(OUT xidEmpresa INT, xNombre VARCHAR(45), xContrasena VARCHAR(45))
BEGIN
    INSERT INTO Empresa (Nombre, Contrasena)
    VALUES (xNombre, xContrasena);
    set xidEmpresa = last_insert_id();
END $$