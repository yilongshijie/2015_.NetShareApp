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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.CircleManage = new HashSet<CircleManage>();
            this.CirclePost = new HashSet<CirclePost>();
            this.CirclePostLog = new HashSet<CirclePostLog>();
            this.CirclePostReply = new HashSet<CirclePostReply>();
            this.CirclePostReplyChild = new HashSet<CirclePostReplyChild>();
            this.CirclePostReplyChild1 = new HashSet<CirclePostReplyChild>();
            this.Complaint = new HashSet<Complaint>();
            this.Complaint1 = new HashSet<Complaint>();
            this.GoodCart = new HashSet<GoodCart>();
            this.GoodCollection = new HashSet<GoodCollection>();
            this.GoodEvaluate = new HashSet<GoodEvaluate>();
            this.GoodExperience = new HashSet<GoodExperience>();
            this.GoodExperienceReply = new HashSet<GoodExperienceReply>();
            this.GoodExperienceReplyChild = new HashSet<GoodExperienceReplyChild>();
            this.GoodExperienceReplyChild1 = new HashSet<GoodExperienceReplyChild>();
            this.Order = new HashSet<Order>();
            this.UserAddress = new HashSet<UserAddress>();
            this.UserBlacklist = new HashSet<UserBlacklist>();
            this.UserBlacklist1 = new HashSet<UserBlacklist>();
            this.UserLog = new HashSet<UserLog>();
            this.UserLog1 = new HashSet<UserLog>();
            this.UserLetter = new HashSet<UserLetter>();
            this.UserLetter1 = new HashSet<UserLetter>();
        }
    
        public int UserID { get; set; }
        public string Tel { get; set; }
        public string NickName { get; set; }
        public string PassWord { get; set; }
        public string Gender { get; set; }
        public string Married { get; set; }
        public string SexualOrientation { get; set; }
        public string Age { get; set; }
        public string Location { get; set; }
        public string Constellation { get; set; }
        public string HeadPortrait { get; set; }
        public int Type { get; set; }
        public int State { get; set; }
        public System.DateTime CreatTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CircleManage> CircleManage { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CirclePost> CirclePost { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CirclePostLog> CirclePostLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CirclePostReply> CirclePostReply { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CirclePostReplyChild> CirclePostReplyChild { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CirclePostReplyChild> CirclePostReplyChild1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaint { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaint1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodCart> GoodCart { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodCollection> GoodCollection { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodEvaluate> GoodEvaluate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodExperience> GoodExperience { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodExperienceReply> GoodExperienceReply { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodExperienceReplyChild> GoodExperienceReplyChild { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodExperienceReplyChild> GoodExperienceReplyChild1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserAddress> UserAddress { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserBlacklist> UserBlacklist { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserBlacklist> UserBlacklist1 { get; set; }
        public virtual UserExtend UserExtend { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLog> UserLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLog> UserLog1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLetter> UserLetter { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLetter> UserLetter1 { get; set; }
    }
}