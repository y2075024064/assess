<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="Plus" @click="addDialogOpen = true">
          添加
        </el-button>
      </el-form-item>

      <el-form-item label="字典键">
        <el-select v-model="searchForm.Key" clearable style="width: 250px">
          <el-option
            v-for="(item, index) in dictionaryTypes"
            v-bind="item"
            :key="index"
          />
        </el-select>
      </el-form-item>

      <el-form-item label="字典值">
        <search-box
          v-model="searchForm.Keyword"
          placeholder="请输入字典值"
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
      <el-table-column label="字典键" prop="Key" width="250" />
      <el-table-column label="字典值" prop="Value" />
      <el-table-column fixed="right" label="操作" align="center" width="100">
        <template #default="scope">
          <ColumnTools
            :show-info="false"
            @on-del="deleteRow(scope.row)"
            @on-edit="openEdit(scope.row)"
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
import { dictionaryTypes } from "@/configs/dictionary";
import { ref, markRaw, onBeforeMount } from "vue";
import http from "@/utils/request";

const total = ref(0);
const pageData = ref([]);
const detailInfo = ref();
const addDialogOpen = ref(false);
const editDialogOpen = ref(false);
const addDialogOptions = ref({
  showMaximize: false,
  icon: markRaw(Plus),
  title: "添加",
  width: 400,
});
const editDialogOptions = ref({
  showMaximize: false,
  icon: markRaw(Edit),
  title: "编辑",
  width: 400,
});
const searchForm = ref({
  Page: 1,
  Take: 30,
  Key: "",
  Keyword: "",
});

const loadPage = () => {
  var url = "/admin/Dictionary/Page";
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
      var url = "/admin/Dictionary/Delete?id=" + row.Id;
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
  name: "DictionaryPage",
};
</script>
