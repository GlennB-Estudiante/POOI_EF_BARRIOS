IF DB_ID('ExamenFinal') IS NULL CREATE DATABASE ExamenFinal;
GO
USE ExamenFinal;
GO
IF OBJECT_ID('tb_Consumidor') IS NOT NULL DROP TABLE tb_Consumidor;
IF OBJECT_ID('tb_paises') IS NOT NULL DROP TABLE tb_paises;
GO
CREATE TABLE tb_paises(idpais char(3) PRIMARY KEY, nompais varchar(60) NOT NULL);
INSERT tb_paises VALUES('PER','Perú'),('ARG','Argentina'),('CHL','Chile'),('COL','Colombia'),('ECU','Ecuador');
CREATE TABLE tb_Consumidor(
 dnicont char(8) PRIMARY KEY, nomcont varchar(40) NOT NULL, apecont varchar(40) NOT NULL,
 emailcont varchar(250) UNIQUE, idpais char(3) REFERENCES tb_paises(idpais));
GO
CREATE OR ALTER PROC usp_listar_paises AS SELECT idpais, nompais FROM tb_paises ORDER BY nompais;
GO
CREATE OR ALTER PROC usp_agrega_consumidor
 @dnicont varchar(20), @nomcont varchar(40), @apecont varchar(40), @emailcont varchar(250), @idpais varchar(3),
 @mensaje varchar(200) OUTPUT
AS BEGIN
 SET NOCOUNT ON;
 IF @dnicont IS NULL OR LEN(@dnicont)<>8 OR @dnicont LIKE '%[^0-9]%' BEGIN SET @mensaje='Error: el DNI debe ser numérico de 8 dígitos'; RETURN; END
 IF LTRIM(RTRIM(ISNULL(@nomcont,'')))='' BEGIN SET @mensaje='Error: el nombre no debe estar vacío'; RETURN; END
 IF LTRIM(RTRIM(ISNULL(@apecont,'')))='' BEGIN SET @mensaje='Error: el apellido no debe estar vacío'; RETURN; END
 IF @idpais IS NULL OR LTRIM(RTRIM(@idpais))='' OR ISNUMERIC(@idpais)=1 BEGIN SET @mensaje='Error: el país debe ser un texto válido'; RETURN; END
 IF NOT EXISTS(SELECT 1 FROM tb_paises WHERE idpais=@idpais) BEGIN SET @mensaje='Error: el país no existe'; RETURN; END
 IF EXISTS(SELECT 1 FROM tb_Consumidor WHERE dnicont=@dnicont) BEGIN SET @mensaje='Error: ya existe un consumidor con ese DNI'; RETURN; END
 IF @emailcont IS NOT NULL AND EXISTS(SELECT 1 FROM tb_Consumidor WHERE emailcont=@emailcont) BEGIN SET @mensaje='Error: el email ya está registrado'; RETURN; END
 INSERT tb_Consumidor VALUES(@dnicont,@nomcont,@apecont,@emailcont,@idpais);
 SET @mensaje='Consumidor registrado correctamente';
END
GO
CREATE OR ALTER PROC usp_eliminar_consumidor @dnicont char(8), @mensaje varchar(200) OUTPUT
AS BEGIN
 SET NOCOUNT ON;
 IF NOT EXISTS(SELECT 1 FROM tb_Consumidor WHERE dnicont=@dnicont) BEGIN SET @mensaje='Error: el DNI '+ISNULL(@dnicont,'')+' no existe'; RETURN; END
 DELETE tb_Consumidor WHERE dnicont=@dnicont;
 SET @mensaje='Consumidor eliminado correctamente';
END
GO
CREATE OR ALTER PROC usp_listar_consumidor AS
 SELECT dnicont,nomcont,apecont,emailcont,idpais FROM tb_Consumidor ORDER BY apecont,nomcont;
GO
CREATE OR ALTER PROC usp_buscar_consumidor @dnicont char(8) AS
 SELECT dnicont,nomcont,apecont,emailcont,idpais FROM tb_Consumidor WHERE dnicont=@dnicont;
GO
CREATE OR ALTER PROC usp_actualizar_consumidor
 @dnicont char(8), @nomcont varchar(40), @apecont varchar(40), @emailcont varchar(250), @idpais varchar(3), @mensaje varchar(200) OUTPUT
AS BEGIN
 SET NOCOUNT ON;
 IF NOT EXISTS(SELECT 1 FROM tb_Consumidor WHERE dnicont=@dnicont) BEGIN SET @mensaje='Error: el consumidor no existe'; RETURN; END
 IF LTRIM(RTRIM(ISNULL(@nomcont,'')))='' OR LTRIM(RTRIM(ISNULL(@apecont,'')))='' BEGIN SET @mensaje='Error: nombre y apellido no deben estar vacíos'; RETURN; END
 IF NOT EXISTS(SELECT 1 FROM tb_paises WHERE idpais=@idpais) BEGIN SET @mensaje='Error: el país no existe'; RETURN; END
 IF @emailcont IS NOT NULL AND EXISTS(SELECT 1 FROM tb_Consumidor WHERE emailcont=@emailcont AND dnicont<>@dnicont) BEGIN SET @mensaje='Error: el email ya está registrado'; RETURN; END
 UPDATE tb_Consumidor SET nomcont=@nomcont,apecont=@apecont,emailcont=@emailcont,idpais=@idpais WHERE dnicont=@dnicont;
 SET @mensaje='Consumidor actualizado correctamente';
END
GO
-- ===== Pregunta 2: base Northwind =====
USE Northwind;
GO
CREATE OR ALTER PROC usp_empleados_pedidos_anio @anio int AS
 SELECT e.EmployeeID AS idempleado, e.FirstName + ' ' + e.LastName AS nombre, COUNT(o.OrderID) AS cantidad
 FROM Employees e INNER JOIN Orders o ON o.EmployeeID = e.EmployeeID
 WHERE YEAR(o.OrderDate) = @anio
 GROUP BY e.EmployeeID, e.FirstName, e.LastName
 ORDER BY cantidad DESC;
GO
