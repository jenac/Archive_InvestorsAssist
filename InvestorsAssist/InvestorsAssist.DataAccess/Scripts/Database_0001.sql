CREATE TABLE [dbo].[Stock](
    [Symbol] [nvarchar](16) NOT NULL,
	[Date] [datetime] NOT NULL,
    [Ibd50Rank] INT NOT NULL,
	[Data] TEXT NULL,
PRIMARY KEY CLUSTERED 
(
	[Symbol] ASC,
    [Date] ASC
));

CREATE PROCEDURE [dbo].[Proc_Stock_Upsert] 
(
	@Symbol NVARCHAR(16)
	, @Date DATETIME
	, @Ibd50Rank INT
	, @Data TEXT
)
AS 
BEGIN
	UPDATE [Stock]
		SET [Ibd50Rank] = @Ibd50Rank,
		[Data] = @Data
		WHERE [Symbol] = @Symbol AND [Date] = @Date
	IF @@ROWCOUNT = 0 
	BEGIN
		INSERT INTO [Stock] ([Symbol], [Date], [Ibd50Rank], [Data])
		VALUES (@Symbol, @Date, @Ibd50Rank, @Data)
	END;
END;

CREATE PROCEDURE [dbo].[Proc_Stock_LastUpdate_Get] 
AS 
BEGIN
	SELECT MAX([Date]) AS LastUpdate FROM [Stock]
END;
