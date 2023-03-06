using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class Insurance
    {
        #region props
        public Guid Guid { get; set; } = Guid.NewGuid();

        public int InsuranceType { get; set; }
        public Guid StaffID { get; set; }
        public string StaffCode { get; set; }
        public string StaffName { get; set; }
        public Guid LaborContractID { get; set; }
        public string LaborContractCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float EnteprisePay { get; set; }
        public float StaffPay { get; set; }
        #endregion
    }
}
