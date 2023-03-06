using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class LaborContract : BaseEntity
    {
        #region props
        public Guid guid { get; set; } = Guid.NewGuid();
        public string LaborContractCode{ get; set; }
        public Guid StaffID { get; set; }

        public int TypeLaborContractID { get; set; }
        
        public Guid positionID { get; set; }
        
        public Guid DepartmentID { get; set; }
        
        public float BasicSalary { get; set; }
        //public int NbOfPay { get; set; }
        //public float Compensation { get; set; }
        public string HealthICode { get; set; }
        public string SocialICode { get; set; }
        public int status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
     
        public string Note { get; set; }
        //public string CreatedDate { get; set; }
        //public string ModifiedDate { get; set; }
        #endregion
    }
}
