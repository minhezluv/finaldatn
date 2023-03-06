import Antd from "ant-design-vue";
import "ant-design-vue/dist/antd.css";

import "../src/assets/css/scss/app.scss";

Vue.use(Antd);
import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import Bus from "./EventBus";
import vueDebounce from "vue-debounce";
import "devextreme/dist/css/dx.light.css";
import FieldInputLabel from "./components/FieldInputLabel.vue";
import ComboBox from "./components/Combobox.vue";
import Dropdown from "./components/Dropdown.vue";
import "./assets/css/nucleo-icons.css";
import "./assets/css/nucleo-svg.css";
// import ArgonDashboard from "./argon-dashboard";
import Resource from "./js/common/Resource";
import Enumeration from "./js/common/Enumeration";
import CommonFn from "./js/common/CommonFn";
import { store } from "./store/index";

// LightBootstrap plugin
// import LightBootstrap from "./light-bootstrap-main";
// Vue.use(LightBootstrap);
Vue.config.productionTip = false;
// Vue.use(ArgonDashboard);
Vue.use(Resource);
Vue.use(Enumeration);
Vue.use(CommonFn);
import VCalendar from "v-calendar";

Vue.use(VCalendar);
//Event bus
Vue.use(Bus);

// debounce
Vue.use(vueDebounce);
Vue.use(vueDebounce, {
  lock: true,
});
Vue.use(vueDebounce, {
  listenTo: ["input", "keyup"],
});
Vue.use(vueDebounce, {
  defaultTime: "700ms",
});

//Component
Vue.component("FieldInputLabel", FieldInputLabel);
Vue.component("ComboBox", ComboBox);
Vue.component("Dropdown", Dropdown);

new Vue({
  store: store,
  router,
  render: (h) => h(App),
}).$mount("#app");
