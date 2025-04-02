import axios from "axios";
import router from "../router/index"; // Import router của bạn

// Tạo một instance của Axios
const api = axios.create({
  baseURL: "https://localhost:7201/api", // Đặt URL của API server
  headers: {
    "Content-Type": "application/json",
  },
});

// Thêm interceptor cho request
api.interceptors.request.use(
  (config) => {
    // Lấy token từ localStorage (nếu có)
    const token = localStorage.getItem("authToken");
    if (token) {
      // Thêm token vào header Authorization
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config; // Trả về config đã chỉnh sửa
  },
  (error) => {
    // Xử lý lỗi trong quá trình gửi request
    return Promise.reject(error);
  }
);

// Thêm interceptor cho response
api.interceptors.response.use(
  (response) => response, // Trả về response nếu thành công
  (error) => {
    if (error.response && error.response.status === 401) {
      // Nếu gặp lỗi xác thực, xóa token và chuyển hướng đến trang đăng nhập
      localStorage.removeItem("authToken");
      localStorage.removeItem("userInfo");
      router.push("/login");
    }
    if (error.response && error.response.status === 403) {
      router.push({ name: "NotFound" });
    }
    return Promise.reject(error); // Trả về lỗi để xử lý thêm (nếu cần)
  }
);

export default api;
