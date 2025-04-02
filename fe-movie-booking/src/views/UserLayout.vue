<script setup>
import AppHeader from "@/components/AppHeader.vue";
import { onMounted, ref } from "vue";
import { useCinemaStore } from "@/store/cinemaStore";

const cinemaStore = useCinemaStore();
const isloading = ref(true);

onMounted(async () => {
  await cinemaStore.fetchCinemas();
  isloading.value = false;
});
</script>
<template>
  <div class="h-dvh flex" v-if="isloading">
    <div class="m-auto loader"></div>
  </div>
  <div v-if="cinemaStore.cinemas.length > 0" class="bg-gray-100">
    <app-header></app-header>
    <router-view class="pt-25" />
  </div>
</template>
