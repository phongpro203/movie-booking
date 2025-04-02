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
        <span class="text-[var(--primary-color)] pt-10 text-xl"
          >45.000 VND</span
        >
      </div>
      <div>
        Ghế vip
        <div class="flex justify-center py-2 border-r-2 border-gray-300">
          <img class="w-20 h-20 highlight" :src="seatUnselect" alt="" />
        </div>
        <span class="text-[var(--primary-color)] pt-10 text-xl"
          >60.000 VND</span
        >
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
  </div>
</template>
<script setup>
import { computed, watch } from "vue";
import { formatCurrency } from "../../uses/formatCurrency";
import { useBookingStore } from "../../store/bookingStore";

// Props và Emits
const props = defineProps({
  seats: {
    type: Array,
    default: () => [],
  },
  selectedSeats: Array,
});

const emit = defineEmits(["update:selectedSeats"]);

// State (Dữ liệu)
// const countdown = ref(0);
const seatUnselect = "src/assets/images/unselect.png";
const seatSelected = "src/assets/images/selected.png";
const seatSold = "src/assets/images/booked.png";
const seatHold = "src/assets/images/hold.png";
const bookingStore = useBookingStore();

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
  seat.status == "booked" ? "" : "cursor-pointer",
  seat.seatType == "2D Vip" ? "highlight" : "",
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

const selectSeat = (seat) => {
  if (seat.status === "booked" || seat.status === "hold") return;

  // Cập nhật trạng thái ghế
  seat.status = seat.status === "selected" ? "available" : "selected";

  // Lọc ra danh sách ghế đã chọn
  const updatedSeats = props.seats.filter((s) => s.status === "selected");

  // Phát sự kiện để cập nhật dữ liệu lên component cha
  emit("update:selectedSeats", updatedSeats);
};

watch(totalPrice, (newVal) => {
  bookingStore.setTotalPriceOriginal(newVal);
});
</script>
