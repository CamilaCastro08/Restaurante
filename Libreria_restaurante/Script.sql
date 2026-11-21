USE master;
GO

-- 1. Eliminar base de datos anterior si existe (cerrando conexiones activas)
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'db_Restaurante')
BEGIN
    ALTER DATABASE db_Restaurante SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE db_Restaurante;
END
GO

-- 2. Crear base de datos limpia
CREATE DATABASE db_Restaurante;
GO

USE db_Restaurante;
GO

-- =========================================================
-- TABLAS MAESTRAS Y DE SEGURIDAD
-- =========================================================

CREATE TABLE [Roles] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre] NVARCHAR(50) NOT NULL,
    [descripcion] NVARCHAR(150) NULL
);

CREATE TABLE [Permisos] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre] NVARCHAR(80) NOT NULL,
    [rol] INT NOT NULL,
    CONSTRAINT FK_Permisos_Roles FOREIGN KEY ([rol]) REFERENCES [Roles]([id])
);

CREATE TABLE [Usuarios] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre] NVARCHAR(80) NOT NULL,
    [correo] NVARCHAR(100) NOT NULL,
    [contrasena] NVARCHAR(150) NOT NULL,
    [estado] NVARCHAR(30) NOT NULL,
    [rol] INT NOT NULL,
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY ([rol]) REFERENCES [Roles]([id])
);

CREATE TABLE [Auditorias] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [tabla] NVARCHAR(80) NOT NULL,
    [accion] NVARCHAR(30) NOT NULL,
    [fecha] DATETIME NOT NULL DEFAULT GETDATE(),
    [datosAnteriores] NVARCHAR(MAX) NULL,
    [datosNuevos] NVARCHAR(MAX) NULL,
    [usuario] INT NULL
);

-- =========================================================
-- PERSONAL Y TURNOS
-- =========================================================

CREATE TABLE [Empleados] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre] NVARCHAR(80) NOT NULL,
    [cedula] NVARCHAR(25) NOT NULL,
    [telefono] NVARCHAR(25) NOT NULL,
    [cargo] NVARCHAR(50) NOT NULL,
    [salario] DECIMAL(18, 2) NOT NULL,
    [fecha_ingreso] DATETIME NOT NULL
);

CREATE TABLE [Meseros] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [empleado] INT NOT NULL,
    [zona_asignada] NVARCHAR(60) NOT NULL,
    [propina_acumulada] DECIMAL(18, 2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Meseros_Empleados FOREIGN KEY ([empleado]) REFERENCES [Empleados]([id])
);

CREATE TABLE [Cocineros] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [empleado] INT NOT NULL,
    [especialidad] NVARCHAR(80) NOT NULL,
    [turno_preferido] NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Cocineros_Empleados FOREIGN KEY ([empleado]) REFERENCES [Empleados]([id])
);

CREATE TABLE [Gerentes] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [empleado] INT NOT NULL,
    [area_responsable] NVARCHAR(80) NOT NULL,
    [bono_anual] DECIMAL(18, 2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Gerentes_Empleados FOREIGN KEY ([empleado]) REFERENCES [Empleados]([id])
);

CREATE TABLE [Turnos] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [empleado] INT NOT NULL,
    [fecha] DATETIME NOT NULL,
    [hora_entrada] DATETIME NOT NULL,
    [hora_salida] DATETIME NOT NULL,
    CONSTRAINT FK_Turnos_Empleados FOREIGN KEY ([empleado]) REFERENCES [Empleados]([id])
);

-- =========================================================
-- CARTA, RECETAS E INVENTARIO
-- =========================================================

CREATE TABLE [Categorias] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre] NVARCHAR(80) NOT NULL,
    [descripcion] NVARCHAR(150) NULL
);

CREATE TABLE [Menus] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre] NVARCHAR(80) NOT NULL,
    [vigencia_desde] DATETIME NOT NULL,
    [vigencia_hasta] DATETIME NOT NULL,
    [tipo] NVARCHAR(50) NOT NULL
);

