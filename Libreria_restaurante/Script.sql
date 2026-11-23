USE master;
GO

-- Borrar la base de datos si ya existe (cerrando conexiones activas)
IF DB_ID('db_Restaurante') IS NOT NULL
BEGIN
    ALTER DATABASE db_Restaurante SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE db_Restaurante;
END
GO

CREATE DATABASE db_Restaurante;
GO

USE db_Restaurante;
GO

-- =============================================
-- 1. CREACIÓN DE TABLAS
-- =============================================

CREATE TABLE [Categorias]
(
	[id] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY,
	[nombre] NVARCHAR(20) NOT NULL
);
GO

CREATE TABLE [MetodosPagos]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[tipo] NVARCHAR(20) NOT NULL
);
GO

CREATE TABLE [Proveedores]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombre] NVARCHAR(50) NOT NULL,
	[contacto] NVARCHAR(50) NOT NULL,
	[direccion] NVARCHAR(100) NOT NULL,
	[telefono] NVARCHAR(20) NOT NULL
);
GO

CREATE TABLE [Clientes]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombre] NVARCHAR(50) NOT NULL,
	[telefono] NVARCHAR(20) NOT NULL,
	[cedula] NVARCHAR(20) NOT NULL,
	[direccion] NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE [Mesas]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[numero] INT NOT NULL,
	[capacidad] INT NOT NULL,
	[estado] NVARCHAR(20) NOT NULL,
	[ubicacion] NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE [Empleados]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombre] NVARCHAR(50) NOT NULL,
	[telefono] NVARCHAR(20) NOT NULL,
	[direccion] NVARCHAR(50) NOT NULL,
	[salario] DECIMAL(18, 2) NOT NULL,
	[fecha_contratacion] DATETIME NOT NULL,
	[cargo] NVARCHAR(20) NOT NULL
);
GO

CREATE TABLE [Menus]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombre] NVARCHAR(50) NOT NULL,
	[vigencia_desde] DATETIME NOT NULL,
	[vigencia_hasta] DATETIME NOT NULL,
	[tipo] NVARCHAR(20) NOT NULL
);
GO

CREATE TABLE [Productos]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombre] NVARCHAR(50) NOT NULL,
	[precio] DECIMAL(18, 2) NOT NULL DEFAULT 0,
	[vigencia_desde] DATETIME NOT NULL,
	[vigencia_hasta] DATETIME NOT NULL,
	[tipo] NVARCHAR(20) NOT NULL,
	[categoria] INT NOT NULL,
	[menu] INT NOT NULL,
	CONSTRAINT FK_Productos_Categorias FOREIGN KEY ([categoria]) REFERENCES [Categorias]([id]),
	CONSTRAINT FK_Productos_Menus FOREIGN KEY ([menu]) REFERENCES [Menus]([id])
);
GO

CREATE TABLE [Recetas]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[producto] INT NOT NULL,
	[tiempo_preparacion] DATETIME NOT NULL,
	[instrucciones] NVARCHAR(MAX) NOT NULL,
	[porciones] INT NOT NULL,
	[dificultad] NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_Recetas_Productos FOREIGN KEY ([producto]) REFERENCES [Productos]([id])
);
GO

CREATE TABLE [Ingredientes]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombre] NVARCHAR(50) NOT NULL,
	[unidad_medida] NVARCHAR(20) NOT NULL,
	[costo_unitario] DECIMAL(18, 2) NOT NULL,
	[proveedor] INT NOT NULL,
	[receta] INT NOT NULL,
	CONSTRAINT FK_Ingredientes_Proveedores FOREIGN KEY ([proveedor]) REFERENCES [Proveedores]([id]),
	CONSTRAINT FK_Ingredientes_Recetas FOREIGN KEY ([receta]) REFERENCES [Recetas]([id])
);
GO

