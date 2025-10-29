-- ============================================
-- Script de População Inicial do Banco de Dados
-- Biblioteca Digital - FIAP Checkpoint
-- ============================================

-- Limpar dados existentes (opcional - use com cuidado!)
-- DELETE FROM TB_EMPRESTIMOS;
-- DELETE FROM TB_LIVROS;
-- DELETE FROM TB_PERFIS_AUTOR;
-- DELETE FROM TB_AUTORES;

-- ============================================
-- 1. INSERIR AUTORES
-- ============================================
INSERT INTO TB_AUTORES (ID_AUTOR, NOME, EMAIL, DATA_NASCIMENTO, NACIONALIDADE, DATA_CRIACAO)
VALUES (1, 'Machado de Assis', 'machado@biblioteca.com', TO_DATE('1839-06-21', 'YYYY-MM-DD'), 'Brasileira', SYSDATE);

INSERT INTO TB_AUTORES (ID_AUTOR, NOME, EMAIL, DATA_NASCIMENTO, NACIONALIDADE, DATA_CRIACAO)
VALUES (2, 'Clarice Lispector', 'clarice@biblioteca.com', TO_DATE('1920-12-10', 'YYYY-MM-DD'), 'Brasileira', SYSDATE);

INSERT INTO TB_AUTORES (ID_AUTOR, NOME, EMAIL, DATA_NASCIMENTO, NACIONALIDADE, DATA_CRIACAO)
VALUES (3, 'Jorge Amado', 'jorge@biblioteca.com', TO_DATE('1912-08-10', 'YYYY-MM-DD'), 'Brasileira', SYSDATE);

-- ============================================
-- 2. INSERIR PERFIS DOS AUTORES (Relação 1:1)
-- ============================================
INSERT INTO TB_PERFIS_AUTOR (ID_PERFIL, ID_AUTOR, BIOGRAFIA, FOTO_URL, WEBSITE, REDES_SOCIAIS, PREMIOS, DATA_CRIACAO)
VALUES (1, 1, 
        'Joaquim Maria Machado de Assis foi um escritor brasileiro, considerado por muitos críticos o maior nome da literatura brasileira.',
        'https://upload.wikimedia.org/wikipedia/commons/thumb/e/e2/Machado_de_Assis_001.jpg/220px-Machado_de_Assis_001.jpg',
        'https://machadodeassis.org.br',
        '{"twitter": "@machadodeassis", "instagram": "@machadodeassis"}',
        'Academia Brasileira de Letras - Fundador',
        SYSDATE);

INSERT INTO TB_PERFIS_AUTOR (ID_PERFIL, ID_AUTOR, BIOGRAFIA, FOTO_URL, WEBSITE, DATA_CRIACAO)
VALUES (2, 2,
        'Clarice Lispector foi uma escritora e jornalista nascida na Ucrânia e naturalizada brasileira. Autora de romances, contos e ensaios.',
        'https://upload.wikimedia.org/wikipedia/commons/thumb/9/99/Clarice_Lispector_1970.jpg/220px-Clarice_Lispector_1970.jpg',
        'https://claricelispector.com.br',
        SYSDATE);

INSERT INTO TB_PERFIS_AUTOR (ID_PERFIL, ID_AUTOR, BIOGRAFIA, FOTO_URL, PREMIOS, DATA_CRIACAO)
VALUES (3, 3,
        'Jorge Amado foi um dos mais famosos e traduzidos escritores brasileiros. Suas obras refletem a vida e a cultura do povo baiano.',
        'https://upload.wikimedia.org/wikipedia/commons/thumb/c/c1/Jorge_Amado_1995.jpg/220px-Jorge_Amado_1995.jpg',
        'Prêmio Jabuti (1959), Prêmio Camões (1994)',
        SYSDATE);

-- ============================================
-- 3. INSERIR LIVROS (Relação 1:N com Autores)
-- ============================================
INSERT INTO TB_LIVROS (ID_LIVRO, TITULO, ID_AUTOR, ISBN, ANO_PUBLICACAO, EDITORA, GENERO, NUMERO_EDICAO, NUMERO_PAGINAS, IDIOMA, SINOPSE, PRECO, ESTOQUE_DISPONIVEL, ESTOQUE_TOTAL, ATIVO, DATA_CRIACAO)
VALUES (1, 'Dom Casmurro', 1, '978-8535911664', 1899, 'Companhia das Letras', 'Romance', 1, 256, 'Português',
        'Romance escrito por Machado de Assis que narra a história de Bentinho e Capitu, um dos maiores clássicos da literatura brasileira.',
        45.90, 10, 15, 1, SYSDATE);

