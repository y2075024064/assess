<template>
  <div style="height: 100%; overflow: auto">
    <el-descriptions border title="用户详情" :column="1">
      <el-descriptions-item label="头像">
        <el-avatar :size="50" :src="detail.Avatar" style="float: left" />
      </el-descriptions-item>
      <el-descriptions-item label="账户类型">
        {{ detail.Type }}
      </el-descriptions-item>
      <el-descriptions-item label="账户名">
        {{ detail.Account }}
      </el-descriptions-item>
      <el-descriptions-item label="真实姓名">
        {{ detail.RealName }}
      </el-descriptions-item>
      <el-descriptions-item label="昵称">
        {{ detail.Nickname }}
      </el-descriptions-item>
      <el-descriptions-item label="生日">
        {{ detail.Birthday }}
      </el-descriptions-item>
      <el-descriptions-item label="手机号码">
        {{ detail.MobilePhone }}
      </el-descriptions-item>
      <el-descriptions-item label="电子邮箱">
        {{ detail.Email }}
      </el-descriptions-item>
      <el-descriptions-item label="推荐码">
        {{ detail.InvitationCode }}
      </el-descriptions-item>
      <el-descriptions-item label="是否绑定微信">
        {{ detail.IsBindWechat ? "是" : "否" }}
      </el-descriptions-item>
      <el-descriptions-item label="是否被冻结">
        <div v-if="detail.IsLocked" style="color: red">
          是
          <el-button
            style="margin-left: 133px"
            type="primary"
            link
            @click="unlockOrLock(false)"
          >
            解冻
          </el-button>
        </div>
        <div v-else style="color: #409eff">
          否
          <el-button
            style="margin-left: 133px"
            type="primary"
            link
            @click="unlockOrLock(true)"
          >
            冻结
          </el-button>
        </div>
      </el-descriptions-item>
      <el-descriptions-item label="注册时间">
        {{ detail.CreationTime }}
      </el-descriptions-item>
      <el-descriptions-item label="最近修改时间">
        {{ detail.ModificationTime }}
      </el-descriptions-item>
      <el-descriptions-item label="封禁开始时间">
        {{ detail.DisableStartTime }}
      </el-descriptions-item>
      <el-descriptions-item label="封禁结束时间">
        {{ detail.DisableEndTime }}
        <el-button
          v-if="canModifyTime"
          style="margin-left: 10px"
          type="primary"
          link
          @click="changeTime()"
        >
          修改
        </el-button>
      </el-descriptions-item>
      <el-descriptions-item label="备注">
        {{ detail.Remark }}
      </el-descriptions-item>
    </el-descriptions>
  </div>
</template>

<script>
export default {
  name: "MemberDetail",
  props: ["detail"],
  emits: ["unlockOrLock", "changeTime"],
  computed: {
    canModifyTime() {
      if (this.detail.DisableEndTime) {
        var now = new Date();
        var date = new Date(this.detail.DisableEndTime);
        if (date > now) {
          return true;
        }
      }

      return false;
    },
  },
  data() {
    return {};
  },
  methods: {
    unlockOrLock(isLock) {
      this.$emit("unlockOrLock", isLock);
    },
    changeTime() {
      this.$emit("changeTime");
    },
  },
};
</script>
