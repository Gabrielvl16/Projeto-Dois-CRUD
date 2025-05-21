-- Criar o banco de dados
CREATE DATABASE IF NOT EXISTS bebidas;
USE bebidas;

-- Criar a tabela de produtos
CREATE TABLE IF NOT EXISTS produtos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    tipo VARCHAR(50) NOT NULL,
    teor_alcoolico DECIMAL(5,2),
    preco DECIMAL(10,2),
    estoque INT,
    imagem VARCHAR(255) -- Caminho da imagem
);
