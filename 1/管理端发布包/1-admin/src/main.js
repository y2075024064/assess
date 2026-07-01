import { createApp } from "vue";

import ElementPlus from "element-plus";
import "element-plus/dist/index.css";

import App from "./App.vue";
import router from "./router";
import store from "./store";

import axios from "./utils/request";
import VueAxios from "vue-axios";
import MyMenu from "@/components/myMenu";
import MyPager from "@/components/myPager";
import MyDialog from "@/components/myDialog";
import MySelect from "@/components/mySelect";
import SearchBox from "@/components/searchBox";

import { Boot } from "@wangeditor/editor";
import formulaModule from "@wangeditor/plugin-formula";

// 开发模式启用模拟后端（注释掉：使用真实后端API）
// if (process.env.NODE_ENV === "development") {
//   import("@/utils/mock").then(function(m) { m.setupMock(axios); });
// }

var app = createApp(App);

app.use(store);
app.use(router);
app.use(ElementPlus);
app.use(VueAxios, axios);

Boot.registerModule(formulaModule);

app.component("MyMenu", MyMenu);
app.component("MyPager", MyPager);
app.component("MyDialog", MyDialog);
app.component("MySelect", MySelect);
app.component("SearchBox", SearchBox);

app.mount("#app");
