<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="题目详情"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="onClickLeft"
  />
  <div class="bg-white h-full flex-grow overflow-y-auto overflow-x-hidden">
    <van-loading
      v-if="loading"
      class="h-full flex justify-center items-center"
      color="#0094ff"
      type="spinner"
      vertical
    >
      加载中
    </van-loading>
    <TestBox v-else :info="topicInfo" />
  </div>
</template>

<script setup lang="ts">
import http from '@http'
import { showNotify } from 'vant'
import { useRoute } from 'vue-router'
import { ref, onBeforeMount } from 'vue'
import { TopicInfo } from '@/types/TopicInfo'
import { TopicDetail } from '@/types/TopicDetail'

import TestBox from './components/TestBox.vue'

const route = useRoute()
const { number } = route.query

const loading = ref(true) //是否显示加载动画
const topicInfo = ref(new TopicDetail()) //题目详情

const onClickLeft = () => history.back()

/** 获取题目详情 */
const getTopicInfo = () => {
  var params = { number, isTested: false }
  http
    .get('api/Topic/TestInfo', { params })
    .right((res) => {
      var data = res.Data
      var info = new TopicInfo(0, data.Number, null, false)
      var detail = new TopicDetail(info)
      detail.collectionNum = data.CollectionNum
      detail.rightAnswer = data.RightAnswer
      detail.commentNum = data.CommentNum
      detail.collected = data.IsCollected
      detail.isOriginal = data.IsOriginal
      detail.content = data.Content
      detail.options = data.Options
      detail.origin = data.Origin
      detail.answer = data.Answer
      detail.level = data.Level
      detail.type = data.Type
      detail.id = data.Id

      topicInfo.value = detail
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        type: 'danger',
        duration: 2000
      })
    })
    .finally(() => {
      loading.value = false
    })
}

onBeforeMount(() => {
  getTopicInfo()
})
</script>
