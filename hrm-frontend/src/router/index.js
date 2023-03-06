import Vue from "vue";
import VueRouter from "vue-router";
// import Home from "../views/Home.vue";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "Home",
    component: () => import("../views/overview/Content.vue"),
  },
  {
    path: "/employees",
    name: "Employees",
    component: () => import("../views/employees/Content.vue"),
  },
  {
    path: "/LaborContract",
    name: "LaborContract",
    component: () => import("../views/laborContracts/Content.vue"),
  },
  {
    path: "/TimeKeeping",
    name: "TimeKeeping",
    component: () => import("../views/timeKeeping/Content.vue"),
  },
  {
    path: "/Department",
    name: "Department",
    component: () => import("../views/department/Content.vue"),
  },
  {
    path: "/Position",
    name: "Position",
    component: () => import("../views/positions/Content.vue"),
  },
  {
    path: "/StaffOverview",
    name: "StaffOverview",
    component: () => import("../views/employees/ContentOverview.vue"),
  },
  {
    path: "/LaborContractOverview",
    name: "LaborContractOverview",
    component: () => import("../views/laborContracts/ContentOverview.vue"),
  },

  {
    path: "/TimeKeepingOverview",
    name: "TimeKeepingOverview",
    component: () => import("../views/timeKeeping/ContentOverview.vue"),
  },
  // {
  //   path: "/taxs",
  //   name: "Taxs",
  //   component: () => import("../views/taxs"),
  // },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

export default router;
