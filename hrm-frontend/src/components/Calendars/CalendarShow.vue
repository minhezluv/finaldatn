<template>
  <div>
    <v-date-picker
      v-if="moveMonth"
      :attributes="attributes"
      v-model="selectedDate"
      :picker-date.sync="pickerDate"
      is-expanded
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
    // const now = new Date();
    // const defaultYear = this.year ?? now.getFullYear();
    // const defaultMonth = this.month ?? now.getMonth();

    // const selectedDate = new Date(defaultYear, defaultMonth, 1);
    return {
      selectedDate: null,
      moveMonth: false,
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
  mounted() {
    this.updateSelectedDate();
  },
  watch: {
    year: function () {
      this.updateSelectedDate();
    },
    month: function () {
      this.updateSelectedDate();
      console.log("date picker: ", this.selectedDate);
    },
    selectedDate: function (newValue, oldValue) {
      if (oldValue == null) {
        this.updateSelectedDate();
        console.log("date picker: ", this.selectedDate);
      } else if (newValue.getTime() !== oldValue.getTime()) {
        this.updateSelectedDate();
        console.log("date picker: ", this.selectedDate);
      }
    },
  },
  computed: {
    pickerDate() {
      if (this.selectedDate) {
        return this.selectedDate.toISOString().substr(0, 10);
      } else {
        return null;
      }
    },
    attributes() {
      console.log("month: ", this.month);
      console.log("year: ", this.year);
      //

      const events = this.generateEventsFromWorkingDays(
        this.year,
        this.month,
        this.workingDays
      );
      console.log("date: ", this.selectedDate);
      // const selectedDateColor = "blue";
      return events.map((event) => ({
        key: `event.${event.id}`,
        highlight: {
          contentStyle: {
            color: "white",
          },
          style: {
            background: event.color,
            // background:
            //   event.start.getTime() === this.selectedDate?.getTime()
            //     ? event.coler
            //     : event.coler,
          },
        },
        dates: [event.start, event.end],
        customData: event,
      }));
    },
  },

  methods: {
    //update month
    updateSelectedDate() {
      const lastDayOfMonth = new Date(this.year, this.month + 1, 0);
      if (this.selectedDate > lastDayOfMonth) {
        this.selectedDate = lastDayOfMonth;
      } else {
        this.selectedDate = new Date(this.year, this.month, 1);
      }
      //  this.pickerDate = this.selectedDate;
      this.moveMonth = true;
    },

    generateEventsFromWorkingDays(year, month, workingDays) {
      const events = [];
      //this.selectedDate = new Date(year, month + 1, 0);
      const daysInMonth = new Date(year, month + 1, 0).getDate();
      var currDate = this.selectedDate.getDate();
      console.log("currDate: ", currDate);
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
