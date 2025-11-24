# Banco de Dados (PostgreSQL)

Este projeto utiliza o PostgreSQL 15 rodando em um container Docker.
Abaixo estão as instruções para execução, conexão e configuração.

=========================================
1. CREDENCIAIS DE ACESSO
=========================================
Host:           localhost
Porta:          5432
Database:       todo
User:           user
Password:       password
Container Name: todo-db

=========================================
2. COMANDOS RÁPIDOS
=========================================

# Iniciar o banco (em background):
docker compose up -d

# Parar o banco:
docker compose down

# ATENÇÃO: Resetar o banco (Apagar todos os dados):
# Use este comando se quiser limpar o banco e rodar os scripts de inicialização novamente.
docker compose down -v

=========================================
3. COMO CONECTAR
=========================================

A) NA APLICAÇÃO .NET (appsettings.json)
Configure sua Connection String no arquivo de configuração da API:

"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=todo;Username=user;Password=password;"
}

B) VIA FERRAMENTA VISUAL (DBeaver, PgAdmin, Datagrip)
Ao criar uma nova conexão PostgreSQL, utilize:
- Host: localhost
- Port: 5432
- Database: todo
- Username: user
- Password: password

C) VIA TERMINAL (Acesso Direto ao Container)
Para acessar o SQL shell (psql) diretamente de dentro do container:

docker exec -it todo-db psql -U user -d todo

(Para sair do psql, digite \q e pressione Enter)

=========================================
4. SCRIPTS INICIAIS (SEEDS)
=========================================

A pasta ./scripts é mapeada para o container.

- Qualquer arquivo .sql colocado nesta pasta será executado AUTOMATICAMENTE
  quando o container for criado pela PRIMEIRA VEZ.
- Ordem de execução: Alfabética (ex: 01_tabelas.sql, 02_dados.sql).

NOTA: Se você adicionar um script novo DEPOIS que o banco já foi criado, 
ele NÃO rodará automaticamente. Você precisará rodar o comando de reset 
(docker compose down -v) ou executar o SQL manualmente.

=========================================
5. PERSISTÊNCIA DE DADOS
=========================================

Os dados são salvos em um volume Docker chamado 'db_data'. 
Isso garante que, mesmo se você desligar o computador ou parar o container, 
seus dados não serão perdidos.