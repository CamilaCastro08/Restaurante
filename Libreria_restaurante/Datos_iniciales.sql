USE db_Restaurante;
GO

IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [nombre] = N'Administrador')
BEGIN
	INSERT INTO [Roles] ([nombre], [descripcion], [activo])
	VALUES (N'Administrador', N'Acceso total al sistema', 1);

	DECLARE @idRol INT = SCOPE_IDENTITY();

	INSERT INTO [Permisos] ([nombre], [descripcion], [rol])
	VALUES
	(N'Categorias', N'Acceso a Categorias', @idRol),
	(N'Clientes', N'Acceso a Clientes', @idRol),
	(N'Cocineros', N'Acceso a Cocineros', @idRol),
	(N'Detalles_Pedidos', N'Acceso a Detalles_Pedidos', @idRol),
	(N'Empleados', N'Acceso a Empleados', @idRol),
	(N'Facturas', N'Acceso a Facturas', @idRol),
	(N'Gerentes', N'Acceso a Gerentes', @idRol),
	(N'Ingredientes', N'Acceso a Ingredientes', @idRol),
	(N'Inventarios', N'Acceso a Inventarios', @idRol),
	(N'Menus', N'Acceso a Menus', @idRol),
	(N'Mesas', N'Acceso a Mesas', @idRol),
	(N'Meseros', N'Acceso a Meseros', @idRol),
	(N'MetodosPagos', N'Acceso a MetodosPagos', @idRol),
	(N'Pagos', N'Acceso a Pagos', @idRol),
	(N'Pedidos', N'Acceso a Pedidos', @idRol),
	(N'Productos', N'Acceso a Productos', @idRol),
	(N'Proveedores', N'Acceso a Proveedores', @idRol),
	(N'Recetas', N'Acceso a Recetas', @idRol),
	(N'Reservas', N'Acceso a Reservas', @idRol),
	(N'Turnos', N'Acceso a Turnos', @idRol),
	(N'Roles', N'Acceso a Roles', @idRol),
	(N'Usuarios', N'Acceso a Usuarios', @idRol),
	(N'Permisos', N'Acceso a Permisos', @idRol),
	(N'Auditorias', N'Acceso a Auditorias', @idRol);

	INSERT INTO [Usuarios] ([nombreUsuario], [correo], [contraseña], [activo], [fechaCreacion], [rol])
	VALUES (N'admin', N'admin@restaurante.com', N'$2a$11$ehkKpEM0JqqtXTwcRU8Ka.bDHkAEQA.7CnehinNiijcGBbAd86ypS', 1, GETDATE(), @idRol);
END
GO
