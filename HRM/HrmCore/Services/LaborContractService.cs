using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HrmCore.Services
{

    public class LaborContractService : BaseService<LaborContract>, ILaborContractService
    {
		#region props and constructor
		ILaborContractRepository JobRepository;
		public LaborContractService(ILaborContractRepository mJobRepository) : base(mJobRepository)
		{
			JobRepository = mJobRepository;
		}

        public ServiceResult CheckLCCodeExists(string lcCode, string lcID)
        {
            var serviceResult = new ServiceResult();

            if ((lcCode == null || lcCode == "") && (lcID == null || lcID == ""))
            {
                serviceResult.success = false;
                serviceResult.devMsg = "không có dữ liệu đầu vào";
            };

            serviceResult.data = this.JobRepository.CheckLCCodeExists(lcCode, lcID);

            return serviceResult;
        }

        public MemoryStream Export(CancellationToken cancellationToken, string filterValue)
        {
            throw new NotImplementedException();
        }

        public ServiceResult GetByFilterPaging(string lcFilter, int pageSize, int pageIndex, string departmentID, string positionID, int Status, int TypeLaborContractID)
        {
            var serviceResult = new ServiceResult();

            var tmp = string.Empty;

            if (!(lcFilter == null || lcFilter == ""))
            {
                tmp = lcFilter;
            };
            var _departmentID = string.Empty;
            var _positionID = string.Empty;
            if (!(departmentID == "" || departmentID == null))
            {
                _departmentID = departmentID;
            }
            if (!(positionID == "" || positionID == null))
            {
                _positionID = positionID;
            }
            var LC = this.JobRepository.GetByFilterPaging(tmp, pageSize, pageIndex, _departmentID, _positionID,  Status,  TypeLaborContractID);
            var InfoPage = this.JobRepository.GetInfoPage(tmp, pageSize, pageIndex, _departmentID, _positionID, Status, TypeLaborContractID);
            var data = new
            {
                data = LC,
                infoPage = InfoPage
            };
            //  serviceResult.dataBonus = this.StaffRepository.GetInfoPage(tmp, pageSize, pageIndex, departmentID, positionID);
            serviceResult.data = data;
            return serviceResult;
        }

        public ServiceResult GetInfoPage(string lcFilter, int pageSize, int pageIndex, string departmentID, string positionID)
        {
            throw new NotImplementedException();
        }

        //public ServiceResult GetInfoPage(string lcFilter, int pageSize, int pageIndex, string departmentID, string positionID,)
        //{
        //    var serviceResult = new ServiceResult();

        //    var tmp = string.Empty;

        //    if (!(lcFilter == null || lcFilter == ""))
        //    {
        //        tmp = lcFilter;
        //    };
        //    var _departmentID = string.Empty;
        //    var _positionID = string.Empty;
        //    if (!(departmentID == "" || departmentID == null))
        //    {
        //        departmentID = _departmentID;
        //    }
        //    if (!(positionID == "" || positionID == null))
        //    {
        //        positionID = _positionID;
        //    }
        //    serviceResult.data = this.JobRepository.GetInfoPage(tmp, pageSize, pageIndex, departmentID, positionID,);
        //    //  serviceResult.dataBonus = this.StaffRepository.GetInfoPage(tmp, pageSize, pageIndex, departmentID, positionID);
        //    return serviceResult;
        //}

        public ServiceResult GetLastLCCode()
        {
            var serviceResult = new ServiceResult();

            var lastStaffCode = this.JobRepository.GetLastLCCode();

            if (lastStaffCode == null)
            {
                serviceResult.data = "HD-001";
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
            throw new NotImplementedException();
        }

        public ServiceResult InsertLC(LaborContract LC)
        {
            var serviceResult = new ServiceResult();
            int rowAffect = 0;
            try
            {
                rowAffect = this.JobRepository.InsertLC(LC);
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
        #endregion
    }
}
