<template>
  <main class="pb-8">
    <div class="max-w-7xl mx-auto px-6">
      <div class="flex justify-center m-4">
        <div class="flex justify-evenly mb-4 w-200">
          <h2
            v-for="tab in movieStatus"
            :key="tab"
            @click="changeMovieStatus(tab)"
            :class="[
              'relative text-3xl font-semibold antonio cursor-pointer transition-all duration-300 px-4 pb-4',
              activeTab === tab
                ? 'text-[var(--primary-color)]'
                : 'text-[var(--text-color)]',
            ]"
          >
            {{ tab }}
            <span
              class="absolute left-0 bottom-0 w-full h-1 bg-[var(--primary-color)] scale-x-0 transition-transform duration-300"
              :class="{ 'scale-x-100': activeTab === tab }"
            ></span>
          </h2>
        </div>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-12">
        <!-- Movie Card 1 -->
        <div
          v-for="(movie, index) in movies"
          :key="index"
          class="product p-2 grid grid-cols-2 md:grid-cols-1 gap-4"
        >
          <div class="relative" @click="MovieDetail(movie.id)">
            <img
              :alt="movie.title"
              class="w-full h-90 object-cover rounded-[20px] overflow-hidden cursor-pointer"
              :src="movie.poster"
            />
            <span class="absolute top-0 right-0 text-white text-xs rounded-lg"
              ><img
                src="https://www.betacinemas.vn/assets/frontend/layout/img/hot.png"
                alt=""
            /></span>
          </div>
          <div class="">
            <h3
              @click="MovieDetail(movie.id)"
              class="font-bold antonio text-xl text-[var(--primary-color)] hover:cursor-pointer truncate w-full"
            >
              {{ movie.title }}
            </h3>
            <p class="text-gray-500">Thể loại: {{ movie.genre }}</p>
            <p class="text-gray-500">Thời lượng: {{ movie.duration }} phút</p>
            <div class="relative h-10">
              <el-button
                @click="fetchShowTime(movie)"
                class="my-5 w-full"
                color="var(--primary-color)"
                type="primary"
                >Mua vé</el-button
              >
            </div>
          </div>
        </div>
      </div>

      <!--Modal showTime-->
      <el-dialog v-model="outerVisible" width="90%">
        <template #header="{ titleId, titleClass }">
          <div class="my-header">
            <h4 class="antonio text-2xl" :id="titleId" :class="titleClass">
              Lịch chiếu - Attack on titan
            </h4>
          </div>
        </template>
        <h1
          class="text-3xl border-y-1 text-[var(--text-color)] border-gray-300 pt-4 pb-2 antonio text-center"
        >
          Rạp Beta Bắc Ninh
        </h1>
        <show-time
          @time-selected="handleSelectTime"
          :ShowTimes="showTimes"
        ></show-time>
        <div class="h-20"></div>
        <el-dialog
          v-model="innerVisible"
          width="70%"
          title="Bạn đang đặt vé xem phim"
          append-to-body
        >
          <h1
            class="text-3xl border-y-1 text-[var(--primary-color)] border-gray-300 pt-4 pb-4 antonio text-center"
          >
            {{ movieSelected.title }}
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
      </el-dialog>
    </div>
  </main>
</template>
<script setup>
import { onMounted, ref, watch } from "vue";
import { useRouter } from "vue-router";
import ShowTime from "./ShowTime.vue";
import useApi from "../../uses/fetchApi";
import { useCinemaStore } from "../../store/cinemaStore";
import { formatTime } from "../../uses/formatTime";

//DATA
const router = useRouter();

const showTimes = ref([]);

//Lưu xuất chiếu người dùng chọn
const showtimeSelected = ref({});
const movies = ref([]);
const cinemaStore = useCinemaStore();
const { data, fetchData } = useApi();
const movieSelected = ref({});
const _nowShowing = ref(true);

const selectedCinema = ref({});

//modal
const outerVisible = ref(false);
const innerVisible = ref(false);

//Lưu tab đang chọn
const activeTab = ref("PHIM ĐANG CHIẾU");
const movieStatus = ref(["PHIM ĐANG CHIẾU", "PHIM SẮP CHIẾU"]);

//METHOD
const fetchAllMovie = async () => {
  await fetchData("get", "/Movie/movie-showing", null, {
    cinemaId: cinemaStore.selectedCinema,
    nowShowing: _nowShowing.value,
  });
  movies.value = data.value.$values;
};

const fetchShowTime = async (movie) => {
  movieSelected.value = movie;
  outerVisible.value = true;
  await fetchData("get", "/ShowTime/ShowTime-by-movieId", null, {
    movieId: movie.id,
    cinemaId: cinemaStore.selectedCinema,
  });
  showTimes.value = data.value.$values;
};

const changeMovieStatus = (tab) => {
  activeTab.value = tab;
  tab === "PHIM SẮP CHIẾU"
    ? (_nowShowing.value = false)
    : (_nowShowing.value = true);
};

onMounted(() => {
  fetchAllMovie();
  selectedCinema.value = cinemaStore.cinemas[0];
});

//Theo dõi rạp chiếu được chọn
watch(
  () => cinemaStore.selectedCinema,
  (newVal) => {
    if (newVal) {
      fetchAllMovie();
      selectedCinema.value = cinemaStore.cinemas.find(
        (cinema) => cinema.id === newVal
      );
    }
  }
);

//Theo dõi trạng thái phim đang chiếu hay sắp chiếu
watch(
  () => _nowShowing.value,
  () => {
    fetchAllMovie();
  }
);

const MovieDetail = (index) => {
  router.push(`/MovieDetail/${index}`);
};

//Lưu xuất chiếu đã chọn đề truyền vào modal
const handleSelectTime = (ShowTime) => {
  showtimeSelected.value = ShowTime;
  innerVisible.value = true;
};

const handleBooking = (showtime) => {
  router.push({ path: "/Booking", query: { showTimeId: showtime.id } });
};
</script>
