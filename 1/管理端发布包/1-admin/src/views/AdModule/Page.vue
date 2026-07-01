<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="plusIcon" @click="openAdd">
          添加
        </el-button>
      </el-form-item>

      <el-form-item label="关键字">
        <search-box
          v-model="searchForm.Keyword"
          placeholder="名称、标识、地点、备注"
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
      <el-table-column label="名称" prop="Name" width="150" />
      <el-table-column label="标识" prop="ModuleName" width="150" />
      <el-table-column label="地点" prop="Location" width="250" />

      <el-table-column align="center" prop="IsEnable" width="100">
        <template #header>
          <div title="是否在客户端启用该广告模块">是否启用</div>
        </template>
        <template #default="scope">
          <el-switch
            v-model="scope.row.IsEnable"
            inactive-text="否"
            active-text="是"
            inline-prompt
            width="50"
            @change="updateField(scope.row, 'IsEnable', scope.row.IsEnable)"
          />
        </template>
      </el-table-column>

      <el-table-column align="center" prop="IsCarousel" width="100">
        <template #header>
          <div title="是否在客户端启用自动轮播">自动轮播</div>
        </template>
        <template #default="scope">
          <el-switch
            v-model="scope.row.IsCarousel"
            inactive-text="否"
            active-text="是"
            inline-prompt
            width="50"
            @change="updateField(scope.row, 'IsCarousel', scope.row.IsCarousel)"
          />
        </template>
      </el-table-column>

      <el-table-column label="备注" prop="Remark" />
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
    <InfoDialog
      :detail="detailInfo"
      ref="editInfoDialog"
      @close="onDialogClose"
    />
  </my-dialog>
</template>

<script>
import { markRaw } from "vue";
import { Plus, Delete, Edit } from "@element-plus/icons-vue";
import ColumnTools from "@/components/columnTools.vue";
import InfoDialog from "./Dialog.vue";

export default {
  name: "AdModulePage",
  components: { InfoDialog, ColumnTools },
  data() {
    var editIcon = markRaw(Edit);
    var plusIcon = markRaw(Plus);
    var deleteIcon = markRaw(Delete);
    return {
      editIcon,
      plusIcon,
      deleteIcon,
      total: 0,
      pageData: [],
      detailInfo: null,
      addDialogOpen: false,
      editDialogOpen: false,
      addDialogOptions: {
        showMaximize: false,
        icon: plusIcon,
        title: "添加",
        width: 500,
      },
      editDialogOptions: {
        showMaximize: false,
        icon: editIcon,
        title: "编辑",
        width: 500,
      },
      searchForm: {
        Page: 1,
        Take: 30,
        Keyword: "",
      },
    };
  },
  mounted() {
    this.loadPage();
  },
  methods: {
    loadPage() {
      var url = "/admin/AdModule/Page";
      this.$http.post(url, this.searchForm).then((res) => {
        if (res.IsSucc) {
          this.total = res.Total;
          this.pageData = res.Data;
        }
      });
    },
    openAdd() {
      this.addDialogOpen = true;
    },
    openEdit(row) {
      this.detailInfo = row;
      this.editDialogOpen = true;
    },
    updateField(row, field, value) {
      var url = "admin/AdModule/ModifySingleField";
      var params = { id: row.Id, field, value };
      this.$http.get(url, { params }).then((res) => {
        if (res.IsSucc) {
          this.$message.success(res.Msg);
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
    deleteRow(row) {
      this.$confirm("确认要删除该条记录？", "操作确认", {
        type: "warning",
      }).then(() => {
        var url = "admin/AdModule/Delete?id=" + row.Id;
        this.$http.delete(url).then((res) => {
          if (res.IsSucc) {
            this.$message.success(res.Msg);
            this.loadPage();
          } else {
            this.$message.error(res.Msg);
          }
        });
      });
    },
    onDialogClose(state) {
      if (state) {
        this.loadPage();
      }
      if (this.addDialogOpen) {
        this.addDialogOpen = false;
      }
      if (this.editDialogOpen) {
        this.editDialogOpen = false;
      }
    },
  },
};
</script>