CREATE TABLE [Productos] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre] NVARCHAR(100) NOT NULL,
    [vigencia_desde] DATETIME NOT NULL,
    [vigencia_hasta] DATETIME NOT NULL,
    [tipo] NVARCHAR(50) NOT NULL,
    [precio] DECIMAL(18, 2) NOT NULL DEFAULT 0,
    [categoria] INT NOT NULL,
    [menu] INT NOT NULL,
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY ([categoria]) REFERENCES [Categorias]([id]),
    CONSTRAINT FK_Productos_Menus FOREIGN KEY ([menu]) REFERENCES [Menus]([id])
);

CREATE TABLE [Proveedores] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre] NVARCHAR(100) NOT NULL,
    [nit] NVARCHAR(30) NOT NULL,
    [telefono] NVARCHAR(25) NOT NULL,
    [correo] NVARCHAR(100) NULL
);

CREATE TABLE [Ingredientes] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre] NVARCHAR(80) NOT NULL,
    [unidad_medida] NVARCHAR(30) NOT NULL,
    [costo_unitario] DECIMAL(18, 2) NOT NULL,
    [proveedor] INT NOT NULL,
    CONSTRAINT FK_Ingredientes_Proveedores FOREIGN KEY ([proveedor]) REFERENCES [Proveedores]([id])
);

CREATE TABLE [Inventarios] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [ingrediente] INT NOT NULL,
    [stock_actual] DECIMAL(18, 2) NOT NULL,
    [stock_minimo] DECIMAL(18, 2) NOT NULL,
    [fecha_actualizacion] DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Inventarios_Ingredientes FOREIGN KEY ([ingrediente]) REFERENCES [Ingredientes]([id])
);

CREATE TABLE [Recetas] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [producto] INT NOT NULL,
    [ingrediente] INT NOT NULL,
    [cantidad_requerida] DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_Recetas_Productos FOREIGN KEY ([producto]) REFERENCES [Productos]([id]),
    CONSTRAINT FK_Recetas_Ingredientes FOREIGN KEY ([ingrediente]) REFERENCES [Ingredientes]([id])
);

-- =========================================================
-- OPERACIÓN, PEDIDOS Y FACTURACIÓN
-- =========================================================

CREATE TABLE [Clientes] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [nombre] NVARCHAR(80) NOT NULL,
    [telefono] NVARCHAR(25) NOT NULL,
    [cedula] NVARCHAR(25) NOT NULL,
    [direccion] NVARCHAR(120) NULL
);

CREATE TABLE [Mesas] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [numero] INT NOT NULL,
    [capacidad] INT NOT NULL,
    [ubicacion] NVARCHAR(60) NOT NULL,
    [estado] NVARCHAR(30) NOT NULL
);

CREATE TABLE [Reservas] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [cliente] INT NOT NULL,
    [mesa] INT NOT NULL,
    [fecha_hora] DATETIME NOT NULL,
    [numero_personas] INT NOT NULL,
    [estado] NVARCHAR(30) NOT NULL,
    CONSTRAINT FK_Reservas_Clientes FOREIGN KEY ([cliente]) REFERENCES [Clientes]([id]),
    CONSTRAINT FK_Reservas_Mesas FOREIGN KEY ([mesa]) REFERENCES [Mesas]([id])
);

CREATE TABLE [Pedidos] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [cliente] INT NOT NULL,
    [mesa] INT NOT NULL,
    [mesero] INT NOT NULL,
    [fecha_hora] DATETIME NOT NULL DEFAULT GETDATE(),
    [numero_personas] INT NOT NULL,
    [estado] NVARCHAR(30) NOT NULL,
    CONSTRAINT FK_Pedidos_Clientes FOREIGN KEY ([cliente]) REFERENCES [Clientes]([id]),
    CONSTRAINT FK_Pedidos_Mesas FOREIGN KEY ([mesa]) REFERENCES [Mesas]([id]),
    CONSTRAINT FK_Pedidos_Meseros FOREIGN KEY ([mesero]) REFERENCES [Meseros]([id])
);

CREATE TABLE [Detalles_Pedidos] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [pedido] INT NOT NULL,
    [producto] INT NOT NULL,
    [cantidad] INT NOT NULL,
    [subtotal] DECIMAL(18, 2) NOT NULL,
    [comentarios] NVARCHAR(200) NULL,
    CONSTRAINT FK_Detalles_Pedidos FOREIGN KEY ([pedido]) REFERENCES [Pedidos]([id]),
    CONSTRAINT FK_Detalles_Productos FOREIGN KEY ([producto]) REFERENCES [Productos]([id])
);

