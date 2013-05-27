using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TraCuuThuatNgu.Models;
using PagedList;

namespace TraCuuThuatNgu.ViewModels
{
    public class AddContentViewModel
    {
        public IPagedList<UserContent> UserContents { get; set; }
    }
}