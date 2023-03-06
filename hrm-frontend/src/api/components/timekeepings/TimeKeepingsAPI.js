import BaseAPI from "../../base/BaseAPI";
import BaseAPIConfig from "../../base/BaseAPIConfig";

class TimeKeepingsAPI extends BaseAPI {
  constructor() {
    super();
    this.controller = "api/TimeKeeping";
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
    positionID,
    year,
    month,
    day
  ) {
    if (departmentID == undefined || departmentID == 1) {
      departmentID = " ";
    }
    if (positionID == undefined || positionID == "0") {
      positionID = " ";
    }
    if (filterValue == undefined || filterValue == null) {
      filterValue = "";
    }
    if (year == undefined || month == "0") {
      year = "0";
    }
    if (month == undefined || month == "0") {
      month = "0";
    }
    if (day == undefined || day == "0") {
      day = "0";
    }
    let queryString = `${this.controller}/Filter?pageSize=${pageSize}&pageIndex=${pageNum}&staffFilter=${filterValue}&departmentID=${departmentID}&positionName=${positionID}&Year=${year}&Month=${month}&Day=${day}`;

    return BaseAPIConfig.get(`${queryString}`);
  }

  /**
   *
   * @param {int} pageSize Số bản ghi / trang
   * @param {int} pageIndex
   * @param {string} filterValue Giá trị cần tìm kiếm
   * @returns
   */
  pageInfo(pageSize, pageNum) {
    let queryString = `${this.controller}/InfoPage?pageSize=${pageSize}&pageIndex=${pageNum}`;

    return BaseAPIConfig.get(`${queryString}`);
  }
  /**
   *
   * @param {int} pageSize Số bản ghi / trang
   * @param {int} pageIndex
   * @param {string} filterValue Giá trị cần tìm kiếm
   * @returns
   */
  getByWeekAll() {
    let queryString = `${this.controller}/GetByWeekAll`;

    return BaseAPIConfig.get(`${queryString}`);
  }

  /**
   *
   * @param {int} pageSize Số bản ghi / trang
   * @param {int} pageIndex
   * @param {string} filterValue Giá trị cần tìm kiếm
   * @returns
   */
  getByYearAll(year) {
    let queryString = `${this.controller}/GetByYearAll?year=${year}`;

    return BaseAPIConfig.get(`${queryString}`);
  }

  /**
   *
   * @param {int} pageSize Số bản ghi / trang
   * @param {int} pageIndex
   * @param {string} filterValue Giá trị cần tìm kiếm
   * @returns
   */
  getByWeekOption(year, week) {
    let queryString = `${this.controller}/GetByWeekOption?year=${year}&week=${week}`;

    return BaseAPIConfig.get(`${queryString}`);
  }

  /**
   * Hàm lấy bản ghi theo property
   * @param {string} propName Tên property cần truy vấn
   * @param {string} propValue Giá trị của property
   * @returns Một bản ghi lấy được có propertyName và propValue truyền vào
   */
  getTimeKeepingByProperty(propName, propValue) {
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
}

export default new TimeKeepingsAPI();
