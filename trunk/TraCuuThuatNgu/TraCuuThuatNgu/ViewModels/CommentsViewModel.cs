using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using TraCuuThuatNgu.Models;

namespace TraCuuThuatNgu.ViewModels
{
    public class CommentsViewModel
    {
        public IPagedList<Comment> Comments { get; set; }
    }
}