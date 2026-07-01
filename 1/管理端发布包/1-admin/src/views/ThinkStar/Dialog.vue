<template>
  <el-form
    v-loading="loading"
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="60px"
    inline
  >
    <el-form-item label="预览图">
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

    <el-form-item label="用户" prop="memberId">
      <my-select
        v-model="formData.memberId"
        url="/admin/Member/Select"
        :init-option="initOption"
        style="width: 260px"
      />
    </el-form-item>

    <el-form-item label="类型" prop="type">
      <my-select
        v-model="formData.type"
        url="/admin/Dictionary/Select?key=ThinkStarTtype"
        style="width: 200px"
      />
    </el-form-item>
    <br />

    <el-form-item label="标题" prop="title">
      <el-input v-model="formData.title" clearable style="width: 550px" />
    </el-form-item>
    <br />

    <el-form-item label="内容" prop="content">
      <WangEditor
        v-model="formData.content"
        type="ThinkStar"
        should-image
        should-video
        ref="wEditor"
      />
    </el-form-item>
    <br />

    <el-form-item label="备注" prop="remark">
      <el-input v-model="formData.remark" clearable style="width: 550px" />
    </el-form-item>
    <br />

    <el-form-item label=" ">
      <el-button type="primary" @click="submit">确定</el-button>
      <el-button @click="close(false)">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script setup>
import WangEditor from "@/components/wangEditor.vue";

import { ref, watch, onBeforeMount, defineProps, defineEmits } from "vue";
import { showError, showSuccess } from "@/utils/elBox";
import http from "@/utils/request";

const props = defineProps({
  id: Number,
});
const emit = defineEmits(["close"]);

const wEditor = ref();
const dialogForm = ref();
const initOption = ref();
const loading = ref(false);
const previewUrl = ref("");
const isUpdate = ref(false);
const formRules = ref({
  memberId: [{ required: true, message: "用户必选！" }],
  type: [{ required: true, message: "类型必选！" }],
  content: [{ required: true, message: "内容必填！" }],
  title: [
    { required: true, message: "标题必填！" },
    { max: 128, message: "标题最大长度 128 ！" },
  ],
});
const formData = ref({
  id: 0,
  memberId: "",
  type: "",
  title: "",
  image: "",
  content: "",
  remark: "",
});

const fileChanged = (uploadFile) => {
  var file = uploadFile.raw;
  formData.value.image = file;
  previewUrl.value = URL.createObjectURL(file);
};

const submitForm = () => {
  var url = `admin/ThinkStar/${isUpdate.value ? "Update" : "Insert"}`;
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

const getData = () => {
  if (props.id) {
    loading.value = true;
    isUpdate.value = true;
    http.get(`/admin/ThinkStar/Detail?id=${props.id}`).then((res) => {
      for (const item in formData.value) {
        const uname = item[0].toUpperCase() + item.substring(1);
        formData.value[item] = res.Data[uname];
      }
      previewUrl.value = res.Data.PreviewImg;
      initOption.value = { Value: res.Data.MemberId, Text: res.Data.Account };
      loading.value = false;
    });
  }
};

watch(
  () => props.id,
  () => getData()
);

onBeforeMount(() => getData());
</script>

<script>
export default {
  name: "DictionaryDialog",
};
</script>
