<template>
  <div class="reg-page">
    <div class="reg-card">
      <div class="reg-title">创建管理员</div>
      <el-form ref="formRef" :model="form" :rules="rules" label-width="80px">
        <el-form-item label="账户名" prop="Account">
          <el-input v-model="form.Account" placeholder="6-32位字母、数字、下划线" />
        </el-form-item>
        <el-form-item label="真实姓名" prop="RealName">
          <el-input v-model="form.RealName" placeholder="请输入真实姓名" />
        </el-form-item>
        <el-form-item label="昵称" prop="Nickname">
          <el-input v-model="form.Nickname" placeholder="留空自动生成" />
        </el-form-item>
        <el-form-item label="用户性别" prop="Gender">
          <el-radio-group v-model="form.Gender">
            <el-radio border label="保密" />
            <el-radio border label="男" />
            <el-radio border label="女" />
          </el-radio-group>
        </el-form-item>
        <el-form-item label="手机号码" prop="MobilePhone">
          <el-input v-model="form.MobilePhone" placeholder="11位手机号" />
        </el-form-item>
        <el-form-item label="登录密码" prop="Password">
          <el-input v-model="form.Password" type="password" placeholder="8-20位密码" show-password />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" style="width:100%" :loading="loading" @click="onRegister">
            创建管理员
          </el-button>
        </el-form-item>
        <div v-if="msg" :class="['msg', msgType]">{{ msg }}</div>
      </el-form>
      <div style="text-align:center;margin-top:16px">
        <el-button link @click="$router.push('/turnLogin')">返回登录</el-button>
      </div>
    </div>
  </div>
</template>

<script>
import request from "@/utils/request";

export default {
  name: "AdminRegister",
  data() {
    return {
      loading: false,
      msg: "",
      msgType: "success",
      form: { Account: "", RealName: "", Nickname: "", Gender: "保密", MobilePhone: "", Password: "" },
      rules: {
        Account: [
          { required: true, message: "账户名必填", trigger: "blur" },
          { max: 32, message: "账户名最大长度32位", trigger: "blur" }
        ],
        RealName: [
          { required: true, message: "真实姓名必填", trigger: "blur" },
          { max: 20, message: "真实姓名最大长度20位", trigger: "blur" }
        ],
        Gender: [
          { required: true, message: "用户性别必选", trigger: "change" }
        ],
        MobilePhone: [
          { required: true, message: "手机号码必填", trigger: "blur" },
          { pattern: /^1[\d]{9}[\d]$/, message: "手机号码格式错误", trigger: "blur" }
        ],
        Password: [
          { required: true, message: "登录密码必填", trigger: "blur" },
          { min: 8, max: 20, message: "密码长度8-20位", trigger: "blur" }
        ]
      }
    };
  },
  methods: {
    onRegister() {
      this.$refs.formRef.validate((valid) => {
        if (!valid) return;
        this.loading = true;
        this.msg = "";
        request({
          url: "admin/AdminRegist/Create",
          method: "post",
          data: {
            Account: this.form.Account,
            RealName: this.form.RealName,
            Nickname: this.form.Nickname || null,
            Gender: this.form.Gender,
            MobilePhone: this.form.MobilePhone,
            Password: this.form.Password
          }
        }).then((res) => {
          if (res.IsSucc) {
            this.msg = "管理员创建成功！";
            this.msgType = "success";
            this.form = { Account: "", RealName: "", Nickname: "", Gender: "保密", MobilePhone: "", Password: "" };
          } else {
            this.msg = res.Msg || "创建失败";
            this.msgType = "error";
          }
        }).catch(() => {
          this.msg = "网络错误";
          this.msgType = "error";
        }).finally(() => {
          this.loading = false;
        });
      });
    }
  }
};
</script>

<style scoped>
.reg-page {
  width: 100vw; height: 100vh;
  display: flex; align-items: center; justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}
.reg-card {
  width: 480px; padding: 40px;
  background: #fff; border-radius: 12px;
  box-shadow: 0 8px 32px rgba(0,0,0,0.15);
}
.reg-title {
  font-size: 22px; font-weight: bold;
  text-align: center; margin-bottom: 28px; color: #333;
}
.msg { text-align: center; font-size: 13px; margin-top: 10px; }
.msg.success { color: #67c23a; }
.msg.error { color: #f56c6c; }
</style>
