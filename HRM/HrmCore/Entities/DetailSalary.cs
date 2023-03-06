using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class DetailSalary
    {
        #region props
        public Guid Guid { get; set; } = Guid.NewGuid();

        public string DetailSalaryCode { get; set; }
        public Guid CoefficientsSalaryID { get; set; }
        public Guid StaffID { get; set; }
        public float Overtime { get; set; }
        public float OvertimeMoney { get; set; }
        public float TotalSalary { get; set; }
        public float TaxMoney { get; set; }
        public float ReduceMoney { get; set; }
        public float BonusMoney { get; set; }
        public float FinalMoney { get; set; }
        public string Note { get; set; }
        #endregion
    }
}
