using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.Models;
using TraCuuThuatNgu.ViewModels;
using PagedList;
using System.Web.Security;

namespace TraCuuThuatNgu.Controllers
{
    public class UserHistoryController : Controller
    {
        //
        // GET: /UserHistory/
        [Authorize]
        public ActionResult Index(int? page)
        {
            UserHistoryModel userHistoryModel = new UserHistoryModel();
            UserHistoryViewModel viewModel = new UserHistoryViewModel();
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;

            viewModel.AllUserHistoryPaged = userHistoryModel.GetAllUserHistoryPagedByUser(pageNumber, 10, userId);
            return View(viewModel);
        }


        //delete history
        [HttpPost]
        public ActionResult Delete(int historyId)
        {
            UserHistoryModel userHistoryModel = new UserHistoryModel();
            int result = userHistoryModel.DeleteUserHistory(historyId);
            if (result > 0)
            {
                return Json(new { message = "SUCCESS" });
            }
            else
            {
                return Json(new { message = "FAIL" });
            }
        }
    }
}
