
<template>
  <div>
    <!-- Charts -->
    <a-row :gutter="24" type="flex" align="stretch">
      <a-col :span="24" :lg="10" class="mb-24">
        <a-row type="flex" style="margin-bottom: 10px">
          <combobox
            :customData="customYearData"
            style="margin-right: 20px"
            :model="'2023'"
            @valueChanged="valueYearChanged"
          ></combobox>
          <combobox
            :customData="customWeekData"
            style="margin-right: 20px"
            :model="'9'"
            @valueChanged="valueWeekChanged"
          ></combobox>
        </a-row>
        <!-- Active Users Card -->
        <CardBarChart v-if="IsBarLoad" :data="dataChart[0]"></CardBarChart>
        <!-- Active Users Card -->
      </a-col>
      <a-col :span="24" :lg="14" class="mb-24">
        <!-- Sales Overview Card -->
        <a-row type="flex" style="margin-bottom: 10px">
          <combobox
            :customData="customYear2Data"
            style="margin-right: 20px"
            :model="'2023'"
            @valueChanged="valueYear2Changed"
          ></combobox>
        </a-row>
        <CardLineChart v-if="IsLineLoad" :ChartLine="TkYear[0]"></CardLineChart>
        <!-- / Sales Overview Card -->
      </a-col>
    </a-row>
    <!-- / Charts -->
    <a-row :gutter="24" type="flex" align="stretch"> </a-row>
    <!-- Table & Timeline -->
  </div>
</template>

<script>
// Bar chart for "Active Users" card.
import CardBarChart from "../../../src/components/Cards/CardBarChart";

// Line chart for "Sales Overview" card.
import CardLineChart from "../../../src/components/Cards/CardLineChart";

