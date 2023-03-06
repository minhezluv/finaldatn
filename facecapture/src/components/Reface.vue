<template>
  <section class="display">
    <video
      ref="videoEl"
      autoplay="true"
      playsinline
      @loadedmetadata="runModel"
    />
    <canvas ref="canvasEl" />
    <li class="board">
      <ul v-for="(item, key) in board" :key="key">
        {{
          key
        }}
        ：
        {{
          item
        }}
      </ul>
    </li>
  </section>
</template>

<script>
import * as faceAPI from "face-api.js";
import { onMounted, reactive, ref } from "vue";
import axios from "axios";
export default {
  setup() {
    let lastCaptureTime = 0;
    const initParams = reactive({
      modelUri: "/models",
      option: new faceAPI.SsdMobilenetv1Options({ minConfidence: 0.5 }),
    });
    const constraints = reactive({
      video: {
        width: {
          min: 320,
          ideal: 1280,
          max: 1920,
        },
        height: {
          min: 240,
          ideal: 720,
          max: 1080,
        },
        frameRate: {
          min: 15,
          ideal: 30,
          max: 60,
        },
        facingMode: "environment",
      },
    });
    const videoEl = ref(null);
    const canvasEl = ref(null);
    const board = reactive({
      "Số người trong hình": 0,
      // male: 0,
      // female: 0,
      fps: 0,
      "Mã nhân viên": "",
    });

    let forwardTimes = [];

    /**
     * caculate fps for detection
     * @function
     * @param number
     */
    const updateTimeStats = (timeInMs) => {
      forwardTimes = [timeInMs].concat(forwardTimes).slice(0, 30);
      const avgTimeInMs =
        forwardTimes.reduce((total, t) => total + t) / forwardTimes.length;
      board.fps = faceAPI.utils.round(1000 / avgTimeInMs);
    };

    /**
     * @function
     * @description detect input video
     */
    const runModel = async () => {
      const beforeDetect = Date.now();
      const result = await faceAPI
        .detectAllFaces(videoEl.value, initParams.option)
        .withAgeAndGender();
      updateTimeStats(Date.now() - beforeDetect);

      if (result) {
        const dims = faceAPI.matchDimensions(
          canvasEl.value,
          videoEl.value,
          true
        );
        const resizeResults = faceAPI.resizeResults(result, dims);
        board["Số người trong hình"] = resizeResults.length;
        //  board.realTimeCounts = resizeResults.length;
        // board.male = resizeResults.filter(
        //   (data) => data.gender === "male"
        // ).length;
        // board.female = resizeResults.filter(
        //   (data) => data.gender === "female"
        // ).length;
        faceAPI.draw.drawDetections(canvasEl.value, resizeResults);

        // Capture and upload image every 10 seconds
        const now = Date.now();
        if (now - lastCaptureTime >= 10000) {
          lastCaptureTime = now;
          const canvas = document.createElement("canvas");
          const context = canvas.getContext("2d");
          canvas.width = videoEl.value.videoWidth;
          canvas.height = videoEl.value.videoHeight;
          context.drawImage(videoEl.value, 0, 0, canvas.width, canvas.height);
          canvas.toBlob(async (blob) => {
            const formData = new FormData();
            formData.append("image", blob, "capture.jpg");
            try {
              const res = await axios.post(
                "https://t5chcqeshd.execute-api.ap-southeast-1.amazonaws.com/Prod/api/TimeKeeping/ReFace",
                formData
              );
              board["Mã nhân viên"] = res.data.data;
              console.log(res.data);
            } catch (error) {
              console.error(error.message);
            }
          }, "image/jpeg");
        }

        // Create a new image element with the detected faces
      }
      setTimeout(() => runModel());
    };

    onMounted(() => {
      /**
       * @function
       * @description load the trained model
       */
      const initModel = async () => {
        await faceAPI.nets.ssdMobilenetv1.loadFromUri(initParams.modelUri);
        await faceAPI.nets.ageGenderNet.loadFromUri(initParams.modelUri);
      };

      /**
       * startup webcam
       * @function
       */
      const startStream = async () => {
        try {
          const stream = await navigator.mediaDevices.getUserMedia(constraints);
          videoEl.value.srcObject = stream;
        } catch (error) {
          console.error(error.message);
        }
      };

      initModel();
      startStream();
    });

    return {
      videoEl,
      canvasEl,
      runModel,
      board,
    };
  },
};
</script>

<style lang="scss" scoped>
.display {
  width: 100%;
  height: 100vh;
  position: relative;
  > video {
    position: absolute;
    object-fit: cover;
    width: 100%;
    height: 100vh;
  }
  > canvas {
    position: absolute;
    z-index: 10;
    width: 100%;
    height: 100%;
  }
  > .board {
    font-size: 30px;
    list-style: none;
    background-color: rgba(255, 255, 255, 0.65);
    border-radius: 10px;
    left: 10px;
    padding: 15px;
    position: absolute;
    top: 10px;
    z-index: 20;
  }
}
</style>
