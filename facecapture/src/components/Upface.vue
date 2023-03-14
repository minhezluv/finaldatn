<template>
  <div class="camera">
    <video ref="video" autoplay></video>
    <button class="capture-btn" @click="capture" :disabled="capturing">
      {{ capturing ? "Capturing..." : "Capture" }}
    </button>
    <input
      type="text"
      v-model="code"
      class="upload-url-input"
      placeholder="Nhập mã nhân viên"
    />
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      capturing: false,
      images: [],
      code: "",
    };
  },
  mounted() {
    navigator.mediaDevices.getUserMedia({ video: true }).then((stream) => {
      this.$refs.video.srcObject = stream;
    });
  },
  methods: {
    capture() {
      this.capturing = true;
      const interval = setInterval(() => {
        if (this.images.length < 5) {
          const canvas = document.createElement("canvas");
          const video = this.$refs.video;
          canvas.width = video.videoWidth;
          canvas.height = video.videoHeight;
          canvas
            .getContext("2d")
            .drawImage(video, 0, 0, canvas.width, canvas.height);
          const dataUrl = canvas.toDataURL();
          const file = this.dataURItoFile(dataUrl);
          const formData = new FormData();
          formData.append(
            "images",
            file,
            `image-${this.images.length + 1}.jpg`
          );
          this.images.push(formData);
          this.showPopup("Success Capture!");
        } else {
          clearInterval(interval);
          this.uploadImages();
        }
      }, 3000);
    },
    dataURItoFile(dataURI) {
      const arr = dataURI.split(",");
      const mime = arr[0].match(/:(.*?);/)[1];
      const bstr = atob(arr[1]);
      let n = bstr.length;
      const u8arr = new Uint8Array(n);
      while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
      }
      return new File([u8arr], "capture.jpeg", { type: mime });
    },
    async uploadImages() {
      try {
        const formData = new FormData();
        this.images.forEach((image) => {
          formData.append(
            "images",
            image.get("images"),
            image.get("images").name
          );
        });
        await axios.post(
          `http://hrm3-env.eba-qghius76.ap-southeast-1.elasticbeanstalk.com/api/Staff/UpFace?code=${this.code}`,
          //  `https://localhost:44384/api/Staff/UpFace?code=${this.code}/`,
          formData,
          {
            headers: {
              "Content-Type": "image/jpeg",
            },
          },
          { timeout: 5000 }
        );
        this.images = [];
        this.showPopup("Upload Success!");
      } catch (error) {
        console.error(error);
        this.showPopup("Upload Error!");
      } finally {
        this.images = [];
        this.capturing = false;
      }
    },
    showPopup(message) {
      alert(message);
    },
  },
};
</script>

<style scoped>
.camera {
  position: relative;
}
video {
  display: block;
  margin: 0 auto;
  max-width: 100%;
  height: auto;
}
.capture-btn {
  position: absolute;
  bottom: 20px;
  left: 50%;
  transform: translateX(-50%);
  background-color: #4caf50;
  border: none;
  color: #ffffff;
  padding: 16px 32px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin: 4px 2px;
  cursor: pointer;
  border-radius: 4px;
  transition-duration: 0.4s;
}
.capture-btn:hover {
  background-color: #3eee;
}
.upload-url-input {
  position: absolute;
  bottom: 27px;
  left: calc(50% + 40px);
  width: 200px;
  height: 40px;
  border-radius: 20px;
  border: none;
  padding: 0 10px;
  font-size: 14px;
  color: #333333;
  outline: none;
  margin-left: 37px;
}
</style>