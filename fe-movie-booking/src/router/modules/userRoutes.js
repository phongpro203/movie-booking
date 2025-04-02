export default [
  {
    path: "/",
    redirect: "/home",
    component: () => import("../../views/UserLayout.vue"),
    children: [
      {
        path: "/home",
        name: "Home",
        component: () => import("../../views/User/HomeView.vue"),
      },
      {
        path: "/test",
        name: "Test",
        component: () => import("../../views/User/TestView.vue"),
      },
      {
        path: "/MovieDetail/:id",
        name: "MovieDetail",
        component: () => import("../../views/User/MovieDetail.vue"),
      },
      {
        path: "/Booking",
        name: "Booking",
        component: () => import("../../views/User/BookingView.vue"),
        meta: { requiresAuth: true },
      },
      {
        path: "/Movies",
        name: "Movies",
        component: () => import("../../views/User/MovieView.vue"),
      },
      {
        path: "/Showtimes",
        name: "Showtimes",
        component: () => import("../../views/User/ShowtimeCinema.vue"),
      },
      {
        path: "/UserInfo",
        name: "UserInfo",
        component: () => import("../../views/User/UserInfo.vue"),
        meta: { requiresAuth: true },
      },
    ],
  },
];
