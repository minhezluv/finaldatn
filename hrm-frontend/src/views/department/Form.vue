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
                  :customData="departmentCodeInput"
                  :model="department.departmentCode"
                  @invalidData="invalidData"
                  @updateValueInput="updateValueInput"
                  @getOriginData="getOriginData"
                  @checkUnique="checkUnique"
                  :ref="departmentCodeInput.inputFieldName"
                />
              </div>
              <FieldInputLabel
                MustValidate="true"
                :saveValidate="saveValidate"
                :customData="departmentNameInput"
                :model="department.departmentName"
                @invalidData="invalidData"
                @updateValueInput="updateValueInput"
                @getOriginData="getOriginData"
              />
            </div>
            <div class="form-col d-flex">
              <div class="form-item">
                <FieldInputLabel
                  :customData="foundingDateInput"
                  :model="department.foundingDate"
                  @updateValueInput="updateValueInput"
                  @getOriginData="getOriginData"
                />
              </div>
            </div>
          </div>
          <div class="form-row">
            <div class="form-col d-flex">
              <div class="form-item">
                <FieldInputLabel
                  :customData="addressInput"
                  :model="department.address"
                  @updateValueInput="updateValueInput"
                  @getOriginData="getOriginData"
                />
              </div>
            </div>
            <div class="form-col d-flex">
              <div class="form-item">
                <FieldInputLabel
                  :customData="faxNumberInput"
                  :model="department.faxNumber"
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
          <div class="form-row">
            <div class="form-col d-flex">
              <div class="form-item">
                <RadioButton
                  style="margin-left: 10px"
                  :customData="statusRadioButton"
                  :model="department.status"
                  @updateValueInput="updateValueInput"
                />
              </div>
            </div>
          </div>
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
import RadioButton from "../../components/RadioButton.vue";
import BasePopup from "../../components/BasePopup.vue";
import Tooltip from "../../components/Tooltip.vue";
import departmentsAPI from "../../api/components/departments/DepartmentsAPI";
// import DepartmentAPI from "../../api/components/departments/DepartmentsAPI";
import Resource from "../../js/common/Resource";
import Enumeration from "../../js/common/Enumeration";
import CommonFn from "../../js/common/CommonFn";

