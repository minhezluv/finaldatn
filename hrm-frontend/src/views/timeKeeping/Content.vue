<template>
  <div class="content">
    <HeaderContent @openForm="openForm" />
    <div class="content__main">
      <ContentUtility
        @refreshData="refreshData"
        @filterDataTable="getDataFilter"
        @exportData="exportData"
        @departmentCurrent="departmentCombobox"
        @positionCurrent="positionCombobox"
        @yearCurrent="yearCombobox"
        @dayCurrent="dayCombobox"
        @monthCurrent="monthCombobox"
        @deleteSelectedRows="
          LCTable.currentSelectedRows.length > 0
            ? openMultipleDeletePopup()
            : (showNoRecordPopup = true)
        "
      />
      <Table
        ref="Table"
        :customData="TimeKeepingTable"
        @dbClickRow="openForm"
        @clickDefaultFunction="openForm"
        @clickFunctionItem="clickFunctionItem"
        @clickPageNum="changePageNum"
        @changePageSize="changePageSize"
        @changeListSelectedRow="changeListSelectedRow"
      />
    </div>
    <Form ref="Form" @refreshData="refreshData" />

    <!-- Popup confirm xóa một bản ghi -->
    <BasePopup v-show="showDeletePopup">
      <template #content>
        <div class="page-icon popup-icon">
          <div class="popup__icon-warning"></div>
        </div>
        <div class="popup__text">
          Bạn có thực sự muốn xóa công {{ deleteString }} không?
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
            TimeKeepingTable.currentSelectedRows.length
          }}</span>
          Chấm công không?
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
import ContentUtility from "./ContentUtility.vue";
import Form from "./Form.vue";
import Table from "../../components/table/Table.vue";
import BasePopup from "../../components/BasePopup.vue";
import TimeKeepingAPI from "../../api/components/timekeepings/TimeKeepingsAPI";
import Resource from "../../js/common/Resource";
// import { log } from "console";

export default {
  components: {
    HeaderContent,
    ContentUtility,
    Table,
    Form,
    BasePopup,
  },
  data() {
    return {
      TimeKeepingTable: {
        departmentID: null,
        positionID: null,
        dayID: null,
        yearID: "2023",
        monthID: "0",
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
            columnName: "Thời gian đến",
            fieldName: "start",
          },
          {
            columnName: "Thời gian về",
            fieldName: "end",
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
  },
  methods: {
    /**
     * Lay id nay
     * @param {Id nay}
     */
    async monthCombobox(val, id) {
      this.TimeKeepingTable.monthID = id;
      this.TimeKeepingTable.currentPageNum = 1;
      console.log("dayID: ", val);
      this.getDataServer();
    },
    /**
     * Lay id nay
     * @param {Id nay}
     */
    async dayCombobox(val, id) {
      this.TimeKeepingTable.dayID = id;
      this.TimeKeepingTable.currentPageNum = 1;
      console.log("dayID: ", val);
      this.getDataServer();
    },
    /**
     * Lay id nay
     * @param {Id nay}
     */
    async yearCombobox(val, id) {
      this.TimeKeepingTable.yearID = id;
      this.TimeKeepingTable.currentPageNum = 1;
      console.log("dayID: ", val);
      this.getDataServer();
    },
    /**
     * Lay id phong ban
     * @param {Id phong ban} val
     */
    async departmentCombobox(val, id) {
      this.TimeKeepingTable.departmentID = id;
      this.TimeKeepingTable.currentPageNum = 1;
      console.log("departmentID: ", val);
      this.getDataServer();
    },
    /**
     * Lay id chuc danh
     */

    async positionCombobox(val, id) {
      this.TimeKeepingTable.positionID = id;
      this.TimeKeepingTable.currentPageNum = 1;
      console.log("positionID: ", val);
      this.getDataServer();
    },

    /**
     * Hàm lấy dữ liệu trên server
     */
    async getDataServer() {
      this.$store.commit("SHOW_LOADER", true);

      await TimeKeepingAPI.filter(
        this.TimeKeepingTable.pageSize,
        this.TimeKeepingTable.currentPageNum,
        this.TimeKeepingTable.filterValue,
        this.TimeKeepingTable.departmentID,
        this.TimeKeepingTable.positionID,
        this.TimeKeepingTable.yearID,
        this.TimeKeepingTable.monthID,
        this.TimeKeepingTable.dayID
      )
        .then((response) => {
          console.log(response.data);
          this.TimeKeepingTable.gridData = response.data.entities;
          this.TimeKeepingTable.totalRecord =
            response.data.infoPage.totalRecord;
          this.TimeKeepingTable.totalPage = response.data.infoPage.totalPage;
        })
        .catch(() => {
          this.showErrorPopup = true;
          this.TimeKeepingTable.gridData = [];
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
      this.TimeKeepingTable.currentPageNum = 1;

      this.getDataServer();
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
          this.deleteString = "<" + item.employeeCode + ">";
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
      await TimeKeepingAPI.delete(this.employeeDelete.guid)
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
      this.TimeKeepingTable.filterValue = filterValue;
      this.TimeKeepingTable.currentPageNum = 1;
      console.log("filter: ", filterValue);
      this.getDataServer();
    },

    /**
     * Hàm thay đổi page size
     */
    changePageSize(pageSize) {
      this.TimeKeepingTable.pageSize = pageSize;

      //Reset page nubmer
      this.TimeKeepingTable.currentPageNum = 1;

      //Lấy lại dữ liệu
      this.getDataServer();
    },

    /**
     * Hàm thay đổi page nubmer
     */
    changePageNum(pageNum) {
      this.TimeKeepingTable.currentPageNum = pageNum;
      this.getDataServer();
    },

    /**
     * Hàm export dữ liệu sang file Excel
     */
    exportData() {
      TimeKeepingAPI.exportData(this.TimeKeepingTable.filterValue)
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
      this.TimeKeepingTable.currentSelectedRows = JSON.parse(
        JSON.stringify(list)
      );
    },

    /**
     * Hàm mở popup xóa nhiều
     */
    openMultipleDeletePopup() {
      //Nếu chỉ có một nhân viên thì hiện popup xóa 1
      if (this.TimeKeepingTable.currentSelectedRows.length == 1) {
        this.employeeDelete =
          this.TimeKeepingTable.gridData[
            this.TimeKeepingTable.currentSelectedRows[0]
          ];

        this.deleteString = "<" + this.employeeDelete.employeeCode + ">";

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
      this.TimeKeepingTable.currentSelectedRows.map((item) => {
        listId.push(this.TimeKeepingTable.gridData[item].employeeId);
      });

      //Xóa
      await TimeKeepingAPI.multipleDelete(listId)
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
  height: calc(100vh - var(--header-height) -100px);
}

.content__main {
  background-color: #fff;
  max-height: calc(100vh - var(--header-height) - 86px);
  padding: 0 16px;
}
</style>