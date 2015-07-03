CREATE TABLE [dbo].[Company](
	[Symbol] [nvarchar](16) NOT NULL,
	[Exchange] [nvarchar](16) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[LastSale] [real] NOT NULL,
	[MarketCap] [decimal](18, 2) NOT NULL,
	[Sector] [nvarchar](256) NULL,
	[Industry] [nvarchar](256) NULL,
	[DateCreated] [datetime] NOT NULL,
	PRIMARY KEY ([Symbol], 	[Exchange]))
GO

ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO



CREATE PROCEDURE [dbo].[Proc_Company_Symbol_Get] 
AS 
BEGIN
	SELECT DISTINCT([Symbol]) FROM [Company]
END;
GO

CREATE PROCEDURE [dbo].[Proc_Company_Upsert] 
(
	@Symbol NVARCHAR(16)
	, @Exchange NVARCHAR(16)
	, @Name NVARCHAR(256)
	, @LastSale REAL
	, @MarketCap DECIMAL
	, @Sector NVARCHAR(256)
	, @Industry NVARCHAR(256)
)
AS 
BEGIN
	UPDATE [Company]
		SET [Name] = @Name
		, [LastSale] = @LastSale
		, [MarketCap] = @MarketCap
		, [Sector] = @Sector
		, [Industry] = @Industry
		WHERE [Symbol] = @Symbol AND [Exchange] = @Exchange
	IF @@ROWCOUNT = 0 
	BEGIN
		INSERT INTO [Company] ([Symbol], [Exchange], [Name], [LastSale], [MarketCap], [Sector], [Industry])
		VALUES (@Symbol, @Exchange, @Name, @LastSale, @MarketCap, @Sector, @Industry)
	END;
END;
GO

CREATE PROCEDURE [dbo].[Proc_DistinctSymbolFromCompany_Get] 
AS 
BEGIN
	SELECT DISTINCT(Symbol) FROM Company
END;
GO
