using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace TraCuuThuatNgu.Controllers
{
    public class TestChatController : Controller
    {
        //
        // GET: /TestChat/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string param)
        {
            QAModel qaModel = new QAModel();
            var list = qaModel.GetQuestionPaged(1, 10).ToList();

            List<QuestionC> question = new List<QuestionC>();

            foreach (var item in list)
            {
                QuestionC q = new QuestionC();
                q.QContent = item.QContent;
                foreach (var i in item.Answers)
                {
                    AnswerC a = new AnswerC();
                    a.AContent = i.AContent;
                    a.Qid = i.QuestionId;
                    q.AList.Add(a);
                }
                question.Add(q);
            }
            return Json(question, JsonRequestBehavior.AllowGet);
        }
    }

    public class QuestionC
    {
        public QuestionC()
        {
            AList = new List<AnswerC>();
        }
        public string QContent { get; set; }
        public List<AnswerC> AList { get; set; }
    }

    public class AnswerC
    {
        public string AContent { get; set; }
        public int Qid { get; set; }
    }
}
