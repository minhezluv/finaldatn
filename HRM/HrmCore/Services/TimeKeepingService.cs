using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Services
{

    public class TimeKeepingService : BaseService<TimeKeeping>, ITimeKeepingService
    {
		#region props and constructor
		ITimeKeepingRepository JobRepository;
		public TimeKeepingService(ITimeKeepingRepository mJobRepository) : base(mJobRepository)
		{
			JobRepository = mJobRepository;
		}

        public ServiceResult GetByFilterPaging(string staffFilter, int pageSize, int pageIndex, string departmentID, string positionName, int Year,int Month, int Day)
        {
            var serviceResult = new ServiceResult();

            var tmp = string.Empty;

            if (!(staffFilter == null || staffFilter == ""))
            {
                tmp = staffFilter;
            };
            var _departmentID = string.Empty;
            var _positionID = string.Empty;
            if (!(departmentID == "" || departmentID == null))
            {
                 _departmentID = departmentID;
            }
            if (!(positionName  == "" || positionName == null))
            {
               _positionID = positionName;
            }
            List<FullTimeKeeping> timeKeepings = this.JobRepository.GetByFilterPaging(tmp, pageSize, pageIndex, _departmentID, _positionID, Year, Month, Day );
            //  serviceResult.dataBonus = this.StaffRepository.GetInfoPage(tmp, pageSize, pageIndex, departmentID, positionID);
            InfoPage infoPage = this.JobRepository.GetInfoPage(tmp, pageSize, pageIndex, departmentID, positionName, Year, Month, Day);
            var data = new
            {
                InfoPage = infoPage,
                Entities = timeKeepings
            };
            serviceResult.data = data;
            return serviceResult;
        }

        public ServiceResult GetByMonth(Guid guid)
        {
            var serviceResult = new ServiceResult();
            List<FullTimeKeeping> temp = this.JobRepository.GetByMonth(guid);
            var tempTk= temp.Find(tk => tk.guid == guid);
            DateTime Date = tempTk.Start;
          //  DateTime Date = temp.First().Start;
            int sizeMonth = DateTime.DaysInMonth(Date.Year, Date.Month) ;
            DateTime now = DateTime.Now;
            int currMonth = now.Month;
            int[] times =  new int[sizeMonth];
            int dayoff = 0;
            int Late = 0;
            int Early = 0;
            //int rest = 0;

          
            for (int i =0; i < sizeMonth; i++)
            {
                DateTime tempDate = new DateTime(Date.Year, Date.Month, i+1);
                if (tempDate.DayOfWeek == DayOfWeek.Saturday || tempDate.DayOfWeek == DayOfWeek.Sunday )
                {
                    times[i] = 3;
                    
                }
            }
            foreach (FullTimeKeeping tk in temp)
            {

                var day = tk.Start;
                if (day.Month > 0 && times[day.Day-1] == 0 || times[day.Day-1] == 3)
                {
                    //  Date = day;
                    if (day.Hour <= 8)
                    {
                        times[day.Day-1] = 2;
                        Early++;
                    }
                    else
                    {
                        times[day.Day-1] = 1;
                        Late++;
                    };
                }
            

            }

           

            //dayoff =sizeMonth - Early - Late;
            if (currMonth == Date.Month)
            {
               // dayoff = Date.Day - Early - Late;
                for(int i =0; i < Date.Day; i++)
                {
                    if (times[i] == 0)
                    {
                        dayoff++;
                        times[i] = 4;
                    }
                    
                }
            }
            else
            {
                for (int i = 0; i < sizeMonth; i++)
                {
                    if (times[i] == 0)
                    {
                        dayoff++;
                        times[i] = 4;
                    }
                }
            }

           
            var data = new
            {
                times = times,
                dayoff = dayoff,
                early = Early,
                late = Late,
                year = Date.Year,
                month = Date.Month

            };
            serviceResult.data = data;
            return serviceResult;
            throw new NotImplementedException();
        }

        public ServiceResult GetByWeekAll()
        {
            var serviceResult = new ServiceResult();

            DateTime now = DateTime.Now;

            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            int currentWeek = calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var tmp = string.Empty;
            var tkAll = JobRepository.GetByFilterPaging(tmp,1000,1,tmp,tmp,now.Year,0,0);
            int[] result = new int[7];
           // int cnt = 0;
            foreach(TimeKeeping tk in tkAll)
            {

                var day = tk.Start;
                int week = calendar.GetWeekOfYear(day, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                int dayweek = (int) day.DayOfWeek;
                if (day.Month > 0 && week == currentWeek)
                {
                    //  Date = day;
                    if (day.Hour <= 8)
                    {
                        result[dayweek]++;
                    }
                    
                }
            }
            serviceResult.data = result;
            return serviceResult;
        }

        public ServiceResult GetByWeekOption(int year, int week)
        {
            var serviceResult = new ServiceResult();

           // DateTime now = DateTime.Now;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
           // int currentWeek = calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var tmp = string.Empty;
            var tkAll = JobRepository.GetByFilterPaging(tmp, 1000, 1, tmp, tmp, year, 0, 0);

            int[] result = new int[7];
            int[] late = new int[7];
            int[] allwork = new int[7];
            int cnt = 0;
            foreach (TimeKeeping tk in tkAll)
            {

                var day = tk.Start;
                int weektmp = calendar.GetWeekOfYear(day, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                int dayweek = (int)day.DayOfWeek;
                if (day.Month > 0 && weektmp == week)
                {
                    //  Date = day;
                    if (day.Hour <= 8 && day.Minute <=15)
                    {
                        result[dayweek]++;
                       // allwork[dayweek]++;
                      //  cnt++;
                    }
                    else
                    {
                        late[dayweek]++;
                       
                        //cnt++;
                    }
                    cnt++;
                    allwork[dayweek]++;

                }
            }
            var data = new
            {
                early = result,
                late = late,
                allworking = cnt,
                allwork = allwork
            };
            serviceResult.data = data;
            return serviceResult;
        }

        public ServiceResult GetByYearAll(int year)
        {
            var serviceResult = new ServiceResult();
            var tmp = string.Empty;
            var tkAll = JobRepository.GetByFilterPaging(tmp, 2000, 1, tmp, tmp, year, 0, 0);
            int[] result = new int[12];
            int[] late = new int[12];
            foreach (TimeKeeping tk in tkAll)
            {

                var day = tk.Start;
               
               
                if (day.Month > 0 )
                {
                    //  Date = day;
                    if (day.Hour <= 8 && day.Minute <=15)
                    {
                        result[day.Month-1]++;
                    }
                    else
                    {
                        late[day.Month - 1]++;
                    }

                }
            }
            var data = new
            {
                early = result,
                late = late
            };
            serviceResult.data = data;
            return serviceResult;
        }

        public ServiceResult GetIDbyStaffCode(string staffCode)
        {
            throw new NotImplementedException();
        }

        public ServiceResult GetInfoPage(string staffFilter, int pageSize, int pageIndex, string departmentID, string positionName, int Year, int Month, int Day)
        {
            var serviceResult = new ServiceResult();

            var tmp = string.Empty;

            if (!(staffFilter == null || staffFilter == ""))
            {
                tmp = staffFilter;
            };
            var _departmentID = string.Empty;
            var _positionID = string.Empty;
            if (!(departmentID == "" || departmentID == null))
            {
                departmentID = _departmentID;
            }
            if (!(positionName == "" || positionName == null))
            {
                positionName = _positionID;
            }
            serviceResult.data = this.JobRepository.GetInfoPage(tmp, pageSize, pageIndex, departmentID, positionName, Year, Month, Day);
            //  serviceResult.dataBonus = this.StaffRepository.GetInfoPage(tmp, pageSize, pageIndex, departmentID, positionID);
          //  InfoPage infoPage
            return serviceResult;
        }

        public ServiceResult GetStaffTimeAll()
        {
           
            throw new NotImplementedException();
        }

        public ServiceResult GetStaffTimeByID(Guid StaffID)
        {
            var serviceResult = new ServiceResult(); ;
            
            throw new NotImplementedException();
        }
        #endregion
    }
}
