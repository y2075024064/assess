<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="plusIcon" @click="openAdd">
          添加
        </el-button>
      </el-form-item>

      <el-form-item label="动态类型">
        <el-select v-model="searchForm.Type" clearable style="width: 120px">
          <el-option value="系统" label="系统" />
          <el-option value="行业" label="行业" />
          <el-option value="新闻" label="新闻" />
          <el-option value="活动" label="活动" />
          <el-option value="用户" label="用户" />
          <el-option value="用户转发" label="用户转发" />
        </el-select>
      </el-form-item>

      <el-form-item label="关键字">
        <search-box
          v-model="searchForm.Keyword"
          placeholder="标题、内容、备注"
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
      <el-table-column label="标题" prop="Title" width="250" />
      <el-table-column label="类型" prop="Type" width="90" />
      <el-table-column label="发布时间" prop="PublishTime" width="180" />
      <el-table-column label="创建时间" prop="CreationTime" width="180" />
      <el-table-column label="修改时间" prop="ModificationTime" width="180" />
      <el-table-column label="原文链接" prop="OriginalHref" width="200" />
      <el-table-column label="备注" prop="Remark" width="150" />
      <el-table-column fixed="right" label="操作" align="center" width="140">
        <template #default="scope">
          <ColumnTools
            @on-info="openDetail(scope.row)"
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

  <el-drawer
    v-model="showDetail"
    :with-header="false"
    direction="ltr"
    :size="580"
  >
    <el-descriptions
      class="assess-detail-box"
      title="动态详情"
      :column="1"
      border
    >
      <el-descriptions-item label="类型" label-align="right">
        {{ detailInfo.Type }}
      </el-descriptions-item>
      <el-descriptions-item label="标题" label-align="right">
        {{ detailInfo.Title }}
      </el-descriptions-item>
      <el-descriptions-item label="预览图片" label-align="right">
        <el-image :src="detailInfo.PreviewImg" fit="contain" />
      </el-descriptions-item>
      <el-descriptions-item label="发布时间" label-align="right">
        {{ detailInfo.PublishTime }}
      </el-descriptions-item>
      <el-descriptions-item label="创建时间" label-align="right">
        {{ detailInfo.CreationTime }}
      </el-descriptions-item>
      <el-descriptions-item label="编辑时间" label-align="right">
        {{ detailInfo.ModificationTime }}
      </el-descriptions-item>
      <el-descriptions-item label="动态内容" label-align="right">
        <div v-html="detailInfo.Content"></div>
      </el-descriptions-item>
      <el-descriptions-item label="原文链接" label-align="right">
        {{ detailInfo.OriginalHref }}
      </el-descriptions-item>
      <el-descriptions-item label="备注" label-align="right">
        {{ detailInfo.Remark }}
      </el-descriptions-item>
    </el-descriptions>
  </el-drawer>
</template>

<script>
import { markRaw } from "vue";
import { Plus, Delete, Edit } from "@element-plus/icons-vue";
import ColumnTools from "@/components/columnTools.vue";
import InfoDialog from "./Dialog.vue";

export default {
  name: "DynamicPage",
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
      showDetail: false,
      addDialogOpen: false,
      editDialogOpen: false,
      addDialogOptions: {
        showMaximize: true,
        icon: plusIcon,
        title: "添加",
        width: 700,
      },
      editDialogOptions: {
        showMaximize: true,
        icon: editIcon,
        title: "编辑",
        width: 700,
      },
      searchForm: {
        Page: 1,
        Take: 30,
        Type: "",
        Keyword: "",
      },
    };
  },
  mounted() {
    this.loadPage();
  },
  methods: {
    loadPage() {
      var url = "/admin/Dynamic/Page";
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
      this.getDetail(row.Id, () => (this.editDialogOpen = true));
    },
    openDetail(row) {
      this.getDetail(row.Id, () => (this.showDetail = true));
    },
    getDetail(id, callback) {
      this.$http.get("/admin/Dynamic/Detail?id=" + id).then((res) => {
        if (res.IsSucc) {
          this.detailInfo = res.Data;
          callback();
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
    deleteRow(row) {
      this.$confirm("确认要删除该条记录？", "操作确认", {
        type: "warning",
      }).then(() => {
        var url = "admin/Dynamic/Delete?id=" + row.Id;
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

<style lang="scss">
.assess-detail-box {
  table {
    tr {
      td:first-child {
        width: 82px;
      }
    }
  }
}
</style>
