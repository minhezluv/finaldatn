using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class Tax
    {
        #region props
        public Guid guid { get; set; } = Guid.NewGuid();

        public string TaxCode { get; set; }
        public float From { get; set; }
        public float To { get; set; }
        #endregion
    }
}
