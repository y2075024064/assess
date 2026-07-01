<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="Plus" @click="addDialogOpen = true">
          添加
        </el-button>
      </el-form-item>

      <el-form-item label="编号">
        <el-input
          v-model="searchForm.Number"
          placeholder="请输入考试编号"
          style="width: 150px"
          clearable
        />
      </el-form-item>

      <el-form-item label="是否开放">
        <el-select
          v-model="searchForm.IsPublic"
          placeholder="请选择"
          style="width: 90px"
          clearable
        >
          <el-option label="是" :value="true" />
          <el-option label="否" :value="false" />
        </el-select>
      </el-form-item>

      <el-form-item label="日期范围">
        <el-date-picker
          start-placeholder="开始日期"
          end-placeholder="结束日期"
          value-format="YYYY-MM-DD"
          style="width: 230px"
          v-model="timeRange"
          range-separator="-"
          type="daterange"
          @change="timeRangChange"
        />
      </el-form-item>

      <el-form-item label="关键字">
        <search-box
          v-model="searchForm.Keyword"
          placeholder="名称、目的、对象、备注"
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
      <el-table-column label="编号" prop="Number" width="150" />
      <el-table-column label="名称" prop="Name" width="250" />
      <el-table-column label="总分" prop="TotalScore" width="70" />
      <el-table-column align="center" prop="IsPublic" width="90">
        <template #header>
          <div title="是否开放该考试">是否开放</div>
        </template>
        <template #default="{ row }">
          <el-switch
            v-model="row.IsPublic"
            inactive-text="否"
            active-text="是"
            inline-prompt
            width="50"
            @change="updateField(row, 'IsPublic', row.IsPublic)"
          />
        </template>
      </el-table-column>
      <el-table-column
        label="目的"
        prop="Purpose"
        min-width="250"
        show-overflow-tooltip
      />
      <el-table-column label="开始时间" prop="StartTime" width="170" />
      <el-table-column label="时长" prop="Duration" width="60" />
      <el-table-column label="报名人数" prop="Count" width="90" />
      <el-table-column label="开始日期" prop="StartDate" width="110" />
      <el-table-column label="结束日期" prop="EndDate" width="110" />
      <el-table-column label="备注" prop="Remark" width="200" />
      <el-table-column fixed="right" label="操作" align="center" width="140">
        <template #default="{ row }">
          <ColumnTools
            info-text="题目"
            @on-info="openTopics(row)"
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

  <el-drawer v-model="showDetail" title="题目管理" direction="ltr" size="100%">
    <Topics :id="detailInfo.Id" />
  </el-drawer>
</template>

<script setup>
import { Plus, Edit } from "@element-plus/icons-vue";
import ColumnTools from "@/components/columnTools.vue";
import InfoDialog from "./Dialog.vue";
import Topics from "./Topics.vue";

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
  Number: "",
  Keyword: "",
  EndTime: "",
  StartTime: "",
  IsPublic: "",
});

const timeRangChange = (val) => {
  searchForm.value.StartTime = val[0];
  searchForm.value.EndTime = val[1];
};

const loadPage = () => {
  var url = "/admin/Exam/Page";
  http.post(url, searchForm.value).then((res) => {
    if (res.IsSucc) {
      total.value = res.Total;
      pageData.value = res.Data;
    }
  });
};

const updateField = (row, field, value) => {
  var url = "/admin/Exam/ModifySingleField";
  var params = { id: row.Id, field, value };
  http.get(url, { params }).then((res) => {
    if (res.IsSucc) {
      showSuccess(res.Msg);
    } else {
      showError(res.Msg);
    }
  });
};

const deleteRow = (row) => {
  showConfirm("确认要删除该条记录？", "操作确认", {
    type: "warning",
  })
    .then(() => {
      var url = "/admin/Exam/Delete?id=" + row.Id;
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

const openTopics = (row) => {
  detailInfo.value = row;
  showDetail.value = true;
};

onBeforeMount(() => {
  loadPage();
});
</script>

<script>
export default {
  name: "ExamPage",
};
</script>
