<template>
  <el-form
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="55px"
  >
    <el-form-item label="头像" prop="Avatar">
      <el-upload
        name="Avatar"
        accept="image/*"
        ref="uploadInput"
        :data="formData"
        :action="uploadUrl"
        :auto-upload="false"
        class="el-img-select"
        :show-file-list="false"
        :headers="uploadHeaders"
        :on-error="onSubmitError"
        :on-change="onImageChange"
        :on-success="onSubmitSuccess"
      >
        <el-avatar class="my-avatar" :src="imageSrc" :size="60" />
      </el-upload>
    </el-form-item>

    <el-form-item label="昵称" prop="Nickname">
      <el-input v-model="formData.Nickname" />
    </el-form-item>

    <el-form-item label="性别" prop="Gender">
      &emsp;
      <el-radio-group v-model="formData.Gender">
        <el-radio label="男" title="男">男</el-radio>
        <el-radio label="女" title="女">女</el-radio>
        <el-radio label="保密" title="保密">保密</el-radio>
      </el-radio-group>
    </el-form-item>

    <el-form-item label="生日" prop="Birthday">
      <el-date-picker v-model="formData.Birthday" type="date" />
    </el-form-item>

    <el-form-item label="手机" prop="MobilePhone">
      <el-input v-model="formData.MobilePhone" readonly />
    </el-form-item>

    <el-form-item label="邮箱" prop="Email">
      <el-input v-model="formData.Email" readonly />
    </el-form-item>

    <el-form-item>
      <el-button type="primary" @click="submit">保存</el-button>
      <el-button @click="close">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  name: "MemberEditDialog",
  emits: ["close"],
  data() {
    var uploadBaseUrl = process.env.VUE_APP_HOST;
    return {
      imageSrc: "",
      uploadUrl: uploadBaseUrl + "admin/Member/EditInfo",
      uploadHeaders: {
        "X-Token": this.$store.getters.token,
        "X-ClientType": this.$store.getters.clientType,
      },
      formData: {
        Email: "",
        Gender: "",
        Avatar: "",
        Nickname: "",
        Birthday: "",
        MobilePhone: "",
      },
      formRules: {
        Gender: [{ required: true, message: "性别必选！" }],
        Nickname: [{ required: true, message: "昵称必填！" }],
      },
    };
  },
  mounted() {
    this.loadDetail();
  },
  methods: {
    close() {
      this.$emit("close");
    },
    loadDetail() {
      var url = "admin/Member/GetEditInfo";
      this.$http.get(url).then((res) => {
        if (res.IsSucc) {
          this.imageSrc = res.Data.AvatarUrl;
          this.formData.Email = res.Data.Email;
          this.formData.Gender = res.Data.Gender;
          this.formData.Nickname = res.Data.Nickname;
          this.formData.Birthday = res.Data.Birthday;
          this.formData.MobilePhone = res.Data.MobilePhone;
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
    submit() {
      this.$refs.dialogForm.validate((isValid) => {
        if (isValid) {
          this.submitForm();
        }
      });
    },
    submitForm() {
      this.$refs.uploadInput.submit();
    },
    onImageChange(file) {
      var _file = file.raw;
      if (_file.type.indexOf("image") < 0) {
        this.$message.error("请选择图片!");
      } else if (_file.size / 1024 / 1024 > 1) {
        this.$message.error("图片大小不能超过 1MB!");
      } else {
        this.formData.Avatar = _file;
        this.imageSrc = URL.createObjectURL(_file);
      }
    },
    onSubmitSuccess(res) {
      if (res.IsSucc) {
        this.$message.success(res.Msg);
        this.$store.dispatch("user/getInfo");
        this.close();
      } else {
        this.$message.error(res.Msg);
      }
    },
    onSubmitError(err) {
      this.$alert("保存失败，" + err.message, "错误", { type: "error" });
    },
  },
};
</script>

<style lang="scss" scoped>
.el-img-select {
  width: 60px;
  height: 60px;
  margin-left: 10px;

  .my-avatar {
    border: 1px rgb(162, 162, 162) dashed;
  }

  .my-avatar:hover {
    border-color: rgb(52, 140, 222);
  }
}
</style>
