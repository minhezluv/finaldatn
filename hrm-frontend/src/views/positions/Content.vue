<template>
  <div class="content">
    <HeaderContent @openForm="openForm" />
    <div class="content__main">
      <ContentUtility
        @refreshData="refreshData"
        @filterDataTable="getDataFilter"
        @exportData="exportData"
        @deleteSelectedRows="
          positionTable.currentSelectedRows.length > 0
            ? openMultipleDeletePopup()
            : (showNoRecordPopup = true)
        "
      />
      <Table
        ref="Table"
        :customData="positionTable"
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
          Bạn có thực sự muốn xóa chức danh {{ deleteString }} không?
        </div>
      </template>
      <template #footer>
        <div
          class="btn btn-default btn-cancel"
          @click="showDeletePopup = false"
        >
          Không
        </div>
        <div class="btn btn-primary" @click="deleteSelectedposition">Có</div>
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
            positionTable.currentSelectedRows.length
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
import ContentUtility from "./ContentUtility.vue";
import Form from "./Form.vue";
import Table from "../../components/table/Table.vue";
import BasePopup from "../../components/BasePopup.vue";
import positionsAPI from "../../api/components/positions/PositionsAPI";
import Resource from "../../js/common/Resource";
//import positionAPI from "../../api/components/positions/positionsAPI";
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
      //defaultValue: "Tất cả phòng ban", // default value for the select box
      // positionID: "",
      positionTable: {
        column: [
          {
            selectBoxColumn: true,
          },
          {
            columnName: "STT",
            dataType: Resource.DataTypeColumn.OrderNumber,
          },
          {
            columnName: "Mã chức danh",
            fieldName: "positionCode",
          },
          {
            columnName: "Tên chức danh",
            fieldName: "positionName",
          },
          {
            columnName: "Mô tả",
            fieldName: "note",
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
        // positionID: null,
      },
      showDeletePopup: false,
      showMultipleDeletePopup: false,
      showErrorPopup: false,
      showNoRecordPopup: false,
      positionDelete: null,
      deleteString: "",
    };
  },
  created() {
    this.getDataServer();
    // this.getpositionData();
  },
  mounted() {},
  methods: {
    /**
     * Hàm lấy dữ liệu trên server
     */
    async getDataServer() {
      this.$store.commit("SHOW_LOADER", true);
      // console.log(this.positionTable.guid);
      await positionsAPI
        .filter(this.positionTable.filterValue)
        .then((response) => {
          console.log(response.data);
          this.positionTable.gridData = response.data;
          this.positionTable.totalRecord = response.data.length;
          this.positionTable.totalPage = 1;
        })
        .catch(() => {
          this.showErrorPopup = true;
          this.positionTable.gridData = [];
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
      this.positionTable.currentPageNum = 1;

      this.getDataServer();
      // this.getpositionData();
    },

    /**
     * Hàm mở form thêm sửa
     */
    openForm(position) {
      if (position) {
        this.$refs.Form.openForm(position.guid);
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
          this.deleteString = "<" + item.positionCode + ">";
          console.log("item: ", item);
          this.positionDelete = item;
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
    async deleteSelectedposition() {
      await positionsAPI
        .delete(this.positionDelete.guid)
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
      this.positionTable.filterValue = filterValue;
      this.positionTable.currentPageNum = 1;

      this.getDataServer();
    },

    /**
     * Hàm thay đổi page size
     */
    changePageSize(pageSize) {
      this.positionTable.pageSize = pageSize;

      //Reset page nubmer
      this.positionTable.currentPageNum = 1;

      //Lấy lại dữ liệu
      this.getDataServer();
    },

    /**
     * Hàm thay đổi page nubmer
     */
    changePageNum(pageNum) {
      this.positionTable.currentPageNum = pageNum;
      this.getDataServer();
    },

    /**
     * Hàm export dữ liệu sang file Excel
     */
    exportData() {
      positionsAPI
        .exportData(this.positionTable.filterValue)
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
      this.positionTable.currentSelectedRows = JSON.parse(JSON.stringify(list));
    },

    /**
     * Hàm mở popup xóa nhiều
     */
    openMultipleDeletePopup() {
      //Nếu chỉ có một nhân viên thì hiện popup xóa 1
      if (this.positionTable.currentSelectedRows.length == 1) {
        this.positionDelete =
          this.positionTable.gridData[
            this.positionTable.currentSelectedRows[0]
          ];

        this.deleteString = "<" + this.positionDelete.positionCode + ">";

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
      this.positionTable.currentSelectedRows.map((item) => {
        listId.push(this.positionTable.gridData[item].guid);
      });

      //Xóa
      await positionsAPI
        .multipleDelete(listId)
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