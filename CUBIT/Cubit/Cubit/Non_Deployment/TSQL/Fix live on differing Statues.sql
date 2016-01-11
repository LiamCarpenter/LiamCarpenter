


select statusid,* from dbo.FeedImportData
 where transactionnumber = 3020602 and statusid<>1000
 
 
select statusid,* from dbo.FeedImportData
 where transactionnumber = 9 and statusid<>1000
 
 select statusid,* from dbo.FeedImportData
 where transactionnumber = 9
 
 update  dbo.FeedImportData
 set transactionnumber = 9
  where transactionnumber = 3020602 and statusid<>1000
  
  
   update  dbo.FeedImportData
 set transactionnumber = 3020602 
  where transactionnumber = 9 and statusid<>1000