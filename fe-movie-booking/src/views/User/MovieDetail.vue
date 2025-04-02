<template>
  <div class="pt-30 container p-5">
    <h3 class="mb-5 text-xl font-semibold">
      <router-link to="/">Trang chủ ></router-link>
      <span class="text-[var(--primary-color)] font-semibold">{{
        movie.title
      }}</span>
    </h3>

    <div class="pl-2 grid grid-cols-1 md:grid-cols-4 gap-6">
      <!-- Cột ảnh -->
      <div class="relative md:col-span-1">
        <div class="rounded-xl overflow-hidden">
          <img
            class="w-full rounded-xl"
            alt="Quỷ Nhập Tràng"
            :src="movie.poster"
          />
        </div>
      </div>

      <!-- Cột nội dung -->
      <div class="md:col-span-3 space-y-4">
        <h1 class="font-bold text-2xl text-[var(--primary-color)]">
          {{ movie.title }}
        </h1>
        <el-rate
          v-model="averageRate"
          disabled
          show-score
          text-color="#ff9900"
          score-template="{value} điểm"
          disabled-void-color="#e1e4e8"
        />
        <p class="text-justify text-lg">
          {{ movie.description }}
        </p>

        <!-- Thông tin chi tiết -->
        <div class="grid grid-cols-[auto_1fr] gap-x-6 gap-y-1 text-lg">
          <div class="font-bold uppercase">Thể loại:</div>
          <div>{{ movie.genre }}</div>

          <div class="font-bold uppercase">Thời lượng:</div>
          <div>{{ movie.duration }} phút</div>

          <div class="font-bold uppercase">Ngôn ngữ:</div>
          <div>Tiếng Việt</div>

          <div class="font-bold uppercase">Ngày khởi chiếu:</div>
          <div>{{ movie.releaseDate }}</div>
        </div>
      </div>
    </div>
    <show-time
      v-if="showTimes.length > 0"
      @time-selected="handleSelectTime"
      :ShowTimes="showTimes"
    ></show-time>
    <!--ShowTimes-->
    <el-dialog
      v-model="innerVisible"
      width="70%"
      title="Bạn đang đặt vé xem phim"
      append-to-body
    >
      <h1
        class="text-3xl border-y-1 text-[var(--primary-color)] border-gray-300 pt-4 pb-4 antonio text-center"
      >
        {{ movie.title }}
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
          @click="handleBooking"
          >Xác nhận</el-button
        >
      </div>
    </el-dialog>
    <div class="trailer">
      <h1 class="text-3xl font-bold my-8 text-[var(--primary-color)]">
        Trailer
      </h1>
      <iframe
        class="w-full h-[80vh] pl-2"
        :src="movie.trailer"
        title="YouTube video player"
        frameborder="0"
        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
        referrerpolicy="strict-origin-when-cross-origin"
        allowfullscreen
      ></iframe>
    </div>

    <div class="rating py-5">
      <h1 class="text-3xl font-bold my-2 text-[var(--primary-color)]">
        Đánh giá
      </h1>
      <div class="pl-2">
        <el-rate
          v-model="tempRate"
          show-score
          text-color="#ff9900"
          @change="rating"
          score-template="{value} điểm"
          disabled-void-color="#e1e4e8"
        />
      </div>
    </div>

    <div class="comment py-5">
      <h1 class="text-3xl font-bold my-2 text-[var(--primary-color)]">
        Comment
      </h1>
      <!-- Comment Section -->
      <div class="max-w-full mx-auto p-4 bg-gray-100 rounded-lg">
        <!-- Form to add new comment -->
        <div class="my-4 flex items-center">
          <input
            v-model="newComment"
            @keyup.enter="addComment"
            placeholder="Viết bình luận..."
            class="flex-1 p-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 border-gray-400"
          />
          <button
            @click="addComment"
            class="ml-2 px-5 py-2 bg-[var(--primary-color)] text-white rounded-lg cursor-pointer hover:opacity-90"
          >
            Gửi
          </button>
        </div>
        <!-- Loop through comments -->
        <div
          v-for="comment in comments"
          :key="comment.id"
          class="flex items-start mb-4"
        >
          <!-- Avatar -->
          <img
            src="https://cellphones.com.vn/sforum/wp-content/uploads/2023/10/avatar-trang-4.jpg"
            alt="Avatar"
            class="w-10 h-10 rounded-full mr-3"
          />
          <!-- Comment Content -->
          <div class="flex-1">
            <p class="font-semibold text-gray-800">
              {{ comment.userName }}
            </p>
            <p class="text-gray-700 leading-4">{{ comment.comment }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
import { computed, onMounted, ref, watch } from "vue";
import ShowTime from "./ShowTime.vue";
import useApi from "../../uses/fetchApi";
import { useCinemaStore } from "../../store/cinemaStore";
import { useRouter, useRoute } from "vue-router";
import { formatTime } from "../../uses/formatTime";
import { getUserId } from "../../uses/getInfor";
import { ElMessage } from "element-plus";

// State
//data
const route = useRoute();
const tempRate = ref(0);
const router = useRouter();
const rate = ref(0);
const { data, fetchData } = useApi();
const movie = ref({});
const showTimes = ref([]);
const cinemaStore = useCinemaStore();
const selectedCinema = ref({});
const innerVisible = ref(false);
const showtimeSelected = ref({});
const reviews = ref([]);
const comments = ref([]);

// Reactive state for new comment input
const newComment = ref("");

//method
const addComment = async () => {
  if (newComment.value.trim() === "") return;
  if (!getUserId()) {
    ElMessage.warning("Vui lòng đăng hập để bình luận");
    return;
  }
  const payload = {
    movieId: movie.value.id,
    comment: newComment.value,
    userId: Number(getUserId()),
  };
  console.log(payload);

  await fetchData("post", "/Review/rating", payload);
  ElMessage.success("Đã bình luận");
  comments.value.unshift({
    userName: "Bạn",
    comment: newComment.value,
  });
};

const rating = async () => {
  if (!getUserId()) {
    ElMessage.warning("Vui lòng đăng hập để đánh giá");
    return;
  }
  if (rate.value !== 0) {
    tempRate.value = rate.value;
    ElMessage.warning("Bạn đã đánh giá rồi");
    return;
  }
  const payload = {
    rate: tempRate.value,
    movieId: movie.value.id,
    userId: Number(getUserId()),
  };
  console.log(payload);

  await fetchData("post", "/Review/rating", payload);
  ElMessage.success("Đánh giá thành công");
  rate.value = tempRate.value;
  validRates.value.push(rate.value);
};

const fetchMovie = async () => {
  await fetchData("get", "/Movie/movie-detail", null, {
    movieId: route.params.id,
    cinemaId: cinemaStore.selectedCinema,
  });
  movie.value = data.value;
  showTimes.value = movie.value.showTimes.$values;
  console.log(showTimes.value);

  reviews.value = movie.value.reviews.$values;
  comments.value = reviews.value.filter((review) => review.comment !== null);
  console.log(reviews.value);

  rate.value =
    reviews.value.find(
      (review) => review.userId == getUserId() && review.rate != null
    )?.rate ?? 0;
  tempRate.value = rate.value;
  console.log(rate.value);
};

const handleSelectTime = (ShowTime) => {
  showtimeSelected.value = ShowTime;
  innerVisible.value = true;
};

const handleBooking = () => {
  router.push({
    path: "/Booking",
    query: { showTimeId: showtimeSelected.value.id },
  });
};

onMounted(() => {
  fetchMovie();
  selectedCinema.value = cinemaStore.cinemas[0];
});

const validRates = computed(() => {
  return reviews.value
    .map((review) => review.rate) // Lấy tất cả rate
    .filter((rate) => rate !== null); // Loại bỏ rate null
});

// Tính trung bình rate
const averageRate = computed(() => {
  if (validRates.value.length === 0) return 0; // Tránh chia cho 0
  const total = validRates.value.reduce((sum, rate) => sum + rate, 0);
  return (total / validRates.value.length).toFixed(1);
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
// Methods
</script>
