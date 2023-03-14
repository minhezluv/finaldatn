
<template>
  <div style="height: 500px; overflow: auto">
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
    <a-row :gutter="24" type="flex" align="stretch" style="margin-top: 30px">
      <a-row type="flex" style="margin-left: 20px; margin-bottom: 10px">
        <combobox
          :customData="customYear3Data"
          style="margin-right: 20px"
          :model="'2023'"
          @valueChanged="valueYear3Changed"
        ></combobox>
        <combobox
          :customData="customMonthData"
          style="margin-right: 20px"
          :model="'9'"
          @valueChanged="valueMonthChanged"
        ></combobox>
        <combobox
          :customData="customDepartmentData"
          style="margin-right: 20px"
          :model="'2023'"
          @valueChanged="valueDepartmentChanged"
        ></combobox>
        <combobox
          :customData="customPositionData"
          style="margin-right: 20px"
          :model="'2023'"
          @valueChanged="valuePositionChanged"
        ></combobox>
      </a-row>
      <a-row>
        <a-col :span="24" :lg="10" class="mb-24">
          <a-card :bordered="false" class="dashboard-bar-chart">
            <h6>Biểu đồ chấm công</h6>
            <div style="width: 480px">
              <pie-chart v-if="IsPieLoad" :pieData="pieData" />
            </div>
          </a-card>
        </a-col>
        <a-col :span="24" :lg="14" class="mb-24">
          <!-- Sales Overview Card -->

          <chart-bar-apex v-if="IsBarMonthLoad" :barData="barData" />
          <!-- / Sales Overview Card -->
        </a-col>
      </a-row>
    </a-row>
    <!-- Table & Timeline -->
  </div>
</template>

<script>
// Bar chart for "Active Users" card.
import CardBarChart from "../../../src/components/Cards/CardBarChart";

