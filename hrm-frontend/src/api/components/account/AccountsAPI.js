import BaseAPI from "../../base/BaseAPI";
import BaseAPIConfig from "../../base/BaseAPIConfig";
class departmentsAPI extends BaseAPI {
  constructor() {
    super();

    this.controller = "api/v1/Account";
  }
  getaccountByProperty(propName, propValue) {
    let queryString = `${this.controller}/Property?propName=${propName}&propValue=${propValue}`;

    return BaseAPIConfig.get(`${queryString}`);
  }
  getCheckAccount(value) {
    let queryString = `${this.controller}/CheckAccount`;
    return BaseAPIConfig.post(`${queryString}`, value);
  }
}

export default new departmentsAPI();
