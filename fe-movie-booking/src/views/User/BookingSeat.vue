<template>
  <div class="text-center mt-6">
    <div
      class="grid grid-cols-2 md:grid-cols-4 text-sm p-4 font-semibold text-[var(--text-color)]"
    >
      <div class="flex items-center space-x-2">
        <img class="h-10 w-10" :src="seatUnselect" alt="" />
        <span>Ghế trống</span>
      </div>
      <div class="flex items-center space-x-2">
        <img class="h-10 w-10" :src="seatSelected" alt="" />
        <span>Ghế đang chọn</span>
      </div>
      <div class="flex items-center space-x-2">
        <img class="h-10 w-10" :src="seatHold" alt="" />
        <span>Ghế đang được giữ</span>
      </div>
      <div class="flex items-center space-x-2">
        <img class="h-10 w-10" :src="seatSold" alt="" />
        <span>Ghế đã bán</span>
      </div>
    </div>
    <div class="overflow-x-auto">
      <!-- Màn hình chiếu -->
      <div class="w-[764px]">
        <img
          src="https://www.betacinemas.vn/Assets/global/img/booking/ic-screen.png"
          alt="Màn hình chiếu"
          class="w-full"
        />
      </div>

      <!-- Ghế ngồi -->
      <div>
        <div
          class="grid grid-cols-10 gap-1 mt-4 p-10 pt-10"
          style="width: max-content; min-width: 700px"
        >
          <button
            v-for="seat in seats"
            :key="seat.id"
            :class="[
              'w-15 h-15 text-gray-700 bg-cover bg-center',
              getSeatClass(seat),
            ]"
            :style="getSeatStyle(seat)"
            @click="selectSeat(seat)"
          >
            <span class="mb-2 text-[10px] font-bold">
              {{ seat.seatNumber }}
            </span>
          </button>
        </div>
      </div>
    </div>

    <div class="p-4 grid grid-cols-3 rounded-l-lg bg-white font-semibold">
      <div>
        Ghế thường
        <div class="flex justify-center py-2 border-r-2 border-gray-300">
          <img class="w-20 h-20" :src="seatUnselect" alt="" />
        </div>
        <span class="text-[var(--primary-color)] pt-10 text-xl">
          45.000 VND
        </span>
      </div>
      <div>
        Ghế vip
        <div class="flex justify-center py-2 border-r-2 border-gray-300">
          <img class="w-20 h-20 highlight" :src="seatUnselect" alt="" />
        </div>
        <span class="text-[var(--primary-color)] pt-10 text-xl">
          60.000 VND
        </span>
      </div>
      <div>
        Tổng tiền
        <div
          class="flex-col justify-center pt-2 text-[var(--primary-color)] font-bold text-2xl mt-10"
        >
          <p>{{ formatCurrency(totalPrice) }} VNĐ</p>
        </div>
      </div>
    </div>
    <div v-if="countdown > 0">
      <p>Ghế của bạn đang được giữ trong {{ countdown }} giây nữa.</p>
    </div>
    <div v-else>
      <p>Thời gian giữ ghế đã hết! Bạn sẽ được chuyển hướng về trang Home.</p>
    </div>
  </div>
</template>

<script setup>
import { computed, onMounted, ref, watch } from "vue";
import { formatCurrency } from "../../uses/formatCurrency";
import { useBookingStore } from "../../store/bookingStore";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { useRouter } from "vue-router";
import { ElMessage } from "element-plus";

// Props và Emits
const props = defineProps({
  seats: {
    type: Array,
    default: () => [],
  },
  selectedSeats: Array,
  showtimeId: {
    type: Number,
    required: true,
  },
});

const emit = defineEmits(["update:selectedSeats"]);

const router = useRouter();
const countdown = ref(300);
const seatUnselect = "src/assets/images/unselect.png";
const seatSelected = "src/assets/images/selected.png";
const seatSold = "src/assets/images/booked.png";
const seatHold = "src/assets/images/hold.png";
const bookingStore = useBookingStore();

