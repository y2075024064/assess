<template>
  <el-form
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="80px"
  >
    <el-form-item label="考试名称" prop="Name">
      <el-input v-model="formData.Name" clearable />
    </el-form-item>

    <el-form-item label="考试目的" prop="Purpose">
      <el-input v-model="formData.Purpose" type="textarea" clearable />
    </el-form-item>

    <el-form-item label="参考对象" prop="ReferenceObj">
      <el-input v-model="formData.ReferenceObj" type="textarea" clearable />
    </el-form-item>

    <el-form-item label="开考时间" prop="StartTime">
      <el-date-picker
        value-format="YYYY-MM-DD HH:mm:ss"
        v-model="formData.StartTime"
        format="YYYY-MM-DD HH:mm:ss"
        style="width: 380px"
        type="datetime"
        clearable
      />
    </el-form-item>

    <el-form-item label="考试时长" prop="Duration">
      <el-input-number v-model="formData.Duration" :max="120" :min="1" />
      &nbsp;&nbsp;分钟
    </el-form-item>

    <el-form-item label="试题总分" prop="TotalScore">
      <el-input-number v-model="formData.TotalScore" :max="500" :min="1" />
    </el-form-item>

    <el-form-item label="日期范围" prop="DateRange">
      <el-date-picker
        v-model="formData.DateRange"
        value-format="YYYY-MM-DD"
        format="YYYY-MM-DD"
        type="daterange"
        clearable
      />
    </el-form-item>

    <el-form-item label="是否公开" prop="IsPublic">
      <el-switch
        v-model="formData.IsPublic"
        inactive-text="否"
        active-text="是"
        inline-prompt
        width="50"
      />
    </el-form-item>

    <el-form-item label="主办方" prop="Organizer">
      <el-input v-model="formData.Organizer" clearable />
    </el-form-item>

    <el-form-item label="协办方" prop="CoOrganizer">
      <el-input v-model="formData.CoOrganizer" clearable />
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
    { required: true, message: "考试名称必填！" },
    { max: 64, message: "考试名称最大长度 64 ！" },
  ],
  Purpose: [{ max: 128, message: "考试目的最大长度 128 ！" }],
  ReferenceObj: [{ max: 128, message: "参考对象最大长度 128 ！" }],
  TotalScore: [{ required: true, message: "试题总分必填！" }],
  StartTime: [{ required: true, message: "开考时间必选！" }],
  Duration: [{ required: true, message: "考试时长必填！" }],
  IsPublic: [{ required: true, message: "是否公开必选！" }],
  Organizer: [{ max: 128, message: "主办方最大长度 128 ！" }],
  CoOrganizer: [{ max: 128, message: "协办方最大长度 128 ！" }],
  Remark: [{ max: 255, message: "备注最大长度 255 ！" }],
});
const formData = ref({
  Id: 0,
  Name: "",
  Purpose: "",
  TotalScore: 0,
  StartTime: "",
  Duration: 0,
  ReferenceObj: "",
  StartDate: "",
  EndDate: "",
  DateRange: ["", ""],
  IsPublic: true,
  Organizer: "",
  CoOrganizer: "",
  Remark: "",
});

const submitForm = () => {
  var url = `admin/Exam/${isUpdate.value ? "Update" : "Insert"}`;
  formData.value.StartDate = formData.value.DateRange[0];
  formData.value.EndDate = formData.value.DateRange[1];

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
    for (const item in props.detail) {
      formData.value[item] = props.detail[item];
    }
    formData.value.DateRange = [
      props.detail.StartDate || "",
      props.detail.EndDate || "",
    ];
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
