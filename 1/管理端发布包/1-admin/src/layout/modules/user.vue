<template>
  <div class="nav-user-header">
    <el-avatar class="user-avatar" :size="34" :src="headImgUrl" fit="cover" />
    <el-dropdown
      class="nav-dropdown"
      placement="bottom-start"
      trigger="click"
      @command="clickCommand"
    >
      <span class="dropdown-content">
        {{ userNickname }}
        <el-icon class="el-icon--right">
          <ArrowDown />
        </el-icon>
      </span>
      <template #dropdown>
        <el-dropdown-menu>
          <el-dropdown-item command="baseInfo">
            <el-icon><User /></el-icon>
            基本资料
          </el-dropdown-item>
          <el-dropdown-item command="editPwd">
            <el-icon><Edit /></el-icon>
            修改密码
          </el-dropdown-item>
          <el-dropdown-item command="logout" divided>
            <el-icon><SwitchButton /></el-icon>
            退出登录
          </el-dropdown-item>
        </el-dropdown-menu>
      </template>
    </el-dropdown>
  </div>

  <my-dialog v-model="repwdDialogOpen" :options="repwdDialogOptions">
    <RepwdDialog @close="repwdDialogOpen = false" />
  </my-dialog>

  <my-dialog v-model="editDialogOpen" :options="editDialogOptions">
    <EditDialog @close="editDialogOpen = false" />
  </my-dialog>
</template>

<script>
import { markRaw } from "vue";
import { ArrowDown, User, Edit, SwitchButton } from "@element-plus/icons-vue";
import RepwdDialog from "@/views/User/RepwdDialog.vue";
import EditDialog from "@/views/User/EditDialog.vue";

export default {
  name: "LayoutUser",
  components: { ArrowDown, User, Edit, SwitchButton, RepwdDialog, EditDialog },
  computed: {
    userNickname() {
      return this.$store.getters.name || "用户昵称";
    },
    headImgUrl() {
      return this.$store.getters.avatar;
    },
  },
  data() {
    return {
      editDialogOpen: false,
      repwdDialogOpen: false,
      editDialogOptions: {
        showMaximize: false,
        icon: markRaw(User),
        appendToBody: true,
        title: "基本资料",
      },
      repwdDialogOptions: {
        showMaximize: false,
        icon: markRaw(Edit),
        appendToBody: true,
        title: "修改密码",
      },
    };
  },
  methods: {
    clickCommand(name) {
      switch (name) {
        case "baseInfo":
          this.editDialogOpen = true;
          break;
        case "editPwd":
          this.repwdDialogOpen = true;
          break;
        case "logout":
          this.logout();
          break;
      }
    },
    logout() {
      this.$store.dispatch("user/logout").then((res) => {
        if (res.isSucc) {
          this.$router.push("/");
        }
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.nav-user-header {
  height: 100%;
  display: flex;
  padding-left: 7px;
  align-items: center;
  justify-content: center;
  -webkit-display: flex;
  -webkit-align-items: center;
  -webkit-justify-content: center;

  .user-avatar {
    width: 34px;
    height: 34px;
    flex-shrink: 0;
    border: 1px gray solid;
  }

  .nav-dropdown {
    cursor: pointer;
    font-size: 15px;
    padding-left: 6px;

    .dropdown-content {
      display: flex;
      align-items: center;
    }

    .dropdown-content:hover {
      color: black;
    }
  }
}
</style>