CREATE TABLE [Facturas] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [pedido] INT NOT NULL,
    [numero] NVARCHAR(30) NOT NULL,
    [fecha_emision] DATETIME NOT NULL DEFAULT GETDATE(),
    [subtotal] DECIMAL(18, 2) NOT NULL,
    [total] DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_Facturas_Pedidos FOREIGN KEY ([pedido]) REFERENCES [Pedidos]([id])
);

CREATE TABLE [MetodosPagos] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [tipo] NVARCHAR(50) NOT NULL,
    [descripcion] NVARCHAR(150) NULL
);

CREATE TABLE [Pagos] (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
    [factura] INT NOT NULL,
    [metodoPago] INT NOT NULL,
    [monto] DECIMAL(18, 2) NOT NULL,
    [fecha] DATETIME NOT NULL DEFAULT GETDATE(),
    [referencia] NVARCHAR(80) NULL,
    [estado] NVARCHAR(30) NOT NULL,
    CONSTRAINT FK_Pagos_Facturas FOREIGN KEY ([factura]) REFERENCES [Facturas]([id]),
    CONSTRAINT FK_Pagos_MetodosPagos FOREIGN KEY ([metodoPago]) REFERENCES [MetodosPagos]([id])
);
GO

-- =========================================================
-- DATOS INICIALES (SIN VARIABLES ENTRE GO)
-- =========================================================

-- 1. Roles
INSERT INTO [Roles] ([nombre], [descripcion]) VALUES
(N'Administrador', N'Acceso total a todos los módulos del sistema'),
(N'Mesero',        N'Gestión de pedidos, comandas, mesas y reservas en sala'),
(N'Cajero',        N'Facturación, cobros y cierre de cuentas');

-- 2. Permisos por rol
INSERT INTO [Permisos] ([nombre], [rol])
SELECT p.[nombre], r.[id]
FROM [Roles] r
CROSS JOIN (VALUES
    (N'Pedidos'), (N'Detalles_Pedidos'), (N'Reservas'), (N'Mesas'), (N'Clientes'),
    (N'Categorias'), (N'Menus'), (N'Productos'), (N'Recetas'), (N'Ingredientes'),
    (N'Inventarios'), (N'Proveedores'), (N'Facturas'), (N'Pagos'), (N'MetodosPagos'),
    (N'Empleados'), (N'Meseros'), (N'Cocineros'), (N'Gerentes'), (N'Turnos'),
    (N'Usuarios'), (N'Roles'), (N'Permisos'), (N'Auditorias')
) AS p([nombre])
WHERE r.[nombre] = N'Administrador';

INSERT INTO [Permisos] ([nombre], [rol])
SELECT p.[nombre], r.[id]
FROM [Roles] r
CROSS JOIN (VALUES
    (N'Pedidos'), (N'Detalles_Pedidos'), (N'Reservas'), (N'Mesas'), (N'Clientes')
) AS p([nombre])
WHERE r.[nombre] = N'Mesero';

INSERT INTO [Permisos] ([nombre], [rol])
SELECT p.[nombre], r.[id]
FROM [Roles] r
CROSS JOIN (VALUES
    (N'Pedidos'), (N'Facturas'), (N'Pagos'), (N'MetodosPagos'), (N'Clientes')
) AS p([nombre])
WHERE r.[nombre] = N'Cajero';

-- 3. Usuarios de acceso
INSERT INTO [Usuarios] ([nombre], [correo], [contrasena], [estado], [rol]) VALUES
(N'Admin Principal', N'admin@restaurante.com',  N'1234', N'Activo', (SELECT TOP 1 [id] FROM [Roles] WHERE [nombre] = N'Administrador')),
(N'Juan Pérez',      N'juan@restaurante.com',   N'1234', N'Activo', (SELECT TOP 1 [id] FROM [Roles] WHERE [nombre] = N'Mesero')),
(N'Laura Gil',       N'laura@restaurante.com',  N'1234', N'Activo', (SELECT TOP 1 [id] FROM [Roles] WHERE [nombre] = N'Mesero')),
(N'Caja Central',    N'caja@restaurante.com',   N'1234', N'Activo', (SELECT TOP 1 [id] FROM [Roles] WHERE [nombre] = N'Cajero'));

