using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Security;

namespace TraCuuThuatNgu.Models
{
    public class AddContentModel:CommonModel
    {
        //add new rawdata
        public int Add(UserContent rawdata)
        {
            try
            {
                context.UserContents.Add(rawdata);
                return context.SaveChanges();
            }
            catch
            {
                return 0;
            }
        }

        // Get all content by user added and paged
        public IPagedList<UserContent> GetAllAddContent(int page, int pageSize)
        {
            return context.UserContents.OrderByDescending(x => x.DateAdd).ToPagedList(page, pageSize);
        }

        // Get all content by user added and paged by userId
        public IPagedList<UserContent> GetAllAddContentByUser(int page, int pageSize, Guid userId)
        {
            return context.UserContents.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateAdd).ToPagedList(page, pageSize);
        }


        //delete history by historyId       
        public int DeleteAddContent(int rawDataId)
        {
            context.UserContents.Remove(context.UserContents.Find(rawDataId));
            return context.SaveChanges();
        }

    }
}