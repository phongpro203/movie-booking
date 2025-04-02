<template>
  <div @click="console.log(filteredMovies)" class="container min-h-dvh">
    <el-dialog v-model="isShow" width="70%" title="Bạn đang đặt vé xem phim">
      <h1
        class="text-3xl border-y-1 text-[var(--text-color)] border-gray-300 pt-4 pb-2 antonio text-center"
      >
        {{ movieSelected }}
      </h1>
      <div
        class="grid grid-cols-3 gap-4 text-center mt-2 text-[var(--text-color)] font-bold"
      >
        <h1>Rạp chiếu</h1>
        <h1>Giờ chiếu</h1>
        <h1>Ngày chiếu</h1>
        <h1>{{ selectedCinema.cinemaName }}</h1>
        <h1 v-if="showtimeSelected">
          {{ formatTime(showtimeSelected.startTime) }} -
          {{ formatTime(showtimeSelected.endTime) }}
        </h1>
        <h1>{{ showtimeSelected.showDate }}</h1>
      </div>
      <div class="flex mt-2 border-t-1 border-gray-300">
        <el-button
          class="mx-auto my-5"
          type="primary"
          color="var(--primary-color)"
          @click="handleBooking(showtimeSelected)"
          >Xác nhận</el-button
        >
      </div>
    </el-dialog>
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
    <div class="grid grid-cols-1 md:grid-cols-2 pb-10 gap-4 px-4">
      <!--Movie img-->
      <div
        v-for="movie in filteredMovies"
        :key="movie.id"
        class="grid grid-cols-[5fr_7fr] gap-5 space-y-9 border-b border-gray-300 mt-9"
      >
        <img
          :src="movie.poster"
          alt="Quỷ Nhập Tràng"
          class="object-cover rounded-lg"
        />
        <!--Movie des-->
        <div class="text-[var(--text-color)]">
          <h1 class="antonio text-2xl text-[var(--primary-color)]">
            {{ movie.title }}
          </h1>
          <div class="flex gap-2 mb-5">
            <div>
              <i class="fa fa-tags"></i>
              <span>{{ movie.genre }}</span>
            </div>
            <div>
              <i class="fa-regular fa-clock"></i>
              <span>{{ movie.duration }} phút</span>
            </div>
          </div>
          <h2 class="font-bold text-lg mb-2">2D Phụ đề</h2>
          <div class="flex flex-wrap gap-4">
            <div
              v-for="showTime in movie.showTimes.$values"
              :key="showTime.id"
              class="text-center cursor-pointer text-sm"
            >
              <button
                @click="handleSelectedShowtime(showTime, movie.title)"
                class="w-full px-4 text-[var(text-color)] cursor-pointer bg-gray-300 py-2 rounded hover:bg-gray-400 transition"
              >
                {{ formatTime(showTime.startTime) }} -
                {{ formatTime(showTime.endTime) }}
              </button>
              <div class="text-[12px] mt-1"></div>
            </div>
          </div>
        </div>
        <!--End Movie des-->
      </div>
      <!--End Movie img-->
    </div>
  </div>
</template>
<script setup>
import { computed, onMounted, ref, watch } from "vue";
import { useRouter } from "vue-router";
import useApi from "../../uses/fetchApi";
import { useCinemaStore } from "../../store/cinemaStore";
import { formatTime } from "../../uses/formatTime";
import { getUpcomingDates } from "../../uses/createDay";

// Router
const router = useRouter();

// State
const dates = ref(getUpcomingDates());
const activeIndex = ref(0);
const isShow = ref(false);
const movies = ref([]);
const { data, fetchData } = useApi();
const cinemaStore = useCinemaStore();
const showtimeSelected = ref(null);
const selectedCinema = ref(null);
const movieSelected = ref(null);

// Methods
const fetchMovie = async () => {
  await fetchData("get", "/Movie/movie-by-cinema", null, {
    cinemaId: cinemaStore.selectedCinema,
  });
  movies.value = data.value.$values;
  console.log(movies.value);
};

const handleSelectedShowtime = (showtime, movieName) => {
  showtimeSelected.value = showtime;
  movieSelected.value = movieName;
  isShow.value = true;
};

onMounted(() => {
  fetchMovie();
  selectedCinema.value = cinemaStore.cinemas[0];
});

watch(
  () => cinemaStore.selectedCinema,
  (newVal) => {
    if (newVal) {
      fetchMovie();
      selectedCinema.value = cinemaStore.cinemas.find(
        (cinema) => cinema.id === newVal
      );
    }
  }
);

//Lọc ra phim và xuất chiếu theo ngày
const filteredMovies = computed(() => {
  const selectedDate = dates.value[activeIndex.value].fullDate;

  return movies.value
    .map((movie) => {
      // Lọc suất chiếu theo ngày
      const filteredShowTimes = movie.showTimes.$values.filter(
        (show) => show.showDate === selectedDate
      );

      // Nếu phim không có suất chiếu phù hợp, loại bỏ nó
      return filteredShowTimes.length > 0
        ? { ...movie, showTimes: { $values: filteredShowTimes } }
        : null;
    })
    .filter(Boolean); // Loại bỏ những phần tử null
});

const handleBooking = (showtime) => {
  router.push({ path: "/Booking", query: { showTimeId: showtime.id } });
};
</script>
