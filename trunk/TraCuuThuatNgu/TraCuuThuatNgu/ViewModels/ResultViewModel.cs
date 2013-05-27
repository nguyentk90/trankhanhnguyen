using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TraCuuThuatNgu.Models;

namespace TraCuuThuatNgu.ViewModels
{
    public class ResultViewModel : CommonViewModel
    {
        public WordIndex Entry { get; set; }
        public IEnumerable<WordIndex> SuggestTerm { get; set; }
    }
}