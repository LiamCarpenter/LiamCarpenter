
Why is invoice number in jumping 
select * from FeedInvoice order by invoiceid desc 

ALTER PROCEDURE [dbo].[FeedInvoice_saveAM] (
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




truncate table FeedImportData
go

select statusid,excludeFromExport,* from dbo.FeedImportData 
where 
   transactionnumber = 2974727

select * from dbo.FeedImportData 
where 
   transactionnumber = 2974727
   
update dbo.FeedImportData 
  set statusid = 1000

where 
   transactionnumber = 2974727
   
   
   update dbo.FeedImportData 
  set excludeFromExport = 0

where 
   transactionnumber = 2974727
   
   select * from dbo.FeedInvoice
   where transactionnumber =   3264553
   
   
   	select DISTINCT 
	F.groupid,G.GROUPNAME,
				f.costcode
			
		from FeedImportData f
		--left outer join CostCodeExp c on c.groupid = f.groupid
		JOIN [NYSDB_TEST].[dbo].[group] G ON F.groupid = G.groupid