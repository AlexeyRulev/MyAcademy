USE Studio3DOrders;
GO


-- 1. ВЫБОРКА С ФИЛЬТРАЦИЕЙ И СОРТИРОВКОЙ
-- Выбираем только активные 3D-принтеры, отсортированные по названию

PRINT '--- ПУНКТ 1: Активные принтеры (фильтрация IsActive = 1) ---';

SELECT PrinterID, ModelName, IsActive 
FROM Printers
WHERE IsActive = 1
ORDER BY ModelName ASC;


-- 2. ИЗМЕНЕНИЕ И УДАЛЕНИЕ ДАННЫХ (с демонстрацией результатов)

PRINT '--- ПУНКТ 2: Изменение цены пластика ABS и удаление неактивных принтеров ---';

-- Покажем состояние материалов ДО изменения
SELECT N'ДО ИЗМЕНЕНИЯ' AS [Статус], MaterialType, PricePerGram, Color 
FROM Materials WHERE MaterialType = N'ABS';

-- Выполняем UPDATE
UPDATE Materials
SET PricePerGram = 6.00
WHERE MaterialType = N'ABS';

-- Показываем материалы ПОСЛЕ изменения (цена ABS должна стать 6.00)
SELECT N'ПОСЛЕ ИЗМЕНЕНИЯ' AS [Статус], MaterialType, PricePerGram, Color 
FROM Materials WHERE MaterialType = N'ABS';


-- Покажем состояние принтеров ДО удаления
SELECT N'ДО УДАЛЕНИЯ' AS [Статус], PrinterID, ModelName, IsActive 
FROM Printers;

-- Выполняем DELETE (удаляем Ender 3 Pro, у которого IsActive = 0)
DELETE FROM Printers
WHERE IsActive = 0;

-- Показываем принтеры ПОСЛЕ удаления (должны остаться только два Bambu Lab)
SELECT N'ПОСЛЕ УДАЛЕНИЯ' AS [Статус], PrinterID, ModelName, IsActive 
FROM Printers;



-- 3. ВЫБОРКА С ГРУППИРОВКОЙ
-- Выводим ID клиентов и общую сумму их заказов (только тех, кто потратил > 500)

PRINT '--- ПУНКТ 3: Группировка трат клиентов (фильтр HAVING > 500.00) ---';

SELECT CustomerID, SUM(TotalPrice) AS TotalSpent
FROM Orders
GROUP BY CustomerID
HAVING SUM(TotalPrice) > 500.00;



-- 4. КОМПЛЕКСНАЯ ВЫБОРКА ИЗ НЕСКОЛЬКИХ ТАБЛИЦ (JOIN)

PRINT '--- ПУНКТ 4.А: INNER JOIN (Детальный состав оформленных заказов) ---';

SELECT 
    O.OrderID AS [Номер заказа], 
    C.FullName AS [ФИО Клиента], 
    M.ModelName AS [3D-Модель], 
    MAT.MaterialType AS [Пластик], 
    OD.WeightGrams AS [Вес детали (г)]
FROM OrderDetails OD
INNER JOIN Orders O ON OD.OrderID = O.OrderID
INNER JOIN Customers C ON O.CustomerID = C.CustomerID
INNER JOIN Models M ON OD.ModelID = M.ModelID
INNER JOIN Materials MAT ON OD.MaterialID = MAT.MaterialID;


PRINT '--- ПУНКТ 4.Б: LEFT JOIN (Все клиенты, даже если у них еще нет заказов) ---';
-- Обрати внимание: у Дмитрия Петрова нет заказов, но благодаря LEFT JOIN он всё равно выведется (со значениями NULL в полях заказа)

SELECT 
    C.FullName AS [ФИО Клиента], 
    O.OrderID AS [Номер заказа], 
    O.OrderDate AS [Дата], 
    O.TotalPrice AS [Сумма]
FROM Customers C
LEFT JOIN Orders O ON C.CustomerID = O.CustomerID;


PRINT '--- ПУНКТ 4.В: RIGHT JOIN (Все материалы, включая неиспользованные в заказах) ---';
-- Мы использовали только PLA и PETG. ABS остался не у дел, но благодаря RIGHT JOIN он красиво отобразится в таблице с NULL в деталях заказа

SELECT 
    M.MaterialType AS [Тип пластика], 
    M.Color AS [Цвет], 
    OD.OrderID AS [Номер заказа в работе], 
    OD.Quantity AS [Количество]
FROM OrderDetails OD
RIGHT JOIN Materials M ON OD.MaterialID = M.MaterialID;