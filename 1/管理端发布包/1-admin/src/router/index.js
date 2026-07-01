import { createRouter, createWebHistory } from "vue-router";
import Layout from "@/layout";

const routes = [
  {
    path: "/turnLogin",
    name: "login",
    component: () => import("@/views/Login.vue"),
  },
  {
    path: "/adminRegist",
    name: "adminRegist",
    component: () => import("@/views/AdminRegister.vue"),
  },
  {
    path: "/",
    name: "home",
    component: Layout,
  },
];

// 定义创建路由实例方法
const this_createRouter = () =>
  createRouter({
    // 采用 Web 模式
    history: createWebHistory(),
    // 路由配置
    routes,
  });

// 创建路由实例
const router = this_createRouter();

// 导出重置路由方法
export function resetRouter() {
  router.matcher = this_createRouter().matcher;
}

// 导出 router
export default router;
