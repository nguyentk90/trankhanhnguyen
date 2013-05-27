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
    public class ManageContentController : Controller
    {
        //
        // GET: /ManageContent/

        public ActionResult Index(int? page)
        {

            var pageNumber = page ?? 1;
            int size = 10;

            AddContentModel addContentModel = new AddContentModel();

            AddContentViewModel viewModel = new AddContentViewModel();
            viewModel.UserContents = addContentModel.GetAllAddContent(pageNumber, size);

            return View(viewModel);
        }

    }
}
