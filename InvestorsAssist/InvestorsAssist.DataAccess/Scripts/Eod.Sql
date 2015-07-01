CREATE TABLE [dbo].[Eod](
	[Symbol] [nvarchar](16) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Open] [float] NOT NULL,
	[High] [float] NOT NULL,
	[Low] [float] NOT NULL,
	[Close] [float] NOT NULL,
	[Volume] [decimal](18, 2) NOT NULL,
	PRIMARY KEY ([Symbol], [Date])
)
GO

CREATE VIEW [dbo].[Vw_Symbols]
AS
  SELECT Symbol, MAX([Date]) AS [Last] FROM Eod GROUP BY Symbol
GO

CREATE VIEW [dbo].[Vw_ComputedEod]
AS
  SELECT Symbol
	, [Date]
	, ([Close] - [Open]) AS [GL]
	FROM Eod
GO


CREATE PROCEDURE [dbo].[Proc_Last3Eod_Get] 
(
	@Symbol NVARCHAR(16)
)
AS 
BEGIN
	SELECT TOP 3 [Symbol], [Date], [Open], [High], [Low], [Close], [Volume]
		FROM Eod  WHERE [Symbol] = @Symbol Order By [Date] DESC
END;
GO
