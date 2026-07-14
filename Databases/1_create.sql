-- 1. Создание базы данных
CREATE DATABASE Studio3DOrders;
GO
USE Studio3DOrders;
GO

-- 2. Таблица "Клиенты"
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,                   
    Phone NVARCHAR(20) NOT NULL,                       
    RegistrationDate DATETIME NOT NULL DEFAULT GETDATE() -- Дата и время
);

-- 3. Таблица "3D-Принтеры"
CREATE TABLE Printers (
    PrinterID INT IDENTITY(1,1) PRIMARY KEY,
    ModelName NVARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1                    -- Булевый тип
);

-- 4. Таблица "Материалы (Пластик)"
CREATE TABLE Materials (
    MaterialID INT IDENTITY(1,1) PRIMARY KEY,
    MaterialType NVARCHAR(20) NOT NULL,                -- Например: PLA, ABS, PETG
    PricePerGram DECIMAL(10,2) NOT NULL,               -- Десятичное число
    Color NVARCHAR(30) NOT NULL
);

-- 5. Таблица "3D-Модели" (Использует GUID как первичный ключ)
CREATE TABLE Models (
    ModelID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), -- GUID
    ModelName NVARCHAR(100) NOT NULL,
    FileSizeBytes BIGINT NOT NULL
);

-- 6. Таблица "Заказы" (Связь 1:N с таблицей Клиентов)
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,                           --Внешний ключ (1:N)
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalPrice DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    IsPaid BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- 7. Состав заказа (Связующая таблица для реализации связи N:N между Заказами и Моделями)
CREATE TABLE OrderDetails (
    OrderID INT NOT NULL,
    ModelID UNIQUEIDENTIFIER NOT NULL,
    MaterialID INT NOT NULL,
    PrinterID INT NOT NULL,
    WeightGrams DECIMAL(10,2) NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    CONSTRAINT PK_OrderDetails PRIMARY KEY (OrderID, ModelID), -- Составной ПК
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderID) REFERENCES Orders(OrderID) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetails_Models FOREIGN KEY (ModelID) REFERENCES Models(ModelID),
    CONSTRAINT FK_OrderDetails_Materials FOREIGN KEY (MaterialID) REFERENCES Materials(MaterialID),
    CONSTRAINT FK_OrderDetails_Printers FOREIGN KEY (PrinterID) REFERENCES Printers(PrinterID)
);
GO