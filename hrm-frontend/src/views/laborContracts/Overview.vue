
<template>
  <div>
    <!-- Counter Widgets -->

    <!-- / Counter Widgets -->

    <!-- Charts -->
    <a-row :gutter="24" type="flex" align="stretch">
      <a-col :span="24" :lg="10" class="mb-24">
        <!-- Active Users Card -->
        <!-- <CardBarChart v-if="IsBarLoad" :data="dataChart[0]"></CardBarChart> -->
        <a-card :bordered="false" class="dashboard-bar-chart">
          <h6>Biểu đồ loại hợp đồng</h6>
          <div style="width: 480px">
            <chart-pie v-if="pieLoad" :pieData="pieData" />
          </div>
        </a-card>
        <!-- Active Users Card -->
      </a-col>
      <a-col :span="24" :lg="14" class="mb-24">
        <!-- Sales Overview Card -->
        <CardLineChart v-if="IsLineLoad" :ChartLine="TkYear[0]"></CardLineChart>
        <!-- / Sales Overview Card -->
      </a-col>
    </a-row>
    <!-- / Charts -->

    <!-- Table & Timeline -->
  </div>
</template>

<script>
// Bar chart for "Active Users" card.
//import CardBarChart from "../../../src/components/Cards/CardBarChart";

// Line chart for "Sales Overview" card.
import CardLineChart from "../../../src/components/Cards/CardLineChart";
import ChartPie from "../../components/Charts/ChartPie.vue";
// Counter Widgets
//import WidgetCounter from "../../../src/components/Widgets/WidgetCounter";
//import DepartmentsAPI from "../../api/components/departments/DepartmentsAPI";
import EmployeesAPI from "../../api/components/employees/EmployeesAPI";
import LaborContractsAPI from "../../api/components/laborcontracts/LaborContractsAPI";
//import PositionsAPI from "../../api/components/positions/PositionsAPI";
//import TK from "../../api/components/timekeepings/TimeKeepingsAPI";
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
          label: "Số nhân viên",
          backgroundColor: "#fff",
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
    labelChart: ["Tổng số nhân viên"],
    headerTitle: "Thống kê tổng số hợp đồng",
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
export default {
  components: {
    //  CardBarChart,
    CardLineChart,
    ChartPie,
  },
  data() {
    return {
      pieLoad: false,
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
          labels: ["thực tập", "chính thức", "thử việc"],

          colors: ["#0000ff", "#FFA500", "#EE4B2B"],
        },
        chartSeries: [0, 0, 0],
      },
      IsLineLoad: false,
      IsBarLoad: false,
      //  tableColumns,

      // Counter Widgets Stats
      //  stats,
      dataChart,
      TkYear,
      titleWorking: "Đi làm đúng giờ",
    };
  },
  created() {
    // this.getByWeekAll();
    this.getStaffData();
    this.getLCData();
    //  this.getDepartmentData();
    //  this.getPositionData();
    this.getByYearAll();
    console.log(this.dataChart[0].barChartData.datasets[0].data);
  },

  methods: {
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
          console.log("LC: ", response.data);
          response.data.forEach((lc) => {
            if (lc.status == 2) {
              if (lc.typeLaborContractID == 1) {
                this.pieData.chartSeries[0]++;
              } else if (lc.typeLaborContractID == 2) {
                this.pieData.chartSeries[1]++;
              } else if (lc.typeLaborContractID == 3) {
                this.pieData.chartSeries[2]++;
              }
            }
          });
          console.log("pie: ", this.pieData.chartSeries);
          this.pieLoad = true;
          //  this.stats[1].value = response.data.length;
        })
        .catch((e) => {
          console.log(e);
        });
    },

    async getByYearAll() {
      await EmployeesAPI.GetAllStaffPerMonth(2023)
        .then((response) => {
          console.log(response.data);

          this.TkYear[0].lineChartData.datasets[0].data = response.data;
          //    this.TkYear[0].lineChartData.datasets[1].data = response.data.late;
          console.log(this.TkYear);
          this.IsLineLoad = true;
        })
        .catch((e) => {
          console.log(e);
        });
    },
  },
  // async beforeCreate() {
  //   await TK.getByWeekAll()
  //     .then((response) => {
  //       console.log(response);
  //       this.dataChart[0].barChartData.datasets[0].data = response.data;
  //       console.log(this.dataChart[0].barChartData.datasets[0].data);
  //     })
  //     .catch((e) => {
  //       console.log(e);
  //     });
  // },
};
</script>

<style lang="scss">
</style>