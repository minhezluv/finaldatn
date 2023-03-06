<template>
  <a-card :bordered="false" class="dashboard-bar-line header-solid">
    <template #title>
      <h6>{{ ChartLine.headerTitle }}</h6>
      <!-- <p>So với năm trước <span class="text-success">+20%</span></p> -->
    </template>
    <template #extra>
      <a-badge
        color="primary"
        class="badge-dot-primary"
        :text="ChartLine.labelChart[0]"
      />
      <a-badge
        color="primary"
        class="badge-dot-secondary"
        :text="ChartLine.labelChart[1]"
      />
    </template>
    <chart-line :height="310" :data="ChartLine.lineChartData"></chart-line>
  </a-card>
</template>

<script>
// Bar chart for "Active Users" card.
import ChartLine from "../Charts/ChartLine";

export default {
  components: {
    ChartLine,
  },
  props: {
    ChartLine: {
      headerTitle: {
        type: String,
        default: "",
      },
      labelChart: {
        type: Array,
        default: [],
      },
      lineChartData: {
        labels: ["Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
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
  },
  created() {
    // console.log("dataline: ", this.lineChartData.data);
  },
  data() {
    return {
      // Data for line chart.
      //   lineChartData: {
      //     labels: ["Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
      //     datasets: [
      //       {
      //         label: this.ChartData.title[0],
      //         tension: 0.4,
      //         // borderWidth: 0,
      //         pointRadius: 0,
      //         borderColor: "#1890FF",
      //         borderWidth: 3,
      //         data: this.ChartData.data[0],
      //         maxBarThickness: 6,
      //       },
      //       {
      //         label: this.ChartData.title[1],
      //         tension: 0.4,
      //         // borderWidth: 0,
      //         pointRadius: 0,
      //         borderColor: "#B37FEB",
      //         borderWidth: 3,
      //         data: this.ChartData.data[1],
      //         maxBarThickness: 6,
      //       },
      //     ],
      //   },
      // };
    };
  },
};
</script>