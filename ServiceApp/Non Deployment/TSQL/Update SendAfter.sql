USE [NYSDB_TEST]
GO

/****** Object:  StoredProcedure [dbo].[CorporateFeedbackLimit_listWithValues]    Script Date: 03/24/2014 09:59:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
--GO

--CREATE procedure [dbo].[CorporateFeedbackLimit_listWithValues] as
--select
--            CorporateFeedbackLimitID,
--            ClientID,
--            AirPercentage,
--            RailPercentage,
--            HotelPercentage,
--            LastInvoiceNoAir,
--            LastInvoiceNoRail,
--            LastInvoiceNoHotel,
--            SendAfterAir,
--            SendAfterRail,
--            SendAfterHotel,
--            notes
--        from corporateFeedbackLimit a
--        WHERE ((AirPercentage is not null and AirPercentage > 0 and SendAfterAir is not null) 
--        or (RailPercentage is not null and RailPercentage > 0 and SendAfterRail is not null)
--        or (HotelPercentage is not null and HotelPercentage > 0 and SendAfterHotel is not null));

--GO


update corporateFeedbackLimit
 set 
 SendAfterAir = 10,
 SendAfterHotel = 10

        WHERE ((AirPercentage is not null and AirPercentage > 0 and SendAfterAir is not null) 
        or (RailPercentage is not null and RailPercentage > 0 and SendAfterRail is not null)
        or (HotelPercentage is not null and HotelPercentage > 0 and SendAfterHotel is not null));