using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using EntityFramework.Extensions;

namespace TraCuuThuatNgu.Models
{
    public class QAModel
    {
        TraCuuThuatNguEntities context = new TraCuuThuatNguEntities();

        // Add new question
        public int NewQuestion(Question question)
        {
            context.Questions.Add(question);
            return context.SaveChanges();
        }

        // Update question
        public int UpdateQuestion(int questionId)
        {
            Question qs = context.Questions.Find(questionId);

            qs.DateModify = DateTime.Now;

            context.Entry(qs).State = System.Data.EntityState.Modified;

            return context.SaveChanges();
        }

        // Add new Anwser
        public int NewAnswer(Answer answer)
        {
            context.Answers.Add(answer);
            return context.SaveChanges();
        }

        // Get list questions from..to
        public IPagedList<Question> GetQuestionPaged(int page, int size)
        {
            return context.Questions
                .OrderByDescending(x => x.DateAdd).ToPagedList(page, size);                
        }

        // Get list questions order by DateModify DESC
        public IPagedList<Question> GetQuestionOrderByDateModifyPaged(int page, int size)
        {
            return context.Questions
                .OrderByDescending(x => x.DateModify).ToPagedList(page, size);
        }

        // Get list questions that were reported by user --paged
        //public IPagedList<Question> GetQuestionReporedPaged(int page, int size)
        //{
        //    return context.Questions
        //        .Where(x => x.Answers.Where(y => y.Reported >0 )
        //            .FirstOrDefault() != null || x.Reported > 0)
        //            .OrderByDescending(x=>x.DateModify)
        //            .ToPagedList(page,size);
        //}

        // Get total question
        public int ToalQuestions()
        {
            return context.Questions.Count();
        }

        // Report question
        public int ReportQuestion(int questionId)
        {
            Question q = context.Questions.Find(questionId);
            q.Reported++;
            q.DateModify = DateTime.Now;
            context.Entry(q).State = System.Data.EntityState.Modified;

            return context.SaveChanges();
        }

        // Report answer
        public int ReportAnswer(int answerId)
        {            
            Answer a = context.Answers.Find(answerId);
            UpdateQuestion(a.QuestionId);
            a.Reported++;
            context.Entry(a).State = System.Data.EntityState.Modified;

            return context.SaveChanges();
        }


        // Clear report question
        public int ClearReportQuestion(int questionId)
        {
            Question q = context.Questions.Find(questionId);
            q.Reported = 0;            
            context.Entry(q).State = System.Data.EntityState.Modified;
            return context.SaveChanges();
        }

        // Clear report answer
        public int ClearReportAnswer(int answerId)
        {
            Answer a = context.Answers.Find(answerId);           
            a.Reported = 0;
            context.Entry(a).State = System.Data.EntityState.Modified;

            return context.SaveChanges();
        }

        // Delete Question
        public int DeleteQuestion(int questionId)
        {
            Question q = context.Questions.Find(questionId);
            
            // Delete all answers
            context.Answers.Delete(x => x.QuestionId == questionId);

            context.Questions.Remove(q);

            return context.SaveChanges();
        }

        // Delete answer
        public int DeleteAnswer(int answerId)
        {
            context.Answers.Delete(x => x.AnswerId == answerId);
            return context.SaveChanges();
        }
    }
}