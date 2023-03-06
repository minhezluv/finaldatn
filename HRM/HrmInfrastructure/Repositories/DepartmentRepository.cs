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
    public class DepartmentRepository: BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IConfiguration config) : base(config)
        { }

        public List<Department> GetByFilterPaging(string departmentFilter)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@departmentFilter", departmentFilter == "" ? null : departmentFilter);
            parameters.Add("@TotalRecord",
                           dbType: DbType.Int32,
                           direction: ParameterDirection.Output);

            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var staffs = dbConnection.Query<Department>("Proc_GetDepartmentFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                return (List<Department>)staffs;
            }
        }
    }
}
