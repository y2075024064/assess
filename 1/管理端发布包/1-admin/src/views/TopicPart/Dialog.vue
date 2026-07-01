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

    <el-form-item label="推荐年级" prop="RcmdGrade">
      <el-select v-model="formData.RcmdGrade" clearable multiple>
        <el-option
          v-for="item in selectData"
          :key="item.Value"
          :label="item.Text"
          :value="item.Value"
        />
      </el-select>
    </el-form-item>

    <el-form-item label="排序级别" prop="SortLevel">
      <el-input v-model="formData.SortLevel" type="number" />
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
  name: "TopicPartDialog",
  props: {
    detail: { type: Object, default: null },
    parentId: { type: Number, default: null },
  },
  emits: ["close"],
  data() {
    return {
      isUpdate: false,
      selectData: [],
      formRules: {
        Name: [
          { required: true, message: "名称必填！" },
          { max: 32, message: "名称最大长度 32 ！" },
        ],
        SortLevel: [{ required: true, message: "排序级别必填！" }],
      },
      formData: {
        Id: 0,
        Name: "",
        Remark: "",
        SortLevel: 0,
        RcmdGrade: "",
        ParentId: this.parentId,
      },
    };
  },
  mounted() {
    this.getSelect();
    if (this.detail) {
      this.isUpdate = true;
      for (var item in this.formData) {
        this.formData[item] = this.detail[item];
      }
      if (this.detail.RcmdGrade) {
        this.formData.RcmdGrade = JSON.parse(this.detail.RcmdGrade);
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
    getSelect() {
      var url = "admin/Grade/Select";
      this.$http.get(url).then((res) => {
        if (res.IsSucc) {
          this.selectData = res.Data;
        }
      });
    },
    submitForm() {
      var url = `admin/TopicPart/${this.isUpdate ? "Update" : "Insert"}`;
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
