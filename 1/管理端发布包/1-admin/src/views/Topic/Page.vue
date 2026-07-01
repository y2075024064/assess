<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="plusIcon" @click="openAdd">
          添加
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

      <el-form-item label="创建用户">
        <el-input
          v-model="searchForm.MemberName"
          placeholder="用户的真实姓名"
          style="width: 120px"
        />
      </el-form-item>

      <el-form-item label="类型">
        <el-select v-model="searchForm.Type" clearable style="width: 90px">
          <el-option label="单选" value="单选" />
          <el-option label="多选" value="多选" />
          <el-option label="填空" value="填空" />
          <el-option label="编程" value="编程" />
        </el-select>
      </el-form-item>

      <el-form-item label="用途">
        <el-select v-model="searchForm.UseType" clearable style="width: 90px">
          <el-option label="通用" value="通用" />
          <el-option label="练习" value="练习" />
          <el-option label="评测" value="评测" />
        </el-select>
      </el-form-item>

      <el-form-item label="所属专项">
        <el-tree-select
          :props="{ label: 'Name', isLeaf: 'IsLeaf' }"
          :remote-method="remoteMethod"
          :loading="treeSelectLoading"
          :render-after-expand="false"
          v-model="searchForm.PartId"
          :reserve-keyword="false"
          :data="treeSelectData"
          style="width: 250px"
          remote-show-suffix
          :load="remoteLoad"
          check-strictly
          value-key="Id"
          filterable
          clearable
          remote
          lazy
        />
      </el-form-item>

      <el-form-item label="关键字">
        <search-box
          v-model="searchForm.Keyword"
          placeholder="编号、标签、目的、内容、备注"
          @search="loadPage"
          width="250px"
        />
      </el-form-item>
    </el-form>

    <el-table
      highlight-current-row
      show-overflow-tooltip
      class="pager-table"
      table-layout="auto"
      :data="pageData"
      ref="pageTable"
      stripe
      border
      @sort-change="sortChange"
    >
      <el-table-column type="index" fixed="left" />
      <el-table-column
        label="编号"
        prop="Number"
        sortable="custom"
        width="150"
      />
      <el-table-column label="题目" prop="Title" width="150" />
      <el-table-column label="类型" prop="Type" width="60" />
      <el-table-column label="用途" prop="UseType" width="60" />
      <el-table-column label="难度" prop="Level" sortable="custom" width="78" />
      <el-table-column label="原创" prop="IsOriginal" width="60" align="center">
        <template #default="scope">
          {{ scope.IsOriginal ? "是" : "否" }}
        </template>
      </el-table-column>
      <el-table-column label="内容" prop="Content" width="250" />
      <el-table-column label="目的" prop="Purpose" width="200" />
      <el-table-column
        label="评论数"
        prop="CommentNum"
        sortable="custom"
        width="100"
      />
      <el-table-column
        label="收藏数"
        prop="CollectionNum"
        sortable="custom"
        width="100"
      />
      <el-table-column
        label="创建时间"
        prop="CreationTime"
        sortable="custom"
        width="170"
      />
      <el-table-column
        label="最近一次修改时间"
        prop="ModificationTime"
        sortable="custom"
        width="170"
      />
      <el-table-column label="备注" prop="Remark" sortable="custom" />
      <el-table-column fixed="right" label="操作" align="center" width="140">
        <template #default="scope">
          <ColumnTools
            @on-del="deleteRow(scope.row)"
            @on-edit="openEdit(scope.row)"
            @on-info="openDetail(scope.row)"
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

  <el-drawer
    v-model="detailDrawerOpen"
    :with-header="false"
    direction="ltr"
    :size="580"
  >
    <Detail :detail="topicDetail" />
  </el-drawer>
</template>

<script>
import { markRaw } from "vue";
import { Plus, Delete, Edit, Download, Upload } from "@element-plus/icons-vue";
import ColumnTools from "@/components/columnTools.vue";
import InfoDialog from "./Dialog.vue";
import Detail from "./Detail.vue";

export default {
  name: "TopicInfo",
  components: { InfoDialog, Detail, ColumnTools, Download, Upload },
  data() {
    var editIcon = markRaw(Edit);
    var plusIcon = markRaw(Plus);
    var deleteIcon = markRaw(Delete);
    var uploadIcon = markRaw(Upload);
    var baseUrl = process.env.VUE_APP_HOST;
    return {
      editIcon,
      plusIcon,
      deleteIcon,
      uploadIcon,
      total: 0,
      pageData: [],
      topicDetail: {},
      detailInfo: null,
      treeSelectData: [],
      showProgress: false,
      addDialogOpen: false,
      editDialogOpen: false,
      detailDrawerOpen: false,
      treeSelectLoading: false,
      addDialogOptions: {
        icon: plusIcon,
        title: "添加",
        width: 755,
      },
      editDialogOptions: {
        icon: editIcon,
        title: "编辑",
        width: 755,
      },
      searchForm: {
        Page: 1,
        Take: 30,
        Type: null,
        PartId: null,
        UseType: null,
        MemberName: "",
        OrderField: "",
        OrderRule: "Asc",
      },
      uploadHeaders: {
        "X-Token": this.$store.getters.token,
        "X-ClientType": this.$store.getters.clientType,
      },
      uploadAction: baseUrl + "admin/Topic/UploadExcel",
      downloadHref: baseUrl + "admin/Topic/DownloadExcel",
    };
  },
  mounted() {
    this.loadPage();
  },
  methods: {
    loadPage() {
      var url = "/admin/Topic/Page";
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
    openDetail(row) {
      var url = "admin/Topic/Detail?id=" + row.Id;
      this.$http.get(url).then((res) => {
        if (res.IsSucc) {
          this.topicDetail = res.Data;
          this.detailDrawerOpen = true;
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
    deleteRow(row) {
      this.$confirm("确认要删除该题目？", "操作确认", {
        type: "warning",
      })
        .then(() => {
          var url = "admin/Topic/Delete?id=" + row.Id;
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
    },
    sortChange(col) {
      this.searchForm.OrderField = col.prop;
      this.searchForm.OrderRule = col.order === "ascending" ? "Asc" : "Desc";
      this.loadPage();
    },
    remoteMethod(q) {
      this.treeSelectLoading = true;
      var url = "admin/TopicPart/Select?q=" + q;
      this.$http.get(url).then((res) => {
        this.treeSelectLoading = false;
        if (res.IsSucc) {
          this.treeSelectData = res.Data;
        }
      });
    },
    remoteLoad(node, resolve) {
      if (node.data.Id) {
        var url = "admin/TopicPart/Select?parentId=" + node.data.Id;
        this.$http.get(url).then((res) => {
          if (res.IsSucc) {
            resolve(res.Data);
          }
        });
      }
    },
    beforeUpload() {
      this.showProgress = true;
      return true;
    },
    uploadSuccess(res) {
      this.showProgress = false;
      if (res.IsSucc) {
        this.loadPage();
        this.$message.success(res.Msg);
      } else {
        this.$message.error(res.Msg);
      }
    },
    uploadError(error) {
      this.showProgress = false;
      this.$message.error(error.message);
      console.error(error);
    },
  },
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
