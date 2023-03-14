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
        <VCalendar
          v-if="calendarShow"
          :year="year"
          :month="month"
          :workingDays="workingDays"
        />
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
//import DepartmentAPI from "../../api/components/departments/DepartmentsAPI";
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
    calendarShow: false,
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
        const url = `http://hrm3-env.eba-qghius76.ap-southeast-1.elasticbeanstalk.com/api/TimeKeeping/GetByMonth/${employeeId}`;
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
            this.calendarShow = true;
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
      this.showForm = false;
    },

    /**
     * Hàm kiểm tra dùng đã thay đổi dữ liệu trên form
    
     */

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