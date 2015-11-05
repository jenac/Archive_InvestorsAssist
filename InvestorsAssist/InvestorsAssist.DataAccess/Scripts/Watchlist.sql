
CREATE TABLE [dbo].[Watchlist](
	[Name] [nvarchar](50) NOT NULL,
	[CSV] [text] NOT NULL,
	[Active] [bit] NOT NULL DEFAULT 1,
	PRIMARY KEY ([Name])
)
GO

CREATE PROCEDURE [dbo].[Proc_Watchlist_GetActive] 
AS 
BEGIN
	SELECT [Name], [CSV], [Active] FROM [Watchlist]
		WHERE [Active] = 1
END
GO

