using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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
            parameters.Add("@Day", Day == 0 ? null : Day);
            parameters.Add("@Month", Month ==0 ? null : Month);
            parameters.Add("@Year", Year == 0 ? null : Year);
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);


            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var timeKeepings = dbConnection.Query<FullTimeKeeping>("Proc_GetTKFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                dbConnection.Close();
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
            parameters.Add("@PageSize", 0);
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
                dbConnection.Close();
                return (List<FullTimeKeeping>)_tkList;
            }
        }

        public List<FullTimeKeeping> GetByMonthOption(int year, int month, string departmentID, string positionID, string staffID)
        {
            //string staffFilter = string.Empty;
            //if (staffID.Length > 0 || staffID ==null)
            //{
            //    //string sqlCmd = $"SELECT * FROM Staff WHERE Guid = @guid";
            //    //var dparams = new DynamicParameters();
            //    //dparams.Add($"@guid", staffID);

            //    //Staff _tk = conn.QueryFirstOrDefault<Staff>(sqlCmd, dparams);
            //    //staffFilter = _tk.StaffCode;
            //    //conn.Close();
            //    staffFilter =staffID;
            //}
            var parameters = new DynamicParameters();
            parameters.Add("@StaffFilter", staffID == "" ? null : staffID);
            parameters.Add("@PageSize", 0);
            parameters.Add("@PageIndex", 1);
            parameters.Add("@DepartmentID", departmentID == "" ? null : departmentID);
            parameters.Add("@PositionID", positionID == "" ? null : positionID);
            parameters.Add("@Day",null);
            parameters.Add("@Month", month == 0 ? null : month);
            parameters.Add("@Year", year == 0 ? null : year);
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var timeKeepings = dbConnection.Query<FullTimeKeeping>("Proc_GetTKFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                dbConnection.Close();
                return (List<FullTimeKeeping>)timeKeepings;

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
                dbConnection.Close();
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
            parameters.Add("@Day", Day == 0 ? null : Day);
            parameters.Add("@Month", Month == 0 ? null : Month);
            parameters.Add("@Year", Year == 0 ? null : Year);
            parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);


            using (IDbConnection dbConnection = new MySqlConnection(connString))
            {
                var timeKeepings = dbConnection.Query<TimeKeeping>("Proc_GetTKFilterPaging", param: parameters, commandType: CommandType.StoredProcedure);
                var data = new InfoPage() { TotalPage = 0, TotalRecord = parameters.Get<Int32>("@TotalRecord") };
                data.TotalPage = (int)Math.Ceiling((float)(data.TotalRecord / pageSize) )+1;
                //pageSize != null ? Math.Ceiling((decimal)((decimal)totalRecord / pageSize)) : 1;
              //  dbConnection.Close();
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
            conn.Close();   
            return _tk.Start;
        }

        public List<StaffTime> GetStaffTimeAll()
        {
            List<StaffTime> mStaffTime = new List<StaffTime>();
            var sqlCmd = $"SELECT * FROM TimeKeeping";
            List<TimeKeeping> timeList = conn.Query<TimeKeeping>(sqlCmd).ToList();
            conn.Close();
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
            conn.Close();
            return staffTime;
        }

      
    }
}
