import { createStore } from "vuex";

import { getCookies } from "@/utils/auth";
import user from "./modules/user";
import app from "./modules/app";

export default createStore({
  getters: {
    remember: (state) => state.user.remember,
    userId: (state) => state.user.userId,
    avatar: (state) => state.user.avatar,
    gender: (state) => state.user.gender,
    name: (state) => state.user.name,
    type: (state) => state.user.type,

    appPath: (state) => state.app.path,
    collapse: (state) => state.app.collapse,
    clientType: (state) => state.app.clientType,

    token: (state) => {
      var _token = state.user.token;
      if (_token) {
        return _token;
      } else {
        return getCookies();
      }
    },
  },
  modules: {
    user,
    app,
  },
});
