
<template>
  <div style="height: 500px; overflow: auto">
    <!-- Charts -->
    <a-row :gutter="24" type="flex" align="stretch">
      <a-col :span="24" :lg="10" class="mb-24">
        <a-row type="flex" style="margin-bottom: 10px">
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
        </a-row>
        <!-- Active Users Card -->
        <a-card :bordered="false" class="dashboard-bar-chart">
          <h6>Biểu đồ chấm công</h6>
          <div style="width: 480px">
            <pie-chart v-if="IsPieLoad" :pieData="pieData" />
          </div>
        </a-card>
        <!-- Active Users Card -->
      </a-col>
      <a-col :span="24" :lg="14" class="mb-24">
        <!-- Sales Overview Card -->
        <a-row type="flex" style="margin-bottom: 40px"> </a-row>
        <a-card :bordered="false" class="dashboard-bar-chart">
          <h6>Biểu đồ chấm công</h6>
          <div style="height: 100%">
            <CalendarShow
              v-if="calendarShow"
              :year="currYear3"
              :month="currMonth - 1"
              :workingDays="workingDays"
            />
          </div>
          <h6>Số ngày nghỉ: {{ dayoff }}</h6>
          <h6>Số ngày đi làm đúng giờ: {{ early }}</h6>
          <h6>Số ngày đi muộn: {{ late }}</h6>
        </a-card>

        <!-- / Sales Overview Card -->
      </a-col>
    </a-row>
    <!-- / Charts -->

    <!-- Table & Timeline -->
  </div>
</template>

<script>
import DepartmentsAPI from "../../api/components/departments/DepartmentsAPI";
import EmployeesAPI from "../../api/components/employees/EmployeesAPI";
import LaborContractsAPI from "../../api/components/laborcontracts/LaborContractsAPI";
import PositionsAPI from "../../api/components/positions/PositionsAPI";
import TK from "../../api/components/timekeepings/TimeKeepingsAPI";
import Combobox from "../../components/Combobox.vue";
import PieChart from "../../components/Charts/ChartPie.vue";
import CalendarShow from "../../components/Calendars/CalendarShow.vue";
// import LaborContractsAPI from "../../api/components/laborcontracts/LaborContractsAPI";
// import PositionsAPI from "../../api/components/positions/PositionsAPI";
// Counter Widgets stats

var yearcurr = "";
var weekcurr = "";
export default {
  components: {
    // CardBarChart,
    // CardLineChart,
    Combobox,
    PieChart,
    CalendarShow,
    //  ChartBarApex,
  },
  data() {
    return {
      dayoff: null,
      early: null,
      late: null,
      staffCode: sessionStorage.getItem("staffCode"),
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
        headerTitle: "Lịch chấm công",
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
      calendarShow: true,
      workingDays: [],
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
      this.CalendarShow = false;
      console.log(oldval, newval);
      this.getByMonthOption();
    },
    currMonth: function (oldval, newValue) {
      this.IsBarMonthLoad = false;
      this.IsPieLoad = false;
      this.calendarShow = false;
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
        this.currPosition,
        this.staffCode
      ).then((response) => {
        // console.log("monthOption: ", response.data.data.nbOfWork);
        this.barData.chartSeries[0].data = response.data.data.nbOfWork;
        console.log("staffCode: ", this.staffCode);
        console.log("monthOption: ", this.barData);
        this.IsBarMonthLoad = true;
        (this.pieData.chartSeries[0] = response.data.data.early),
          (this.pieData.chartSeries[1] = response.data.data.late),
          (this.pieData.chartSeries[2] = response.data.data.dayoff);
        this.workingDays = response.data.data.times;
        this.dayoff = response.data.data.dayoff;
        this.early = response.data.data.early;
        this.late = response.data.data.late;
        this.calendarShow = true;
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