-- 4. Empleados y especializaciones
INSERT INTO [Empleados] ([nombre], [cedula], [telefono], [cargo], [salario], [fecha_ingreso]) VALUES
(N'Carlos Mendoza', N'1010101010', N'3001112233', N'Gerente General', 4500000, GETDATE()),
(N'Juan Pérez',     N'1020202020', N'3002223344', N'Mesero',          1800000, GETDATE()),
(N'Laura Gil',      N'1030303030', N'3003334455', N'Mesero',          1800000, GETDATE()),
(N'Andrés Rojas',   N'1040404040', N'3004445566', N'Chef Ejecutivo',  3200000, GETDATE());

INSERT INTO [Gerentes] ([empleado], [area_responsable], [bono_anual]) VALUES
((SELECT TOP 1 [id] FROM [Empleados] WHERE [cedula] = N'1010101010'), N'Operación General', 5000000);

INSERT INTO [Meseros] ([empleado], [zona_asignada], [propina_acumulada]) VALUES
((SELECT TOP 1 [id] FROM [Empleados] WHERE [cedula] = N'1020202020'), N'Salón Principal', 0),
((SELECT TOP 1 [id] FROM [Empleados] WHERE [cedula] = N'1030303030'), N'Terraza',         0);

INSERT INTO [Cocineros] ([empleado], [especialidad], [turno_preferido]) VALUES
((SELECT TOP 1 [id] FROM [Empleados] WHERE [cedula] = N'1040404040'), N'Carnes y Parrilla', N'Tarde / Noche');

INSERT INTO [Turnos] ([empleado], [fecha], [hora_entrada], [hora_salida]) VALUES
((SELECT TOP 1 [id] FROM [Empleados] WHERE [cedula] = N'1020202020'), CAST(GETDATE() AS DATE), DATEADD(HOUR, 12, CAST(CAST(GETDATE() AS DATE) AS DATETIME)), DATEADD(HOUR, 20, CAST(CAST(GETDATE() AS DATE) AS DATETIME))),
((SELECT TOP 1 [id] FROM [Empleados] WHERE [cedula] = N'1030303030'), CAST(GETDATE() AS DATE), DATEADD(HOUR, 12, CAST(CAST(GETDATE() AS DATE) AS DATETIME)), DATEADD(HOUR, 20, CAST(CAST(GETDATE() AS DATE) AS DATETIME)));

-- 5. Categorías, Menús y Productos (con precio real en BD)
INSERT INTO [Categorias] ([nombre], [descripcion]) VALUES
(N'Entradas',      N'Platos ligeros para iniciar'),
(N'Platos Fuertes',N'Especialidades principales de la casa'),
(N'Bebidas',       N'Vinos, cocteles y bebidas frías'),
(N'Postres',       N'Repostería artesanal');

INSERT INTO [Menus] ([nombre], [vigencia_desde], [vigencia_hasta], [tipo]) VALUES
(N'Carta Principal', GETDATE(), DATEADD(YEAR, 1, GETDATE()),  N'Permanente'),
(N'Menú Ejecutivo',  GETDATE(), DATEADD(MONTH, 6, GETDATE()), N'Almuerzo');