// Counter Widgets
//import WidgetCounter from "../../../src/components/Widgets/WidgetCounter";
import DepartmentsAPI from "../../api/components/departments/DepartmentsAPI";
import EmployeesAPI from "../../api/components/employees/EmployeesAPI";
import LaborContractsAPI from "../../api/components/laborcontracts/LaborContractsAPI";
import PositionsAPI from "../../api/components/positions/PositionsAPI";
import TK from "../../api/components/timekeepings/TimeKeepingsAPI";
import Combobox from "../../components/Combobox.vue";
// import LaborContractsAPI from "../../api/components/laborcontracts/LaborContractsAPI";
// import DepartmentsAPI from "../../api/components/departments/DepartmentsAPI";
// import PositionsAPI from "../../api/components/positions/PositionsAPI";
// Counter Widgets stats
var dataChart = [
  {
    title: "Số nhân viên đi làm",
    barChartData: {
      labels: ["CN", "02", "03", "04", "05", "06", "07"],
      datasets: [
        {
          label: "Số nhân viên đi đúng giờ",
          backgroundColor: "#fff",
          borderWidth: 0,
          borderSkipped: false,
          borderRadius: 6,
          data: [0, 0, 0, 1, 0, 0, 0],
          maxBarThickness: 20,
        },

        {
          label: "Số nhân viên đi muộn",
          backgroundColor: "red",
          borderWidth: 0,
          borderSkipped: false,
          borderRadius: 6,
          data: [0, 0, 0, 1, 0, 0, 0],
          maxBarThickness: 20,
        },
      ],
    },
  },
];
var stats = [
  {
    title: "Tổng số nhân viên",
    value: null,
    prefix: "",
    suffix: "",
    icon: `
						<svg width="22" height="22" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
							<path d="M8.43338 7.41784C8.58818 7.31464 8.77939 7.2224 9 7.15101L9.00001 8.84899C8.77939 8.7776 8.58818 8.68536 8.43338 8.58216C8.06927 8.33942 8 8.1139 8 8C8 7.8861 8.06927 7.66058 8.43338 7.41784Z" fill="#111827"/>
							<path d="M11 12.849L11 11.151C11.2206 11.2224 11.4118 11.3146 11.5666 11.4178C11.9308 11.6606 12 11.8861 12 12C12 12.1139 11.9308 12.3394 11.5666 12.5822C11.4118 12.6854 11.2206 12.7776 11 12.849Z" fill="#111827"/>
							<path fill-rule="evenodd" clip-rule="evenodd" d="M10 18C14.4183 18 18 14.4183 18 10C18 5.58172 14.4183 2 10 2C5.58172 2 2 5.58172 2 10C2 14.4183 5.58172 18 10 18ZM11 5C11 4.44772 10.5523 4 10 4C9.44772 4 9 4.44772 9 5V5.09199C8.3784 5.20873 7.80348 5.43407 7.32398 5.75374C6.6023 6.23485 6 7.00933 6 8C6 8.99067 6.6023 9.76515 7.32398 10.2463C7.80348 10.5659 8.37841 10.7913 9.00001 10.908L9.00002 12.8492C8.60902 12.7223 8.31917 12.5319 8.15667 12.3446C7.79471 11.9275 7.16313 11.8827 6.74599 12.2447C6.32885 12.6067 6.28411 13.2382 6.64607 13.6554C7.20855 14.3036 8.05956 14.7308 9 14.9076L9 15C8.99999 15.5523 9.44769 16 9.99998 16C10.5523 16 11 15.5523 11 15L11 14.908C11.6216 14.7913 12.1965 14.5659 12.676 14.2463C13.3977 13.7651 14 12.9907 14 12C14 11.0093 13.3977 10.2348 12.676 9.75373C12.1965 9.43407 11.6216 9.20873 11 9.09199L11 7.15075C11.391 7.27771 11.6808 7.4681 11.8434 7.65538C12.2053 8.07252 12.8369 8.11726 13.254 7.7553C13.6712 7.39335 13.7159 6.76176 13.354 6.34462C12.7915 5.69637 11.9405 5.26915 11 5.09236V5Z" fill="#111827"/>
						</svg>`,
  },
  {
    title: "Tổng số hợp đồng",
    value: null,
    suffix: "",
    icon: `
						<svg width="22" height="22" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
							<path d="M9 6C9 7.65685 7.65685 9 6 9C4.34315 9 3 7.65685 3 6C3 4.34315 4.34315 3 6 3C7.65685 3 9 4.34315 9 6Z" fill="#111827"/>
							<path d="M17 6C17 7.65685 15.6569 9 14 9C12.3431 9 11 7.65685 11 6C11 4.34315 12.3431 3 14 3C15.6569 3 17 4.34315 17 6Z" fill="#111827"/>
							<path d="M12.9291 17C12.9758 16.6734 13 16.3395 13 16C13 14.3648 12.4393 12.8606 11.4998 11.6691C12.2352 11.2435 13.0892 11 14 11C16.7614 11 19 13.2386 19 16V17H12.9291Z" fill="#111827"/>
							<path d="M6 11C8.76142 11 11 13.2386 11 16V17H1V16C1 13.2386 3.23858 11 6 11Z" fill="#111827"/>
						</svg>`,
  },
  {
    title: "Tổng số phòng ban",
    value: null,

    icon: `
						<svg width="22" height="22" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
							<path fill-rule="evenodd" clip-rule="evenodd" d="M3.17157 5.17157C4.73367 3.60948 7.26633 3.60948 8.82843 5.17157L10 6.34315L11.1716 5.17157C12.7337 3.60948 15.2663 3.60948 16.8284 5.17157C18.3905 6.73367 18.3905 9.26633 16.8284 10.8284L10 17.6569L3.17157 10.8284C1.60948 9.26633 1.60948 6.73367 3.17157 5.17157Z" fill="#111827"/>
						</svg>`,
  },
  {
    title: "Tổng số chức danh",
    value: null,

    icon: `
						<svg width="22" height="22" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
							<path fill-rule="evenodd" clip-rule="evenodd" d="M10 2C7.79086 2 6 3.79086 6 6V7H5C4.49046 7 4.06239 7.38314 4.00612 7.88957L3.00612 16.8896C2.97471 17.1723 3.06518 17.455 3.25488 17.6669C3.44458 17.8789 3.71556 18 4 18H16C16.2844 18 16.5554 17.8789 16.7451 17.6669C16.9348 17.455 17.0253 17.1723 16.9939 16.8896L15.9939 7.88957C15.9376 7.38314 15.5096 7 15 7H14V6C14 3.79086 12.2091 2 10 2ZM12 7V6C12 4.89543 11.1046 4 10 4C8.89543 4 8 4.89543 8 6V7H12ZM6 10C6 9.44772 6.44772 9 7 9C7.55228 9 8 9.44772 8 10C8 10.5523 7.55228 11 7 11C6.44772 11 6 10.5523 6 10ZM13 9C12.4477 9 12 9.44772 12 10C12 10.5523 12.4477 11 13 11C13.5523 11 14 10.5523 14 10C14 9.44772 13.5523 9 13 9Z" fill="#111827"/>
						</svg>`,
  },
];

