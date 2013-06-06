using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraCuuThuatNgu.Models;
using System.Diagnostics;

namespace TraCuuThuatNgu.Controllers
{
    public class QAController : Controller
    {
        //
        // GET: /QA/

        public ActionResult Index()
        {
            // get questions
            QAModel qaModel = new QAModel();            
            return View(qaModel.GetQuestionPaged(1, 5));
        }

        [HttpPost]
        public ActionResult Index(int limit)
        {
            QAModel qaModel = new QAModel();

            int total = qaModel.ToalQuestions();
            int size = 5;

            int pages = (int)Math.Ceiling(total / (double)size);


            if (limit <= pages)
            {
                var list = qaModel.GetQuestionPaged(limit, size);

                List<QuestionJSON> question = new List<QuestionJSON>();

                foreach (var item in list)
                {
                    QuestionJSON q = new QuestionJSON();
                    q.QContent = item.QContent;
                    q.Qid = item.QuestionId;
                    q.Username = item.aspnet_Users.UserName;
                    q.DateAdd = String.Format("{0:d/M/yyyy HH:mm}", item.DateAdd);
                    foreach (var i in item.Answers)
                    {
                        AnswerJSON a = new AnswerJSON();
                        a.AContent = i.AContent;
                        a.Qid = i.QuestionId;
                        a.Username = i.aspnet_Users.UserName;
                        a.DateAdd = String.Format("{0:d/M/yyyy HH:mm}", i.DateAdd);
                        q.AList.Add(a);
                    }
                    question.Add(q);
                }
                question.Reverse();
                return Json(question);
            }
            else
            {
                return Json(new { message = "full" });
            }
        }

    }

    // Extra classes
    public class QuestionJSON
    {
        public QuestionJSON()
        {
            AList = new List<AnswerJSON>();
        }
        public int Qid { get; set; }
        public string Username { get; set; }
        public string QContent { get; set; }
        public string DateAdd { get; set; }
        public List<AnswerJSON> AList { get; set; }
    }

    public class AnswerJSON
    {
        public string Username { get; set; }
        public string AContent { get; set; }
        public string DateAdd { get; set; }
        public int Qid { get; set; }
    }
}
