import axios from 'axios';
import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import process from 'node:process';
import './assets/main.css'
import '@fortawesome/fontawesome-free/css/all.css';
import '@fortawesome/fontawesome-free/js/all.js';

import "./assets/main.css";

axios.defaults.baseURL = process.env.NODE_ENV == 'production' ? "/api" : "https://localhost:5001/api";

const app = createApp(App);
app.use(router);
app.mount("#app");