CREATE TABLE [Turnos]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[empleado] INT NOT NULL,
	[fecha] DATETIME NOT NULL,
	[hora_entrada] DATETIME NOT NULL,
	[hora_salida] DATETIME NOT NULL,
	CONSTRAINT FK_Turnos_Empleados FOREIGN KEY ([empleado]) REFERENCES [Empleados]([id])
);
GO

CREATE TABLE [Meseros]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[empleado] INT NOT NULL,
	[zona_asignada] NVARCHAR(50) NOT NULL,
	[num_mesas_asignadas] INT NOT NULL,
	[calificacion_promedio] DECIMAL(3, 2) NOT NULL,
	CONSTRAINT FK_Meseros_Empleados FOREIGN KEY ([empleado]) REFERENCES [Empleados]([id])
);
GO

CREATE TABLE [Gerentes]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[empleado] INT NOT NULL,
	[nivel_autorizacion] NVARCHAR(20) NOT NULL,
	[bono_desempeno] DECIMAL(18, 2) NOT NULL,
	[area_responsable] NVARCHAR(50) NOT NULL,
	CONSTRAINT FK_Gerentes_Empleados FOREIGN KEY ([empleado]) REFERENCES [Empleados]([id])
);
GO

CREATE TABLE [Cocineros]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[empleado] INT NOT NULL,
	[especialidad] NVARCHAR(50) NOT NULL,
	[anios_experiencia] INT NOT NULL,
	[estacion_asignada] NVARCHAR(50) NOT NULL,
	CONSTRAINT FK_Cocineros_Empleados FOREIGN KEY ([empleado]) REFERENCES [Empleados]([id])
);
GO

CREATE TABLE [Inventarios]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[ingrediente] INT NOT NULL,
	[stock_actual] DECIMAL(18, 2) NOT NULL,
	[stock_minimo] DECIMAL(18, 2) NOT NULL,
	[fecha_actualizacion] DATETIME NOT NULL,
	CONSTRAINT FK_Inventarios_Ingredientes FOREIGN KEY ([ingrediente]) REFERENCES [Ingredientes]([id])
);
GO

CREATE TABLE [Reservas]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[cliente] INT NOT NULL,
	[mesa] INT NOT NULL,
	[fecha_hora] DATETIME NOT NULL,
	[numero_personas] INT NOT NULL,
	[estado] NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_Reservas_Clientes FOREIGN KEY ([cliente]) REFERENCES [Clientes]([id]),
	CONSTRAINT FK_Reservas_Mesas FOREIGN KEY ([mesa]) REFERENCES [Mesas]([id])
);
GO

CREATE TABLE [Pedidos]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[cliente] INT NOT NULL,
	[mesa] INT NOT NULL,
	[mesero] INT NOT NULL,
	[fecha_hora] DATETIME NOT NULL,
	[numero_personas] INT NOT NULL,
	[estado] NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_Pedidos_Clientes FOREIGN KEY ([cliente]) REFERENCES [Clientes]([id]),
	CONSTRAINT FK_Pedidos_Mesas FOREIGN KEY ([mesa]) REFERENCES [Mesas]([id]),
	CONSTRAINT FK_Pedidos_Meseros FOREIGN KEY ([mesero]) REFERENCES [Meseros]([id])
);
GO

CREATE TABLE [Detalles_Pedidos]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[pedido] INT NOT NULL,
	[producto] INT NOT NULL,
	[cantidad] INT NOT NULL,
	[subtotal] DECIMAL(18, 2) NOT NULL,
	[comentarios] NVARCHAR(200) NULL,
	CONSTRAINT FK_DetallesPedidos_Pedidos FOREIGN KEY ([pedido]) REFERENCES [Pedidos]([id]),
	CONSTRAINT FK_DetallesPedidos_Productos FOREIGN KEY ([producto]) REFERENCES [Productos]([id])
);
GO

CREATE TABLE [Facturas]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[pedido] INT NOT NULL,
	[numero] NVARCHAR(20) NOT NULL,
	[fecha_emision] DATETIME NOT NULL,
	[subtotal] DECIMAL(18, 2) NOT NULL,
	[total] DECIMAL(18, 2) NOT NULL,
	CONSTRAINT FK_Facturas_Pedidos FOREIGN KEY ([pedido]) REFERENCES [Pedidos]([id])
);
GO

