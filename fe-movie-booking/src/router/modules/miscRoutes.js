export default [
  {
    path: "/payment",
    name: "payment",
    component: () => import("../../views/User/PaymentSucess.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/:pathMatch(.*)*",
    name: "NotFound",
    component: () => import("../../components/404NotFound.vue"),
  },
];
