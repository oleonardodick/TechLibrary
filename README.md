# Tech Library API

Esta API tem como objetivo realizar o controle básico de uma biblioteca.

## Database

Este projeto utiliza o banco de dados PostgreSQL. Para realizar a conexão deve ser adicionado o seguinte código no arquivo appsetings.json

```json
  "ConnectionStrings": {
    "DefaultConnection": "User ID=<USUARIO>;Password=<SENHA>;Host=<HOST>;Port=5432;Database=<DATABASE>;Pooling=true;"
  },
```

## Token JWT

Esta API utiliza a estratégia de TOKEN JWT para realizar a autenticação. Para isso, é necessário adicionar a chave do token no arquivo appsettings.json. A key deve ser adicionada através do seguinte código

```json
  "JwtSettings": {
    "SigningKey": "CHAVE DE 32 CARACTERES"
  },
```
