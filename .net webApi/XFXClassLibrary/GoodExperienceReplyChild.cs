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
    
    public partial class GoodExperienceReplyChild
    {
        public int GoodExperienceReplyChildID { get; set; }
        public int GoodExperienceReplyID { get; set; }
        public int GoodID { get; set; }
        public int InitiativeUserID { get; set; }
        public int PassivityUserID { get; set; }
        public string ImgList { get; set; }
        public string Detail { get; set; }
        public int State { get; set; }
        public System.DateTime CreateTime { get; set; }
    
        public virtual Good Good { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
