CREATE DATABASE [VitaTestDB];
GO

USE [VitaTestDB];
GO

-- 1. Таблица "Заказы"
CREATE TABLE [dbo].[Orders] ([ID] INT IDENTITY(1,1) PRIMARY KEY,                    
							 [TotalAmount] DECIMAL(18, 2) NOT NULL,            
							 [PaidAmount] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
							 [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),  
							 [UpdatedAt] DATETIME NOT NULL DEFAULT GETDATE()   
							);
GO

-- 2. Таблица "Приход денег"
CREATE TABLE [dbo].[Incomes] ([ID] INT IDENTITY(1,1) PRIMARY KEY,                           
							  [Amount] DECIMAL(18, 2) NOT NULL,                 
							  [Balance] DECIMAL(18, 2) NOT NULL DEFAULT 0.00,   
							  [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),  
							  [UpdatedAt] DATETIME NOT NULL DEFAULT GETDATE()   
							 );
GO

-- 3. Таблица "Платежи"
CREATE TABLE [dbo].[Payments] ([ID] INT IDENTITY(1,1) PRIMARY KEY,        
							   [OrderID] INT NOT NULL,                           
							   [IncomeID] INT NOT NULL,                          
							   [PaymentAmount] DECIMAL(18, 2) NOT NULL,          
							   [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),

			CONSTRAINT [FK_Payments_Orders] FOREIGN KEY ([OrderID]) 
				REFERENCES [dbo].[Orders]([ID])
				ON DELETE NO ACTION ON UPDATE CASCADE,
        
			CONSTRAINT [FK_Payments_Income] FOREIGN KEY ([IncomeID]) 
				REFERENCES [dbo].[Incomes]([ID]) 
				ON DELETE NO ACTION ON UPDATE CASCADE

							  );
GO

-- 4. Таблица "Версия"
CREATE TABLE [dbo].[DatabaseVersion] ([ID] INT PRIMARY KEY DEFAULT 1,                   
									  [Version] VARCHAR(20) NOT NULL DEFAULT '0.0.0',   
									  [LastUpdated] DATETIME DEFAULT GETDATE(),         
			
			CONSTRAINT [CK_DatabaseVersion_SingleRow] CHECK ([ID] = 1)
			
									 );
GO

INSERT INTO [dbo].[DatabaseVersion] ([ID], [Version], [LastUpdated]) 
VALUES (1, '0.0.0', GETDATE());
GO