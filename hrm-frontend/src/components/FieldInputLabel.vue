<template>
  <div
    class="input-label tooltip-error"
    :style="{ '--scale': scaleTooltip, '--tooltip-message': tooltipMsg }"
  >
    <label :for="customData.inputFieldName" class="text-semibold"
      >{{ customData.labelText }}
      <span v-if="customData.isRequired">
        <span style="color: red">*</span>
      </span>
    </label>
    <!-- <Tooltip :customData="errorMessage"> -->
    <template>
      <DxDateBox
        v-if="customData.dataType == 'Date'"
        v-model="cloneModel"
        displayFormat="dd/MM/yyyy"
        :height="customData.height"
        placeholder="dd/mm/yyyy"
        :useMaskBehavior="true"
        :width="customData.width"
        :showClearButton="true"
        class="dxDateBox"
      />
      <input
        v-else-if="customData.dataType == 'Number'"
        autocomplete="off"
        :type="customData.inputType"
        class="field-input text-right"
        :class="{ invalid: invalidInput }"
        :placeholder="customData.placeholder"
        :id="customData.inputFieldName"
        @focus="focus"
        @blur="blur"
        ref="input"
        v-model="cloneModel"
        v-money="money"
      />
      <input
        v-else
        autocomplete="off"
        :type="customData.inputType"
        :style="[{ width: customData.width }, { height: customData.height }]"
        class="field-input"
        :class="{ invalid: invalidInput }"
        :placeholder="customData.placeholder"
        :id="customData.inputFieldName"
        @focus="focus"
        @blur="blur"
        @keypress="validateMaxLength($event, cloneModel)"
        ref="input"
        v-model="cloneModel"
      />
    </template>
    <!-- </Tooltip> -->
  </div>
</template>

<script>
import DxDateBox from "devextreme-vue/date-box";
import Resource from "../js/common/Resource";
import moment from "moment";
import CommonFn from "../js/common/CommonFn";
import Enumeration from "../js/common/Enumeration";
// import Tooltip from "../components/Tooltip.vue";

