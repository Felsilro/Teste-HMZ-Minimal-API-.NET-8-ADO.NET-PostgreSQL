# Teste-HMZ-Minimal-API-.NET-8-ADO.NET-PostgreSQL-

Projeto de **minimal API em .NET 8** CRUD, estruturada em camadas (`Domain`, `Application`, `Api`) seguindo princípios **SOLID**.

---

## 🛠️ Stack utilizada
- **.NET 8 Minimal API**
- **ADO.NET + Npgsql**
- **PostgreSQL 15**
- **JWT (Bearer)** para autenticação
- **Swagger/OpenAPI** com suporte a Authorization
- **Docker & docker-compose** para subir Postgres + API
- **Serilog** para logging estruturado

---

## 📂 Estrutura do projeto

```
src/
 ├─ HMZ.Api            # Camada de apresentação (endpoints minimal API)
 ├─ HMZ.Application    # Handlers, services, DTOs
 └─ HMZ.Domain         # Entidades, modelos e repositórios (ADO.NET)
scripts/               # schema.sql + seed.sql
devops/                # Dockerfile
docker-compose.yml
.env
```

---

## 🚀 Como rodar

### Via Docker (recomendado)
```bash
docker compose up --build
```
Swagger disponível em: [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

### Via .NET CLI
1. Configure o banco local:
```bash
psql -h localhost -U hmz_user -d hmz_db -f scripts/schema.sql
psql -h localhost -U hmz_user -d hmz_db -f scripts/seed.sql
```
2. Rode a API:
```bash
dotnet restore HMZ.MinimalApi.sln
dotnet build HMZ.MinimalApi.sln -c Release
dotnet run --project src/HMZ.Api/HMZ.Api.csproj
```

---

## 🔑 Usuários de teste (seed.sql)
- **admin@hmz.com / Admin@123**  
- **janedoe@hmz.com / userpass**

---

## 📡 Endpoints principais
- `POST /api/login` → autentica e retorna JWT
- `GET /api/users` → lista usuários (paginado)
- `GET /api/users/{id}` → busca por ID
- `PUT /api/users/{id}` → atualiza dados
- `DELETE /api/users/{id}` → remove usuário

---

Enjoy it.

