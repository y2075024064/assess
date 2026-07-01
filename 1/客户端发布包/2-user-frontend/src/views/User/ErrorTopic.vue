<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="我的错题"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="onClickLeft"
  />

  <van-dropdown-menu class="border-b border-[var(--van-border-color)]">
    <van-dropdown-item v-model="partId" :options="option1" @change="changePart()" />
    <van-dropdown-item
      v-model="subPartId"
      :options="option2"
      :disabled="!partId"
      @change="changeSubPart()"
    />
  </van-dropdown-menu>

  <div v-if="!errorList">
    <van-empty description="当前还没有错题记录" />
  </div>

  <div class="h-full flex-grow overflow-x-hidden overflow-y-auto" v-else>
    <van-cell
      is-link
      center
      :title="item.Text"
      :label="item.Value"
      v-for="(item, index) in errorList"
      :key="index"
      @click="goToOneError(item.Text)"
    />
  </div>

  <div class="fixed bottom-8 right-4" v-if="errorList">
    <button
      class="p-[0.5rem] border-3 bg-orange-400 rounded-[7px] text-white"
      @click="goToErrorPage"
    >
      全部练习
    </button>
  </div>
</template>

<script setup lang="ts">
import http from '@http'
import { showNotify } from 'vant'
import { ref, onBeforeMount } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

type errorInfo = {
  Text: string
  Value: string
}

const errorList = ref<errorInfo[]>([])
const subPartId = ref<number>(0)
const partId = ref<number>(0)

var option1 = [{ text: '全部', value: 0 }]
var option2 = [{ text: '全部', value: 0 }]

const onClickLeft = () => history.back()

/** 改变主选择 */
const changePart = () => {
  getErrorPartById()
  getErrorList(partId.value)
}

/** 改变次级选择 */
const changeSubPart = () => {
  getErrorList(subPartId.value)
}

/** 前往错题练习页 */
const goToErrorPage = () => {
  let _partId = undefined
  if (subPartId.value) _partId = subPartId.value
  else if (partId.value) _partId = partId.value
  router.push({ name: 'practice', query: { type: 'error', partId: _partId } })
}

/** 跳转一个错题 */
const goToOneError = (errorNumber: string) => {
  router.push({ name: 'testInfo', query: { number: errorNumber } })
}

/** 分类获取全部错题 */
const getErrorList = (partId: number = -1) => {
  let params = {}
  if (partId > 0) {
    params = { partId }
  }
  http
    .get('/api/TopicError/List', { params })
    .right((res) => {
      errorList.value = res.Data
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
}

/** 获取错题类型 */
const getErrorPart = () => {
  http
    .get('/api/TopicPart/List')
    .right((res) => {
      res.Data.forEach((element: { Text: string; Value: string }) => {
        option1.push({
          text: element.Text,
          value: parseInt(element.Value)
        })
      })
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
}

/** 获取错题类型 */
const getErrorPartById = () => {
  http
    .get('/api/TopicPart/ListByParentId', { params: { parentId: partId.value } })
    .right((res) => {
      // 重新初始化次级选择菜单
      option2 = option2.slice(0, 1)
      subPartId.value = 0
      res.Data.forEach((element: { Text: string; Value: string }) => {
        option2.push({
          text: element.Text,
          value: parseInt(element.Value)
        })
      })
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
}

onBeforeMount(() => {
  getErrorList()
  getErrorPart()
})
</script>
