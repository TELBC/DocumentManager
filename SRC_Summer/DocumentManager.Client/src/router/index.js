import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import LoginForm from "../components/LoginForm.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/login",
      name: "login",
      component: LoginForm,
    },
    {
      path: "/",
      name: "home",
      component: HomeView,
      meta: { requiresAuth: true },
    },
    {
      path: "/about",
      name: "about",
      component: () => import("../views/AboutView.vue"),
      meta: { requiresAuth: true },
    },
    {
      path: "/folder/:id",
      name: "folder",
      component: () => import("../views/FolderView.vue"),
      meta: { requiresAuth: true },
    },
    {
      path: "/:catchAll(.*)",
      redirect: { name: "home" },
    },
  ],
});

router.beforeEach((to, from, next) => {
  const isLoggedIn = localStorage.getItem("jwtToken");
  if (to.name === "login") {
    if (isLoggedIn) {
      next({ name: "home" });
    } else {
      next();
    }
  } else {
    if (to.matched.some((route) => route.meta.requiresAuth) && !isLoggedIn) {
      next({ name: "login" });
    } else {
      next();
    }
  }
});

export default router;
