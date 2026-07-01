<template>
  <el-form
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="80px"
  >
    <el-form-item label="字典键" prop="key">
      <el-select
        v-model="formData.key"
        :disabled="isUpdate"
        style="width: 280px"
      >
        <el-option
          v-for="(item, index) in dictionaryTypes"
          v-bind="item"
          :key="index"
        />
      </el-select>
    </el-form-item>

    <el-form-item label="字典值" prop="value">
      <el-input v-model="formData.value" clearable />
    </el-form-item>

    <el-form-item label=" ">
      <el-button type="primary" @click="submit">确定</el-button>
      <el-button @click="close(false)">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script setup>
import { ref, onMounted, defineProps, defineEmits } from "vue";
import { showError, showSuccess } from "@/utils/elBox";
import { dictionaryTypes } from "@/configs/dictionary";
import http from "@/utils/request";

const props = defineProps({
  detail: Object,
});
const emit = defineEmits(["close"]);

const dialogForm = ref();
const isUpdate = ref(false);
const formRules = ref({
  key: [{ required: true, message: "字典键必选！" }],
  value: [
    { required: true, message: "字典值必填！" },
    { max: 255, message: "字典值最大长度 255 ！" },
  ],
});
const formData = ref({
  id: 0,
  key: "",
  value: "",
});

const submitForm = () => {
  var url = `admin/Dictionary/${isUpdate.value ? "Update" : "Insert"}`;
  http.get(url, { params: formData.value }).then((res) => {
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

onMounted(() => {
  if (props.detail) {
    isUpdate.value = true;
    formData.value.id = props.detail.Id;
    formData.value.key = props.detail.Key;
    formData.value.value = props.detail.Value;
  }
});
</script>

<script>
export default {
  name: "DictionaryDialog",
};
</script>
