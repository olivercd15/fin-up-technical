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



# Detalles Tecnicos de la Prueba FinUp

## 🚀 Payments API — .NET 8 (Clean Architecture, CQRS, DDD, EF Core, Dapper, Kafka, Redis)



Este proyecto implementa un microservicio de **procesamiento de pagos** utilizando una arquitectura moderna basada en:

- **Clean Architecture**
- **CQRS (Commands y Queries)**
- **DDD (Domain-Driven Design)**
- **Entity Framework Core (Escrituras)**
- **Dapper (Lecturas)**
- **Kafka (eventos asincrónicos)**
- **Redis (caching en queries)**
- **Docker & Docker Compose**
- **xUnit (Unit Tests)**

---

## 🧱 **Arquitectura del Proyecto**

Este proyecto sigue estrictamente **Clean Architecture**, separando:

```bash
src/
├── Payments.Domain → Entidades, enums, lógica del dominio
├── Payments.Application → Casos de uso (CQRS), validaciones, DTOs
├── Payments.Infrastructure → EF Core, Dapper, Kafka, Redis, Repositorios
└── Payments.WebApi → Endpoints REST (Controllers)
```

### **Patrones aplicados:**

- **CQRS:**  
  - *Commands* → Escrituras con EF Core  
  - *Queries* → Lecturas con Dapper

- **DDD:**  
  - Entidad raíz: `Payment`  
  - Estados: Pending, Completed, Rejected

- **SOLID Principles**  
- **DTOs**, **Validators**, **Handlers**

---

## 🧪 **Ejecutar Tests**

Desde la raíz del repo:

```bash
dotnet test
```

Incluyen:

- Validaciones del command  
- Reglas de negocio  
- Comportamiento del handler  
- Query + repositorio mockeado  

---

## 🛢 **Base de Datos**

### Connection String usada:

```bash
Server=localhost,1432;
Database=PaymentsDb;
User Id=sa;
Password=Sqlserver123$;
TrustServerCertificate=true;
```

### Crear nuevas migraciones:

```bash
cd Payments.Infrastructure
dotnet ef migrations add NombreMigration --startup-project ../Payments.WebApi --output-dir Persistence/Migrations
```

### Actualizar BD:

```bash
dotnet ef database update --startup-project ../Payments.WebApi
```

---

## 📨 **Kafka — Eventos de Pago**

Cada vez que se crea un pago, se publica un evento en Kafka:

**Topic:** `payments.created`

Ejemplo del evento:

```json
{
  "PaymentId": "guid",
  "CustomerId": "guid",
  "Amount": 120.50,
  "ServiceProvider": "Servicios Eléctricos",
  "Currency": "BOB",
  "CreatedAt": "2025-12-09T13:00:00Z"
}
```


Ver topics:

```bash
docker exec -it payments_kafka bash
kafka-topics --bootstrap-server payments_kafka:9092 --list
```


## ⚡ **Redis — Caching de Queries**

El endpoint GET usa Redis como cache opcional:


```json
await _cache.SetAsync(key, data, TimeSpan.FromMinutes(5));
```


## 📡 **Endpoints de la API**

POST /api/v1/payments

Crear un pago.


```json
{
  "customerId": "uuid",
  "serviceProvider": "SERVICIOS ELÉCTRICOS",
  "amount": 120.50,
  "currency": "BOB"
}
```


GET /api/v1/payments?customerId=uuid

Devuelve lista de pagos del cliente.




## 🧰 **Dockerfile de la API (compatible con Railway, Render y Docker Hub)**

Se tiene dockerfile especializado para la publicacion a cualquier servidor como Imagen



## 🔧 **Docker Compose (Infraestructura + API opcional)**

Se tiene dockerfile especializado para la publicacion a cualquier servidor como Imagen


```json
zookeeper
kafka
sqlserver
redis
```

## 🔁 **CI/CD — GitHub Actions + Railway**

Este proyecto implementa un pipeline completo:

### 🧪 CI — GitHub Actions

Flujo:

- **Cada push o pull request a** `develop` y `main` 

- Restaura dependencias

- Compila solución

- Ejecuta pruebas unitarias con xUnit

- Genera resultados de test como artefactos

- Archivo `.github/workflows/ci.yml`

✔️ Ramas CI:

- `develop` → pruebas automáticas

- `main` → pruebas + despliegue continuo

### 🚀 CD — Railway

Flujo:

- Railway se encarga del despliegue automático cuando se hace push a main:

- Construye la imagen Docker usando tu Dockerfile

- Expone el servicio con el puerto `$PORT` requerido por Railway

Flujo completo:

```json
develop → CI (pruebas)
main → CI + CD (deploy en Railway)
```


## 🎯  **Objetivo del Proyecto**

Este microservicio demuestra una arquitectura moderna y escalable:

- CLEAN Architecture

- Procesamiento robusto de pagos

- Lecturas optimizadas con Dapper

- Escritor persistente con EF Core

- Eventos asincrónicos con Kafka

- Caché distribuido con Redis

- Contenedores Docker listos para producción

- CI/CD completo con GitHub Actions + Railway


## Autor

Desarrollado por Oliver Carranza Diaz - Software Developer


