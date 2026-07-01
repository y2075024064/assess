<template>
  <el-form
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="80px"
  >
    <el-form-item label="账户名" prop="Account">
      <el-input v-model="formData.Account" :disabled="!canModifyAccount" />
    </el-form-item>

    <el-form-item label="真实姓名" prop="RealName">
      <el-input v-model="formData.RealName" />
    </el-form-item>

    <el-form-item label="用户性别" prop="Gender">
      <el-radio-group v-model="formData.Gender">
        <el-radio border label="保密" />
        <el-radio border label="男" />
        <el-radio border label="女" />
      </el-radio-group>
    </el-form-item>

    <el-form-item label="手机号码" prop="MobilePhone">
      <el-input v-model="formData.MobilePhone" />
    </el-form-item>

    <el-form-item label="备注" prop="Remark">
      <el-input v-model="formData.Remark" />
    </el-form-item>

    <el-form-item>
      <el-button type="primary" @click="submit">确定</el-button>
      <el-button @click="close(false)">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  name: "AdminDialog",
  props: {
    detail: { type: Object, default: null },
  },
  emits: ["close"],
  data() {
    return {
      isUpdate: false,
      canModifyAccount: true,
      formRules: {
        Account: [
          { required: true, message: "账户名必填！" },
          { max: 32, message: "账户名最大长度 32 ！" },
        ],
        RealName: [
          { required: true, message: "真实姓名必填！" },
          { max: 20, message: "真实姓名最大长度 20 ！" },
        ],
        Gender: [{ required: true, message: "用户性别必选！" }],
        MobilePhone: [
          { required: true, message: "手机号码必填！" },
          {
            pattern: /^1[\d]{9}[\d]$/,
            message: "手机号码格式错误",
          },
        ],
      },
      formData: {
        Id: 0,
        Account: "",
        RealName: "",
        Gender: "保密",
        MobilePhone: "",
        Remark: "",
      },
    };
  },
  mounted() {
    if (this.detail) {
      this.isUpdate = true;
      for (var item in this.formData) {
        this.formData[item] = this.detail[item];
      }
      this.canModifyAccount = this.detail.CanModifyAccount;
    }
  },
  methods: {
    close(state) {
      this.$emit("close", state);
    },
    submit() {
      this.$refs.dialogForm.validate((isValid) => {
        if (isValid) {
          this.submitForm();
        }
      });
    },
    submitForm() {
      var url = `admin/Member/${this.isUpdate ? "Update" : "Insert"}`;
      this.$http.post(url, this.formData).then((res) => {
        if (res.IsSucc) {
          if (this.isUpdate) {
            this.$message.success(res.Msg);
          } else {
            this.$alert(res.Msg);
          }
          this.close(true);
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
  },
};
</script>
