using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraCuuThuatNgu.Models
{
    public class ResultModel : CommonModel
    {
        //get entry by keyword
        public WordIndex GetEntryByKeyword(string keyword)
        {
            return context.WordIndexes.Find(keyword);           
        }
    }
}