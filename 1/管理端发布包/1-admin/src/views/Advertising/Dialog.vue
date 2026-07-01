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

    <el-form-item label="所属模块" prop="ModuleId">
      <MySelect
        v-model="formData.ModuleId"
        url="/admin/AdModule/Select"
        style="width: 300px"
      />
    </el-form-item>

    <el-form-item label="广告类型" prop="Type">
      <el-select v-model="formData.Type" style="width: 120px">
        <el-option value="外链接" label="外链接" />
        <el-option value="富文本" label="富文本" />
        <el-option value="Html文件" label="Html文件" />
      </el-select>
    </el-form-item>
    <br />

    <el-form-item label="标题" prop="Title">
      <el-input v-model="formData.Title" style="width: 530px" />
    </el-form-item>
    <br />

    <el-form-item label="所属公司" prop="Company">
      <el-input v-model="formData.Company" style="width: 530px" />
    </el-form-item>
    <br />

    <el-form-item
      v-if="formData.Type == '外链接'"
      label="超链接"
      prop="Hyperlink"
    >
      <el-input v-model="formData.Hyperlink" style="width: 530px" />
    </el-form-item>

    <el-form-item
      v-else-if="formData.Type == '富文本'"
      label="广告内容"
      prop="Content"
    >
      <WangEditor
        v-model="formData.Content"
        type="AdContent"
        should-image
        should-video
        ref="wEditor"
      />
    </el-form-item>

    <el-form-item
      v-else-if="formData.Type == 'Html文件'"
      label="网页文件"
      prop="HtmlFiles"
    >
      <el-upload
        action="#"
        multiple
        :auto-upload="false"
        :on-change="filesChanged"
      >
        <el-button type="primary">选择文件</el-button>
      </el-upload>
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
import MySelect from "@/components/mySelect.vue";

export default {
  name: "AdvertisingDialog",
  components: { UploadFilled, MySelect, WangEditor },
  props: {
    detail: { type: Object, default: null },
  },
  emits: ["close"],
  data() {
    return {
      isUpdate: false,
      previewUrl: null,
      formRules: {
        Title: [
          { required: true, message: "标题必填！" },
          { max: 128, message: "标题最大长度 128 ！" },
        ],
        Hyperlink: [
          { required: true, message: "超链接必填！" },
          { max: 255, message: "超链接最大长度 255 ！" },
        ],
        Content: [{ required: true, message: "广告内容必填！" }],
        HtmlFiles: [{ required: true, message: "网页文件必选！" }],
        Company: [{ max: 32, message: "所属公司最大长度 32 ！" }],
        Remark: [{ max: 255, message: "备注最大长度 255 ！" }],
      },
      formData: {
        Id: 0,
        ModuleId: "",
        PreviewFile: "",
        HtmlFiles: "",
        Type: "外链接",
        Title: "",
        Content: "",
        Hyperlink: "",
        Company: "",
        Remark: "",
      },
    };
  },
  mounted() {
    if (this.detail) {
      this.isUpdate = true;
      for (var item in this.detail) {
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
      if (this.formData.Type == "富文本") {
        var text = this.$refs.wEditor.getText();
        if (!text) this.formData.Content = "";
      }

      if (
        this.isUpdate &&
        this.formData.Type == "Html文件" &&
        !this.formData.HtmlFiles
      ) {
        this.formData.HtmlFiles = [];
      }

      this.$refs.dialogForm.validate((isValid) => {
        if (isValid) {
          this.submitForm();
        }
      });
    },
    submitForm() {
      var url = `admin/Advertising/${this.isUpdate ? "Update" : "Insert"}`;
      var formDa = new FormData();
      for (var item in this.formData) {
        formDa.append(item, this.formData[item]);
      }

      if (this.formData.HtmlFiles) {
        formDa.delete("HtmlFiles");
        for (var i = 0; i < this.formData.HtmlFiles.length; i++) {
          formDa.append("HtmlFiles", this.formData.HtmlFiles[i]);
        }
      }

      this.$http.postForm(url, formDa).then((res) => {
        if (res.IsSucc) {
          var wEditor = this.$refs.wEditor;
          if (wEditor) wEditor.verifyImages();
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
    filesChanged(uploadFile, uploadFiles) {
      this.formData.HtmlFiles = "";

      if (uploadFiles.length > 0) {
        this.formData.HtmlFiles = [];
        for (var i = 0; i < uploadFiles.length; i++) {
          this.formData.HtmlFiles.push(uploadFiles[i].raw);
        }
      }
    },
  },
};
</script>
