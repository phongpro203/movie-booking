import { defineConfig } from "vite";
import { fileURLToPath } from "url";
import vue from "@vitejs/plugin-vue";
import path from "path";
import tailwindcss from "@tailwindcss/vite";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
// https://vite.dev/config/
export default defineConfig({
  plugins: [vue(), tailwindcss()],
  server: {
    host: "0.0.0.0", // Cho phép truy cập từ tất cả địa chỉ IP trong mạng
    port: 5173, // Đặt cổng chạy (mặc định là 5173)
    strictPort: true, // Cố định chạy đúng cổng 5173
  },
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "src"), // Định nghĩa @ là src
    },
  },
});
