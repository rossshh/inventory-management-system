-- =============================================
-- Database Creation Queries & Stored Procedures
-- For Inventory Management System
-- =============================================

-- Database Creation (if not running through EF Core)
-- CREATE DATABASE InventoryDB;
-- GO
-- USE InventoryDB;
-- GO

-- =============================================
-- TABLES
-- =============================================

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(MAX) NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role INT NOT NULL -- 0: Admin, 1: Manager, 2: Staff
);

CREATE TABLE Suppliers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ContactPerson NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(250)
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL,
    SupplierId INT NOT NULL FOREIGN KEY REFERENCES Suppliers(Id)
);

CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    OrderDate DATETIME2 NOT NULL
);

CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    OrderId INT NOT NULL FOREIGN KEY REFERENCES Orders(Id)
);

GO


IF OBJECT_ID('GetSupplierOrderReport', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE GetSupplierOrderReport;
END
GO 

CREATE PROCEDURE GetSupplierOrderReport
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.Name AS Supplier,
        ISNULL(SUM(oi.Quantity), 0) AS TotalQuantity,
        ISNULL(SUM(oi.Quantity * oi.UnitPrice), 0) AS TotalValue
    FROM Suppliers s
    LEFT JOIN Products p ON s.Id = p.SupplierId
    LEFT JOIN OrderItems oi ON p.Id = oi.ProductId
    GROUP BY s.Name;
END
GO


-- 2. Get Low Stock Products
-- Requirement: Returns products that have fallen below a specified quantity threshold.
IF OBJECT_ID('GetLowStockProducts', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE GetLowStockProducts;
END
GO

CREATE PROCEDURE GetLowStockProducts
    @Threshold INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Id,
        p.Name,
        p.Description,
        p.Price,
        p.Quantity,
        p.SupplierId
    FROM Products p
    WHERE p.Quantity < @Threshold;
END
GO