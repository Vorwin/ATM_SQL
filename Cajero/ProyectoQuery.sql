CREATE DATABASE clientesATM; 
USE clientesATM;

Create table inforClientesATM(
	codigo int primary key,
	usuario varchar(30),
	contra varchar(30),
	nombre varchar (30),
	apellido varchar(30),
	fechar varchar(15),
	DPI BIGINT, 
	numero BIGINT, 
	correo varchar(60),
	banco varchar (30),
	monto BIGINT,
	cuentaActiva bit not null
)

Select * from inforClientesATM
Delete from inforClientesATM where codigo = 4040; 

Insert into inforClientesATM(codigo, usuario, contra, nombre, apellido, fechar, DPI, numero, correo, banco, monto, cuentaActiva)
	Values(1, 'Vorwin', '1234', 'Andres', 'Solorzano', '17/08/2003', 1234567890, 12345678, 'andres@gmail.com' , 'Industrial', 1000, 1);

Insert into inforClientesATM(codigo, usuario, contra, nombre, apellido, fechar, DPI, numero, correo, banco, monto, cuentaActiva)
	Values(2, 'Alex', '02202', 'Angelo', 'Martinez', '02/02/2004', 0987654321, 87654321, 'angelo@gmail.com' , 'Industrial', 1000, 1);

Insert into inforClientesATM(codigo, usuario, contra, nombre, apellido, fechar, DPI, numero, correo, banco, monto, cuentaActiva)
	Values(3, 'PruebaUser', '1234', 'Jhon', 'Doe', '01/01/2001', 1212121212, 49494949, 'prueba@gmail.com' , 'Banrural', 1000, 1);

--Drop table inforClientesATM


