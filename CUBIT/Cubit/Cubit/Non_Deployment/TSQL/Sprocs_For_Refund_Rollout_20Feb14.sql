USE [CUBIT_TEST]
GO

/****** Object:  StoredProcedure [dbo].[FeedData_exportlistAM]    Script Date: 04/07/2014 09:16:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FeedData_exportlistAM] @groupid int,
	@statusid int
as
SET NOCOUNT ON;
    select transactionnumber,dataid
    from FeedImportData
	where groupid = @groupid
	and statusid = @statusid
	and (excludefromexport is null or excludefromexport = 0)
	and currency = 'GBP'
	order by transactionnumber;


GO


USE [CUBIT_TEST]
GO

/****** Object:  StoredProcedure [dbo].[FeedImportData_saveExcludeFromExportAM]    Script Date: 04/07/2014 09:17:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FeedImportData_saveExcludeFromExportAM] (
    @transactionnumber Int,
    @excludefromexport bit
) as
SET NOCOUNT ON;
--DECLARE @CatID int
--SET @CatID = (select categoryid from feedcategory where categorycode = @categoryid)

    
   begin
        update FeedImportData set
                excludeFromExport = @excludefromexport
            where
                dataid= @transactionnumber;
        select
                @transactionnumber as transactionnumber;
end;


GO


USE [CUBIT_TEST]
GO

/****** Object:  StoredProcedure [dbo].[FeedInvoice_saveAM]    Script Date: 04/07/2014 09:17:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FeedInvoice_saveAM] (
    @invoiceid Int,
    @transactionnumber Int,
    @systemnysuserid Int,
    @invoiceexportednett VarChar(50),
    @invoiceexportedvat VarChar(50),
    @invoiceexportedgross VarChar(50),
    @invoiceexport Bit,
    @invoiceexportdate DateTime,
    @transactionfee VarChar(50)
) as
SET NOCOUNT ON;
if not exists(select invoiceid from FeedInvoice where transactionnumber=@transactionnumber)
	begin
		insert into FeedInvoice (
				transactionnumber,
                systemnysuserid,
                invoiceexportednett,
                invoiceexportedvat,
                invoiceexportedgross,
                invoiceexport,
                invoiceexportdate,
				transactionfee,
                datecreated,
                lastupdated
            ) values (
                @transactionnumber,
				@systemnysuserid,
                @invoiceexportednett,
                @invoiceexportedvat,
                @invoiceexportedgross,
                @invoiceexport,
                @invoiceexportdate,
				@transactionfee,
                getdate(),
                getdate());
        select
                scope_identity() as invoiceid;
    end else begin
    
            update FeedInvoice set
                transactionnumber = @transactionnumber,
				systemnysuserid = @systemnysuserid,
                invoiceexportednett = @invoiceexportednett,
                invoiceexportedvat = @invoiceexportedvat,
                invoiceexportedgross = @invoiceexportedgross,
                invoiceexport = @invoiceexport,
                invoiceexportdate = @invoiceexportdate,
				transactionfee = @transactionfee,
                lastupdated = getdate()
            where
                transactionnumber = @transactionnumber;
                
          select invoiceid as invoiceid from FeedInvoice where transactionnumber=@transactionnumber;
end;


GO


USE [CUBIT_TEST]
GO

/****** Object:  StoredProcedure [dbo].[SetBookingIDtoExportStatusAM]    Script Date: 04/07/2014 09:17:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SetBookingIDtoExportStatusAM] (
    @transactionnumber Int,
    @statusid Int
) as
SET NOCOUNT ON;
    update FeedImportData
	set statusid = @statusid
	where dataid= @transactionnumber
    select @transactionnumber as transactionnumber;


GO
