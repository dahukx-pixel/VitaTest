USE [VitaTestDB];
GO

DECLARE @CurrentVersion VARCHAR(20);
DECLARE @ExpectedVersion VARCHAR(20) = '0.0.0';
DECLARE @NewVersion VARCHAR(20) = '0.0.1';

SELECT @CurrentVersion = [Version] 
FROM [dbo].[DatabaseVersion] 
WHERE [ID] = 1;

IF @CurrentVersion IS NULL
BEGIN
    THROW 50100, N'Таблица DatabaseVersion пуста или не существует. Инициализируйте базу данных.', 1;
END

IF @CurrentVersion <> @ExpectedVersion
BEGIN
    DECLARE @ErrorMessage NVARCHAR(500) = N'Неверная версия базы данных. Ожидается: ' + @ExpectedVersion + N', текущая версия: ' + @CurrentVersion;
    THROW 50101, @ErrorMessage, 1;
END
GO

-- =============================================
-- ТРИГГЕР 1: Обновление таблицы Orders
-- =============================================
CREATE OR ALTER TRIGGER [dbo].[TR_Payments_UpdateOrders]
ON [dbo].[Payments]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @OrderID INT;
    DECLARE @PaymentAmount DECIMAL(18, 2);
    DECLARE @OrderTotalAmount DECIMAL(18, 2);
    DECLARE @OrderPaidAmount DECIMAL(18, 2);
    DECLARE @OrderRemainingAmount DECIMAL(18, 2);

    BEGIN TRY
        SELECT 
            @OrderID = OrderID,
            @PaymentAmount = PaymentAmount
        FROM inserted;

        IF @PaymentAmount <= 0
        BEGIN
            THROW 50001, N'Сумма платежа должна быть положительной.', 1;
        END

        IF NOT EXISTS (SELECT 1 FROM [dbo].[Orders] WHERE ID = @OrderID)
        BEGIN
            THROW 50002, N'Указанный заказ не существует.', 1;
        END

        SELECT 
            @OrderTotalAmount = TotalAmount,
            @OrderPaidAmount = PaidAmount
        FROM [dbo].[Orders]
        WHERE ID = @OrderID;

        SET @OrderRemainingAmount = @OrderTotalAmount - @OrderPaidAmount;

        IF @PaymentAmount > @OrderRemainingAmount
        BEGIN
            DECLARE @ErrorMessage NVARCHAR(500) = N'Сумма платежа (' + CAST(@PaymentAmount AS NVARCHAR(20)) + N') превышает остаток к оплате (' + CAST(@OrderRemainingAmount AS NVARCHAR(20)) + N').';
            THROW 50003, @ErrorMessage, 1;
        END

        UPDATE [dbo].[Orders]
        SET PaidAmount = PaidAmount + @PaymentAmount,
            UpdatedAt = GETDATE()
        WHERE ID = @OrderID;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

-- =============================================
-- ТРИГГЕР 2: Обновление таблицы Incomes
-- =============================================
CREATE OR ALTER TRIGGER [dbo].[TR_Payments_UpdateIncomes]
ON [dbo].[Payments]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IncomeID INT;
    DECLARE @PaymentAmount DECIMAL(18, 2);
    DECLARE @IncomeBalance DECIMAL(18, 2);

    BEGIN TRY
        SELECT 
            @IncomeID = IncomeID,
            @PaymentAmount = PaymentAmount
        FROM inserted;

        IF @PaymentAmount <= 0
        BEGIN
            THROW 50004, N'Сумма платежа должна быть положительной.', 1;
        END

        IF NOT EXISTS (SELECT 1 FROM [dbo].[Incomes] WHERE ID = @IncomeID)
        BEGIN
            THROW 50005, N'Указанный приход денег не существует.', 1;
        END

        SELECT @IncomeBalance = Balance
        FROM [dbo].[Incomes]
        WHERE ID = @IncomeID;

        IF @PaymentAmount > @IncomeBalance
        BEGIN
            DECLARE @ErrorMessage NVARCHAR(500) = N'Сумма платежа (' + CAST(@PaymentAmount AS NVARCHAR(20)) + N') превышает баланс прихода денег (' + CAST(@IncomeBalance AS NVARCHAR(20)) + N').';
            THROW 50006, @ErrorMessage, 1;
        END

        UPDATE [dbo].[Incomes]
        SET Balance = Balance - @PaymentAmount,
            UpdatedAt = GETDATE()
        WHERE ID = @IncomeID;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO

DECLARE @CurrentVersion VARCHAR(20);
DECLARE @NewVersion VARCHAR(20) = '0.0.1';

SELECT @CurrentVersion = [Version] 
FROM [dbo].[DatabaseVersion] 
WHERE [ID] = 1;

IF @CurrentVersion = '0.0.0'
BEGIN
    UPDATE [dbo].[DatabaseVersion]
    SET [Version] = @NewVersion,
        [LastUpdated] = GETDATE()
    WHERE [ID] = 1;

    PRINT N'Версия базы данных успешно обновлена с 0.0.0 до ' + @NewVersion;
END
ELSE
BEGIN
    PRINT N'Версия базы данных уже обновлена. Текущая версия: ' + ISNULL(@CurrentVersion, 'NULL');
END
GO