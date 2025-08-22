# Aurora 🚚✨

<p align="center">
  <img src="https://et12.edu.ar/imgs/computacion/vamoaprogramabanner.png" alt="Banner Computación">
</p>

**Aurora** es un sistema de logística desarrollado como proyecto académico para la materia **Programación sobre Redes (6° Año, 8° División)**.  
Su propósito es gestionar y realizar un **seguimiento dinámico y simple de los paquetes despachados**, integrando distintos roles y entidades dentro del proceso de envío y recepción.

El proyecto está desarrollado en **.NET 8.0**, utilizando **Dapper** como micro ORM y una base de datos **MySQL** para el almacenamiento de datos.

---

## 📑 Tabla de Contenidos
1. [Características principales](#-características-principales)
2. [Entidades del sistema](#-entidades-del-sistema)
3. [Ramas del repositorio](#-ramas-del-repositorio)
4. [Arquitectura del proyecto](#-arquitectura-del-proyecto)
5. [Despliegue](#-despliegue)
6. [Guía de ejecución](#-guía-de-ejecución)
7. [Pre-requisitos](#-pre-requisitos)
8. [Autores](#-autores)
9. [Licencia](#-licencia)

---

## 🚀 Características principales
- Gestión de empresas, administradores, conductores y vehículos.
- Control de pedidos con distintos estados predefinidos.
- Registro histórico de cada pedido.
- Validaciones mediante **triggers** y métodos de verificación en vehículos.
- Programación sincrónica y asincrónica según la rama de trabajo.
- Soporte para API mínima con pruebas HTTP/TCP.

---

## 🗂 Entidades del sistema
Cada entidad cumple un rol específico dentro del flujo de trabajo:

- **Empresa**  
  - Añadir, editar y eliminar administradores.  
  - Visualizar pedidos enviados y recibidos.  
  - Añadir o dar de baja conductores.  
  - Añadir o eliminar vehículos.  

- **Administradores**  
  - Despachar pedidos.  
  - Recibir y confirmar pedidos.  
  - Asignar vehículos disponibles a pedidos.  
  - Asignar conductores a vehículos.  

- **Conductores**  
  - Consultar su información personal.  
  - Cambiar el estado de un pedido.  

- **Vehículo**  
  - Implementa triggers y validaciones de peso y disponibilidad.  

- **Pedidos**  
  - Estados: `Creado → Despachado → Entregado → Recibido`.  

- **Ruta**  
  - Cada pedido incluye origen y destino definidos.  

- **Historial de pedido**  
  - Registro completo del seguimiento de cada envío.  

---

## 🌿 Ramas del repositorio
El repositorio está estructurado en varias ramas (branches), cada una representando un enfoque o práctica distinta:

- **Main** → Proyecto sincrónico, incluye base de datos, SRC y pruebas con xUnit.  
- **Secuencial-mode** → Variante secuencial del proyecto.  
- **UX-UI** → Incluye únicamente el frontend.  
- **async-mode** → Proyecto asincrónico con programación distribuida y `Tasks`. Incluye DB forkeada + SRC.  
- **MinimalApi** → Implementación con **HTTP/TCP** y API mínima, asincrónica, con DB forkeada.  

---

## 🏗 Arquitectura del proyecto
La estructura del repositorio se organiza en carpetas principales:

- **DataBase** → Scripts y estructura de la base de datos.  
- **Document** → Documentación del proyecto (diagramas, manuales, guías, datos relevantes).  
- **MinimalAPI** → Acciones HTTP y DTOs asociados.  
- **SRC**  
  - `Aurora.Core` → Modelos e interfaces.  
  - `Aurora.ADO.Dapper` → Lógica de datos implementada en repositorios por entidad.  
  - `Aurora.Test` → Pruebas unitarias organizadas por entidad.  

---

## 🛠 Despliegue
Para clonar el repositorio:  

```bash
git clone https://github.com/SirFrancis2007/Aurora.git
```

## Guia de Ejecucion

1. Inicializar la base de datos

- Dentro de la carpeta DataBase, abrir consola MySQL y ejecutar:
```bash
mysql -u <usuario> -p
# ingresar contraseña
SOURCE INSTALL.SQL;
```

2. Restaurar y compilar

- Desde la carpeta SRC, en la terminal:

```bash
dotnet restore
dotnet build
```

3. Ejecutar Pruebas

```bash
dotnet test
```

4. Levantar la API mínima

Ir a la carpeta MinimalAPI y ejecutar:

```bash
dotnet run
```

En el navegador, Modificar el dominio localhost:

```bash
http://localhost:<puerto>/scalar
```

Esto abrirá el emulador de acciones HTTP, organizado por entidad.

# Pre-Requisitos

- Visual Studio Code
- [.NET 8.0](https://dotnet.microsoft.com/es-es/download/dotnet/8.0).
- [.MySQL] (https://www.mysql.com/) o extension de Visual Studio Code.

# Autor

 - Francisco Agustin Garcia (SirFrancis2007)

**Hecho con cariño para los open source**
**Agradezco quienes hicieron posible este sueño, mi formacion**

*Atte: Francisco Agustin Garcia, Abanderado Nacional*