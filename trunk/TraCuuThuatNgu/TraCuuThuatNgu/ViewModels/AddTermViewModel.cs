using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TraCuuThuatNgu.Models;
using System.ComponentModel.DataAnnotations;

namespace TraCuuThuatNgu.ViewModels
{
    public class AddTermViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập thuật ngữ.")]
        public string HeadWord { get; set; }
        public string Catagory { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập giải thích.")]
        public string Def { get; set; }
        public string Exa { get; set; }
        public string Synonyms { get; set; }
    }
}