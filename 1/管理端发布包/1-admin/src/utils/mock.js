/* ============================================================
 *  模拟后端 API - 开发模式下使用
 *  文件位置：src/utils/mock.js
 * ============================================================ */

// 模拟用户数据库
var mockDB = {
  users: [],
  tokens: {},
};

export function setupMock(axiosInstance) {
  // 请求拦截器 - 模拟后端响应
  axiosInstance.interceptors.request.use(
    (config) => {
      var url = config.url || "";
      var method = (config.method || "get").toLowerCase();
      var mockResult = null;

      // ========== Home/GetToken ==========
      if (url === "Home/GetToken" && method === "get") {
        mockResult = {
          IsSucc: true,
          Code: 200,
          Msg: "获取成功",
          Data: "mock_token_2024",
        };
      }

      // ========== Auth/Regist ==========
      if (url === "Auth/Regist" && method === "post") {
        var _data = typeof config.data === "string" ? JSON.parse(config.data) : config.data;
        var _exists = mockDB.users.find((u) => u.Account === _data.Account);
        if (_exists) {
          mockResult = { IsSucc: false, Code: 0, Msg: "该账户名已被注册！" };
        } else {
          mockDB.users.push({ Account: _data.Account, Password: _data.Password, MemberType: _data.MemberType });
          mockResult = { IsSucc: true, Code: 200, Msg: "注册成功！" };
        }
      }

      // ========== Auth/Login ==========
      if (url === "Auth/Login" && method === "post") {
        var loginData = typeof config.data === "string" ? JSON.parse(config.data) : config.data;
        // 先从 mockDB 查找
        var loginUser = mockDB.users.find(function(u) { return u.Account === loginData.Account && u.Password === loginData.Password; });
        // 再从 localStorage 查找（注册页存进去的）
        if (!loginUser) {
          var localUsers = JSON.parse(typeof window !== "undefined" ? (window.localStorage.getItem("assess_accounts") || "[]") : "[]");
          var localPasswords = JSON.parse(typeof window !== "undefined" ? (window.localStorage.getItem("assess_passwords") || "{}") : "{}");
          if (localUsers.indexOf(loginData.Account) >= 0 && localPasswords[loginData.Account] === loginData.Password) {
            loginUser = { Account: loginData.Account, Password: loginData.Password, MemberType: "普通" };
            // 同步到 mockDB
            mockDB.users.push(loginUser);
          }
        }
        if (loginUser) {
          var mockToken = "token_" + Date.now();
          mockDB.tokens[mockToken] = loginUser;
          mockResult = {
            IsSucc: true,
            Code: 200,
            Msg: "登录成功",
            Data: {
              Token: mockToken,
              Nickname: loginUser.Account,
              MemberType: loginUser.MemberType || "普通",
              Avatar: "",
              Gender: "",
            },
          };
        } else {
          mockResult = { IsSucc: false, Code: 0, Msg: "账号或密码错误！" };
        }
      }

      // ========== Auth/Logout ==========
      if (url === "Auth/Logout" && method === "get") {
        mockResult = { IsSucc: true, Code: 200, Msg: "退出成功" };
      }

      // ========== admin/Member/GetInfo ==========
      if (url === "admin/Member/GetInfo" && method === "get") {
        var token = config.headers["X-Token"] || readCookie("assess_ms_token") || "";
        var user = mockDB.tokens[token];
        if (user) {
          mockResult = {
            IsSucc: true,
            Code: 200,
            Msg: "获取成功",
            Data: {
              Nickname: user.Account,
              MemberType: user.MemberType || "普通",
              Avatar: "",
              Gender: "",
            },
          };
        } else {
          mockResult = { IsSucc: false, Code: 401, Msg: "未登录" };
        }
      }

      // ========== 如果匹配到模拟接口 ==========
      if (mockResult) {
        console.log(`[Mock] ${method.toUpperCase()} ${url} ->`, mockResult);
        // 直接返回模拟数据，阻止实际请求
        config.mockResult = mockResult;
        // 修改 adapter 返回模拟数据
        config.adapter = function () {
          return new Promise((resolve) => {
            setTimeout(() => {
              resolve({
                data: mockResult,
                status: 200,
                statusText: "OK",
                headers: {},
                config,
              });
            }, 300); // 模拟网络延迟
          });
        };
      }

      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );
}

function readCookie(name) {
  var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
  return arr ? decodeURIComponent(arr[2]) : null;
}


