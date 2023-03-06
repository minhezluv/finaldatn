import BaseAPI from "../../base/BaseAPI";
import BaseAPIConfig from "../../base/BaseAPIConfig";

class LaborContractsAPI extends BaseAPI {
  constructor() {
    super();
    this.controller = "api/LaborContract";
  }

  /**
   *
   * @param {int} pageSize Số bản ghi / trang
   * @param {int} pageIndex
   * @param {string} filterValue Giá trị cần tìm kiếm
   * @returns
   */
  filter(
    pageSize,
    pageNum,
    filterValue,
    departmentID,
    positionName,
    typeLaborContract,
    status
  ) {
    if (departmentID == undefined || departmentID == 0) {
      departmentID = " ";
    }
    if (positionName == undefined || positionName == 0) {
      positionName = " ";
    }
    if (filterValue == null) {
      filterValue = "%20";
    }
    var tmpstring = ``;
    if (!(status == undefined || status == 0)) {
      tmpstring += `&status=${status}`;
    }
    if (!(typeLaborContract == undefined || typeLaborContract == 10)) {
      tmpstring += `&TypeLaborContractID=${typeLaborContract}`;
    }
    let queryString = `${this.controller}/Filter?LCFilter=${filterValue}&pageSize=${pageSize}&pageIndex=${pageNum}&departmentID=${departmentID}&positionID=${positionName}${tmpstring}`;

    return BaseAPIConfig.get(`${queryString}`);
  }

  /**
   * Hàm lấy mã nhân viên mới
   * @returns
   */
  getNewLCCode() {
    return BaseAPIConfig.get(`${this.controller}/LastLCCode`);
  }

  /**
   * Hàm lấy bản ghi theo property
   * @param {string} propName Tên property cần truy vấn
   * @param {string} propValue Giá trị của property
   * @returns Một bản ghi lấy được có propertyName và propValue truyền vào
   */
  getLaborcontractByProperty(propName, propValue) {
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
  //   /**
  //    * Hàm thêm nhân viên mới
  //    */
  //   insertData(data) {
  //     let queryString = `${this.controller}/InsertStaff`;
  //     return BaseAPIConfig.post(`${queryString}`, data);
  //   }
  /**
   * Hàm thêm hợp đồng mới
   */
  insertData(data) {
    let queryString = `${this.controller}/InsertLC`;
    return BaseAPIConfig.post(`${queryString}`, data);
  }
}

export default new LaborContractsAPI();
