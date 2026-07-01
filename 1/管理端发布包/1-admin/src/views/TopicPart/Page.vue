<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="plusIcon" @click="openAdd">
          添加根节点
        </el-button>
      </el-form-item>

      <el-form-item>
        <el-button type="primary" :icon="plusIcon" @click="openSubAdd">
          添加子节点
        </el-button>
      </el-form-item>

      <el-form-item>
        <el-divider direction="vertical" style="height: 32px" />
      </el-form-item>

      <el-form-item label="关键字" label-width="55">
        <search-box
          v-model="searchForm.Keyword"
          placeholder="名称、备注"
          @search="loadPage"
          width="250px"
        />
      </el-form-item>
    </el-form>

    <el-table
      :tree-props="{ children: 'Children', hasChildren: 'HasChildren' }"
      highlight-current-row
      :load="loadChildren"
      class="pager-table"
      table-layout="auto"
      :data="pageData"
      ref="pageTable"
      row-key="Id"
      stripe
      border
      lazy
      @sort-change="sortChange"
      @current-change="handleCurrentChange"
    >
      <el-table-column type="index" fixed="left" />
      <el-table-column label="名称" prop="Name" sortable="custom" width="150" />
      <el-table-column label="ID" prop="Id" sortable="custom" width="100" />
      <el-table-column label="原名称" prop="OldName" width="150" />
      <el-table-column
        label="排序级别"
        prop="SortLevel"
        align="center"
        width="100"
      />
      <el-table-column label="推荐年级" prop="RcmdGrade" width="200" />
      <el-table-column
        label="最近一次修改时间"
        prop="ModificationTime"
        sortable="custom"
        width="200"
      />
      <el-table-column label="备注" prop="Remark" />
      <el-table-column fixed="right" label="操作" align="right" width="165">
        <template #default="scope">
          <ColumnTools
            info-text="测评方案"
            :show-info="!scope.row.ParentId"
            @on-del="deleteRow(scope.row)"
            @on-info="openPlan(scope.row)"
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
    <InfoDialog @close="onDialogClose" :parent-id="currentParentId" />
  </my-dialog>

  <my-dialog v-model="planDialogOpen" :options="planDialogOptions">
    <TestPlan @close="onDialogClose" :part-id="currentRowId" />
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
import { Plus, Delete, Edit, Files } from "@element-plus/icons-vue";
import ColumnTools from "@/components/columnTools.vue";
import TestPlan from "./TestPlan.vue";
import InfoDialog from "./Dialog.vue";

export default {
  name: "TopicPart",
  components: { InfoDialog, TestPlan, ColumnTools },
  data() {
    var editIcon = markRaw(Edit);
    var plusIcon = markRaw(Plus);
    var filesIcon = markRaw(Files);
    var deleteIcon = markRaw(Delete);
    return {
      editIcon,
      plusIcon,
      filesIcon,
      deleteIcon,
      total: 0,
      pageData: [],
      detailInfo: null,
      currentRow: null,
      currentRowId: 0,
      addDialogOpen: false,
      editDialogOpen: false,
      planDialogOpen: false,
      currentParentId: null,
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
      planDialogOptions: {
        showMaximize: false,
        icon: filesIcon,
        title: "测评方案",
      },
      searchForm: {
        Page: 1,
        Take: 30,
        OrderRule: "Asc",
        OrderField: "SortLevel",
      },
    };
  },
  mounted() {
    this.loadPage();
  },
  methods: {
    loadPage() {
      var url = "/admin/TopicPart/Page";
      this.$http.post(url, this.searchForm).then((res) => {
        if (res.IsSucc) {
          this.total = res.Total;
          this.pageData = res.Data;
        }
      });
    },
    loadChildren(row, treeNode, resolve) {
      var url = "admin/TopicPart/Childrens?id=" + row.Id;
      this.$http.get(url).then((res) => {
        if (res.IsSucc) {
          resolve(res.Data);
        }
      });
    },
    openAdd() {
      this.currentParentId = null;
      this.addDialogOpen = true;
    },
    openSubAdd() {
      if (this.currentRow) {
        this.currentParentId = this.currentRow.Id;
        this.addDialogOpen = true;
      }
    },
    openEdit(row) {
      this.detailInfo = row;
      this.editDialogOpen = true;
    },
    openPlan(row) {
      this.currentRowId = row.Id;
      this.planDialogOpen = true;
    },
    deleteRow(row) {
      this.$confirm("确认要删除该节点及其所有子节点？", "操作确认", {
        type: "warning",
      })
        .then(() => {
          var url = "admin/TopicPart/Delete?id=" + row.Id;
          this.$http.delete(url).then((res) => {
            if (res.IsSucc) {
              this.$message.success(res.Msg);
              this.loadPage();
            } else {
              this.$message.error(res.Msg);
            }
          });
        })
        .catch(() => {});
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
      if (this.planDialogOpen) {
        this.planDialogOpen = false;
      }
    },
    sortChange(col) {
      this.searchForm.OrderField = col.prop;
      this.searchForm.OrderRule = col.order === "ascending" ? "Asc" : "Desc";
      this.loadPage();
    },
    handleCurrentChange(row) {
      this.currentRow = row;
    },
  },
};
</script>
