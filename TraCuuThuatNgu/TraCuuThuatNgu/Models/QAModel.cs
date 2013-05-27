using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public IEnumerable<Question> GetQuestionPaged(int page, int size)
        {
            return context.Questions
                .OrderByDescending(x => x.DateAdd)
                .Skip((page - 1) * size)
                .Take(size);
        }
    }
}