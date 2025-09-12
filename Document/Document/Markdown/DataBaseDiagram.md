### Diagrama de Base de Datos

```mermaid
erDiagram
    Empresa ||--o{ Administrador : "tiene"
    Empresa ||--o{ Pedido : "destino"
    Administrador ||--o{ Pedido : "crea"
    Ruta ||--o{ Pedido : "asignada"
    Vehiculo ||--o{ Pedido : "asignado"
    Pedido ||--o{ HistorialPedido : "registra"
    Conductor }|--|| Conductor_has_Vehiculo : "asignado"
    Vehiculo }|--|| Conductor_has_Vehiculo : "asignado"

    Empresa {
        int idEmpresa PK
        varchar(45) Nombre
    }
    Administrador {
        int idAdministrador PK
        varchar(45) Nombre
        varchar(45) Contrasena
        int idEmpresa FK
    }
    Ruta {
        int idRuta PK
        varchar(45) Origen
        varchar(45) Destino
    }
    Vehiculo {
        int idVehiculo PK
        varchar(45) Tipo
        varchar(45) Matricula
        double CapacidadMax
        tinyint Estado
    }
    Pedido {
        int idPedido PK
        varchar(45) Name
        varchar(45) Volumen
        varchar(45) Peso
        varchar(45) EstadoPedido
        date FechaDespacho
        int idAdministrador FK
        int idEmpresa FK
        int idRuta FK
        int idVehiculo FK
    }
    Conductor {
        int idConductor PK
        varchar(45) Name
        varchar(45) Licencia
        tinyint Disponibilidad
    }
    Conductor_has_Vehiculo {
        int idConductor PK,FK
        int idVehiculo PK,FK
        date FechaAsignado
    }
    HistorialPedido {
        int idHistorialPedido PK
        varchar(45) EstadoAnterior
        varchar(45) EstadoNuevo
        datetime FechaCambio
        int idPedido FK
    }
```