IF DB_ID(N'HotelDB') IS NULL CREATE DATABASE HotelDB;
GO
USE HotelDB;
GO
CREATE TABLE TipoHabitacion (
    IdTipoHabitacion INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    PrecioNoche DECIMAL(10,2) NOT NULL CHECK (PrecioNoche > 0)
);
CREATE TABLE Habitacion (
    IdHabitacion INT IDENTITY(1,1) PRIMARY KEY,
    Numero NVARCHAR(10) NOT NULL UNIQUE,
    IdTipoHabitacion INT NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Habitacion_Activo DEFAULT 1,
    CONSTRAINT FK_Habitacion_Tipo FOREIGN KEY (IdTipoHabitacion) REFERENCES TipoHabitacion(IdTipoHabitacion)
);
CREATE TABLE Huesped (
    IdHuesped INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto NVARCHAR(150) NOT NULL,
    Documento NVARCHAR(20) NOT NULL UNIQUE,
    Email NVARCHAR(120) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Huesped_Activo DEFAULT 1
);
CREATE TABLE Reserva (
    IdReserva INT IDENTITY(1,1) PRIMARY KEY,
    IdHuesped INT NOT NULL,
    IdHabitacion INT NOT NULL,
    FechaIngreso DATE NOT NULL,
    FechaSalida DATE NOT NULL,
    Estado NVARCHAR(15) NOT NULL CHECK (Estado IN (N'Pendiente', N'Confirmada', N'Cancelada', N'Finalizada')),
    CONSTRAINT CK_Reserva_Fechas CHECK (FechaSalida > FechaIngreso),
    CONSTRAINT FK_Reserva_Huesped FOREIGN KEY (IdHuesped) REFERENCES Huesped(IdHuesped),
    CONSTRAINT FK_Reserva_Habitacion FOREIGN KEY (IdHabitacion) REFERENCES Habitacion(IdHabitacion)
);
INSERT INTO TipoHabitacion (Nombre, PrecioNoche) VALUES (N'Estándar', 150.00), (N'Doble', 220.00), (N'Suite', 380.00);
INSERT INTO Habitacion (Numero, IdTipoHabitacion) VALUES (N'101', 1), (N'102', 1), (N'201', 2), (N'202', 2), (N'301', 3);
INSERT INTO Huesped (NombreCompleto, Documento, Email) VALUES (N'Ana Torres', N'70000001', N'ana@example.com'), (N'Luis Díaz', N'70000002', N'luis@example.com');
GO
