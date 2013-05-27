using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.Models;
using TraCuuThuatNgu.ViewModels;
using System.Collections;

namespace TraCuuThuatNgu.Controllers
{
    public class ResultController : Controller
    {
        //
        // GET: /Result/

        public ActionResult Index(string keyword)
        {
            // If keyword is null or empty then system will redirect to home page
            if (keyword == null || keyword.Trim().Equals(""))
                return RedirectToAction("Index", "Home");

            // Instance model layer
            ResultModel resultModel = new ResultModel();

            // Instance view model for result
            ResultViewModel resultViewModel = new ResultViewModel();

            // Get entry by keyword
            WordIndex entry = resultModel.GetEntryByKeyword(keyword);

            // Property of search history
            bool isExistData = true;

            // Check has entry
            if (entry != null)
            {
                //set view model
                resultViewModel.Entry = entry;

                // List result
                if (entry.Synsets.Where(x => x.Category == "n").Count() > 0)
                    ViewBag.Noun = entry.Synsets.Where(x => x.Category == "n");
                if (entry.Synsets.Where(x => x.Category == "v").Count() > 0)
                    ViewBag.Verb = entry.Synsets.Where(x => x.Category == "v");

                //add history
                isExistData = true;
            }
            else
            {
                isExistData = false;
            }

            //add history
            SearchHistoryModel searchHistoryModel = new SearchHistoryModel();
            searchHistoryModel.AddSearchHistory(keyword, isExistData);

            //add user history
            if (Request.IsAuthenticated)
            {
                UserHistoryModel userHistoryModel = new UserHistoryModel();
                userHistoryModel.AddUserHistory(keyword);
            }


            //---------------------------------------
            // Store 5 recent keywords
            if (Session["Recent"] == null)
            {
                Session["Recent"] = new List<string>();
            }

            int countRecent = ((List<string>)Session["Recent"]).Count;


            if (((List<string>)Session["Recent"]).Contains(keyword))
            {
                ((List<string>)Session["Recent"]).Remove(keyword);
            }
            else if (countRecent >= 5)
            {
                ((List<string>)Session["Recent"]).RemoveAt(0);
            }
            ((List<string>)Session["Recent"]).Add(keyword);

            //((List<string>)Session["Recent"]).Reverse();
            //string[] arraySuggest = new string[((List<string>)Session["Recent"]).Count];
            //((List<string>)Session["Recent"]).CopyTo(arraySuggest);
            //ViewBag.Recent = arraySuggest;
            //((List<string>)Session["Recent"]).Reverse();
            //---------------------------------------

            //ViewBag.Title
            ViewBag.Title = "Kết quả tra cứu cho '"+keyword+"'";

            // add suggest terms
             EntriesModel entryModel = new EntriesModel();
             resultViewModel.SuggestTerm = entryModel.SuggestTerm(keyword);

            // get questions
             QAModel qaModel = new QAModel();
             resultViewModel.Questions = qaModel.GetQuestionPaged(1, 10);

            return View(resultViewModel);
        }
    }
}
