--This SQL function need for stop repeat sql query and stop incorrect request.
CREATE FUNCTION dbo.ConvertTimeStampToDateWithOffset (@TimeStamp BIGINT, @TimeZoneOffset int)
RETURNS DATETIMEOFFSET
AS
BEGIN
    DECLARE @Epoch DATETIMEOFFSET = '1970-01-01';
    DECLARE @DateTime DATETIMEOFFSET;
    SET @DateTime = SWITCHOFFSET(DATEADD(SECOND, @TimeStamp, @Epoch), @TimeZoneOffset);
    RETURN @DateTime;
END;


