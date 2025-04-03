<template>
  <div class="flex flex-col min-h-screen bg-gray-100 pt-[10vh] pb-10">
    <div
      class="w-full mx-auto max-w-120 p-8 bg-white shadow-lg rounded-2xl transition-all duration-500 transform relative"
      :class="{ 'scale-105': isRegister }"
    >
      <h2 class="text-center text-2xl font-bold text-primary mb-4">
        🎬 Đặt Vé Xem Phim
      </h2>
      <p class="text-center text-gray-500 mb-6">
        Nhanh chóng, tiện lợi, trải nghiệm tuyệt vời!
      </p>

      <div class="flex justify-between mb-4">
        <button
          @click="isRegister = false"
          class="font-semibold text-lg cursor-pointer"
          :class="isRegister ? 'text-gray-400' : 'text-primary'"
        >
          Đăng nhập
        </button>
        <button
          @click="isRegister = true"
          class="font-semibold text-lg cursor-pointer"
          :class="isRegister ? 'text-primary' : 'text-gray-400'"
        >
          Đăng ký
        </button>
      </div>
      <!--Đăng nhập-->
      <form v-if="!isRegister" @submit.prevent="handleLogin">
        <div class="input-group">
          <i class="fas fa-user"></i>
          <input
            required
            v-model="loginForm.username"
            type="text"
            placeholder="Tên đăng nhập"
            class="input-field"
          />
        </div>
        <div class="input-group">
          <i class="fas fa-lock"></i>
          <input
            required
            v-model="loginForm.password"
            type="password"
            placeholder="Mật khẩu"
            class="input-field"
          />
        </div>
        <div class="flex justify-end">
          <router-link class="text-primary">Quên mật khẩu?</router-link>
        </div>
        <h3 class="text-red-500" v-if="errorMessage">{{ errorMessage }}</h3>
        <button type="submit" class="btn-primary">Đăng nhập</button>
      </form>

      <!--Đăng ký-->
      <form
        class="grid grid-cols-1 md:grid-cols-2 gap-2"
        v-else
        @submit.prevent="register"
      >
        <div class="input-group">
          <i class="fas fa-user"></i>
          <input
            required
            v-model="registerForm.username"
            type="text"
            placeholder="Tên đăng nhập"
            class="input-field"
          />
        </div>
        <div class="input-group">
          <i class="fas fa-lock"></i>
          <input
            ref="passwordInput"
            required
            v-model="registerForm.password"
            type="password"
            placeholder="Mật khẩu"
            class="input-field"
          />
        </div>

        <div class="input-group">
          <i class="fas fa-user"></i>
          <input
            required
            v-model="registerForm.fullname"
            type="text"
            placeholder="Họ và tên"
            class="input-field"
          />
        </div>
        <div class="input-group">
          <i class="fas fa-envelope"></i>
          <input
            required
            v-model="registerForm.email"
            type="email"
            placeholder="Email"
            class="input-field"
          />
        </div>

        <div class="input-group">
          <i class="fas fa-calendar-alt"></i>
          <el-date-picker
            v-model="registerForm.dob"
            type="date"
            placeholder="Ngày sinh"
            class="w-full"
          />
        </div>
        <!-- Chọn tỉnh/thành phố -->
        <div class="input-group">
          <i class="fas fa-map-marker-alt"></i>
          <el-select
            v-model="registerForm.province"
            filterable
            placeholder="Chọn Tỉnh/Thành phố"
            class="w-full"
            @change="fetchDistricts"
          >
            <el-option
              v-for="item in provinces"
              :key="item.code"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </div>

        <!-- Chọn quận/huyện -->
        <div class="input-group">
          <i class="fas fa-map-marker-alt"></i>
          <el-select
            v-model="registerForm.district"
            filterable
            placeholder="Chọn Quận/Huyện"
            class="w-full"
            @change="fetchWards"
          >
            <el-option
              v-for="item in districts"
              :key="item.code"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </div>

        <!-- Chọn phường/xã -->
        <div class="input-group">
          <i class="fas fa-map-marker-alt"></i>
          <el-select
            v-model="registerForm.ward"
            filterable
            placeholder="Chọn Phường/Xã"
            class="w-full"
          >
            <el-option
              v-for="item in wards"
              :key="item.code"
              :label="item.name"
              :value="item.code"
            />
          </el-select>
        </div>
        <!--Tên đường + số nhà-->
        <div class="grid grid-cols-2 gap-2 md:col-span-2">
          <div class="input-group mr">
            <i class="fa-solid fa-house"></i>
            <input
              required
              type="text"
              class="input-field"
              placeholder="Số nhà"
              v-model="registerForm.houseNumber"
            />
          </div>
          <div class="input-group">
            <i class="fa-solid fa-road"></i>
            <input
              required
              type="text"
              class="input-field"
              placeholder="Tên đường"
              v-model="registerForm.street"
            />
          </div>
        </div>
        <!-- End Tên đường + số nhà-->
        <span class="text-red-500 col-span-2" v-if="passMessage">{{
          passMessage
        }}</span>
        <button type="submit" class="btn-primary md:col-span-2">Đăng ký</button>
      </form>

      <div class="mt-6 text-center text-sm text-gray-500">
        <p>🎟️ Đặt vé ngay để không bỏ lỡ bộ phim yêu thích!</p>
        <p class="mt-2">🍿 Trải nghiệm xem phim tuyệt vời với chúng tôi.</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { login } from "../uses/auth";
