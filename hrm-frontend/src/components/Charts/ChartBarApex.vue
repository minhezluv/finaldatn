
   <template>
  <div class="example">
    <a-card :bordered="false" class="dashboard-bar-line header-solid">
      <h6>{{ barData.headerTitle }}</h6>
      <apexcharts
        width="700"
        height="350"
        type="bar"
        :options="barData.chartOptions"
        :series="barData.chartSeries"
      ></apexcharts>
    </a-card>
  </div>
</template>
 
 <script>
import VueApexCharts from "vue-apexcharts";

export default {
  name: "Chart",
  components: {
    apexcharts: VueApexCharts,
  },
  props: {
    barData: {
      chartOptions: {
        type: Object,
        default: {
          chart: {
            id: "basic-bar",
          },
          xaxis: {
            categories: [],
          },
        },
      },
      chartSeries: {
        Type: Array,
        default: [],
      },
      headerTitle: {
        type: String,
      },
      height: {
        type: Number,
      },
    },
  },
  data: function () {
    return {
      chartOptions: {
        chart: {
          id: "basic-bar",
        },
        xaxis: {
          categories: [1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998],
        },
      },
      series: [
        {
          name: "series-1",
          data: [30, 40, 45, 50, 49, 60, 70, 91],
        },
      ],
    };
  },
};
</script>