INSERT INTO TB_LIVROS (ID_LIVRO, TITULO, ID_AUTOR, ISBN, ANO_PUBLICACAO, EDITORA, GENERO, NUMERO_EDICAO, NUMERO_PAGINAS, IDIOMA, SINOPSE, PRECO, ESTOQUE_DISPONIVEL, ESTOQUE_TOTAL, ATIVO, DATA_CRIACAO)
VALUES (2, 'Memórias Póstumas de Brás Cubas', 1, '978-8535912661', 1881, 'Companhia das Letras', 'Romance', 1, 368, 'Português',
        'Romance inovador de Machado de Assis, narrado por um defunto autor que conta sua vida de forma irônica e crítica.',
        42.90, 8, 12, 1, SYSDATE);

INSERT INTO TB_LIVROS (ID_LIVRO, TITULO, ID_AUTOR, ISBN, ANO_PUBLICACAO, EDITORA, GENERO, NUMERO_EDICAO, NUMERO_PAGINAS, IDIOMA, SINOPSE, PRECO, ESTOQUE_DISPONIVEL, ESTOQUE_TOTAL, ATIVO, DATA_CRIACAO)
VALUES (3, 'A Hora da Estrela', 2, '978-8532530851', 1977, 'Rocco', 'Romance', 1, 88, 'Português',
        'Último romance de Clarice Lispector, conta a história de Macabéa, uma nordestina que vive no Rio de Janeiro.',
        38.90, 5, 10, 1, SYSDATE);

INSERT INTO TB_LIVROS (ID_LIVRO, TITULO, ID_AUTOR, ISBN, ANO_PUBLICACAO, EDITORA, GENERO, NUMERO_EDICAO, NUMERO_PAGINAS, IDIOMA, SINOPSE, PRECO, ESTOQUE_DISPONIVEL, ESTOQUE_TOTAL, ATIVO, DATA_CRIACAO)
VALUES (4, 'Capitães da Areia', 3, '978-8535914063', 1937, 'Companhia das Letras', 'Romance', 1, 280, 'Português',
        'Romance que retrata a vida de meninos de rua em Salvador, mostrando a realidade social da Bahia.',
        44.90, 12, 15, 1, SYSDATE);

-- ============================================
-- 4. INSERIR EMPRÉSTIMOS (Relação 1:N com Livros)
-- ============================================
INSERT INTO TB_EMPRESTIMOS (ID_EMPRESTIMO, ID_LIVRO, NOME_USUARIO, EMAIL_USUARIO, CPF_USUARIO, TELEFONE_USUARIO, 
                           DATA_EMPRESTIMO, DATA_DEVOLUCAO_PREVISTA, DEVOLVIDO, STATUS, DATA_CRIACAO)
VALUES (1, 1, 'João da Silva', 'joao@email.com', '123.456.789-00', '(11) 98765-4321',
        SYSDATE - 5, SYSDATE + 10, 0, 'ATIVO', SYSDATE - 5);

INSERT INTO TB_EMPRESTIMOS (ID_EMPRESTIMO, ID_LIVRO, NOME_USUARIO, EMAIL_USUARIO, CPF_USUARIO, TELEFONE_USUARIO,
                           DATA_EMPRESTIMO, DATA_DEVOLUCAO_PREVISTA, DEVOLVIDO, STATUS, DATA_CRIACAO)
VALUES (2, 3, 'Maria Santos', 'maria@email.com', '987.654.321-00', '(11) 91234-5678',
        SYSDATE - 20, SYSDATE - 6, 0, 'ATRASADO', SYSDATE - 20);

INSERT INTO TB_EMPRESTIMOS (ID_EMPRESTIMO, ID_LIVRO, NOME_USUARIO, EMAIL_USUARIO, CPF_USUARIO, TELEFONE_USUARIO,
                           DATA_EMPRESTIMO, DATA_DEVOLUCAO_PREVISTA, DATA_DEVOLUCAO_REAL, DEVOLVIDO, STATUS, DATA_CRIACAO)
VALUES (3, 2, 'Pedro Oliveira', 'pedro@email.com', '456.789.123-00', '(11) 99876-5432',
        SYSDATE - 30, SYSDATE - 16, SYSDATE - 15, 1, 'DEVOLVIDO', SYSDATE - 30);

-- ============================================
-- Commit das alterações
-- ============================================
COMMIT;

-- ============================================
-- Verificação dos dados inseridos
-- ============================================
SELECT 'AUTORES' AS TABELA, COUNT(*) AS TOTAL FROM TB_AUTORES
UNION ALL
SELECT 'PERFIS', COUNT(*) FROM TB_PERFIS_AUTOR
UNION ALL
SELECT 'LIVROS', COUNT(*) FROM TB_LIVROS
UNION ALL
SELECT 'EMPRESTIMOS', COUNT(*) FROM TB_EMPRESTIMOS;

-- ============================================
-- Fim do Script
-- ============================================
