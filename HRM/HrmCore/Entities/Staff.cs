using HrmCore.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class Staff : BaseEntity
    {
        #region props
        public Guid guid { get; set; } = Guid.NewGuid();
        [PropExport(("Mã nhân viên"))]
        public string StaffCode { get; set; }
        [PropExport(("Tên nhân viên"))]
        public string StaffName { get; set; }
        [PropExport(("Ngày sinh"))]
        public DateTime DateOfBirth { get; set; }
        [PropExport(("Giới tính"))]
        public int Gender { get; set; }
        [PropExport(("Quê quán"))]
        public string HomeTown { get; set; }
        //[PropExport(("Dân tộc"))]
        //public string Ethnic { get; set; }
        //[PropExport(("Tôn giáo"))]
        //public string Religion { get; set; }
        //[PropExport(("Quốc tịch"))]
        //public string Nationality { get; set; }
        [PropExport("Địa chỉ")]
        public string Address { get; set; }
        [PropExport(("Số điện thoại"))]
        public string PhoneNumber { get; set; }
        [PropExport(("Email"))]
        public string Email { get; set; }
        [PropExport(("Cmt/Cccd"))]
        public string IdentityCard { get; set; }
        [PropExport(("Nơi cấp"))]
        public string IDCardPlace { get; set; }
        [PropExport(("Ngày cấp"))]
        public DateTime identityDate { get; set; }
        //[PropExport(("Ngày bắt đầu công việc"))]
        //public DateTime StartDate { get; set; }
        //[PropExport(("Ngày kết thúc công việc"))]
        //public DateTime EndDate { get; set; }
        //[PropExport(("Ghi chú"))]
        //public string Note { get; set; }
        //[PropExport(("Người giám sát"))]
        //public string FollowerGuid { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        //public DateTime CreatedDate { get; set; }

        //public DateTime ModifiedDate { get; set; }
        #endregion
    }
}
