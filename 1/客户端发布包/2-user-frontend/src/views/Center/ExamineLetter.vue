<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="信件审核"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="urlBack"
  />
  <!-- 列表 -->
  <van-pull-refresh class="h-full overflow-y-auto" v-model="refreshing" @refresh="onRefresh">
    <van-list
      v-model:loading="loading"
      finished-text="没有更多了"
      :finished="finished"
      @load="onLoad"
    >
      <div class="h-full flex-grow overflow-x-hidden overflow-y-auto">
        <div
          class="w-full flex flex-col justify-center p-[10px] mb-[10px] bg-white"
          v-for="(item, index) in letterList"
          :key="index"
        >
          <div class="w-full flex">
            <div class="flex-none w-16 justify-center items-center">
              <van-image height="50" width="50" fit="cover" radius="25" :src="item.Image" />
            </div>
            <div class="flex-1 w-64 relative">
              <div class="text-[16px] font-bold">{{ item.Title }}</div>
              <div class="text-[12px] text-[#000] mt-[5px]">{{ item.Nickname }}</div>
              <van-tag type="warning" plain class="absolute top-0 right-0 py-[2px]">
                {{ states[item.State] }}
              </van-tag>
              <van-tag
                type="warning"
                plain
                class="absolute top-8 right-0 py-[2px]"
                v-if="item.IsModel"
              >
                精华
              </van-tag>
            </div>
          </div>
          <div class="w-full shrink">
            <div class="flex flex-col pr-[5px]">
              <div class="text-[14px] py-[5px]">{{ item.Content }}</div>
            </div>
            <div class="basis-1/4" v-if="item.Image">
              <van-image width="8rem" height="6rem" fit="cover" radius="5" :src="item.Image" />
            </div>
          </div>
          <div class="flex justify-between">
            <div class="text-[10px] text-slate-400 mt-[15px]">{{ item.CreationTime }}</div>
            <div @click="openReply(item.Id)" class="cursor-pointer text-[#9b9b9b]">审核</div>
          </div>
        </div>
      </div>
    </van-list>
  </van-pull-refresh>
  <!-- 回复窗 -->
  <van-popup
    v-model:show="show"
    safe-area-inset-bottom
    position="bottom"
    round
    @open="clearContent"
  >
    <div class="p-[10px]">
      <textarea
        placeholder="在这里输入内容"
        class="min-h-[200px] max-h-[60vh] w-full"
        :value="content"
        @input="setContent($event)"
      >
      </textarea>
      <!-- 用户上传图片 -->
      <van-cell-group style="padding: 10px 10px 2px">
        <van-uploader v-model="contentImgs" multiple :max-count="3" />
      </van-cell-group>
      <div class="flex items-center mt-[10px] pl-[10px]">
        <!-- 是否设置精华 -->
        <div class="flex items-center">
          <div class="text-[14px] text-[#9b9b9b]">设置为精华</div>
          <van-switch v-model="isModel" class="scale-75" @change="console.log(isModel)" />
        </div>
        <!-- 信件状态选择 -->
        <div class="select">
          <select name="state" @change="getSelect">
            <option :value="item.value" v-for="(item, index) in stateList" :key="index">
              {{ item.text }}
            </option>
          </select>
        </div>
        <!-- 回复按键 -->
        <button
          class="grow bg-orange-400 text-white py-[5px] px-[1rem] ml-[1.5rem] rounded-[5px]"
          @click="adminReply"
        >
          通过
        </button>
      </div>
    </div>
  </van-popup>
</template>

<script setup lang="ts">
import http from '@http'
import { ref, onMounted } from 'vue'
import type { LetterInfo } from '@/types/LetterInfo'
import { showNotify } from 'vant'

const urlBack = () => history.back()
const letterList = ref<LetterInfo[]>([])

const show = ref(false)
const isModel = ref(false)
const currentLetterId = ref(-1)
const currentState = ref('New')
const contentImgs = ref<any[]>([]) // 评论图片
const content = ref('')
const loading = ref(false)
const finished = ref(false)
const refreshing = ref(false)
const stateList = [
  { text: '新增', value: 'New' },
  { text: '已查看', value: 'Viewed' },
  { text: '处理中', value: 'Processing' },
  { text: '已解决', value: 'Resolved' },
  { text: '已完成', value: 'Finished' }
]
const states: any = {
  New: '新增',
  Viewed: '已查看',
  Processing: '处理中',
  Resolved: '已解决',
  Finished: '已完成'
}

let page = 1
const take = 10

/** 更新用户输入内容 */
const setContent = (event: any) => {
  content.value = event.target.value
}

/** 回复回复时选择的状态 */
const getSelect = (event: any) => {
  currentState.value = event.target.value
}

/** 下拉刷新 */
const onRefresh = () => {
  // 清空列表数据
  finished.value = false

  // 重新加载数据
  letterList.value = []
  page = 1
  onLoad()
}

/** 获取所有待审核信件 */
const onLoad = () => {
  loading.value = true
  http
    .post('api/Letter/AdminList', { Page: page, Take: take })
    .right((res) => {
      const arr = res.Data as LetterInfo[]
      if (arr.length) {
        arr.forEach((e) => letterList.value.push(e))
        ++page
      } else {
        finished.value = true
      }
    })
    .finally(() => {
      loading.value = false
      if (refreshing.value) refreshing.value = false
    })
}

/** 重置已选内容 */
const clearContent = () => {
  content.value = ''
  contentImgs.value = []
  isModel.value = false
  currentState.value = ''
}

/**  */
const openReply = (id: number) => {
  show.value = true
  currentLetterId.value = id
}

/** 管理员回复 */
const adminReply = () => {
  const formData = new FormData()
  formData.append('objectId', currentLetterId.value + '')
  formData.append('isModel', isModel.value.toString())
  formData.append('state', currentState.value)
  formData.append('content', content.value)
  for (const item of contentImgs.value) {
    formData.append('images', item.file)
  }
  http
    .postForm('api/Letter/AdminReply', formData)
    .right((res) => {
      clearContent()
      onRefresh()
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'success'
      })
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
    .finally(() => {
      show.value = false
    })
}

onMounted(() => {})
</script>

<style scoped>
select {
  border: none;
  outline: none;
  width: 100%;
  height: 35px;
  line-height: 35px;
  text-align: center;
  color: #9b9b9b;
}

select option {
  border: none;
  color: #777777 !important;
  font-family: Arial;
}
</style>
