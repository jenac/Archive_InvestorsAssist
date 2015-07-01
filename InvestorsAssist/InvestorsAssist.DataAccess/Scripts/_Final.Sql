CREATE PROCEDURE [dbo].[Proc_EodState_Get] 
AS 
BEGIN
  SELECT C.Symbol, VW.[Last] FROM [Vw_Symbols] VW 
    RIGHT JOIN Company C 
    ON VW.Symbol = C.Symbol
END
GO