using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using PagedList;


namespace TraCuuThuatNgu.Models
{
    public class LikeModel
    {
        //DbContext
        TraCuuThuatNguEntities context = null;

        public static int SUCCESS = 1;
        public static int FAIL = 0;
        public static int EXISTED = 2;

        public LikeModel()
        {
            context = new TraCuuThuatNguEntities(); ;
        }


        //get paged like
        public IPagedList<Favorite> GetPagedFavorite(int page, int size, Guid userId)
        {            
            return context.Favorites.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateAdd).ToPagedList(page, size);
        }



        //add a like
        public int Add(string headWord)
        {
            try
            {
                Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                //check have liked
                if (context.Favorites.Find(headWord, userId) != null)
                {
                    return LikeModel.EXISTED;
                }
                else
                {
                    Favorite favorite = new Favorite();
                    favorite.HeadWord = headWord;
                    favorite.UserId = userId;
                    favorite.DateAdd = DateTime.Now;

                    context.Favorites.Add(favorite);
                    context.SaveChanges();

                    return LikeModel.SUCCESS;
                }
            }
            catch
            {
                return LikeModel.FAIL;
            }

        }



        //delete a like
        public int Delete(string headWord)
        {
            try
            {
                Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                //check have liked
                if (context.Favorites.Find(headWord, userId) != null)
                {
                    Favorite favorite = context.Favorites.Find(headWord, userId);
                    context.Favorites.Remove(favorite);
                    return context.SaveChanges();
                }
                else
                {
                    return 0;
                }
                
            }
            catch
            {
                return 0;
            }
        }
    }
}