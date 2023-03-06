<template>
  <form v-if="showForm">
    <div class="form">
      <div class="form__title">
        <div class="form__title-left">
          <div class="form__title-text">
            <span class="text-semibold">Thông tin phòng ban</span>
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
      <div class="form__content" ref="FormData">
        <div class="form-group">
          <div class="form-row">
            <div class="form-col d-flex">
              <div class="form-item">
                <FieldInputLabel
                  MustValidate="true"
                  :saveValidate="saveValidate"
                  :customData="positionCodeInput"
                  :model="position.positionCode"
                  @invalidData="invalidData"
                  @updateValueInput="updateValueInput"
                  @getOriginData="getOriginData"
                  @checkUnique="checkUnique"
                  :ref="positionCodeInput.inputFieldName"
                />
              </div>
              <FieldInputLabel
                MustValidate="true"
                :saveValidate="saveValidate"
                :customData="positionNameInput"
                :model="position.positionName"
                @invalidData="invalidData"
                @updateValueInput="updateValueInput"
                @getOriginData="getOriginData"
              />
            </div>
          </div>
          <div class="form-row">
            <div class="form-col d-flex">
              <div class="form-item">
                <FieldInputLabel
                  :customData="noteInput"
                  :model="position.note"
                  @updateValueInput="updateValueInput"
                  @getOriginData="getOriginData"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="form-group">
        <div class="form-row">
          <div class="form-row"></div>
        </div>
      </div>
      <div class="form__footer">
        <div
          class="btn btn-default text-semibold"
          tabindex="0"
          @click="showForm = false"
          @keyup.enter="showForm = false"
        >
          Hủy
        </div>
        <div class="btn-group d-flex">
          <div
            class="btn btn-default text-semibold"
            tabindex="0"
            @click="saveAndOut"
            @keyup.enter="saveAndOut"
          >
            Cất
          </div>
          <div
            class="btn btn-primary text-semibold"
            tabindex="0"
            @click="saveAndAdd"
            @keyup.enter="saveAndAdd"
          >
            Cất và thêm
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
import FieldInputLabel from "../../components/FieldInputLabel.vue";
// import ComboBox from "../../components/Combobox.vue";
//import RadioButton from "../../components/RadioButton.vue";
import BasePopup from "../../components/BasePopup.vue";
import Tooltip from "../../components/Tooltip.vue";
import positionsAPI from "../../api/components/positions/PositionsAPI";
// import positionAPI from "../../api/components/positions/positionsAPI";
import Resource from "../../js/common/Resource";
import Enumeration from "../../js/common/Enumeration";
import CommonFn from "../../js/common/CommonFn";

function initState(show) {
  return {
    //input
    positionCodeInput: {
      inputFieldName: "positionCode",
      labelText: "Mã",
      width: "calc(var(--column-width) * 1.5 - 10px)",
      height: "35px",
      isRequired: true,
      inputType: "text",
      isUnique: true,
      maxLength: 50,
    },
    positionNameInput: {
      inputFieldName: "positionName",
      labelText: "Tên",
      width: "calc(var(--column-width) * 2.5 - 10px)",
      height: "35px",
      isRequired: true,
      inputType: "text",
      dataType: Resource.DataTypeColumn.Name,
      maxLength: 100,
    },

    noteInput: {
      inputFieldName: "note",
      labelText: "Địa chỉ",
      width: "calc(var(--column-width) * 4 - 12px)",
      height: "35px",
      inputType: "text",
    },

    showForm: show,
    showConfirmPopup: false,
    showErrorPopup: false,
    customerBoxSelected: false,
    providerBoxSelected: false,
    position: {},
    originData: {},
    guid: null,
    saveValidate: false,
    allInputValid: true,
    allUniue: true,
    formType: null,
    errorMessage: "",
    uniqueError: false,
  };
}

