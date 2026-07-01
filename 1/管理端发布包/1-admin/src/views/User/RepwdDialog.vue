<template>
  <el-form
    ref="form"
    :rules="rules"
    :model="formData"
    label-width="80px"
    style="width: 300px"
  >
    <el-form-item label="原密码" prop="OldPwd">
      <el-input v-model="formData.OldPwd" show-password />
    </el-form-item>

    <el-form-item label="新密码" prop="NewPwd">
      <el-input v-model="formData.NewPwd" show-password />
    </el-form-item>

    <el-form-item label="确认密码" prop="RePwd">
      <el-input v-model="formData.RePwd" show-password />
    </el-form-item>

    <el-form-item label="验证码" prop="VerCode">
      <el-input
        v-model="formData.VerCode"
        placeholder="图形验证码"
        class="input-check"
        maxlength="4"
      >
        <template #append>
          <img :src="vercodeUrl" @click="changeImg" />
        </template>
      </el-input>
    </el-form-item>

    <el-form-item>
      <el-button type="primary" @click="submit">确定</el-button>
      <el-button @click="close">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  name: "RepwdDialog",
  emits: ["close"],
  data() {
    return {
      vercodeUrl: "",
      formData: {
        OldPwd: "",
        NewPwd: "",
        RePwd: "",
        VerCode: "",
      },
      rules: {
        OldPwd: [{ required: true, message: "原密码必填！" }],
        NewPwd: [
          { required: true, message: "新密码必填！" },
          { min: 8, max: 20, message: "密码长度必须是 8-20 !" },
        ],
        VerCode: [{ required: true, message: "验证码必填！" }],
        RePwd: [
          { validator: this.repwdCheck, trigger: "blur" },
          { min: 8, max: 20, message: "密码长度必须是 8-20 !" },
        ],
      },
    };
  },
  mounted() {
    this.changeImg();
  },
  methods: {
    close() {
      this.$emit("close");
    },
    changeImg() {
      var url = "admin/MemberAuth/VerCode?codeType=RePassword";
      this.$http.get(url, { responseType: "arraybuffer" }).then((res) => {
        var blob = new Blob([res]);
        var url = URL.createObjectURL(blob);
        this.vercodeUrl = url;
      });
    },
    repwdCheck(rule, value, callback) {
      if (value === "") {
        callback(new Error("确认密码必填！"));
      } else if (value !== this.formData.NewPwd) {
        callback(new Error("两次密码不一致！"));
      } else {
        callback();
      }
    },
    submit() {
      this.$refs.form.validate((isValid) => {
        if (isValid) {
          this.submitForm();
        }
      });
    },
    submitForm() {
      var url = "Auth/Repwd";
      this.$http.post(url, this.formData).then((res) => {
        if (res.IsSucc) {
          this.$alert(res.Msg, "信息提示", {
            type: "info",
            callback: () => {
              this.close();
              this.$store.dispatch("user/logout").then((res) => {
                if (res.IsSucc) {
                  this.$router.push("/");
                }
              });
            },
          });
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
  },
};
</script>

<style lang="scss">
.input-check {
  .el-input-group__append {
    border: none;
    padding: 0;
  }

  img {
    height: 29px;
    cursor: pointer;
    margin-top: -1px;
    margin-right: 2px;
    border-radius: 0px 4px 4px 0px;
  }
}
</style>
