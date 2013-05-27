using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace TraCuuThuatNgu.Models
{
    public class ProfileModel:CommonModel
    {
        //get profile
        public Profile GetProfileByUserId(Guid userId)
        {
            return context.Profiles.Find(userId);
        }

        //update profile
        public int UpdateProfile(Profile profile)
        {
            context.Entry(profile).State = EntityState.Modified;
            return context.SaveChanges();
        }
    }
}