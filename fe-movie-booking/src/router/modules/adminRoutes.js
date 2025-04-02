import AdminLayout from "../../views/AdminLayout.vue";
export default [
  {
    path: "/admin",
    name: "Admin",
    component: AdminLayout,
    redirect: "/admin/dashboard",
    children: [
      {
        path: "/admin/dashboard",
        name: "dashboard",
        component: () => import("../../views/Admin/DashboardView.vue"),
        meta: { requiresAuth: true, role: "admin" },
      },
      {
        path: "/admin/movie-manager",
        name: "movieM",
        component: () => import("../../views/Admin/MovieManager.vue"),
        meta: { requiresAuth: true, role: "admin" },
      },
      {
        path: "/admin/showtime-manager",
        name: "showtimeM",
        component: () => import("../../views/Admin/ShowtimeManager.vue"),
        meta: { requiresAuth: true, role: "admin" },
      },
      {
        path: "/admin/seat-manager",
        name: "seatM",
        component: () => import("../../views/Admin/SeatManager.vue"),
        meta: { requiresAuth: true, role: "admin" },
      },
      {
        path: "/admin/room-manager",
        name: "roomM",
        component: () => import("../../views/Admin/RoomManager.vue"),
        meta: { requiresAuth: true, role: "admin" },
      },
      {
        path: "/admin/cinema-manager",
        name: "cinemaM",
        component: () => import("../../views/Admin/CinemManager.vue"),
        meta: { requiresAuth: true, role: "admin" },
      },
      {
        path: "/admin/food-manager",
        name: "foodM",
        component: () => import("../../views/Admin/FoodManager.vue"),
        meta: { requiresAuth: true, role: "admin" },
      },
    ],
  },
];