// SignalR Connection
let connection = null;
const connectSignalR = async () => {
  connection = new HubConnectionBuilder()
    .withUrl("https://localhost:7201/seathub") // URL của SignalR Hub
    .build();

  connection.on("SeatLocked", (showtimeId, seatId) => {
    if (showtimeId == props.showtimeId) {
      const seat = props.seats.find((s) => s.id == seatId);
      console.log(showtimeId);

      if (seat) seat.status = "hold"; // Cập nhật ghế đã được giữ
    }
  });

  connection.on("SeatUnlocked", (showtimeId, seatId) => {
    if (showtimeId == props.showtimeId) {
      const seat = props.seats.find((s) => s.id == seatId);
      if (seat) seat.status = "available"; // Cập nhật ghế đã mở lại
    }
  });

  await connection.start();
};

connectSignalR();

// Computed Properties
const totalPrice = computed(() => {
  let total = 0;
  props.selectedSeats.forEach((seat) => {
    total += seat.price;
  });
  return total;
});

// Methods (Hàm xử lý)

const getSeatClass = (seat) => [
  "flex items-center justify-center text-[12px]",
  seat.status === "booked" ? "" : "cursor-pointer",
  seat.seatType === "2D Vip" ? "highlight" : "",
];

const getSeatStyle = (seat) => {
  let backgroundImageUrl = "";

  switch (seat.status) {
    case "available":
      backgroundImageUrl = seatUnselect;
      break;
    case "selected":
      backgroundImageUrl = seatSelected;
      break;
    case "booked":
      backgroundImageUrl = seatSold;
      break;
    case "hold":
      backgroundImageUrl = seatHold;
      break;
    default:
      backgroundImageUrl = "";
  }

  return {
    backgroundImage: `url(${backgroundImageUrl})`,
  };
};

const selectSeat = async (seat) => {
  if (seat.status === "booked" || seat.status === "hold") {
    ElMessage.info("Ghế này đã được giữ hoặc đã đặt, vui lòng chọn ghế khác");
    return;
  }

  const amountSeatSelected = props.seats.filter((s) => s.status === "selected");
  if (amountSeatSelected.length > 10) {
    ElMessage.info(
      "Bạn chỉ được chọn tối đa 10 ghế, vui lòng đến tại rạp để đặt nhiều hơn!"
    );
    return;
  }

  // Cập nhật trạng thái ghế
  seat.status = seat.status === "selected" ? "available" : "selected";

  const wasSelected = seat.status === "selected";

  // Gửi thông báo tới các client khác về việc ghế đã bị chọn
  if (wasSelected) {
    await connection.invoke(
      "SelectSeat",
      props.showtimeId.toString(),
      seat.id.toString()
    );
  } else {
    await connection.invoke(
      "UnselectSeat",
      props.showtimeId.toString(),
      seat.id.toString()
    );
  }

  // Lọc ra danh sách ghế đã chọn
  const updatedSeats = props.seats.filter((s) => s.status === "selected");

  // Phát sự kiện để cập nhật dữ liệu lên component cha
  emit("update:selectedSeats", updatedSeats);
};

watch(totalPrice, (newVal) => {
  bookingStore.setTotalPriceOriginal(newVal);
});
watch(countdown, (newVal) => {
  if (newVal <= 0) {
    router.push({ name: "Home" });
  }
});

onMounted(() => {
  const interval = setInterval(() => {
    if (countdown.value > 0) {
      countdown.value -= 1; // Giảm số giây mỗi giây
    } else {
      clearInterval(interval); // Dừng bộ đếm khi hết thời gian
      // Chuyển hướng về trang Home khi hết thời gian
      ElMessage.info(
        "Đã hết thời gian chọn ghế, bạn đã được đưa về trang chủ!"
      );
      router.push({ name: "Home" });
    }
  }, 1000); // Giảm mỗi giây
});
</script>

<style scoped>
.selected {
  background-color: red;
}
</style>
