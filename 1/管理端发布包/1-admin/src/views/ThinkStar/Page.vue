<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="Plus" @click="addDialogOpen = true">
          添加
        </el-button>
      </el-form-item>

      <el-form-item label="类型">
        <my-select
          v-model="searchForm.Type"
          url="/admin/Dictionary/Select?key=ThinkStarTtype"
          style="width: 150px"
        />
      </el-form-item>

      <el-form-item label="时间范围" label-width="70">
        <el-date-picker
          value-format="YYYY-MM-DD HH:mm:ss"
          start-placeholder="开始时间"
          end-placeholder="结束时间"
          style="width: 360px"
          type="datetimerange"
          range-separator="-"
          v-model="timeRange"
          @change="timeRangChange"
        />
      </el-form-item>

      <el-form-item label="关键字">
        <search-box
          v-model="searchForm.Keyword"
          placeholder="标题、备注"
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
      <el-table-column label="用户账户" prop="Account" width="150" />
      <el-table-column label="用户姓名" prop="Name" width="100" />
      <el-table-column label="类型" prop="Type" width="100" />
      <el-table-column label="标题" prop="Title" width="250" />
      <el-table-column
        label="内容"
        prop="Content"
        min-width="250"
        show-overflow-tooltip
      />
      <el-table-column label="创建时间" prop="CreationTime" width="170" />
      <el-table-column label="备注" prop="Remark" width="150" />
      <el-table-column fixed="right" label="操作" align="center" width="140">
        <template #default="{ row }">
          <ColumnTools
            @on-info="openDetail(row)"
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
    <InfoDialog :id="detailInfo.Id" @close="onDialogClose" />
  </my-dialog>

  <el-drawer
    v-model="showDetail"
    :with-header="false"
    direction="ltr"
    :size="580"
  >
    <Detail :id="detailInfo.Id" />
  </el-drawer>
</template>

<script setup>
import ColumnTools from "@/components/columnTools.vue";
import { Plus, Edit } from "@element-plus/icons-vue";
import InfoDialog from "./Dialog.vue";
import Detail from "./Detail.vue";

import { showError, showSuccess, showConfirm } from "@/utils/elBox";
import { ref, markRaw, onBeforeMount } from "vue";
import http from "@/utils/request";

const showDetail = ref(false);
const addDialogOpen = ref(false);
const editDialogOpen = ref(false);

const total = ref(0);
const pageData = ref([]);
const detailInfo = ref();
const timeRange = ref(["", ""]);
const addDialogOptions = ref({
  showMaximize: true,
  icon: markRaw(Plus),
  title: "添加",
  width: 700,
});
const editDialogOptions = ref({
  showMaximize: true,
  icon: markRaw(Edit),
  title: "编辑",
  width: 700,
});
const searchForm = ref({
  Page: 1,
  Take: 30,
  Type: "",
  Keyword: "",
  EndTime: "",
  StartTime: "",
});

const timeRangChange = (val) => {
  searchForm.value.StartTime = val[0];
  searchForm.value.EndTime = val[1];
};

const loadPage = () => {
  var url = "/admin/ThinkStar/Page";
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
      var url = "/admin/ThinkStar/Delete?id=" + row.Id;
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

const openDetail = (row) => {
  detailInfo.value = row;
  showDetail.value = true;
};

onBeforeMount(() => {
  loadPage();
});
</script>

<script>
export default {
  name: "ThinkStarPage",
};
</script>
