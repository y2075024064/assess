<template>
  <el-descriptions
    class="star-detail-box"
    v-loading="loading"
    title="详情"
    :column="1"
    border
  >
    <el-descriptions-item label="类型" label-align="right">
      {{ detailInfo.Type }}
    </el-descriptions-item>

    <el-descriptions-item label="标题" label-align="right">
      {{ detailInfo.Title }}
    </el-descriptions-item>

    <el-descriptions-item label="用户账户" label-align="right">
      {{ detailInfo.Account }}
    </el-descriptions-item>

    <el-descriptions-item label="用户姓名" label-align="right">
      {{ detailInfo.Name }}
    </el-descriptions-item>

    <el-descriptions-item label="预览图片" label-align="right">
      <el-image :src="detailInfo.PreviewImg" fit="contain" />
    </el-descriptions-item>

    <el-descriptions-item label="所属公司" label-align="right">
      {{ detailInfo.Company }}
    </el-descriptions-item>

    <el-descriptions-item label="内容" label-align="right">
      <div v-html="detailInfo.Content"></div>
    </el-descriptions-item>

    <el-descriptions-item label="创建时间" label-align="right">
      {{ detailInfo.CreationTime }}
    </el-descriptions-item>

    <el-descriptions-item label="备注" label-align="right">
      {{ detailInfo.Remark }}
    </el-descriptions-item>
  </el-descriptions>
</template>

<script setup>
import { ref, watch, onBeforeMount, defineProps } from "vue";
import http from "@/utils/request";

const props = defineProps({
  id: Number,
});

const loading = ref(true);
const detailInfo = ref({});

const getData = () => {
  if (props.id) {
    loading.value = true;
    http.get(`/admin/ThinkStar/Detail?id=${props.id}`).then((res) => {
      detailInfo.value = res.Data;
      loading.value = false;
    });
  }
};

watch(
  () => props.id,
  () => getData()
);

onBeforeMount(() => getData());
</script>

<script>
export default {
  name: "ThinkStarDetail",
};
</script>

<style lang="scss">
.star-detail-box {
  table {
    tr {
      td:first-child {
        width: 82px;
      }
    }
  }
}
</style>
