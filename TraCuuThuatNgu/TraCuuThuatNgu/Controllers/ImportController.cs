using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TraCuuThuatNgu.Models;
using System.Data;

namespace TraCuuThuatNgu.Controllers
{
    public class ImportController : Controller
    {
        //
        // GET: /Import/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            int result = 0;
            // Upload file
            if (file.ContentLength <= 0)
            {
                return Json(new { data = "FAIL", message = "Sai định dạng" });
            }

            var fileName = Path.GetFileName(file.FileName);

            var path = Path.Combine(Server.MapPath("~/Resources"), fileName);

            var checkExtention = Path.GetExtension(path);

            // Check Excel file
            if (checkExtention.Equals(".xls") || checkExtention.Equals(".xlsx"))
            {
                file.SaveAs(path);

                // Do import
                EntriesModel entriesModel = new EntriesModel();

                var source = entriesModel.ReadExcelContents(path);
                result = entriesModel.ImportData(source);

                if (result == -1)
                {
                    return Json(new { data = "FAIL", message = "Cấu trúc file dữ liệu không đúng!" });
                }

                return Json(new { data = "SUCCESS", rows = result });
            }
            else
            {
                return Json(new { data = "FAIL", message = "Sai định dạng" });
            }
        }


    }
}
