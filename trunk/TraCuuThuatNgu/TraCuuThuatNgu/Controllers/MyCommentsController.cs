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
    public class MyCommentsController : Controller
    {
        //
        // GET: /MyComments/

        
        public ActionResult Index(int? page)
        {

            var pageNumber = page ?? 1;

            int size = 10;

            MembershipUser currentUser = Membership.GetUser();
            Guid userId = (Guid)currentUser.ProviderUserKey;

            CommentModel commentModel = new CommentModel();
            ViewBag.MyComments = commentModel.GetCommentPagedByUser(pageNumber, size, userId);

            return View();
        }

    }
}
