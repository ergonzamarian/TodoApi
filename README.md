# API de Tarefas

API REST em .NET 10 para gestão de tarefas (ToDo).

Permite criar, listar, atualizar e remover tarefas, com filtro por status e data de vencimento. Usa Entity Framework Core com SQLite, Repository, Unit of Work, injeção de dependência e Swagger.

- Código: https://github.com/ergonzamarian/TodoApi
- Imagem Docker: https://hub.docker.com/r/ergonlima/todoapi

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- (opcional) Docker Desktop, se preferir subir com container

## Como rodar

Abra um terminal na pasta do projeto e execute:

```bash
dotnet restore
dotnet run --project src/TodoApi.Api --launch-profile http
```

Depois abra no navegador:

http://localhost:5080

O Swagger fica nessa URL. O banco SQLite (`tarefas.db`) é criado automaticamente na primeira execução, com base na migration.

## Endpoints

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/tarefas` | Lista tarefas |
| GET | `/api/tarefas/{id}` | Busca por id |
| POST | `/api/tarefas` | Cria tarefa |
| PUT | `/api/tarefas/{id}` | Atualiza tarefa |
| DELETE | `/api/tarefas/{id}` | Remove tarefa |

### Campos

- `id`
- `titulo`
- `descricao`
- `status` (`Pendente`, `Em andamento`, `Concluído`)
- `dataVencimento`

### Filtros do GET

```text
GET /api/tarefas?status=Pendente
GET /api/tarefas?dataVencimento=2026-11-20
GET /api/tarefas?status=Em%20andamento&dataVencimento=2026-11-20
```

### Exemplo de criação

```json
{
  "titulo": "Preparar entrega",
  "descricao": "Revisar endpoints e documentação",
  "dataVencimento": "2026-11-20"
}
```

### Exemplo de atualização

```json
{
  "titulo": "Preparar entrega",
  "descricao": "Revisar endpoints e documentação",
  "status": "Em andamento",
  "dataVencimento": "2026-11-20"
}
```

## Testes

```bash
dotnet test
```

## Docker

Com a imagem publicada:

```bash
docker pull ergonlima/todoapi:latest
docker run --rm -p 8080:8080 ergonlima/todoapi:latest
```

Ou construindo localmente:

```bash
docker compose up --build
```

Acesse http://localhost:8080

## Estrutura

```text
src/
  TodoApi.Domain          entidades e enums
  TodoApi.Application     DTOs, serviços e interfaces
  TodoApi.Infrastructure  EF Core, repositórios e migrations
  TodoApi.Api             controllers e Swagger
tests/
  TodoApi.UnitTests
```

## Observações

- SQLite foi escolhido para facilitar a execução em qualquer máquina, sem precisar instalar servidor de banco.
- A solução está em camadas para separar domínio, regras de aplicação, infraestrutura e API.
- Repository e Unit of Work concentram o acesso a dados e o `SaveChanges`.