CREATE TABLE [Pagos]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[factura] INT NOT NULL,
	[metodoPago] INT NOT NULL,
	[monto] DECIMAL(18, 2) NOT NULL,
	[fecha] DATETIME NOT NULL,
	[referencia] NVARCHAR(50) NOT NULL,
	[estado] NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_Pagos_Facturas FOREIGN KEY ([factura]) REFERENCES [Facturas]([id]),
	CONSTRAINT FK_Pagos_MetodosPagos FOREIGN KEY ([metodoPago]) REFERENCES [MetodosPagos]([id])
);
GO

CREATE TABLE [Roles]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombre] NVARCHAR(50) NOT NULL,
	[descripcion] NVARCHAR(200) NULL,
	[activo] BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE [Usuarios]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombreUsuario] NVARCHAR(50) NOT NULL,
	[correo] NVARCHAR(150) NOT NULL,
	[contraseña] NVARCHAR(255) NOT NULL,
	[activo] BIT NOT NULL DEFAULT 1,
	[fechaCreacion] DATETIME NOT NULL DEFAULT GETDATE(),
	[rol] INT NOT NULL,
	CONSTRAINT FK_Usuarios_Roles FOREIGN KEY ([rol]) REFERENCES [Roles]([id])
);
GO

CREATE TABLE [Permisos]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombre] NVARCHAR(100) NOT NULL,
	[descripcion] NVARCHAR(200) NULL,
	[rol] INT NOT NULL,
	CONSTRAINT UQ_Permisos_Rol_Nombre UNIQUE ([rol], [nombre]),
	CONSTRAINT FK_Permisos_Roles FOREIGN KEY ([rol]) REFERENCES [Roles]([id])
);
GO

CREATE TABLE [Auditorias]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[tabla] NVARCHAR(100) NOT NULL,
	[accion] NVARCHAR(50) NOT NULL,
	[fecha] DATETIME NOT NULL DEFAULT GETDATE(),
	[datosAnteriores] NVARCHAR(MAX) NULL,
	[datosNuevos] NVARCHAR(MAX) NULL,
	[usuario] INT NULL,
	CONSTRAINT FK_Auditorias_Usuarios FOREIGN KEY ([usuario]) REFERENCES [Usuarios]([id])
);
GO

-- =============================================
-- 2. INSERCIÓN DE DATOS INICIALES
-- =============================================

-- 1. Categorias
INSERT INTO Categorias (nombre) VALUES 
(N'Entradas'),
(N'Platos Fuertes'),
(N'Pastas y Arroces'),
(N'Carnes y Parrilla'),
(N'Bebidas'),
(N'Postres');

-- 2. Menus
INSERT INTO Menus (nombre, vigencia_desde, vigencia_hasta, tipo) VALUES
(N'Carta Principal', '2026-01-01', '2026-12-31', N'Almuerzo y Cena'),
(N'Menú Ejecutivo', '2026-01-01', '2026-12-31', N'Almuerzo'),
(N'Especial Fin de Semana', '2026-01-01', '2026-12-31', N'Fin de Semana');

-- 3. Productos (con precio incluido)
DECLARE @catCarnes INT = (SELECT TOP 1 id FROM Categorias WHERE nombre = N'Carnes y Parrilla');
DECLARE @catPastas INT = (SELECT TOP 1 id FROM Categorias WHERE nombre = N'Pastas y Arroces');
DECLARE @catBebidas INT = (SELECT TOP 1 id FROM Categorias WHERE nombre = N'Bebidas');
DECLARE @catPostres INT = (SELECT TOP 1 id FROM Categorias WHERE nombre = N'Postres');
DECLARE @catEntradas INT = (SELECT TOP 1 id FROM Categorias WHERE nombre = N'Entradas');
DECLARE @menuCarta INT = (SELECT TOP 1 id FROM Menus WHERE nombre = N'Carta Principal');

