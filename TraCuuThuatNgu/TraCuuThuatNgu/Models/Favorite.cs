//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TraCuuThuatNgu.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Favorite
    {
        public string HeadWord { get; set; }
        public System.Guid UserId { get; set; }
        public System.DateTime DateAdd { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual WordIndex WordIndex { get; set; }
    }
}
