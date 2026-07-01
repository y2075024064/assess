<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="Plus" @click="addDialogOpen = true">
          添加
        </el-button>
      </el-form-item>

      <el-form-item label="学校类型">
        <el-select
          v-model="searchForm.SchoolType"
          placeholder="请选择"
          style="width: 120px"
          clearable
        >
          <el-option label="小学" value="小学" />
          <el-option label="初中" value="初中" />
          <el-option label="高中" value="高中" />
          <el-option label="大学" value="大学" />
        </el-select>
      </el-form-item>

      <el-form-item label="关键字">
        <search-box
          v-model="searchForm.Keyword"
          placeholder="名称、简介、校长、备注"
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
      stripe
      border
    >
      <el-table-column type="index" fixed="left" />
      <el-table-column label="Logo" prop="Logo" width="80">
        <template #default="{ row }">
          <el-image
            style="width: 40px; height: 40px"
            :src="row.Logo"
            fit="contain"
          />
        </template>
      </el-table-column>
      <el-table-column label="账户名" prop="Account" width="120" />
      <el-table-column label="简称" prop="Name" width="150" />
      <el-table-column label="类型" prop="SchoolType" width="60" />
      <el-table-column label="全称" prop="FullName" width="250" />
      <el-table-column label="校长" prop="Rector" width="75" />
      <el-table-column label="联系电话" prop="PhoneNumber" width="120" />
      <el-table-column label="学校邮箱" prop="Email" width="180" />
      <el-table-column label="学校地址" prop="Address" width="220" />
      <el-table-column label="学校简介" prop="Intro" min-width="200" />
      <el-table-column label="创建时间" prop="CreationTime" width="170" />
      <el-table-column label="修改时间" prop="ModificationTime" width="170" />
      <el-table-column label="备注" prop="Remark" width="150" />
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

  <my-dialog v-model="addDialogOpen" :options="addDialogOptions">
    <InfoDialog @close="onDialogClose" />
  </my-dialog>

  <my-dialog v-model="editDialogOpen" :options="editDialogOptions">
    <InfoDialog :detail="detailInfo" @close="onDialogClose" />
  </my-dialog>
</template>

<script setup>
import { Plus, Edit } from "@element-plus/icons-vue";
import ColumnTools from "@/components/columnTools.vue";
import InfoDialog from "./Dialog.vue";

import { showError, showSuccess, showConfirm } from "@/utils/elBox";
import { ref, markRaw, onBeforeMount } from "vue";
import http from "@/utils/request";

const addDialogOpen = ref(false);
const editDialogOpen = ref(false);

const total = ref(0);
const pageData = ref([]);
const detailInfo = ref();
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
  SchoolType: "",
});

const loadPage = () => {
  var url = "/admin/School/Page";
  http.post(url, searchForm.value).then((res) => {
    if (res.IsSucc) {
      total.value = res.Total;
      pageData.value = res.Data;
    }
  });
};

const deleteRow = (row) => {
  showConfirm("确认要删除该条记录？", "操作确认", {
    type: "warning",
  })
    .then(() => {
      var url = "/admin/School/Delete?id=" + row.Id;
      http.delete(url).then((res) => {
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

onBeforeMount(() => {
  loadPage();
});
</script>

<script>
export default {
  name: "SchoolPage",
};
</script>
