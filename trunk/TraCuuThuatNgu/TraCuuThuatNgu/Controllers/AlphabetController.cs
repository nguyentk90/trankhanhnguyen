using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.Models;
using TraCuuThuatNgu.ViewModels;

namespace TraCuuThuatNgu.Controllers
{
    public class AlphabetController : Controller
    {
        //
        // GET: /Aphabet/

        public ActionResult Index(int? page, string startWith)
        {
            EntriesModel entriesModel = new EntriesModel();
            EntriesViewModel viewModel = new EntriesViewModel();

            int size = 5;

            var pageNumber = page ?? 1;

            var startW = String.IsNullOrEmpty(startWith) ? "none" : startWith;


            if (startW.Equals("none"))
            {
                viewModel.AllEntries = entriesModel.GetEntriesPaged(pageNumber, size);
                ViewBag.Title = "Danh sách thuật ngữ";
            }
            else
            {
                viewModel.AllEntries = entriesModel.GetEntriesByStartWithPaged(pageNumber, size, startW);
                ViewBag.Title = "Danh sách thuật ngữ bắt đầu với \""+startW+"\"";
            }


            ViewBag.Size = size;

           

            return View(viewModel);
        }

    }
}
