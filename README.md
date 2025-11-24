# 🚀 TODO.Service.Api - Backend

Este é o projeto backend da aplicação ToDo, construído com .NET e utilizando PostgreSQL como banco de dados.

## 🧱 Arquitetura e Tecnologias


| Componente | Tecnologia | Localização |
| :--- | :--- | :--- |
| **API** | .NET (C#) | `./TODO.Service.Api` |
| **Database (DB)** | PostgreSQL 15 | `./db` |
| **Orquestração** | Docker Compose | `./db/docker-compose.yml` |

---

## ✅ Pré-Requisitos

Para rodar este projeto localmente, você precisará ter instalado:

1.  **[.NET SDK (8.0+ ou superior)](https://dotnet.microsoft.com/download)**
2.  **Docker Desktop** (Com o Engine rodando)

---

## ⚙️ Configuração Inicial

Antes de rodar, garanta que suas dependências estejam prontas.

1.  **Restauração de Pacotes .NET:**
    Vá para a pasta da API e restaure os pacotes NuGet:
    ```bash
    cd TODO.Service.Api
    dotnet restore
    ```
2.  **Verificação da String de Conexão:**
    Confirme que o arquivo `./TODO.Service.Api/appsettings.json` contém a string de conexão correta para o Docker:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=5432;Database=todo;Username=user;Password=password;"
    }
    ```

---

## ▶️ Como Rodar o Projeto

Você precisa iniciar o banco de dados e depois a API.

### Passo 1: Iniciar o Database (PostgreSQL)

Execute este comando na raiz do projeto (onde está o `README.md`):

```bash
# Entra na pasta do DB
cd db/

# Inicia o container em modo 'detached' (-d)
docker compose up -d
