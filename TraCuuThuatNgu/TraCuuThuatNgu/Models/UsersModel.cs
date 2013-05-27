using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using EntityFramework.Extensions;

namespace TraCuuThuatNgu.Models
{
    public class UsersModel : CommonModel
    {

        // Get all user
        public IPagedList<aspnet_Users> GetAllUserPaged(int page, int size)
        {
            return context.aspnet_Users.OrderBy(x => x.UserName).ToPagedList(page, size);
        }

        // Get all user contains keyword
        public IPagedList<aspnet_Users> GetAllUserPagedContainsKeyword(int page, int size, string keyword)
        {
            return context.aspnet_Users.Where(x => x.UserName.Contains(keyword)).OrderBy(x => x.UserName).ToPagedList(page, size);
        }


        // Get user by username
        public aspnet_Users GetUserByUsername(string username)
        {
            return context.aspnet_Users.Where(x => x.UserName == username).FirstOrDefault();
        }

        // Delete User
        public int Delete(Guid userId)
        {
            aspnet_Users user = context.aspnet_Users.Find(userId);

            // Delete profile
            context.Profiles.Remove(context.Profiles.Where(x => x.UserId == userId).FirstOrDefault());           
            // Delete Membership
            context.aspnet_Membership.Remove(context.aspnet_Membership.Where(x => x.UserId == userId).FirstOrDefault());           
            // Delete Roles
            user.aspnet_Roles.Clear();

            // Delete add Foreign key
          
            context.UserHistories.Delete(x => x.UserId == userId);
            
            context.UserContents.Delete(x => x.UserId == userId);
            context.Comments.Delete(x => x.UserId == userId);
            context.Favorites.Delete(x => x.UserId == userId);
            context.Questions.Delete(x => x.UserId == userId);
            context.Answers.Delete(x => x.UserId == userId);
           
         
            // Delete user
            context.aspnet_Users.Remove(user);
            return context.SaveChanges();
        }
    }
}