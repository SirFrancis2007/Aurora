SELECT 
    em.Nombre AS Nombre_Empresa_Origen, 
    admi.Nombre AS Nombre_Administrador, 
    pe.Name AS Nombre_Pedido,
    pe.Volumen, 
    pe.Peso, 
    pe.EstadoPedido, 
    pe.FechaDespacho, 
    emd.Nombre AS Nombre_Empresa_Destino
FROM Empresa em
JOIN Administrador admi USING (idEmpresa)
JOIN Pedido pe ON pe.idAdministrador = admi.idAdministrador
JOIN Empresa emd ON emd.idEmpresa = pe.idEmpresa
WHERE em.idEmpresa = 1 OR pe.idEmpresa = 1;
