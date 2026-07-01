<template>
  <el-form
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="80px"
    inline
  >
    <el-form-item label="图片">
      <el-upload
        drag
        action="#"
        :auto-upload="false"
        :show-file-list="false"
        :on-change="fileChanged"
        style="min-width: 400px"
      >
        <el-image v-if="previewUrl" :src="previewUrl" fit="contain" />
        <div v-else>
          <el-icon class="el-icon--upload"><upload-filled /></el-icon>
          <div class="el-upload__text">拖拽图片到此或者<em>点击上传</em></div>
        </div>
        <template #tip>
          <div class="el-upload__tip">图片大小不能超过 3M</div>
        </template>
      </el-upload>
    </el-form-item>
    <br />

    <el-form-item label="动态类型" prop="Type">
      <el-select v-model="formData.Type" style="width: 120px">
        <el-option value="系统" label="系统" />
        <el-option value="行业" label="行业" />
        <el-option value="新闻" label="新闻" />
        <el-option value="活动" label="活动" />
      </el-select>
    </el-form-item>

    <el-form-item label="发布时间" prop="PublishTime">
      <el-date-picker
        v-model="formData.PublishTime"
        value-format="YYYY-MM-DD HH:mm:ss"
        :editable="false"
        type="datetime"
      />
    </el-form-item>
    <br />

    <el-form-item label="标题" prop="Title">
      <el-input v-model="formData.Title" style="width: 530px" />
    </el-form-item>
    <br />

    <el-form-item label="动态内容" prop="Content">
      <WangEditor
        v-model="formData.Content"
        type="DynamicContent"
        should-image
        should-video
        ref="wEditor"
      />
    </el-form-item>
    <br />

    <el-form-item label="原文链接" prop="OriginalHref">
      <el-input v-model="formData.OriginalHref" style="width: 530px" />
    </el-form-item>
    <br />

    <el-form-item label="备注" prop="Remark">
      <el-input v-model="formData.Remark" style="width: 530px" />
    </el-form-item>
    <br />

    <el-form-item label=" ">
      <el-button type="primary" @click="submit">确定</el-button>
      <el-button @click="close(false)">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import { UploadFilled } from "@element-plus/icons-vue";
import WangEditor from "@/components/wangEditor.vue";

export default {
  name: "DynamicDialog",
  components: { UploadFilled, WangEditor },
  props: {
    detail: { type: Object, default: null },
  },
  emits: ["close"],
  data() {
    return {
      isUpdate: false,
      previewUrl: null,
      formRules: {
        Type: [{ required: true, message: "类型必选！" }],
        Title: [
          { required: true, message: "标题必填！" },
          { max: 125, message: "标题最大长度 125 ！" },
        ],
        Content: [{ required: true, message: "动态内容必填！" }],
        OriginalHref: [{ max: 255, message: "原文链接最大长度 255 ！" }],
        Remark: [{ max: 255, message: "备注最大长度 255 ！" }],
      },
      formData: {
        Id: 0,
        Type: "",
        Title: "",
        Content: "",
        OriginalHref: "",
        PublishTime: "",
        PreviewFile: "",
        Remark: "",
      },
    };
  },
  beforeMount() {
    if (this.detail) {
      this.isUpdate = true;
      for (var item in this.formData) {
        this.formData[item] = this.detail[item];
      }
      this.previewUrl = this.detail.PreviewImg;
    }
  },
  methods: {
    close(state) {
      this.$emit("close", state);
    },
    submit() {
      var text = this.$refs.wEditor.getText();
      if (!text) this.formData.Content = "";

      this.$refs.dialogForm.validate((isValid) => {
        if (isValid) {
          this.submitForm();
        }
      });
    },
    submitForm() {
      var url = `admin/Dynamic/${this.isUpdate ? "Update" : "Insert"}`;
      var formDa = new FormData();
      for (var item in this.formData) {
        formDa.append(item, this.formData[item]);
      }

      this.$http.postForm(url, formDa).then((res) => {
        if (res.IsSucc) {
          this.$message.success(res.Msg);
          this.close(true);
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
    fileChanged(uploadFile) {
      var file = uploadFile.raw;
      this.formData.PreviewFile = file;
      this.previewUrl = URL.createObjectURL(file);
    },
  },
};
</script>
