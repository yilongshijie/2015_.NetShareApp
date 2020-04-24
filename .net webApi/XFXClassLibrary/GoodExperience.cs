//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace XFXClassLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class GoodExperience
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GoodExperience()
        {
            this.GoodExperienceReply = new HashSet<GoodExperienceReply>();
        }
    
        public int GoodExperienceID { get; set; }
        public int UserID { get; set; }
        public int GoodGategoryID { get; set; }
        public int GoodID { get; set; }
        public int State { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Deatil { get; set; }
        public int ReplyNum { get; set; }
        public string Images { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public int OrderBy { get; set; }
    
        public virtual Good Good { get; set; }
        public virtual GoodGategory GoodGategory { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodExperienceReply> GoodExperienceReply { get; set; }
    }
}