import Vue from "vue";
import VueRouter from "vue-router";
import Login from "../views/Login.vue";
import MainApp from "../MainApp.vue";
import User from "../UserApp.vue";
Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "Login",
    component: Login,
  },
  {
    path: "/User",
    name: "User",
    component: User,
    children: [
      {
        path: "/UserOverview",
        name: "UserOverview",
        component: () => import("../views/user/ContentOverview.vue"),
      },
    ],
  },
  {
    path: "/home",
    name: "Home",
    component: MainApp,
    beforeEnter: (to, from, next) => {
      const token = sessionStorage.getItem("token");
      console.log("token: ", token);
      if (token) {
        next();
      } else {
        // If the user is not authenticated, redirect to the login page
        next({ name: "Login" });
      }
    },

    children: [
      {
        path: "/Overview",
        name: "Overview",
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
    ],
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

export default router;
