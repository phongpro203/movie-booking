<template>
  <div class="container-admin w-full mx-auto p-6">
    <el-row :gutter="20">
      <el-col :span="24">
        <h1 class="font-bold text-2xl mb-4 text-[var(--primary-color)]">
          Xin chào
        </h1>
      </el-col>

      <!-- Filter Section -->
      <el-col :span="24">
        <el-card shadow="hover" class="mb-6 rounded-lg">
          <el-row :gutter="20" align="middle">
            <el-col :span="12">
              <el-date-picker
                v-model="dateRange"
                type="daterange"
                range-separator="Đến"
                start-placeholder="Từ ngày"
                end-placeholder="Đến ngày"
                format="YYYY-MM-DD"
                class="w-full"
                @change="fetchStatistics"
              />
            </el-col>
          </el-row>
        </el-card>
      </el-col>

      <!-- Statistics Cards -->
      <el-col
        v-for="(stat, key) in statsDisplay"
        :key="key"
        :span="6"
        class="mb-6"
      >
        <el-card shadow="hover" class="rounded-lg">
          <div class="p-4">
            <h3 class="text-gray-600 font-semibold">{{ stat.label }}</h3>
            <p :class="stat.class">{{ stat.value }}</p>
          </div>
        </el-card>
      </el-col>

      <!-- Charts -->
      <!--Doanh thu-->
      <el-col :span="24" class="mb-6">
        <el-card shadow="hover" class="rounded-lg">
          <h3 class="text-lg font-semibold mb-4">Doanh thu</h3>
          <div class="chart-container">
            <BarChart
              v-if="!loading"
              :chart-data="revenueChartData"
              :options="chartOptions"
            />
            <el-skeleton v-else :rows="5" animated />
          </div>
        </el-card>
      </el-col>
      <!--Vé bán-->
      <el-col :span="12" class="mb-6">
        <el-card shadow="hover" class="rounded-lg">
          <h3 class="text-lg font-semibold mb-4">Vé bán</h3>
          <div class="chart-container">
            <BarChart
              v-if="!loading"
              :chart-data="ticketsChartData"
              :options="chartOptions"
            />
            <el-skeleton v-else :rows="5" animated />
          </div>
        </el-card>
      </el-col>
      <!--Đồ ăn-->
      <el-col :span="12" class="mb-6">
        <el-card shadow="hover" class="rounded-lg">
          <h3 class="text-lg font-semibold mb-4">Đồ ăn bán được</h3>
          <div class="chart-container">
            <BarChart
              v-if="!loading"
              :chart-data="foodsChartData"
              :options="chartOptions"
            />
            <el-skeleton v-else :rows="5" animated />
          </div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { BarChart } from "vue-chart-3";
import { Chart, registerables } from "chart.js";
import { ElRow, ElCol, ElCard, ElDatePicker, ElSkeleton } from "element-plus";
import useApi from "../../uses/fetchApi";

Chart.register(...registerables);

// Lấy ngày hôm nay và 7 ngày trước
const today = new Date();
const lastWeek = new Date();
lastWeek.setDate(today.getDate() - 7);

// Trạng thái reactive
const dateRange = ref([lastWeek, today]); // Mặc định chọn 1 tuần trước đến hôm nay
const statistics = ref({
  stats: { totalRevenue: 0, totalTickets: 0, totalBookings: 0 },
  data: [],
});
const loading = ref(false);

// Cấu hình biểu đồ
const chartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  scales: {
    y: { beginAtZero: true },
  },
};

const { data, fetchData } = useApi();

const formatDate = (date) => {
  if (!date) return null;
  return `${date.getFullYear()}-${(date.getMonth() + 1)
    .toString()
    .padStart(2, "0")}-${date.getDate().toString().padStart(2, "0")}`;
};

// Hàm fetch dữ liệu thống kê
const fetchStatistics = async () => {
  loading.value = true;
  try {
    const startDate = formatDate(dateRange.value[0]);
    const endDate = formatDate(dateRange.value[1]);

    console.log("Fetching statistics from:", startDate, "to", endDate);

    await fetchData("get", "/booking/statics", null, {
      StartDate: startDate,
      EndDate: endDate,
    });

    statistics.value = data.value || { stats: {}, data: [] };
    console.log(statistics.value);
  } catch (error) {
    console.error("Error fetching statistics:", error);
  } finally {
    loading.value = false;
  }
};

// Computed properties
const statsDisplay = computed(() => [
  {
    label: "Tổng doanh thu",
    value: formatCurrency(statistics.value.stats.totalRevenue),
    class: "text-2xl font-bold text-[var(--primary-color)]",
  },
  {
    label: "Vé đã bán",
    value: statistics.value.stats.totalTickets,
    class: "text-2xl font-bold text-[var(--primary-color)]",
  },
  {
    label: "Đồ ăn đã bán",
    value: statistics.value.stats.totalFoods,
    class: "text-2xl font-bold text-[var(--primary-color)]",
  },
  {
    label: "Số lượng đặt ",
    value: statistics.value.stats.totalBookings,
    class: "text-2xl font-bold text-[var(--primary-color)]",
  },
]);

const revenueChartData = computed(() => ({
  labels: statistics.value.data?.$values?.map((item) => item.date),
  datasets: [
    {
      label: "Doanh thu (VND)",
      data: statistics.value.data?.$values?.map((item) => item.revenue),
      backgroundColor: "rgba(34, 197, 94, 0.5)",
      borderColor: "rgba(34, 197, 94, 1)",
      borderWidth: 1,
    },
  ],
}));

const ticketsChartData = computed(() => ({
  labels: statistics.value.data?.$values?.map((item) => item.date),
  datasets: [
    {
      label: "Vé bán",
      data: statistics.value.data?.$values?.map((item) => item.tickets),
      backgroundColor: "rgba(59, 130, 246, 0.5)",
      borderColor: "rgba(59, 130, 246, 1)",
      borderWidth: 1,
    },
  ],
}));

const foodsChartData = computed(() => ({
  labels: statistics.value.data?.$values?.map((item) => item.date),
  datasets: [
    {
      label: "Số lượng",
      data: statistics.value.data?.$values?.map((item) => item.foods),
      backgroundColor: "rgba(59, 130, 246, 0.5)",
      borderColor: "rgba(59, 130, 246, 1)",
      borderWidth: 1,
    },
  ],
}));
// Hàm định dạng tiền tệ
const formatCurrency = (value) => {
  return new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
  }).format(value);
};

// Lifecycle hooks
onMounted(() => {
  fetchStatistics();
});
</script>

<style scoped>
.chart-container {
  position: relative;
  min-height: 300px;
}
.el-card {
  border-radius: 10px;
}
</style>
