<template>
  <div>
    <v-date-picker
      :attributes="attributes"
      v-model="selectedDate"
      is-expanded
      :ref="calendar"
    ></v-date-picker>
  </div>
</template>

<script>
import Vue from "vue";
import VCalendar from "v-calendar";

Vue.use(VCalendar);

export default {
  name: "CustomCalendar",
  data() {
    return {
      selectedDate: null,
    };
  },
  props: {
    year: {
      type: Number,
      required: true,
    },
    month: {
      type: Number,
      required: true,
    },
    workingDays: {
      type: Array,
      required: true,
    },
  },
  // async created() {
  //   await this.$refs.calendar.move(new Date(2023, 1, 1));
  // },
  watch: {
    // month: function (oldval, newval) {
    //   this.selectedDate = new Date(2023, newval, 1);
    //   console.log(newval);
    // },
  },
  computed: {
    attributes() {
      console.log("month: ", this.month);

      //

      const events = this.generateEventsFromWorkingDays(
        this.year,
        this.month,
        this.workingDays
      );
      console.log(this.selectedDate);
      return events.map((event) => ({
        key: `event.${event.id}`,
        highlight: {
          contentStyle: {
            color: "white",
          },
          style: {
            background: event.color,
          },
        },
        dates: [event.start, event.end],
        customData: event,
      }));
    },
  },
  methods: {
    generateEventsFromWorkingDays(year, month, workingDays) {
      const events = [];

      const daysInMonth = new Date(year, month + 1, 0).getDate();

      for (let i = 1; i <= daysInMonth; i++) {
        if (workingDays[i - 1] === 1) {
          const event = {
            id: i,
            key: "workingDay.${i}",
            title: "Working Day",
            description: "This is a working day.",
            start: new Date(year, month, i),
            end: new Date(year, month, i),
            color: "orange",
          };

          events.push(event);
        }
        if (workingDays[i - 1] === 2) {
          const event = {
            id: i,
            title: "Working Day",
            description: "This is a working day.",
            start: new Date(year, month, i),
            end: new Date(year, month, i),
            color: "blue",
          };

          events.push(event);
        }
        if (workingDays[i - 1] === 3) {
          const event = {
            id: i,
            title: "Working Day",
            description: "This is a working day.",
            start: new Date(year, month, i),
            end: new Date(year, month, i),
            color: "grey",
          };

          events.push(event);
        }
        if (workingDays[i - 1] === 4) {
          const event = {
            id: i,
            title: "Working Day",
            description: "This is a working day.",
            start: new Date(year, month, i),
            end: new Date(year, month, i),
            color: "red",
          };

          events.push(event);
        }
      }

      return events;
    },
  },
};
</script>
