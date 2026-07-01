<template>
  <div class="pager-container">
    <el-form class="pager-tools" :model="searchForm" inline>
      <el-form-item>
        <el-button type="primary" :icon="plusIcon" @click="openAdd">
          添加
        </el-button>
      </el-form-item>

      <el-form-item>
        <el-divider direction="vertical" style="height: 32px" />
      </el-form-item>

      <el-form-item label="性别" label-width="40">
        <el-select v-model="searchForm.Gender" clearable style="width: 100px">
          <el-option label="保密" value="保密" />
          <el-option label="男" value="男" />
          <el-option label="女" value="女" />
        </el-select>
      </el-form-item>

      <el-form-item label="推荐码" label-width="55">
        <el-input
          v-model="searchForm.InvitationCode"
          placeholder="输入完整推荐码"
          style="width: 120px"
          clearable
        />
      </el-form-item>

      <el-form-item label="手机号码" label-width="70">
        <el-input
          v-model="searchForm.MobilePhone"
          placeholder="输入完整号码"
          style="width: 130px"
          clearable
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

      <el-form-item label="关键字" label-width="55">
        <search-box
          v-model="searchForm.Keyword"
          placeholder="账户名、真实姓名、昵称、备注"
          @search="loadPage"
          width="280px"
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
      <el-table-column type="index" fixed="left" />
      <el-table-column label="头像" prop="Avatar" width="64">
        <template #default="scope">
          <el-avatar :size="40" :src="scope.Avatar" style="float: left" />
        </template>
      </el-table-column>
      <el-table-column label="账户名" prop="Account" width="140" />
      <el-table-column
        label="真实姓名"
        prop="RealName"
        sortable="custom"
        width="110"
      />
      <el-table-column label="昵称" prop="Nickname" width="170" />
      <el-table-column label="性别" prop="Gender" width="60" />
      <el-table-column label="手机号码" prop="MobilePhone" width="120" />
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
      <el-table-column label="推荐码" prop="InvitationCode" width="120" />
      <el-table-column label="备注" prop="Remark" />
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

  <my-dialog v-model="addDialogOpen" :options="addDialogOptions">
    <InfoDialog @close="onDialogClose" />
  </my-dialog>

  <my-dialog v-model="editDialogOpen" :options="editDialogOptions">
    <InfoDialog :detail="detailInfo" @close="onDialogClose" />
  </my-dialog>

  <el-drawer v-model="detailDrawerOpen" :with-header="false" direction="ltr">
    <Detail
      :detail="memberDetail"
      @change-time="changeTime"
      @unlock-or-lock="unlockOrLock"
    />
  </el-drawer>
</template>

<script>
import { markRaw } from "vue";
import { Plus, Delete, Edit } from "@element-plus/icons-vue";
import ColumnTools from "@/components/columnTools.vue";
import InfoDialog from "./Dialog.vue";
import Detail from "./Detail.vue";

export default {
  name: "AdminPage",
  components: { InfoDialog, Detail, ColumnTools },
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
      timeRange: null,
      detailInfo: null,
      memberDetail: {},
      addDialogOpen: false,
      editDialogOpen: false,
      detailDrawerOpen: false,
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
        Type: "管理员",
        Gender: null,
        EndTime: null,
        StartTime: null,
        OrderRule: "",
        OrderField: "",
        MobilePhone: "",
        InvitationCode: "",
      },
    };
  },
  mounted() {
    this.loadPage();
  },
  methods: {
    loadPage() {
      var url = "/admin/Member/Page";
      this.$http.post(url, this.searchForm).then((res) => {
        if (res.IsSucc) {
          this.total = res.Total;
          this.pageData = res.Data;
        } else {
          this.$message.error(res.Msg);
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
      if (this.memberDetail && this.memberDetail.Id === row.Id) {
        this.detailDrawerOpen = true;
      } else {
        var url = "admin/Member/Detail?id=" + row.Id;
        this.$http.get(url).then((res) => {
          if (res.IsSucc) {
            this.memberDetail = res.Data;
            this.detailDrawerOpen = true;
          } else {
            this.$message.error(res.Msg);
          }
        });
      }
    },
    deleteRow(row) {
      this.$confirm("确认要删除该用户？", "操作确认", {
        type: "warning",
      })
        .then(() => {
          var url = "admin/Member/Delete?id=" + row.Id;
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
    timeRangChange(val) {
      if (!val) {
        val = [null, null];
      }
      this.searchForm.StartTime = val[0];
      this.searchForm.EndTime = val[1];
    },
    unlockOrLock(isLock) {
      var url = `admin/Member/${isLock ? "Lock" : "UnLock"}?id=${
        this.memberDetail.Id
      }`;
      this.$http.get(url).then((res) => {
        if (res.IsSucc) {
          this.$message.success(res.Msg);
          this.memberDetail.IsLocked = isLock;
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
    changeTime() {},
  },
};
</script>
