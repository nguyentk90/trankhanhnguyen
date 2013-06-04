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
        // Add content Model
        AddContentModel model = new AddContentModel();

        //
        // GET: /AddContent/
        [HttpPost]
        public ActionResult Add(string def, string catagory, string exa, string keyword)
        {
            // Create new UserContent Ojbect
            UserContent rawdata = new UserContent();
            rawdata.UserId = (Guid)Membership.GetUser().ProviderUserKey;
            rawdata.Catagory = !String.IsNullOrEmpty(catagory) ? catagory : "";
            rawdata.Keyword = !String.IsNullOrEmpty(keyword) ? keyword : "";
            rawdata.Def = !String.IsNullOrEmpty(def) ? def : "";
            rawdata.Exa = !String.IsNullOrEmpty(exa) ? exa : "";
            rawdata.DateAdd = DateTime.Now;

            string message = "";

            // Exce model function
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
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            // Exce model function
            ViewBag.AddContentList = model.GetAllAddContentByUser(pageNumber, 10, userId);

            return View();
        }

        // Delete add content
        [HttpPost]
        public ActionResult Delete(int rawDataId)
        {
            int result = model.DeleteAddContent(rawDataId);
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
