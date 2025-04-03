<template>
  <div class="text-[var(--text-color)]">
    <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mt-5">
      <div
        class="grid grid-cols-2"
        v-for="(food, index) in foods"
        :key="food.id"
      >
        <img
          src="https://png.pngtree.com/png-vector/20230817/ourmid/pngtree-cute-kawaii-food-stickers-clipart-vector-png-image_7022118.png"
          alt=""
        />
        <div class="space-y-2">
          <h1 class="text-xl font-bold text-[var(--primary-color)]">
            {{ food.name }}
          </h1>
          <p class="text-[var(--text-color)]">
            {{ food.detail }}
          </p>
          <p class="font-bold text-[var(--primary-color)]">
            {{ formatCurrency(food.price) }} VND
          </p>
          <div v-if="quantities.length > 0">
            <el-input-number
              v-model="quantities[index].quantity"
              :min="0"
              :max="10"
              @change="
                (value) => handleQuantityChange(value, index, food.price)
              "
            />
          </div>
        </div>
      </div>
    </div>
    <!--Voucher icon-->
    <div class="text-2xl mt-5">
      <div @click="isOpen = !isOpen" class="cursor-pointer">
        <i class="fa-solid fa-ticket mr-2"></i>
        <span>Voucher (Nhấn để xem voucher)</span>
      </div>
      <hr class="text-gray-400 my-5" />
    </div>
    <!--Voucher content-->
    <div
      class="overflow-y-auto transition-all duration-700 ease-in-out"
      :class="isOpen ? 'max-h-40 opacity-100' : 'max-h-0 opacity-0'"
    >
      <ul class="py-2">
        <li
          v-for="voucher in vouchers"
          @click="handleSelectVoucher(voucher)"
          :key="voucher.id"
          class="px-4 py-2 hover:bg-gray-200 cursor-pointer"
        >
          <h1 class="text-xl text-[var(--primary-color)]">
            {{ voucher.name }}
          </h1>
          <p>Giảm giá {{ voucher.discount }}%</p>
          <p class="text-[0.8rem]">HSD: {{ voucher.expirationDate }}</p>
        </li>
      </ul>
    </div>
    <div>Bạn đã chọn: {{ voucherSelected.name }}</div>
    <!--End voucher content-->
    <!--Điểm thưởng-->
    <div class="text-2xl mt-3">
      <div>
        <i class="fa-solid fa-ticket mr-2"></i>
        <span>Điểm thưởng</span>
      </div>
      <hr class="text-gray-400 my-5" />
    </div>

    <div class="grid grid-cols-1 md:grid-cols-4">
      <div>
        <h1>Điểm hiện có</h1>
        <h2 class="text-[var(--primary-color)]">
          {{ reward ? reward.pointCount : 0 }}
        </h2>
      </div>
      <div>
        <h1>Nhập điểm</h1>
        <input
          class="border-2 border-gray-300 outline-none h-10 w-40 text-[var(--primary-color)]"
          type="number"
          v-model="pointInput"
          @input="handlePoint"
        />
      </div>
      <div>
        <h1>Số tiền được giảm:</h1>
        <h2 class="text-[var(--primary-color)]">
          {{ formatCurrency(pointCountSelected) }} VND
        </h2>
      </div>
      <el-button
        @click="exchangePoint"
        type="primary"
        color="var(--primary-color)"
      >
        Đổi điểm</el-button
      >
    </div>
    <!--End Điểm thưởng-->
    <hr class="text-gray-400 my-5" />
    <!--Tổng tiền-->
    <div class="flex flex-col items-end mb-4">
      <h1>
        Tổng tiền: {{ formatCurrency(bookingStore.totalPriceOriginal) }} VND
      </h1>
      <h1 v-if="voucherSelected?.discount">
        Voucher: - {{ voucherSelected.discount }}%
      </h1>
      <h1>Điểm thưởng: - {{ formatCurrency(pointCountSelected) }} VND</h1>
      <h1>
        Số tiền cần thanh toán:
        {{ formatCurrency(bookingStore.totalPriceBooking) }} VND
      </h1>
    </div>
  </div>
</template>
<script setup>
import { computed, onMounted, ref, watch } from "vue";
import { formatCurrency } from "../../uses/formatCurrency";
import { useBookingStore } from "../../store/bookingStore";

const emits = defineEmits(["update-payment-info"]);
const quantities = ref([]);
const isOpen = ref(false);
const voucherSelected = ref("");
const pointCount = ref(0);
const pointInput = ref(0);
const pointCountSelected = ref(0);
const totalPrice = ref(0);
const bookingStore = useBookingStore();

const props = defineProps({
  vouchers: {
    type: Array,
    default: () => [],
  },
  foods: {
    type: Array,
    default: () => [],
  },
  reward: {
    type: Object,
    default: () => ({}),
  },
});

const selectedFoods = computed(() => {
  return quantities.value.filter((item) => item.quantity > 0);
});

const handleSelectVoucher = (voucher) => {
  isOpen.value = !isOpen.value;
  voucherSelected.value = voucher;
  bookingStore.setVoucher(voucher.discount);
};

const handlePoint = () => {
  if (pointInput.value > pointCount.value) {
    pointInput.value = pointCount.value; // Giới hạn số điểm nhập
  }
  pointCountSelected.value = pointInput.value;
};

const exchangePoint = () => {
  bookingStore.setPointCount(pointCountSelected.value);
};

const previousValues = ref([]); // Mảng lưu giá trị trước đó

const handleQuantityChange = (newValue, index, price) => {
  const oldValue = previousValues.value[index];

  if (newValue > oldValue) {
    bookingStore.addTotalPriceOriginal(price);
  } else if (newValue < oldValue) {
    bookingStore.minusTotalPriceOriginal(price);
  } else {
    bookingStore.addTotalPriceOriginal(price);
  }

  // Cập nhật giá trị cũ sau khi xử lý xong
  previousValues.value[index] = newValue;
};

watch([selectedFoods, voucherSelected, pointCountSelected], () => {
  emits("update-payment-info", {
    foods: selectedFoods.value,
    voucher: voucherSelected.value,
    points: pointCountSelected.value,
  });
});

onMounted(() => {
  if (props.reward) {
    pointCount.value = props.reward.pointCount;
  }
  quantities.value = props.foods.map((food) => ({ id: food.id, quantity: 0 }));
  totalPrice.value = bookingStore.totalPrice;
});
</script>
