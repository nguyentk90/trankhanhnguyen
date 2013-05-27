using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.Models;
using System.Web.Security;

namespace TraCuuThuatNgu.Controllers
{
    [Authorize]
    public class LikeController : Controller
    {
        //model, work with database
        LikeModel likeModel = new LikeModel();

        //
        // GET: /Like/

        public ActionResult Index(int? page)
        {

            var pageNumber = page ?? 1;
            int size = 10;

            MembershipUser currentUser = Membership.GetUser();
            Guid userId = (Guid)currentUser.ProviderUserKey;

            ViewBag.FavoriteList = likeModel.GetPagedFavorite(pageNumber, size, userId);

            return View();
        }

        //
        // POST: /Like/Add
        [HttpPost]
        public ActionResult Add(string headWord)
        {
            int result = likeModel.Add(headWord);

            if (result == LikeModel.SUCCESS)
            {
                return Json(new { message = "SUCCESS" });
            }
            else if (result == LikeModel.EXISTED)
            {
                return Json(new { message = "EXISTED" });
            }
            else
            {
                return Json(new { message = "FAIL" });
            }
        }


        // Delete a favotite
        [HttpPost]
        public ActionResult Delete(string headWord)
        {
            if (likeModel.Delete(headWord) > 0)
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
