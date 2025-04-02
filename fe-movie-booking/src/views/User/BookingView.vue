<template>
  <div class="p-6 bg-gray-100 min-h-screen">
    <div class="container mx-auto p-6 rounded-lg">
      <h2
        v-if="showTime?.movie"
        class="text-xl font-bold text-[var(--primary-color)]"
      >
        Đặt vé &gt; {{ showTime.movie.title }}
      </h2>
      <div
        class="grid grid-cols-1 md:grid-cols-[minmax(300px,2fr)_minmax(250px,1fr)] gap-7 relative"
      >
        <booking-payment
          v-if="vouchers.$values && foods.$values"
          :vouchers="vouchers.$values"
          :foods="foods.$values"
          :reward="reward"
          @update-payment-info="handlePaymentInfo"
          v-show="isPayment"
        ></booking-payment>
        <!-- Loại ghế -->
        <booking-seat
          v-if="showTime?.seats"
          :seats="showTime.seats.$values"
          v-model:selectedSeats="selectedSeats"
          v-show="!isPayment"
        ></booking-seat>

        <!-- Thông tin phim -->
        <div
          v-if="showTime?.movie"
          class="gap-4 p-4 -mt-7 bg-white rounded-lg sticky -top-7 self-start"
        >
          <!-- Tiêu đề phim -->
          <div class="grid grid-cols-2">
            <img
              :src="showTime.movie.poster"
              alt="Quỷ Nhập Tràng"
              class="w-full object-cover rounded"
            />
            <div class="pl-4 space-y-4">
              <h3 class="antonio text-2xl text-[var(--primary-color)]">
                {{ showTime.movie.title }}
              </h3>
              <h4 class="text-gray-700">2D Phụ đề</h4>
            </div>
          </div>

          <!-- Thông tin phim -->
          <div class="p-4 rounded-lg">
            <ul class="space-y-4">
              <li class="grid grid-cols-2">
                <div class="flex items-center space-x-2">
                  <i class="fa fa-tags text-gray-500"></i>
                  <span>Thể loại</span>
                </div>
                <span class="font-bold">{{ showTime.movie.genre }}</span>
              </li>
              <li class="grid grid-cols-2">
                <div class="flex items-center space-x-2">
                  <i class="fa-regular fa-clock"></i>
                  <span>Thời lượng</span>
                </div>
                <span class="font-bold"
                  >{{ showTime.movie.duration }} phút</span
                >
              </li>
            </ul>
          </div>

          <!-- Thông tin chiếu phim -->
          <div class="text-[var(--text-color)]">
            <hr class="border-dashed border-t-2 my-8 border-gray-400" />
            <ul class="rounded-lg space-y-4 pt-4">
              <li
                v-for="(item, index) in bookingInfo"
                :key="index"
                class="grid grid-cols-2"
              >
                <div class="flex items-center space-x-2">
                  <i :class="item.icon" class="text-gray-500"></i>
                  <span>{{ item.label }}</span>
                </div>
                <span class="font-bold" :class="item.class">{{
                  item.value
                }}</span>
              </li>
              <li class="grid grid-cols-2">
                <div class="flex items-center space-x-2">
                  <i class="fa-solid fa-couch text-gray-500"></i>
                  <span>Ghế ngồi</span>
                </div>
                <span class="font-bold">{{
                  Array.isArray(selectedSeatsName)
                    ? selectedSeatsName.join(", ")
                    : selectedSeatsName
                }}</span>
              </li>
            </ul>
            <div class="flex my-4">
              <el-button
                @click="handleContinue"
                class="mx-auto"
                type="primary"
                color="var(--primary-color)"
                style="padding: 16px 32px"
                >{{ isPayment ? "Quay lại" : "Tiếp tục" }}</el-button
              >
              <el-button
                v-if="isPayment"
                class="mx-auto"
                type="primary"
                color="var(--primary-color)"
                style="padding: 16px 32px"
                @click="HandlePayment"
                >Thanh toán</el-button
              >
            </div>
          </div>
          <!-- End thông tin chiếu phim-->
        </div>
        <!--End right content-->
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeMount, reactive } from "vue";
import BookingPayment from "./BookingPayment.vue";
import BookingSeat from "./BookingSeat.vue";
import useApi from "../../uses/fetchApi";
import { useRoute } from "vue-router";
import router from "../../router";
import { formatTime } from "../../uses/formatTime";
import { getUserId } from "../../uses/getInfor";
import { ElLoading, ElMessage } from "element-plus";

