<!-- ShowTime.vue -->
<template>
  <div class="container-admin flex-1">
    <h1 class="font-bold text-2xl mb-4 text-[var(--primary-color)]">
      Quản lí xuất chiếu
    </h1>
    <!-- Add Button -->
    <div class="flex justify-end">
      <el-button type="primary" class="mb-4" @click="handleAdd"
        >Thêm mới</el-button
      >
    </div>
    <!-- Table -->
    <el-table :data="showTimes" style="width: 100%" border>
      <el-table-column prop="showDate" label="Ngày chiếu" width="150" />
      <el-table-column prop="startTime" label="Giờ bắt đầu" width="120" />
      <el-table-column prop="movie.title" label="Phim" />
      <el-table-column prop="room.name" label="Phòng" />
      <el-table-column prop="cinema.name" label="Rạp" />
      <el-table-column label="Hành động" width="200">
        <template #default="scope">
          <el-button size="small" @click="handleEdit(scope.row)">Sửa</el-button>
          <el-button size="small" type="danger" @click="handleDelete(scope.row)"
            >Xóa</el-button
          >
        </template>
      </el-table-column>
    </el-table>

    <!-- Pagination -->
    <div class="mt-4 flex justify-end">
      <el-pagination
        v-model:current-page="currentPage"
        :page-size="10"
        :total="totalItems"
        layout="prev, pager, next, jumper"
        @current-change="handlePageChange"
      />
    </div>

    <!-- Add/Edit Dialog -->
    <el-dialog
      :title="form.id ? 'Sửa Suất Chiếu' : 'Thêm Suất Chiếu'"
      v-model="dialogVisible"
      width="30%"
    >
      <el-form :model="form" ref="formRef" :rules="rules" label-position="top">
        <el-form-item label="Phim" prop="movieId">
          <el-select
            v-model="form.movieId"
            placeholder="Chọn phim"
            class="w-full"
          >
            <el-option
              v-for="movie in movies"
              :key="movie.id"
              :label="movie.title"
              :value="movie.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="Rạp" prop="cinemaId">
          <el-select
            v-model="form.cinemaId"
            placeholder="Chọn rạp"
            class="w-full"
          >
            <el-option
              v-for="cinema in cinemas"
              :key="cinema.id"
              :label="cinema.name"
              :value="cinema.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="Phòng" prop="roomId">
          <el-select
            v-model="form.roomId"
            placeholder="Chọn phòng"
            class="w-full"
          >
            <el-option
              v-for="room in filteredRooms"
              :key="room.id"
              :label="room.name"
              :value="room.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="Ngày chiếu" prop="showDate">
          <el-date-picker
            v-model="form.showDate"
            type="date"
            placeholder="Chọn ngày"
            format="YYYY-MM-DD"
            value-format="YYYY-MM-DD"
            class="w-full"
          />
        </el-form-item>
        <el-form-item label="Giờ bắt đầu" prop="startTime">
          <el-time-picker
            v-model="form.startTime"
            format="HH:mm"
            value-format="HH:mm"
            placeholder="Chọn giờ"
            class="w-full"
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">Hủy</el-button>
        <el-button type="primary" @click="submitForm">Lưu</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import useApi from "../../uses/fetchApi";

// State
const { data, fetchData } = useApi();
const showTimes = ref([]);
const movies = ref([]);
const rooms = ref([]);
const cinemas = ref([]);
const dialogVisible = ref(false);
const formRef = ref(null);
const currentPage = ref(1);
const pageSize = ref(10);
const totalItems = ref(0); // Đổi từ totalPage sang totalItems để khớp với số bản ghi

const form = ref({
  id: null,
  showDate: "",
  startTime: "",
  movieId: null,
  roomId: null,
  cinemaId: null,
});

// Validation rules
const validateStartTime = (rule, value, callback) => {
  if (!value) return callback(new Error("Vui lòng chọn giờ bắt đầu"));

  const selectedMovie = movies.value.find(
    (movie) => movie.id === form.value.movieId
  );
  if (!selectedMovie) return callback(new Error("Vui lòng chọn phim trước"));

  // Lấy ngày chiếu từ form
  const selectedDate = form.value.showDate;
  if (!selectedDate) return callback(new Error("Vui lòng chọn ngày chiếu"));

  // Lọc các suất chiếu cùng ngày, cùng phòng
  const roomShowTimes = showTimes.value.filter(
    (show) =>
      show.room.id === form.value.roomId && show.showDate === selectedDate
  );

  // Chuyển đổi thời gian startTime thành Date object với cùng ngày chiếu
  const inputTime = new Date(`${selectedDate}T${value}`);
  const inputEndTime = new Date(
    inputTime.getTime() + selectedMovie.duration * 60000 + 10 * 60000
  );

  // Kiểm tra trùng lịch chiếu
  const isConflict = roomShowTimes.some((show) => {
    const showStart = new Date(`${show.showDate}T${show.startTime}`);
    const showEnd = new Date(
      showStart.getTime() + show.movie.duration * 60000 + 10 * 60000
    );

    return (
      (inputTime >= showStart && inputTime < showEnd) ||
      (inputEndTime > showStart && inputEndTime <= showEnd) ||
      (inputTime <= showStart && inputEndTime >= showEnd)
    );
  });

  if (isConflict) {
    return callback(new Error("Giờ chiếu không còn trống trong phòng này"));
  }

  callback();
};

