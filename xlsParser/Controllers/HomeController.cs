using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xlsParser.Models.Context;

namespace xlsParser.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new DataBaseContext())
            {
                return View();
            }
        }
    }
}