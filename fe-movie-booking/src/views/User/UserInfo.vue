<template>
  <div class="container min-h-dvh bg-[var(--el-fill-color-blank)]">
    <el-tabs
      v-model="activeName"
      class="demo-tabs p-5"
      @tab-click="handleClick"
    >
      <!--Thông tin người dùng-->
      <el-tab-pane label="Thông tin tài khoản" name="first">
        <el-descriptions direction="vertical" border>
          <el-descriptions-item
            :rowspan="2"
            :width="140"
            label="Ảnh"
            align="center"
          >
            <el-image
              style="width: 100px; height: 100px"
              src="https://i.pinimg.com/236x/5e/e0/82/5ee082781b8c41406a2a50a0f32d6aa6.jpg"
            />
          </el-descriptions-item>
          <el-descriptions-item label="Họ và tên">{{
            userInfo.name
          }}</el-descriptions-item>

          <el-descriptions-item label="Tên đăng nhập">{{
            userInfo.username
          }}</el-descriptions-item>
          <el-descriptions-item label="Ngày sinh">
            <el-tag size="small">{{ userInfo.birthday }}</el-tag>
          </el-descriptions-item>
          <el-descriptions-item label="Địa chỉ">
            {{ formatAddress(userInfo.address) }}
          </el-descriptions-item>
        </el-descriptions>
      </el-tab-pane>
      <!--End thông tin người dùng-->
      <!--Điểm thưởng-->
      <el-tab-pane label="Điểm thưởng" name="second">
        <h1 class="text-2xl text-[var(--primary-color)]">Điểm của bạn:</h1>
        <div class="ml-2 my-4">
          <p>Điểm hiện có: {{ totalPoint }}</p>
        </div>
        <h1 class="text-2xl text-[var(--primary-color)] mb-5">
          Lịch sử nhận điểm:
        </h1>
        <el-table border :data="rewards" style="width: 100%">
          <el-table-column prop="pointCount" label="Điểm" width="180" />
          <el-table-column prop="earnedDate" label="Ngày nhận" width="180" />
          <el-table-column prop="expiryDate" label="Ngày hết hạn" />
        </el-table>
      </el-tab-pane>
      <!--End điểm Thường-->
      <!--Giao dịch-->
      <el-tab-pane label="Lịch sử giao dịch" name="third"
        ><el-table border :data="bookings" style="width: 100%">
          <el-table-column label="Mã vé" width="125">
            <template #default="scope">
              <qrcode-vue
                :value="scope.row.id.toString()"
                :size="100"
                level="H"
              />
            </template>
          </el-table-column>
          <el-table-column prop="movieName" label="Tên phim" width="" />
          <el-table-column label="Giờ chiếu" width="100">
            <template #default="scope">
              {{ formatTime(scope.row.showtime) }}
            </template>
          </el-table-column>
          <el-table-column label="Ngày mua" width="200">
            <template #default="scope">
              {{ formatDate(scope.row.bookingDate) }}
            </template>
          </el-table-column>
          <el-table-column label="Tổng tiền" width="100">
            <template #default="scope">
              {{ formatCurrency(scope.row.totalPrice) }}
            </template>
          </el-table-column>
          <el-table-column label="Ghế đã mua" width="200">
            <template #default="scope">
              <el-tag
                v-for="seat in scope.row.seats?.$values"
                :key="seat.Id"
                class="mr-1 mb-1"
                size="small"
              >
                {{ seat }}
              </el-tag>
            </template>
          </el-table-column>
          <el-table-column label="Đồ ăn" width="200">
            <template #default="scope">
              <div
                v-for="food in scope.row.foods?.$values"
                :key="food.Id"
                class="text-sm"
              >
                {{ food.name }} x {{ food.quantity }}
              </div>
            </template></el-table-column
          >
        </el-table>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>
<script setup>
import { onMounted, ref } from "vue";
import QrcodeVue from "qrcode.vue";
import useApi from "../../uses/fetchApi";
import { getUserId } from "../../uses/getInfor";
import { formatTime } from "../../uses/formatTime";
import { formatCurrency } from "../../uses/formatCurrency";

const userInfo = ref({});
const activeName = ref("first");
const { data, fetchData } = useApi();

const rewards = ref([]);
const totalPoint = ref(0);

const bookings = ref([]);
const fetchUserInfo = async () => {
  await fetchData("get", "/UserInfo/user-info", null, {
    userId: getUserId(),
  });
  if (data.value) {
    userInfo.value = data.value;
  }
  console.log(userInfo.value);
};

const fetchReward = async () => {
  await fetchData("get", "/UserInfo/reward-info", null, {
    userId: getUserId(),
  });
  if (data.value) {
    rewards.value = data.value.$values;
    console.log(rewards.value);
    totalPoint.value = rewards.value.reduce(
      (total, reward) => total + reward.pointCount,
      0
    );
  }
};

const fetchBooking = async () => {
  await fetchData("get", "/userinfo/booking-info", null, {
    userId: getUserId(),
  });
  if (data.value) {
    bookings.value = data.value.$values;
    console.log(bookings.value);
  }
};

const formatDate = (date) => {
  return new Date(date).toLocaleString("vi-VN", {
    dateStyle: "medium",
    timeStyle: "short",
  });
};

const formatAddress = (address) => {
  if (!address) return "Không có địa chỉ";
  return `${address.houseNumber || ""}, ${address.street || ""}, ${
    address.ward || ""
  }, ${address.district || ""}, ${address.city || ""}`
    .replace(/, ,/g, ",")
    .trim();
};

onMounted(() => {
  fetchUserInfo();
  fetchReward();
  fetchBooking();
});
</script>

<style scrope>
.demo-tabs > .el-tabs__content {
  padding: 16px;
  color: #6b778c;
  font-size: 1rem;
  font-weight: 600;
}

.el-tabs__item {
  padding: 1.3rem;
  color: #6b778c;
  font-size: 1.3rem;
  font-weight: 600;
}

.el-tabs__item.is-active,
.el-tabs__item:hover {
  color: var(--primary-color);
}

.el-tabs__active-bar {
  background-color: var(--primary-color);
}
</style>
