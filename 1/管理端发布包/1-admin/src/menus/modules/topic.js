import { PriceTag, Memo, Document, Tickets } from "@element-plus/icons-vue";

const childrens = [
  {
    name: "topicLabel",
    title: "题目标签",
    icon: PriceTag,
    component: () => import("@/views/TopicLabel/Page.vue"),
  },
  {
    name: "topicPart",
    title: "科目专项",
    icon: Memo,
    component: () => import("@/views/TopicPart/Page.vue"),
  },
  {
    name: "topicInfo",
    title: "题目管理",
    icon: Document,
    component: () => import("@/views/Topic/Page.vue"),
  },
  {
    name: "examInfo",
    title: "考试管理",
    icon: Tickets,
    component: () => import("@/views/Exam/Page.vue"),
  },
];

export default childrens;
