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
    public class TimeKeepingRepository : BaseRepository<TimeKeeping>, ITimeKeepingRepository
    {
        public TimeKeepingRepository(IConfiguration config) : base(config)
        { }

        public List<FullTimeKeeping> GetByFilterPaging(string staffFilter, int pageSize, int pageIndex, string departmentID, string positionName, int Year, int Month, int Day)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@StaffFilter", staffFilter == "" ? null : staffFilter);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@DepartmentID", departmentID == "" ? null : departmentID);
            parameters.Add("@PositionID", positionName == "" ? null : positionName);
            parameters.Add("@Day", Day );
            parameters.Add("@Month", Month);
            parameters.Add("@Year", Year);
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);


            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var timeKeepings = dbConnection.Query<FullTimeKeeping>("Proc_GetTKFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                return (List<FullTimeKeeping>) timeKeepings;
            }
        }

        public List<FullTimeKeeping> GetByMonth(Guid guid)
        {
            string sqlCmd = $"SELECT * FROM {tableName} WHERE Guid = @guid";
            var dparams = new DynamicParameters();
            dparams.Add($"@guid", guid.ToString());

            TimeKeeping _tk = conn.QueryFirstOrDefault<TimeKeeping>(sqlCmd, dparams);
            int Month = (_tk.Start).Month;
            int Year = _tk.Start.Year;
            Guid StaffID = _tk.StaffID;

            var parameters = new DynamicParameters();
            parameters.Add("@StaffFilter",null);
            parameters.Add("@PageSize", 10000);
            parameters.Add("@PageIndex", 1);
            parameters.Add("@DepartmentID", null);
            parameters.Add("@PositionID",null);
            parameters.Add("@Day", null);  ;
            parameters.Add("@Month", Month);
            parameters.Add("@Year", Year);
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);


            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var timeKeepings = dbConnection.Query<FullTimeKeeping>("Proc_GetTKFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                List<FullTimeKeeping> _tkList = new List<FullTimeKeeping>();
                foreach(FullTimeKeeping tk in timeKeepings)
                {
                    if(tk.StaffID == StaffID)
                    {
                        _tkList.Add(tk);
                    }
                }
                return (List<FullTimeKeeping>)_tkList;
            }
        }

        public Guid getIDbyStaffCode(string staffCode)
        {

            var parameters = new DynamicParameters();

            parameters.Add("@StaffFilter", staffCode == ""? null:staffCode);
            parameters.Add("@PageSize", 10);
            parameters.Add("@PageIndex", 1);
            parameters.Add("@DepartmentID", null);
            parameters.Add("@PositionID",null);
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Status", 2);

            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var staffs = dbConnection.Query<FullStaff>("Proc_GetStaffFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                //(List<FullStaff>)staffs;
                if (data.TotalRecord == 0)
                {
                    return Guid.Empty;
                }
                var tmp =staffs.First();
                Guid currguid = tmp.guid;
                return currguid;
                //return (List<FullStaff>)staffs;
                //if (List<FullStaff>) staffs == 0)
                //{
                //    return Guid(1);
                //}
                //return staffs.

            }
            //Guid staffID = new Guid();
            //string sqlCmd = $"SELECT * FROM staff WHERE StaffCode = @guid";
            //var dparams = new DynamicParameters();
            //dparams.Add($"@guid", staffCode);
            //return staffID;
           // throw new NotImplementedException();
        }

        public InfoPage GetInfoPage(string staffFilter, int pageSize, int pageIndex, string departmentID, string positionName, int Year, int Month, int Day)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@StaffFilter", staffFilter == "" ? null : staffFilter);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@DepartmentID", departmentID == "" ? null : departmentID);
            parameters.Add("@PositionID", positionName == "" ? null : positionName);
            parameters.Add("@Day", Day );
            parameters.Add("@Month", Month );
            parameters.Add("@Year", Year);
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);


            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var timeKeepings = dbConnection.Query<TimeKeeping>("Proc_GetTKFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                data.TotalPage = (int)Math.Ceiling((float)(data.TotalRecord / pageSize) )+1;
                //pageSize != null ? Math.Ceiling((decimal)((decimal)totalRecord / pageSize)) : 1;
                return data;
            }
        }

        public DateTime GetMonth(Guid guid)
        {
            string sqlCmd = $"SELECT * FROM {tableName} WHERE Guid = @guid";
            var dparams = new DynamicParameters();
            dparams.Add($"@guid", guid.ToString());

            TimeKeeping _tk = conn.QueryFirstOrDefault<TimeKeeping>(sqlCmd, dparams);
            //int Month = (_tk.Start).Month;
            //int Year = _tk.Start.Year;
            //int Day = _tk.Start.Day;
            //DateTime _time = new DateTime(Year, Month, Day);
            return _tk.Start;
        }

        public List<StaffTime> GetStaffTimeAll()
        {
            List<StaffTime> mStaffTime = new List<StaffTime>();
            var sqlCmd = $"SELECT * FROM TimeKeeping";
            List<TimeKeeping> timeList = conn.Query<TimeKeeping>(sqlCmd).ToList();
            
            return mStaffTime;
        }

        public StaffTime GetStaffTimeByID(Guid StaffID)
        {
            StaffTime staffTime = new StaffTime();
            var sqlCmd = $"SELECT * FROM TimeKeeping Where StaffID = {StaffID}";
            List<TimeKeeping> timeList = conn.Query<TimeKeeping>(sqlCmd).ToList();
            staffTime = (StaffTime)timeList[0];
            foreach(var timeKeeping in timeList)
            {
                staffTime.TimeIn.Add(timeKeeping.Start);
                staffTime.TimeOut.Add(timeKeeping.End);
            }
            return staffTime;
        }

      
    }
}
