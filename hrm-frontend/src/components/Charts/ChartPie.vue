<!-- <template>
  <div>
    <apexchart
      width="380"
      type="donut"
      :options="options"
      :series="series"
    ></apexchart>
  </div>
</template>
<script>
import Vue from "vue";
import VueApexCharts from "vue-apexcharts";
Vue.use(VueApexCharts);

Vue.component("apexchart", VueApexCharts);

export default {
  name: "PieChart",
  components: {
    apexchart: VueApexCharts,
  },
  data: function () {
    return {
      options: {},
      series: [44, 55, 41, 17, 15],
      dataLabels: {
        enabled: false,
      },
    };
  },
};
</script> -->

<template>
  <div>
    <apexchart
      type="pie"
      :options="pieData.chartOptions"
      :series="pieData.chartSeries"
    />
  </div>
</template>

<script>
import VueApexCharts from "vue-apexcharts";

export default {
  components: {
    apexchart: VueApexCharts,
  },
  props: {
    pieData: {
      chartOptions: {
        type: Object,
        default: {
          legend: {
            show: false,
          },
          labels: [],

          colors: [],
        },
      },
      chartSeries: {
        Type: Array,
        default: [],
      },
    },
    // chartSeries1: {
    //   Type: Array,
    //   default: [],
    // },
  },
  computed: {
    // chartOptions: this.pieData.chartOptions,
    //   chartSeries: this.chartSeries1,
  },
  data() {
    return {
      // chartOptions: {
      // },
      // chartSeries: [44, 55, 13],
      // chartOptions:{
      // }
    };
  },
};
</script>
