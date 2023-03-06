<template>
  <DxSelectBox
    ref="DxDateBox"
    :height="customData.height"
    :width="customData.width"
    :dataSource="customData.displayValues"
    :onValueChanged="onValueChanged"
    class="dropdown"
    v-model="currentValue"
  />
</template>


<script>
import DxSelectBox from "devextreme-vue/select-box";

export default {
  components: {
    DxSelectBox,
  },
  props: {
    customData: {
      type: Object,
      required: true,
    },
    model: {},
  },
  created() {
    // console.log("customdata: ", this.customData);
    this.currentValue =
      this.customData.displayValues[this.customData.keys.indexOf(this.model)];
  },
  data() {
    return {
      currentValue: "",
    };
  },
  methods: {
    /**
     * Hàm xử lý khi thay chọn giá trị
     *
     */
    onValueChanged() {
      //Lấy index của giá trị hiện tại
      let index = this.customData.displayValues.indexOf(this.currentValue);
      console.log("id: ", this.customData.keys[index]);
      //truyền key ra ngoài để thay đổi
      this.$emit("valueChanged", this.customData.keys[index]);
    },
  },
};
</script>

<style>
@import url("../assets/css/components/dropdown.css");
</style>