INSERT INTO Productos (nombre, precio, vigencia_desde, vigencia_hasta, tipo, categoria, menu) VALUES
(N'Lomo al Trapo Angus 350g', 45000.00, '2026-01-01', '2026-12-31', N'Plato Fuerte', @catCarnes, @menuCarta),
(N'Fettuccine Alfredo con Camarón', 38000.00, '2026-01-01', '2026-12-31', N'Plato Fuerte', @catPastas, @menuCarta),
(N'Carpaccio de Salmón Ahumado', 26000.00, '2026-01-01', '2026-12-31', N'Entrada', @catEntradas, @menuCarta),
(N'Limonada de Coco Cremosita', 12000.00, '2026-01-01', '2026-12-31', N'Bebida', @catBebidas, @menuCarta),
(N'Copa de Vino Tinto Malbec Reserva', 22000.00, '2026-01-01', '2026-12-31', N'Bebida', @catBebidas, @menuCarta),
(N'Cheesecake de Frutos Rojos', 16500.00, '2026-01-01', '2026-12-31', N'Postre', @catPostres, @menuCarta),
(N'Tiramisú Artesanal Italiano', 18000.00, '2026-01-01', '2026-12-31', N'Postre', @catPostres, @menuCarta);

-- 4. Proveedores
INSERT INTO Proveedores (nombre, contacto, direccion, telefono) VALUES
(N'Distribuidora Gourmet del Valle', N'Carlos Mendoza', N'Calle 45 # 12-34, Bogotá', N'3101234567'),
(N'Carnes Premium Angus SAS', N'Laura Restrepo', N'Carrera 68 # 80-20, Medellín', N'3159876543'),
(N'Lácteos y Vinos El Poblado', N'Andrés Gómez', N'Avenida Las Palmas # 15-50', N'3204567890');

-- 5. Recetas e Ingredientes
DECLARE @prodLomo INT = (SELECT TOP 1 id FROM Productos WHERE nombre LIKE N'%Lomo%');
DECLARE @provCarnes INT = (SELECT TOP 1 id FROM Proveedores WHERE nombre LIKE N'%Carnes%');

INSERT INTO Recetas (producto, tiempo_preparacion, instrucciones, porciones, dificultad) VALUES
(@prodLomo, '2026-01-01 00:25:00', N'Sazonar el corte Angus en sal marina y hierbas finas. Envolver en tela y cocinar a fuego directo 20 min.', 1, N'Media');

DECLARE @idReceta INT = SCOPE_IDENTITY();

INSERT INTO Ingredientes (nombre, unidad_medida, costo_unitario, proveedor, receta) VALUES
(N'Corte Angus Baby Beef', N'Gramos', 45000.00, @provCarnes, @idReceta),
(N'Sal Marina Fina', N'Gramos', 2000.00, @provCarnes, @idReceta);

-- 6. Inventarios
DECLARE @ing1 INT = (SELECT TOP 1 id FROM Ingredientes);
INSERT INTO Inventarios (ingrediente, stock_actual, stock_minimo, fecha_actualizacion) VALUES
(@ing1, 35.50, 10.00, GETDATE());

-- 7. Mesas 
INSERT INTO Mesas (numero, capacidad, estado, ubicacion) VALUES
(1, 4, N'Disponible', N'Salón Principal'),
(2, 2, N'Ocupada', N'Salón Principal'),
(3, 6, N'Reservada', N'Salón Principal'),
(4, 4, N'Disponible', N'Salón Principal'),
(5, 2, N'Disponible', N'Terraza'),
(6, 4, N'Disponible', N'Terraza'),
(7, 4, N'Reservada', N'Terraza'),
(8, 8, N'Disponible', N'Zona VIP'),
(9, 10, N'Disponible', N'Zona VIP'),
(10, 2, N'Disponible', N'Barra');

