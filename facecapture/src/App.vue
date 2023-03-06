<template>
  <div>
    <nav>
      <ul>
        <li :class="{ active: activeComponent === 'ReFace' }">
          <button @click="activeComponent = 'ReFace'">Nhận diện</button>
        </li>
        <li :class="{ active: activeComponent === 'UpFace' }">
          <button @click="activeComponent = 'UpFace'">Đăng ảnh</button>
        </li>
      </ul>
    </nav>
    <ReFace :is="activeComponent"></ReFace>
  </div>
</template>

<script>
import ReFace from "./components/Reface.vue";
import UpFace from "./components/Upface.vue";

export default {
  components: {
    ReFace,
    UpFace,
  },
  data() {
    return {
      activeComponent: "ReFace",
    };
  },
};
</script>

<style>
nav {
  position: absolute;
  z-index: 1000;
  bottom: 0px;
}
nav ul {
  display: flex;
  list-style-type: none;
  margin: 0;
  padding: 0;
}

nav li {
  margin: 0;
}

nav button {
  background-color: #4caf50;
  border: none;
  color: white;
  cursor: pointer;
  font-size: 16px;
  padding: 12px 16px;
  text-align: center;
  text-decoration: none;
}

nav button:hover {
  background-color: #3e8e41;
}

nav li.active button {
  background-color: #333;
}
</style>