// "Projects" table list of columns and their properties.
var tableColumns = [
  {
    title: "COMPANIES",
    dataIndex: "company",
    scopedSlots: { customRender: "company" },
    width: 300,
  },
  {
    title: "MEMBERS",
    dataIndex: "members",
    scopedSlots: { customRender: "members" },
  },
  {
    title: "BUDGET",
    dataIndex: "budget",
    class: "font-bold text-muted text-sm",
  },
  {
    title: "COMPLETION",
    scopedSlots: { customRender: "completion" },
    dataIndex: "completion",
  },
];
var TkYear = [
  {
    labelChart: ["số nhân viên đi làm đúng giờ", "số nhân viên đi làm muộn"],
    headerTitle: "Thống kê chấm công",
    lineChartData: {
      labels: [
        "Jan",
        "Feb",
        "Mar",
        "Apr",
        "May",
        "Jun",
        "Jul",
        "Aug",
        "Sep",
        "Oct",
        "Nov",
        "Dec",
      ],
      datasets: [
        {
          label: null,
          tension: 0.4,
          // borderWidth: 0,
          pointRadius: 0,
          borderColor: "#1890FF",
          borderWidth: 3,
          data: null,
          maxBarThickness: 6,
        },
        {
          label: null,
          tension: 0.4,
          // borderWidth: 0,
          pointRadius: 0,
          borderColor: "#B37FEB",
          borderWidth: 3,
          data: null,
          maxBarThickness: 6,
        },
      ],
    },
  },
];
var yearcurr = "";
var weekcurr = "";
export default {
  components: {
    CardBarChart,
    CardLineChart,
    Combobox,
  },
  data() {
    return {
      // Associating table data with its corresponding property.
      // tableData,

      // Associating table columns with its corresponding property.
      // dataChart: {

      // },
      IsLineLoad: false,
      IsBarLoad: false,
      tableColumns,

      // Counter Widgets Stats
      yearLine: 2023,
      yearcurr,
      weekcurr,
      stats,
      dataChart,
      TkYear,
      titleWorking: "Đi làm đúng giờ",

      customYearData: {
        height: 32,
        width: 150,
        displayValues: ["2023", "2022"],
        keys: ["2023", "2022"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "2023",
      },
      customWeekData: {
        height: 32,
        width: 150,
        displayValues: [],
        keys: [],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: null,
      },
      customYear2Data: {
        height: 32,
        width: 150,
        displayValues: ["2023", "2022"],
        keys: ["2023", "2022"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "2023",
      },
      curryear: "2023",
      currweek: "9",
      curryear2: "2023",
    };
  },
  created() {
    // Get current date
    const currentDate = new Date();

    // Get current year
    this.yearcurr = currentDate.getFullYear();

    // Get current week number (ISO week)
    this.weekcurr = this.getWeekNumber(currentDate);
    this.customWeekData.defaultValue = this.weekcurr.toString();
    this.generateWeek();
    this.getByWeekOption();
    this.getStaffData();
    this.getLCData();
    this.getDepartmentData();
    this.getPositionData();
    this.getByYearAll();
    console.log(this.dataChart[0].barChartData.datasets[0].data);
  },
  watch: {
    curryear: function (oldval, newval) {
      this.IsBarLoad = false;
      console.log(oldval, newval);
      this.getByWeekOption();
    },
    currweek: function (oldval, newval) {
      this.IsBarLoad = false;
      console.log(oldval, newval);
      this.getByWeekOption();
    },
    curryear2: function (oldval, newval) {
      this.IsLineLoad = false;
      console.log(oldval, newval);
      this.getByYearAll();
    },
  },
  methods: {
    async generateWeek() {
      for (var i = 1; i <= 53; i++) {
        this.customWeekData.displayValues.push(i);
        this.customWeekData.keys.push(i);
      }
    },

    async valueWeekChanged(val, id) {
      this.currweek = id;
      console.log(val);
    },

    async valueYearChanged(val, id) {
      this.curryear = id;
      console.log(val);
    },
    async valueYear2Changed(val, id) {
      this.curryear2 = id;
      console.log(val);
    },

    async getStaffData() {
      await EmployeesAPI.getAll()
        .then((response) => {
          console.log(response.data.length);
          this.stats[0].value = response.data.length;
        })
        .catch((e) => {
          console.log(e);
        });
    },

    async getLCData() {
      await LaborContractsAPI.getAll()
        .then((response) => {
          console.log(response.data.length);
          this.stats[1].value = response.data.length;
        })
        .catch((e) => {
          console.log(e);
        });
    },

    async getDepartmentData() {
      await DepartmentsAPI.getAll()
        .then((response) => {
          console.log(response.data.length);
          this.stats[2].value = response.data.length;
        })
        .catch((e) => {
          console.log(e);
        });
    },

    async getPositionData() {
      await PositionsAPI.getAll()
        .then((response) => {
          console.log(response.data.length);
          this.stats[3].value = response.data.length;
        })
        .catch((e) => {
          console.log(e);
        });
    },

    async getByWeekAll() {
      await TK.getByWeekAll()
        .then((response) => {
          console.log(response);
          this.dataChart[0].barChartData.datasets[0].data = response.data;
          console.log(this.dataChart[0].barChartData.datasets[0].data);
          this.IsBarLoad = true;
        })
        .catch((e) => {
          console.log(e);
        });
    },
    async getByYearAll() {
      //  year = this.yearLine;
      await TK.getByYearAll(this.curryear2)
        .then((response) => {
          console.log(response.data);

          this.TkYear[0].lineChartData.datasets[0].data = response.data.early;
          this.TkYear[0].lineChartData.datasets[1].data = response.data.late;
          console.log(this.TkYear);
          this.IsLineLoad = true;
        })
        .catch((e) => {
          console.log(e);
        });
    },

    async getByWeekOption() {
      await TK.getByWeekOption(this.curryear, this.currweek)
        .then((response) => {
          console.log(response);
          this.dataChart[0].barChartData.datasets[0].data = response.data.early;
          this.dataChart[0].barChartData.datasets[1].data = response.data.late;
          console.log(this.dataChart[0].barChartData.datasets[0].data);
          this.IsBarLoad = true;
        })
        .catch((e) => {
          console.log(e);
        });
    },

    // Function to get ISO week number
    getWeekNumber(date) {
      const oneJan = new Date(date.getFullYear(), 0, 1);
      const weekNum = Math.ceil(
        ((date - oneJan) / 86400000 + oneJan.getDay() + 1) / 7
      );
      return weekNum;
    },
  },

  async beforeCreate() {},
};
</script>

<style lang="scss">
</style>