import App from "./App.vue";
import router from "./router";
import { createApp } from "vue";
import ElementPlus from "element-plus";
import "element-plus/dist/index.css";
import "./style.css";
import { createPinia } from "pinia";

const pinia = createPinia();

//kiểm tra xem còn hạn của token ko
const token = localStorage.getItem("authToken");
if (token) {
  const payload = JSON.parse(atob(token.split(".")[1])); // Giải mã payload
  const currentTime = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại
  if (payload.exp < currentTime) {
    // Token đã hết hạn
    console.log("Token expired. Redirecting to login...");
    localStorage.removeItem("authToken"); // Xóa token
    localStorage.removeItem("userInfo");
  }
}

createApp(App).use(router).use(ElementPlus).use(pinia).mount("#app");
