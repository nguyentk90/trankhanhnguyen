using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.Models;
using TraCuuThuatNgu.ViewModels;
using System.Data;
using System.Text;

namespace TraCuuThuatNgu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageEntriesController : Controller
    {
        //
        // GET: /ManageEntries/

        public ActionResult Index(int? page, string startWith, string keyword)
        {
            EntriesModel entriesModel = new EntriesModel();
            EntriesViewModel viewModel = new EntriesViewModel();

            int size = 5;

            var pageNumber = page ?? 1;

            var startW = String.IsNullOrEmpty(startWith) ? "none" : startWith;

            if (!String.IsNullOrWhiteSpace(keyword))
            {
                viewModel.AllEntries = entriesModel.GetEntriesContainsPaged(pageNumber, size, keyword);
            }
            else
            {
                if (startW.Equals("none"))
                {
                    viewModel.AllEntries = entriesModel.GetEntriesPaged(pageNumber, size);
                }
                else
                {
                    viewModel.AllEntries = entriesModel.GetEntriesByStartWithPaged(pageNumber, size, startW);
                }
            }


            ViewBag.Size = size;

            return View(viewModel);
        }


        //
        // GET: /ManageEntries/Add        
        public ActionResult Add()
        {
            return View();
        }


        //
        // POST: /ManageEntries/Add
        [HttpPost]
        public ActionResult Add(AddTermViewModel term)
        {
            try
            {
                EntriesModel entriesModel = new EntriesModel();
                entriesModel.AddNewTermOrSynset(term);
                ViewBag.Success = "Thêm thuật ngữ thành công!";
            }
            catch
            {
                ViewBag.Fail = "Thêm thuật ngữ không thành công!";
            }
            return View();
        }

        //delete synset
        [Authorize]
        public ActionResult Delete(int synsetId, string headWord)
        {
            try
            {
                EntriesModel entriesModel = new EntriesModel();
                int result = entriesModel.DeleteSynsetOfTerm(headWord, synsetId);
                if (result < 1) return Json(new { message = "FAIL" });
                return Json(new { message = "SUCCESS" });
            }
            catch
            {
                return Json(new { message = "FAIL" });
            }
        }

        //edit synset
        [HttpGet]
        public ActionResult Edit(int? synsetId, string headWord)
        {
            EntriesModel entriesModel = new EntriesModel();

            var synsetId_ = synsetId ?? -1;



            AddTermViewModel editSynset = entriesModel.ViewEditSynset(synsetId_, headWord);
            ViewBag.ListAnotherSynset = entriesModel.GetAnotherSynsetOfTerm(headWord, synsetId_);
            ViewBag.ListSynonyms = entriesModel.GetSynonyms(synsetId_, headWord);
            return View(editSynset);
        }


        // POST: edit synset
        [HttpPost]
        public ActionResult Edit(AddTermViewModel editedSynset, int synsetId)
        {
            EntriesModel entriesModel = new EntriesModel();
            int result = entriesModel.EditSynsetBySynsetId(editedSynset, synsetId);
            ViewBag.Result = result;
            return RedirectToAction("Edit", new { synsetId = synsetId, headWord = editedSynset.HeadWord, r = "success" });
            //return View();
        }


        // POST: Import from Excel file
        [HttpPost]
        public ActionResult Import()
        {
            EntriesModel entriesModel = new EntriesModel();

            string path = Server.MapPath("~/Resources/TestImport1.xls");

            DataTable data = entriesModel.ReadExcelContents(path);

            StringBuilder str = new StringBuilder();

            for (int i = 0; i < data.Rows.Count; i++)
            {
                str.AppendLine(data.Rows[i]["Thuật ngữ"].ToString());
            }
            return Content(str.ToString());

        }
    }
}
