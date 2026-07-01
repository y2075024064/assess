<template>
  <el-form
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="80px"
  >
    <el-form-item label="姓名" prop="Name">
      <el-input v-model="formData.Name" clearable />
    </el-form-item>

    <el-form-item label="学校" prop="SchoolId">
      <MySelect
        v-model="formData.SchoolId"
        url="/admin/School/Select"
        style="width: 380px"
      />
    </el-form-item>

    <el-form-item label="学级" prop="GradeNum">
      <el-input v-model="formData.GradeNum" clearable />
    </el-form-item>

    <el-form-item label="班级" prop="ClassNum">
      <el-input v-model="formData.ClassNum" clearable />
    </el-form-item>

    <el-form-item label="序号" prop="No">
      <el-input-number v-model="formData.No" :max="300" :min="1" />
    </el-form-item>

    <el-form-item label="年级" prop="CurrentGrade">
      <el-input v-model="formData.CurrentGrade" clearable />
    </el-form-item>

    <el-form-item label="性别" prop="Gender">
      <el-select v-model="formData.Gender" placeholder="请选择" clearable>
        <el-option label="保密" value="保密" />
        <el-option label="男" value="男" />
        <el-option label="女" value="女" />
      </el-select>
    </el-form-item>

    <el-form-item label=" ">
      <el-button type="primary" @click="submit">确定</el-button>
      <el-button @click="close(false)">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script setup>
import { ref, watch, onBeforeMount, defineProps, defineEmits } from "vue";
import { showError, showSuccess } from "@/utils/elBox";
import http from "@/utils/request";

const props = defineProps({
  detail: Object,
});
const emit = defineEmits(["close"]);

const dialogForm = ref();
const isUpdate = ref(false);
const formRules = ref({
  Name: [
    { required: true, message: "姓名必填！" },
    { max: 32, message: "姓名最大长度 32 ！" },
  ],
  SchoolId: [{ required: true, message: "学校必选！" }],
  Gender: [{ required: true, message: "性别必选！" }],
  GradeNum: [{ required: true, message: "学级必填！" }],
  ClassNum: [{ required: true, message: "班级必填！" }],
  No: [{ required: true, message: "序号必填！" }],
  CurrentGrade: [{ required: true, message: "年级必填！" }],
});
const formData = ref({
  Id: 0,
  Name: "",
  SchoolId: "",
  GradeNum: "",
  ClassNum: "",
  No: 1,
  CurrentGrade: "",
  Gender: "",
});

const submitForm = () => {
  var url = `admin/Student/${isUpdate.value ? "Update" : "Insert"}`;
  http.post(url, formData.value).then((res) => {
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
    const account = props.detail.Account;
    for (const item in props.detail) {
      if (item === "No") continue;
      formData.value[item] = props.detail[item];
    }
    formData.value.No = account.substr(account.length - 2);
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
  name: "StudentDialog",
};
</script>
