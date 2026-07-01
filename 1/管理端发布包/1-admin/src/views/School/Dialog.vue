<template>
  <el-form
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="80px"
  >
    <el-form-item label="Logo">
      <el-upload
        action="#"
        :auto-upload="false"
        :show-file-list="false"
        :on-change="fileChanged"
        class="logo-uploader"
      >
        <el-image
          v-if="previewUrl"
          class="logo"
          :src="previewUrl"
          fit="contain"
        />
        <el-icon v-else class="logo-uploader-icon"><upload-filled /></el-icon>
      </el-upload>
    </el-form-item>

    <el-form-item label="简称" prop="Name">
      <el-input v-model="formData.Name" clearable />
    </el-form-item>

    <el-form-item label="全称" prop="FullName">
      <el-input v-model="formData.FullName" clearable />
    </el-form-item>

    <el-form-item label="类型" prop="SchoolType">
      <el-select v-model="formData.SchoolType" placeholder="请选择" clearable>
        <el-option label="小学" value="小学" />
        <el-option label="初中" value="初中" />
        <el-option label="高中" value="高中" />
        <el-option label="大学" value="大学" />
      </el-select>
    </el-form-item>

    <el-form-item label="校长" prop="Rector">
      <el-input v-model="formData.Rector" clearable />
    </el-form-item>

    <el-form-item label="简介" prop="Intro">
      <el-input v-model="formData.Intro" type="textarea" clearable />
    </el-form-item>

    <el-form-item label="地址" prop="Address">
      <el-input v-model="formData.Address" clearable />
    </el-form-item>

    <el-form-item label="电话" prop="PhoneNumber">
      <el-input v-model="formData.PhoneNumber" clearable />
    </el-form-item>

    <el-form-item label="邮箱" prop="Email">
      <el-input v-model="formData.Email" clearable />
    </el-form-item>

    <el-form-item label="备注" prop="Remark">
      <el-input v-model="formData.Remark" clearable />
    </el-form-item>

    <el-form-item label=" ">
      <el-button type="primary" @click="submit">确定</el-button>
      <el-button @click="close(false)">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script setup>
import { UploadFilled } from "@element-plus/icons-vue";

import { ref, watch, onBeforeMount, defineProps, defineEmits } from "vue";
import { showError, showSuccess } from "@/utils/elBox";
import http from "@/utils/request";

const props = defineProps({
  detail: Object,
});
const emit = defineEmits(["close"]);

const previewUrl = ref("");
const dialogForm = ref();
const isUpdate = ref(false);
const formRules = ref({
  Name: [
    { required: true, message: "简称必填！" },
    { max: 32, message: "简称最大长度 32 ！" },
  ],
  FullName: [
    { required: true, message: "全称必填！" },
    { max: 64, message: "全称最大长度 64 ！" },
  ],
  Rector: [{ max: 32, message: "校长最大长度 32 ！" }],
  SchoolType: [{ required: true, message: "类型必选！" }],
  Intro: [{ max: 560, message: "简介最大长度 560 ！" }],
  Address: [{ max: 255, message: "地址最大长度 255 ！" }],
  PhoneNumber: [{ max: 20, message: "电话最大长度 20 ！" }],
  Email: [{ max: 100, message: "邮箱最大长度 100 ！" }],
  Remark: [{ max: 255, message: "备注最大长度 255 ！" }],
});
const formData = ref({
  Id: 0,
  Name: "",
  FullName: "",
  Rector: "",
  Intro: "",
  Address: "",
  SchoolType: "",
  PhoneNumber: "",
  Email: "",
  LogoFile: "",
  Remark: "",
});

const fileChanged = (uploadFile) => {
  var file = uploadFile.raw;
  formData.value.LogoFile = file;
  previewUrl.value = URL.createObjectURL(file);
};

const submitForm = () => {
  var url = `admin/School/${isUpdate.value ? "Update" : "Insert"}`;
  const data = new FormData();
  for (const item in formData.value) {
    data.append(item, formData.value[item]);
  }
  http.postForm(url, data).then((res) => {
    if (res.IsSucc) {
      showSuccess(res.Msg);
      close(true);
    } else {
      showError(res.Msg);
    }
  });
};

const submit = () => {
  dialogForm.value.validate((isValid) => {
    if (isValid) {
      submitForm();
    }
  });
};

const close = (state) => {
  emit("close", state);
};

const delData = () => {
  if (props.detail) {
    isUpdate.value = true;
    for (const item in props.detail) {
      formData.value[item] = props.detail[item];
    }
    previewUrl.value = props.detail.Logo;
  }
};

watch(
  () => props.detail,
  () => delData()
);

onBeforeMount(() => delData());
</script>

<script>
export default {
  name: "ExamDialog",
};
</script>

<style>
.logo-uploader .logo {
  min-width: 100px;
  height: 100px;
  display: block;
}

.logo-uploader .el-upload {
  border: 1px dashed var(--el-border-color);
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
  transition: var(--el-transition-duration-fast);
}

.logo-uploader .el-upload:hover {
  border-color: var(--el-color-primary);
}

.el-icon.logo-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  min-width: 100px;
  height: 100px;
  text-align: center;
}
</style>
