 using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Threading.Tasks;
using HrmCore.Entities.Excel;
using HrmCore.Attributes;

namespace HrmCore.Services
{
    public class StaffService : BaseService<Staff>, IStaffService
    {
		#region props and constructor
		IStaffRepository StaffRepository;
        IAccountRepository AccountRepository;
		public StaffService(IStaffRepository mJobRepository,IAccountRepository mAccountRepository) : base(mJobRepository)
		{
			StaffRepository = mJobRepository;
            AccountRepository = mAccountRepository;
		}

        public ServiceResult CheckEmployeeCodeExists(string staffCode, string staffID)
        {
            var serviceResult = new ServiceResult();

            if ((staffCode == null || staffCode == "")&&(staffID==null || staffID ==""))
            {
                serviceResult.success = false;
                serviceResult.devMsg = "không có dữ liệu đầu vào";
            };

            serviceResult.data = this.StaffRepository.CheckEmployeeCodeExists(staffCode,staffID);

            return serviceResult;
        }

        public MemoryStream Export(CancellationToken cancellationToken, string filterValue)
        {
            var stream = new MemoryStream();

            List<Staff> staffsList = this.StaffRepository.GetAll();
            var properties = typeof(Staff).GetProperties();

            using (var package = new ExcelPackage(stream))
            {

                var workSheet = package.Workbook.Worksheets.Add("Danh Sách Nhân Viên");
                //workSheet.Cells.LoadFromCollection(employees.Employees, true);

                // Chình tiêu đề trong bảng.

                // STT
                workSheet.Cells[3, 1].Value = "STT";
                workSheet.Cells[3, 1].Style.Font.Bold = true;
                workSheet.Cells[3, 1].Style.Fill.SetBackground(Color.LightGray);
                workSheet.Cells[3, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[3, 1].Style.Border.Top.Color.SetColor(Color.Black);
                workSheet.Cells[3, 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[3, 1].Style.Border.Bottom.Color.SetColor(Color.Black);
                workSheet.Cells[3, 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[3, 1].Style.Border.Left.Color.SetColor(Color.Black);
                workSheet.Cells[3, 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                workSheet.Cells[3, 1].Style.Border.Right.Color.SetColor(Color.Black);


                var column = 2;

                foreach (var prop in properties)
                {
                    var propMISAExport = prop.GetCustomAttributes(typeof(PropExport), true);

                    //Xét các trường có được export không?
                    if (propMISAExport.Length == 1)
                    {

                        // định dạng ngày/tháng/năm
                        if (prop.PropertyType.Name.Contains(typeof(Nullable).Name) && prop.PropertyType.GetGenericArguments()[0] == typeof(DateTime))
                        {
                            workSheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        else
                        {
                            workSheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }

                        workSheet.Cells[3, column].Value = (propMISAExport[0] as PropExport).Name;
                        workSheet.Cells[3, column].Style.Font.Bold = true;
                        workSheet.Cells[3, column].Style.Fill.SetBackground(Color.LightGray);
                        workSheet.Cells[3, column].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[3, column].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, column].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[3, column].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, column].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[3, column].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, column].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[3, column].Style.Border.Right.Color.SetColor(Color.Black);

                        column++;
                    }
                }

                // Chỉnh bản ghi vào hàng, cell
                for (int i = 0; i < staffsList.Count(); i++)
                {
                    workSheet.Cells[i + 4, 1].Value = i + 1;
                    workSheet.Cells[i + 4, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 4, 1].Style.Border.Top.Color.SetColor(Color.Black);
                    workSheet.Cells[i + 4, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 4, 1].Style.Border.Left.Color.SetColor(Color.Black);
                    workSheet.Cells[i + 4, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 4, 1].Style.Border.Right.Color.SetColor(Color.Black);
                    workSheet.Cells[i + 4, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 4, 1].Style.Border.Bottom.Color.SetColor(Color.Black);

                    int col = 2;

                    foreach (var prop in properties)
                    {
                        var propMISAExport = prop.GetCustomAttributes(typeof(PropExport), true);

                        //Xét các trường có được export không?
                        if (propMISAExport.Length == 1)
                        {

                            if (prop.PropertyType.Name.Contains(typeof(Nullable).Name) && prop.PropertyType.GetGenericArguments()[0] == typeof(DateTime))
                            {
                                var tmp = staffsList[i].GetType().GetProperty(prop.Name).GetValue(staffsList[i], null);
                                workSheet.Cells[i + 4, col].Value = tmp == null ? "" : Convert.ToDateTime(tmp).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                workSheet.Cells[i + 4, col].Value = staffsList[i].GetType().GetProperty(prop.Name).GetValue(staffsList[i], null);
                            }

                            workSheet.Cells.AutoFitColumns();

                            workSheet.Cells[i + 4, col].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            workSheet.Cells[i + 4, col].Style.Border.Top.Color.SetColor(Color.Black);
                            workSheet.Cells[i + 4, col].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            workSheet.Cells[i + 4, col].Style.Border.Bottom.Color.SetColor(Color.Black);
                            workSheet.Cells[i + 4, col].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            workSheet.Cells[i + 4, col].Style.Border.Left.Color.SetColor(Color.Black);
                            workSheet.Cells[i + 4, col].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            workSheet.Cells[i + 4, col].Style.Border.Right.Color.SetColor(Color.Black);

                            col++;
                        }
                    }
                }

                // Chỉnh tiêu đề cho workSheet
                workSheet.Cells["A1:I1"].Merge = true;
                workSheet.Cells["A2:I2"].Merge = true;
                workSheet.Cells[1, 1].Value = "DANH SÁCH NHÂN VIÊN";
                workSheet.Cells[1, 1].Style.Font.Size = 16;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                package.Save();
            }

            stream.Position = 0;
            return stream;
        }

        public ServiceResult GetAllStaffPerMonth(int year)
        {
            var serviceResult = new ServiceResult();
            var tmp = string.Empty;
            
            var staffs = this.StaffRepository.GetByFilterPaging(null, 1000, 1, null, null, 2);
            int[] _month = new int[12];
        
            DateTime now = DateTime.Now;
            int _year = now.Year;
            int _maxMonth = 0;

            if(year == _year)
            {
                _maxMonth = now.Month;
            }else if (year < _year)
            {
                _maxMonth = 12;
            }

            foreach (Staff s in staffs)
            {
                int createYear = s.CreatedDate.Year;
                if(createYear == year)
                {
                    int createMonth = s.CreatedDate.Month - 1;
                    for (int i = createMonth; i < _maxMonth; i++)
                    {
                        _month[i]++;
                    }
                }
               
            }
            serviceResult.data = _month;
            return serviceResult;
        }

        public ServiceResult GetByFilterPaging(string employeeFilter, int pageSize, int pageIndex, string departmentID, string positionID, int status)
        {
            var serviceResult = new ServiceResult();

            var tmp = string.Empty;

            if (!(employeeFilter == null || employeeFilter == ""))
            {
                tmp = employeeFilter;
            };
            var _departmentID = string.Empty;
            var _positionID = string.Empty;
            if(!(departmentID == "" || departmentID == null))
            {
                _departmentID = departmentID;
            }
            if(!(positionID == "" || positionID ==null))
            {
                _positionID = positionID;
            }
            var staffs = this.StaffRepository.GetByFilterPaging(tmp, pageSize, pageIndex,_departmentID,_positionID,status) ;
            var InfoPage = this.StaffRepository.GetInfoPage(tmp, pageSize, pageIndex, _departmentID, _positionID,status);
            var data = new
            {
                data = staffs,
                infoPage = InfoPage
            };
            //  serviceResult.dataBonus = this.StaffRepository.GetInfoPage(tmp, pageSize, pageIndex, departmentID, positionID);
            serviceResult.data = data;
            return serviceResult;
        }

        public ServiceResult GetInfoPage(string employeeFilter, int pageSize, int pageIndex,string departmentID, string positionID, int status)
        {
            var serviceResult = new ServiceResult();

            var tmp = string.Empty;

            if (!(employeeFilter == null || employeeFilter == ""))
            {
                tmp = employeeFilter;
            };
            var _departmentID = string.Empty;
            var _positionID = string.Empty;
            if (!(departmentID == "" || departmentID == null))
            {
                departmentID = _departmentID;
            }
            if (!(positionID == "" || positionID == null))
            {
                positionID = _positionID;
            }
            serviceResult.data = this.StaffRepository.GetInfoPage(tmp, pageSize, pageIndex, departmentID, positionID,status);
            //  serviceResult.dataBonus = this.StaffRepository.GetInfoPage(tmp, pageSize, pageIndex, departmentID, positionID);
            return serviceResult;
        }

        public ServiceResult GetLastStaffCode()
        {
            var serviceResult = new ServiceResult();

            var lastStaffCode = this.StaffRepository.GetLastStaffCode();

            if (lastStaffCode == null)
            {
                serviceResult.data = "NV-001";
                return serviceResult;
            }

            // Tách mã nhân viên thành 2 phần: mã chứ, mã số
            string prefix = String.Empty;
            string code = String.Empty;
            for (var i = 0; i < lastStaffCode.Length; i++)
            {
                if (Char.IsDigit(lastStaffCode[i])) code += lastStaffCode[i];
                else prefix += lastStaffCode[i];
            }

            string newStaffCode = prefix + (Int64.Parse(code) + 1).ToString();
            serviceResult.data = newStaffCode;

            return serviceResult;
        }

        public int Import(Staff staff)
        {
            return 0;
        }

        public ServiceResult InsertStaff(Staff staff)
        {
            var serviceResult = new ServiceResult();
            int rowAffect = 0;
            try
            {
                staff.guid = Guid.NewGuid();
               rowAffect =  this.StaffRepository.InsertStaff(staff);
                if (rowAffect > 0) {
                Account account= new Account();
                    
                    account.StaffID = staff.guid;
                    account.Username = staff.StaffCode;
                    account.Password = staff.PhoneNumber;
                    account.RoleID = "1";
                    rowAffect = this.AccountRepository.insertAccount(account);
                }
                
                serviceResult.data = rowAffect;
                serviceResult.success = true;
            }
            catch
            {
                serviceResult.success = false;
                serviceResult.data = -1;
                
            }
            return serviceResult; 
        }

        public ServiceResult StaffNumberInfo(int year)

        {
            //var serviceResult = new ServiceResult();
            //var dataAll = this.StaffRepository.GetInfoPage(null, 1000, 1, null, null, 0);
            //var dataActive = this.StaffRepository.GetInfoPage(null, 1000, 1, null, null, 0);
            throw new NotImplementedException();
        }
        #endregion
        private object GetValueByProperty(Staff employee, string propName)
        {
            var propertyInfo = employee.GetType().GetProperty(propName);
            if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
            {
                var value = employee.GetType().GetProperty(propName).GetValue(employee, null);
                var date = Convert.ToDateTime(value).ToString("dd/MM/yyyy");

                return value != null ? date : "";
            }

            return employee.GetType().GetProperty(propName).GetValue(employee, null);
        }
    }
}
