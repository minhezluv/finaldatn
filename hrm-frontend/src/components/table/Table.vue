<template>
  <div class="grid" ref="Grid">
    <div class="table">
      <table>
        <thead>
          <tr>
            <td
              v-for="(item, index) in customData.column"
              :class="{
                'text-right': item.dataType == Resource.DataTypeColumn.Number,
                'text-center':
                  item.dataType == Resource.DataTypeColumn.Date ||
                  item.dataType == Resource.DataTypeColumn.OrderNumber,
                'col-function text-center': item.functionColumn,
                'col-selectBox': item.selectBoxColumn,
              }"
              :key="index"
              :title="item.columnName"
            >
              <div
                class="select-box"
                v-show="item.selectBoxColumn"
                @click="toggleSelectAllRow"
                :class="{
                  'rotate-90 box-selected':
                    currentSelectedRows.length == customData.pageSize,
                }"
              >
                <div
                  class="table__icon-row-select"
                  v-show="currentSelectedRows.length == customData.pageSize"
                ></div>
              </div>
              {{ item.columnName }}
            </td>
          </tr>
        </thead>
        <tbody ref="tbody">
          <tr
            tabindex="0"
            :ref="index"
            v-for="(item, index) in customData.gridData"
            :ItemId="item[customData.idFieldName]"
            :key="index"
            @mouseenter.exact="rowHover"
            @mouseleave="rowUnhover"
            @click.exact="clickRow(index)"
            @click.ctrl="ctrlClickRow(index)"
            @dblclick="dbClickRow(item)"
            @keydown.up="pressUpArrowKey(index)"
            @keydown.down="pressDownArrowKey(index)"
            @mouseenter.shift="mouseEnterWhenShiftPressed(index)"
            @contextmenu.prevent="rightClickRow($event, index)"
          >
            <td
              v-for="(col, index) in customData.column"
              :class="{
                'text-right': col.dataType == Resource.DataTypeColumn.Number,
                'text-center':
                  col.dataType == Resource.DataTypeColumn.Date ||
                  col.dataType == Resource.DataTypeColumn.OrderNumber,
                'col-function': col.functionColumn,
                'col-selectBox': col.selectBoxColumn,
                'tr-selected': checkRowSelected(
                  customData.gridData.indexOf(item)
                ),
              }"
              :style="[
                col.functionColumn
                  ? {
                      'z-index': dropup
                        ? customData.gridData.indexOf(item)
                        : customData.gridData.length -
                          customData.gridData.indexOf(item),
                    }
                  : {},
              ]"
              :key="index"
              :title="
                getDisplayValue(item[col.fieldName], col.dataType, col.enumName)
              "
            >
              <div
                class="select-box"
                :class="{
                  'rotate-90 box-selected': checkRowSelected(
                    customData.gridData.indexOf(item)
                  ),
                }"
                v-show="col.selectBoxColumn"
                @click.stop="selectBoxClick(customData.gridData.indexOf(item))"
                @dblclick.prevent.stop
              >
                <div
                  class="table__icon-row-select"
                  v-show="
                    currentSelectedRows.includes(
                      customData.gridData.indexOf(item)
                    )
                  "
                ></div>
              </div>
              <div class="function-cell" v-show="col.functionColumn">
                <span
                  class="text-semibold"
                  @click="clickDefaultFunction(item)"
                  >{{ customData.defaultFunction }}</span
                >
                <div
                  class="function-cel__dropdown"
                  tabindex="0"
                  @blur="closeDropdown"
                >
                  <div
                    class="page-icon"
                    :class="{
                      'dropdown-icon-select':
                        currentDropdown == customData.gridData.indexOf(item),
                    }"
                    @click.stop="toggleDropdownFunction($event, item)"
                    @dblclick.prevent.stop
                  >
                    <div class="table__icon-dropdown"></div>
                  </div>
                  <transition name="slide-fade">
                    <div
                      ref="Dropdown"
                      class="dropdown-function"
                      :style="[
                        dropup
                          ? { top: '-' + (dropdownHeight + 6) + 'px' }
                          : {},
                        dropdownRightPosition
                          ? { right: dropdownRightPosition + 'px' }
                          : {},
                      ]"
                      v-if="
                        currentDropdown == customData.gridData.indexOf(item)
                      "
                    >
                      <div
                        class="dropdown-function__item"
                        v-for="(fn, index) in customData.functions"
                        :key="index"
                        @click="clickFunctionItem(fn, item)"
                      >
                        {{ fn }}
                      </div>
                    </div>
                  </transition>
                </div>
              </div>
              {{
                col.dataType == Resource.DataTypeColumn.OrderNumber
                  ? customData.gridData.indexOf(item) + 1
                  : getDisplayValue(
                      item[col.fieldName],
                      col.dataType,
                      col.enumName
                    )
              }}
            </td>
          </tr>
        </tbody>
      </table>
      <div
        class="data-empty"
        v-if="!customData.gridData || customData.gridData.length == 0"
      >
        <div class="data-empty__content">
          <img src="../../assets/images/eport_nodata.76e50bd8.svg" alt="" />
          <span>Kh??ng c?? d??? li???u</span>
        </div>
      </div>
    </div>
    <Paging
      :customData="paging"
      @clickPageNum="clickPageNum"
      @changePageSize="changePageSize"
      ref="Paging"
    />
  </div>
