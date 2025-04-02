import { defineStore } from "pinia";
import { ref } from "vue";

export const useBookingStore = defineStore("bookingStore", () => {
  const totalPriceOriginal = ref(0); // Giá gốc chưa giảm
  const discountedPrice = ref(0); // Giá sau khi áp dụng voucher
  const totalPriceBooking = ref(0); // Giá cuối cùng sau khi trừ cả voucher & điểm đổi
  const discountPercent = ref(0); // Phần trăm giảm giá (voucher)
  const pointCount = ref(0); // Số điểm đổi

  // Cập nhật giá sau khi áp dụng voucher
  const updateDiscountedPrice = () => {
    discountedPrice.value =
      totalPriceOriginal.value * (1 - discountPercent.value / 100);
    updateFinalPrice();
  };

  // Cập nhật tổng tiền sau khi trừ điểm đổi
  const updateFinalPrice = () => {
    const maxUsablePoints = Math.min(pointCount.value, discountedPrice.value);
    pointCount.value = maxUsablePoints; // Đảm bảo không vượt quá giá trị được giảm
    totalPriceBooking.value = discountedPrice.value - maxUsablePoints;
  };

  // Đặt giá gốc ban đầu
  const setTotalPriceOriginal = (price) => {
    totalPriceOriginal.value = price;
    discountPercent.value = 0;
    pointCount.value = 0;
    updateDiscountedPrice();
  };

  // Thêm vào tổng giá gốc
  const addTotalPriceOriginal = (price) => {
    totalPriceOriginal.value += price;
    updateDiscountedPrice();
  };

  const minusTotalPriceOriginal = (price) => {
    totalPriceOriginal.value -= price;
    updateDiscountedPrice();
  };

  // Áp dụng mã giảm giá (voucher)
  const setVoucher = (discount) => {
    discountPercent.value = discount;
    updateDiscountedPrice();
  };

  // Áp dụng điểm đổi
  const setPointCount = (points) => {
    pointCount.value = points;
    updateFinalPrice();
  };

  // Reset toàn bộ booking sau khi thanh toán
  const resetBooking = () => {
    totalPriceOriginal.value = 0;
    discountedPrice.value = 0;
    totalPriceBooking.value = 0;
    discountPercent.value = 0;
    pointCount.value = 0;
  };

  return {
    totalPriceBooking,
    totalPriceOriginal,
    discountedPrice,
    setTotalPriceOriginal,
    setVoucher,
    setPointCount,
    resetBooking,
    addTotalPriceOriginal,
    minusTotalPriceOriginal,
  };
});
