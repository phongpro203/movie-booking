<!-- PaymentSuccess.vue -->
<template>
  <div
    class="min-h-screen bg-gray-100 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8 text-[var(--text--color)]]"
  >
    <div v-if="booking == null">Không tìm thấy thông tin hóa đơn</div>
    <div
      v-else
      class="max-w-md w-full space-y-8 bg-white py-6 rounded-xl shadow-lg"
    >
      <!-- Success Icon and Header -->
      <div class="text-center text-green-600">
        <el-icon class="text-green-500 text-6xl mx-auto" :size="60">
          <CircleCheck />
        </el-icon>
        <h2 class="mt-6 text-3xl font-bold">Thanh toán thành công!</h2>
        <p class="mt-2 text-sm text-gray-600">
          Cảm ơn bạn đã đặt vé. Dưới đây là thông tin đặt vé của bạn:
        </p>
      </div>

      <div class="border-t-2 border-dashed border-gray-400 pt-4">
        <!-- Booking Information -->
        <div class="space-y-4 px-6">
          <h3 class="text-lg font-medium text-gray-900">Chi tiết đặt vé</h3>

          <div class="mt-4 space-y-3">
            <div class="flex justify-between">
              <span class="text-gray-600">Mã đặt vé:</span>
              <span class="font-medium">{{ booking.id }}</span>
            </div>

            <div class="flex justify-between">
              <span class="text-gray-600">Ngày đặt:</span>
              <span class="font-medium">{{
                formatDate(booking.bookingDate)
              }}</span>
            </div>

            <div class="flex justify-between">
              <span class="text-gray-600">Trạng thái:</span>
              <el-tag type="success">{{ booking.status }}</el-tag>
            </div>

            <div class="flex justify-between">
              <span class="text-gray-600">Tổng tiền:</span>
              <span class="font-medium text-[var(--primary-color)]">
                {{ formatCurrency(booking.totalPrice) }} VNĐ
              </span>
            </div>

            <!-- Movie name -->
            <div class="flex justify-between">
              <span class="text-gray-600">Tên phim:</span>
              <span class="font-medium">{{ booking.movieName }}</span>
            </div>

            <!-- Show Time Info -->
            <div class="flex justify-between">
              <span class="text-gray-600">Suất chiếu:</span>
              <span class="font-medium">{{
                formatTime(booking.showtime)
              }}</span>
            </div>

            <!-- Seats -->
            <div class="flex justify-between" v-if="booking.seats">
              <span class="text-gray-600">Ghế đã chọn:</span>
              <div class="mt-1">
                <el-tag
                  v-for="seat in booking.seats.$values"
                  :key="seat.Id"
                  class="mr-1 mb-1"
                  size="small"
                >
                  {{ seat }}
                </el-tag>
              </div>
            </div>

            <!-- Foods -->
            <div class="flex justify-between" v-if="booking.foods">
              <span class="text-gray-600">Đồ ăn:</span>
              <div class="mt-1">
                <div
                  v-for="food in booking.foods.$values"
                  :key="food.Id"
                  class="text-sm"
                >
                  {{ food.name }} x {{ food.quantity }}
                </div>
              </div>
            </div>

            <!-- Voucher -->
            <div v-if="booking.voucher" class="flex justify-between">
              <span class="text-gray-600">Voucher:</span>
              <span class="font-medium">{{ booking.voucher }}</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Actions -->
      <div class="flex justify-center gap-4">
        <el-button
          color="text-[var(--primary-color)]"
          type="primary"
          @click="goToHome"
        >
          Về trang chủ
        </el-button>
        <el-button plain @click="viewBookingDetail">
          Xem lịch sử giao dịch
        </el-button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";
import { useRouter, useRoute } from "vue-router";
import { CircleCheck } from "@element-plus/icons-vue";
import { formatCurrency } from "../../uses/formatCurrency";
import useApi from "../../uses/fetchApi";
import { formatTime } from "../../uses/formatTime";

// Giả lập dữ liệu booking
const booking = ref(null);

const router = useRouter();
const route = useRoute();

const { data, fetchData } = useApi();

const fetchBookingDetail = async () => {
  await fetchData("get", "/Booking/booking-detail", null, {
    bookingId: route.query.bookingId,
  });

  if (data.value) {
    booking.value = data.value;
  }
};

// Format ngày tháng
const formatDate = (date) => {
  return new Date(date).toLocaleString("vi-VN", {
    dateStyle: "medium",
    timeStyle: "short",
  });
};

// Điều hướng
const goToHome = () => {
  router.push("/");
};

const viewBookingDetail = () => {
  router.push("/UserInfo");
};

onMounted(() => {
  fetchBookingDetail();
});
</script>

<style scoped>
/* Có thể thêm style tùy chỉnh nếu cần */
</style>
