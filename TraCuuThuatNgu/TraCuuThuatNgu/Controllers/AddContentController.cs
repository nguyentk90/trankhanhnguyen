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
    public class AddContentController : Controller
    {
        //
        // GET: /AddContent/
        [HttpPost]
        public ActionResult Add(string def, string catagory, string exa, string keyword)
        {
            UserContent rawdata = new UserContent();
            rawdata.UserId = (Guid)Membership.GetUser().ProviderUserKey;
            rawdata.Catagory = !String.IsNullOrEmpty(catagory) ? catagory : "";
            rawdata.Keyword = !String.IsNullOrEmpty(keyword) ? keyword : "";
            rawdata.Def = !String.IsNullOrEmpty(def) ? def : "";
            rawdata.Exa = !String.IsNullOrEmpty(exa) ? exa : "";
            rawdata.DateAdd = DateTime.Now;

            string message = "";

            AddContentModel model = new AddContentModel();
            if (model.Add(rawdata) > 0)
            {
                message = "SUCCESS";
            }
            else
            {
                message = "FAIL";
            }

            return Json(new { message = message });
        }



        //GET: /AddContent/List
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            AddContentModel model = new AddContentModel();

            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;

            ViewBag.AddContentList = model.GetAllAddContentByUser(pageNumber, 10, userId);

            return View();
        }

        //delete add content
        [HttpPost]
        public ActionResult Delete(int rawDataId)
        {
            AddContentModel addContentModel = new AddContentModel();
            int result = addContentModel.DeleteAddContent(rawDataId);
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
