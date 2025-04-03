<!-- Movie.vue -->
<template>
  <div class="container-admin w-full">
    <h1 class="font-bold text-2xl mb-4 text-[var(--primary-color)]">
      Quản lí phim
    </h1>
    <!-- Add Button -->
    <div class="flex justify-end">
      <el-button type="primary" class="mb-4" @click="handleAdd"
        >Thêm mới</el-button
      >
    </div>
    <!-- Table -->
    <el-table :data="movies" style="width: 100%" border>
      <el-table-column prop="title" label="Tên phim" />
      <el-table-column label="Chi tiết" width="180">
        <template #default="scope">
          <div class="line-clamp-2">{{ scope.row.description }}</div>
        </template>
      </el-table-column>
      <el-table-column prop="genre" label="Thể loại" width="150" />
      <el-table-column prop="duration" label="Thời lượng (phút)" width="120">
        <template #default="scope"> {{ scope.row.duration }} Phút </template>
      </el-table-column>
      <el-table-column prop="releaseDate" label="Ngày phát hành" width="150" />
      <el-table-column label="Poster" width="100">
        <template #default="scope">
          <img
            :src="scope.row.poster"
            alt="Poster"
            class="w-20 h-20 object-cover"
            v-if="scope.row.poster"
          />
          <span v-else>Không có ảnh</span>
        </template>
      </el-table-column>
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
      :title="form.id ? 'Sửa Phim' : 'Thêm Phim'"
      v-model="dialogVisible"
      width="30%"
    >
      <el-form :model="form" ref="formRef" :rules="rules" label-position="top">
        <el-form-item label="Tên phim" prop="title">
          <el-input
            v-model="form.title"
            placeholder="Nhập tên phim"
            ref="titleInput"
          />
        </el-form-item>
        <el-form-item label="Mô tả" prop="description">
          <el-input
            v-model="form.description"
            type="textarea"
            placeholder="Nhập mô tả"
          />
        </el-form-item>
        <el-form-item label="Thể loại" prop="genre">
          <el-input v-model="form.genre" placeholder="Nhập thể loại" />
        </el-form-item>
        <el-form-item label="Thời lượng (phút)" prop="duration">
          <el-input-number
            v-model="form.duration"
            :min="1"
            placeholder="Nhập thời lượng"
            class="w-full"
          />
        </el-form-item>
        <el-form-item label="Ngày phát hành" prop="releaseDate">
          <el-date-picker
            v-model="form.releaseDate"
            type="date"
            placeholder="Chọn ngày"
            format="YYYY-MM-DD"
            value-format="YYYY-MM-DD"
            class="w-full"
          />
        </el-form-item>
        <el-form-item label="Poster URL" prop="poster">
          <el-input v-model="form.poster" placeholder="Nhập URL poster" />
        </el-form-item>
        <el-form-item label="Trailer URL" prop="trailer">
          <el-input v-model="form.trailer" placeholder="Nhập URL trailer" />
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
import { ref, onMounted, nextTick } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import useApi from "../../uses/fetchApi";

// State
const { data, fetchData } = useApi();
const movies = ref([]);
const dialogVisible = ref(false);
const formRef = ref(null);
const titleInput = ref(null); // Ref cho ô input title
const currentPage = ref(1);
const pageSize = ref(10);
const totalItems = ref(0);

const form = ref({
  id: null,
  title: "",
  description: "",
  duration: null,
  genre: "",
  poster: "",
  trailer: "",
  releaseDate: "",
});

// Validation rules
const rules = {
  title: [
    { required: true, message: "Vui lòng nhập tên phim", trigger: "blur" },
  ],
  description: [
    { required: true, message: "Vui lòng nhập mô tả", trigger: "blur" },
  ],
  genre: [
    { required: true, message: "Vui lòng nhập thể loại", trigger: "blur" },
  ],
  duration: [
    { required: true, message: "Vui lòng nhập thời lượng", trigger: "blur" },
    {
      type: "number",
      min: 1,
      message: "Thời lượng phải lớn hơn 0",
      trigger: "blur",
    },
  ],
  releaseDate: [
    {
      required: true,
      message: "Vui lòng chọn ngày phát hành",
      trigger: "change",
    },
  ],
  poster: [
    { required: false, message: "Vui lòng nhập URL poster", trigger: "blur" },
  ],
  trailer: [
    { required: false, message: "Vui lòng nhập URL trailer", trigger: "blur" },
  ],
};

// Methods
const fetchMovies = async (page = 1) => {
  try {
    await fetchData("get", "/Movie", null, {
      pageIndex: page,
      pageSize: pageSize.value,
    });

    movies.value = data.value.items.$values || [];

    currentPage.value = data.value.pageIndex || page;
    totalItems.value = data.value.totalCount || 0;
  } catch (error) {
    console.log(error);
    ElMessage.error("Lỗi khi tải dữ liệu phim");
  }
};

const handleAdd = () => {
  form.value = {
    id: null,
    title: "",
    description: "",
    duration: null,
    genre: "",
    poster: "",
    trailer: "",
    releaseDate: "",
  };
  dialogVisible.value = true;

  // Focus vào ô title sau khi dialog mở
  nextTick(() => {
    if (titleInput.value) {
      titleInput.value.focus();
    }
  });
};

const handleEdit = (row) => {
  form.value = { ...row };
  dialogVisible.value = true;

  // Focus vào ô title sau khi dialog mở
  nextTick(() => {
    if (titleInput.value) {
      titleInput.value.focus();
    }
  });
};

const handleDelete = (row) => {
  ElMessageBox.confirm("Bạn có chắc muốn xóa phim này?", "Xác nhận", {
    confirmButtonText: "Xóa",
    cancelButtonText: "Hủy",
    type: "warning",
  }).then(async () => {
    try {
      await fetchData("delete", `/Movie/${row.id}`);
      await fetchMovies(currentPage.value);
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
        const url = form.value.id ? `/Movie/${form.value.id}` : "/Movie";

        const payload = {
          title: form.value.title,
          description: form.value.description,
          duration: form.value.duration,
          genre: form.value.genre,
          poster: form.value.poster || null,
          trailer: form.value.trailer || null,
          releaseDate: form.value.releaseDate,
        };
        if (form.value.id) {
          payload.id = form.value.id;
        }

        await fetchData(method, url, payload);

        await fetchMovies(currentPage.value);
        dialogVisible.value = false;
        ElMessage.success("Lưu thành công");
      } catch (error) {
        console.error("Lỗi:", error.response?.data);
        ElMessage.error("Lưu thất bại");
      }
    }
  });
};

const handlePageChange = (page) => {
  currentPage.value = page;
  fetchMovies(page);
};

// Lifecycle
onMounted(() => {
  fetchMovies();
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
