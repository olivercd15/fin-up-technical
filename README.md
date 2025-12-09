# Prueba tecnica para FinUp - Desarrollador Backend

## Ejecutar proyecto
**Requisitos previos**:  
- .NET 8
- Visual Studio
- Docker / Docker Desktop 
- Git
- Cliente de SQL Server

**Pasos para la ejecucion**:  

- Clonar el repositorio
```bash
git clone https://github.com/olivercd15/fin-up-technical.git
```

- Cambiar en app.settings del proyecto Payments.WebApi, la conexion a la base de datos de ser necesario a local, descomentando la siguiente linea
```bash
//"DefaultConnection": "Server=localhost,1432;Database=PaymentsDb;User Id=sa;Password=Sqlserver123$;TrustServerCertificate=True;"
```

- Una vez se apunta a local, se debe ejecutar primero la generacion de las bases de datos (SQL Server, Redis y Kafka), ejecutar docker compose para correr las bases
```bash
docker-compose up -d --build
```

- Luego se deben realizar las migraciones a la base de datos, por lo que debemos dirigirnos a:
```bash
cd Payments.Infrastructure
```

- Ejecutar para migrar la base de datos:
```bash
dotnet ef database update
```

- Todo estaria listo, solo se debe hacer correr con el IDE de Visual Studio colocando como proyecto de Inicio a Payments.WebApi o ejecutar:

```bash
cd Payments.WebApi
dotnet run
```

- Se generara una direccion local con un puerto determinado que dirige al Swagger del proyecto deberia estar habilitado

