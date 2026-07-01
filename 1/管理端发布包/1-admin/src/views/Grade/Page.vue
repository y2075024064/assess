<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="plusIcon" @click="openAdd">
          添加
        </el-button>
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
      <el-table-column label="名称" prop="Name" width="200" />
      <el-table-column
        label="排序级别"
        prop="SortLevel"
        align="center"
        width="100"
      />
      <el-table-column align="center" prop="IsUse" width="100">
        <template #header>
          <div title="是否在程序中使用该年级参与记录用户信息">是否使用</div>
        </template>
        <template #default="scope">
          <el-switch
            v-model="scope.row.IsUse"
            inactive-text="否"
            active-text="是"
            inline-prompt
            width="50"
            @change="updateField(scope.row, 1, scope.row.IsUse)"
          />
        </template>
      </el-table-column>
      <el-table-column align="center" prop="IsShow" width="100">
        <template #header>
          <div title="是否在练习界面显示该年级分类项">是否显示</div>
        </template>
        <template #default="scope">
          <el-switch
            v-model="scope.row.IsShow"
            inactive-text="否"
            active-text="是"
            inline-prompt
            width="50"
            @change="updateField(scope.row, 0, scope.row.IsShow)"
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
  name: "GradeInfo",
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
      },
      editDialogOptions: {
        showMaximize: false,
        icon: editIcon,
        title: "编辑",
      },
      searchForm: {
        Page: 1,
        Take: 30,
      },
    };
  },
  mounted() {
    this.loadPage();
  },
  methods: {
    loadPage() {
      var url = "/admin/Grade/Page";
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
    updateField(row, type, value) {
      var url = "admin/Grade/UpdateField";
      var params = { id: row.Id, type: type, value: value };
      this.$http.get(url, { params }).then((res) => {
        if (res.IsSucc) {
          this.$message.success(res.Msg);
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
    deleteRow(row) {
      this.$confirm("确认要删除该年级信息？", "操作确认", {
        type: "warning",
      }).then(() => {
        var url = "admin/Grade/Delete?id=" + row.Id;
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
