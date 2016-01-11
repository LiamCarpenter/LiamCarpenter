using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using System.Data;
using Online_MI.Models;
using System.Globalization;

namespace Online_MI.Handlers
{
    /// <summary>
    /// Summary description for DetailDownload
    /// </summary>
    public class DetailDownload : IHttpHandler
    {

        private NYSDBEntities db = new NYSDBEntities();

        public void ProcessRequest(HttpContext context)
        {

            string GroupID, ClientPrefix, ClientPrefix2, FromDate, ToDate;

            

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void GenerateOutput(string GroupID, string ClientPrefix, string ClientPrefix2, string TravelType, string FromDate, string ToDate)
        {
            var workBook = new XLWorkbook();
            var workSheet = workBook.AddWorksheet("Data");

            CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
            DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
            DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

            switch (TravelType)
            {
                case "Flights":

                    


                    break;

                case "Rail":
                    break;

                case "Accommodation":
                    break;

                case "Conference":
                    break;

                case "Complaints":
                    break;

                case "Feedback":
                    break;
            }


        }

    }
}