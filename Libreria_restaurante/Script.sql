CREATE DATABASE db_Restaurante;
GO

USE db_Restaurante;
GO

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
	[nombre] NVARCHAR(20) NOT NULL,
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
	[nombre] NVARCHAR(20) NOT NULL,
	[telefono] NVARCHAR(20) NOT NULL,
	[direccion] NVARCHAR(50) NOT NULL,
	[cedula] NVARCHAR(20) NOT NULL,
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