export default {
  components: {
    DxDateBox,
    // Tooltip,
  },
  props: {
    customData: {
      type: Object,
      require: true,
    },
    model: {
      default: null,
    },
    saveValidate: {
      default: false,
    },
  },
  data() {
    return {
      errorMessage: "",
      tooltipMsg: "",
      invalidInput: false,
      valueUnique: true,
      cloneModel: "",
      focused: false,
      typed: false,
      originData: null,
      showTooltip: false,
      scaleTooltip: 0,
      money: {
        decimal: "",
        thousands: ".",
        prefix: "",
        suffix: " (VND) ",
        precision: 0,
        masked: false,
      },
    };
  },
  created() {
    this.cloneModel = this.model;
  },
  watch: {
    cloneModel: function (val) {
      //N???u ng?????i d??ng ch??a nh???n g?? th?? truy???n ra d??? li???u g???c
      if (!this.focused) {
        this.originData = val;
        this.$emit("getOriginData", this.customData.inputFieldName, val);
      }

      //N???u ng?????i d??ng ???? t???ng g?? th?? ????nh d???u l?? ???? g?? ????? validate
      if (val != this.originData) {
        this.typed = true;
      }

      //N???u l?? required th?? validate khi ??ang nh???p
      if (this.customData.isRequired && this.typed && val != null) {
        this.invalidInput = !this.validateRequired(val);
      }

      //N???u ng?????i d??ng ??ang nh???p th?? b??? c???nh b??o ????? v?? format d??? li???u
      if (val) {
        this.invalidInput = false;
        this.scaleTooltip = 0;
        this.errorMessage = "";

        //Format d??? li???u d???a theo ki???u d??? li???u t????ng ???ng
        val = this.formatData(val);
      }

      //N???u x??a h???t trong khi tr?????ng ???? kh??ng b???t bu???c nh???p th?? x??a h???t l???i
      if (!val && !this.customData.isRequired) {
        this.invalidInput = false;
        this.scaleTooltip = 0;
        this.errorMessage = "";
      }

      //Thay ?????i model g???c ra ngo??i
      this.$emit("updateValueInput", this.customData.inputFieldName, val);
    },
    model: {
      deep: true,
      immediate: true,
      handler(val) {
        this.cloneModel = JSON.parse(JSON.stringify(val));
      },
    },
    saveValidate(val) {
      if (val) {
        this.validate();
      }
    },
  },
  methods: {
    /**
     * H??m x??? l?? s??? ki???n focus
     *
     */
    focus() {
      this.focused = true;
      this.$refs.input.select();
    },
    /**
     * H??m x??? l?? s??? ki???n khi kh??ng focus v??o input n???a (check validate)
     *
     */
    blur() {
      if (this.typed) {
        this.validate();
      }
    },

    /**
     * H??m format d??? li???u ????? hi???n th??? theo ki???u d??? li???u t????ng ???ng
     *
     */
    formatData(val) {
      switch (this.customData.dataType) {
        case Resource.DataTypeColumn.Number:
          return val.toString().replace(/[^\d]/gi, "");
        case Resource.DataTypeColumn.Date:
          return (this.cloneModel = moment(this.cloneModel).format(
            "yyyy-MM-DD"
          ));
        case Resource.DataTypeColumn.Name:
          return CommonFn.formatName(val);
        default:
          return val;
      }
    },

    /**
     * H??m validate d??? li???u
     *
     */
    validate() {
      let value = this.cloneModel,
        isValid = true;

      if (this.customData.isRequired) {
        isValid = this.validateRequired(value);
      }

      if (isValid && value) {
        isValid = this.validateDataType(value);
      }

      if (!isValid) {
        this.invalidInput = true;
      }
    },

    /**
     * H??m validate required
     *
     */
    validateRequired(value) {
      if (!value || value.length <= 0) {
        //Th??ng ??i???p l???i
        this.errorMessage = CommonFn.getRequiredErrorMsg(
          this.customData.labelText
        );
        this.displayTooltip();

        //D??? li???u ch??a h???p l???
        this.$emit(
          "invalidData",
          this.errorMessage,
          Enumeration.ErrorType.Require
        );

        return false;
      }

      return true;
    },

    /**
     * H??m g???i cha ????? validate unique
     *
     */
    async validateUnique(value) {
      //g???i cha truy v???n api xem c?? d??? li???u hay ch??a
      await this.$emit("checkUnique", this.customData.inputFieldName, value);

      return true;
    },

    /**
     * H??m ????nh d???u gi?? tr??? nh???p kh??ng unique
     *
     */
    async valueNotUnique() {
      //????nh d???u kh??ng unque ????? hi???n l???i
      this.invalidInput = true;

      //Th??ng ??i???p l???i
      let valueMsg = this.customData.labelText + "<" + this.cloneModel + ">";
      this.errorMessage = CommonFn.getUniqueErrorMsg(valueMsg);
      this.displayTooltip();

      //D??? li???u ch??a h???p l???
      await this.$emit(
        "invalidData",
        this.errorMessage,
        Enumeration.ErrorType.Unique
      );
    },

    /**
     * H??m validate ????? d??i t???i ??a
     *
     */
    validateMaxLength(event, val) {
      //N???u ???? nh???p ?????n ????? d??i t???i ??a
      if (
        this.customData.maxLength &&
        val &&
        val.length == this.customData.maxLength
      ) {
        //Ng??n ng?????i d??ng ti???p t???c nh???p
        event.preventDefault();

        //Hi???n l???i
        this.invalidInput = true;

        //T???o th??ng ??i???p l???i
        this.errorMessage = CommonFn.getMaxLengthErrorMsg(
          this.customData.labelText,
          this.customData.maxLength
        );
        this.displayTooltip();

        //Th??ng b??o l???i ra ngo??i
        this.$emit(
          "invalidData",
          this.errorMessage,
          Enumeration.ErrorType.MaxLength
        );
      }
    },

    /**
     * H??m validate theo ki???u d??? li???u
     *
     */
    validateDataType(value) {
      let res = true;

      switch (this.customData.dataType) {
        case Resource.DataTypeColumn.Email:
          res = this.validateEmail(value);
          break;
      }

      if (!res) {
        this.errorMessage = CommonFn.getDataTypeErrorMsg(
          this.customData.labelText
        );
        this.displayTooltip();

        //D??? li???u ch??a h???p l???
        this.$emit(
          "invalidData",
          this.errorMessage,
          Enumeration.ErrorType.DataType
        );
      }

      return res;
    },

    /**
     * Validate Email
     *
     */
    validateEmail(value) {
      let regex =
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

      return regex.test(value);
    },

    /**
     * H??m hi???n th??? tooltip
     *
     */
    displayTooltip() {
      this.tooltipMsg = "'" + this.errorMessage + "'";
      this.scaleTooltip = 1;
    },
  },
};
</script>

<style scoped>
@import url("../assets/css/components/filed-input-label.css");
@import url("../assets/css/common/tooltip-error.css");

.tooltip-error::before,
.tooltip-error::after {
  top: 30px;
}
</style>