CREATE TABLE Reservas (
    IdReserva INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(100) NOT NULL,
    FechaReserva DATE NOT NULL,
    HoraReserva TIME NOT NULL,
    Lugar NVARCHAR(50) NOT NULL
);
