<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="Plus" @click="addDialogOpen = true">
          添加
        </el-button>
        <el-button type="danger" :icon="Delete" @click="deleteRow()">
          删除
        </el-button>
        <el-button type="success" :icon="Bottom" @click="exportExcel()">
          导出
        </el-button>
      </el-form-item>

      <el-form-item>
        <div class="inline-box">
          <el-upload
            accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            :before-upload="beforeUpload"
            :on-success="uploadSuccess"
            :headers="uploadHeaders"
            :on-error="uploadError"
            :show-file-list="false"
            :action="uploadAction"
            class="upload-btn"
            name="excel"
          >
            <div class="btn-body">
              <el-icon><Upload /> </el-icon>
              &nbsp;批量上传
            </div>
          </el-upload>
          <a class="download-btn" title="下载Excel模板" :href="downloadHref">
            <el-icon> <Download /> </el-icon>
          </a>
        </div>
      </el-form-item>

      <el-form-item>
        <el-divider direction="vertical" style="height: 32px" />
      </el-form-item>

      <el-form-item label="性别">
        <el-select
          v-model="searchForm.Gender"
          placeholder="请选择"
          style="width: 90px"
          clearable
        >
          <el-option label="保密" value="保密" />
          <el-option label="男" value="男" />
          <el-option label="女" value="女" />
        </el-select>
      </el-form-item>

      <el-form-item label="学校">
        <MySelect
          v-model="searchForm.SchoolId"
          url="/admin/School/Select"
          style="width: 300px"
        />
      </el-form-item>

      <el-form-item label="年级">
        <MySelect
          v-model="searchForm.Grade"
          url="/admin/Grade/Select"
          style="width: 150px"
        />
      </el-form-item>

      <el-form-item label="关键字">
        <search-box
          v-model="searchForm.Keyword"
          placeholder="姓名、账号"
          @search="loadPage"
          width="250px"
        />
      </el-form-item>
    </el-form>

    <el-table
      highlight-current-row
      class="pager-table"
      table-layout="auto"
      :data="pageData"
      ref="tableRef"
      stripe
      border
    >
      <el-table-column type="selection" fixed="left" />
      <el-table-column label="账号" prop="Account" width="150" />
      <el-table-column label="姓名" prop="Name" width="250" />
      <el-table-column label="性别" prop="Gender" width="80" />
      <el-table-column label="学校" prop="SchoolName" min-width="200" />
      <el-table-column label="学级" prop="GradeNum" width="80" />
      <el-table-column label="班级" prop="ClassNum" width="80" />
      <el-table-column label="年级" prop="CurrentGrade" width="100" />
      <el-table-column fixed="right" label="操作" align="center" width="110">
        <template #default="{ row }">
          <ColumnTools
            :show-info="false"
            @on-del="deleteRow(row)"
            @on-edit="openEdit(row)"
          />
        </template>
      </el-table-column>
    </el-table>

    <my-pager
      v-model:currentPage="searchForm.Page"
      v-model:pageSize="searchForm.Take"
      :total="total"
      @sizeChange="loadPage"
      @currentChange="loadPage"
    />
  </div>

  <el-dialog
    v-model="showProgress"
    title="上传中。。。"
    :show-close="false"
    destroy-on-close
    width="300px"
    align-center
  >
    <el-progress
      :indeterminate="true"
      :show-text="false"
      :stroke-width="25"
      :percentage="50"
      striped
    />
  </el-dialog>

  <my-dialog v-model="addDialogOpen" :options="addDialogOptions">
    <InfoDialog @close="onDialogClose" />
  </my-dialog>

  <my-dialog v-model="editDialogOpen" :options="editDialogOptions">
    <InfoDialog :detail="detailInfo" @close="onDialogClose" />
  </my-dialog>
</template>

<script setup>
import {
  Plus,
  Edit,
  Delete,
  Upload,
  Download,
  Bottom,
} from "@element-plus/icons-vue";
import ColumnTools from "@/components/columnTools.vue";
import InfoDialog from "./Dialog.vue";