</template>

<script>
import CommonFn from "../../js/common/CommonFn";
import Paging from "./Paging.vue";
import Resource from "../../js/common/Resource";

export default {
  components: {
    Paging,
  },
  props: {
    customData: {
      type: Object,
      required: true,
    },
  },
  mounted() {
    this.displayDropdown = true;
  },
  data() {
    return {
      Resource: Resource,
      currentSelectedRows: [],
      selectedRow: null,
      countClick: 0,
      timer: null,
      currentDropdown: null,
      dropup: false,
      dropdownHeight: null,
      dropdownRightPosition: 0,
      showEmptyData: false,
      paging: {
        pageSize: 1,
        totalRecord: 0,
        totalPage: 1,
        maxPageNumDispaly: 0,
        currentPageNum: 1,
      },
    };
  },
  created() {
    this.paging.pageSize = this.customData.pageSize;
    this.paging.totalRecord = this.customData.totalRecord;
    this.paging.totalPage = this.customData.totalPage;
    this.paging.maxPageNumDisplay = this.customData.maxPageNumDisplay;
    this.paging.currentPageNum = this.customData.currentPageNum;
  },
  watch: {
    customData: {
      deep: true,
      handler(val) {
        //D??? li???u paging
        this.paging.pageSize = val.pageSize;
        this.paging.totalRecord = val.totalRecord;
        this.paging.totalPage = val.totalPage;
        this.paging.maxPageNumDisplay = val.maxPageNumDisplay;
        this.paging.currentPageNum = val.currentPageNum;
      },
    },
    currentSelectedRows: {
      deep: true,
      immediate: true,
      handler(val) {
        this.$emit("changeListSelectedRow", val);
      },
    },
  },
  methods: {
    /**
     * H??m ch???n/b??? ch???n t???t c??? c??c row
     */
    toggleSelectAllRow() {
      //Auto ch???n b???n ghi ?????u
      if (this.currentSelectedRows.length == this.customData.pageSize) {
        this.clickRow(0);
      } else {
        this.currentSelectedRows = [
          ...Array(this.customData.gridData.length).keys(),
        ];
      }
    },
    /**
     * H??m x??? l?? hover chu???t v??o row
     */
    rowHover(e) {
      e.target.classList.add("tr-hover");
    },

    /**
     * H??m x??? l?? b??? hover chu???t v??o row
     */
    rowUnhover(e) {
      e.target.classList.remove("tr-hover");
    },

    /**
     * H??m x??? l?? click ch???n m???t row
     */
    clickRow(index) {
      //Reset list
      this.currentSelectedRows = [];

      //Th??m row ???????c ch???n v??o list
      this.currentSelectedRows.push(index);

      //focus v??o ?? ???????c ch???n
      // this.$refs[index][0].focus();
    },

    /**
     * H??m x??? l?? khi ctrl click v??o row
     */
    ctrlClickRow(index) {
      //Th??m row ???????c ch???n v??o list
      this.currentSelectedRows.push(index);

      //focus v??o ?? ???????c ch???n
      this.$refs[index][0].focus();
    },

    /**
     * H??m x??? l?? click chu???t v??o row
     */
    dbClickRow(item) {
      this.$emit("dbClickRow", item);
    },

    /**
     * H??m x??? l?? ch???n row khi ???n m??i t??n l??n
     */
    pressUpArrowKey(index) {
      if (index > 0) {
        this.clickRow(index - 1);
      }
    },

    /**
     * H??m x??? l?? ch???n row khi ???n m??i t??n xu???ng
     */
    pressDownArrowKey(index) {
      if (index < this.customData.gridData.length - 1) {
        this.clickRow(index + 1);
      }
    },

    /**
     * H???m ch???n nhi???u khi ch???y chu???t v?? nh???n shift
     */
    mouseEnterWhenShiftPressed(index) {
      if (!this.currentSelectedRows.includes(index)) {
        this.ctrlClickRow(index);
      }
    },

    /**
     * H??m hi???n context menu
     */
    rightClickRow(event, index) {
      this.clickRow(index);

      //Hi???n th??? context menu
      this.currentDropdown = index;

      //v??? tr?? ???n hi???n t???i
      var currentClickPosition = event.target.getBoundingClientRect();

      //L???y ra function cell
      var listCell = event.target.closest("tr").querySelectorAll("td");
      var functonCell = listCell[listCell.length - 1];

      functonCell.querySelector(".function-cel__dropdown").focus();

      //X??t v??? tr?? context menu
      setTimeout(() => {
        //Th??ng tin c???a dropdown
        let dropdownBound = functonCell
          .querySelector(".dropdown-function")
          .getBoundingClientRect();

        //Chi???u cao c???a dropdown
        this.dropdownHeight = dropdownBound.height;
        this.dropdownRightPosition =
          window.innerWidth - event.x - dropdownBound.width;

        //?????m cu???i c???a table
        var tableHeight =
          this.$refs.Grid.querySelector(".table").getBoundingClientRect()
            .bottom;

        //N???u v??? tr?? ???n hi???n t???i + height dropdown v?????t ra kh???i table => dropdown ng?????c l??n
        if (currentClickPosition.y + this.dropdownHeight > tableHeight) {
          this.dropup = true;
        }
      }, 0.2);
    },

    /**
     * H??m x??? l?? click chu???t v??o ?? select
     */
    selectBoxClick(index) {
      if (this.currentSelectedRows.includes(index)) {
        this.currentSelectedRows.splice(
          this.currentSelectedRows.indexOf(index),
          1
        );
      } else {
        //Kh??ng th?? th??m v??o list ??ang ???????c ch???n
        this.currentSelectedRows.push(index);
      }
    },

    /**
     * H??m ki???m tra xem row ???? ???????c ch???n hay ch??a
     */
    checkRowSelected(index) {
      return this.currentSelectedRows.includes(index);
    },

    /**
     * H??m chuy???n ?????i d??? li???u ????? hi???n th??? l??n b???ng
     */
    getDisplayValue(data, dataType, enumName) {
      return CommonFn.convertOriginData(data, dataType, enumName);
    },

    /**
     * H??m l???y ra s??? l?????ng item ??ang ???????c ch???n ????? hi???n l??n th??ng b??o
     */
    getNumberSelectedItem() {
      return this.currentSelectedRows.length;
    },

    /**
     * M??? dropdown function
     */
    toggleDropdownFunction(event, item) {
      if (this.currentDropdown == null) {
        this.currentDropdown = this.customData.gridData.indexOf(item);
      } else {
        this.currentDropdown = null;
      }

      //v??? tr?? ???n hi???n t???i
      var currentClickPosition = event.target.getBoundingClientRect();

      var cellFunction = event.target.closest(".function-cel__dropdown");

      //Hi???n th??? dropdown function
      setTimeout(() => {
        cellFunction.focus();

        //Chi???u cao c???a dropdown
        this.dropdownHeight = cellFunction
          .querySelector(".dropdown-function")
          .getBoundingClientRect().height;

        //?????m cu???i c???a table
        var tableHeight =
          this.$refs.Grid.querySelector(".table").getBoundingClientRect()
            .bottom;

        //N???u v??? tr?? ???n hi???n t???i + height dropdown v?????t ra kh???i table => dropdown ng?????c l??n
        if (currentClickPosition.y + this.dropdownHeight > tableHeight) {
          this.dropup = true;
        }
      }, 0.2);
    },

    /**
     * H??m ????ng dropdown function
     */
    closeDropdown() {
      this.currentDropdown = null;
      this.dropup = false;
      this.dropdownRightPosition = 0;
    },

    /**
     * G???i cha ????? th??ng b??o ng?????i d??ng click v??o default function
     */
    clickDefaultFunction(item) {
      this.$emit("clickDefaultFunction", item);
    },

    /**
     * G???i cha ????? th??ng b??o ng?????i d??ng click v??o m???t item function
     */
    clickFunctionItem(fn, item) {
      this.$emit("clickFunctionItem", fn, item);
    },

    /**
     * H??m g???i cha ????? chuy???n trang ???????c click
     */
    clickPageNum(pageNum) {
      this.$emit("clickPageNum", pageNum);
    },

    /**
     * H??m l???y id c???a nh???ng row ??ang ???????c ch???n
     */
    getListIdSelectedItem() {
      let listId = [],
        rows = this.$refs.tbody.querySelectorAll(".tr-selected");

      //L???y t???t c??? id c???a row ??ang ???????c ch???n
      for (let i = 0; i < rows.length; i++) {
        listId.push(rows[i].getAttribute("ItemId"));
      }

      return listId;
    },

    /**
     * H??m th???c hi???n reset list item ??ang ???????c ch???n v??? focus v??o d??ng ?????u ti??n
     */
    resetCurrentSelectedRows() {
      this.clickRow(0);
    },

    /**
     * H??m g???i cha thay ?????i page size
     */
    changePageSize(pageSize) {
      this.$emit("changePageSize", pageSize);
    },
  },
};
</script>

<style scoped>
@import url("../../assets/css/components/table/table.css");
.slide-fade-enter-active {
  transition: transform 0.2s ease;
}
.slide-fade-leave-active {
  transition: transform 0s cubic-bezier(1, 0.5, 0.8, 1);
}
.slide-fade-enter,
.slide-fade-leave-to {
  transform: translateY(10px);
  opacity: 0;
}
</style>