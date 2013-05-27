using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TraCuuThuatNgu.ViewModels
{
    public class InfoViewModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Email không được bỏ trống.")]      
        [Display(Name = "Email")]
        public string Email { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }

    }
}