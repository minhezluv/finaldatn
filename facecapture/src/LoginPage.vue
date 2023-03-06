<template>
  <div class="login-page">
    <div class="login-box">
      <h2 class="login-heading">Sign In</h2>
      <form class="login-form" @submit.prevent="submitForm">
        <div class="form-group">
          <label class="form-label" for="username">Username</label>
          <input
            class="form-input"
            type="text"
            id="username"
            v-model="form.username"
            required
          />
        </div>
        <div class="form-group">
          <label class="form-label" for="password">Password</label>
          <input
            class="form-input"
            type="password"
            id="password"
            v-model="form.password"
            required
          />
        </div>
        <button class="login-button">Sign In</button>
      </form>
      <div class="login-footer">
        <a class="login-link" href="#">Forgot Password?</a>
      </div>
    </div>
  </div>
</template>
  
  <script>
export default {
  data() {
    return {
      form: {
        username: "",
        password: "",
      },
    };
  },
  methods: {
    submitForm() {
      // Your login form submission logic here
    },
  },
};
</script>
  
  <style scoped>
.login-page {
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #00bfa5;
}

.login-box {
  max-width: 400px;
  width: 100%;
  padding: 20px;
  background-color: #ffffff;
  box-shadow: 0px 3px 9px rgba(0, 0, 0, 0.25);
  border-radius: 5px;
}

.login-heading {
  margin-top: 0;
  margin-bottom: 20px;
  font-size: 28px;
  font-weight: 700;
  color: #00bfa5;
  text-align: center;
}

.form-group {
  margin-bottom: 15px;
}

.form-label {
  display: block;
  margin-bottom: 5px;
  font-size: 16px;
  font-weight: 700;
  color: #4c4c4c;
}

.form-input {
  width: 100%;
  height: 40px;
  padding: 10px;
  font-size: 16px;
  color: #4c4c4c;
  background-color: #f2f2f2;
  border: none;
  border-radius: 5px;
  box-shadow: none;
}

.login-button {
  width: 100%;
  height: 50px;
  margin-top: 20px;
  font-size: 16px;
  font-weight: 700;
  color: #ffffff;
  background-color: #00bfa5;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.login-button:hover {
  background-color: #009c86;
}

.login-footer {
  margin-top: 20px;
  text-align: center;
  font-size: 16px;
  color: #4c4c4c;
}

.login-link {
  color: #00bfa5;
  text-decoration: none;
}

.login-link:hover {
  text-decoration: underline;
}
</style>
  