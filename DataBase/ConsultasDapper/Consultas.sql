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

SELECT Nombre, idEmpresa, Contrasena FROM Administrador where idAdministrador = 1;


SELECT * FROM historialpedido;

-- CREAR UNA CONSULTA QUE TRAIGA TODOS EL HISTORIAL DE PEDIDOS DE UNA EMPRESA QUE DESPACHE O TENGA COMO DESTINO FINA.
/*
    DE PEDIDO SE TENDRA QUE TRAER EL NOMBRE, VOLUMEN, PESO, ESTADO DEL PEDIDO Y FECHA DE DESPACHO. EL VEHICULO QUE LO TRASPORTO, EL NOMBRE DEL CONDUCTOR, EL NOMBRE DE LA EMPRESA DE ORIGEN Y EL NOMBRE DE LA EMPRESA DESTINO.
    DEL HISTORIAL SE DEBERA TRAER EL ESTADO ANTERIOR, EL NUEVO ESTADO Y LA FECHA DE CAMBIO. LA RUTA TANTO DE ORIGEN COMO DE DESTINO.
    DEBERA FILTRAR POR EL ID DE LA EMPRESA. 
*/

SELECT 
    p.idPedido,
    p.Name AS NombrePedido,
    p.Volumen,
    p.Peso,
    p.EstadoPedido,
    p.FechaDespacho,
    v.Tipo AS VehiculoTipo,
    v.Matricula AS VehiculoMatricula,
    c.Name AS NombreConductor,
    eo.Nombre AS EmpresaOrigen,
    p.idEmpresa AS idEmpresaOrigen,
    r.Origen AS RutaOrigen,
    r.Destino AS RutaDestino,
    ed.Nombre AS EmpresaDestino,
    h.EstadoAnterior,
    h.EstadoNuevo,
    h.FechaCambio
FROM Pedido p
INNER JOIN Empresa eo ON p.idEmpresa = eo.idEmpresa
INNER JOIN Ruta r ON p.idRuta = r.idRuta
INNER JOIN Vehiculo v ON p.idVehiculo = v.idVehiculo
INNER JOIN Conductor_has_Vehiculo cv ON v.idVehiculo = cv.idVehiculo
INNER JOIN Conductor c ON cv.idConductor = c.idConductor
INNER JOIN HistorialPedido h ON p.idPedido = h.idPedido
LEFT JOIN Empresa ed ON r.Destino = ed.Nombre
WHERE eo.idEmpresa = 2
   OR ed.idEmpresa = 2;

SELECT * FROM conductor WHERE Disponibilidad = 1;
SELECT * FROM vehiculo WHERE vehiculo.Estado = 1;