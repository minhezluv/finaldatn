<template>
  <div class="content__util">
    <combobox
      :customData="customYearData"
      style="margin-right: 20px"
      :model="'2023'"
      @valueChanged="valueYearChanged"
    ></combobox>
    <combobox
      :customData="customMonthData"
      style="margin-right: 20px"
      :model="'Tháng'"
      @valueChanged="valueMonthChanged"
    ></combobox>
    <combobox
      :customData="customDayData"
      style="margin-right: 20px"
      :model="'Năm'"
      @valueChanged="valueDayChanged"
    ></combobox>
    <combobox
      :customData="customPositionData"
      style="margin-right: 20px"
      :model="'Tất cả chức danh'"
      @valueChanged="valuePositionChanged"
    ></combobox>
    <combobox
      :customData="customData"
      style="margin-right: 20px"
      :model="'Tất cả phòng ban'"
      @valueChanged="valueChanged"
    ></combobox>

    <div class="search-box" :class="{ 'search-box-active': searchFocused }">
      <input
        type="text"
        class="field-input__input"
        placeholder="Tìm theo mã, tên hoặc số điện thoại"
        v-debounce:400ms="filterDataTable"
        @focus="searchFocused = true"
        @blur="searchFocused = false"
      />
      <div class="page-icon">
        <div class="util__icon-search"></div>
      </div>
    </div>
    <div class="btn-refresh page-icon" @click="refreshData">
      <Tooltip :customData="'Làm mới'">
        <div class="util__icon-refresh"></div>
      </Tooltip>
    </div>
    <div class="btn-export page-icon" @click="exportData">
      <Tooltip :customData="'Xuất khẩu'">
        <div class="util__icon-export"></div>
      </Tooltip>
    </div>
    <div class="page-icon" @click="deleteSelectedRows">
      <Tooltip style="height: 20px" :customData="'Xóa bản ghi'">
        <svg
          width="20px"
          height="20px"
          opacity="0.4"
          aria-hidden="true"
          focusable="false"
          data-prefix="fas"
          data-icon="trash-alt"
          class="svg-inline--fa fa-trash-alt fa-w-14"
          role="img"
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 448 512"
        >
          <path
            fill="currentColor"
            d="M32 464a48 48 0 0 0 48 48h288a48 48 0 0 0 48-48V128H32zm272-256a16 16 0 0 1 32 0v224a16 16 0 0 1-32 0zm-96 0a16 16 0 0 1 32 0v224a16 16 0 0 1-32 0zm-96 0a16 16 0 0 1 32 0v224a16 16 0 0 1-32 0zM432 32H312l-9.4-18.7A24 24 0 0 0 281.1 0H166.8a23.72 23.72 0 0 0-21.4 13.3L136 32H16A16 16 0 0 0 0 48v32a16 16 0 0 0 16 16h416a16 16 0 0 0 16-16V48a16 16 0 0 0-16-16z"
          ></path>
        </svg>
      </Tooltip>
    </div>
  </div>
</template>

<script>
import Tooltip from "../../components/Tooltip.vue";
import DepartmentAPI from "../../api/components/departments/DepartmentsAPI";
import Resource from "../../js/common/Resource";
import Combobox from "../../components/Combobox.vue";
import PositionsAPI from "../../api/components/positions/PositionsAPI";
export default {
  components: {
    Tooltip,
    Combobox,
  },
  data() {
    return {
      searchFocused: false,
      refreshFocused: false,
      exportFocused: false,
      customData: {
        height: 32,
        width: 200,
        displayValues: ["Tất cả phòng ban"],
        keys: ["0"],
        // labelText: "Select an item",
        defaultValue: "Tất cả phòng ban", // use default value from grandparent
      },
      customPositionData: {
        height: 32,
        width: 200,
        displayValues: ["Tất cả chức danh"],
        keys: ["0"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "Tất cả chức danh",
      },

      customDayData: {
        height: 32,
        width: 150,
        displayValues: ["Tất cả ngày"],
        keys: ["0"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "Tất cả ngày",
      },
      customMonthData: {
        height: 32,
        width: 150,
        displayValues: ["Tất cả tháng"],
        keys: ["0"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "Tất cả tháng",
      },
      customYearData: {
        height: 32,
        width: 150,
        displayValues: ["2023", "2022"],
        keys: ["2023", "2022"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "2023",
      },
    };
  },
  created() {
    this.getDepartmentData();
    this.getPositionData();
    this.generateDay();
    this.generateMonth();
  },
  methods: {
    async generateDay() {
      for (var i = 1; i <= 31; i++) {
        this.customDayData.displayValues.push(i);
        this.customDayData.keys.push(i);
      }
    },
    async generateMonth() {
      for (var i = 1; i <= 12; i++) {
        this.customMonthData.displayValues.push(i);
        this.customMonthData.keys.push(i);
      }
    },

    async valueDayChanged(val, id) {
      this.$emit("dayCurrent", val, id);
    },

    async valueYearChanged(val, id) {
      this.$emit("yearCurrent", val, id);
    },
    async valueMonthChanged(val, id) {
      this.$emit("monthCurrent", val, id);
    },
    async getDepartmentData() {
      //Lấy department để bind lên combobox
      // console.log("hihi: ", this.comboboxData);
      await DepartmentAPI.getAll()
        .then((response) => {
          console.log(response.data);
          //    this.comboboxData = [];
          response.data.map((department) => {
            this.customData.displayValues.push(department.departmentName);

            this.customData.keys.push(department.guid);
          });

          // console.log("combobox: ", this.comboboxData);
        })
        .catch(() => {
          //Nếu không lấy được phòng ban
          this.allInputValid = false;
          this.errorMessage = Resource.Message.GetDepartmentError;
          this.showErrorPopup = true;
        });
    },
    /**
     * Lấy thông tin chức danh
     */
    async getPositionData() {
      //Lấy department để bind lên combobox
      // console.log("hihi: ", this.comboboxData);
      await PositionsAPI.getAll()
        .then((response) => {
          console.log(response.data);
          //    this.comboboxData = [];
          response.data.map((position) => {
            this.customPositionData.displayValues.push(position.positionName);

            this.customPositionData.keys.push(position.guid);
          });

          console.log("combobox: ", this.customPositionData);
        })
        .catch(() => {
          //Nếu không lấy được phòng ban
          this.allInputValid = false;
          this.errorMessage = Resource.Message.GetDepartmentError;
          this.showErrorPopup = true;
        });
    },

    /**
     * Hàm refresh dữ liệu
  
     */
    refreshData() {
      this.$emit("refreshData");
    },

    /**
     * Hàm filter dữ liệu trên bảng
    
     */
    filterDataTable(val) {
      this.$emit("filterDataTable", val);
    },

    /**
     * Hàm export dữ liệu sang excel
    12/07/2021
     */
    exportData() {
      this.$emit("exportData");
    },

    /**
     * Hàm xóa bản ghi đã chọn

     */
    deleteSelectedRows() {
      this.$emit("deleteSelectedRows");
    },

    valueChanged(val, id) {
      console.log("val: ", val);
      this.$emit("departmentCurrent", val, id);
    },
    valuePositionChanged(val, id) {
      console.log("posID: ", id);
      this.$emit("positionCurrent", val, id);
    },
  },
};
</script>

<style scoped>
@import url("../../assets/css/views/employees/contentUtility.css");
</style>