CREATE TABLE [dbo].[IbdPick](
	[Symbol] [nvarchar](16) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Ibd50Rank] [int] NULL,
	[Data] [text] NULL,
	[Following] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Symbol] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[IbdPick] ADD  CONSTRAINT [DF_IbdPick_Following]  DEFAULT ((1)) FOR [Following]
GO


CREATE PROCEDURE [dbo].[Proc_IbdPick_Upsert] 
(
	@Symbol NVARCHAR(16)
	, @Date DATETIME
	, @Ibd50Rank INT
	, @Data TEXT
	, @Following BIT
)
AS 
BEGIN
	UPDATE [IbdPick]
		SET [Ibd50Rank] = @Ibd50Rank,
		[Data] = @Data,
		[Following] = @Following
		WHERE [Symbol] = @Symbol AND [Date] = @Date
	IF @@ROWCOUNT = 0 
	BEGIN
		INSERT INTO [IbdPick] ([Symbol], [Date], [Ibd50Rank], [Data], [Following])
		VALUES (@Symbol, @Date, @Ibd50Rank, @Data, @Following)
	END;
END;
GO

CREATE PROCEDURE [dbo].[Proc_IbdPick_Following_Get] 
AS 
BEGIN
	SELECT DISTINCT([Symbol]) FROM [IbdPick] WHERE [Following] = 1
END;
GO

CREATE PROCEDURE [dbo].[Proc_IbdPick_Last2_Dates_Get] 
AS 
BEGIN
	WITH CTE ([Date]) AS (
	SELECT DISTINCT([Date]) 
		FROM [IbdPick] 
		WHERE [Ibd50Rank] IS NOT NULL 
		)
	SELECT TOP 2 [Date] FROM CTE ORDER BY [Date] DESC
END;
GO

CREATE PROCEDURE [dbo].[Proc_IbdPick_Ibd50_Symbol_By_Date_Get] 
(
	@Date DATETIME
)
AS 
BEGIN
	SELECT [Symbol] 
		FROM [IbdPick] 
		WHERE [Ibd50Rank] IS NOT NULL 
			AND [Date] = @Date
END;
GO

CREATE PROCEDURE [dbo].[Proc_IbdPick_LastUpdate_Get] 
AS 
BEGIN
	SELECT MAX([Date]) AS LastUpdate FROM [IbdPick]
END;
GO