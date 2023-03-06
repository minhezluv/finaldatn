using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]


    public class Required : Attribute
    {
        public Required()
        {

        }
    }

    /// <summary>
    /// Thuộc tính được xuất file excel
    /// </summary>
    /// CreatedBy: NHHoang (27/8/2021)
    public class PropExport : Attribute
    {
        public readonly string Name;
        public PropExport(string name)
        {
            Name = name;
        }
    }
}
