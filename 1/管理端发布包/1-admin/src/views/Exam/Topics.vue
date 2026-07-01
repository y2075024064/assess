<template>
  <div class="topic-content">
    <div class="half-box">
      <div class="body">
        <div class="header">
          <el-input v-model="keyWord" placeholder="标题查询">
            <template #append>
              <el-button type="primary" :icon="Search" @click="getAllTopics" />
            </template>
          </el-input>
        </div>
        <el-table
          :header-row-style="{ height: '45px' }"
          ref="allTableRef"
          :data="allTopics"
          class="w-full"
          height="100%"
          border
        >
          <el-table-column
            :width="38"
            type="selection"
            :selectable="rowSelectable"
          />
          <el-table-column
            label="标题"
            prop="Title"
            :width="200"
            show-overflow-tooltip
          />
          <el-table-column label="难度" prop="Level" :width="60" />
          <el-table-column label="内容" prop="Content" show-overflow-tooltip />
        </el-table>
        <my-pager
          v-model:currentPage="page"
          v-model:pageSize="take"
          :total="total"
          class="footer"
          @sizeChange="getAllTopics"
          @currentChange="getAllTopics"
        />
      </div>
    </div>

    <div class="middle">
      <div class="btn">
        <el-button type="primary" :icon="DArrowRight" @click="addTopics" />
      </div>
    </div>

    <div class="half-box">
      <div class="body">
        <div class="header">
          <el-button type="primary" :icon="Setting" @click="setScore">
            设置成绩
          </el-button>
          <el-button type="danger" :icon="Delete" @click="removeTopic">
            删除题目
          </el-button>
        </div>
        <el-table
          :header-row-style="{ height: '45px' }"
          ref="selectedTableRef"
          :data="selectedTopics"
          class="w-full"
          height="100%"
          border
        >
          <el-table-column :width="38" type="selection" />
          <el-table-column
            label="标题"
            prop="Title"
            :width="200"
            show-overflow-tooltip
          />
          <el-table-column label="难度" prop="Level" :width="60" />
          <el-table-column label="分数" prop="Score" :width="60" />
          <el-table-column label="内容" prop="Content" show-overflow-tooltip />
        </el-table>
        <div class="footer"></div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { Search, Delete, Setting, DArrowRight } from "@element-plus/icons-vue";

import { showError, showSuccess, showPrompt } from "@/utils/elBox";
import { ref, watch, onBeforeMount, defineProps } from "vue";
import http from "@/utils/request";

const props = defineProps({
  id: Number,
});

const allTableRef = ref();
const selectedTableRef = ref();

const page = ref(1);
const total = ref(0);
const take = ref(30);
const keyWord = ref("");
const allTopics = ref([]);
const selectedTopics = ref([]);

const rowSelectable = (row, index) => {
  return selectedTopics.value.findIndex((e) => e.TopicId === row.Id) < 0;
};

const addTopics = () => {
  const ids = [];
  const rows = allTableRef.value.getSelectionRows();
  for (const item of rows) {
    ids.push(item.Id);
  }
  if (ids.length <= 0) return;

  showPrompt("请输入分数：", "提示", {
    inputPattern: /\d+/,
    inputErrorMessage: "格式不正确",
  }).then(({ value }) => {
    const param = { examId: props.id, score: value };
    http.post("/admin/ExamTopic/Add", ids, { params: param }).then((res) => {
      if (res.IsSucc) {
        showSuccess(res.Msg);
        getSelectedTopics();
      } else {
        showError(res.Msg);
      }
    });
  });
};

const setScore = () => {
  const ids = [];
  const rows = selectedTableRef.value.getSelectionRows();
  for (const item of rows) {
    ids.push(item.Id);
  }
  if (ids.length <= 0) return;

  showPrompt("请输入分数：", "提示", {
    inputPattern: /\d+/,
    inputErrorMessage: "格式不正确",
  }).then(({ value }) => {
    const param = { score: value };
    http
      .post("/admin/ExamTopic/SetScore", ids, { params: param })
      .then((res) => {
        if (res.IsSucc) {
          showSuccess(res.Msg);
          getSelectedTopics();
        } else {
          showError(res.Msg);
        }
      });
  });
};

const removeTopic = () => {
  const ids = [];
  const rows = selectedTableRef.value.getSelectionRows();
  for (const item of rows) {
    ids.push(item.Id);
  }
  if (ids.length <= 0) return;

  http.delete("/admin/ExamTopic/Remove", { data: ids }).then((res) => {
    if (res.IsSucc) {
      showSuccess(res.Msg);
      getSelectedTopics();
    } else {
      showError(res.Msg);
    }
  });
};

const getSelectedTopics = () => {
  if (props.id) {
    http.get(`/admin/ExamTopic/List?examId=${props.id}`).then((res) => {
      if (res.IsSucc) {
        selectedTopics.value = res.Data;
      } else {
        showError(res.Msg);
      }
    });
  }
};

const getAllTopics = () => {
  const param = {
    UseType: "评测",
    Page: page.value,
    Take: take.value,
    Keyword: keyWord.value,
  };
  http.post("/admin/Topic/Page", param).then((res) => {
    if (res.IsSucc) {
      allTopics.value = res.Data;
      total.value = res.Total;
    } else {
      showError(res.Msg);
    }
  });
};

watch(
  () => props.id,
  () => getSelectedTopics()
);

onBeforeMount(() => {
  getAllTopics();
  getSelectedTopics();
});
</script>

<script>
export default {
  name: "ExamTopics",
};
</script>

<style lang="scss" scoped>
.topic-content {
  width: 100%;
  height: 100%;
  display: block;

  .half-box {
    height: 100%;
    vertical-align: top;
    display: inline-block;
    width: calc(50% - 50px);

    .body {
      height: 100%;
      display: flex;
      flex-direction: column;

      .header {
        flex-shrink: 0;
        margin-bottom: 10px;
      }

      .footer {
        height: 24px;
        flex-shrink: 0;
        padding: 5px 10px;
      }
    }
  }

  .middle {
    width: 100px;
    height: 100%;
    display: inline-block;

    .btn {
      height: 100%;
      display: flex;
      align-items: center;
      justify-content: center;
    }
  }
}
</style>
