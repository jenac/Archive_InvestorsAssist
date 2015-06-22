CREATE TABLE [dbo].[Stock](
	[Symbol] [nvarchar](16) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Ibd50Rank] [int] NOT NULL,
	[Data] [text] NULL,
	[Following] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Symbol] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Stock] ADD  CONSTRAINT [DF_Stock_Following]  DEFAULT ((1)) FOR [Following]
GO


CREATE PROCEDURE [dbo].[Proc_Stock_Upsert] 
(
	@Symbol NVARCHAR(16)
	, @Date DATETIME
	, @Ibd50Rank INT
	, @Data TEXT
	, @Following BIT
)
AS 
BEGIN
	UPDATE [Stock]
		SET [Ibd50Rank] = @Ibd50Rank,
		[Data] = @Data,
		[Following] = @Following
		WHERE [Symbol] = @Symbol AND [Date] = @Date
	IF @@ROWCOUNT = 0 
	BEGIN
		INSERT INTO [Stock] ([Symbol], [Date], [Ibd50Rank], [Data], [Following])
		VALUES (@Symbol, @Date, @Ibd50Rank, @Data, @Following)
	END;
END;