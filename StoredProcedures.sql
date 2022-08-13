USE [naturesbasket-2448-yash];
GO


-------------------------------------------------------
--	STORED PROCEDURES
-------------------------------------------------------


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<yash manani>
-- Create date: <12-08-2022>
-- Description:	<to log errors in ErrorLogs table>
-- =============================================
CREATE PROCEDURE dbo.uspLogError 
	-- Add the parameters for the stored procedure here
	@errorJson nvarchar(max),
	@parentId int = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
	DECLARE @identity int;
	DECLARE @json nvarchar(max);

	IF OBJECT_ID(N'tempdb..#temp',N'U') IS NOT NULL
		DROP TABLE #temp;

	SELECT * 
	INTO #temp
	FROM OPENJSON(@errorJson)
	WITH
	(
		ClassName nvarchar(500),
		[Message] nvarchar(500),
		[Data] nvarchar(500),
		InnerException nvarchar(max) as JSON,
		HelpURL nvarchar(500),
		StackTraceString nvarchar(max),
		RemoteStackTraceString nvarchar(max),
		RemoteStackIndex bigint,
		ExceptionMethod nvarchar(500),
		HResult bigint,
		[Source] nvarchar(500),
		WatsonBuckets nvarchar(500),
		ErrorDate datetime2
	);

	INSERT INTO dbo.ErrorLogs (ClassName, [Message], [Data], InnerException, HelpURL, StackTraceString, RemoteStackTraceString, RemoteStackIndex, ExceptionMethod, HResult, [Source], WatsonBuckets, ErrorDate, [ParentId])
	SELECT ClassName, [Message], [Data],
		CASE
			WHEN InnerException IS NULL THEN 0
			ELSE 1
		END, 
		HelpURL, StackTraceString, RemoteStackTraceString, RemoteStackIndex, ExceptionMethod, HResult, [Source], WatsonBuckets, ErrorDate, [ParentId] = @parentId
	FROM #temp

	SET @identity = SCOPE_IDENTITY();
		
	IF(JSON_QUERY(@errorJson,'$.InnerException') IS NOT NULL)
	BEGIN
		SELECT @json = InnerException FROM #temp
		DROP TABLE #temp
		EXEC dbo.uspLogError @json, @identity
	END
END TRY
BEGIN CATCH
IF OBJECT_ID(N'tempdb..#temp',N'U') IS NOT NULL
	DROP TABLE #temp;
SELECT ERROR_LINE() AS [ERROR_LINE],
	ERROR_MESSAGE() AS [ERROR_MESSAGE],
	ERROR_NUMBER() AS [ERROR_NUMBER],
	ERROR_PROCEDURE() AS [ERROR_PROCEDURE], 
	ERROR_SEVERITY() AS [ERROR_SEVERITY],
	ERROR_STATE() AS [ERROR_STATE];
END CATCH
END
GO

