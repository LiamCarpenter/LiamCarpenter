using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Online_MI.Models;
using System.Net;
using System.Globalization;
using ClosedXML.Excel;
using System.IO;
using System.Web.Script.Serialization;

namespace Online_MI.Controllers
{

    public class HomeController : Controller
    {

        private NYSDBEntities db = new NYSDBEntities();
        private SSOEntities dbSSO = new SSOEntities();

        public ActionResult GetIndexData(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];

            if(FromDate == null && ToDate == null)
            {
                IList<Online_MI.Models.Dashboardv2_Result> Dashboard = db.Dashboardv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)), client.ssoClientPrefix, "XXXXXX",client.ssoClientName).ToList();

                return ReturnSerializedData(Dashboard);
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                IList<Online_MI.Models.Dashboardv2_Result> Dashboard = db.Dashboardv2(client.ssoClientID, fDate, tDate, client.ssoClientPrefix, "XXXXXX", client.ssoClientName).ToList();

                return ReturnSerializedData(Dashboard);
            }
        }


        //Paste this TOKEN to end of URL for test - will return LV data:
        // /Home/Index?TOKEN=9CD982C790EDADFA63CDE5A7D57B1DA8E90D764467228DAC97E463EB046BBE772CA3A3F6C360355F5570EA2E10A6DB2BA1590A468D7FB0B1F065EE3E5633C325
        public ActionResult Index(string TOKEN)
        {

            if (LoggedInUser())
            {
                return View();
            }

            if (TOKEN == null)
            {
                return Redirect("~/Login");
            }
            else
            {
                
                string decryptedToken = Encryption.DecryptParam(Encryption.GetKey("NysMI%20*"), TOKEN);

                if (decryptedToken.Contains("ENC_TIME") && decryptedToken.Contains("LOGINNAME"))
                {
                    //Successful transfer token
                    string userEmail = HttpUtility.HtmlEncode(decryptedToken.Substring(decryptedToken.IndexOf("LOGINNAME=") + 10, decryptedToken.Length - (decryptedToken.IndexOf("LOGINNAME=") + 10)));
           
                    ssoUser user = (from u in dbSSO.ssoUser where u.ssoUserEmail == userEmail select u).FirstOrDefault();
                    ssoClient client;

                    if(user != null)
                    {
                        client = (from c in dbSSO.ssoClient where c.ssoClientID == user.ssoClientID select c).FirstOrDefault();

                        Session["loggedInUser"] = true;
                        Session["user"] = user;
                        Session["client"] = client;

                        return View();

                    }
                    else
                    {
                        return Redirect("~/Login");
                    }
                    

                }
                else
                {
                    return Redirect("~/Login");
                }

            }

        }

        private bool LoggedInUser()
        {
            if (Session["loggedInUser"] != null)
            {
                bool loggedIn = (bool)Session["loggedInUser"];

                if (loggedIn)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public ActionResult GetComplaints(String FromDate, String ToDate)
        {

            ssoClient client = (ssoClient)Session["client"];


            if (FromDate == null && ToDate == null)
            {
                IList<Online_MI.Models.ComplaintsSummary_Result> Complaints = db.ComplaintsSummary(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)), client.ssoClientPrefix.ToUpper(), "XXXXXX").ToList();

                return ReturnSerializedData(Complaints);
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);
       
                IList<Online_MI.Models.ComplaintsSummary_Result> Complaints = db.ComplaintsSummary(client.ssoClientID, fDate, tDate, client.ssoClientPrefix.ToUpper(), "XXXXXX").ToList();

                return ReturnSerializedData(Complaints);
            }

        }

        public void DownloadComplaints(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];
            IList<Online_MI.Models.ComplaintsSummary_Result> Complaints;

            if (FromDate == null && ToDate == null)
            {
                Complaints = db.ComplaintsSummary(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)), client.ssoClientPrefix.ToUpper(), "XXXXXX").ToList();
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                Complaints = db.ComplaintsSummary(client.ssoClientID, fDate, tDate, client.ssoClientPrefix.ToUpper(), "XXXXXX").ToList();
            }



            XLWorkbook wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Complaint Data");

            ws.Cell("A1").Value = "Complaints Data";
            ws.Cell("A1").Style.Font.Bold = true;

            ws.Cell("A3").Value = "Against";
            ws.Cell("B3").Value = "Raised";
            ws.Cell("C3").Value = "Details";
            ws.Cell("D3").Value = "Resolution";

            int i = 4;

            foreach (Online_MI.Models.ComplaintsSummary_Result c in Complaints)
            {
                ws.Cell("A" + i).Value = c.ComplaintDetailAgainst;
                ws.Cell("B" + i).Value = c.ComplaintDetailDate;
                ws.Cell("C" + i).Value = c.ComplaintDetailText;
                ws.Cell("D" + i).Value = c.ComplaintDetailResolution;

                i++;
            }

            ProduceExcelOutput(wb);

        }

        public void ProduceExcelOutput(XLWorkbook wb)
        {

            HttpResponse httpResponse = System.Web.HttpContext.Current.Response;
            httpResponse.Clear();

            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=\"Data.xlsx\"");

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
        }

        public ActionResult GetFlights(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];

            if (FromDate == null && ToDate == null)
            {
                IList<Online_MI.Models.FlightDetailsv2_Result> Flights = db.FlightDetailsv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).ToList();

                return ReturnSerializedData(Flights);
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                IList<Online_MI.Models.FlightDetailsv2_Result> Flights = db.FlightDetailsv2(client.ssoClientID, fDate, tDate).ToList();

                return ReturnSerializedData(Flights);
            }


        }

        public void DownloadFlights(String FromDate, String ToDate, String FlightDepartment)
        {
            ssoClient client = (ssoClient)Session["client"];

            IList<Online_MI.Models.FlightDetailsv2_Result> Flights;

            if (FromDate == null && ToDate == null)
            {
                Flights = db.FlightDetailsv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).ToList();
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                Flights = db.FlightDetailsv2(client.ssoClientID, fDate, tDate).ToList();
            }

            if (FlightDepartment != null)
            {
               List<FlightDetailsv2_Result> deleteFlight = new List<FlightDetailsv2_Result>();

                foreach (Online_MI.Models.FlightDetailsv2_Result f in Flights)
                {
                    if(f.DeptCode != null)
                    {
                        if (!f.DeptCode.ToLower().Contains(FlightDepartment.ToLower()))
                        {
                            deleteFlight.Add(f);
                        }
                    }
                    
                }

                foreach (FlightDetailsv2_Result ind in deleteFlight)
                {
                    Flights.Remove(ind);
                }
            }


            XLWorkbook wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Flights Data");

            ws.Cell("A1").Value = "Flights Data";
            ws.Cell("A1").Style.Font.Bold = true;

            ws.Cell("A3").Value = "Invoice";
            ws.Cell("B3").Value = "Passenger";
            ws.Cell("C3").Value = "Supplier";
            ws.Cell("D3").Value = "Start";
            ws.Cell("E3").Value = "End";
            ws.Cell("F3").Value = "Route";
            ws.Cell("G3").Value = "Fare";
            ws.Cell("H3").Value = "Fee";
            ws.Cell("I3").Value = "Tax";
            ws.Cell("J3").Value = "Billed";

            if (client.ssoClientName == "University of York" )
            {
                ws.Cell("K3").Value = "Work Order";
                ws.Cell("L3").Value = "Dept";
                ws.Cell("M3").Value = "Booker";
            }
            else
            {
                ws.Cell("K3").Value = "Booker";
            }


            

            int i = 4;

            foreach (Online_MI.Models.FlightDetailsv2_Result f in Flights)
            {
                ws.Cell("A" + i).Value = f.rInvoiceNo;
                ws.Cell("B" + i).Value = f.rPassengerName;
                ws.Cell("C" + i).Value = f.Supplier;
                ws.Cell("D" + i).Value = f.StartDate;
                ws.Cell("E" + i).Value = f.EndDate;
                ws.Cell("F" + i).Value = f.Route;
                ws.Cell("G" + i).Value = f.Fare;
                ws.Cell("H" + i).Value = f.Fee;
                ws.Cell("I" + i).Value = f.Tax;
                ws.Cell("J" + i).Value = f.Billed;

                if (client.ssoClientName == "University of York")
                {
                    ws.Cell("K" + i).Value = f.WorkOrder;
                    ws.Cell("L" + i).Value = f.DeptCode;
                    ws.Cell("M" + i).Value = f.Booker;
                }
                else
                {
                    ws.Cell("K" + i).Value = f.Booker;
                }

                i++;
            }

            ProduceExcelOutput(wb);

        }

        public ActionResult GetUKRail(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];

            if (FromDate == null && ToDate == null)
            {
                IList<Online_MI.Models.RailBookingDetailsv2_Result> UKRail = db.RailBookingDetailsv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).ToList();

                return ReturnSerializedData(UKRail);
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                IList<Online_MI.Models.RailBookingDetailsv2_Result> UKRail = db.RailBookingDetailsv2(client.ssoClientID, fDate, tDate).ToList();

                return ReturnSerializedData(UKRail);

            }


        }

        private ContentResult ReturnSerializedData<T>(IList<T> data)
        {
            var serialize = new JavaScriptSerializer();
            serialize.MaxJsonLength = Int32.MaxValue;
            var result = new ContentResult();
            result.Content = serialize.Serialize(data);
            result.ContentType = "text/json";
            return result;
        }

        public ActionResult GetIntRail(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];

            if (FromDate == null && ToDate == null)
            {
                IList<Online_MI.Models.InternationalRailDetailsv2_Result> IntRail = db.InternationalRailDetailsv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).ToList();

                return ReturnSerializedData(IntRail);
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                IList<Online_MI.Models.InternationalRailDetailsv2_Result> IntRail = db.InternationalRailDetailsv2(client.ssoClientID, fDate, tDate).ToList();

                return ReturnSerializedData(IntRail);
            }


        }

        public void DownloadRail(String FromDate, String ToDate, String UKRailDepartment, String IntRailDepartment)
        {
            ssoClient client = (ssoClient)Session["client"];

            IList<Online_MI.Models.RailBookingDetailsv2_Result> UKRail;
            IList<Online_MI.Models.InternationalRailDetailsv2_Result> IntRail;

            if (FromDate == null && ToDate == null)
            {
                UKRail = db.RailBookingDetailsv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).ToList();
                IntRail = db.InternationalRailDetailsv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).ToList();

            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                UKRail = db.RailBookingDetailsv2(client.ssoClientID, fDate, tDate).ToList();
                IntRail = db.InternationalRailDetailsv2(client.ssoClientID, fDate, tDate).ToList();

            }

            if (UKRailDepartment != null)
            {
                List<RailBookingDetailsv2_Result> deleteUKRail = new List<RailBookingDetailsv2_Result>();

                foreach (Online_MI.Models.RailBookingDetailsv2_Result r in UKRail)
                {
                    if(r.DeptCode != null)
                    {
                        if (!r.DeptCode.ToLower().Contains(UKRailDepartment.ToLower()))
                        {
                            deleteUKRail.Add(r);
                        }
                    }
                }

                foreach (RailBookingDetailsv2_Result ind in deleteUKRail)
                {
                    UKRail.Remove(ind);
                }
            }

            if (IntRailDepartment != null)
            {
                List<InternationalRailDetailsv2_Result> deleteIntRail = new List<InternationalRailDetailsv2_Result>();

                foreach (Online_MI.Models.InternationalRailDetailsv2_Result r in IntRail)
                {
                    if(r.DeptCode != null)
                    {
                        if (!r.DeptCode.ToLower().Contains(IntRailDepartment.ToLower()))
                        {
                            deleteIntRail.Add(r);
                        }
                    }
                }

                foreach (InternationalRailDetailsv2_Result ind in deleteIntRail)
                {
                    IntRail.Remove(ind);
                }
            }



            XLWorkbook wb = new XLWorkbook();
            var wsUKRail = wb.AddWorksheet("UK Rail Data");
            var wsIntRail = wb.AddWorksheet("Int Rail Data");

            wsUKRail.Cell("A1").Value = "UK Rail Data";
            wsUKRail.Cell("A1").Style.Font.Bold = true;

            wsUKRail.Cell("A3").Value = "Invoice";
            wsUKRail.Cell("B3").Value = "Passenger";
            wsUKRail.Cell("C3").Value = "Start";
            wsUKRail.Cell("D3").Value = "End";
            wsUKRail.Cell("E3").Value = "Route";
            wsUKRail.Cell("F3").Value = "Fare";
            wsUKRail.Cell("G3").Value = "Charge";
            wsUKRail.Cell("H3").Value = "Discount";
            wsUKRail.Cell("I3").Value = "Billed";

            if(client.ssoClientName == "University of York")
            {
                wsUKRail.Cell("J3").Value = "Work Order";
                wsUKRail.Cell("K3").Value = "Dept";
                wsUKRail.Cell("L3").Value = "Booker";
            }
            else
            {
            
                wsUKRail.Cell("J3").Value = "Booker";

            }

            int i = 4;

            foreach (Online_MI.Models.RailBookingDetailsv2_Result r in UKRail)
            {
                wsUKRail.Cell("A" + i).Value = r.rInvoiceNo;
                wsUKRail.Cell("B" + i).Value = r.rPassengerName;
                wsUKRail.Cell("C" + i).Value = r.StartDate;
                wsUKRail.Cell("D" + i).Value = r.EndDate;
                wsUKRail.Cell("E" + i).Value = r.Route;
                wsUKRail.Cell("F" + i).Value = r.Fare;
                wsUKRail.Cell("G" + i).Value = r.Charge;
                wsUKRail.Cell("H" + i).Value = r.Discount;
                wsUKRail.Cell("I" + i).Value = r.Billed;

                if(client.ssoClientName == "University of York")
                {
                    wsUKRail.Cell("J" + i).Value = r.WorkOrder;
                    wsUKRail.Cell("K" + i).Value = r.DeptCode;
                    wsUKRail.Cell("L" + i).Value = r.Booker;
                }
                else
                {
                    wsUKRail.Cell("J" + i).Value = r.Booker;
                }

                i++;
            }

            wsIntRail.Cell("A1").Value = "International Rail Data";
            wsIntRail.Cell("A1").Style.Font.Bold = true;

            wsIntRail.Cell("A3").Value = "Invoice";
            wsIntRail.Cell("B3").Value = "Passenger";
            wsIntRail.Cell("C3").Value = "Supplier";
            wsIntRail.Cell("D3").Value = "Start";
            wsIntRail.Cell("E3").Value = "End";
            wsIntRail.Cell("F3").Value = "Route";
            wsIntRail.Cell("G3").Value = "Fare";
            wsIntRail.Cell("H3").Value = "Charge";
            wsIntRail.Cell("I3").Value = "Discount";
            wsIntRail.Cell("J3").Value = "Billed";

            if(client.ssoClientName == "University of York")
            {
                wsIntRail.Cell("K3").Value = "Work Order";
                wsIntRail.Cell("L3").Value = "Dept";
                wsIntRail.Cell("M3").Value = "Booker";
            }
            else
            {
                wsIntRail.Cell("K3").Value = "Booker";
            }

            i = 4;

            foreach (Online_MI.Models.InternationalRailDetailsv2_Result r in IntRail)
            {
                wsIntRail.Cell("A" + i).Value = r.rInvoiceNo;
                wsIntRail.Cell("B" + i).Value = r.rPassengerName;
                wsIntRail.Cell("C" + i).Value = r.Supplier;
                wsIntRail.Cell("D" + i).Value = r.StartDate;
                wsIntRail.Cell("E" + i).Value = r.EndDate;
                wsIntRail.Cell("F" + i).Value = r.Route;
                wsIntRail.Cell("G" + i).Value = r.Fare;
                wsIntRail.Cell("H" + i).Value = r.Charge;
                wsIntRail.Cell("I" + i).Value = r.Discount;
                wsIntRail.Cell("J" + i).Value = r.Billed;

                if (client.ssoClientName == "University of York")
                {
                    wsIntRail.Cell("K" + i).Value = r.WorkOrder;
                    wsIntRail.Cell("L" + i).Value = r.DeptCode;
                    wsIntRail.Cell("M" + i).Value = r.Booker;
                }
                else
                {
                    wsIntRail.Cell("K" + i).Value = r.Booker;
                }

                i++;
            }

            ProduceExcelOutput(wb);


        }

        public ActionResult GetAccommodation(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];

            if (FromDate == null && ToDate == null)
            {
                IList<Online_MI.Models.AccomodationDetailsv2_Result> Accom = db.AccomodationDetailsv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).ToList();

                return ReturnSerializedData(Accom);
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                IList<Online_MI.Models.AccomodationDetailsv2_Result> Accom = db.AccomodationDetailsv2(client.ssoClientID, fDate, tDate).ToList();

                return ReturnSerializedData(Accom);
            }


        }

        public void DownloadAccommodation(String FromDate, String ToDate, String AccommodationDepartment)
        {
            ssoClient client = (ssoClient)Session["client"];

            IList<Online_MI.Models.AccomodationDetailsv2_Result> Accommodation;

            if (FromDate == null && ToDate == null)
            {
                Accommodation = db.AccomodationDetailsv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).ToList();
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                Accommodation = db.AccomodationDetailsv2(client.ssoClientID, fDate, tDate).ToList();
            }

            if (AccommodationDepartment != null)
            {
                List<AccomodationDetailsv2_Result> deleteAcc = new List<AccomodationDetailsv2_Result>();

                foreach (Online_MI.Models.AccomodationDetailsv2_Result a in Accommodation)
                {
                    if(a.DeptCode != null)
                    {
                        if (!a.DeptCode.ToLower().Contains(AccommodationDepartment.ToLower()))
                        {
                            deleteAcc.Add(a);
                        }
                    }
                    
                }

                foreach (AccomodationDetailsv2_Result ind in deleteAcc)
                {
                    Accommodation.Remove(ind);
                }
            }

            XLWorkbook wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Accommodation Data");

            ws.Cell("A1").Value = "Accommodation Data";
            ws.Cell("A1").Style.Font.Bold = true;

            ws.Cell("A3").Value = "Invoice";
            ws.Cell("B3").Value = "Guest";
            ws.Cell("C3").Value = "Venue";
            ws.Cell("D3").Value = "Location";
            ws.Cell("E3").Value = "Arrival";
            ws.Cell("F3").Value = "Departure";
            ws.Cell("G3").Value = "Fare(inc)";
            ws.Cell("H3").Value = "Fare(exc)";
            ws.Cell("I3").Value = "VAT";

            if(client.ssoClientName == "University of York")
            {
                ws.Cell("J3").Value = "Work Order";
                ws.Cell("K3").Value = "Dept";
                ws.Cell("L3").Value = "Booker";
            } else
            {
                ws.Cell("J3").Value = "Booker";
            }

            

            int i = 4;

            foreach (Online_MI.Models.AccomodationDetailsv2_Result a in Accommodation)
            {
                ws.Cell("A" + i).Value = a.inm_no;
                ws.Cell("B" + i).Value = a.inm_ldname;
                ws.Cell("C" + i).Value = a.VenueName;
                ws.Cell("D" + i).Value = a.Location;
                ws.Cell("E" + i).Value = a.Arrival;
                ws.Cell("F" + i).Value = a.Departure;
                ws.Cell("G" + i).Value = a.FareIncVat;
                ws.Cell("H" + i).Value = a.FareExcVat;
                ws.Cell("I" + i).Value = a.VAT;

                if (client.ssoClientName == "University of York")
                {
                    ws.Cell("J" + i).Value = a.WorkOrder;
                    ws.Cell("K" + i).Value = a.DeptCode;
                    ws.Cell("L" + i).Value = a.Booker;
                }
                else
                {
                    ws.Cell("J" + i).Value = a.Booker;
                }

                i++;
            }

            ProduceExcelOutput(wb);

        }

        public ActionResult GetConferences(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];

            if (FromDate == null && ToDate == null)
            {
                IList<Online_MI.Models.ConferenceDetailsv2_Result> Conference = db.ConferenceDetailsv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).ToList();

                return ReturnSerializedData(Conference);
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                IList<Online_MI.Models.ConferenceDetailsv2_Result> Conference = db.ConferenceDetailsv2(client.ssoClientID, fDate, tDate).ToList();

                return ReturnSerializedData(Conference);
            }


        }

        public void DownloadConference(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];

            IList<Online_MI.Models.ConferenceDetailsv2_Result> Conference;

            if (FromDate == null && ToDate == null)
            {
                Conference = db.ConferenceDetailsv2(client.ssoClientID, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).ToList();
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                Conference = db.ConferenceDetailsv2(client.ssoClientID, fDate, tDate).ToList();
            }

            XLWorkbook wb = new XLWorkbook();
            var ws = wb.AddWorksheet("Conference Data");

            ws.Cell("A1").Value = "Conference Data";
            ws.Cell("A1").Style.Font.Bold = true;

            ws.Cell("A3").Value = "Invoice";
            ws.Cell("B3").Value = "Guest";
            ws.Cell("C3").Value = "Venue";
            ws.Cell("D3").Value = "Location";
            ws.Cell("E3").Value = "Arrival";
            ws.Cell("F3").Value = "Departure";
            ws.Cell("G3").Value = "Fare(inc)";
            ws.Cell("H3").Value = "Fare(exc)";
            ws.Cell("I3").Value = "VAT";

            int i = 4;

            foreach (Online_MI.Models.ConferenceDetailsv2_Result c in Conference)
            {
                

                i++;
            }

            ProduceExcelOutput(wb);

        }

        public ActionResult GetConferenceFeedback(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];

            if (FromDate == null && ToDate == null)
            {
                IList<Online_MI.Models.ConferenceFeedbackDetail_Result> ConfFeed = db.ConferenceFeedbackDetail(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)), client.ssoClientName).ToList();

                return ReturnSerializedData(ConfFeed);
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                IList<Online_MI.Models.ConferenceFeedbackDetail_Result> ConfFeed = db.ConferenceFeedbackDetail(fDate, tDate, client.ssoClientName).ToList();

                return ReturnSerializedData(ConfFeed);
            }


        }

        public ActionResult GetCorporateFeedback(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];

            if (FromDate == null && ToDate == null)
            {
                IList<Online_MI.Models.CorporateFeedbackDetail_Result> CorpFeed = db.CorporateFeedbackDetail(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)), client.ssoClientName).ToList();

                return ReturnSerializedData(CorpFeed);
            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                IList<Online_MI.Models.CorporateFeedbackDetail_Result> CorpFeed = db.CorporateFeedbackDetail(fDate, tDate, client.ssoClientName).ToList();

                return ReturnSerializedData(CorpFeed);
            }


        }

        public void DownloadFeedback(String FromDate, String ToDate)
        {
            ssoClient client = (ssoClient)Session["client"];

            IList<Online_MI.Models.ConferenceFeedbackDetail_Result> confeed;
            IList<Online_MI.Models.CorporateFeedbackDetail_Result> corfeed;

            if (FromDate == null && ToDate == null)
            {
                confeed = db.ConferenceFeedbackDetail(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)), client.ssoClientName).ToList();
                corfeed = db.CorporateFeedbackDetail(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)),client.ssoClientName).ToList();

            }
            else
            {
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", provider);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", provider);

                confeed = db.ConferenceFeedbackDetail(fDate, tDate, client.ssoClientName).ToList();
                corfeed = db.CorporateFeedbackDetail(fDate, tDate, client.ssoClientName).ToList();

            }

            XLWorkbook wb = new XLWorkbook();
            var wsConFeed = wb.AddWorksheet("Conference Feedback");
            var wsCorFeed = wb.AddWorksheet("Corporate Feedback");

            wsConFeed.Cell("A1").Value = "Conference Feedback";
            wsConFeed.Cell("A1").Style.Font.Bold = true;

            wsConFeed.Cell("A3").Value = "NYS Feedback";
            wsConFeed.Cell("A3").Style.Font.Bold = true;
            wsConFeed.Cell("K3").Value = "Venue Feedback";
            wsConFeed.Cell("K3").Style.Font.Bold = true;

            wsConFeed.Cell("A4").Value = "Eqnuiry Reference";
            wsConFeed.Cell("B4").Value = "Date Recevied";
            wsConFeed.Cell("C4").Value = "Initial Enquiry";
            wsConFeed.Cell("D4").Value = "Variety of venues";
            wsConFeed.Cell("E4").Value = "Negotiation of rates";
            wsConFeed.Cell("F4").Value = "Pre-event admin";
            wsConFeed.Cell("G4").Value = "Communication";
            wsConFeed.Cell("H4").Value = "Invoicing Process";
            wsConFeed.Cell("I4").Value = "Quality Checking";
            wsConFeed.Cell("J4").Value = "Comments";

            wsConFeed.Cell("K4").Value = "Efficiency";
            wsConFeed.Cell("L4").Value = "Helpfulness";
            wsConFeed.Cell("M4").Value = "Quality of food";
            wsConFeed.Cell("N4").Value = "Variety of food";
            wsConFeed.Cell("O4").Value = "Cleanliness of rooms";
            wsConFeed.Cell("P4").Value = "Technical facilities";
            wsConFeed.Cell("Q4").Value = "Overall impression";
            wsConFeed.Cell("R4").Value = "Would use again";
            wsConFeed.Cell("S4").Value = "Comments";

            int i = 5;

            foreach (Online_MI.Models.ConferenceFeedbackDetail_Result c in confeed)
            {
                wsConFeed.Cell("A" + i).Value = c.enquirynysref;
                wsConFeed.Cell("B" + i).Value = c.datereceived;
                wsConFeed.Cell("C" + i).Value = c.initialenquiry;
                wsConFeed.Cell("D" + i).Value = c.varietyofvenues;
                wsConFeed.Cell("E" + i).Value = c.negotiationofrates;
                wsConFeed.Cell("F" + i).Value = c.preeventadmin;
                wsConFeed.Cell("G" + i).Value = c.nyscommunication;
                wsConFeed.Cell("H" + i).Value = c.invoicingprocess;
                wsConFeed.Cell("I" + i).Value = c.qualitychecking;
                wsConFeed.Cell("J" + i).Value = c.nyscomments;

                wsConFeed.Cell("K" + i).Value = c.staffefficiency;
                wsConFeed.Cell("L" + i).Value = c.staffhelpfulness;
                wsConFeed.Cell("M" + i).Value = c.qualityoffood;
                wsConFeed.Cell("N" + i).Value = c.varietyoffood;
                wsConFeed.Cell("O" + i).Value = c.cleanlinessofrooms;
                wsConFeed.Cell("P" + i).Value = c.technicalfacilities;
                wsConFeed.Cell("Q" + i).Value = c.overallimpression;
                wsConFeed.Cell("R" + i).Value = c.woulduseagain;
                wsConFeed.Cell("S" + i).Value = c.venuecomments;
                
                i++;
            }

            wsCorFeed.Cell("A1").Value = "Corporate Feedback";
            wsCorFeed.Cell("A1").Style.Font.Bold = true;

            wsCorFeed.Cell("A3").Value = "Contact";
            wsCorFeed.Cell("B3").Value = "Speed of logon";
            wsCorFeed.Cell("C3").Value = "Effectiveness";
            wsCorFeed.Cell("D3").Value = "Efficiency";
            wsCorFeed.Cell("E3").Value = "Availability";
            wsCorFeed.Cell("F3").Value = "Ticket Confirmation Received";
            wsCorFeed.Cell("G3").Value = "Travel Options";
            wsCorFeed.Cell("H3").Value = "Overall Service";
            wsCorFeed.Cell("I3").Value = "Improvements";
            wsCorFeed.Cell("J3").Value = "Other Comments";
            wsCorFeed.Cell("K3").Value = "NYS Investigation";
            wsCorFeed.Cell("L3").Value = "Action Taken";
            wsCorFeed.Cell("M3").Value = "Date Received";

            i = 4;

            foreach (Online_MI.Models.CorporateFeedbackDetail_Result c in corfeed)
            {
                wsCorFeed.Cell("A" + i).Value = c.contact;
                wsCorFeed.Cell("B" + i).Value = c.speedOfLogon;
                wsCorFeed.Cell("C" + i).Value = c.effectiveness;
                wsCorFeed.Cell("D" + i).Value = c.efficiency;
                wsCorFeed.Cell("E" + i).Value = c.availability;
                wsCorFeed.Cell("F" + i).Value = c.ticketConfirmationsReceived;
                wsCorFeed.Cell("G" + i).Value = c.travelOptions;
                wsCorFeed.Cell("H" + i).Value = c.overallService;
                wsCorFeed.Cell("I" + i).Value = c.improvements;
                wsCorFeed.Cell("J" + i).Value = c.otherComments;
                wsCorFeed.Cell("K" + i).Value = c.nysinvestigate;
                wsCorFeed.Cell("L" + i).Value = c.actiontaken;
                wsCorFeed.Cell("M" + i).Value = c.datereceived;

                i++;
            }

            ProduceExcelOutput(wb);


        }

        //public ActionResult GetUOYCompanies()
        //{
        //    var companyList = (from c in db.company where c.groupid == 100042 select new { ID = c.companyid, Name = c.companyname }).ToList();

        //    return Json(companyList, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Logout()
        {
            Session["loggedInUser"] = null;

            return Redirect("/Login");
        }

    }
}