import { createRouter, createWebHistory } from "vue-router";
import userRoutes from "./modules/userRoutes";
import adminRoutes from "./modules/adminRoutes";
import authRoutes from "./modules/authRoutes";
import miscRoutes from "./modules/miscRoutes";

const routes = [...userRoutes, ...adminRoutes, ...authRoutes, ...miscRoutes];

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition; // Giữ vị trí khi nhấn nút "Quay lại"
    } else {
      return { top: 0 }; // Mặc định cuộn lên đầu
    }
  },
});
router.beforeEach((to, from, next) => {
  const userInfo = JSON.parse(localStorage.getItem("userInfo"));
  const token = localStorage.getItem("authToken");

  // Kiểm tra nếu có token nhưng đã hết hạn
  if (token) {
    const payload = JSON.parse(atob(token.split(".")[1])); // Giải mã payload của token
    const currentTime = Math.floor(Date.now() / 1000); // Lấy thời gian hiện tại

    if (payload.exp < currentTime) {
      console.log("Token expired. Redirecting to login...");
      localStorage.removeItem("authToken");
      localStorage.removeItem("userInfo");
    }
  }

  // Kiểm tra nếu route yêu cầu phải đăng nhập nhưng không có token
  if (to.meta.requiresAuth && !token) {
    return next({ name: "Login" }); // Nếu chưa đăng nhập, chuyển hướng đến login
  }

  // Kiểm tra nếu route yêu cầu phải có token (đã đăng nhập)
  if (to.meta.requiresAuth && !token) {
    next({ name: "Login" }); // Nếu chưa đăng nhập, chuyển hướng đến login
  } else if (
    to.meta.requiresAuth &&
    userInfo &&
    to.meta.role &&
    userInfo.role !== to.meta.role
  ) {
    // Kiểm tra nếu role không đúng thì chuyển hướng
    next({ name: "Home" }); // Nếu không có quyền, chuyển về trang chủ hoặc route khác
  } else {
    next(); // Tiếp tục nếu không có vấn đề gì
  }
});

export default router;
