var width = document.documentElement.width || window.innerWidth;
var clientType = "PCH5";
var userAgent = navigator.userAgent.toLowerCase();
if (userAgent.indexOf("mobile") > -1) {
  clientType = "MobileH5";
}

const state = {
  path: "/",
  collapse: width <= 1100,
  clientType: clientType,
};

const mutations = {
  setCollapse: (state, val) => {
    state.collapse = val;
  },
  setPath: (state, val) => {
    state.path = val;
  },
};

const actions = {
  path({ commit }, val) {
    commit("setPath", val);
  },
  collapse({ commit }, val) {
    commit("setCollapse", val);
  },
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
};
