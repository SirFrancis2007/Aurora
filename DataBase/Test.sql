use aurorabd;

-- Datos de prueba
Delimiter $$
Call PSCrearEmpresa ('Empresa Norte src') $$
Call PSCrearEmpresa ('Empresa Sur SA') $$
Call PSCrearEmpresa ('Sancor Seguro') $$

/*se borrara ya que no tiene nada asignado*/
Call SPDelEmpresa(3) $$

Call SPNuevoAdministrador('Victor', 'victor1234', 1) $$
Call SPNuevoAdministrador('Victoria', 'vic1234', 1) $$
Call SPNuevoAdministrador('pepe', 'pepe1234', 2) $$
Call SPNuevoAdministrador('messi', 'messi1234', 2) $$

Call SPDelAdministrador(4) $$

/*Conductores*/

Call SPNewConductor('Samuel', '123456789', TRUE) $$
Call SPNewConductor('juan', 'juan1234', TRUE) $$
Call SPNewConductor('julian', 'julian1234', TRUE) $$
Call SPNewConductor('pepe', 'pepe1234', TRUE) $$

call SPDelConductor(4) $$

call UpdateConductor(1, 'samuel1234') $$

/*Nuevos Vehiculos*/

call SPCrearVehiculo ('camioncito', 'm34c123', 1000, TRUE) $$ -- 1000 serian 1000 kg = 1 Tonelada
call SPCrearVehiculo('Furgoneta', 'MAT002', 500, TRUE) $$
call SPCrearVehiculo('Camión Grande', 'MAT003', 2000, TRUE) $$
call SPCrearVehiculo('Furgoneta Pequeña', 'MAT004', 300, TRUE) $$

call SPDelVehiculo(2) $$

call UpdateVehiculo(1, 'ABC021HC') $$

/*Rutas*/

CALL SPCrearRuta('Cordoba', 'BSAS') $$
CALL SPCrearRuta('BSAS', 'Cordoba') $$
CALL SPCrearRuta('Posadas', 'Resistencia') $$
CALL SPCrearRuta('Montevideo', 'BSAS') $$
CALL SPCrearRuta('Santa Rosa', 'Chivilcoy') $$
CALL SPCrearRuta('Chacomus', 'La Plata') $$
CALL SPCrearRuta('Viedma', 'Bariloche') $$

/*Asignar condcutor a un vehiculo*/
CALL AsignarVehiculoAConductor(1, 1);
CALL AsignarVehiculoAConductor(2, 2);
CALL AsignarVehiculoAConductor(3, 3);
CALL AsignarVehiculoAConductor(4, 4);

/*Desagsignar vehiculo a conductor*/

call SPDesasignarVehiculoAConductor (4,4) $$
call SPDesasignarVehiculoAConductor (1,1) $$

/*Registrar pedido*/

CALL SPCrearPedido('Caucho', 20, 50, 'Creado', curdate(), 1, 2, 4);
CALL SPCrearPedido('Troncos', 1500, 600, 'Creado', curdate(), 1, 2,2);
CALL SPCrearPedido('aceite 1lt', 200, 100, 'Creado', curdate(), 1, 2,6);
CALL SPCrearPedido('aceitunas 1lt', 200, 100, 'Creado', curdate(), 1, 2,6);
CALL SPCrearPedido('leche La Serenicima', 200, 100, 'Creado', curdate(), 1, 2,6);
CALL SPCrearPedido('Cafe molido', 123, 80, 'Creado', curdate(), 1, 2,6);

/*Asignar pedido a vehiculo*/

call SPAsignarPedidoAVehiculo (1,1) $$
call SPAsignarPedidoAVehiculo (2,2) $$
call SPAsignarPedidoAVehiculo (1,4) $$
call SPAsignarPedidoAVehiculo (1,4) $$
call SPAsignarPedidoAVehiculo (1,4) $$

/*Simular que el paquete ya fue recibido y confirmado su entrega por parte del receptor*/

call SPUpdateEstadoPedido(1, 'Recibido') $$


/*INSERT INTO Administrador (IdEmpresa, Nombre, Contrasena) VALUES 
(1, 'Admin1', SHA2('password1', 256)),
(2, 'Admin2', SHA2('password2', 256)),
(3, 'Admin3', SHA2('password3', 256)),
(4, 'Admin4', SHA2('password4', 256));

INSERT INTO Conductor (Nombre, Licencia, Disponibilidad) VALUES 
('Conductor 1', 'LIC001', TRUE),
('Conductor 2', 'LIC002', TRUE),
('Conductor 3', 'LIC003', TRUE),
('Conductor 4', 'LIC004', TRUE);

INSERT INTO Vehiculo (Tipo, Matricula, CapacidadMax, Disponible) VALUES 
('Camión', 'MAT001', 1000, TRUE),
('Furgoneta', 'MAT002', 500, TRUE),
('Camión Grande', 'MAT003', 2000, TRUE),
('Furgoneta Pequeña', 'MAT004', 300, TRUE);

INSERT INTO Ruta (Origen, Destino) VALUES 
('Madrid', 'Barcelona'),
('Barcelona', 'Valencia'),
('Valencia', 'Sevilla'),
('Sevilla', 'Madrid');


CALL RegistrarPedido(1, 2, 1, 1, 'Paquete de electrónica', 200, 150, @id_pedido);
SELECT @id_pedido;

-- Asignar vehículo a un pedido
CALL AsignarVehiculoAPedido(@id_pedido, 1);

-- Asignar conductor a vehículo
CALL AsignarConductorAVehiculo(1, 1);

-- Marcar pedido como entregado
CALL MarcarPedidoComoEntregado(@id_pedido, 1);

-- Generar informe de pedidos por empresa
CALL InformePedidosPorEmpresa(1);

-- Listar vehículos disponibles con capacidad suficiente
CALL ListarVehiculosDisponibles(200);*/