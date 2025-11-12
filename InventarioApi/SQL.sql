CREATE DATABASE inventario_db CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE inventario_db;

CREATE TABLE productos (
  id INT AUTO_INCREMENT PRIMARY KEY,
  nombre VARCHAR(100) NOT NULL,
  descripcion TEXT,
  categoria VARCHAR(50),
  precio DECIMAL(10,2) NOT NULL DEFAULT 0.00,
  stock INT NOT NULL DEFAULT 0,
  fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
