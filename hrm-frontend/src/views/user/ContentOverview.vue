<template>
  <div class="content">
    <HeaderContent @openForm="openForm" />
    <div class="content__main"></div>
    <Form ref="Form" @refreshData="refreshData" />
    <overview style="margin-top: 20px" />
    <!-- Popup confirm xóa một bản ghi -->
    <BasePopup v-show="showDeletePopup">
      <template #content>
        <div class="page-icon popup-icon">
          <div class="popup__icon-warning"></div>
        </div>
        <div class="popup__text">
          Bạn có thực sự muốn xóa Nhân viên {{ deleteString }} không?
        </div>
      </template>
      <template #footer>
        <div
          class="btn btn-default btn-cancel"
          @click="showDeletePopup = false"
        >
          Không
        </div>
        <div class="btn btn-primary" @click="deleteSelectedEmployee">Có</div>
      </template>
    </BasePopup>

    <!-- Popup confirm xóa nhiều -->
    <BasePopup v-show="showMultipleDeletePopup">
      <template #content>
        <div class="page-icon popup-icon">
          <div class="popup__icon-warning"></div>
        </div>
        <div class="popup__text">
          Bạn có thực sự muốn xóa
          <span class="text-semibold">{{
            staffTable.currentSelectedRows.length
          }}</span>
          Nhân viên không?
        </div>
      </template>
      <template #footer>
        <div
          class="btn btn-default btn-cancel"
          @click="showMultipleDeletePopup = false"
        >
          Không
        </div>
        <div class="btn btn-primary" @click="deleteSelectedRows">Có</div>
      </template>
    </BasePopup>

    <!-- Popup khi ấn xóa mà chưa chọn gì -->
    <BasePopup class="popup-error" v-show="showNoRecordPopup">
      <template #content>
        <div class="page-icon popup-icon">
          <div class="popup__icon-warning"></div>
        </div>
        <div class="popup__text">
          Vui lòng chọn ít nhất một bản ghi trước khi xóa
        </div>
      </template>
      <template #footer>
        <div class="footer__btn d-flex flex-center">
          <div class="btn btn-primary" @click="showNoRecordPopup = false">
            Đóng
          </div>
        </div>
      </template>
    </BasePopup>

    <!-- popup thông báo lỗi -->
    <BasePopup class="popup-error" v-show="showErrorPopup">
      <template #content>
        <div class="page-icon popup-icon">
          <div class="popup__icon-danger"></div>
        </div>
        <div class="popup__text">Có lỗi xảy ra, vui lòng liên hệ</div>
      </template>
      <template #footer>
        <div class="footer__btn d-flex flex-center">
          <div class="btn btn-primary" @click="showErrorPopup = false">
            Đóng
          </div>
        </div>
      </template>
    </BasePopup>
  </div>
</template>
  
  <script>
import HeaderContent from "./HeaderContent.vue";
//import ContentUtility from "./ContentUtility.vue";
import Form from "./Form.vue";
//import Table from "../../components/table/Table.vue";
//import BasePopup from "../../components/BasePopup.vue";
import EmployeesAPI from "../../api/components/employees/EmployeesAPI";
import Resource from "../../js/common/Resource";
import Overview from "./Overview";
//import DepartmentAPI from "../../api/components/departments/DepartmentsAPI";
// import { log } from "console";

