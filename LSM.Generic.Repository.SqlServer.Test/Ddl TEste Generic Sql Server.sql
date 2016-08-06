CREATE TABLE Pessoa
(
IdPessoa INT NOT NULL IDENTITY(1,1),
Nome varchar(100) NOT NULL,
SobreNome varchar(100) NOT NULL,
DataNascimento DATETIME NOT NULL,
Ativo BIT NOT NULL
);
GO
CREATE PROCEDURE AddPessoa
@Nome varchar(100),
@SobreNome varchar(100) ,
@DataNascimento DATETIME 

AS


INSERT INTO Pessoa
VALUES(@Nome,@SobreNome,@DataNascimento, 1);
GO
CREATE PROCEDURE UpdatePessoa
@IdPessoa INT,
@Nome varchar(100),
@SobreNome varchar(100) ,
@DataNascimento DATETIME 

AS

UPDATE Pessoa
SET Nome = @Nome,
SobreNome = @SobreNome,
DataNascimento = @DataNascimento
WHERE IdPessoa = @IdPessoa
GO
CREATE PROCEDURE RemovePessoa
@IdPessoa INT
AS

UPDATE Pessoa
SET Ativo = 0
WHERE IdPessoa = @IdPessoa
GO
CREATE PROCEDURE GetPessoaById
@IdPessoa INT
AS

SELECT * FROM Pessoa
WHERE IdPessoa = @IdPessoa
AND Ativo = 1
GO
ALTER PROCEDURE GetAllPessoa
AS

SELECT * FROM Pessoa
WHERE Ativo = 1
GO