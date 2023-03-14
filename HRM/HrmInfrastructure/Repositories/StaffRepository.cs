using Dapper;
using HrmCore.Entities;
using HrmCore.Entities.Excel;
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
    public class StaffRepository : BaseRepository<Staff>, IStaffRepository
    {
        public StaffRepository(IConfiguration config) : base(config)
        {
            
        }

        public bool CheckEmployeeCodeExists(string staffCode, string staffID)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@StaffCode", staffCode);
            dynamicParameters.Add("@StaffID", staffID);
            dynamicParameters.Add("@IsExist", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                dbConnection.Execute("Proc_CheckStaffCodeExists", dynamicParameters, commandType: CommandType.StoredProcedure);
                bool isExists = dynamicParameters.Get<Boolean>("@IsExist");
                return isExists;
            }
        }

        public List<FullStaff> GetByFilterPaging(string employeeFilter, int pageSize, int pageIndex, string departmentID, string positionID,int status)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@StaffFilter", employeeFilter == "" ? null : employeeFilter);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@DepartmentID", departmentID == "" ? null : departmentID);
            parameters.Add("@PositionID", positionID == "" ? null : positionID);
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Status", status == 0 ? null: status);

            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var staffs = dbConnection.Query<FullStaff>("Proc_GetStaffFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                return (List<FullStaff>)staffs;
            }
        }

        public IEnumerable<StaffColumn> GetEmployeeExportColumns()
        {
            return _dbConnection.Query<StaffColumn>("Proc_GetStaff", commandType: CommandType.StoredProcedure);
        }
        //public InfoPage GetInfoPage(string employeeFilter, int pageSize, int pageIndex)
        //{
        //    var parameters = new DynamicParameters();

        //    parameters.Add("@StaffFilter", employeeFilter == "" ? null : employeeFilter);
        //    parameters.Add("@Size", pageSize);
        //    parameters.Add("@Offset", pageIndex);
        //    parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //    parameters.Add("@TotalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);

        //    using (IDbConnection dbConnection = new MySqlConnection(connString))
        //    {
        //        var employees = dbConnection.Execute("Proc_GetInfoPage", param: parameters, commandType: CommandType.StoredProcedure);

        //        var data = new InfoPage() { TotalPage = parameters.Get<Int32>("@TotalPage"), TotalRecord = parameters.Get<Int32>("@TotalRecord") };

        //        return data;
        //    };
        //}

        public InfoPage GetInfoPage(string employeeFilter, int pageSize, int pageIndex, string departmentID, string positionID,int status)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@StaffFilter", employeeFilter == "" ? null : employeeFilter);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@DepartmentID", departmentID == "" ? null : departmentID);
            parameters.Add("@PositionID", positionID == "" ? null : positionID);
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Status", status == 0 ? null : status);
            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var staffs = dbConnection.Query<FullStaff>("Proc_GetStaffFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                data.TotalPage = (int)Math.Ceiling((float)(data.TotalRecord / pageSize))+1;
                return data;
            }
        }

        public string GetLastStaffCode()
        {
            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var staffLastCode = dbConnection.QueryFirstOrDefault<String>("Proc_GetLastStaffCode", commandType: CommandType.StoredProcedure);
                
                return staffLastCode;
            }
        }

        public int Import(List<Staff> staffs)
        {
            int rowAffects = 0;
            int cnt = 0;
            foreach (var s in staffs)
            {
                rowAffects = _dbConnection.Execute($"Proc_Insert{tableName}", s, commandType: CommandType.StoredProcedure);
                cnt++;
            }
            //Insert
           _dbConnection.Close();

            return cnt;
        }

        public int InsertStaff(Staff staff)
        {
            int rowAffects = 0;
            int cnt = 0;
            try
            {
               
              //  staff.guid = new Guid();

                rowAffects = _dbConnection.Execute($"Proc_Insert{tableName}", staff, commandType: CommandType.StoredProcedure);
                //Account account = new Account();
                //account.Username = staff.StaffCode;
                //account.Password = staff.PhoneNumber;
                //account.RoleID = "1";
                //account.StaffID = staff.guid;
              //  rowAffects = _dbConnection.Execute($"Proc_InsertAccount", account, commandType: CommandType.StoredProcedure);

                cnt++;
            }
            catch (Exception)
            {

                throw;
            }
           
            _dbConnection.Close();
            //Insert


            return rowAffects;
        }
        //public int InsertStaff(Staff staff)
        //{

        //}
    }
}
