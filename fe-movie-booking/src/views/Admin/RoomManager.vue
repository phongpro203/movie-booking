<!-- Room.vue -->
<template>
  <div class="container-admin flex-1">
    <h1 class="font-bold text-2xl mb-4 text-[var(--primary-color)]">
      Quản lí phòng chiếu
    </h1>
    <!-- Add Button -->
    <div class="flex justify-end">
      <el-button type="primary" class="mb-4" @click="handleAdd"
        >Thêm mới</el-button
      >
    </div>
    <!-- Table -->
    <el-table :data="rooms" style="width: 100%" border>
      <el-table-column prop="name" label="Tên phòng" />
      <el-table-column prop="seatCount" label="Số ghế" width="120" />
      <el-table-column prop="roomType" label="Loại phòng" width="150" />
      <el-table-column prop="cinema.name" label="Rạp" width="250" />
      <el-table-column label="Hành động" width="200">
        <template #default="scope">
          <el-button size="small" @click="handleEdit(scope.row)">Sửa</el-button>
          <el-button size="small" type="danger" @click="handleDelete(scope.row)"
            >Xóa</el-button
          >
        </template>
      </el-table-column>
    </el-table>

    <!-- Add/Edit Dialog -->
    <el-dialog
      :title="form.id ? 'Sửa Phòng Chiếu' : 'Thêm Phòng Chiếu'"
      v-model="dialogVisible"
      width="30%"
    >
      <el-form :model="form" ref="formRef" :rules="rules" label-position="top">
        <el-form-item label="Tên phòng" prop="name">
          <el-input
            v-model="form.name"
            placeholder="Nhập tên phòng"
            ref="nameInput"
          />
        </el-form-item>
        <el-form-item label="Số ghế" prop="seatCount">
          <el-input-number
            v-model="form.seatCount"
            :min="1"
            placeholder="Nhập số ghế"
            class="w-full"
          />
        </el-form-item>
        <el-form-item label="Loại phòng" prop="roomType">
          <el-select
            v-model="form.roomType"
            placeholder="Chọn loại phòng"
            class="w-full"
          >
            <el-option label="2D" value="2D" />
            <el-option label="3D" value="3D" />
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
const rooms = ref([]);
const cinemas = ref([]);
const dialogVisible = ref(false);
const formRef = ref(null);
const nameInput = ref(null);

const form = ref({
  id: null,
  name: "",
  seatCount: null,
  roomType: "",
  cinemaId: null,
});

// Validation rules
const rules = {
  name: [
    { required: true, message: "Vui lòng nhập tên phòng", trigger: "blur" },
  ],
  seatCount: [
    { required: true, message: "Vui lòng nhập số ghế", trigger: "blur" },
    {
      type: "number",
      min: 1,
      message: "Số ghế phải lớn hơn 0",
      trigger: "blur",
    },
  ],
  roomType: [
    { required: true, message: "Vui lòng chọn loại phòng", trigger: "change" },
  ],
  cinemaId: [
    { required: true, message: "Vui lòng chọn rạp", trigger: "change" },
  ],
};

// Methods
const fetchRooms = async () => {
  try {
    await fetchData("get", "/room", null, {});

    console.log("Dữ liệu phòng từ API:", data.value.$values); // Log dữ liệu từ API

    rooms.value = data.value.$values;
  } catch (error) {
    console.log(error);
    ElMessage.error("Lỗi khi tải dữ liệu phòng");
  }
};

const fetchCinemas = async () => {
  try {
    await fetchData("get", "/cinema");
    console.log("Dữ liệu rạp từ API:", data.value.$values); // Log dữ liệu từ API
    cinemas.value = data.value.$values || [];
    rooms.value.forEach((room) => {
      const cinema = cinemas.value.find((c) => c.id === room.cinemaId);
      if (cinema) {
        room.cinema = cinema;
      }
    });
    console.log(rooms.value);
  } catch (error) {
    console.log(error);
    ElMessage.error("Lỗi khi tải dữ liệu rạp");
  }
};

const handleAdd = () => {
  form.value = {
    id: null,
    name: "",
    seatCount: null,
    roomType: "",
    cinemaId: null,
  };
  dialogVisible.value = true;

  nextTick(() => {
    if (nameInput.value) {
      nameInput.value.focus();
    }
  });
};

const handleEdit = (row) => {
  console.log("Dữ liệu row:", row); // Log dữ liệu của row
  form.value = { ...row };
  console.log("form.id sau khi gán:", form.value.id); // Log form.id
  dialogVisible.value = true;

  nextTick(() => {
    if (nameInput.value) {
      nameInput.value.focus();
    }
  });
};

const handleDelete = (row) => {
  ElMessageBox.confirm("Bạn có chắc muốn xóa phòng chiếu này?", "Xác nhận", {
    confirmButtonText: "Xóa",
    cancelButtonText: "Hủy",
    type: "warning",
  }).then(async () => {
    try {
      await fetchData("delete", `/Room/${row.id}`);
      await fetchRooms();
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
        const url = form.value.id ? `/Room/${form.value.id}` : "/Room";

        const payload = {
          name: form.value.name,
          seatCount: form.value.seatCount,
          roomType: form.value.roomType,
          cinemaId: form.value.cinemaId,
        };
        if (form.value.id) {
          payload.id = form.value.id;
        }

        await fetchData(method, url, payload);

        await fetchRooms();
        dialogVisible.value = false;
        ElMessage.success("Lưu thành công");
      } catch (error) {
        console.error("Lỗi:", error.response?.data);
        ElMessage.error("Lưu thất bại");
      }
    }
  });
};

// Lifecycle
onMounted(async () => {
  fetchRooms();
  await fetchCinemas();
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
