# Exemplos de Requisições para Biblioteca Digital API

Este arquivo contém exemplos de requisições HTTP para testar todos os endpoints da API.

## Base URL
```
http://localhost:5219
```

## 📚 AUTORES

### 1. Listar todos os autores
```http
GET /api/autores
```

### 2. Buscar autor por ID
```http
GET /api/autores/1
```

### 3. Buscar autor com perfil
```http
GET /api/autores/1/perfil
```

### 4. Buscar autor com livros
```http
GET /api/autores/1/livros
```

### 5. Buscar autores por nome
```http
GET /api/autores/buscar/Machado
```

### 6. Criar novo autor (com perfil)
```http
POST /api/autores
Content-Type: application/json

{
  "nome": "José de Alencar",
  "email": "jose.alencar@literatura.com.br",
  "dataNascimento": "1829-05-01T00:00:00",
  "nacionalidade": "Brasileira",
  "perfil": {
    "biografia": "José Martiniano de Alencar foi um jornalista, político, advogado, orador, crítico, cronista, polemista, romancista e dramaturgo brasileiro. É considerado o fundador do romance de literatura brasileira.",
    "website": "https://www.josealencar.com.br",
    "redesSociais": "Facebook: @JoseAlencarOficial, Instagram: @josealencar_oficial",
    "premios": "Patrono da cadeira 23 da Academia Brasileira de Letras"
  }
}
```

### 7. Criar autor simples (sem perfil)
```http
POST /api/autores
Content-Type: application/json

{
  "nome": "Graciliano Ramos",
  "email": "graciliano@literatura.com.br",
  "dataNascimento": "1892-10-27T00:00:00",
  "nacionalidade": "Brasileira"
}
```

### 8. Atualizar autor
```http
PUT /api/autores/1
Content-Type: application/json

{
  "nome": "Joaquim Maria Machado de Assis",
  "email": "machado.assis@literatura.com",
  "dataNascimento": "1839-06-21T00:00:00",
  "nacionalidade": "Brasileira"
}
```

### 9. Excluir autor
```http
DELETE /api/autores/1
```

## 📖 LIVROS

### 1. Listar todos os livros
```http
GET /api/livros
```

### 2. Buscar livro por ID
```http
GET /api/livros/1
```

### 3. Buscar livros por título
```http
GET /api/livros/buscar/Dom
```

### 4. Listar livros por autor
```http
GET /api/livros/autor/1
```

### 5. Listar livros disponíveis
```http
GET /api/livros/disponiveis
```

### 6. Criar novo livro
```http
POST /api/livros
Content-Type: application/json

{
  "titulo": "O Guarani",
  "autorId": 3,
  "isbn": "978-85-250-0001-0",
  "anoPublicacao": 1857,
  "editora": "B. L. Garnier",
  "genero": "Romance",
  "numeroEdicao": 1,
  "numeroPaginas": 320,
  "idioma": "Português",
  "sinopse": "O Guarani é um romance do escritor brasileiro José de Alencar, publicado em 1857. A obra é considerada o primeiro romance indianista da literatura brasileira.",
  "preco": 35.90,
  "estoqueDisponivel": 8,
  "estoqueTotal": 10
}
```

### 7. Atualizar livro
```http
PUT /api/livros/1
Content-Type: application/json

{
  "titulo": "Dom Casmurro - Edição Comentada",
  "autorId": 1,
  "isbn": "978-85-250-0002-1",
  "anoPublicacao": 1899,
  "editora": "Garnier",
  "genero": "Romance",
  "numeroEdicao": 2,
  "numeroPaginas": 280,
  "idioma": "Português",
  "sinopse": "Romance narrado em primeira pessoa por Bentinho, que conta a história de seu amor por Capitu desde a infância até o casamento, levantando dúvidas sobre uma possível traição.",
  "capaUrl": "https://exemplo.com/dom-casmurro.jpg",
  "preco": 32.90,
  "estoqueDisponivel": 6,
  "estoqueTotal": 12,
  "ativo": true
}
```

### 8. Atualizar estoque de livro
```http
PATCH /api/livros/1/estoque
Content-Type: application/json

8
```

### 9. Excluir livro (soft delete)
```http
DELETE /api/livros/1
```

## 📋 EMPRÉSTIMOS

### 1. Listar todos os empréstimos
```http
GET /api/emprestimos
```

### 2. Buscar empréstimo por ID
```http
GET /api/emprestimos/1
```