// State (dữ liệu)
const selectedSeats = ref([]);
const isPayment = ref(false);
const { data, fetchData } = useApi();
const route = useRoute();
const showTime = ref({});
const foods = ref([]);
const vouchers = ref([]);
const reward = ref({});
const selectedPaymentInfo = ref({ foods: [], voucher: "", points: 0 });
const request = reactive({
  userId: 0,
  showTimeId: 0,
  seatIds: [],
  foodItems: [],
  voucherId: 0,
  pointCount: 0,
});

const bookingInfo = ref([
  { icon: "fa fa-institution", label: "Rạp chiếu", value: "" },
  { icon: "fa fa-calendar", label: "Ngày chiếu", value: "" },
  { icon: "fa-solid fa-clock", label: "Giờ chiếu", value: "" },
  { icon: "fa fa-desktop", label: "Phòng chiếu", value: "" },
]);

const InitBookingInfo = () => {
  bookingInfo.value[0].value = showTime.value.cinemaName;
  bookingInfo.value[1].value = showTime.value.showDate;
  bookingInfo.value[2].value = formatTime(showTime.value.startTime);
  bookingInfo.value[3].value = showTime.value.roomName;
};

const handlePaymentInfo = (data) => {
  selectedPaymentInfo.value = data;
};

// Fetch data

const fetchShowTimeDetail = async () => {
  await fetchData("get", "/ShowTime/showtime-detail", null, {
    showTimeId: route.query.showTimeId,
    userId: getUserId(),
  });

  if (data.value) {
    showTime.value = data.value.showTime;
    foods.value = data.value.food;
    vouchers.value = data.value.vouchers;
    reward.value = data.value.reward;
  }
};

const handleContinue = () => {
  if (selectedSeats.value.length <= 0) {
    alert("Vui lòng chọn ghế trước khi tiếp tục");
    return;
  }
  isPayment.value = !isPayment.value;
};

const checkoutBooking = async () => {
  const loading = ElLoading.service({
    lock: true,
    text: "Đang xử lý...",
    background: "rgba(0, 0, 0, 0.7)", // Làm mờ nền
  });
  try {
    await fetchData("post", "/Booking/checkout", request, null);
    if (data.value.length > 0) {
      window.location.href = data.value;
    }
  } catch (error) {
    console.error("Lỗi khi đặt vé:", error);
    ElMessage.error("Đặt vé thất bại");
  } finally {
    loading.close(); // Tắt loading
  }
};

onBeforeMount(() => {
  if (!route.query.showTimeId || route.query.showTimeId == "") {
    router.replace("/");
  }
});

onMounted(async () => {
  if (route.query.showTimeId && route.query.showTimeId.trim() !== "") {
    await fetchShowTimeDetail();
  }
  if (showTime.value) {
    InitBookingInfo();
  }
});

// Computed Properties
// const totalPrice = computed(() => selectedSeats.value.length * 50000);
const selectedSeatsName = computed(() =>
  selectedSeats.value.map((seat) => seat.seatNumber)
);

// Methods
const HandlePayment = () => {
  if (selectedSeats.value.length <= 0) {
    alert("Vui lòng chọn ghế trước khi thanh toán");
    return;
  }
  request.foodItems = selectedPaymentInfo.value.foods || [];
  request.voucherId = selectedPaymentInfo.value.voucher.id;
  request.pointCount = selectedPaymentInfo.value.points || 0;
  request.seatIds = selectedSeats.value.map((seat) => seat.id);
  request.userId = getUserId();
  request.showTimeId = route.query.showTimeId;

  checkoutBooking();
};
</script>

<style>
button {
  transition: background 0.3s;
}
</style>