INSERT INTO [Productos] ([nombre], [vigencia_desde], [vigencia_hasta], [tipo], [precio], [categoria], [menu]) VALUES
(N'Carpaccio de Res',   GETDATE(), DATEADD(YEAR, 1, GETDATE()), N'Entrada',      28000, (SELECT TOP 1 [id] FROM [Categorias] WHERE [nombre] = N'Entradas'),       (SELECT TOP 1 [id] FROM [Menus] WHERE [nombre] = N'Carta Principal')),
(N'Lomo al Vino Tinto', GETDATE(), DATEADD(YEAR, 1, GETDATE()), N'Plato Fuerte', 48000, (SELECT TOP 1 [id] FROM [Categorias] WHERE [nombre] = N'Platos Fuertes'), (SELECT TOP 1 [id] FROM [Menus] WHERE [nombre] = N'Carta Principal')),
(N'Risotto de Hongos',  GETDATE(), DATEADD(YEAR, 1, GETDATE()), N'Plato Fuerte', 42000, (SELECT TOP 1 [id] FROM [Categorias] WHERE [nombre] = N'Platos Fuertes'), (SELECT TOP 1 [id] FROM [Menus] WHERE [nombre] = N'Carta Principal')),
(N'Copa Cabernet',      GETDATE(), DATEADD(YEAR, 1, GETDATE()), N'Bebida',       18000, (SELECT TOP 1 [id] FROM [Categorias] WHERE [nombre] = N'Bebidas'),        (SELECT TOP 1 [id] FROM [Menus] WHERE [nombre] = N'Carta Principal')),
(N'Tiramisú Clásico',   GETDATE(), DATEADD(YEAR, 1, GETDATE()), N'Postre',       16000, (SELECT TOP 1 [id] FROM [Categorias] WHERE [nombre] = N'Postres'),        (SELECT TOP 1 [id] FROM [Menus] WHERE [nombre] = N'Carta Principal'));

-- 6. Proveedores, Ingredientes, Inventario y Recetas
INSERT INTO [Proveedores] ([nombre], [nit], [telefono], [correo]) VALUES
(N'Carnes Premium S.A.', N'900111222-1', N'6045551122', N'ventas@carnespremium.com'),
(N'Bodega del Sur',      N'900333444-2', N'6045553344', N'pedidos@bodegadelsur.com');

INSERT INTO [Ingredientes] ([nombre], [unidad_medida], [costo_unitario], [proveedor]) VALUES
(N'Lomo fino de res', N'Gramo',     45, (SELECT TOP 1 [id] FROM [Proveedores] WHERE [nit] = N'900111222-1')),
(N'Vino tinto reserva',N'Mililitro', 30, (SELECT TOP 1 [id] FROM [Proveedores] WHERE [nit] = N'900333444-2'));

INSERT INTO [Inventarios] ([ingrediente], [stock_actual], [stock_minimo], [fecha_actualizacion]) VALUES
((SELECT TOP 1 [id] FROM [Ingredientes] WHERE [nombre] = N'Lomo fino de res'),  15000, 3000, GETDATE()),
((SELECT TOP 1 [id] FROM [Ingredientes] WHERE [nombre] = N'Vino tinto reserva'), 8000, 1500, GETDATE());

INSERT INTO [Recetas] ([producto], [ingrediente], [cantidad_requerida]) VALUES
((SELECT TOP 1 [id] FROM [Productos] WHERE [nombre] = N'Lomo al Vino Tinto'), (SELECT TOP 1 [id] FROM [Ingredientes] WHERE [nombre] = N'Lomo fino de res'),  250),
((SELECT TOP 1 [id] FROM [Productos] WHERE [nombre] = N'Lomo al Vino Tinto'), (SELECT TOP 1 [id] FROM [Ingredientes] WHERE [nombre] = N'Vino tinto reserva'), 100);

-- 7. Clientes, Mesas y Métodos de Pago
INSERT INTO [Clientes] ([nombre], [telefono], [cedula], [direccion]) VALUES
(N'Consumidor Final', N'0000000000', N'222222222222', N'Local'),
(N'Camila Castro',    N'3109876543', N'1098765432',   N'Calle 10 # 43-20');

INSERT INTO [Mesas] ([numero], [capacidad], [ubicacion], [estado]) VALUES
(1, 4, N'Salón Principal', N'Disponible'),
(2, 2, N'Salón Principal', N'Disponible'),
(3, 6, N'Terraza',         N'Disponible'),
(4, 4, N'Terraza',         N'Disponible'),
(5, 8, N'VIP',             N'Disponible');

INSERT INTO [MetodosPagos] ([tipo], [descripcion]) VALUES
(N'Efectivo',        N'Pago en moneda local en caja'),
(N'Tarjeta Crédito', N'Datáfono Visa / Mastercard'),
(N'Transferencia',   N'Bancolombia / Nequi / Daviplata');
GO