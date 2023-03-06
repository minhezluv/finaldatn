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
    public class InsuranceRepository : BaseRepository<Insurance>, IInsuranceRepository
    {
        public InsuranceRepository(IConfiguration config) : base(config)
        { }
    }
}
