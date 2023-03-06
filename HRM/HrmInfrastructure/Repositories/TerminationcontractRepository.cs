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
    public class TerminationcontractRepository : BaseRepository<Terminationcontract>, ITerminationcontractRepository
    {
        public TerminationcontractRepository(IConfiguration config) : base(config)
        { }
    }
}
