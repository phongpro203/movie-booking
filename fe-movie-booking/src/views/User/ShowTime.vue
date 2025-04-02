<template>
  <div
    class="mt-2 flex w-full border-b-1 border-gray-400 p-y-2 flex-1 text-2xl font-semibold mb-6 text-[var(--text-color)]"
  >
    <div
      v-for="(date, index) in dates"
      :key="index"
      @click="activeIndex = index"
      class="relative py-2 w-[175px] flex cursor-pointer transition-all duration-300"
      :class="{ 'text-[var(--primary-color)]': activeIndex === index }"
    >
      <div class="mx-auto">
        <span class="text-4xl">{{ date.day }}</span
        >/{{ date.month }} - {{ date.weekday }}
      </div>
      <span
        class="absolute left-0 bottom-0 w-full h-1 bg-[var(--primary-color)] scale-x-0 transition-transform duration-300"
        :class="{ 'scale-x-100': activeIndex === index }"
      ></span>
    </div>
  </div>
  <h2 class="font-bold text-lg mb-5">2D Phụ đề</h2>
  <div class="grid grid-cols-3 md:grid-cols-4 lg:grid-cols-7 gap-3">
    <div
      v-for="(ShowTime, index) in filteredShowTimes"
      :key="index"
      class="text-center cursor-pointer w-full"
    >
      <button
        class="w-full text-[var(--text-color)] cursor-pointer bg-gray-300 py-2 rounded hover:bg-gray-400 transition"
        @click="handleClick(ShowTime)"
      >
        {{ formatTime(ShowTime.startTime) }} -
        {{ formatTime(ShowTime.endTime) }}
      </button>
    </div>
  </div>
</template>
<script setup>
import { computed, ref } from "vue";
import { formatTime } from "../../uses/formatTime";
import { getUpcomingDates } from "../../uses/createDay";

// Props
const props = defineProps({
  ShowTimes: {
    type: Array,
    default: () => [],
  },
});

// State
const activeIndex = ref(0);

// Khởi tạo mảng dates từ ngày hiện tại
const dates = ref(getUpcomingDates());

const filteredShowTimes = computed(() => {
  const selectedDate = dates.value[activeIndex.value].fullDate;
  return props.ShowTimes.filter((show) => show.showDate === selectedDate);
});

// Emit sự kiện
const emit = defineEmits(["time-selected"]);

//  Methods
const handleClick = (ShowTime) => {
  emit("time-selected", ShowTime);
};
</script>
