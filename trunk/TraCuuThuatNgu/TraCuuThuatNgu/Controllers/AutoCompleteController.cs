using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.Models;
using System.Web.Script.Serialization;



namespace TraCuuThuatNgu.Controllers
{
    public class AutoCompleteController : Controller
    {
        //
        // GET: /AutoComplete/
        [HttpGet]
        public ActionResult Index()
        {
            return View();          
        }

        //
        // POST: /AutoComplete/
        [HttpPost]
        public ActionResult Index(string prefix)
        {
            //get model
            AutoCompleteModel autoCompleteModel = new AutoCompleteModel();   
            return Json(autoCompleteModel.GetByPrefix(prefix), JsonRequestBehavior.DenyGet);
        }

    }
}
