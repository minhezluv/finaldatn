<template>
  <!-- Active Users Card -->
  <a-card :bordered="false" class="dashboard-bar-chart">
    <!-- <div class="card-title"> -->
    <h6>{{ data.title }}</h6>
    <!-- <p>than last week <span class="text-success">+23%</span></p> -->
    <!-- </div> -->
    <chart-bar :height="300" :data="data.barChartData"></chart-bar>
  </a-card>
  <!-- Active Users Card -->
</template>

<script>
// Bar chart for "Active Users" card.
import ChartBar from "../Charts/ChartBar";
//import TK from "../../api/components/timekeepings/TimeKeepingsAPI";

export default {
  components: {
    ChartBar,
  },
  props: {
    data: {
      title: {
        type: String,
        default: "",
      },
      barChartData: {
        type: Object,
        require: true,
        default: {
          labels: ["CN", "02", "03", "04", "05", "06", "07"],
          datasets: [
            {
              label: "Đúng giờ",
              backgroundColor: "#fff",
              borderWidth: 0,
              borderSkipped: false,
              borderRadius: 6,
              data: [0, 0, 0, 0, 1, 0, 0],
              maxBarThickness: 20,
            },
          ],
        },
      },
    },
  },
  created() {
    //  this.getByWeekAll();
    //this.barChartData.datasets[0].data = newdata;
    // console.log("newdata: ", newdata);
    // this.data.barChartData = {
    //   labels: [],
    //   datasets: [
    //     {
    //       label: "Đúng giờ",
    //       backgroundColor: "#fff",
    //       borderWidth: 0,
    //       borderSkipped: false,
    //       borderRadius: 6,
    //       data: null,
    //       maxBarThickness: 20,
    //     },
    //   ],
    // };
  },

  data() {
    return {
      // // Data for bar chart.
      // newdata: [],
      // barChartData: {
      //   labels: ["CN", "02", "03", "04", "05", "06", "07"],
      //   datasets: [
      //     {
      //       label: "Đúng giờ",
      //       backgroundColor: "#fff",
      //       borderWidth: 0,
      //       borderSkipped: false,
      //       borderRadius: 6,
      //       data: [1],
      //       maxBarThickness: 20,
      //     },
      //   ],
      // },
    };
  },

  methods: {
    // async getByWeekAll() {
    //   await TK.getByWeekAll()
    //     .then((response) => {
    //       this.barChartData.datasets[0].data = response.data;
    //       // console.log(newdata);
    //     })
    //     .catch((e) => {
    //       console.log(e);
    //     });
    // },
  },
};
</script>