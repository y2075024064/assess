import { markRaw } from "vue";
import { Setting, List } from "@element-plus/icons-vue";

import topicItems from "./modules/topic";
import systemItems from "./modules/system";

const menus = [
  {
    name: "system",
    title: "系统管理",
    icon: Setting,
    component: null,
    children: systemItems,
  },
  {
    name: "topic",
    title: "题目管理",
    icon: List,
    component: null,
    children: topicItems,
  },
];

/**
 * 初始化菜单内容
 * @param { Array } arr 菜单数组
 */
function initMenus(arr) {
  if (arr && arr.length) {
    arr.forEach((item) => {
      if (item.icon) {
        item.icon = markRaw(item.icon);
      }

      if (item.component && typeof item.component === "object") {
        item.component = markRaw(item.component);
      }

      initMenus(item.children);
    });
  }
}

export function getMenus() {
  initMenus(menus);
  return menus;
}
