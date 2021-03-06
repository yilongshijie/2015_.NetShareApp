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
    
    public partial class CirclePost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CirclePost()
        {
            this.CirclePostLog = new HashSet<CirclePostLog>();
            this.CirclePostReply = new HashSet<CirclePostReply>();
            this.CirclePostReplyChild = new HashSet<CirclePostReplyChild>();
            this.Complaint = new HashSet<Complaint>();
            this.UserLetter = new HashSet<UserLetter>();
        }
    
        public int CirclePostID { get; set; }
        public int CircleTypeID { get; set; }
        public int UserID { get; set; }
        public Nullable<double> CoordX { get; set; }
        public Nullable<double> CoordY { get; set; }
        public string Title { get; set; }
        public string DetailDigest { get; set; }
        public string Detail { get; set; }
        public string ImgList { get; set; }
        public int ReplyNum { get; set; }
        public int Floor { get; set; }
        public int ComplaintNum { get; set; }
        public int ComplaintUntreated { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public int State { get; set; }
        public string Mark { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    
        public virtual CircleType CircleType { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CirclePostLog> CirclePostLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CirclePostReply> CirclePostReply { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CirclePostReplyChild> CirclePostReplyChild { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaint { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLetter> UserLetter { get; set; }
    }
}
