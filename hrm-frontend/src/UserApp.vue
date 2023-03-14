<template>
  <div class="wrapper" id="app2" :class="{ 'nav-small': smallNav }">
    <Navbar ref="Navbar" :customData="navbarData" @makeNavBig="makeNavBig" />
    <Main ref="Main" @makeNavSmall="makeNavSmall" />
    <div class="loader" v-show="showLoader">
      <Loader />
    </div>
    <ToastMessage ref="ToastMessage" :customData="customToast" />
  </div>
</template>

<script>
import Navbar from "./layout/TheNavbar.vue";
import Main from "./layout/TheMain.vue";
import Loader from "./components/Loader.vue";
import ToastMessage from "./components/ToastMessage.vue";

export default {
  name: "App",
  components: {
    Navbar,
    Main,
    Loader,
    ToastMessage,
  },
  data() {
    return {
      smallNav: false,
      overlayShow: false,
      navbarData: [
        {
          iconClass: "nav__icon-dashboard",
          itemName: "Tổng quan",
          routerLink: "/UserOverview",
        },
      ],
    };
  },
  computed: {
    customToast() {
      return this.$store.getters.toastData;
    },
    showLoader() {
      return this.$store.getters.showLoader;
    },
  },
  watch: {
    customToast: {
      deep: true,
      handler() {
        this.$refs.ToastMessage.showToast();
      },
    },
  },
  methods: {
    /**
     * Hàm thu nhỏ navbar
     *
     */
    makeNavSmall() {
      this.$refs.Navbar.makeNavSmall();
      this.smallNav = true;
    },

    /**
     * Hàm thông báo cho component con mở rộng navbar
     *
     */
    makeNavBig() {
      this.$refs.Main.makeNavBig();
      this.smallNav = false;
    },
  },
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
