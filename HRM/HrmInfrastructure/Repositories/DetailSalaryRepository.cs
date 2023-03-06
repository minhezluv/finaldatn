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
    public class DetailSalaryRepository : BaseRepository<DetailSalary>, IDetailSalaryRepository
    {
        public DetailSalaryRepository(IConfiguration config) : base(config)
        { }
    }
}
