<template>
  <div class="pt-25">
    <el-carousel :height="carouselHeight" autoplay indicator-position="outside">
      <el-carousel-item v-for="image in images" :key="image">
        <img :src="image" alt="" class="carousel-image" />
      </el-carousel-item>
    </el-carousel>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from "vue";

const images = ref([
  "https://files.betacorp.vn/media/images/2025/02/14/1702x621-29-130857-140225-28.jpg",
  "https://files.betacorp.vn/media/images/2025/02/14/ngt-payoff-poster-1702x621-sneak-155826-140225-38.jpg",
  "https://external-preview.redd.it/heres-like-100-more-environment-screenshots-in-4k-to-use-as-v0-OdiurmQhB7HNiEZLKdDxwcU_F3qizHXF0Qun8S1RejQ.jpg?auto=webp&s=5469e972b0297a27e7071e86d39ccd48472681bc",
]);

const carouselHeight = ref("509px");

const updateHeight = () => {
  if (window.innerWidth < 768) {
    carouselHeight.value = "250px"; // Mobile nhỏ hơn
  } else {
    carouselHeight.value = "509px"; // Mặc định khi màn hình lớn
  }
};

onMounted(() => {
  updateHeight();
  window.addEventListener("resize", updateHeight);
});

onBeforeUnmount(() => {
  window.removeEventListener("resize", updateHeight);
});
</script>

<style scoped>
.carousel-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
</style>