-- 8. Clientes 
INSERT INTO Clientes (nombre, telefono, cedula, direccion) VALUES
(N'Consumidor Final', N'N/A', N'2222222222', N'Mostrador / Salón'),
(N'Juan David Pérez', N'3001122334', N'1020304050', N'Calle 100 # 15-20, Apto 501'),
(N'María Fernanda Ortiz', N'3119988776', N'1030405060', N'Carrera 7 # 120-45'),
(N'Santiago Gómez Vélez', N'3205544332', N'1040506070', N'Calle 85 # 11-18'),
(N'Camila Rodríguez', N'3154433221', N'1050607080', N'Avenida 19 # 104-30');

-- 9. Empleados, Meseros, Cocineros y Gerentes
INSERT INTO Empleados (nombre, telefono, direccion, salario, fecha_contratacion, cargo) VALUES
(N'Carlos Ruiz', N'3008877665', N'Barrio Laureles', 1800000.00, '2025-01-15', N'Mesero'),
(N'Valentina M', N'3127766554', N'Barrio El Poblado', 1800000.00, '2025-02-01', N'Mesero'),
(N'Mario Batali', N'3156655443', N'Envigado', 3800000.00, '2024-06-10', N'Chef Principal'),
(N'Gloria Tobon', N'3201122445', N'Belen', 3200000.00, '2024-03-01', N'Gerente');

DECLARE @empMesero1 INT = (SELECT TOP 1 id FROM Empleados WHERE nombre = N'Carlos Ruiz');
DECLARE @empMesero2 INT = (SELECT TOP 1 id FROM Empleados WHERE nombre = N'Valentina M');
DECLARE @empCocinero INT = (SELECT TOP 1 id FROM Empleados WHERE nombre = N'Mario Batali');
DECLARE @empGerente INT = (SELECT TOP 1 id FROM Empleados WHERE nombre = N'Gloria Tobon');

INSERT INTO Meseros (empleado, zona_asignada, num_mesas_asignadas, calificacion_promedio) VALUES
(@empMesero1, N'Salón Principal', 4, 4.85),
(@empMesero2, N'Terraza y VIP', 5, 4.92);

INSERT INTO Cocineros (empleado, especialidad, anios_experiencia, estacion_asignada) VALUES
(@empCocinero, N'Parrilla y Carnes', 12, N'Estación Caliente 1');

INSERT INTO Gerentes (empleado, nivel_autorizacion, bono_desempeno, area_responsable) VALUES
(@empGerente, N'Nivel Alto - Total', 500000.00, N'Operaciones Generales');

-- 10. Metodos de Pago
INSERT INTO MetodosPagos (tipo) VALUES
(N'Efectivo'),
(N'Tarjeta Credito'),
(N'Tarjeta Debito'),
(N'Transf Nequi');

-- 11. Pedido activo de ejemplo en Mesa 2
DECLARE @cli1 INT = (SELECT TOP 1 id FROM Clientes WHERE cedula = N'1020304050');
DECLARE @mesa2 INT = (SELECT TOP 1 id FROM Mesas WHERE numero = 2);
DECLARE @mesero1 INT = (SELECT TOP 1 id FROM Meseros);
DECLARE @prod1 INT = (SELECT TOP 1 id FROM Productos WHERE nombre LIKE N'%Lomo%');
DECLARE @prod2 INT = (SELECT TOP 1 id FROM Productos WHERE nombre LIKE N'%Limonada%');

INSERT INTO Pedidos (cliente, mesa, mesero, fecha_hora, numero_personas, estado) VALUES
(@cli1, @mesa2, @mesero1, GETDATE(), 2, N'En preparación');

DECLARE @idPedido INT = SCOPE_IDENTITY();

INSERT INTO Detalles_Pedidos (pedido, producto, cantidad, subtotal, comentarios) VALUES
(@idPedido, @prod1, 2, 90000.00, N'Término 3/4, una sin sal'),
(@idPedido, @prod2, 2, 24000.00, N'Bien frías');

-- 12. Rol 1: Administrador y Usuario Admin
INSERT INTO [Roles] ([nombre], [descripcion], [activo])
VALUES (N'Administrador', N'Acceso total al sistema', 1);

