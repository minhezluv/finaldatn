<template>
  <div class="content__header">
    <div class="content__name text-bold">
      <span>Hợp đồng lao động</span>
    </div>
    <div class="btn btn-primary btn__add" @click="openForm">
      <span class="btn__name">Thêm mới hợp đồng</span>
    </div>
  </div>
</template>

<script>
export default {
  methods: {
    /**
     * Hàm gọi cha mở form thêm sửa
     */
    openForm() {
      this.$emit("openForm", null);
    },
  },
};
</script>

<style scoped>
@import url("../../assets/css/common/button.css");
@import url("../../assets/css/views/employees/headerContent.css");
</style>