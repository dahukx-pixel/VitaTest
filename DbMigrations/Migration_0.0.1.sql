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

CREATE PROCEDURE [dbo].[SP_ProcessPayment_FIFO]
    @PaymentAmount DECIMAL(18, 2),
    @OrderID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IncomeID INT;
    DECLARE @RemainingAmount DECIMAL(18, 2);
    DECLARE @CurrentBalance DECIMAL(18, 2);
    DECLARE @DeductAmount DECIMAL(18, 2);
    DECLARE @TotalBalance DECIMAL(18, 2);
    DECLARE @OrderTotalAmount DECIMAL(18, 2);
    DECLARE @OrderPaidAmount DECIMAL(18, 2);
    DECLARE @OrderRemainingAmount DECIMAL(18, 2);
    DECLARE @ErrorMessage NVARCHAR(500);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Проверка 1: PaymentAmount должен быть положительным
        IF @PaymentAmount <= 0
        BEGIN
            THROW 50001, N'Сумма платежа должна быть положительной.', 1;
        END

        -- Проверка 2: Заказ должен существовать
        IF NOT EXISTS (SELECT 1 FROM [dbo].[Orders] WHERE ID = @OrderID)
        BEGIN
            THROW 50002, N'Указанный заказ не существует.', 1;
        END

        -- Получаем информацию о заказе
        SELECT 
            @OrderTotalAmount = TotalAmount,
            @OrderPaidAmount = PaidAmount
        FROM [dbo].[Orders]
        WHERE ID = @OrderID;

        SET @OrderRemainingAmount = @OrderTotalAmount - @OrderPaidAmount;

        -- Проверка 3: Сумма платежа не должна превышать остаток к оплате заказа
        IF @PaymentAmount > @OrderRemainingAmount
        BEGIN
            SET @ErrorMessage = N'Сумма платежа превышает остаток к оплате заказа. Остаток: ' + CAST(@OrderRemainingAmount AS NVARCHAR(20));
            THROW 50004, @ErrorMessage, 1;
        END

        -- Проверка 4: Общая сумма балансов должна быть достаточной
        SELECT @TotalBalance = SUM(Balance) 
        FROM [dbo].[Incomes] 
        WHERE Balance > 0;

        IF @TotalBalance < @PaymentAmount
        BEGIN
            SET @ErrorMessage = N'Недостаточно средств на балансах приходов денег. Доступно: ' + CAST(@TotalBalance AS NVARCHAR(20)) + N', требуется: ' + CAST(@PaymentAmount AS NVARCHAR(20));
            THROW 50005, @ErrorMessage, 1;
        END

        SET @RemainingAmount = @PaymentAmount;

        -- Курсор для получения самых старых записей Incomes
        DECLARE income_cursor CURSOR FOR
        SELECT ID
        FROM [dbo].[Incomes]
        WHERE Balance > 0
        ORDER BY [CreatedAt] ASC, ID ASC;

        OPEN income_cursor;
        FETCH NEXT FROM income_cursor INTO @IncomeID;

        WHILE @@FETCH_STATUS = 0 AND @RemainingAmount > 0
        BEGIN
            SELECT @CurrentBalance = Balance 
            FROM [dbo].[Incomes] 
            WHERE ID = @IncomeID;

            IF @CurrentBalance >= @RemainingAmount
                SET @DeductAmount = @RemainingAmount;
            ELSE
                SET @DeductAmount = @CurrentBalance;

			--Запись в Payments
			INSERT INTO [dbo].[Payments] ([OrderID], [IncomeID], [PaymentAmount], [CreatedAt])
            VALUES (@OrderID, @IncomeID, @DeductAmount, GETDATE());

			--Обновление баланса в Incomes
            UPDATE [dbo].[Incomes]
            SET Balance = Balance - @DeductAmount,
                UpdatedAt = GETDATE()
            WHERE ID = @IncomeID;

            SET @RemainingAmount = @RemainingAmount - @DeductAmount;

            FETCH NEXT FROM income_cursor INTO @IncomeID;
        END

        CLOSE income_cursor;
        DEALLOCATE income_cursor;

        UPDATE [dbo].[Orders]
        SET PaidAmount = PaidAmount + @PaymentAmount,
            UpdatedAt = GETDATE()
        WHERE ID = @OrderID;

        COMMIT TRANSACTION;

        PRINT N'Платеж успешно обработан. Сумма: ' + CAST(@PaymentAmount AS NVARCHAR(20));
    END TRY
    BEGIN CATCH
        IF CURSOR_STATUS('global', 'income_cursor') >= 0
        BEGIN
            CLOSE income_cursor;
            DEALLOCATE income_cursor;
        END

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