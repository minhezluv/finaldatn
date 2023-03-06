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
    public class LaborContractRepository : BaseRepository<LaborContract>, ILaborContractRepository
    {
        public LaborContractRepository(IConfiguration config) : base(config)
        { }

        public bool CheckLCCodeExists(string lcCode, string lcID)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@LaborContractCode", lcCode);
           // dynamicParameters.Add("@StaffID", lcID);
            dynamicParameters.Add("@IsExist", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                dbConnection.Execute("Proc_CheckLCCodeExists", dynamicParameters, commandType: CommandType.StoredProcedure);
                bool isExists = dynamicParameters.Get<Boolean>("@IsExist");
                return isExists;
            }
        }

        public List<FullLaborContract> GetByFilterPaging(string LCFilter, int pageSize, int pageIndex, string departmentID, string positionID, int Status, int TypeLaborContractID)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@LCFilter", LCFilter == "" ? null : LCFilter);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@DepartmentID", departmentID == "" ? null : departmentID);
            parameters.Add("@PositionID", positionID == "" ? null : positionID);
            parameters.Add("@Status", Status);
            parameters.Add("@TypeLaborContractID", TypeLaborContractID );
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var laborContracts = dbConnection.Query<FullLaborContract>("Proc_GetLCFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                return (List<FullLaborContract>) laborContracts;
            }
        }

        public InfoPage GetInfoPage(string LCFilter, int pageSize, int pageIndex, string departmentID, string positionID, int Status, int TypeLaborContractID)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@LCFilter", LCFilter == "" ? null : LCFilter);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@DepartmentID", departmentID == "" ? null : departmentID);
            parameters.Add("@PositionID", positionID == "" ? null : positionID);
            parameters.Add("@Status", Status);
            parameters.Add("@TypeLaborContractID", TypeLaborContractID );
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var staffs = dbConnection.Query<FullStaff>("Proc_GetLCFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                data.TotalPage = (int)Math.Ceiling((float)(data.TotalRecord / pageSize))+1;
                return data;
            }
        }

        public string GetLastLCCode()
        {
            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var staffLastCode = dbConnection.QueryFirstOrDefault<String>("Proc_GetLastLCCode", commandType: CommandType.StoredProcedure);

                return staffLastCode;
            }
        }

        public int Import(List<LaborContract> laborContracts)
        {
            int rowAffects = 0;
            int cnt = 0;
            foreach (var s in laborContracts)
            {
                rowAffects = _dbConnection.Execute($"Proc_InsertLC", s, commandType: CommandType.StoredProcedure);
                cnt++;
            }
            //Insert


            return cnt;
        }

        public int InsertLC(LaborContract LC)
        {
            int rowAffects = 0;
            int cnt = 0;

            rowAffects = _dbConnection.Execute($"Proc_InsertLC", LC, commandType: CommandType.StoredProcedure);
            cnt++;

            //Insert


            return rowAffects;
        }
    }
}
