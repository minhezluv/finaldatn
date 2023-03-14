<template>
  <header class="header-page" id="header-page">
    <div class="header__left">
      <div class="page-icon" @click="makeNavSmall" v-show="bigNav">
        <div class="header__icon-small-nav"></div>
      </div>
      <div class="header__branch">
        <span class="header__branch-name text-uppercase">Công ty A</span>
        <div class="page-icon">
          <div class="header__icon-branch-work"></div>
        </div>
      </div>
    </div>
    <div class="header__right">
      <div class="page-icon">
        <div class="header__icon-notify"></div>
      </div>
      <div class="header__user">
        <div class="header__icon-user"></div>
        <div class="user__name text-semibold">{{ staffName }}</div>
        <div class="page-icon" @click="toggleDropdown">
          <div class="header__icon-user-menu"></div>
          <div class="dropdown__menu" v-show="showDropdown">
            <button class="dropdown__item">Logout</button>
          </div>
        </div>
      </div>
    </div>
  </header>
</template>

<script>
import EmployeesAPI from "../../api/components/employees/EmployeesAPI";
export default {
  data() {
    return {
      bigNav: true,
      staffName: null,
      staffCode: sessionStorage.getItem("staffCode"),
      showDropdown: false,
      //  isLoadName:false,
    };
  },
  created() {
    this.getStaffName();
  },
  methods: {
    /**
     * Hàm gọi cha để thu nhỏ navbar
     *
     */
    makeNavSmall() {
      this.$emit("makeNavSmall");
      this.bigNav = false;
    },

    /**
     * Hàm hiện icon khi navbar to
     *
     */
    makeNavBig() {
      this.bigNav = true;
    },
    toggleDropdown() {
      this.showDropdown = !this.showDropdown;
      console.log("switch");
      this.$router.push("/");
    },

    /**
     * Lấy tên nhân viên
     */

    async getStaffName() {
      await EmployeesAPI.filter(
        1,
        1,
        sessionStorage.getItem("staffCode"),
        null,
        null,
        2
      ).then((response) => {
        console.log("header: ", response.data.data);
        this.staffName = response.data.data[0].staffName;
      });
    },
  },
};
</script>

<style scoped>
@import url("../../assets/css/layout/header.css");
.dropdown {
  position: relative;
  display: inline-block;
}

.dropdown__toggle {
  background-color: transparent;
  border: none;
  cursor: pointer;
  padding: 0;
  margin: 0;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.dropdown__menu {
  position: absolute;
  top: 32px;
  right: 0;
  background-color: #fff;
  border: 1px solid #ccc;
  border-radius: 4px;
  padding: 8px;
  display: none;
  z-index: 999;
}

.dropdown__item {
  background-color: transparent;
  border: none;
  cursor: pointer;
  padding: 8px;
  margin: 0;
  width: 100%;
  text-align: left;
  font-size: 14px;
}

.dropdown__item:hover {
  background-color: #f1f1f1;
}
</style>