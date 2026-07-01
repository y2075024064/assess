<template>
  <el-form
    ref="dialogForm"
    :model="formData"
    :rules="formRules"
    label-width="80px"
  >
    <el-form-item label="类型" prop="Type" style="float: left">
      <el-select
        v-model="formData.Type"
        style="width: 120px"
        @change="typeChanged"
      >
        <el-option value="单选" label="单选" />
        <el-option value="多选" label="多选" />
        <el-option value="填空" label="填空" />
        <el-option value="编程" label="编程" />
      </el-select>
    </el-form-item>

    <el-form-item label="用途" prop="UseType">
      <el-select v-model="formData.UseType" style="width: 120px">
        <el-option value="通用" label="通用" />
        <el-option value="练习" label="练习" />
        <el-option value="评测" label="评测" />
      </el-select>
    </el-form-item>

    <el-form-item label="所属专项" prop="PartId" style="float: left">
      <el-tree-select
        :props="{ label: 'Name', isLeaf: 'IsLeaf' }"
        :remote-method="remoteMethod"
        :loading="treeSelectLoading"
        :render-after-expand="false"
        v-model="formData.PartId"
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

    <el-form-item label="难度" prop="Level">
      <el-input-number
        v-model="formData.Level"
        style="width: 110px"
        :precision="1"
        :step="0.1"
        :min="0.0"
        :max="1"
      />
    </el-form-item>

    <el-form-item label="题目标题" prop="Title">
      <el-input v-model="formData.Title" clearable />
    </el-form-item>

    <el-form-item label="考察目的" prop="Purpose">
      <el-input v-model="formData.Purpose" type="textarea" rows="1" />
    </el-form-item>

    <el-form-item label="推荐年级" prop="RcmdGrade">
      <el-select
        v-model="formData.RcmdGrade"
        clearable
        multiple
        style="width: 100%"
      >
        <el-option
          v-for="item in selectData"
          :key="item.Value"
          :label="item.Text"
          :value="item.Value"
        />
      </el-select>
    </el-form-item>

    <el-form-item label="标签" prop="Label">
      <mySelect
        url="admin/TopicLabel/Select"
        placeholder="请输入关键字"
        v-model="formData.Label"
        style="width: 100%"
        allow-create
        multiple
      />
    </el-form-item>

    <el-form-item label="题目内容" prop="Content">
      <WangEditor
        v-model="formData.Content"
        ref="ContentEditor"
        type="TopicContent"
        should-formula
        should-image
        should-video
      />
    </el-form-item>

    <el-form-item v-if="selectOptions" label="答案选项">
      <el-descriptions
        style="width: 100%; border: 1px solid #ccc"
        :column="2"
        border
      >
        <template #extra>
          <el-button
            style="margin: 5px 6px -7px 0px"
            :icon="plusIcon"
            title="添加选项"
            type="primary"
            @click="addSelectOption()"
          />
        </template>
        <el-descriptions-item
          v-for="(item, index) in answerData"
          :label="item.Label"
          :key="index"
        >
          <div class="descriptions-item-style">
            <el-input v-model="item.Content" type="textarea" :rows="1" />
            <el-button
              style="margin: 4px 0px 0px 7px"
              :icon="deleteIcon"
              title="删除该选项"
              type="danger"
              size="small"
              @click="delSelectOption(index)"
            />
          </div>
        </el-descriptions-item>
      </el-descriptions>
    </el-form-item>

    <el-form-item label="正确答案" prop="RightAnswer">
      <el-input
        v-if="formData.Type == '填空'"
        v-model="formData.RightAnswer"
        placeholder="每一行对应一个填空"
        type="textarea"
        rows="2"
      />
      <el-input
        v-else-if="answerData.length == 0"
        v-model="formData.RightAnswer"
      />
      <el-radio-group
        v-else-if="formData.Type == '单选'"
        v-model="formData.RightAnswer"
      >
        <el-radio
          v-for="(item, index) in answerData"
          :label="item.Label"
          :key="index"
        />
      </el-radio-group>
      <el-checkbox-group
        v-else-if="formData.Type == '多选'"
        v-model="formData.RightAnswer"
      >
        <el-checkbox
          v-for="(item, index) in answerData"
          :label="item.Label"
          :key="index"
        />
      </el-checkbox-group>
      <el-input v-else v-model="formData.RightAnswer" readonly />
    </el-form-item>

    <el-form-item label="答案解析" prop="Analysis">
      <WangEditor
        v-model="formData.Analysis"
        ref="AnalysisEditor"
        type="TopicContent"
        should-formula
        should-image
        should-video
      />
    </el-form-item>

    <el-form-item label="是否原创" prop="IsOriginal">
      <el-switch
        v-model="formData.IsOriginal"
        inactive-text="否"
        active-text="是"
        inline-prompt
        size="large"
        :width="80"
      />
    </el-form-item>

    <el-form-item v-if="!formData.IsOriginal" label="来源" prop="Origin">
      <el-input v-model="formData.Origin" />
    </el-form-item>

    <el-form-item label="备注" prop="Remark">
      <el-input v-model="formData.Remark" />
    </el-form-item>

    <el-form-item>
      <el-button type="primary" @click="submit">确定</el-button>
      <el-button @click="close(false)">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import { markRaw } from "vue";
