using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.ViewModels;
using TraCuuThuatNgu.Models;

namespace TraCuuThuatNgu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/        
        public ActionResult Index()
        {
            //instance search history model
            SearchHistoryModel searchHistoryModel = new SearchHistoryModel();


            AdminIndexViewModel adminIndexViewModel = new AdminIndexViewModel();
            adminIndexViewModel.SearchHistoriesTopXLastest = searchHistoryModel.GetTopLastest(5);

            return View(adminIndexViewModel);
        }
            

    }
}
