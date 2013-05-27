using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TraCuuThuatNgu.Models;

namespace TraCuuThuatNgu.ViewModels
{
    public class AdminIndexViewModel
    {
       public IQueryable<SearchHistory> SearchHistoriesTopXLastest { get; set; }
    }
}