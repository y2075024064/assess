import axios from "axios";

import { WarnTriangleFilled } from "@element-plus/icons-vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { getCookies } from "@/utils/auth";
import { markRaw } from "vue";
import router from "@/router";
import store from "@/store";

var alertShowed = false;
var messageShowed = false;

// 创建 axios 实例
const service = axios.create({
  // url = base url + request url
  baseURL: process.env.VUE_APP_HOST, 
  // baseURL:'/admin',
  timeout: 300000,
});

// 添加请求拦截器（先注册）
service.interceptors.request.use(
  (config) => {
    // 在发送请求之前添加 Header 参数
    config.headers["X-ClientType"] = store.getters.clientType;
    // 添加 登录凭证 参数
    var token = getCookies();
    if (token) {
      config.headers["X-Token"] = token;
    }

    return config;
  },
  (error) => {
    // 对请求错误 打印日志
    console.log("req err:", error.message);
    return Promise.reject(error);
  }
);

// 添加响应拦截器
service.interceptors.response.use(
  (response) => {
    // 解析并处理响应数据
    const res = response.data;

    if (process.env.NODE_ENV === "development") {
      console.log(res);
    }

    if (ArrayBuffer.isView(res)) {
      return res;
    }

    switch (res.Code) {
      // 请求失败
      case 0:
        break;
      // 请求成功
      case 200:
        break;
      // 未登录
      case 401: {
        router.push("turnLogin");
        break;
      }
      // 没有权限
      case 402: {
        if (messageShowed) {
          break;
        }

        messageShowed = true;
        ElMessage({
          message: "您没有相应的访问权限！",
          duration: 1000,
          type: "warning",
          onClose: () => {
            messageShowed = false;
          },
        });
        break;
      }
      // 拒绝请求
      case 406: {
        if (messageShowed) {
          break;
        }

        messageShowed = true;
        ElMessage({
          message: "您的请求被拒绝！",
          duration: 1000,
          type: "error",
          onClose: () => {
            messageShowed = false;
          },
        });
        break;
      }
      // 参数解析失败
      case 412: {
        console.log("参数解析失败，" + res.msg);
        if (alertShowed) {
          break;
        }

        alertShowed = true;
        ElMessageBox.alert("参数解析失败！", "错误", {
          icon: markRaw(WarnTriangleFilled),
          type: "error",
          callback: () => {
            alertShowed = false;
          },
        });
        break;
      }
      // 服务器异常
      case 500: {
        console.log("服务器异常，" + res.msg);
        if (alertShowed) {
          break;
        }

        alertShowed = true;
        ElMessageBox.alert("服务器异常！", "错误", {
          icon: markRaw(WarnTriangleFilled),
          type: "error",
          callback: () => {
            alertShowed = false;
          },
        });
        break;
      }
    }

    return res;
  },
  (error) => {
    // 对响应错误 打印日志
    console.log("res err:", error.message);
    return Promise.reject(error);
  }
);

export default service;