export default {
  components: {
    HeaderContent,
    Overview,
    // ContentUtility,
    // Table,
    Form,
    //  BasePopup,
  },
  data() {
    return {
      //defaultValue: "Tất cả phòng ban", // default value for the select box
      // departmentID: "",
      staffTable: {
        column: [
          {
            selectBoxColumn: true,
          },
          {
            columnName: "STT",
            dataType: Resource.DataTypeColumn.OrderNumber,
          },
          {
            columnName: "Mã nhân viên",
            fieldName: "staffCode",
          },
          {
            columnName: "Tên nhân viên",
            fieldName: "staffName",
          },
          {
            columnName: "Giới tính",
            fieldName: "gender",
            dataType: Resource.DataTypeColumn.Enum,
            enumName: "Gender",
          },
          {
            columnName: "Ngày sinh",
            fieldName: "dateOfBirth",
            dataType: Resource.DataTypeColumn.Date,
          },
          {
            columnName: "Số CMND",
            fieldName: "identityCard",
          },
          {
            columnName: "Chức danh",
            fieldName: "positionName",
          },
          {
            columnName: "Tên đơn vị",
            fieldName: "departmentName",
          },
          {
            columnName: "Số tài khoản",
            fieldName: "bankAccount",
          },
          {
            columnName: "Tên ngân hàng",
            fieldName: "bankName",
          },
          {
            columnName: "Chi nhánh ngân hàng",
            fieldName: "bankBranchName",
          },
          {
            columnName: "Chức năng",
            functionColumn: true,
          },
        ],
        functions: ["Nhân bản", "Xóa", "Ngưng sử dụng"],
        defaultFunction: "Sửa",
        gridData: null,
        currentSelectedRows: [],
        idFieldName: "guid",
        pageSize: 10,
        currentPageNum: 1,
        totalPage: 1,
        maxPageNumDisplay: 5,
        totalRecord: 0,
        filterValue: null,
        departmentID: null,
        positionID: null,
      },
      showDeletePopup: false,
      showMultipleDeletePopup: false,
      showErrorPopup: false,
      showNoRecordPopup: false,
      employeeDelete: null,
      deleteString: "",
    };
  },
  created() {
    this.getDataServer();
    // this.getDepartmentData();
  },
  mounted() {},
  methods: {
    /**
     * Hàm lấy dữ liệu trên server
     */
    async getDataServer() {
      this.$store.commit("SHOW_LOADER", true);
      console.log(this.staffTable.departmentID);
      await EmployeesAPI.filter(
        this.staffTable.pageSize,
        this.staffTable.currentPageNum,
        this.staffTable.filterValue,
        this.staffTable.departmentID,
        this.staffTable.positionID
      )
        .then((response) => {
          console.log(response.data.data);
          this.staffTable.gridData = response.data.data;
          this.staffTable.totalRecord = response.data.infoPage.totalRecord;
          this.staffTable.totalPage = response.data.infoPage.totalPage;
        })
        .catch(() => {
          this.showErrorPopup = true;
          this.staffTable.gridData = [];
          this.$store.commit("SHOW_LOADER", false);
        });

      await this.$store.commit("SHOW_LOADER", false);

      this.$refs.Table.resetCurrentSelectedRows();
    },

    /**
     * Hàm refresh dữ liệu
     */
    refreshData() {
      //Reset lại các dữ liệu
      this.staffTable.currentPageNum = 1;

      this.getDataServer();
      this.getDepartmentData();
    },

    /**
     * Hàm mở form thêm sửa
     */
    openForm(employee) {
      if (employee) {
        this.$refs.Form.openForm(employee.guid);
      } else {
        this.$refs.Form.openForm("");
      }
    },

    /**
     * Hàm xử lý sự kiện của item function
     */
    clickFunctionItem(fn, item) {
      switch (fn) {
        case "Xóa":
          this.deleteString = "<" + item.staffCode + ">";
          console.log("item: ", item);
          this.employeeDelete = item;
          this.showDeletePopup = true;
          break;
        case "Nhân bản":
          this.$refs.Form.openForm("", item);
          break;
      }
    },

    /**
     * Hàm xóa một nhân viên đã chọn
     */
    async deleteSelectedEmployee() {
      await EmployeesAPI.delete(this.employeeDelete.guid)
        .then(async (response) => {
          if (response.status != 204) {
            //Tắt popup
            this.showDeletePopup = false;

            //Reload dữ liệu
            await this.getDataServer();

            //Hiển thị toast message
            this.$store.commit("SHOW_TOAST", {
              toastType: "success",
              toastMessage: Resource.Message.DeleteSuccess,
            });
          }
        })
        .catch(() => {
          this.showErrorPopup = true;
        });
    },

    /**
     * Hàm filter dữ liệu
     */
    async getDataFilter(filterValue) {
      this.staffTable.filterValue = filterValue;
      this.staffTable.currentPageNum = 1;

      this.getDataServer();
    },
    /**
     * Lay id phong ban
     * @param {Id phong ban} val
     */
    async departmentCombobox(val, id) {
      this.staffTable.departmentID = id;
      this.staffTable.currentPageNum = 1;
      console.log("departmentID: ", val);
      this.getDataServer();
    },
    /**
     * Lay id chuc danh
     */

    async positionCombobox(val, id) {
      this.staffTable.positionID = id;
      this.staffTable.currentPageNum = 1;
      console.log("positionID: ", val);
      this.getDataServer();
    },
    /**
     * Hàm thay đổi page size
     */
    changePageSize(pageSize) {
      this.staffTable.pageSize = pageSize;

      //Reset page nubmer
      this.staffTable.currentPageNum = 1;

      //Lấy lại dữ liệu
      this.getDataServer();
    },

    /**
     * Hàm thay đổi page nubmer
     */
    changePageNum(pageNum) {
      this.staffTable.currentPageNum = pageNum;
      this.getDataServer();
    },

    /**
     * Hàm export dữ liệu sang file Excel
     */
    exportData() {
      EmployeesAPI.exportData(this.staffTable.filterValue)
        .then((response) => {
          if (response) {
            const blob = new Blob([response.data], {
              type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            });
            const link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = "Danh sách nhân viên";
            link.click();
            URL.revokeObjectURL(link.href);
          }
        })
        .catch(() => {
          this.showErrorPopup = true;
        });
    },

    /**
     * Hàm thay đổi list bản ghi được chọn
     */
    changeListSelectedRow(list) {
      this.staffTable.currentSelectedRows = JSON.parse(JSON.stringify(list));
    },

    /**
     * Hàm mở popup xóa nhiều
     */
    openMultipleDeletePopup() {
      //Nếu chỉ có một nhân viên thì hiện popup xóa 1
      if (this.staffTable.currentSelectedRows.length == 1) {
        this.employeeDelete =
          this.staffTable.gridData[this.staffTable.currentSelectedRows[0]];

        this.deleteString = "<" + this.employeeDelete.staffCode + ">";

        this.showDeletePopup = true;
      } else {
        this.showMultipleDeletePopup = true;
      }
    },

    /**
     * Hàm xóa tất cả bản ghi đã được chọn
     */
    async deleteSelectedRows() {
      let listId = [];

      //Lấy ra list id bản ghi đã chọn
      this.staffTable.currentSelectedRows.map((item) => {
        listId.push(this.staffTable.gridData[item].guid);
      });

      //Xóa
      await EmployeesAPI.multipleDelete(listId)
        .then(async (response) => {
          if (response.status != 204) {
            //Tắt popup
            this.showMultipleDeletePopup = false;

            //Reload dữ liệu
            await this.getDataServer();

            //Hiển thị toast message
            this.$store.commit("SHOW_TOAST", {
              toastType: "success",
              toastMessage: Resource.Message.DeleteSuccess,
            });
          }
        })
        .catch(() => {
          this.showErrorPopup = true;
        });
    },
  },
};
</script>
  
  <style scoped>
.content {
  padding: 0 20px;
  background-color: #f4f5f8;
  height: calc(100vh - var(--header-height) - 64px);
}

.content__main {
  background-color: #fff;
  max-height: calc(100vh - var(--header-height) - 86px);
  padding: 0 16px;
  border-radius: 0 0 10px 10px;
}
</style>