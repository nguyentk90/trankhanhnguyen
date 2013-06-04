using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.Models;
using System.Web.Security;
using TraCuuThuatNgu.ViewModels;

namespace TraCuuThuatNgu.Controllers
{
   
    public class CommentController : Controller
    {
        //Comment model
        CommentModel commentModel = new CommentModel();


        //
        // GET: /Comment/     
         [Authorize(Roles = "Admin")]
        public ActionResult Index(int? page)
        {
            var pageNumber = page??1;

            int size = 10;

            
                        
            CommentsViewModel viewModel = new CommentsViewModel();
            viewModel.Comments = commentModel.GetCommentPaged(pageNumber, size);

            return View(viewModel);
        }


        //
        // POST: /Add Comment/        
        [HttpPost]
        [Authorize]
        public ActionResult Add(string headWord, string content)
        {
            Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;

            Comment cmt = new Comment();

            cmt.HeadWord = headWord;
            cmt.CmContent = content;
            cmt.DateAdd = DateTime.Now;
            cmt.UserId = userGuid;
            cmt.Reported = 0;

            commentModel.Add(cmt);

            return Redirect("/Result?keyword=" + headWord);
        }

        //
        // POST: /Delete Comment        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int commentId)
        {            
            CommentModel cmModel = new CommentModel();
            if (cmModel.Delete(commentId) > 0)
            {
                return Json(new { message = "SUCCESS" });
            }
            else
            {
                return Json(new { message = "FAIL" });
            }
        }


        //
        // POST: /Report Comment        
        [HttpPost]
        [Authorize]
        public ActionResult Report(int commentId)
        {
            CommentModel cmModel = new CommentModel();
            if (cmModel.Report(commentId) > 0)
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