import { ref, markRaw, onBeforeMount, getCurrentInstance } from "vue";
import { showError, showSuccess, showConfirm } from "@/utils/elBox";
import http from "@/utils/request";
import * as XLSX from "xlsx";

const baseUrl = process.env.VUE_APP_HOST;
const addDialogOpen = ref(false);
const editDialogOpen = ref(false);

const uploadHeaders = ref({});
const uploadAction = baseUrl + "admin/Student/UploadExcel";
const downloadHref = baseUrl + "admin/Student/DownloadExcel";

const total = ref(0);
const tableRef = ref();
const pageData = ref([]);
const detailInfo = ref();
const showProgress = ref(false);
const addDialogOptions = ref({
  showMaximize: false,
  icon: markRaw(Plus),
  title: "添加",
  width: 500,
});
const editDialogOptions = ref({
  showMaximize: false,
  icon: markRaw(Edit),
  title: "编辑",
  width: 500,
});
const searchForm = ref({
  Page: 1,
  Take: 30,
  Keyword: "",
  SchoolId: "",
  Grade: "",
  Gender: "",
});

const loadPage = () => {
  var url = "/admin/Student/Page";
  http.post(url, searchForm.value).then((res) => {
    if (res.IsSucc) {
      total.value = res.Total;
      pageData.value = res.Data;
    }
  });
};

const exportExcel = () => {
  const tableDom = tableRef.value?.$el;
  if (!tableDom) {
    return;
  }

  const wb = XLSX.utils.table_to_book(tableDom);
  XLSX.writeFile(wb, "学生数据.xlsx");
};

const deleteRow = (row) => {
  const ids = [];
  if (row) {
    ids.push(row.Id);
  } else {
    const rows = tableRef.value.getSelectionRows();
    for (const item of rows) {
      ids.push(item.Id);
    }
  }

  if (ids.length <= 0) {
    return;
  }

  showConfirm("确认要删除这些记录？", "操作确认", {
    type: "warning",
  })
    .then(() => {
      var url = "?id=" + row.Id;
      http.delete("/admin/Student/Delete", { data: ids }).then((res) => {
        if (res.IsSucc) {
          showSuccess(res.Msg);
          loadPage();
        } else {
          showError(res.Msg);
        }
      });
    })
    .catch((err) => {
      console.log(err);
    });
};

const onDialogClose = (state) => {
  if (state) {
    loadPage();
  }
  addDialogOpen.value = false;
  editDialogOpen.value = false;
};

const openEdit = (row) => {
  detailInfo.value = row;
  editDialogOpen.value = true;
};

const beforeUpload = () => {
  showProgress.value = true;
  return true;
};

const uploadSuccess = (res) => {
  showProgress.value = false;
  if (res.IsSucc) {
    loadPage();
    showSuccess(res.Msg);
  } else {
    showError(res.Msg);
  }
};

const uploadError = (error) => {
  showProgress.value = false;
  showError(error.message);
  console.error(error);
};

onBeforeMount(() => {
  const instance = getCurrentInstance();
  if (instance) {
    const { proxy } = instance;
    uploadHeaders.value = {
      "X-Token": proxy.$store.getters.token,
      "X-ClientType": proxy.$store.getters.clientType,
    };
  }

  loadPage();
});
</script>

<script>
export default {
  name: "StudentPage",
};
</script>

<style lang="scss" scoped>
.inline-box {
  height: 32px;
  display: inline-flex;
  border-radius: var(--el-border-radius-base);
  background-color: var(--el-color-primary);
}

.upload-btn {
  padding: 0px 8px;
  border-right: 1px solid rgba(255, 255, 255, 0.5);

  .btn-body {
    display: flex;
    color: white;
    align-items: center;
    justify-content: center;
  }
}

.download-btn {
  width: 30px;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
