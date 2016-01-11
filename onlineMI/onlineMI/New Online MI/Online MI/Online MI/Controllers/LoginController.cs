using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_MI.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            Session["loggedInUser"] = false;
            return View();
        }

        public ActionResult Login(string username, string password)
        {
            ViewBag.Failed = "";

            if (username != "" && password != "")
            {
                //Attempt login
                if (username == System.Configuration.ConfigurationManager.AppSettings["TempUN"].ToString() && password == System.Configuration.ConfigurationManager.AppSettings["TempPW"].ToString())
                {
                    Session["loggedInUser"] = true;

                    return Redirect("/Home");
                }
                else
                {
                    Session["loggedInUser"] = false;
                    ViewBag.Failed = "Credentials not matched please try again";
                    return View("Index");
                }
            }
            else
            {
                //No details
                Session["loggedInUser"] = false;
                ViewBag.Failed = "Credentials not matched please try again";
                return View("Index");
            }
        }
    }
}