// Line chart for "Sales Overview" card.
import CardLineChart from "../../../src/components/Cards/CardLineChart";
import ChartBarApex from "../../components/Charts/ChartBarApex.vue";
// Counter Widgets
//import WidgetCounter from "../../../src/components/Widgets/WidgetCounter";
import DepartmentsAPI from "../../api/components/departments/DepartmentsAPI";
import EmployeesAPI from "../../api/components/employees/EmployeesAPI";
import LaborContractsAPI from "../../api/components/laborcontracts/LaborContractsAPI";
import PositionsAPI from "../../api/components/positions/PositionsAPI";
import TK from "../../api/components/timekeepings/TimeKeepingsAPI";
import Combobox from "../../components/Combobox.vue";
import PieChart from "../../components/Charts/ChartPie.vue";
// import LaborContractsAPI from "../../api/components/laborcontracts/LaborContractsAPI";
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
    PieChart,
    ChartBarApex,
  },
  data() {
    return {
      // Associating table data with its corresponding property.
      // tableData,

      // Associating table columns with its corresponding property.
      // dataChart: {

      // },

      pieData: {
        chartOptions: {
          legend: {
            show: true,
          },
          labels: ["đi đúng giờ", "đi muộn", " nghỉ"],

          colors: ["#0000ff", "#FFA500", "#EE4B2B"],
        },
        chartSeries: [],
      },
      barData: {
        chartOptions: {
          xaxis: {
            categories: [],
          },
        },
        chartSeries: [
          {
            name: "series-1",
            data: [],
          },
        ],
      },
      customPositionData: {
        height: 32,
        width: 200,
        displayValues: ["Tất cả chức danh"],
        keys: ["1"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "Tất cả chức danh",
      },
      customDepartmentData: {
        height: 32,
        width: 200,
        displayValues: ["Tất cả phòng ban"],
        keys: ["1"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "Tất cả phòng ban",
      },
      IsLineLoad: false,
      IsBarLoad: false,
      IsBarMonthLoad: false,
      IsPieLoad: false,
      // Counter Widgets Stats
      currDepartment: null,
      yearLine: 2023,
      yearcurr,
      weekcurr,
      currYear3: 2023,
      currMonth: 3,
      currPosition: null,
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
      customMonthData: {
        height: 32,
        width: 150,
        displayValues: ["Tất cả tháng"],
        keys: ["0"],
        // labelText: "Select an item",
        //defaultValue: this.$parent.defaultValue, // use default value from grandparent
        defaultValue: "Tất cả tháng",
      },
      customYear3Data: {
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
    this.currMonth = currentDate.getMonth() + 1;
    this.currweek = this.weekcurr.toString();
    this.customMonthData.defaultValue = currentDate.getMonth() + 1;
    this.generateDay();
    this.generateMonth();
    this.generateWeek();
    this.getByWeekOption();
    this.getStaffData();
    this.getLCData();
    this.getDepartmentData();
    this.getPositionData();
    this.getByYearAll();
    this.getByMonthOption();
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
    currYear3: function (oldval, newval) {
      this.IsBarMonthLoad = false;
      this.IsPieLoad = false;
      console.log(oldval, newval);
      this.getByMonthOption();
    },
    currMonth: function (oldval, newValue) {
      this.IsBarMonthLoad = false;
      this.IsPieLoad = false;
      console.log(oldval, newValue);
      this.getByMonthOption();
    },
    currDepartment: function (oldval, newValue) {
      this.IsBarMonthLoad = false;
      this.IsPieLoad = false;
      console.log(oldval, newValue);
      this.getByMonthOption();
    },
    currPosition: function (oldval, newValue) {
      this.IsBarMonthLoad = false;
      this.IsPieLoad = false;
      console.log(oldval, newValue);

      this.getByMonthOption();
    },
  },
  methods: {
    async generateMonth() {
      for (var i = 1; i <= 12; i++) {
        this.customMonthData.displayValues.push(i);
        this.customMonthData.keys.push(i);
      }
    },
    async generateDay() {
      for (var i = 1; i <= 31; i++) {
        this.barData.chartOptions.xaxis.categories.push(i);
        // this.barData.chartSeries[0].data.push(i);
      }
    },
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
    async valueDepartmentChanged(val, id) {
      this.currDepartment = id;
      console.log("department: ", val);
    },
    async valuePositionChanged(val, id) {
      this.currPosition = id;
      console.log("postion: ", val);
    },
    async valueMonthChanged(val, id) {
      this.currMonth = id;
      console.log("postion: ", val);
    },
    async valueYearChanged(val, id) {
      this.curryear = id;
      console.log(val);
    },
    async valueYear3Changed(val, id) {
      this.currYear3 = id;
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
          response.data.map((department) => {
            this.customDepartmentData.displayValues.push(
              department.departmentName
            );

            this.customDepartmentData.keys.push(department.guid);
          });
        })
        .catch((e) => {
          console.log(e);
        });
    },

    async getPositionData() {
      await PositionsAPI.getAll()
        .then((response) => {
          console.log(response.data.length);
          response.data.map((position) => {
            this.customPositionData.displayValues.push(position.positionName);

            this.customPositionData.keys.push(position.guid);
          });
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
      await TK.getByWeekOption(
        this.curryear,
        this.currweek,
        this.currDepartment,
        this.currPosition
      )
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
    async getByMonthOption() {
      await TK.getByMonthOption(
        this.currYear3,
        this.currMonth,
        this.currDepartment,
        this.currPosition
      ).then((response) => {
        // console.log("monthOption: ", response.data.data.nbOfWork);
        this.barData.chartSeries[0].data = response.data.data.nbOfWork;
        console.log("monthOption: ", this.barData);
        this.IsBarMonthLoad = true;
        (this.pieData.chartSeries[0] = response.data.data.early),
          (this.pieData.chartSeries[1] = response.data.data.late),
          (this.pieData.chartSeries[2] = response.data.data.dayoff);
        this.IsPieLoad = true;
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
.timeKeeping__element {
  padding: 10px;
  margin: 5px;
  width: 250px;
  border-radius: 15px;
}
.tk_row {
  padding-right: 5px;
  font-weight: 1000;
  color: white;
}
.day-off {
  background-color: red;
}
.working {
  background-color: blue;
}
.late {
  background-color: orange;
}
.info {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}
</style>