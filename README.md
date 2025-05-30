<h1 align="center">E.T. Nº12 D.E. 1º "Libertador Gral. José de San Martín"</h1>
<p align="center">
  <img src="https://et12.edu.ar/imgs/computacion/vamoaprogramabanner.png" alt="Banner Computación">
</p>

## Computación 2025

**Asignatura**: Programacion sobre Redes

**Curso**: 6° 8°

# Aurora

Aurora es un sistema de logistica que logra mediante una experiencia simple y rapido despachar mercancia y hace run seguimiento dinamico del mismo hasta llegar a destino. Desarrollado en la Plataforma .NET 8.0, MYSQL y Dapper como micro ORM para la administracion y gestion con la BD. Aurora logra un codigo limpio y estrucurado preparado para la evolucion futura.  

## Comenzando 🚀

Clonar el repositorio github, desde Github Desktop o ejecutar en la terminal o CMD:
```
git clone https://github.com/SirFrancis2007/Aurora
```

### Pre-requisitos 📋

- [Visual Studio Code](https://code.visualstudio.com/download)
- [.NET 8.0](https://dotnet.microsoft.com/es-es/download/dotnet/8.0).
- [MySQL WorkBench](https://dev.mysql.com/downloads/workbench/)


## Despliegue 📦

- Para poder correr estos scripts ejecuta el siguiente comando dentro de la terminal integrada de la carpeta `install.sql`

```shell
mysql -u tuUsuario -p
```

- Una vez loggeado ejecute el siguiente comando para crear la BD.

```shell
source Install.sql
```

- Inciado previmente la Base de datos, en la carpeta 'Aurora Test' se podran hacer testeo integrales del software. 

```shell
dotnet test -v d
```

## Construido con 🛠️

- C# 12.0
- MySQL 8.0
- Visual Studio Code.

## Versionado 📌

Usamos [SemVer](http://semver.org/) para el versionado. Para todas las versiones disponibles, mira los [tags en este repositorio](https://github.com/ET12DE1Computacion/simpleTemplateCSharp/tags).

## Autores ✒️

- **Francisco García** - [SirFrancis2007](https://github.com/SirFrancis2007) 

## Licencia 📄

Este proyecto está bajo la Licencia Creative Commons Attribution 4.0 International - mira el archivo [LICENSE](LICENSE) para detalles.