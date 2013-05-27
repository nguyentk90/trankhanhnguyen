using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraCuuThuatNgu.Models
{
    public class CommonModel
    {
        protected TraCuuThuatNguEntities context = null;

        public CommonModel()
        {
            context = new TraCuuThuatNguEntities();
        }
    }
}