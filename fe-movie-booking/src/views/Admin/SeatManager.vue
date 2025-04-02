<!-- Seat.vue -->
<template>
  <div class="container-admin w-full">
    <h1 class="font-bold text-2xl mb-4 text-[var(--primary-color)]">
      Quản lí ghế
    </h1>
    <!-- Add Button -->
    <div class="flex justify-end">
      <el-button type="primary" class="mb-4" @click="handleAdd"
        >Thêm mới</el-button
      >
    </div>
    <div class="w-[25%] mb-4">
      <el-select
        v-model="roomSelected"
        placeholder="Chọn phòng"
        @change="selectRoom"
      >
        <el-option
          v-for="room in rooms"
          :key="room.id"
          :label="room.name"
          :value="room.id"
        />
      </el-select>
    </div>
    <!-- Table -->
    <el-table
      :data="seats"
      style="width: 99%; max-height: 70vh; overflow-y: auto"
      border
    >
      <el-table-column prop="seatNumber" label="Số ghế" />
      <el-table-column prop="seatType.name" label="Loại ghế" width="250" />
      <el-table-column prop="room.name" label="Phòng" width="250" />
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
      :title="form.id ? 'Sửa Ghế' : 'Thêm Ghế'"
      v-model="dialogVisible"
      width="40%"
    >
      <el-form :model="form" ref="formRef" :rules="rules" label-position="top">
        <el-form-item label="Phòng" prop="roomId">
          <el-select
            v-model="form.roomId"
            placeholder="Chọn phòng"
            class="w-full"
            @change="resetSeatRange"
          >
            <el-option
              v-for="room in rooms"
              :key="room.id"
              :label="room.name"
              :value="room.id"
            />
          </el-select>
        </el-form-item>
        <el-form-item label="Loại ghế" prop="seatTypeId">
          <el-select
            v-model="form.seatTypeId"
            placeholder="Chọn loại ghế"
            class="w-full"
          >
            <el-option
              v-for="seatType in seatTypes"
              :key="seatType.id"
              :label="seatType.name"
              :value="seatType.id"
            />
          </el-select>
        </el-form-item>

        <!-- Nếu là sửa ghế, chỉ hiển thị ô nhập số ghế -->
        <el-form-item v-if="form.id" label="Số ghế" prop="seatNumber">
          <el-input
            v-model="form.seatNumber"
            placeholder="Nhập số ghế (ví dụ: A1)"
          />
        </el-form-item>

        <!-- Nếu là thêm ghế, hiển thị ô nhập dải ghế -->
        <el-form-item v-else label="Dải ghế" prop="seatRange">
          <div class="flex space-x-4">
            <el-input
              v-model="form.seatRange.start"
              placeholder="Bắt đầu (ví dụ: A1)"
              class="w-1/2"
            />
            <el-input
              v-model="form.seatRange.end"
              placeholder="Kết thúc (ví dụ: A10)"
              class="w-1/2"
            />
          </div>
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
const seats = ref([]);
const rooms = ref([]);
const roomSelected = ref(null);
const seatTypes = ref([
  { id: 1, name: "2D vip" },
  { id: 2, name: "2D thường" },
  { id: 3, name: "3D thường" },
  { id: 4, name: "3D vip" },
]);
const dialogVisible = ref(false);
const formRef = ref(null);

const form = ref({
  id: null,
  roomId: null,
  seatNumber: "",
  seatTypeId: null,
  seatRange: {
    start: "",
    end: "",
  },
});

// Validation rules
const rules = {
  roomId: [
    { required: true, message: "Vui lòng chọn phòng", trigger: "change" },
  ],
  seatTypeId: [
    { required: true, message: "Vui lòng chọn loại ghế", trigger: "change" },
  ],
  seatNumber: [
    { required: true, message: "Vui lòng nhập số ghế", trigger: "blur" },
  ],
  seatRange: [
    {
      validator: (rule, value, callback) => {
        if (!form.value.id) {
          // Chỉ validate khi thêm ghế
          if (!value.start || !value.end) {
            callback(new Error("Vui lòng nhập dải ghế"));
          } else if (
            !/^[A-Z]\d+$/.test(value.start) ||
            !/^[A-Z]\d+$/.test(value.end)
          ) {
            callback(new Error("Số ghế phải có định dạng như A1, B10, v.v."));
          } else {
            callback();
          }
        } else {
          callback();
        }
      },
      trigger: "blur",
    },
  ],
};

// Event handlers
const selectRoom = () => {
  fetchSeats(roomSelected.value);
};

