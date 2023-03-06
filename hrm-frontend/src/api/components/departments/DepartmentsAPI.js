import BaseAPI from "../../base/BaseAPI";
import BaseAPIConfig from "../../base/BaseAPIConfig";
class departmentsAPI extends BaseAPI {
  constructor() {
    super();

    this.controller = "api/v1/Department";
  }
  getdepartmentByProperty(propName, propValue) {
    let queryString = `${this.controller}/Property?propName=${propName}&propValue=${propValue}`;

    return BaseAPIConfig.get(`${queryString}`);
  }

  /**
   *
   * @param {string} filterValue Giá trị cần tìm kiếm
   * @returns
   */
  filter(filterValue) {
    if (filterValue == null) {
      filterValue = "%20";
    }
    let queryString = `${this.controller}/Filter?departmentFilter=${filterValue}`;

    return BaseAPIConfig.get(`${queryString}`);
  }
}

export default new departmentsAPI();
