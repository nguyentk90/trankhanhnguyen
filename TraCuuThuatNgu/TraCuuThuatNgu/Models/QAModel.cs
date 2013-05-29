using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

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

        // Get total question
        public int ToalQuestions()
        {
            return context.Questions.Count();
        }
    }
}