// Methods
const fetchSeats = async (roomId) => {
  try {
    await fetchData("get", "/Seat", null, {
      roomId: roomId,
    });
    console.log("Dữ liệu ghế từ API:", data.value.$values);
    seats.value = data.value.$values.map((seat) => {
      // Tìm room tương ứng với seat.roomId
      const matchedRoom = rooms.value.find((room) => room.id === seat.roomId);
      const seatType = seatTypes.value.find((st) => st.id === seat.seatTypeId);
      return {
        ...seat,
        room: matchedRoom || {},
        seatType: seatType || {},
      };
    });
  } catch (error) {
    console.log(error);
    ElMessage.error("Lỗi khi tải dữ liệu ghế");
  }
};

const fetchRooms = async () => {
  try {
    await fetchData("get", "/Room");
    console.log("Dữ liệu phòng từ API:", data.value.$values);
    rooms.value = (data.value.$values || []).map((room) => ({
      id: room.id || room.Id,
      name: room.name || room.Name,
    }));
  } catch (error) {
    console.log(error);
    ElMessage.error("Lỗi khi tải dữ liệu phòng");
  }
};

// Reset dải ghế khi thay đổi phòng
const resetSeatRange = () => {
  form.value.seatRange.start = "";
  form.value.seatRange.end = "";
};

// Sinh danh sách ghế từ dải ghế (ví dụ: A1 -> A10)
const generateSeats = (start, end) => {
  const seats = [];
  const startRow = start.charAt(0); // Hàng (A, B, C, ...)
  const startNum = parseInt(start.slice(1)); // Số ghế (1, 2, 3, ...)
  const endRow = end.charAt(0);
  const endNum = parseInt(end.slice(1));

  if (startRow !== endRow) {
    ElMessage.error(
      "Hàng ghế bắt đầu và kết thúc phải giống nhau (ví dụ: A1 -> A10)"
    );
    return [];
  }

  for (let i = startNum; i <= endNum; i++) {
    seats.push(`${startRow}${i}`);
  }

  return seats;
};

const handleAdd = () => {
  form.value = {
    id: null,
    roomId: null,
    seatNumber: "",
    seatTypeId: null,
    seatRange: {
      start: "",
      end: "",
    },
  };
  dialogVisible.value = true;

  nextTick(() => {
    if (formRef.value) {
      formRef.value.resetFields();
    }
  });
};

const handleEdit = (row) => {
  console.log("Dữ liệu row:", row);
  form.value = {
    id: row.id,
    roomId: row.roomId,
    seatNumber: row.seatNumber,
    seatTypeId: row.seatTypeId,
    seatRange: {
      start: "",
      end: "",
    },
  };
  dialogVisible.value = true;
};

const handleDelete = (row) => {
  ElMessageBox.confirm("Bạn có chắc muốn xóa ghế này?", "Xác nhận", {
    confirmButtonText: "Xóa",
    cancelButtonText: "Hủy",
    type: "warning",
  }).then(async () => {
    try {
      await fetchData("delete", `/Seat/${row.id}`);
      await fetchSeats();
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
        if (form.value.id) {
          // Sửa ghế
          const method = "put";
          const url = `/Seat/${form.value.id}`;
          const payload = {
            roomId: form.value.roomId,
            seatNumber: form.value.seatNumber,
            seatTypeId: form.value.seatTypeId,
          };

          await fetchData(method, url, payload);
          ElMessage.success("Sửa thành công");
        } else {
          // Thêm nhiều ghế
          const seatNumbers = generateSeats(
            form.value.seatRange.start,
            form.value.seatRange.end
          );
          if (seatNumbers.length === 0) return;
          const payload = seatNumbers.map((seatNumber) => ({
            roomId: form.value.roomId,
            seatNumber: seatNumber,
            seatTypeId: form.value.seatTypeId,
          }));
          console.log("Dữ liệu payload:", payload);
          await fetchData("post", "/Seat", payload);
          roomSelected.value = payload[0].roomId;
          console.log("Phòng được chọn:", roomSelected.value);

          fetchSeats(roomSelected.value);
          ElMessage.success(`Thêm ${seatNumbers.length} ghế thành công`);
        }

        await fetchSeats();
        dialogVisible.value = false;
      } catch (error) {
        console.error("Lỗi:", error.response?.data);
        ElMessage.error("Lưu thất bại");
      }
    }
  });
};

// Lifecycle
onMounted(async () => {
  await fetchRooms();
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
