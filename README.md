## Задача

Необходимо написать sql-запрос который выводит топ 10 абонентов по кол-ву звонков. 

В результате выводим Id абонента, ФИО абонента и кол-во звонков в которых он участвовал.

## Создание таблиц
```sql
-- Абоненты
CREATE TABLE [dbo].[Abonents]
(
	[Id] BIGINT PRIMARY KEY,
	[FullName] NVARCHAR(256) NOT NULL
);
GO
​
-- Звонки
CREATE TABLE [dbo].[Calls]
(
	[Id] BIGINT PRIMARY KEY,
	[CallerId] BIGINT NOT NULL FOREIGN KEY REFERENCES [dbo].[Abonents] ([Id]),
	[ReceiverId] BIGINT NOT NULL FOREIGN KEY REFERENCES [dbo].[Abonents] ([Id]),
	[Duration] INT NOT NULL
);
GO
```

## Добавление данных
```sql
DECLARE @abonentId BIGINT = 1;
​
WHILE @abonentId <= 1000
BEGIN
	INSERT INTO [dbo].[Abonents] ([Id], [FullName])
	VALUES (@abonentId, CONVERT(NVARCHAR(256), NEWID()));
​
	SET @abonentId = @abonentId + 1;
END;
GO
​
DECLARE @callId BIGINT = 1;
DECLARE @callerId BIGINT
DECLARE @receiverId BIGINT
DECLARE @duration INT
​
WHILE @callId <= 200000
BEGIN
	SET @callerId = ABS(CHECKSUM(NEWID()) % 1000) + 1;
	SET @receiverId = ABS(CHECKSUM(NEWID()) % 1000) + 1;
	SET @duration = ABS(CHECKSUM(NEWID()) % 1000);
​
	INSERT INTO [dbo].[Calls]
		([Id], [CallerId], [ReceiverId], [Duration])
	VALUES
		(@callId, @callerId, @receiverId, @duration);
​
	SET @callId = @callId + 1;
END;
GO
```

## Результат
![image](https://github.com/artemgorbatuk/Task-Pravo-Tech-Set-02-Sql-Phone-Calls-Report/assets/7283674/effffc1f-3e8f-4120-ad37-edf7ac5672b0)
