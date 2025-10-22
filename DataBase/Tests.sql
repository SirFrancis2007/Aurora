use aurorabd;

Call PSCrearEmpresa (@Idempresa1,'Empresa1', '1234');
Call PSCrearEmpresa (@Idempresa2,'Empresa2', '1234');

Call SPNuevoAdministrador(@id1,'admin1', '1234', @Idempresa1);
Call SPNuevoAdministrador(@id2,'admin2', '1234', @Idempresa2);

Call SPNuevoConductor(@id1c,'conductor1', '1234', TRUE); -- disponible
Call SPNuevoConductor(@id2c,'conductor2', '1234', TRUE); -- disponible

call SPCrearVehiculo(@Veh1,'vehiculo1', 'H4G23Z', 1000, TRUE); -- disponible
call SPCrearVehiculo(@Veh2,'vehiculo2', 'ID3SBX', 1200, TRUE); -- disponible

CALL SPCrearRuta(@r1,'Cordoba', 'BSAS') ;
CALL SPCrearRuta(@r2,'BSAS', 'Cordoba') ;
CALL SPCrearRuta(@r3,'Posadas', 'Resistencia') ;
CALL SPCrearRuta(@r4,'Montevideo', 'BSAS') ;
CALL SPCrearRuta(@r5,'Santa Rosa', 'Chivilcoy') ;
CALL SPCrearRuta(@r6,'Chacomus', 'La Plata') ;
CALL SPCrearRuta(@r7,'Viedma', 'Bariloche') ;

CALL AsignarVehiculoAConductor(@id1c, @Veh1) ;
CALL AsignarVehiculoAConductor(@id2c, @Veh2) ;

-- ID + NAME + VOL + PESO + ESTADO + FECHA + IDADMIN + IDDESTINO + IDRUTA + IDVEHICULO

CALL SPCrearPedido(@p1,'Troncos', 1500, 600, 'Despachado', curdate(), 2, 1,1,1) ;
CALL SPCrearPedido(@p2,'aceite 1lt', 200, 100, 'Despachado', curdate(), 2, 1,6,1) ;
CALL SPCrearPedido(@p3,'aceitunas 1lt', 200, 100, 'Despachado', curdate(), 2, 1,6,1) ;