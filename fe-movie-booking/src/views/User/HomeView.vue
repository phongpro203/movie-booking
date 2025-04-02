<template>
  <div class="pt-25">
    <div class="relative w-full mb-8">
      <!-- Khung chứa slider -->
      <el-carousel
        :interval="4000"
        :height="carouselHeight"
        type="card"
        arrow="always"
        indicator-position="outside"
      >
        <el-carousel-item v-for="(image, index) in images" :key="index">
          <img :src="image" alt="Movie Image" class="carousel-img h-full" />
        </el-carousel-item>
      </el-carousel>
    </div>

    <div class="container mx-auto">
      <movie-view></movie-view>
    </div>
  </div>
</template>

<script setup>
import { onBeforeUnmount, onMounted, ref } from "vue";
import MovieView from "./MovieView.vue";

// State (dữ liệu)
const images = ref([
  "https://files.betacorp.vn/media%2fimages%2f2025%2f02%2f17%2fmv5botm5odblotatyjcwzi00yzkzlwizodetmtm2mtzlndfmmwu2xkeyxkfqcgc%2Dv1%2Dfmjpg%2Dux1000%2D151445%2D170225%2D93.jpg",
  "https://files.betacorp.vn/media%2fimages%2f2025%2f03%2f12%2fcopy%2Dof%2D250220%2Ddr25%2Dmain%2Db1%2Dlocalized%2Dembbed%2D164332%2D120325%2D55.jpg",
  "https://s9.opensubtitles.com/features/5/4/2/1408245.jpg",
]);
const carouselHeight = ref("600px");

// Method
const updateCarouselHeight = () => {
  if (window.innerWidth < 640) {
    carouselHeight.value = "400px"; // Mobile
  } else {
    carouselHeight.value = "580px"; // Desktop
  }
};

// Gọi khi component mounted
onMounted(() => {
  updateCarouselHeight();
  window.addEventListener("resize", updateCarouselHeight);
});

// Cleanup event listener khi component bị hủy
onBeforeUnmount(() => {
  window.removeEventListener("resize", updateCarouselHeight);
});
</script>

<style scoped>
/* Căn chỉnh toàn bộ carousel */

/* Tuỳ chỉnh item */
.el-carousel__item {
  display: flex;
  align-items: center;
  justify-content: center;
  background: transparent !important;
  border-radius: 10px;
}

/* Hình ảnh hiển thị đẹp hơn */
.carousel-img {
  object-fit: cover; /* Đảm bảo ảnh không bị méo */
  border-radius: 10px;
}
</style>
