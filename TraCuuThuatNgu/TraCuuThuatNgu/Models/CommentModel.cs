using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Data;

namespace TraCuuThuatNgu.Models
{
    public class CommentModel:CommonModel
    {
        // View all comment paging
        public IPagedList<Comment> GetCommentPaged(int page, int size)
        {
            return context.Comments.OrderByDescending(x => x.DateAdd).ToPagedList(page, size);
        }


        // View all comment of special user
        public IPagedList<Comment> GetCommentPagedByUser(int page, int size, Guid UserId)
        {
            return context.Comments.Where(x=>x.UserId==UserId)
                .OrderByDescending(x => x.DateAdd).ToPagedList(page, size);
        }

        // Delete comment by commnetId
        public int Delete(int commentId)
        {
            Comment cm = context.Comments.Find(commentId);
            context.Comments.Remove(cm);
            return context.SaveChanges();
        }


        // Report comment by commnetId
        public int Report(int commentId)
        {
            Comment cm = context.Comments.Find(commentId);
            cm.Reported++;
            context.Entry(cm).State = EntityState.Modified;
            return context.SaveChanges();
        }


        // Add comment
        public int Add(Comment cmt)
        {
            context.Comments.Add(cmt);
            return context.SaveChanges();
        }
    }
}