CREATE TABLE [dbo].[Indicator](
	[Symbol] [nvarchar](16) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Data] [xml] NOT NULL,
	PRIMARY KEY
	(
		[Symbol] ASC,
		[Name] ASC
	)
)

GO

CREATE PROCEDURE [dbo].[Proc_Indicator_Upsert] 
(
	@Symbol NVARCHAR(16)
	, @Name NVARCHAR(50)
	, @Date DATETIME
	, @Data XML
)
AS 
BEGIN
	UPDATE [Indicator]
		SET [Date] = @Date,
		[Data] = @Data
		WHERE [Symbol] = @Symbol AND [Name] = @Name
	IF @@ROWCOUNT = 0 
	BEGIN
		INSERT INTO [Indicator] ([Symbol], [Name], [Date], [Data])
		VALUES (@Symbol, @Name, @Date, @Data)
	END;
END
GO
