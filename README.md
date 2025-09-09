# 🖥️ To-Do API - .NET + PostgreSQL + Redis

API desenvolvida em **.NET** seguindo conceitos de **DDD** e boas práticas de arquitetura.  
Esse backend serve como base para a aplicação **To-Do List** feita em Angular.  

A stack completa roda com **Docker Compose**, incluindo:

- **PostgreSQL** (banco de dados)
- **Redis** (cache)
- **Todo API** (backend .NET)
- **Todo Web** (frontend Angular)

---

## 🚀 Tecnologias

- [.NET 8](https://dotnet.microsoft.com/)
- [PostgreSQL 15](https://www.postgresql.org/)
- [Redis 7](https://redis.io/)
- [Docker & Docker Compose](https://docs.docker.com/)

---

## ⚙️ Como Rodar Localmente

### 1. Pré-requisitos

- [Docker](https://www.docker.com/get-started) instalado
- [Docker Compose](https://docs.docker.com/compose/install/) instalado

### 2. Subir os containers

Na raiz do projeto, rode:

```bash
docker compose up -d
````

Isso vai iniciar os seguintes serviços:

- **Postgres** em `localhost:5432`
- **Redis** em `localhost:6379`
- **Todo API** em `http://localhost:8080`
- **Todo Web** em `http://localhost:8081`

### 3. Ver logs (opcional)

```bash
docker compose logs -f
```

### 4. Derrubar os containers

```bash
docker compose down
```

---

## 📌 Funcionalidades da API

- ✅ CRUD de tarefas
- ✅ Filtro por status e título
- ✅ Persistência em PostgreSQL
- ✅ Cache de entidades com Redis
- ✅ Configurações de ambiente via variáveis

---

## 🔗 Endpoints Principais

- `GET /api/todos` → Lista todas as tarefas
- `POST /api/todos` → Cria uma nova tarefa
- `PUT /api/todos/{id}` → Atualiza uma tarefa
- `DELETE /api/todos/{id}` → Remove uma tarefa

---

## 🌍 Integração com Frontend

O **frontend Angular** já está incluído no `docker-compose` e roda em:

👉 `http://localhost:8081`

Ele se comunica diretamente com a API disponível em:

👉 `http://localhost:8080`

---

👨‍💻 Desenvolvido por Rafael Santos
