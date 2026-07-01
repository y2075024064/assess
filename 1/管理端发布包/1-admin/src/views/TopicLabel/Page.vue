<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="plusIcon" @click="openAdd">
          添加
        </el-button>
      </el-form-item>

      <el-form-item>
        <el-button type="danger" :icon="deleteIcon" @click="deleteLot">
          批量删除
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
      highlight-current-row
      class="pager-table"
      table-layout="auto"
      :data="pageData"
      ref="pageTable"
      stripe
      border
      @sort-change="sortChange"
    >
      <el-table-column type="selection" fixed="left" width="40" />
      <el-table-column type="index" fixed="left" />
      <el-table-column label="名称" prop="Name" sortable="custom" width="150" />
      <el-table-column label="原名称" prop="OldName" width="150" />
      <el-table-column
        label="最近一次修改时间"
        prop="ModificationTime"
        sortable="custom"
        width="200"
      />
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
  name: "TopicLabel",
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
        OrderField: "",
        OrderRule: "Asc",
      },
    };
  },
  mounted() {
    this.loadPage();
  },
  methods: {
    loadPage() {
      var url = "/admin/TopicLabel/Page";
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
    deleteLot() {
      var rows = this.$refs.pageTable.getSelectionRows();
      if (rows && rows.length > 0) {
        var ids = [];
        rows.forEach((item) => ids.push(item.Id));
        this.$confirm("确认要删除选中的题目标签？", "操作确认", {
          type: "warning",
        })
          .then(() => {
            var url = "admin/TopicLabel/Delete";
            this.$http.delete(url, { data: ids }).then((res) => {
              if (res.IsSucc) {
                this.$message.success(res.Msg);
                this.loadPage();
              } else {
                this.$message.error(res.Msg);
              }
            });
          })
          .catch(() => {});
      }
    },
    openEdit(row) {
      this.detailInfo = row;
      this.editDialogOpen = true;
    },
    deleteRow(row) {
      this.$confirm("确认要删除该题目标签？", "操作确认", {
        type: "warning",
      })
        .then(() => {
          var url = "admin/TopicLabel/Delete";
          this.$http.delete(url, { data: [row.Id] }).then((res) => {
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
    },
    sortChange(col) {
      this.searchForm.OrderField = col.prop;
      this.searchForm.OrderRule = col.order === "ascending" ? "Asc" : "Desc";
      this.loadPage();
    },
  },
};
</script>
