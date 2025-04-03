<template>
  <div class="container-admin w-full">
    <h1 class="font-bold text-2xl mb-4 text-[var(--primary-color)]">
      Quản lí rạp chiếu phim
    </h1>

    <div class="flex justify-end">
      <el-button type="primary" class="mb-4" @click="handleAdd"
        >Thêm mới</el-button
      >
    </div>

    <el-table :data="cinemas" style="width: 100%" border>
      <el-table-column prop="name" label="Tên rạp" />
      <el-table-column label="Địa chỉ" width="550">
        <template #default="scope">
          <div class="line-clamp-2">
            {{ formatAddress(scope.row.address) }}
          </div>
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

    <el-dialog
      :title="form.id ? 'Sửa Rạp' : 'Thêm Rạp'"
      v-model="dialogVisible"
      width="30%"
    >
      <el-form :model="form" ref="formRef" :rules="rules" label-position="top">
        <el-form-item label="Tên rạp" prop="name">
          <el-input v-model="form.name" placeholder="Nhập tên rạp" />
        </el-form-item>
        <el-form-item label="Số nhà">
          <el-input
            v-model="form.address.houseNumber"
            placeholder="Nhập số nhà"
          />
        </el-form-item>
        <el-form-item label="Đường">
          <el-input
            v-model="form.address.street"
            placeholder="Nhập tên đường"
          />
        </el-form-item>
        <el-form-item label="Phường/Xã">
          <el-input v-model="form.address.ward" placeholder="Nhập phường/xã" />
        </el-form-item>
        <el-form-item label="Quận/Huyện">
          <el-input
            v-model="form.address.district"
            placeholder="Nhập quận/huyện"
          />
        </el-form-item>
        <el-form-item label="Thành phố">
          <el-input v-model="form.address.city" placeholder="Nhập thành phố" />
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
import { ref, onMounted } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import useApi from "../../uses/fetchApi";

const { data, fetchData } = useApi();
const cinemas = ref([]);
const dialogVisible = ref(false);
const formRef = ref(null);

const form = ref({
  id: null,
  name: "",
  address: { houseNumber: "", street: "", ward: "", district: "", city: "" },
});

const rules = {
  name: [{ required: true, message: "Vui lòng nhập tên rạp", trigger: "blur" }],
};

const fetchCinemas = async () => {
  try {
    await fetchData("get", "/cinema");
    cinemas.value = data.value.$values || [];
  } catch (error) {
    console.log(error);

    ElMessage.error("Lỗi khi tải dữ liệu rạp");
  }
};

const formatAddress = (address) => {
  if (!address) return "Không có địa chỉ";
  return `${address.houseNumber || ""}, ${address.street || ""}, ${
    address.ward || ""
  }, ${address.district || ""}, ${address.city || ""}`
    .replace(/, ,/g, ",")
    .trim();
};

const handleAdd = () => {
  form.value = {
    id: null,
    name: "",
    address: { houseNumber: "", street: "", ward: "", district: "", city: "" },
  };
  dialogVisible.value = true;
};

const handleEdit = (row) => {
  form.value = { ...row, address: row.address || {} };
  dialogVisible.value = true;
};

const handleDelete = (row) => {
  ElMessageBox.confirm("Bạn có chắc muốn xóa rạp này?", "Xác nhận", {
    confirmButtonText: "Xóa",
    cancelButtonText: "Hủy",
    type: "warning",
  }).then(async () => {
    try {
      await fetchData("delete", `/cinema/${row.id}`);
      await fetchCinemas();
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
        const url = form.value.id ? `/cinema/${form.value.id}` : "/cinema";
        form.value.id = 0;
        await fetchData(method, url, form.value);
        await fetchCinemas();
        dialogVisible.value = false;
        ElMessage.success("Lưu thành công");
      } catch (error) {
        console.log(error);

        ElMessage.error("Lưu thất bại");
      }
    }
  });
};

onMounted(() => {
  fetchCinemas();
});
</script>

<style>
:root {
  --el-color-primary: rgb(101, 105, 215);
}
</style>
