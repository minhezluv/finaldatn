<template>
  <div class="content__util">
    <combobox
      :customData="customStatusData"
      style="margin-right: 20px"
      :model="'Tất cả trạng thái'"
      @valueChanged="valueStatusChanged"
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
//import Dropdown from "../../components/Dropdown.vue";
import DepartmentAPI from "../../api/components/departments/DepartmentsAPI";
import Resource from "../../js/common/Resource";
import Combobox from "../../components/Combobox.vue";
import PositionsAPI from "../../api/components/positions/PositionsAPI";
export default {
  components: {
    Tooltip,
    // Dropdown,
    Combobox,
  },
  props: {},
  created() {
    this.getDepartmentData();
    this.getPositionData();
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
        keys: ["1"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "Tất cả phòng ban",
      },
      customPositionData: {
        height: 32,
        width: 200,
        displayValues: ["Tất cả chức danh"],
        keys: ["1"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "Tất cả chức danh",
      },

      customStatusData: {
        height: 32,
        width: 150,
        displayValues: ["Tất cả trạng thái", "Đang làm việc", "Nghỉ việc"],
        keys: ["0", "2", "1"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "Tất cả trạng thái",
      },
    };
  },
  methods: {
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

          // console.log("combobox: ", this.comboboxData);
        })
        .catch(() => {
          //Nếu không lấy được phòng ban
          this.allInputValid = false;
          this.errorMessage = Resource.Message.GetDepartmentError;
          this.showErrorPopup = true;
        });
    },
    refreshData() {
      this.$emit("refreshData");
    },

    filterDataTable(val) {
      this.$emit("filterDataTable", val);
    },
    valueChanged(val, id) {
      console.log("val: ", val);
      this.$emit("departmentCurrent", val, id);
    },
    valuePositionChanged(val, id) {
      this.$emit("positionCurrent", val, id);
    },
    valueStatusChanged(val, id) {
      this.$emit("statusCurrent", val, id);
    },
    exportData() {
      this.$emit("exportData");
    },

    deleteSelectedRows() {
      this.$emit("deleteSelectedRows");
    },
  },
};
</script>

<style scoped>
@import url("../../assets/css/views/employees/contentUtility.css");
</style>