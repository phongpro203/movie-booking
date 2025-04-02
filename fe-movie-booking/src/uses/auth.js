import api from "../services/api"; // Import axios instance
import { jwtDecode as jwt_decode } from "jwt-decode"; // Import jwt-decode để giải mã token

// Đăng nhập và lưu thông tin người dùng vào localStorage
export const login = async (request) => {
  try {
    // Gửi yêu cầu đăng nhập
    const response = await api.post("/Auth/Login", request);

    // Kiểm tra mã trạng thái HTTP
    if (response.status === 200) {
      // Kiểm tra nếu backend trả về lỗi (status false hoặc thông báo lỗi)
      if (response.data.success === false) {
        throw new Error(response.data.message || "Đăng nhập không thành công");
      }

      // Lưu token vào localStorage nếu đăng nhập thành công
      const token = response.data;
      localStorage.setItem("authToken", token);

      // Giải mã token để lấy thông tin người dùng
      const decodedToken = jwt_decode(token);

      // Lưu thông tin người dùng vào localStorage (lấy userid và role)
      const userInfo = {
        userid: decodedToken.nameid, // Lấy userid từ decoded token
        name: decodedToken.unique_name,
        role: decodedToken.role, // Lấy role từ decoded token
      };

      // Lưu thông tin người dùng vào localStorage
      localStorage.setItem("userInfo", JSON.stringify(userInfo)); // Lưu thông tin người dùng

      return decodedToken; // Trả về thông tin người dùng
    } else {
      // Nếu không phải 200, xử lý lỗi
      throw new Error("Đã xảy ra lỗi khi đăng nhập.");
    }
  } catch (error) {
    // Kiểm tra xem lỗi có phải do Unauthorized không
    if (error.response && error.response.status === 401) {
      console.error("Login failed: Tên đăng nhập hoặc mật khẩu không đúng.");
      throw "Tên đăng nhập hoặc mật khẩu không đúng.";
    } else {
      console.error("Login failed:", error);
      throw "Có lỗi xảy ra vui lòng thử lại sau."; // Ném lỗi để xử lý sau, ví dụ hiển thị thông báo lỗi
    }
  }
};
