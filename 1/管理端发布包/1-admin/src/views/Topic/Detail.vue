<template>
  <div style="height: 100%; overflow-y: auto; overflow-x: hidden">
    <h4 style="margin-bottom: 16px">题目详情</h4>
    <table class="topic-detail">
      <tbody>
        <tr>
          <td style="width: 80px">类型</td>
          <td style="width: 170px">{{ detail.Type }}</td>
          <td style="width: 80px">用途</td>
          <td style="width: 170px">{{ detail.UseType }}</td>
        </tr>
        <tr>
          <td>编号</td>
          <td>{{ detail.Number }}</td>
          <td>创建者</td>
          <td>{{ detail.MemberName }}</td>
        </tr>
        <tr>
          <td>难度</td>
          <td>{{ detail.Level }}</td>
          <td>专项</td>
          <td>{{ detail.PartName }}</td>
        </tr>
        <tr>
          <td>评论数量</td>
          <td>{{ detail.CommentNum }}</td>
          <td>收藏数量</td>
          <td>{{ detail.CollectionNum }}</td>
        </tr>
        <tr>
          <td>标签</td>
          <td>
            <el-tag class="ml-tag" v-for="(item, index) in Label" :key="index">
              {{ item }}
            </el-tag>
          </td>
          <td>推荐年级</td>
          <td>
            <el-tag
              class="ml-tag"
              v-for="(item, index) in RcmdGrade"
              :key="index"
            >
              {{ item }}
            </el-tag>
          </td>
        </tr>
        <tr>
          <td>目的</td>
          <td colspan="3">{{ detail.Purpose }}</td>
        </tr>
        <tr>
          <td>内容</td>
          <td colspan="3" v-html="detail.Content"></td>
        </tr>
        <tr v-if="detail.Options">
          <td>选项</td>
          <td colspan="3">
            <div
              class="option-item"
              v-for="(item, index) in Options"
              :key="index"
            >
              <div class="option-label">{{ item.Label + "." }}</div>
              <div class="option-content" v-html="item.Content"></div>
            </div>
          </td>
        </tr>
        <tr>
          <td>正确答案</td>
          <td colspan="3">{{ RightAnswer }}</td>
        </tr>
        <tr>
          <td>答案解析</td>
          <td colspan="3" v-html="detail.Analysis"></td>
        </tr>
        <tr>
          <td>是否原创</td>
          <td colspan="3">{{ detail.IsOriginal ? "是" : "否" }}</td>
        </tr>
        <tr v-if="!detail.IsOriginal">
          <td>来源</td>
          <td colspan="3">{{ detail.Origin }}</td>
        </tr>
        <tr>
          <td>创建时间</td>
          <td>{{ detail.CreationTime }}</td>
          <td>编辑时间</td>
          <td>{{ detail.ModificationTime }}</td>
        </tr>
        <tr>
          <td>备注</td>
          <td colspan="3">{{ detail.Remark }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: "TopicDetail",
  props: ["detail"],
  computed: {
    Label() {
      if (this.detail.Label) {
        return this.detail.Label.split(",");
      } else {
        return [];
      }
    },
    RcmdGrade() {
      if (this.detail.RcmdGrade) {
        return this.detail.RcmdGrade.split(",");
      } else {
        return [];
      }
    },
    Options() {
      if (this.detail.Options) {
        return JSON.parse(this.detail.Options);
      } else {
        return [];
      }
    },
    RightAnswer() {
      var answer = JSON.parse(this.detail.RightAnswer);
      if (answer && answer.length) {
        switch (this.detail.Type) {
          case "单选":
            return answer[0];
          case "多选":
            return answer.join("、");
          case "填空":
            return answer.join("、");
          case "编程":
            return answer[0];
        }
      }
      return "";
    },
  },
  data() {
    return {};
  },
};
</script>

<style lang="scss" scoped>
.topic-detail {
  border-collapse: collapse;
  width: 100%;

  tr {
    td {
      border: 1px solid #ebeef5;
      padding: 8px 11px;
      box-sizing: border-box;
      font-weight: 400;
      line-height: 23px;
      font-size: 14px;
    }

    td:nth-child(2n + 1) {
      min-width: 80px;
      max-width: 80px;
      font-weight: 700;
      color: #606266;
      text-align: center;
      background-color: #f5f7fa;
    }

    td:nth-child(2n) {
      min-width: 170px;
      color: #303133;
      text-align: left;
    }
  }
}

.ml-tag {
  margin-right: 5px;
}

.option-item {
  display: inline-flex;
  align-items: center;
  margin: 5px 0px;
  width: 100%;

  .option-label {
    text-align: center;
    flex-shrink: 0;
    width: 40px;
  }

  .option-content {
    width: 100%;
    flex-grow: 1;
  }
}
</style>