export default {
  components: {
    FieldInputLabel,
    // ComboBox,
    // RadioButton,
    BasePopup,
    Tooltip,
  },
  data() {
    return initState(false);
  },
  methods: {
    /**
     * Hàm mở form
     */
    async openForm(guid, position) {
      //Gán lại giá trị của form
      console.log("position1: ", guid);
      await Object.assign(this.$data, initState(false));
      this.showForm = true;
      console.log("hihihi: ", this.position);
      //Nếu là form sửa
      if (guid.length > 0) {
        //Xác định formType
        console.log("hihihi: ", this.position.guid);
        this.guid = guid;
        this.formType = Enumeration.FormMode.Edit;

        await positionsAPI
          .getById(guid)
          .then((response) => {
            this.position = response.data.data;
            console.log("e: ", this.position);
          })
          .catch(() => {
            //Nếu không lấy được dữ liệu từ db
            this.allInputValid = false;
            this.errorMessage = Resource.Message.ServerError;
            this.showErrorPopup = true;
          });
      } else {
        //Xác định formType
        this.formType = Enumeration.FormMode.Add;

        //Nếu có position thì là nhân bản
        if (position) {
          //Xác định formType
          this.formType = Enumeration.FormMode.Clone;

          this.position = JSON.parse(JSON.stringify(position));
          this.position.positionName = "";
        }

        // await positionsAPI
        //   .getNewpositionCode()
        //   .then((response) => {
        //     this.position.positionCode = response.data;
        //   })
        //   .catch(() => {
        //     //Nếu không lấy được mã nhân viên mới
        //     this.allInputValid = false;
        //     this.errorMessage = Resource.Message.GetNewpositionCodeError;
        //     this.showErrorPopup = true;
        //   });
      }

      // //Focus vào ô đầu tiên
      this.$refs[this.positionCodeInput.inputFieldName].$vnode.elm
        .querySelector(".field-input")
        .focus();

      //Lấy dữ liệu gốc để đối chiếu xem người dùng đã thay đổi dữ liệu chưa
      this.originData = JSON.parse(JSON.stringify(this.position));
    },

    /**
     * Hàm update dữ liệu khi người dùng thay đổi
     */
    updateValueInput(key, value) {
      this.$set(this.position, key, value);
      console.log(this.position);
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
      for (let prop in this.position) {
        if (this.position[prop] != null) {
          if (this.position[prop].toString().length == 0) {
            if (this.originData[prop]) {
              return true;
            }
          } else if (this.position[prop] !== this.originData[prop]) return true;
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
        this.position = {};

        //Lấy lại mã nhan viên
        // await positionsAPI
        //   .getNewpositionCode()
        //   .then((response) => {
        //     this.position.positionCode = response.data;

        //     //Lấy dữ liệu gốc để đối chiếu xem người dùng đã thay đổi dữ liệu chưa
        //     this.originData = JSON.parse(JSON.stringify(this.position));
        //   })
        //   .catch(() => {
        //     //Nếu không lưu được thì thông báo lỗi
        //     this.allInputValid = false;
        //     this.errorMessage = Resource.Message.ServerError;
        //     this.showErrorPopup = true;
        //   });
      }
    },

    /**
     * Hàm gọi api check unique
     */
    async checkUnique(propName, propValue) {
      await positionsAPI
        .getpositionByProperty(propName, propValue)
        .then(async (response) => {
          switch (this.formType) {
            case Enumeration.FormMode.Add:
              if (response.position != 204) {
                this.allUniue = true;
              }
              break;

            case Enumeration.FormMode.Clone:
              if (response.position != 204) {
                this.allUniue = false;
              }
              break;

            case Enumeration.FormMode.Edit:
              if (response.data.guid != this.position.guid) {
                this.allUniue = true;
              }
              break;
          }
        });
    },

    /**
     * Hàm Lưu dữ liệu
     */
    async saveData() {
      console.log("position: ", this.position);
      //Biến đánh dấu để hiện popup
      this.saveValidate = true;

      //Trước khi lưu cần validate hết
      await this.validateAll();

      if (this.allInputValid) {
        switch (this.formType) {
          //Nếu là form thêm
          case Enumeration.FormMode.Add:
          case Enumeration.FormMode.Clone:
            await positionsAPI
              .insert(this.position)
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
                console.log(e);
                this.allInputValid = false;
                // this.errorMessage = e.response.data.data.position[0]
                //   ? e.response.data.data.status[0]
                //   : Resource.Message.ServerError;
                this.errorMessage = "Lỗi";
                this.showErrorPopup = true;
              });
            break;
          //Nếu là form sửa
          case Enumeration.FormMode.Edit:
            await positionsAPI
              .update(this.guid, this.position)
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

      await this.checkUnique("positionCode", this.position.positionCode);

      if (!this.allUniue) {
        let errorMessage = CommonFn.getUniqueErrorMsg(
          "Mã phòng ban <" + this.position.positionCode + ">"
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
</style>