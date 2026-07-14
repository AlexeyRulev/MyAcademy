-- Заполнение клиентов
INSERT INTO Customers (FullName, Phone) VALUES 
(N'Алексей Рулев', N'+79991112233'),
(N'Иван Иванов', N'+79994445566'),
(N'Дмитрий Петров', N'+79997778899');

-- Заполнение принтеров
INSERT INTO Printers (ModelName, IsActive) VALUES 
(N'Bambu Lab P2S 01', 1),
(N'Bambu Lab P2S 02', 1),
(N'Ender 3 Pro', 0);

-- Заполнение материалов
INSERT INTO Materials (MaterialType, PricePerGram, Color) VALUES 
(N'PLA', 4.00, N'Черный'),
(N'PETG', 4.00, N'Прозрачный'),
(N'ABS', 5.50, N'Красный');

-- Заполнение моделей (Объявляем переменные для хранения сгенерированных GUID)
DECLARE @Model1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Model2 UNIQUEIDENTIFIER = NEWID();
DECLARE @Model3 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Models (ModelID, ModelName, FileSizeBytes) VALUES 
(@Model1, N'Lighthouse_V2.stl', 1542000),
(@Model2, N'Ship_Hull.stl', 4850000),
(@Model3, N'Gears_Set.stl', 820000);

-- Заполнение заказов
INSERT INTO Orders (CustomerID, TotalPrice, IsPaid) VALUES 
(1, 1250.00, 1),
(2, 450.00, 0),
(1, 2100.00, 0);

-- Заполнение состава заказов
INSERT INTO OrderDetails (OrderID, ModelID, MaterialID, PrinterID, WeightGrams, Quantity) VALUES 
(1, @Model1, 1, 1, 150.00, 1),
(1, @Model3, 3, 2, 50.00, 2),
(2, @Model3, 1, 1, 45.00, 1),
(3, @Model2, 2, 2, 400.00, 1);
GO