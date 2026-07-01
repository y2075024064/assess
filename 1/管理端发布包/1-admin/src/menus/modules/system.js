import {
  Avatar,
  User,
  Ticket,
  Help,
  Wallet,
  Handbag,
  Notebook,
  Star,
  School,
} from "@element-plus/icons-vue";

const childrens = [
  {
    name: "dictionary",
    title: "字典管理",
    icon: Notebook,
    component: () => import("@/views/Dicts/Page.vue"),
  },
  {
    name: "admin",
    title: "管理员管理",
    icon: Avatar,
    component: () => import("@/views/Admin/Page.vue"),
  },
  {
    name: "student",
    title: "学生管理",
    icon: User,
    component: () => import("@/views/Student/Page.vue"),
  },
  {
    name: "grade",
    title: "年级管理",
    icon: Ticket,
    component: () => import("@/views/Grade/Page.vue"),
  },
  {
    name: "school",
    title: "学校管理",
    icon: School,
    component: () => import("@/views/School/Page.vue"),
  },
  {
    name: "adModule",
    title: "广告模块",
    icon: Wallet,
    component: () => import("@/views/AdModule/Page.vue"),
  },
  {
    name: "advertising",
    title: "广告管理",
    icon: Handbag,
    component: () => import("@/views/Advertising/Page.vue"),
  },
  {
    name: "dynamic",
    title: "动态管理",
    icon: Help,
    component: () => import("@/views/Dynamic/Page.vue"),
  },
  {
    name: "thinkStar",
    title: "思维之星",
    icon: Star,
    component: () => import("@/views/ThinkStar/Page.vue"),
  },
];

export default childrens;
