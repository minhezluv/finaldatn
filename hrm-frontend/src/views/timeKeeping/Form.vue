<template>
  <form v-if="showForm">
    <div class="form">
      <div class="form__title">
        <div class="form__title-left">
          <div class="form__title-text">
            <span class="text-semibold">Thông tin nhân viên</span>
          </div>
        </div>
        <div class="form__title-right d-flex">
          <Tooltip :customData="'Trợ giúp'">
            <div class="page-icon">
              <div class="form__icon-help"></div>
            </div>
          </Tooltip>
          <Tooltip :customData="'Đóng'">
            <div class="page-icon" @click="closeForm">
              <div class="form__icon-cancel"></div>
            </div>
          </Tooltip>
        </div>
      </div>
      <div class="form__content" ref="FormData" style="width: 500px">
        <VCalendar :year="year" :month="month" :workingDays="workingDays" />
        <div class="info">
          <div class="row_1">
            <div class="timeKeeping__element day-off">
              <span class="tk_row">Số ngày nghỉ</span>
              <span class="tk_row">{{ dayoff }}</span>
            </div>

            <div class="timeKeeping__element working">
              <span class="tk_row">Số ngày đi đúng giờ</span>
              <span class="tk_row">{{ early }}</span>
            </div>

            <div class="timeKeeping__element late">
              <span class="tk_row">Số ngày đi muộn</span>
              <span class="tk_row">{{ late }}</span>
            </div>
          </div>
          <div class="row_2">
            <pie-chart :pieData="pieData" />
          </div>
        </div>
      </div>
    </div>
    <!-- Popup hỏi lại khi đã sửa dữ liệu mà thoát -->
    <BasePopup
      ref="ConfirmPopup"
      class="popup-confirm"
      v-show="showConfirmPopup"
    >
      <template #content>
        <div class="page-icon popup-icon">
          <div class="popup__icon-confirm"></div>
        </div>
        <div class="popup__text">
          Dữ liệu đã bị thay đổi. Bạn có muốn cất không?
        </div>
      </template>
      <template #footer>
        <div
          class="btn btn-default btn-cancel"
          @click="showConfirmPopup = false"
        >
          Hủy
        </div>
        <div class="btn-group d-flex">
          <div class="btn btn-default" @click="showForm = false">Không</div>
          <div class="btn btn-primary">Có</div>
        </div>
      </template>
    </BasePopup>

    <!-- Popup thông báo lỗi -->
    <BasePopup class="popup-error" v-show="showErrorPopup">
      <template #content>
        <div class="page-icon popup-icon">
          <div
            :class="uniqueError ? 'popup__icon-warning' : 'popup__icon-danger'"
          ></div>
        </div>
        <div class="popup__text">
          {{ errorMessage }}
        </div>
      </template>
      <template #footer>
        <div
          class="footer__btn d-flex"
          :class="uniqueError ? 'flex-end' : 'flex-center'"
        >
          <div class="btn btn-primary" @click="closeErrorPopup">
            {{ uniqueError ? "Đồng ý" : "Đóng" }}
          </div>
        </div>
      </template>
    </BasePopup>
  </form>
</template>

<script>
// import FieldInputLabel from "../../components/FieldInputLabel.vue";
// import ComboBox from "../../components/Combobox.vue";
// import RadioButton from "../../components/RadioButton.vue";
import BasePopup from "../../components/BasePopup.vue";
import Tooltip from "../../components/Tooltip.vue";
import EmployeesAPI from "../../api/components/employees/EmployeesAPI";
//import DepartmentAPI from "../../api/components/departments/DepartmentsAPI";
import Resource from "../../js/common/Resource";
import Enumeration from "../../js/common/Enumeration";
import CommonFn from "../../js/common/CommonFn";
import VCalendar from "../../components/Calendars/CalendarShow.vue";
import PieChart from "../../components/Charts/ChartPie.vue";
import axios from "axios";
function initState(show) {
  return {
    dayoff: null,
    early: null,
    late: null,
    showForm: show,
    showConfirmPopup: false,
    showErrorPopup: false,
    customerBoxSelected: false,
    providerBoxSelected: false,
    employee: {},
    originData: {},
    employeeId: null,
    saveValidate: false,
    allInputValid: true,
    allUniue: true,
    formType: null,
    errorMessage: "",
    uniqueError: false,
    year: 0,
    month: 0,
    workingDays: [],
    pieData: {
      chartOptions: {
        legend: {
          show: false,
        },
        labels: ["đi đúng giờ", "đi muộn", " nghỉ"],

        colors: ["#0000ff", "#FFA500", "#EE4B2B"],
      },
      chartSeries: [],
    },
  };
}

