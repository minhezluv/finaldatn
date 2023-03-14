<template>
  <div>
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: "App",
};
</script>

<style>
@import url("./assets/css/common/reset.css");
@import url("./assets/css/common/global.css");

.nav-small {
  --nav-width: 52px;
}

.wrapper {
  display: flex;
  width: 100%;
  background-color: #f4f5f8;
}

.loader {
  position: fixed;
  top: 0;
  bottom: 0;
  right: 0;
  left: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 999;
}
</style>