### 3. Listar empréstimos por usuário (CPF)
```http
GET /api/emprestimos/usuario/123.456.789-00
```

### 4. Listar empréstimos vencidos
```http
GET /api/emprestimos/vencidos
```

### 5. Listar empréstimos por livro
```http
GET /api/emprestimos/livro/1
```

### 6. Calcular multa de empréstimo
```http
GET /api/emprestimos/1/multa
```

### 7. Criar novo empréstimo
```http
POST /api/emprestimos
Content-Type: application/json

{
  "livroId": 2,
  "nomeUsuario": "Maria Silva Santos",
  "cpfUsuario": "123.456.789-00",
  "emailUsuario": "maria.santos@email.com",
  "telefoneUsuario": "(11) 99999-9999",
  "observacoes": "Primeiro empréstimo da usuária. Interesse em literatura brasileira."
}
```

### 8. Criar empréstimo mínimo
```http
POST /api/emprestimos
Content-Type: application/json

{
  "livroId": 1,
  "nomeUsuario": "João Oliveira",
  "cpfUsuario": "987.654.321-00",
  "emailUsuario": "joao.oliveira@email.com"
}
```

### 9. Atualizar dados do empréstimo
```http
PUT /api/emprestimos/1
Content-Type: application/json

{
  "nomeUsuario": "Maria Silva Santos Costa",
  "emailUsuario": "maria.santos.costa@newemail.com",
  "telefoneUsuario": "(11) 88888-8888",
  "observacoes": "Usuária atualizou seus dados de contato."
}
```

### 10. Devolver livro
```http
PATCH /api/emprestimos/1/devolver
Content-Type: application/json

{
  "dataDevolucao": "2024-01-20T14:30:00",
  "observacoes": "Livro devolvido em perfeito estado. Usuária elogiou a obra."
}
```

### 11. Devolver livro com atraso
```http
PATCH /api/emprestimos/1/devolver
Content-Type: application/json

{
  "dataDevolucao": "2024-02-15T09:15:00",
  "observacoes": "Livro devolvido com 3 dias de atraso. Multa aplicada."
}
```

### 12. Excluir empréstimo
```http
DELETE /api/emprestimos/1
```

## 🚨 CENÁRIOS DE TESTE

### Validações de Negócio

#### 1. Tentar emprestar livro indisponível
```http
POST /api/emprestimos
Content-Type: application/json

{
  "livroId": 999,
  "nomeUsuario": "Teste Usuario",
  "cpfUsuario": "000.000.000-00",
  "emailUsuario": "teste@email.com"
}
```
**Resultado esperado**: Erro 404 - Livro não encontrado

#### 2. Tentar criar livro com autor inexistente
```http
POST /api/livros
Content-Type: application/json

{
  "titulo": "Livro Teste",
  "autorId": 999,
  "estoqueDisponivel": 5,
  "estoqueTotal": 5
}
```
**Resultado esperado**: Erro 400 - Autor não encontrado

#### 3. Tentar devolver livro já devolvido
```http
PATCH /api/emprestimos/1/devolver
Content-Type: application/json

{
  "dataDevolucao": "2024-01-20T14:30:00"
}
```
**Resultado esperado**: Erro 400 - Livro já foi devolvido

### Dados Inválidos

#### 1. Criar autor com dados inválidos
```http
POST /api/autores
Content-Type: application/json

{
  "nome": "",
  "email": "email-invalido"
}
```
**Resultado esperado**: Erro 400 - Validation errors

#### 2. Criar empréstimo com CPF inválido
```http
POST /api/emprestimos
Content-Type: application/json

{
  "livroId": 1,
  "nomeUsuario": "Teste",
  "cpfUsuario": "123",
  "emailUsuario": "email-invalido"
}
```
**Resultado esperado**: Erro 400 - Validation errors

## 📊 DADOS DE EXEMPLO PRÉ-CARREGADOS

A aplicação já vem com dados de exemplo:

### Autores:
- **ID 1**: Machado de Assis (com perfil)
- **ID 2**: Clarice Lispector (com perfil)

### Livros:
- **ID 1**: Dom Casmurro (Machado de Assis)
- **ID 2**: A Hora da Estrela (Clarice Lispector)

### Para testar rapidamente:
1. Acesse `http://localhost:5219` no navegador
2. Use o Swagger UI para testar os endpoints interativamente
3. Os dados de exemplo já estarão disponíveis para consulta