import { useRouter } from "vue-router";
import useApi from "../uses/fetchApi";
import { ElMessage } from "element-plus";
import { getRolde } from "../uses/getInfor";

const isRegister = ref(false);
const loginForm = ref({ username: "", password: "" });
const errorMessage = ref("");
const passwordInput = ref(null);
const { data, fetchData } = useApi();

const registerForm = ref({
  username: "",
  fullname: "",
  email: "",
  password: "",
  dob: "",
  province: "",
  district: "",
  ward: "",
  houseNumber: "",
  street: "",
});

const provinces = ref([]);
const districts = ref([]);
const wards = ref([]);
const router = useRouter();
const passMessage = ref("");

// Hàm tìm tên từ code
const getNameByCode = (list, code) => {
  const item = list.find((i) => i.code === code);
  return item ? item.name : "";
};

const handleLogin = async () => {
  try {
    const request = {
      username: loginForm.value.username,
      password: loginForm.value.password,
    };
    await login(request);

    if (getRolde() == "admin") {
      router.push("/admin");
      return;
    }
    router.push("/");
  } catch (error) {
    errorMessage.value = error;
  }
};

const register = async () => {
  try {
    // Lấy tên từ code thay vì gửi code trực tiếp
    if (registerForm.value.password.length < 8) {
      passMessage.value = "Mật khẩu phải có ít nhất 8 ký tự";
      passwordInput.value.focus();
      return;
    }

    const user = {
      username: registerForm.value.username,
      password: registerForm.value.password,
      name: registerForm.value.fullname,
      birthday: registerForm.value.dob
        ? new Date(registerForm.value.dob).toISOString().split("T")[0]
        : null,
      role: "user",
      address: {
        city: getNameByCode(provinces.value, registerForm.value.province),
        district: getNameByCode(districts.value, registerForm.value.district),
        ward: getNameByCode(wards.value, registerForm.value.ward),
        houseNumber: registerForm.value.houseNumber,
        street: registerForm.value.street,
      },
    };

    await fetchData("post", "/Auth/register", user);

    if (data.value) {
      ElMessage({
        message: "Đăng ký thành công",
        type: "success",
      });

      router.push("/login");
    }
  } catch (error) {
    console.log(error);
  }
};

const fetchProvinces = async () => {
  try {
    const response = await axios.get(
      "https://provinces.open-api.vn/api/?depth=1"
    );
    provinces.value = response.data;
  } catch (error) {
    console.error("Lỗi khi tải danh sách tỉnh/thành:", error);
  }
};

const fetchDistricts = async () => {
  if (!registerForm.value.province) return;
  try {
    const response = await axios.get(
      `https://provinces.open-api.vn/api/p/${registerForm.value.province}?depth=2`
    );
    districts.value = response.data.districts;
    wards.value = [];
    registerForm.value.district = "";
    registerForm.value.ward = "";
  } catch (error) {
    console.error("Lỗi khi tải danh sách quận/huyện:", error);
  }
};

const fetchWards = async () => {
  if (!registerForm.value.district) return;
  try {
    const response = await axios.get(
      `https://provinces.open-api.vn/api/d/${registerForm.value.district}?depth=2`
    );
    wards.value = response.data.wards;
    registerForm.value.ward = "";
  } catch (error) {
    console.error("Lỗi khi tải danh sách phường/xã:", error);
  }
};

onMounted(fetchProvinces);
</script>

<style scoped>
:root {
  --primary-color: #3498db;
}

.text-primary {
  color: var(--primary-color);
}

.input-group {
  display: flex;
  align-items: center;
  background: #f9f9f9;
  border: 1px solid #ddd;
  border-radius: 20px;
  padding: 10px;
  margin: 8px 0;
}

.input-group i {
  margin-right: 10px;
  color: gray;
}

.input-field {
  flex: 1;
  border: none;
  background: transparent;
  outline: none;
}

.input-field:focus {
  border-color: var(--primary-color);
  outline: none;
}

.btn-primary {
  width: 100%;
  padding: 10px;
  background: var(--primary-color);
  color: white;
  font-weight: bold;
  border-radius: 20px;
  transition: 0.3s;
  cursor: pointer;
  margin-top: 10px;
}
.btn-primary:hover {
  background: #217dbb;
}
</style>
