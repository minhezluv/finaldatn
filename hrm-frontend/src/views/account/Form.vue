<template>
  <form v-if="showForm">
    <div class="form">
      <div class="form__title">
        <div class="form__title-left">
          <div class="form__title-text">
            <span class="text-semibold">Thông tin tài khoản</span>
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
                  :customData="accountCodeInput"
                  :model="account.accountCode"
                  @invalidData="invalidData"
                  @updateValueInput="updateValueInput"
                  @getOriginData="getOriginData"
                  @checkUnique="checkUnique"
                  :ref="accountCodeInput.inputFieldName"
                />
              </div>
              <div style="position: flex-end">
                <label class="text-semibold"
                  >Thông tin nhân viên <span style="color: red">*</span></label
                >
                <div MustValidate="true">
                  <ComboBox
                    tabindex="0"
                    :saveValidate="saveValidate"
                    style="margin-top: 4px"
                    :customData="staffComboBox"
                    :model="account.staffID"
                    @valueChanged="updateValueInput"
                    @invalidData="invalidData"
                  />
                </div>
              </div>
            </div>
            <div class="form-col d-flex">
              <RadioButton
                style="margin-left: 10px"
                :customData="typeaccountRadioButton"
                :model="account.typeaccountID"
                @updateValueInput="updateValueInput"
              />
            </div>
          </div>
          <div class="form-row">
            <div class="form-col">
              <label class="text-semibold"
                >Đơn vị <span style="color: red">*</span></label
              >
              <div MustValidate="true">
                <ComboBox
                  tabindex="0"
                  :saveValidate="saveValidate"
                  style="margin-top: 4px"
                  :customData="departmentComboBox"
                  :model="account.departmentID"
                  @valueChanged="updateValueInput"
                  @invalidData="invalidData"
                />
              </div>
            </div>
            <div class="form-col d-flex">
              <div class="form-item">
                <FieldInputLabel
                  :customData="HealthICodeInput"
                  :model="account.healthICode"
                  @updateValueInput="updateValueInput"
                  @getOriginData="getOriginData"
                />
              </div>
            </div>
          </div>
          <div class="form-row">
            <div class="form-col">
              <label class="text-semibold"
                >Chức danh <span style="color: red">*</span></label
              >
              <div MustValidate="true">
                <ComboBox
                  tabindex="0"
                  :saveValidate="saveValidate"
                  style="margin-top: 4px"
                  :customData="positionComboBox"
                  :model="account.positionID"
                  @valueChanged="updateValueInput"
                  @invalidData="invalidData"
                />
              </div>
            </div>
            <FieldInputLabel
              :customData="SocialICodeInput"
              :model="account.socialICode"
              @updateValueInput="updateValueInput"
              @getOriginData="getOriginData"
            />
          </div>
        </div>
        <div class="form-group">
          <div class="form-row">
            <FieldInputLabel
              :customData="BasicSalaryInput"
              :model="account.basicSalary"
              @updateValueInput="updateValueInput"
              @getOriginData="getOriginData"
            />
          </div>
          <div class="form-row">
            <div class="form-col d-flex"></div>
            <div class="form-col"></div>
          </div>
          <div class="form-row">
            <div class="form-col d-flex">
              <div class="form-item"></div>
              <FieldInputLabel
                :customData="EndDateInput"
                :model="account.endDate"
                @updateValueInput="updateValueInput"
                @getOriginData="getOriginData"
              />
            </div>
            <div class="form-col">
              <FieldInputLabel
                :customData="StartDateInput"
                :model="account.startDate"
                @updateValueInput="updateValueInput"
                @getOriginData="getOriginData"
              />
            </div>

            <div class="form-col">
              <RadioButton
                style="margin-left: 10px"
                :customData="StatusButton"
                :model="account.status"
                @updateValueInput="updateValueInput"
              />
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
import ComboBox from "../../components/Combobox.vue";
import RadioButton from "../../components/RadioButton.vue";
import BasePopup from "../../components/BasePopup.vue";
import Tooltip from "../../components/Tooltip.vue";
import accountsAPI from "../../api/components/accounts/accountsAPI";
import DepartmentAPI from "../../api/components/departments/DepartmentsAPI";
import PositionAPI from "../../api/components/positions/PositionsAPI";
import Resource from "../../js/common/Resource";
import Enumeration from "../../js/common/Enumeration";
import CommonFn from "../../js/common/CommonFn";
import EmployeesAPI from "../../api/components/employees/EmployeesAPI";
//import EmployeeAPI from "../../api/components/employees/EmployeesAPI.js"
function initState(show) {
  return {
    //input

    accountCodeInput: {
      inputFieldName: "accountCode",
      labelText: "Mã hợp đồng",
      width: "calc(var(--column-width) * 1.5 - 10px)",
      height: "35px",
      isRequired: true,
      inputType: "text",
      isUnique: true,
      maxLength: 50,
    },
    StaffCodeInput: {
      inputFieldName: "staffCode",
      labelText: "Mã nhân viên",
      width: "calc(var(--column-width) * 2.5 - 10px)",
      height: "35px",
      isRequired: true,
      inputType: "text",
      dataType: Resource.DataTypeColumn.Name,
      maxLength: 100,
    },

    positionNameInput: {
      inputFieldName: "positionName",
      labelText: "Chức danh",
      width: "calc(var(--column-width) * 4 - 2px)",
      height: "35px",
      inputType: "text",
    },

    HealthICodeInput: {
      inputFieldName: "healthICode",
      labelText: "Mã bảo hiểm y tế",
      width: "calc(var(--column-width) * 2.5 - 10px)",
      height: "35px",
      inputType: "text",
      maxLength: 20,
    },
    SocialICodeInput: {
      inputFieldName: "socialICode",
      labelText: "Mã bảo hiểm xã hội",
      width: "calc(var(--column-width) * 4 - 1px)",
      height: "35px",
      inputType: "text",
    },
    BasicSalaryInput: {
      inputFieldName: "basicSalary",
      labelText: "Lương cơ bản",
      width: "calc(var(--column-width) * 4)",
      height: "35px",
      inputType: "text",
    },
    StartDateInput: {
      inputFieldName: "startDate",
      labelText: "Ngày bắt đầu hợp đồng",
      width: "calc(var(--column-width) * 2 - 12px)",
      height: "35px",
      inputType: "date",
      dataType: Resource.DataTypeColumn.Date,
    },
    EndDateInput: {
      inputFieldName: "endDate",
      labelText: "Ngày kết thúc hợp đồng",
      width: "calc(var(--column-width) * 2 - 2px)",
      height: "35px",
      inputType: "date",
      dataType: Resource.DataTypeColumn.Date,
    },

    //ComboBox
    departmentComboBox: {
      inputFieldName: "departmentID",
      labelText: "Đơn vị",
      defaultValue: "",
      displayValues: [],
      keys: [],
      width: "calc(var(--column-width) * 4 )",
      height: "37px",
    },
    //ComboBox
    positionComboBox: {
      inputFieldName: "positionID",
      labelText: "Chức danh",
      defaultValue: "",
      displayValues: [],
      keys: [],
      width: "calc(var(--column-width) * 4 )",
      height: "37px",
    },
    //ComboBox
    staffComboBox: {
      inputFieldName: "staffID",
      labelText: "Mã nhân viên",
      defaultValue: "",
      displayValues: [],
      keys: [],
      width: "calc(var(--column-width) * 2 + 50px)",
      height: "37px",
    },

    //Radio button
    typeaccountRadioButton: {
      inputFieldName: "typeaccountID",
      labelText: "Loại hợp đồng ",
      height: "37px",
      items: [
        {
          text: "Thử việc",
          value: 3,
        },
        {
          text: "Thực tập",
          value: 1,
        },
        {
          text: "Chính thức",
          value: 2,
        },
      ],
      defaultValue: 2,
    },

    //Radio button
    StatusButton: {
      inputFieldName: "status",
      labelText: "Tình trạng hợp đồng ",
      height: "37px",
      items: [
        {
          text: "Đang thực hiện",
          value: 2,
        },
        {
          text: "Kết thúc",
          value: 1,
        },
      ],
      defaultValue: 0,
    },
    showForm: show,
    showConfirmPopup: false,
    showErrorPopup: false,
    customerBoxSelected: false,
    providerBoxSelected: false,
    account: {},
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
    ComboBox,
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
    async openForm(guid, account) {
      //Gán lại giá trị của form
      await Object.assign(this.$data, initState(false));
      this.showForm = true;
      await EmployeesAPI.getAll()
        .then((response) => {
          response.data.map((staff) => {
            var tmp = `${staff.staffName}      ${staff.staffCode}`;
            this.staffComboBox.displayValues.push(tmp);

            this.staffComboBox.keys.push(staff.guid);
          });
          console.log(this.staffComboBox);
        })
        .catch(() => {
          //Nếu không lấy được phòng ban
          this.allInputValid = false;
          this.errorMessage = "Lỗi lấy mã nhân viên";
          this.showErrorPopup = true;
        });

      //Lấy department để bind lên combobox
      await DepartmentAPI.getAll()
        .then((response) => {
          response.data.map((department) => {
            this.departmentComboBox.displayValues.push(
              department.departmentName
            );

            this.departmentComboBox.keys.push(department.guid);
          });
          console.log(this.departmentComboBox);
        })
        .catch(() => {
          //Nếu không lấy được phòng ban
          this.allInputValid = false;
          this.errorMessage = Resource.Message.GetDepartmentError;
          this.showErrorPopup = true;
        });

      /**
       * lấy chức danh
       */
      await PositionAPI.getAll()
        .then((response) => {
          response.data.map((position) => {
            this.positionComboBox.displayValues.push(position.positionName);

            this.positionComboBox.keys.push(position.guid);
          });
          console.log(this.positionComboBox);
        })
        .catch(() => {
          //Nếu không lấy được phòng ban
          this.allInputValid = false;
          this.errorMessage = Resource.Message.GetPositionError;
          this.showErrorPopup = true;
        });
      //Nếu là form sửa
      if (guid.length > 0) {
        //Xác định formType
        this.guid = guid;
        this.formType = Enumeration.FormMode.Edit;
        console.log(this.guid);
        await accountsAPI
          .getById(guid)
          .then((response) => {
            this.account = response.data.data;
            console.log("hihi: ", this.account);
          })
          .catch((e) => {
            console.log(e);
            //Nếu không lấy được dữ liệu từ db
            this.allInputValid = false;
            this.errorMessage = Resource.Message.ServerError;
            this.showErrorPopup = true;
          });
      } else {
        //Xác định formType
        this.formType = Enumeration.FormMode.Add;

        //Nếu có account thì là nhân bản
        if (account) {
          //Xác định formType
          this.formType = Enumeration.FormMode.Clone;

          this.account = JSON.parse(JSON.stringify(account));
          this.account.accountCode = "";
        }

        await accountsAPI
          .getNewLCCode()
          .then((response) => {
            this.account.accountCode = response.data;
            console.log("get new");
          })
          .catch(() => {
            //Nếu không lấy được mã nhân viên mới
            this.allInputValid = false;
            this.errorMessage = Resource.Message.GetNewLCCodeError;
            this.showErrorPopup = true;
          });
      }

      // //Focus vào ô đầu tiên
      this.$refs[this.accountCodeInput.inputFieldName].$vnode.elm
        .querySelector(".field-input")
        .focus();

      //Lấy dữ liệu gốc để đối chiếu xem người dùng đã thay đổi dữ liệu chưa
      this.originData = JSON.parse(JSON.stringify(this.account));
    },

    /**
     * Hàm update dữ liệu khi người dùng thay đổi
     */
    updateValueInput(key, value) {
      console.log(key, value);
      this.$set(this.account, key, value);
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
      for (let prop in this.account) {
        if (this.account[prop] != null) {
          if (this.account[prop].toString().length == 0) {
            if (this.originData[prop]) {
              return true;
            }
          } else if (this.account[prop] !== this.originData[prop]) return true;
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
        this.account = {};

        //Lấy lại mã nhan viên
        await accountsAPI
          .getNewLCCode()
          .then((response) => {
            this.account.accountCode = response.data;

            //Lấy dữ liệu gốc để đối chiếu xem người dùng đã thay đổi dữ liệu chưa
            this.originData = JSON.parse(JSON.stringify(this.account));
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
      await accountsAPI
        .getaccountByProperty(propName, propValue)
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
              console.log("edit: ", response);
              if (response.data.guid != this.account.guid) {
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
      //Biến đánh dấu để hiện popup
      this.saveValidate = true;

      //Trước khi lưu cần validate hết
      await this.validateAll();

      if (this.allInputValid) {
        console.log(this.account);
        console.log(this.departmentID);
        // var StaffID = "4016b0db-b3e7-11ed-a841-b4b686f217b6";
        // this.account.StaffID = StaffID;
        switch (this.formType) {
          //Nếu là form thêm

          case Enumeration.FormMode.Add:
          case Enumeration.FormMode.Clone:
            await accountsAPI
              .insertData(this.account)
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
                console.log(e);
                this.errorMessage = "lỗi";

                this.showErrorPopup = true;
              });
            break;
          //Nếu là form sửa
          case Enumeration.FormMode.Edit:
            await accountsAPI
              .update(this.guid, this.account)
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

      await this.checkUnique("accountCode", this.account.accountCode);

      if (!this.allUniue) {
        let errorMessage = CommonFn.getUniqueErrorMsg(
          "Mã hợp đồng <" + this.account.accountCode + ">"
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