import mySelect from "@/components/mySelect.vue";
import WangEditor from "@/components/wangEditor.vue";
import { Plus, Delete } from "@element-plus/icons-vue";

export default {
  name: "TopicInfoDialog",
  components: { mySelect, WangEditor },
  props: {
    detail: { type: Object, default: null },
  },
  emits: ["close"],
  data() {
    var plusIcon = markRaw(Plus);
    var deleteIcon = markRaw(Delete);
    return {
      plusIcon,
      deleteIcon,
      isUpdate: false,
      selectData: [],
      answerData: [],
      selectOptions: true,
      treeSelectData: [],
      treeSelectLoading: false,
      formRules: {
        Type: [{ required: true, message: "类型必选！" }],
        Level: [{ required: true, message: "难度必填！" }],
        UseType: [{ required: true, message: "用途必选！" }],
        Content: [{ required: true, message: "内容必填！" }],
        RightAnswer: [{ required: true, message: "正确答案必填！" }],
      },
      formData: {
        Id: 0,
        Type: "单选",
        Level: 1.0,
        Title: "",
        UseType: "通用",
        Label: [],
        Purpose: "",
        Content: "",
        PartId: null,
        RcmdGrade: [],
        RightAnswer: "",
        Analysis: "",
        Options: "",
        Origin: "",
        IsOriginal: false,
        Remark: "",
      },
    };
  },
  mounted() {
    this.getSelect();
    if (this.detail) {
      this.isUpdate = true;
      var url = "admin/Topic/Detail?id=" + this.detail.Id;
      this.$http.get(url).then((res) => {
        if (res.IsSucc) {
          var data = res.Data;
          this.treeSelectData.push({
            Name: data.PartName,
            Id: data.PartId,
            IsLeaf: true,
          });
          for (var item in this.formData) {
            this.formData[item] = data[item];
          }
          if (data.Label) {
            this.formData.Label = data.Label.split(",");
          }
          if (data.Options) {
            this.answerData = JSON.parse(data.Options);
          }
          if (data.RcmdGrade) {
            this.formData.RcmdGrade = [];
            var temp = data.RcmdGrade.split(",");
            for (var i = 0; i < temp.length; i++) {
              var items = temp[i].split(":");
              this.formData.RcmdGrade.push(items[0]);
            }
          }
          if (data.RightAnswer) {
            var _answer = null;
            var answer = JSON.parse(data.RightAnswer);
            if (answer.length > 0) {
              switch (data.Type) {
                case "单选":
                  _answer = answer[0];
                  break;
                case "多选":
                  _answer = answer;
                  break;
                case "填空":
                  _answer = answer.join("\n");
                  break;
                case "编程":
                  _answer = answer[0];
                  break;
              }
              this.formData.RightAnswer = _answer;
            } else {
              this.formData.RightAnswer = null;
            }
          }
        } else {
          this.$message.error(res.Msg);
        }
      });
    }
  },
  methods: {
    close(state) {
      this.$emit("close", state);
    },
    submit() {
      this.$refs.dialogForm.validate((isValid) => {
        if (isValid) {
          this.submitForm();
        }
      });
    },
    submitForm() {
      var data = {};
      for (var item in this.formData) {
        data[item] = this.formData[item];
      }

      if (this.formData.Type == "多选") {
        this.formData.RightAnswer = JSON.stringify(this.formData.RightAnswer);
      }

      if (this.answerData.length > 0) {
        data.Options = JSON.stringify(this.answerData);
      }

      var url = `admin/Topic/${this.isUpdate ? "Update" : "Insert"}`;
      this.$http.post(url, data).then((res) => {
        if (res.IsSucc) {
          this.$message.success(res.Msg);
          this.close(true);
        } else {
          this.$message.error(res.Msg);
        }
      });
    },
    getSelect() {
      var url = "admin/Grade/Select";
      this.$http.get(url).then((res) => {
        if (res.IsSucc) {
          this.selectData = res.Data;
        }
      });
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
    typeChanged(val) {
      switch (val) {
        case "单选":
          this.formData.RightAnswer = "";
          this.selectOptions = true;
          break;
        case "多选":
          this.formData.RightAnswer = [];
          this.selectOptions = true;
          break;
        case "填空":
          this.formData.RightAnswer = "";
          this.selectOptions = false;
          break;
        case "编程":
          this.formData.RightAnswer = "success";
          this.selectOptions = false;
          break;
      }
    },
    addSelectOption() {
      var count = this.answerData.length;
      this.answerData.push({
        Label: String.fromCharCode(count + 65),
        Content: "",
      });
    },
    delSelectOption(index) {
      this.answerData.splice(index, 1);
      for (var i = 0; i < this.answerData.length; i++) {
        this.answerData[i].Label = String.fromCharCode(i + 65);
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.descriptions-item-style {
  width: 100%;
  display: inline-flex;
}
</style>
