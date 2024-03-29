﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using TraCuuThuatNgu.Models;
using PagedList;

namespace TraCuuThuatNgu.ViewModels
{
    public class CommonViewModel
    {
        public CommonViewModel()
        {
            LogOnModel = new LogOnModel();
        }
        
        public LogOnModel LogOnModel { get; set; }

        public IPagedList<Question> Questions { get; set; }
       
    }
}