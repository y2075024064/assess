<template>
  <el-form
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="80px"
  >
    <el-form-item label="总分数" prop="TotalScore">
      <el-input-number v-model="formData.TotalScore" :min="50" :max="150" />
    </el-form-item>

    <el-form-item label="题目数量" prop="TopicNumber">
      <el-input-number v-model="formData.TopicNumber" :min="10" :max="200" />
    </el-form-item>

    <el-form-item label="答题时长" prop="TestTime">
      <el-input-number
        v-model="formData.TestTime"
        :max="10800"
        :step="60"
        :min="60"
      />&nbsp;&nbsp;秒
    </el-form-item>

    <el-form-item>
      <el-button type="primary" @click="submit">确定</el-button>
      <el-button @click="close(false)">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  name: "TestPlan",
  props: ["partId"],
  emits: ["close"],
  data() {
    var testTimeValidator = function (rule, value, callback) {
      if (value && (value < 60 || value > 10800)) {
        callback(new Error("答题时间范围 60 ~ 10800 秒"));
      } else {
        callback();
      }
    };

    var totalScoreValidator = function (rule, value, callback) {
      if (!value) {
        callback(new Error("总分数必填！"));
      } else if (value < 50 || value > 150) {
        callback(new Error("总分数范围 50 ~ 150 分"));
      } else {
        callback();
      }
    };

    var topicNumberValidator = function (rule, value, callback) {
      if (!value) {
        callback(new Error("题目数量必填！"));
      } else if (value < 10 || value > 200) {
        callback(new Error("题目数量范围 10 ~ 200 道"));
      } else {
        callback();
      }
    };

    return {
      isUpdate: false,
      selectData: [],
      formRules: {
        TestTime: [{ validator: testTimeValidator }],
        TotalScore: [{ validator: totalScoreValidator }],
        TopicNumber: [{ validator: topicNumberValidator }],
      },
      formData: {
        PartId: this.partId,
        TestTime: null,
        TotalScore: 0,
        TopicNumber: 0,
      },
    };
  },
  mounted() {
    this.getDetail();
  },
  methods: {
    close(state) {
      this.$emit("close", state);
    },
    getDetail() {
      var params = { partId: this.partId };
      this.$http.get("admin/TestPlan/Detail", { params }).then((res) => {
        if (res.IsSucc) {
          this.isUpdate = true;
          this.formData.TestTime = res.Data.TestTime;
          this.formData.TotalScore = res.Data.TotalScore;
          this.formData.TopicNumber = res.Data.TopicNumber;
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
      var url = `admin/TestPlan/${this.isUpdate ? "Update" : "Insert"}`;
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
