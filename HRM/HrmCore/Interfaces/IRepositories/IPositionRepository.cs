using HrmCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Interfaces.IRepositories
{
    public interface IPositionRepository : IBaseRepository<Position>
    {
        List<Position> GetByFilterPaging(string positionFilter);
    }
}
