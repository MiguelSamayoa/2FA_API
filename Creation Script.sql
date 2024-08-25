
CREATE TABLE `producto` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(255) DEFAULT NULL,
  `Descripcion` text,
  `Precio` float DEFAULT NULL,
  `Stock` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `user` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserTag` varchar(255) DEFAULT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `Salt` blob,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `sessiontoken` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Token` varchar(255) DEFAULT NULL,
  `Creacion` datetime DEFAULT NULL,
  `UserId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `sessiontoken_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


INSERT INTO `producto` (`Nombre`, `Descripcion`, `Precio`, `Stock`) VALUES
('Laptop ASUS', 'Laptop ASUS con 16GB de RAM y 512GB SSD', 1200.50, 10),
('Smartphone Samsung Galaxy', 'Samsung Galaxy S21 con pantalla AMOLED', 950.00, 25),
('Tablet Lenovo', 'Tablet Lenovo 10 pulgadas, 64GB', 200.99, 15),
('Teclado Mecánico', 'Teclado mecánico retroiluminado RGB', 75.00, 50),
('Mouse Inalámbrico', 'Mouse inalámbrico con sensor óptico avanzado', 35.75, 80),
('Monitor LG 24"', 'Monitor LG de 24 pulgadas Full HD', 180.40, 12),
('Impresora HP', 'Impresora multifuncional HP con conexión Wi-Fi', 120.00, 20),
('Disco Duro Externo', 'Disco duro externo de 2TB USB 3.0', 89.99, 30),
('Auriculares Bluetooth', 'Auriculares Bluetooth con cancelación de ruido', 45.50, 60),
('Cámara Web HD', 'Cámara web HD 1080p para videollamadas', 55.99, 35),
('Silla Gamer', 'Silla ergonómica para gaming con soporte lumbar', 220.00, 8),
('Mochila para Laptop', 'Mochila resistente al agua para laptops de hasta 15"', 65.30, 45),
('Smartwatch', 'Reloj inteligente con monitor de actividad y notificaciones', 99.90, 40),
('Router Wi-Fi', 'Router Wi-Fi de doble banda con cobertura amplia', 70.25, 18),
('Cargador Rápido USB-C', 'Cargador rápido USB-C con 30W de potencia', 25.99, 100),
('Micrófono Condensador', 'Micrófono condensador con soporte antichoque', 150.50, 22),
('Lámpara LED Escritorio', 'Lámpara LED de escritorio con ajuste de brillo', 30.99, 70),
('Disco SSD 1TB', 'Disco SSD de 1TB con velocidad de transferencia ultra rápida', 150.00, 15),
('Adaptador HDMI a VGA', 'Adaptador HDMI a VGA para pantallas antiguas', 12.50, 55),
('Batería Portátil 10000mAh', 'Batería portátil con capacidad de 10000mAh y carga rápida', 40.00, 50);