DECLARE @idRolAdmin INT = SCOPE_IDENTITY();

INSERT INTO [Permisos] ([nombre], [descripcion], [rol])
VALUES
(N'Categorias', N'Acceso a Categorias', @idRolAdmin),
(N'Clientes', N'Acceso a Clientes', @idRolAdmin),
(N'Cocineros', N'Acceso a Cocineros', @idRolAdmin),
(N'Detalles_Pedidos', N'Acceso a Detalles_Pedidos', @idRolAdmin),
(N'Empleados', N'Acceso a Empleados', @idRolAdmin),
(N'Facturas', N'Acceso a Facturas', @idRolAdmin),
(N'Gerentes', N'Acceso a Gerentes', @idRolAdmin),
(N'Ingredientes', N'Acceso a Ingredientes', @idRolAdmin),
(N'Inventarios', N'Acceso a Inventarios', @idRolAdmin),
(N'Menus', N'Acceso a Menus', @idRolAdmin),
(N'Mesas', N'Acceso a Mesas', @idRolAdmin),
(N'Meseros', N'Acceso a Meseros', @idRolAdmin),
(N'MetodosPagos', N'Acceso a MetodosPagos', @idRolAdmin),
(N'Pagos', N'Acceso a Pagos', @idRolAdmin),
(N'Pedidos', N'Acceso a Pedidos', @idRolAdmin),
(N'Productos', N'Acceso a Productos', @idRolAdmin),
(N'Proveedores', N'Acceso a Proveedores', @idRolAdmin),
(N'Recetas', N'Acceso a Recetas', @idRolAdmin),
(N'Reservas', N'Acceso a Reservas', @idRolAdmin),
(N'Turnos', N'Acceso a Turnos', @idRolAdmin),
(N'Roles', N'Acceso a Roles', @idRolAdmin),
(N'Usuarios', N'Acceso a Usuarios', @idRolAdmin),
(N'Permisos', N'Acceso a Permisos', @idRolAdmin),
(N'Auditorias', N'Acceso a Auditorias', @idRolAdmin),
(N'Reportes', N'Acceso a Reportes PDF y Excel', @idRolAdmin),
(N'MapaMesas', N'Acceso a Plano Interactivo de Mesas', @idRolAdmin),
(N'Impresion', N'Acceso a Impresion de Comandas y Facturas', @idRolAdmin);

INSERT INTO [Usuarios] ([nombreUsuario], [correo], [contraseña], [activo], [fechaCreacion], [rol])
VALUES (N'admin', N'admin@restaurante.com', N'1234', 1, GETDATE(), @idRolAdmin);

-- 13. Rol 2: Mesero y Usuario Mesero
INSERT INTO [Roles] ([nombre], [descripcion], [activo])
VALUES (N'Mesero', N'Atención de mesas, toma de pedidos, reservas y facturación', 1);

DECLARE @idRolMesero INT = SCOPE_IDENTITY();

INSERT INTO [Permisos] ([nombre], [descripcion], [rol])
VALUES
(N'MapaMesas', N'Acceso a Plano Interactivo de Mesas', @idRolMesero),
(N'Pedidos', N'Toma y gestión de pedidos', @idRolMesero),
(N'Detalles_Pedidos', N'Gestión de platos en pedidos', @idRolMesero),
(N'Clientes', N'Registro opcional de clientes', @idRolMesero),
(N'Reservas', N'Consulta y registro de reservas', @idRolMesero),
(N'Productos', N'Consulta de carta y precios', @idRolMesero),
(N'Facturas', N'Generación y consulta de facturas', @idRolMesero),
(N'Pagos', N'Registro de pagos', @idRolMesero),
(N'Impresion', N'Impresión de comandas y tickets', @idRolMesero);

INSERT INTO [Usuarios] ([nombreUsuario], [correo], [contraseña], [activo], [fechaCreacion], [rol])
VALUES (N'mesero', N'mesero@restaurante.com', N'1234', 1, GETDATE(), @idRolMesero);
GO