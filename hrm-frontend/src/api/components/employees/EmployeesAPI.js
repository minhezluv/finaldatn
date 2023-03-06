import BaseAPI from "../../base/BaseAPI";
import BaseAPIConfig from "../../base/BaseAPIConfig";

class EmployeesAPI extends BaseAPI {
  constructor() {
    super();
    this.controller = "api/Staff";
  }

  /**
   *
   * @param {int} pageSize Số bản ghi / trang
   * @param {int} pageIndex
   * @param {string} filterValue Giá trị cần tìm kiếm
   * @returns
   */
  filter(pageSize, pageNum, filterValue, departmentID, positionName, Status) {
    if (departmentID == undefined || departmentID == 1) {
      departmentID = " ";
    }
    if (positionName == undefined || positionName == "1") {
      positionName = " ";
    }
    if (filterValue == null) {
      filterValue = "%20";
    }
    if (Status == null || Status == undefined) {
      Status = 0;
    }
    let queryString = `${this.controller}/Filter?employeeFilter=${filterValue}&pageSize=${pageSize}&pageIndex=${pageNum}&departmentID=${departmentID}&positionID=${positionName}&status=${Status}`;

    return BaseAPIConfig.get(`${queryString}`);
  }

  GetAllStaffPerMonth(year) {
    let queryString = `${this.controller}/GetAllStaffPerMonth?year=${year}`;

    return BaseAPIConfig.get(`${queryString}`);
  }

  GetInforPage(
    pageSize,
    pageNum,
    filterValue,
    departmentID,
    positionName,
    Status
  ) {
    if (departmentID == undefined || departmentID == 1) {
      departmentID = " ";
    }
    if (positionName == undefined || positionName == "1") {
      positionName = " ";
    }
    if (filterValue == null) {
      filterValue = "%20";
    }
    if (Status == null || Status == undefined) {
      Status = 0;
    }
    let queryString = `${this.controller}/InfoPage?employeeFilter=${filterValue}&pageSize=${pageSize}&pageIndex=${pageNum}&departmentID=${departmentID}&positionID=${positionName}&status=${Status}`;

    return BaseAPIConfig.get(`${queryString}`);
  }
  /**
   * Hàm lấy mã nhân viên mới
   * @returns
   */
  getNewEmployeeCode() {
    return BaseAPIConfig.get(`${this.controller}/LastStaffCode`);
  }

  /**
   * Hàm lấy bản ghi theo property
   * @param {string} propName Tên property cần truy vấn
   * @param {string} propValue Giá trị của property
   * @returns Một bản ghi lấy được có propertyName và propValue truyền vào
   */
  getEmployeeByProperty(propName, propValue) {
    let queryString = `${this.controller}/Property?propName=${propName}&propValue=${propValue}`;

    return BaseAPIConfig.get(`${queryString}`);
  }

  /**
   * Hàm export dữ liệu
   * @param {string} filterValue Giá trị filter
   */
  exportData(filterValue) {
    let queryString = `${this.controller}/Export${
      filterValue ? "?&filterValue=" + filterValue : ""
    }`;

    return BaseAPIConfig.get(`${queryString}`, { responseType: "blob" });
  }
  /**
   * Hàm thêm nhân viên mới
   */
  insertData(data) {
    let queryString = `${this.controller}/InsertStaff`;
    return BaseAPIConfig.post(`${queryString}`, data);
  }
}

export default new EmployeesAPI();
