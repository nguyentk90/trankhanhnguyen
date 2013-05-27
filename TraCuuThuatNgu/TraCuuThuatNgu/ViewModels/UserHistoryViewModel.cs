using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TraCuuThuatNgu.Models;

namespace TraCuuThuatNgu.ViewModels
{
    public class UserHistoryViewModel
    {
        public IEnumerable<UserHistory> AllUserHistory { get; set; }
        public IEnumerable<UserHistory> AllUserHistoryPaged { get; set; }
    }
}