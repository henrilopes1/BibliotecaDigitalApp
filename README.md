# Biblioteca Digital API - Checkpoint FIAP

Este projeto implementa uma API completa para gerenciamento de biblioteca digital, seguindo os princípios de Clean Architecture e desenvolvido para o checkpoint da FIAP.

## 🚀 Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **ASP.NET Core Web API** - Para criação da API REST
- **Entity Framework Core** - ORM para acesso a dados
- **Oracle Database** - Banco de dados de produção (preparado para credenciais da FIAP)
- **InMemory Database** - Banco temporário para desenvolvimento
- **Swagger/OpenAPI** - Documentação automática da API
- **Clean Architecture** - Padrão arquitetural

## 🏗️ Arquitetura do Projeto

O projeto segue a Clean Architecture com separação em 3 camadas:

```
BibliotecaDigitalApp/
├── src/
│   ├── BibliotecaDigital.Domain/     # Entidades de domínio e interfaces
│   │   ├── Models/                   # Entidades (Autor, Livro, Emprestimo, PerfilAutor)
│   │   └── Interfaces/               # Contratos dos repositórios
│   │
│   ├── BibliotecaDigital.Data/       # Camada de acesso a dados
│   │   ├── Context/                  # DbContext do Entity Framework
│   │   └── Repositories/             # Implementações dos repositórios
│   │
│   └── BibliotecaDigital.API/        # Camada de apresentação (API)
│       ├── Controllers/              # Controllers da API REST
│       └── DTOs/                     # Data Transfer Objects
│
└── BibliotecaDigitalApp.sln         # Arquivo de solução
```

### Entidades e Relacionamentos

#### 📚 **Autor** (Entidade Principal)
- **Propriedades**: Id, Nome, Email, DataNascimento, Nacionalidade
- **Relacionamentos**:
  - 1:1 com PerfilAutor (Perfil detalhado do autor)
  - 1:N com Livro (Um autor pode ter vários livros)

#### 👤 **PerfilAutor** (Relacionamento 1:1)
- **Propriedades**: Id, AutorId, Biografia, FotoUrl, Website, RedesSociais, Premios
- **Relacionamento**: Pertence a um Autor

#### 📖 **Livro** 
- **Propriedades**: Id, Titulo, AutorId, ISBN, AnoPublicacao, Editora, Genero, Sinopse, Preco, EstoqueDisponivel, EstoqueTotal
- **Relacionamentos**:
  - N:1 com Autor (Vários livros podem ter o mesmo autor)
  - 1:N com Emprestimo (Um livro pode ter vários empréstimos)

#### 📋 **Emprestimo**
- **Propriedades**: Id, LivroId, NomeUsuario, CpfUsuario, EmailUsuario, DataEmprestimo, DataDevolucaoPrevista, DataDevolucaoReal, Devolvido, MultaAtraso
- **Relacionamento**: N:1 com Livro (Vários empréstimos podem ser do mesmo livro)

## 🔧 Como Executar

### Pré-requisitos
- .NET 8 SDK
- Visual Studio Code ou Visual Studio

### Passos para execução:

1. **Clone/abra o projeto**
```bash
cd "BibliotecaDigitalApp"
```

2. **Restaurar dependências**
```bash
dotnet restore
```

3. **Compilar o projeto**
```bash
dotnet build
```

4. **Executar a aplicação**
```bash
cd src/BibliotecaDigital.API
dotnet run
```

5. **Acessar a documentação da API**
   - URL: `http://localhost:5219`
   - Swagger UI estará disponível na raiz da aplicação

## 📡 Endpoints da API

### 👥 **Autores** (`/api/autores`)
- `GET /api/autores` - Lista todos os autores
- `GET /api/autores/{id}` - Busca autor por ID
- `GET /api/autores/{id}/perfil` - Busca autor com perfil detalhado
- `GET /api/autores/{id}/livros` - Busca autor com seus livros
- `GET /api/autores/buscar/{nome}` - Busca autores por nome
- `POST /api/autores` - Cria novo autor
- `PUT /api/autores/{id}` - Atualiza autor existente
- `DELETE /api/autores/{id}` - Remove autor

### 📚 **Livros** (`/api/livros`)
- `GET /api/livros` - Lista todos os livros
- `GET /api/livros/{id}` - Busca livro por ID
- `GET /api/livros/buscar/{titulo}` - Busca livros por título
- `GET /api/livros/autor/{autorId}` - Lista livros por autor
- `GET /api/livros/disponiveis` - Lista livros disponíveis para empréstimo
- `POST /api/livros` - Cria novo livro
- `PUT /api/livros/{id}` - Atualiza livro existente
- `DELETE /api/livros/{id}` - Remove livro (soft delete)
- `PATCH /api/livros/{id}/estoque` - Atualiza estoque do livro

