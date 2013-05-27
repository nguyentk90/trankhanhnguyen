using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TraCuuThuatNgu.Models;

namespace TraCuuThuatNgu.ViewModels
{
    public class ManageSearchHistoryViewModel
    {
        public IEnumerable<SearchHistory> GetAllSearchHistory { get; set; }

        public SummarySearchHistoryModel SummarySearchHistoryModel { get; set; }
    }
}