using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.Models;
using TraCuuThuatNgu.ViewModels;

namespace TraCuuThuatNgu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageSearchHistoryController : Controller
    {

        SearchHistoryModel searchHistoryModel = new SearchHistoryModel();

        //
        // GET: /ManageSearch/

        public ActionResult Index(int? page, int? orderBy)
        {
            //view models for search history
            ManageSearchHistoryViewModel managesearchHistoryModel = new ManageSearchHistoryViewModel();


            var pageNumber = page ?? 1;

            var order = orderBy ?? 0;

            ViewBag.OrderBy = order;

            //search history list
            managesearchHistoryModel.GetAllSearchHistory = searchHistoryModel.GetAllSearchHistory(pageNumber,15,order);
            
            //summary search history 
            managesearchHistoryModel.SummarySearchHistoryModel = new SummarySearchHistoryModel();

            return View(managesearchHistoryModel);
        }

    }
}
