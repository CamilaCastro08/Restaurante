USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'db_Restaurante')
BEGIN
    ALTER DATABASE db_Restaurante SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE db_Restaurante;
END
GO

CREATE DATABASE db_Restaurante;
GO

USE db_Restaurante;
GO


CREATE TABLE [Categorias]
(
	[id] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY,
	[nombre] NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE [MetodosPagos]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[tipo] NVARCHAR(50) NOT NULL
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
	[direccion] NVARCHAR(100) NOT NULL
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
	[direccion] NVARCHAR(100) NOT NULL,
	[salario] DECIMAL(18, 2) NOT NULL,
	[fecha_contratacion] DATETIME NOT NULL,
	[cargo] NVARCHAR(30) NOT NULL
);
GO

CREATE TABLE [Menus]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombre] NVARCHAR(50) NOT NULL,
	[vigencia_desde] DATETIME NOT NULL,
	[vigencia_hasta] DATETIME NOT NULL,
	[tipo] NVARCHAR(30) NOT NULL
);
GO

CREATE TABLE [Productos]
(
	[id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[nombre] NVARCHAR(50) NOT NULL,
	[vigencia_desde] DATETIME NOT NULL,
	[vigencia_hasta] DATETIME NOT NULL,
	[tipo] NVARCHAR(30) NOT NULL,
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
--Creación de auditorias
CREATE TABLE CategoriasAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE ClientesAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE CocinerosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Detalles_PedidosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE EmpleadosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE FacturasAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE GerentesAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE IngredientesAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE InventariosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE MenusAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE MesasAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE MeserosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE MetodosPagosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE PagosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE PedidosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE PermisosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE ProductosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE ProveedoresAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE RecetasAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE ReservasAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE RolesAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO
CREATE TABLE TurnosAuditorias (
    [IdAuditoria] INT IDENTITY(1,1) PRIMARY KEY,
    [IdPersona] INT NOT NULL,
    [Accion] VARCHAR(20) NOT NULL,
    [Fecha] DATETIME NOT NULL DEFAULT GETDATE()
);
GO


-- =========================================================================
-- 2. INSERTS DE DATOS INICIALES (CONTRASEÑAS SIMPLES SIN BCRYPT)
-- =========================================================================

-- Roles
INSERT INTO [Roles] ([nombre], [descripcion], [activo])
VALUES 
(N'Administrador', N'Acceso total al sistema', 1),
(N'Mesero', N'Acceso a salon y pedidos', 1);
GO

-- Permisos para Administrador (Rol = 1)
INSERT INTO [Permisos] ([nombre], [descripcion], [rol])
VALUES
(N'Categorias', N'Acceso a Categorias', 1),
(N'Clientes', N'Acceso a Clientes', 1),
(N'Cocineros', N'Acceso a Cocineros', 1),
(N'Detalles_Pedidos', N'Acceso a Detalles_Pedidos', 1),
(N'Empleados', N'Acceso a Empleados', 1),
(N'Facturas', N'Acceso a Facturas', 1),
(N'Gerentes', N'Acceso a Gerentes', 1),
(N'Ingredientes', N'Acceso a Ingredientes', 1),
(N'Inventarios', N'Acceso a Inventarios', 1),
(N'Menus', N'Acceso a Menus', 1),
(N'Mesas', N'Acceso a Mesas', 1),
(N'Meseros', N'Acceso a Meseros', 1),
(N'MetodosPagos', N'Acceso a MetodosPagos', 1),
(N'Pagos', N'Acceso a Pagos', 1),
(N'Pedidos', N'Acceso a Pedidos', 1),
(N'Productos', N'Acceso a Productos', 1),
(N'Proveedores', N'Acceso a Proveedores', 1),
(N'Recetas', N'Acceso a Recetas', 1),
(N'Reservas', N'Acceso a Reservas', 1),
(N'Turnos', N'Acceso a Turnos', 1),
(N'Roles', N'Acceso a Roles', 1),
(N'Usuarios', N'Acceso a Usuarios', 1),
(N'Permisos', N'Acceso a Permisos', 1),
GO

-- Permisos limitados para Mesero (Rol = 2)
INSERT INTO [Permisos] ([nombre], [descripcion], [rol])
VALUES
(N'Mesas', N'Acceso a Mesas', 2),
(N'Reservas', N'Acceso a Reservas', 2),
(N'Clientes', N'Acceso a Clientes', 2),
(N'Pedidos', N'Acceso a Pedidos', 2),
(N'Detalles_Pedidos', N'Acceso a Detalles_Pedidos', 2);
GO

-- Usuarios con contraseña simple '1234'
INSERT INTO [Usuarios] ([nombreUsuario], [correo], [contraseña], [activo], [fechaCreacion], [rol])
VALUES 
(N'admin', N'admin@restaurante.com', N'1234', 1, GETDATE(), 1),
(N'mesero1', N'mesero@restaurante.com', N'1234', 1, GETDATE(), 2);
GO

-- Categorias
INSERT INTO [Categorias] ([nombre])
VALUES (N'Entradas'), (N'Platos Fuertes'), (N'Bebidas'), (N'Postres');
GO

-- MetodosPagos
INSERT INTO [MetodosPagos] ([tipo])
VALUES (N'Efectivo'), (N'Tarjeta de Credito'), (N'Transferencia Nequi/Bancolombia');
GO

-- Proveedores
INSERT INTO [Proveedores] ([nombre], [contacto], [direccion], [telefono])
VALUES 
(N'Carnes Premium Antioquia', N'Carlos Restrepo', N'Cra 50 # 32-10, Medellin', N'3001112233'),
(N'Fruver La Cosecha', N'Marta Gomez', N'Cl 44 # 65-20, Bello', N'3104445566');
GO

-- Clientes
INSERT INTO [Clientes] ([nombre], [telefono], [cedula], [direccion])
VALUES 
(N'Andres Felipe Lopez', N'3125558899', N'1035888999', N'Cl 52 # 48-15, Bello'),
(N'Laura Sofia Martinez', N'3207774411', N'1017222333', N'Cra 43A # 10-50, Medellin');
GO

-- Mesas
INSERT INTO [Mesas] ([numero], [capacidad], [estado], [ubicacion])
VALUES 
(1, 4, N'Disponible', N'Salon Principal'),
(2, 2, N'Ocupada', N'Terraza'),
(3, 6, N'Reservada', N'Zona VIP');
GO

-- Empleados
INSERT INTO [Empleados] ([nombre], [telefono], [direccion], [salario], [fecha_contratacion], [cargo])
VALUES 
(N'Juan Camilo Perez', N'3012223344', N'Bello Centro', 1800000.00, '2025-02-10', N'Mesero'),
(N'Sofia Morales', N'3023334455', N'Envigado', 2500000.00, '2024-08-15', N'Cocinero'),
(N'Ricardo Valencia', N'3045556677', N'Laureles', 3800000.00, '2024-01-20', N'Gerente');
GO

-- Menus
INSERT INTO [Menus] ([nombre], [vigencia_desde], [vigencia_hasta], [tipo])
VALUES 
(N'Carta General 2026', '2026-01-01', '2026-12-31', N'Permanente'),
(N'Menu Ejecutivo', '2026-01-01', '2026-12-31', N'Almuerzo');
GO

-- Productos
INSERT INTO [Productos] ([nombre], [vigencia_desde], [vigencia_hasta], [tipo], [categoria], [menu])
VALUES 
(N'Churrasco Argentino 350g', '2026-01-01', '2026-12-31', N'Plato', 2, 1),
(N'Limonada de Coco', '2026-01-01', '2026-12-31', N'Bebida', 3, 1),
(N'Empanadas Criollas x4', '2026-01-01', '2026-12-31', N'Entrada', 1, 1);
GO

-- Recetas
INSERT INTO [Recetas] ([producto], [tiempo_preparacion], [instrucciones], [porciones], [dificultad])
VALUES 
(1, '2026-01-01 00:25:00', N'Sellar corte a fuego alto 4 min por lado, reposar y servir con chimichurri.', 1, N'Media'),
(2, '2026-01-01 00:05:00', N'Licuar leche de coco, zumo de limon, azucar y hielo frappe.', 1, N'Facil');
GO

-- Ingredientes
INSERT INTO [Ingredientes] ([nombre], [unidad_medida], [costo_unitario], [proveedor], [receta])
VALUES 
(N'Lomo Ancho de Res', N'Kilogramo', 34000.00, 1, 1),
(N'Limon Tahití', N'Kilogramo', 4500.00, 2, 2);
GO

-- Turnos
INSERT INTO [Turnos] ([empleado], [fecha], [hora_entrada], [hora_salida])
VALUES 
(1, GETDATE(), DATEADD(HOUR, -4, GETDATE()), DATEADD(HOUR, 4, GETDATE())),
(2, GETDATE(), DATEADD(HOUR, -5, GETDATE()), DATEADD(HOUR, 3, GETDATE()));
GO

-- Meseros
INSERT INTO [Meseros] ([empleado], [zona_asignada], [num_mesas_asignadas], [calificacion_promedio])
VALUES (1, N'Salon Principal y Terraza', 5, 4.85);
GO

-- Gerentes
INSERT INTO [Gerentes] ([empleado], [nivel_autorizacion], [bono_desempeno], [area_responsable])
VALUES (3, N'Total', 650000.00, N'Operaciones y Caja');
GO

-- Cocineros
INSERT INTO [Cocineros] ([empleado], [especialidad], [anios_experiencia], [estacion_asignada])
VALUES (2, N'Parrilla y Carnes', 6, N'Estacion Caliente 1');
GO

-- Inventarios
INSERT INTO [Inventarios] ([ingrediente], [stock_actual], [stock_minimo], [fecha_actualizacion])
VALUES 
(1, 25.50, 10.00, GETDATE()),
(2, 14.00, 5.00, GETDATE());
GO

-- Reservas
INSERT INTO [Reservas] ([cliente], [mesa], [fecha_hora], [numero_personas], [estado])
VALUES (2, 3, DATEADD(HOUR, 3, GETDATE()), 4, N'Confirmada');
GO

-- Pedidos
INSERT INTO [Pedidos] ([cliente], [mesa], [mesero], [fecha_hora], [numero_personas], [estado])
VALUES (1, 2, 1, GETDATE(), 2, N'Entregado');
GO

-- Detalles_Pedidos
INSERT INTO [Detalles_Pedidos] ([pedido], [producto], [cantidad], [subtotal], [comentarios])
VALUES 
(1, 1, 2, 96000.00, N'Termino medio'),
(1, 2, 2, 24000.00, N'Sin azucar');
GO

-- Facturas
INSERT INTO [Facturas] ([pedido], [numero], [fecha_emision], [subtotal], [total])
VALUES (1, N'FAC-0001', GETDATE(), 120000.00, 129600.00);
GO

-- Pagos
INSERT INTO [Pagos] ([factura], [metodoPago], [monto], [fecha], [referencia], [estado])
VALUES (1, 3, 129600.00, GETDATE(), N'NEQUI-998877', N'Aprobado');
GO

