<template>
  <div class="login-page">
    <div class="login-card">
      <div class="login-title">后台管理登录</div>
      <el-form ref="formRef" :model="form" :rules="rules" label-width="0">
        <el-form-item prop="Account">
          <el-input v-model="form.Account" placeholder="管理员账号" size="large" />
        </el-form-item>
        <el-form-item prop="Password">
          <el-input v-model="form.Password" type="password" placeholder="密码" size="large" show-password />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" size="large" style="width:100%" :loading="loading" @click="onLogin">
            登 录
          </el-button>
        </el-form-item>
        <div v-if="errorMsg" class="error-msg">{{ errorMsg }}</div>
      </el-form>
      <div style="text-align:center;margin-top:16px">
        <el-button link type="primary" @click="$router.push('/adminRegist')">
          创建管理员账号
        </el-button>
      </div>
    </div>
  </div>
</template>

<script>
/* eslint-disable */
export default {
  name: "AdminLogin",
  data() {
    return {
      loading: false,
      errorMsg: "",
      form: { Account: "admin", Password: "admin888" },
      rules: {
        Account: [{ required: true, message: "请输入账号", trigger: "blur" }],
        Password: [{ required: true, message: "请输入密码", trigger: "blur" }],
      },
    };
  },
  methods: {
    onLogin() {
      this.$refs.formRef.validate((valid) => {
        if (!valid) return;
        this.loading = true;
        this.errorMsg = "";
        this.$store.dispatch("user/login", { Account: this.form.Account, Password: this.form.Password, Remember: true })
          .then((res) => {
            if (res.IsSucc) {
              this.$router.push("/");
            } else {
              this.errorMsg = res.Msg || "登录失败";
            }
          })
          .catch(() => { this.errorMsg = "网络错误"; })
          .finally(() => { this.loading = false; });
      });
    },
  },
};
</script>

<style scoped>
.login-page {
  width: 100vw;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}
.login-card {
  width: 400px;
  padding: 40px;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 8px 32px rgba(0,0,0,0.15);
}
.login-title {
  font-size: 24px;
  font-weight: bold;
  text-align: center;
  margin-bottom: 30px;
  color: #333;
}
.error-msg {
  color: #f56c6c;
  font-size: 13px;
  text-align: center;
  margin-top: 10px;
}
</style>
