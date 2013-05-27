using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using PagedList;

namespace TraCuuThuatNgu.Models
{
    public class UserHistoryModel : CommonModel
    {
        // Add user history
        public int AddUserHistory(string keyword)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;

            UserHistory userHistory = new UserHistory();
            userHistory.Keyword = keyword;
            userHistory.UserId = userId;
            userHistory.DateAdd = DateTime.Now;
            context.UserHistories.Add(userHistory);
            return context.SaveChanges();
        }


        // Get all history by userid and paged
        public IPagedList<UserHistory> GetAllUserHistoryPagedByUser(int page,int pageSize, Guid userId)
        {            
            return context.UserHistories.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateAdd).ToPagedList(page, pageSize);
        }


        // Delete history by historyId       
        public int DeleteUserHistory(int historyId)
        {
            context.UserHistories.Remove(context.UserHistories.Find(historyId));
            return context.SaveChanges();
        }

    }
}