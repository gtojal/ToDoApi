# ToDoApi
API RESTful desenvolvida em .NET para gerenciamento de tarefas (ToDo).

O projeto foi desenvolvido como desafio técnico, aplicando boas práticas de arquitetura, organização e desenvolvimento de APIs.

# Como Executar o Projeto

## 1. Clonar o repositório

```bash
git clone https://github.com/SEU-USUARIO/ToDoApi.git
```

---

## 2️. Acessar a pasta

```bash
cd ToDoApi
```

---

## 3️. Configurar a connection string

No arquivo:

```txt
appsettings.json
```

Configure a conexão com SQL Server:

```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ToDoDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
```

---

## 4️. Executar as migrations

```bash
dotnet ef database update
```
> É necessário possuir o SQL Server LocalDB instalado na máquina.

---

## 5️. Rodar a aplicação

```bash
dotnet run
```

---

# Swagger

Após executar o projeto:

```txt
https://localhost:xxxx/swagger
```

---

#  Funcionalidades

- Criar tarefas  
- Listar tarefas  
- Buscar tarefa por ID  
- Atualizar tarefas  
- Remover tarefas  
- Filtrar por status e data de vencimento  

---

#  Estrutura da Tarefa

| Campo | Tipo |
|---|---|
| Id | int |
| Titulo | string |
| Descricao | string |
| Status | enum |
| DataVencimento | datetime |

---

# Status disponíveis

| Valor | Descrição |
|---|---|
| 1 | Pendente |
| 2 | Em andamento |
| 3 | Concluído |



# Autor
Gabriel Tojal
