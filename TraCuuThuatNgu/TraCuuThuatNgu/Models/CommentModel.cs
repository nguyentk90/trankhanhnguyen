using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Data;
using System.Diagnostics;

namespace TraCuuThuatNgu.Models
{
    public class CommentModel : CommonModel
    {

        // ERROR: Return -1 (Insert/Update/Delete)
        // ERROR: Return NULL (Select)

        // View all comment paging
        public IPagedList<Comment> GetCommentPaged(int page, int size)
        {
            return context.Comments.OrderByDescending(x => x.DateAdd).ToPagedList(page, size);
        }


        // View all comment of special user
        public IPagedList<Comment> GetCommentPagedByUser(int page, int size, Guid UserId)
        {
            return context.Comments.Where(x => x.UserId == UserId)
                .OrderByDescending(x => x.DateAdd).ToPagedList(page, size);
        }

        // Delete comment by commnetId
        public int Delete(int commentId)
        {
            try
            {
                Comment cm = context.Comments.Find(commentId);
                context.Comments.Remove(cm);
                return context.SaveChanges();
            }
            catch (EntitySqlException ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }
        }


        // Report comment by commnetId
        public int Report(int commentId)
        {
            try
            {
                Comment cm = context.Comments.Find(commentId);
                cm.Reported++;
                context.Entry(cm).State = EntityState.Modified;
                return context.SaveChanges();
            }
            catch (EntitySqlException ex)
            {
                Debug.WriteLine(ex.ToString());

                return -1;
            }
        }


        // Add comment
        public int Add(Comment cmt)
        {

            try
            {
                context.Comments.Add(cmt);
                return context.SaveChanges();
            }
            catch (EntitySqlException ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }

        }
    }
}