const validateRoom = (rule, value, callback) => {
  if (!value) {
    return callback(new Error("Vui lòng chọn phòng"));
  }
  const selectedRoom = rooms.value.find((room) => room.id === value);
  if (!selectedRoom || selectedRoom.cinemaId !== form.value.cinemaId) {
    return callback(new Error("Rạp không hợp lệ với phòng đã chọn"));
  }
  callback();
};
const rules = {
  showDate: [
    { required: true, message: "Vui lòng chọn ngày chiếu", trigger: "change" },
  ],
  startTime: [
    { required: true, message: "Vui lòng chọn giờ", trigger: "change" },
    // { validator: validateStartTime, trigger: "change" }, // Thêm custom validator
  ],
  movieId: [
    { required: true, message: "Vui lòng chọn phim", trigger: "change" },
  ],
  roomId: [
    { required: true, message: "Vui lòng chọn phòng", trigger: "change" },
    { validator: validateRoom, trigger: "change" },
  ],
  cinemaId: [
    { required: true, message: "Vui lòng chọn rạp", trigger: "change" },
  ],
};

// Methods
const fetchShowTimes = async (page = 1) => {
  try {
    // Gọi API với tham số phân trang
    await fetchData("get", "/ShowTime", null, {
      pageIndex: page,
      pageSize: pageSize.value,
    });

    // Gán dữ liệu từ API
    showTimes.value = data.value.items.$values || [];
    currentPage.value = data.value.pageIndex || page;
    totalItems.value = data.value.totalCount || 0; // Giả định API trả về totalItems
    console.log(data.value);
  } catch (error) {
    console.log(error);

    ElMessage.error("Lỗi khi tải dữ liệu");
  }
};

const fetchMovies = async () => {
  try {
    await fetchData("get", "/Movie/all-movie");
    movies.value = data.value.$values || [];
    console.log(movies.value);
  } catch (error) {
    console.log(error);
    ElMessage.error("Lỗi khi tải dữ liệu");
  }
};

const fetchCinemas = async () => {
  try {
    await fetchData("get", "/Cinema");
    cinemas.value = data.value.$values || [];
    console.log(cinemas.value);
  } catch (error) {
    console.log(error);
    ElMessage.error("Lỗi khi tải dữ liệu");
  }
};

const fetchRooms = async () => {
  try {
    await fetchData("get", "/Room");
    rooms.value = data.value.$values || [];
    console.log(rooms.value);
  } catch (error) {
    console.log(error);
    ElMessage.error("Lỗi khi tải dữ liệu");
  }
};

const filteredRooms = computed(() => {
  if (!form.value.cinemaId) {
    return []; // Nếu chưa chọn rạp, trả về mảng rỗng
  }
  return rooms.value.filter((room) => room.cinemaId === form.value.cinemaId);
});

const handleAdd = () => {
  form.value = {
    id: null,
    showDate: "",
    startTime: "",
    movieId: null,
    roomId: null,
    cinemaId: null,
  };
  dialogVisible.value = true;
};

const handleEdit = (row) => {
  form.value = { ...row };
  dialogVisible.value = true;
};

const handleDelete = (row) => {
  ElMessageBox.confirm("Bạn có chắc muốn xóa suất chiếu này?", "Xác nhận", {
    confirmButtonText: "Xóa",
    cancelButtonText: "Hủy",
    type: "warning",
  }).then(async () => {
    try {
      await fetchData("delete", `/ShowTime/${row.id}`);
      // Tải lại dữ liệu trang hiện tại sau khi xóa
      await fetchShowTimes(currentPage.value);
      ElMessage.success("Xóa thành công");
    } catch (error) {
      console.log(error);
      ElMessage.error("Xóa thất bại");
    }
  });
};

const submitForm = () => {
  formRef.value.validate(async (valid) => {
    if (valid) {
      try {
        const method = form.value.id ? "put" : "post";
        const url = form.value.id ? `/ShowTime/${form.value.id}` : "/ShowTime";

        console.log("form", form.value);

        // Tạo payload, loại bỏ id khi thêm mới
        const payload = {
          showDate: form.value.showDate,
          startTime: form.value.startTime,
          movieId: form.value.movieId,
          roomId: form.value.roomId,
          cinemaId: form.value.cinemaId,
        };
        if (form.value.id) {
          payload.id = form.value.id; // Chỉ thêm id khi sửa
        }

        await fetchData(method, url, payload);

        await fetchShowTimes(currentPage.value);
        dialogVisible.value = false;
        ElMessage.success("Lưu thành công");
      } catch (error) {
        console.error("Lỗi:", error.response.data); // Log lỗi để debug
        ElMessage.error("Lưu thất bại");
      }
    }
  });
};

const handlePageChange = (page) => {
  currentPage.value = page;
  fetchShowTimes(page); // Gọi API với trang mới
};

// Lifecycle
onMounted(() => {
  fetchShowTimes(); // Tải dữ liệu trang đầu tiên khi component được mount
  fetchMovies();
  fetchCinemas();
  fetchRooms();
});
</script>

<style>
:root {
  --el-color-primary: rgb(101, 105, 215);
}

.el-button--primary {
  --el-button-bg-color: rgb(101, 105, 215);
  --el-button-border-color: rgb(101, 105, 215);
  --el-button-hover-bg-color: rgb(121, 125, 235);
  --el-button-hover-border-color: rgb(121, 125, 235);
}
</style>
