using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using TraCuuThuatNgu.Models;

namespace TraCuuThuatNgu.ViewModels
{
    public class UsersViewModel
    {
        public IPagedList<aspnet_Users> Users { get; set; }
    }
}