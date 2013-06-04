using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Security;
using System.Diagnostics;

namespace TraCuuThuatNgu.Models
{
    public class AddContentModel : CommonModel
    {
        // ERROR: Return -1 (Insert/Update/Delete)
        // ERROR: Return NULL (Select)


        // ---
        // Add new rawdata - content was contributed from user
        public int Add(UserContent rawdata)
        {
            try
            {
                context.UserContents.Add(rawdata);
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Write error to debug windows
                Debug.WriteLine(ex.ToString());

                return -1;
            }
        }

        // ---
        // Get all contents that users added and paged
        public IPagedList<UserContent> GetAllAddContent(int page, int pageSize)
        {
            try
            {
                return context.UserContents.OrderByDescending(x => x.DateAdd).ToPagedList(page, pageSize);
            }
            catch (Exception ex)
            {
                // Write error to debug windows
                Debug.WriteLine(ex.ToString());

                return null;
            }
        }


        // ---
        // Get all contents that special user added and paged (by userId)
        public IPagedList<UserContent> GetAllAddContentByUser(int page, int pageSize, Guid userId)
        {
            try
            {
                return context.UserContents.Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.DateAdd).ToPagedList(page, pageSize);
            }
            catch (Exception ex)
            {
                // Write error to debug windows
                Debug.WriteLine(ex.ToString());

                return null;
            }
        }


        // --
        // Delete content by contentId
        public int DeleteAddContent(int rawDataId)
        {
            try
            {
                context.UserContents.Remove(context.UserContents.Find(rawDataId));
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Write error to debug windows
                Debug.WriteLine(ex.ToString());

                return -1;
            }
        }

    }
}