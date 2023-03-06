using Dapper;
using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmInfrastructure.Repositories
{
    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        private string departmentFilter;

        public PositionRepository(IConfiguration config) : base(config)
        { }

        public List<Position> GetByFilterPaging(string positionFilter)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@PositionFilter", positionFilter == "" ? null : positionFilter);
          
            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var staffs = dbConnection.Query<Position>("Proc_GetPositionFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
               // var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                return (List<Position>)staffs;
            }
        }
    }
}