export default {
  components: {
    // FieldInputLabel,
    // ComboBox,
    // RadioButton,
    VCalendar,
    BasePopup,
    Tooltip,
    PieChart,
  },
  data() {
    return initState(false);
  },
  mounted() {
    this.fetchData();
  },
  methods: {
    fetchData() {},
    /**
     * Hàm mở form
    
     */
    async openForm(employeeId) {
      //Gán lại giá trị của form
      await Object.assign(this.$data, initState(false));
      this.showForm = true;

      //Nếu là form sửa
      if (employeeId.length > 0) {
        console.log("ee: ", this.employee);
        //Xác định formType
        // Replace this with your API endpoint
        const url = `https://localhost:44384/api/TimeKeeping/GetByMonth/${employeeId}`;
        // Make an API request
        await axios
          .get(url)
          .then((response) => {
            console.log(response.data);
            const data = response.data;
            this.year = data.year;
            this.month = data.month - 1;
            const times = data.times;
            times.forEach((time) => {
              this.workingDays.push(time);
            });
            console.log(this.workingDays);
            this.early = response.data.early;
            this.late = response.data.late;
            this.dayoff = response.data.dayoff;
            this.pieData.chartSeries.push(this.early),
              (this.pieData.chartSeries[1] = this.late),
              (this.pieData.chartSeries[2] = this.dayoff);
            console.log(this.pieData.chartSeries);
          })
          .catch((error) => console.error(error));
      }

      // //Focus vào ô đầu tiên
      this.$refs[this.employeeCodeInput.inputFieldName].$vnode.elm
        .querySelector(".field-input")
        .focus();

      //Lấy dữ liệu gốc để đối chiếu xem người dùng đã thay đổi dữ liệu chưa
      this.originData = JSON.parse(JSON.stringify(this.employee));
    },

    /**
     * Hàm update dữ liệu khi người dùng thay đổi
    
     */
    updateValueInput(key, value) {
      this.$set(this.employee, key, value);
    },

    /**
     * Hàm lấy ra dữ liệu gốc
 
     */
    getOriginData(key, value) {
      this.$set(this.originData, key, value);
    },

    /**
     * Hàm đóng form

     */
    closeForm() {
      if (this.dataChanged()) {
        this.showConfirmPopup = true;
      } else {
        this.showForm = false;
      }
    },

    /**
     * Hàm kiểm tra dùng đã thay đổi dữ liệu trên form
    
     */
    dataChanged() {
      // this.userEdit = false;
      for (let prop in this.employee) {
        if (this.employee[prop] != null) {
          if (this.employee[prop].toString().length == 0) {
            if (this.originData[prop]) {
              return true;
            }
          } else if (this.employee[prop] !== this.originData[prop]) return true;
        }
      }

      return false;
    },

    /**
     * Hàm thêm và thoát form
 
     */
    async saveAndOut() {
      await this.saveData();

      //Nếu thêm thành công
      if (this.allInputValid) {
        this.showForm = false;
      }
    },

    /**
     * Hàm cất và thêm dữ liệu
  
     */
    async saveAndAdd() {
      this.saveValidate = true;
      await this.saveData();

      //Nếu thêm thành công
      if (this.allInputValid) {
        this.saveValidate = false;
        this.employee = {};

        //Lấy lại mã nhan viên
        await EmployeesAPI.getNewEmployeeCode()
          .then((response) => {
            this.employee.employeeCode = response.data;

            //Lấy dữ liệu gốc để đối chiếu xem người dùng đã thay đổi dữ liệu chưa
            this.originData = JSON.parse(JSON.stringify(this.employee));
          })
          .catch(() => {
            //Nếu không lưu được thì thông báo lỗi
            this.allInputValid = false;
            this.errorMessage = Resource.Message.ServerError;
            this.showErrorPopup = true;
          });
      }
    },

    /**
     * Hàm gọi api check unique
  
     */
    async checkUnique(propName, propValue) {
      await EmployeesAPI.getEmployeeByProperty(propName, propValue).then(
        async (response) => {
          switch (this.formType) {
            case Enumeration.FormMode.Add:
              if (response.status != 204) {
                this.allUniue = false;
              }
              break;

            case Enumeration.FormMode.Clone:
              if (response.status != 204) {
                this.allUniue = false;
              }
              break;

            case Enumeration.FormMode.Edit:
              if (response.data.employeeId != this.employee.employeeId) {
                this.allUniue = false;
              }
              break;
          }
        }
      );
    },

    /**
     * Hàm Lưu dữ liệu

     */
    async saveData() {
      //Biến đánh dấu để hiện popup
      this.saveValidate = true;

      //Trước khi lưu cần validate hết
      await this.validateAll();

      if (this.allInputValid) {
        switch (this.formType) {
          //Nếu là form thêm
          case Enumeration.FormMode.Add:
          case Enumeration.FormMode.Clone:
            await EmployeesAPI.insert(this.employee)
              .then((response) => {
                if (response.data.code == 200) {
                  //Hiển thị toast message
                  this.$store.commit("SHOW_TOAST", {
                    toastType: Resource.Toast.Success,
                    toastMessage: Resource.Message.AddSuccess,
                  });
                }

                this.$emit("refreshData");
              })
              .catch((e) => {
                //Nếu không lưu được thì thông báo lỗi
                this.allInputValid = false;
                this.errorMessage = e.response.data.data.status[0]
                  ? e.response.data.data.status[0]
                  : Resource.Message.ServerError;
                this.showErrorPopup = true;
              });
            break;
          //Nếu là form sửa
          case Enumeration.FormMode.Edit:
            await EmployeesAPI.update(this.employeeId, this.employee)
              .then((response) => {
                if (response.data.code == 200) {
                  //Hiển thị toast message
                  this.$store.commit("SHOW_TOAST", {
                    toastType: Resource.Toast.Success,
                    toastMessage: Resource.Message.EditSuccess,
                  });
                }

                this.$emit("refreshData");
              })
              .catch(() => {
                //Nếu không lưu được thì thông báo lỗi
                this.allInputValid = false;
                this.errorMessage = Resource.Message.ServerError;
                this.showErrorPopup = true;
              });
            break;
        }
      }
    },

    /**
     * Hàm validate tất cả dữ liệu trước khi thêm
 
     */
    async validateAll() {
      //Reset
      this.allInputValid = true;
      this.allUniue = true;

      //Lấy tất cả trường cần validate
      let elValidate = this.$refs.FormData.querySelectorAll("[MustValidate]");

      //Validate tất cả trường
      for (let i = 0; i < elValidate.length; i++) {
        await elValidate[i].querySelector(".field-input").focus();
        await elValidate[i].querySelector(".field-input").blur();
      }

      await this.checkUnique("EmployeeCode", this.employee.employeeCode);

      if (!this.allUniue) {
        let errorMessage = CommonFn.getUniqueErrorMsg(
          "Mã nhân viên <" + this.employee.employeeCode + ">"
        );

        await this.invalidData(errorMessage, Enumeration.ErrorType.Unique);
      }
    },

    /**
     * Đánh dấu dữ liệu chưa hợp lệ
     */
    invalidData(errorMessage, typeError) {
      this.allInputValid = false;

      //Nếu là ấn nút save và chưa có lỗi nào trước đó thì lấy lỗi đầu tiên để hiển thị
      if (!this.errorMessage && this.saveValidate) {
        this.errorMessage = errorMessage;

        //Tùy vào các loại lỗi thì hiển thị popup tương ứng
        switch (typeError) {
          case Enumeration.ErrorType.Require:
          case Enumeration.ErrorType.DataType:
          case Enumeration.ErrorType.MaxLength:
            this.uniqueError = false;
            break;
          case Enumeration.ErrorType.Unique:
            this.uniqueError = true;
            break;
        }

        this.showErrorPopup = true;
      }
    },

    /**
     * Hàm thực hiện đóng popup lỗi
 
     */
    closeErrorPopup() {
      this.showErrorPopup = false;

      //Reset lại giá trị all valid
      this.allInputValid = true;

      //Reset lại thông điệp lỗi
      this.errorMessage = "";

      //reset biến đánh dấu ấn save
      this.saveValidate = false;
    },

    /**
     * Hàm thực hiện khi nhấn chọn là khách hàng

     */
    selectCustomerBox() {
      this.customerBoxSelected = !this.customerBoxSelected;
    },

    /**
     * Hàm thực hiện khi nhấn chọn là nhà cung cấp

     */
    selectProviderBox() {
      this.providerBoxSelected = !this.providerBoxSelected;
    },
  },
};
</script>

<style scoped>
@import url("../../assets/css/views/employees/form.css");
@import url("../../assets/css/components/popup.css");

.slide-leave-active,
.slide-enter-active {
  transition: all 0.5s ease;
}
.slide-enter,
.slide-leave-to {
  transform: translateY(-50%);
  opacity: 0;
}
.timeKeeping__element {
  padding: 10px;
  margin: 5px;
  width: 150px;
  border-radius: 15px;
}
.tk_row {
  padding-right: 5px;
  font-weight: 1000;
  color: white;
}
.day-off {
  background-color: red;
}
.working {
  background-color: blue;
}
.late {
  background-color: orange;
}
.info {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}
</style>