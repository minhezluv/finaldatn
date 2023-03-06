using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmInfrastructure.Repositories
{
    public class CoefficientssalaryRepository : BaseRepository<Coefficientssalary>, ICoefficientssalaryRepository
    {
        public CoefficientssalaryRepository(IConfiguration config) : base(config)
        { }
    }
}