function initState(show) {
  return {
    //input
    departmentCodeInput: {
      inputFieldName: "departmentCode",
      labelText: "Mã",
      width: "calc(var(--column-width) * 1.5 - 10px)",
      height: "35px",
      isRequired: true,
      inputType: "text",
      isUnique: true,
      maxLength: 50,
    },
    departmentNameInput: {
      inputFieldName: "departmentName",
      labelText: "Tên",
      width: "calc(var(--column-width) * 2.5 - 10px)",
      height: "35px",
      isRequired: true,
      inputType: "text",
      dataType: Resource.DataTypeColumn.Name,
      maxLength: 100,
    },

    foundingDateInput: {
      inputFieldName: "foundingDate",
      labelText: "Ngày thành lập",
      width: "calc(var(--column-width) * 1.5 + 3px)",
      height: "37px",
      inputType: "date",
      dataType: Resource.DataTypeColumn.Date,
    },
    addressInput: {
      inputFieldName: "address",
      labelText: "Địa chỉ",
      width: "calc(var(--column-width) * 4 - 12px)",
      height: "35px",
      inputType: "text",
    },

    faxNumberInput: {
      inputFieldName: "faxNumber",
      labelText: "Số fax",
      width: "calc(var(--column-width) * 2 - 54px)",
      height: "35px",
      inputType: "text",
      maxLength: 20,
    },
    // telefaxNumberInput: {
    //   inputFieldName: "telefaxNumber",
    //   labelText: "ĐT cố định",
    //   width: "calc(var(--column-width) * 2 - 12px)",
    //   height: "35px",
    //   inputType: "text",
    //   maxLength: 20,
    // },

    //Radio button
    statusRadioButton: {
      inputFieldName: "status",
      labelText: "Tình trạng",
      height: "37px",
      items: [
        {
          text: "Hoạt động",
          value: 2,
        },
        {
          text: "Ngừng hoạt động",
          value: 1,
        },
      ],
      defaultValue: 2,
    },

    showForm: show,
    showConfirmPopup: false,
    showErrorPopup: false,
    customerBoxSelected: false,
    providerBoxSelected: false,
    department: {},
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
    RadioButton,
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
    async openForm(guid, department) {
      //Gán lại giá trị của form
      console.log("department1: ", guid);
      await Object.assign(this.$data, initState(false));
      this.showForm = true;

      // //Lấy department để bind lên combobox
      // await DepartmentAPI.getAll()
      //   .then((response) => {
      //     console.log("department: ", response.data);
      //     response.data.map((department) => {
      //       this.departmentComboBox.displayValues.push(
      //         department.departmentName
      //       );
      //       console.log(this.departmentComboBox);
      //       this.departmentComboBox.keys.push(department.departmentId);
      //     });
      //   })
      //   .catch(() => {
      //     //Nếu không lấy được phòng ban
      //     this.allInputValid = false;
      //     this.errorMessage = Resource.Message.GetDepartmentError;
      //     this.showErrorPopup = true;
      //   });
      console.log("hihihi: ", this.department);
      //Nếu là form sửa
      if (guid.length > 0) {
        //Xác định formType
        console.log("hihihi: ", this.department.guid);
        this.guid = guid;
        this.formType = Enumeration.FormMode.Edit;

        await departmentsAPI
          .getById(guid)
          .then((response) => {
            this.department = response.data.data;
            console.log("e: ", this.department);
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

        //Nếu có department thì là nhân bản
        if (department) {
          //Xác định formType
          this.formType = Enumeration.FormMode.Clone;

          this.department = JSON.parse(JSON.stringify(department));
          this.department.departmentName = "";
        }

        // await departmentsAPI
        //   .getNewdepartmentCode()
        //   .then((response) => {
        //     this.department.departmentCode = response.data;
        //   })
        //   .catch(() => {
        //     //Nếu không lấy được mã nhân viên mới
        //     this.allInputValid = false;
        //     this.errorMessage = Resource.Message.GetNewdepartmentCodeError;
        //     this.showErrorPopup = true;
        //   });
      }

      // //Focus vào ô đầu tiên
      this.$refs[this.departmentCodeInput.inputFieldName].$vnode.elm
        .querySelector(".field-input")
        .focus();

      //Lấy dữ liệu gốc để đối chiếu xem người dùng đã thay đổi dữ liệu chưa
      this.originData = JSON.parse(JSON.stringify(this.department));
    },

    /**
     * Hàm update dữ liệu khi người dùng thay đổi
     */
    updateValueInput(key, value) {
      this.$set(this.department, key, value);
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
      for (let prop in this.department) {
        if (this.department[prop] != null) {
          if (this.department[prop].toString().length == 0) {
            if (this.originData[prop]) {
              return true;
            }
          } else if (this.department[prop] !== this.originData[prop])
            return true;
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
        this.department = {};

        //Lấy lại mã nhan viên
        // await departmentsAPI
        //   .getNewdepartmentCode()
        //   .then((response) => {
        //     this.department.departmentCode = response.data;

        //     //Lấy dữ liệu gốc để đối chiếu xem người dùng đã thay đổi dữ liệu chưa
        //     this.originData = JSON.parse(JSON.stringify(this.department));
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
      await departmentsAPI
        .getdepartmentByProperty(propName, propValue)
        .then(async (response) => {
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
              if (response.data.guid != this.department.guid) {
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
      console.log(this.department);
      //Biến đánh dấu để hiện popup
      this.saveValidate = true;

      //Trước khi lưu cần validate hết
      await this.validateAll();

      if (this.allInputValid) {
        switch (this.formType) {
          //Nếu là form thêm
          case Enumeration.FormMode.Add:
          case Enumeration.FormMode.Clone:
            await departmentsAPI
              .insert(this.department)
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
                this.errorMessage = e.response.data.data.status[0]
                  ? e.response.data.data.status[0]
                  : Resource.Message.ServerError;
                this.showErrorPopup = true;
              });
            break;
          //Nếu là form sửa
          case Enumeration.FormMode.Edit:
            await departmentsAPI
              .update(this.guid, this.department)
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

      await this.checkUnique("departmentCode", this.department.departmentCode);

      if (!this.allUniue) {
        let errorMessage = CommonFn.getUniqueErrorMsg(
          "Mã phòng ban <" + this.department.departmentCode + ">"
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