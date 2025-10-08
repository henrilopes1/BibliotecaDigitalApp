# Exemplo de Teste do Endpoint PesquisarLivros

## Testando busca por livros existentes na base local
GET http://localhost:5219/api/livros/pesquisar/Dom

## Testando busca por livros que não existem na base local (busca externa no Google Books)
GET http://localhost:5219/api/livros/pesquisar/python

## Testando busca por livros com termo mais específico
GET http://localhost:5219/api/livros/pesquisar/Clean Code

## Testando busca com termo vazio (deve retornar erro)
GET http://localhost:5219/api/livros/pesquisar/

## Testando com termo com espaços
GET http://localhost:5219/api/livros/pesquisar/Design Patterns