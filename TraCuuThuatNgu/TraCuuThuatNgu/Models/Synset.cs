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
    
    public partial class Synset
    {
        public Synset()
        {
            this.WordIndexes = new HashSet<WordIndex>();
        }
    
        public int SynsetId { get; set; }
        public string Category { get; set; }
        public string Def { get; set; }
        public string Exa { get; set; }
    
        public virtual ICollection<WordIndex> WordIndexes { get; set; }
    }
}