### 📋 **Empréstimos** (`/api/emprestimos`)
- `GET /api/emprestimos` - Lista todos os empréstimos
- `GET /api/emprestimos/{id}` - Busca empréstimo por ID
- `GET /api/emprestimos/usuario/{cpf}` - Lista empréstimos ativos por usuário
- `GET /api/emprestimos/vencidos` - Lista empréstimos vencidos
- `GET /api/emprestimos/livro/{livroId}` - Lista empréstimos por livro
- `GET /api/emprestimos/{id}/multa` - Calcula multa por atraso
- `POST /api/emprestimos` - Cria novo empréstimo
- `PUT /api/emprestimos/{id}` - Atualiza empréstimo existente
- `PATCH /api/emprestimos/{id}/devolver` - Processa devolução de livro
- `DELETE /api/emprestimos/{id}` - Remove empréstimo

## 💾 Banco de Dados

### Desenvolvimento (Atual)
- **InMemory Database**: Banco temporário em memória
- **Dados de Exemplo**: Pré-carregados automaticamente (seed data)
- **Autores**: Machado de Assis, Clarice Lispector
- **Livros**: Dom Casmurro, A Hora da Estrela

### Produção (Configuração Futura)
- **Oracle Database**: Preparado para uso com credenciais da FIAP
- **Configuração**: No arquivo `Program.cs`, descomente a linha do Oracle e configure a connection string

```csharp
// Para Oracle Database (quando credenciais da FIAP estiverem disponíveis):
options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
```

## 🔄 Funcionalidades Implementadas

### ✅ **Gerenciamento de Autores**
- CRUD completo de autores
- Relacionamento 1:1 com perfil detalhado
- Busca por nome
- Listagem de livros por autor

### ✅ **Gerenciamento de Livros**
- CRUD completo de livros
- Controle de estoque (disponível/total)
- Busca por título
- Filtro por disponibilidade
- Soft delete (marcar como inativo)

### ✅ **Sistema de Empréstimos**
- Criação de empréstimos com validações
- Controle automático de estoque
- Cálculo automático de devolução (14 dias)
- Cálculo de multas por atraso (R$ 2,00/dia)
- Histórico completo de empréstimos
- Limite de 3 empréstimos ativos por usuário

### ✅ **Validações de Negócio**
- Verificação de disponibilidade de livros
- Controle de estoque automático
- Validação de CPF e email
- Validação de dados obrigatórios
- Tratamento de erros

### ✅ **Recursos Técnicos**
- Clean Architecture
- Dependency Injection
- Repository Pattern
- Entity Framework Core
- Swagger/OpenAPI
- CORS habilitado
- Logging integrado
- DTOs para transferência de dados

## 📋 Exemplos de Uso

### Criar um Autor com Perfil
```json
POST /api/autores
{
  "nome": "José de Alencar",
  "email": "jose.alencar@literatura.com.br",
  "dataNascimento": "1829-05-01",
  "nacionalidade": "Brasileira",
  "perfil": {
    "biografia": "José Martiniano de Alencar foi um jornalista, político, advogado, orador, crítico, cronista, polemista, romancista e dramaturgo brasileiro.",
    "website": "https://www.josealencar.com.br",
    "redesSociais": "Facebook: @JoseAlencarOficial",
    "premios": "Patrono da cadeira 23 da Academia Brasileira de Letras"
  }
}
```

### Criar um Empréstimo
```json
POST /api/emprestimos
{
  "livroId": 1,
  "nomeUsuario": "Maria Silva",
  "cpfUsuario": "123.456.789-00",
  "emailUsuario": "maria.silva@email.com",
  "telefoneUsuario": "(11) 99999-9999",
  "observacoes": "Primeiro empréstimo da usuária"
}
```

### Devolver um Livro
```json
PATCH /api/emprestimos/1/devolver
{
  "dataDevolucao": "2024-01-15T10:00:00",
  "observacoes": "Livro devolvido em perfeito estado"
}
```

## 🚧 Próximas Implementações

- [ ] Sistema de autenticação JWT
- [ ] Relatórios de empréstimos
- [ ] Notificações por email
- [ ] Sistema de reservas
- [ ] Dashboard administrativo
- [ ] Integração com Oracle Database da FIAP

## 👥 Desenvolvido para

**FIAP - Checkpoint de C# (.NET)**  
Implementação de sistema de biblioteca digital seguindo Clean Architecture e boas práticas de desenvolvimento.

---

### 🎯 Objetivos Atendidos

✅ **Clean Architecture** - Separação adequada de responsabilidades  
✅ **Relacionamentos 1:1 e 1:N** - Autor/PerfilAutor (1:1) e Autor/Livros (1:N)  
✅ **Entity Framework Core** - ORM configurado com InMemory e Oracle  
✅ **API REST Completa** - CRUD para todas as entidades  
✅ **Validações de Negócio** - Regras implementadas nos repositórios  
✅ **Documentação** - Swagger/OpenAPI integrado  
✅ **Padrões de Projeto** - Repository Pattern e Dependency Injection  