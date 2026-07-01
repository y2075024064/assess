<template>
  <el-form
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="80px"
  >
    <el-form-item label="名称" prop="Name">
      <el-input v-model="formData.Name" />
    </el-form-item>

    <el-form-item label="标识" prop="ModuleName">
      <el-input v-model="formData.ModuleName" />
    </el-form-item>

    <el-form-item label="地点" prop="Location">
      <el-input v-model="formData.Location" />
    </el-form-item>

    <el-form-item label="是否启用" prop="IsEnable">
      <el-switch
        v-model="formData.IsEnable"
        inactive-text="否"
        active-text="是"
        inline-prompt
        size="large"
        :width="80"
      />
    </el-form-item>

    <el-form-item label="自动轮播" prop="IsCarousel">
      <el-switch
        v-model="formData.IsCarousel"
        inactive-text="否"
        active-text="是"
        inline-prompt
        size="large"
        :width="80"
      />
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
  name: "AdModuleDialog",
  props: {
    detail: { type: Object, default: null },
  },
  emits: ["close"],
  data() {
    return {
      isUpdate: false,
      formRules: {
        Name: [
          { required: true, message: "名称必填！" },
          { max: 32, message: "名称最大长度 32 ！" },
        ],
        ModuleName: [
          { required: true, message: "标识必填！" },
          { max: 32, message: "标识最大长度 32 ！" },
        ],
        Location: [
          { required: true, message: "地点必填！" },
          { max: 64, message: "地点最大长度 64 ！" },
        ],
        Remark: [{ max: 255, message: "备注最大长度 255 ！" }],
      },
      formData: {
        Id: 0,
        Name: "",
        ModuleName: "",
        Location: "",
        IsEnable: true,
        IsCarousel: false,
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
      var url = `admin/AdModule/${this.isUpdate ? "Update" : "Insert"}`;
      this.$http.post(url, this.formData).then((res) => {
        if (res.IsSucc) {
          this.$message.success(res.Msg);
          this.close(true);
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
  },
};
</script>
