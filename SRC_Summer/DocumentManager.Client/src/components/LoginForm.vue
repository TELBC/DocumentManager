<template>
  <div class="edit-dialog-overlay">
    <div class="login-form edit-dialog">
      <h2>Login</h2>
      <form @submit.prevent="login">
        <label for="username">Username:</label>
        <input
          type="text"
          id="username"
          v-model="username"
          required
          class="rounded-input"
        />
        <label for="password">Password:</label>
        <input
          type="password"
          id="password"
          v-model="password"
          required
          class="rounded-input"
        />
        <div class="edit-dialog-actions">
          <button type="submit">Login</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import axios from "../axios";

export default {
  data() {
    return {
      username: "",
      password: "",
    };
  },
  methods: {
    async login() {
      try {
        const response = await axios.post("/users/login", {
          username: this.username,
          password: this.password,
        });

        const token = response.data.token;
        localStorage.setItem("jwtToken", token);
        location.reload();
      } catch (error) {
        console.error(error);
      }
    },
  },
};
</script>

<style scoped>
.edit-dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.login-form {
  background-color: white;
  border-radius: 10px;
  padding: 30px;
  width: 300px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1), 0 1px 3px rgba(0, 0, 0, 0.08);
  display: flex;
  flex-direction: column;
}

label {
  font-weight: bold;
  font-size: 14px;
  margin-bottom: 5px;
}

input,
textarea {
  font-size: 14px;
  padding: 5px 10px;
  border: 1px solid #ccc;
  margin-bottom: 15px;
  width: 96%;
}

.rounded-input {
  border-radius: 5px;
}

.edit-dialog-actions {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1em;
}

button {
  padding: 0.5em 1em;
  margin-right: 0.5em;
  margin-top: 20px;
  background-color: var(--icon-color);
  border: none;
  color: white;
  font-size: 1em;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
}

button:hover {
  background-color: var(--icon-hover-color);
}
</style>
