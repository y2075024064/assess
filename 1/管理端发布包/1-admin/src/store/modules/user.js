import { getCookies, setCookies, removeCookies } from "@/utils/auth";
import router, { resetRouter } from "@/router/index";
import request from "@/utils/request";

var this_login = function (data) {
  return request({
    url: "Auth/Login",
    method: "post",
    data,
  });
};

var this_logout = function () {
  return request({
    url: "Auth/Logout",
    method: "get",
  });
};

var this_getInfo = function () {
  return request({
    url: "admin/Member/GetInfo",
    method: "get",
  });
};

const re_key = "assess_ms_login_remember";

const getDefaultState = () => {
  var remember = getCookies(re_key);

  return {
    name: "",
    type: "",
    gender: "",
    avatar: "",
    token: getCookies(),
    remember: remember === "true",
  };
};

const state = getDefaultState();

const mutations = {
  resetState: (state) => {
    Object.assign(state, getDefaultState());
  },
  setGender: (state, gender) => {
    state.gender = gender;
  },
  setToken: (state, token) => {
    state.token = token;
    setCookies(token, true);
  },
  setName: (state, name) => {
    state.name = name;
  },
  setAvatar: (state, avatar) => {
    state.avatar = avatar;
  },
  setRemember: (state, remember) => {
    state.remember = remember;
  },
  setUserType: (state, userType) => {
    state.type = userType;
  },
};

const actions = {
  name({ commit }, nickname) {
    commit("setName", nickname);
  },
  avatar({ commit }, avatar) {
    commit("setAvatar", avatar);
  },
  gender({ commit }, gender) {
    commit("setGender", gender);
  },
  token({ commit }, token) {
    commit("setToken", token);
  },
  userType({ commit }, userType) {
    commit("setUserType", userType);
  },
  // user login
  login({ commit }, userInfo) {
    setCookies(userInfo.Remember, true, re_key);
    commit("setRemember", userInfo.Remember);

    return new Promise((resolve, reject) => {
      userInfo.Account = userInfo.Account.trim();
      this_login(userInfo)
        .then((res) => {
          if (res.IsSucc) {
            commit("setName", res.Data.Nickname || res.Data.Account);
            commit("setUserType", res.Data.MemberType);
            commit("setAvatar", res.Data.Avatar);
            commit("setGender", res.Data.Gender);
            commit("setToken", res.Data.Token)
          }

          resolve(res);
        })
        .catch((error) => {
          reject(error);
        });
    });
  },
  // user getInfo
  getInfo({ commit }) {
    return new Promise((resolve, reject) => {
      this_getInfo()
        .then((res) => {
          if (res.IsSucc) {
            commit("setName", res.Data.Nickname || res.Data.account);
            commit("setUserType", res.Data.MemberType);
            commit("setAvatar", res.Data.Avatar);
            commit("setGender", res.Data.Gender);
          }

          resolve(res);
        })
        .catch((error) => {
          reject(error);
        });
    });
  },
  // user logout
  logout({ commit }) {
    return new Promise((resolve, reject) => {
      this_logout()
        .then((res) => {
          // 移除账号信息
          removeCookies();

          resetRouter();

          commit("resetState");

          resolve(res);

          router.push("/");
        })
        .catch((error) => {
          reject(error);
        });
    });
  },
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
};
