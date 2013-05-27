using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.ViewModels;
using TraCuuThuatNgu.Models;

namespace TraCuuThuatNgu.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            //3-5-2013
            EntriesModel entriesModel = new EntriesModel();
            ViewBag.ListWords = entriesModel.SuggestTerm("chinh phu");
            
            return View(new CommonViewModel());
        }

        //
        // GET: /Test/
        [HttpPost]
        public ActionResult Index(CommonViewModel cm)
        {
            return View(cm);
        }

    }
}
