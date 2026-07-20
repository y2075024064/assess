<template>
  <div class="px-3 pb-3 h-full flex flex-col overflow-hidden">
    <!-- 头部内容部分 -->
    <div class="flex-shrink-0">
      <!-- 第一行内容 -->
      <div class="header-row">
        <!--类型标签-->
        <van-tag class="flex-shrink-0" round type="primary" size="medium">
          {{ info.type }}
        </van-tag>
        <!--原创标签-->
        <van-tag class="flex-shrink-0" v-if="info.isOriginal" round type="default" size="medium">
          原创
        </van-tag>
        <!--收藏按钮-->
        <div class="ml-4 flex-shrink-0" @click="collectTopic">
          <van-action-bar-icon
            class="bar-item-center-left"
            icon-class="bar-icon-big"
            :color="collectInfo.color"
            :text="collectInfo.text"
            :icon="collectInfo.icon"
          />
        </div>
        <div class="w-full"></div>
        <!--交卷按钮-->
        <div class="flex-shrink-0" v-if="testId">
          <van-action-bar-icon
            class="bar-item-center-left"
            icon-class="bar-icon-big"
            icon="completed"
            color="#07c160"
            text="交卷"
            @click="emit('submitTest')"
          />
        </div>
      </div>

      <!-- 第二行内容-来源说明 -->
      <div v-if="info.origin" class="header-row text-xs text-gray-600">来源：{{ info.origin }}</div>

      <!-- 第三行内容 -->
      <div class="header-row text-[15px] leading-[25px]">
        <!-- 难度五角星图标列 -->
        <span class="flex-shrink-0">难度&nbsp;</span>
        <van-rate
          v-model:model-value="level"
          class="flex-shrink-0"
          color="#ffd21e"
          allow-half
          size="17"
          readonly
        />
        <!-- 显示总题目列表按钮 -->
        <div v-if="testedNumber" class="inline-flex items-center w-full flex-grow justify-end">
          <van-action-bar-icon
            class="bar-item-center-left"
            icon-class="bar-icon-big"
            :text="testedNumber"
            icon="apps-o"
            @click="emit('showList')"
          />
        </div>
      </div>
    </div>

    <div class="h-full flex-grow overflow-y-auto">
      <!-- 题目内容部分 -->
      <div class="text-lg mt-4" v-html="content"></div>

      <!-- 答题区 -->
      <van-cell-group class="mx-0 my-6 px-4 py-2 border border-gray-200" inset>
        <van-radio-group
          v-if="info.type === '单选'"
          :disabled="info.baseInfo.isTested"
          v-model="singleSelected"
        >
          <van-radio class="my-3" v-for="(item, index) in options" :key="index" :name="item.Label">
            <div class="inline-flex items-center w-full">
              <div class="w-7 flex-shrink-0">{{ item.Label }}.</div>
              <div class="w-full flex-grow" v-html="item.Content"></div>
            </div>
          </van-radio>
        </van-radio-group>
        <van-checkbox-group
          v-else-if="info.type === '多选'"
          :disabled="info.baseInfo.isTested"
          v-model="multiSelected"
        >
          <van-checkbox
            class="my-3"
            v-for="(item, index) in options"
            :key="index"
            :name="item.Label"
          >
            <div class="inline-flex items-center w-full">
              <div class="w-7 flex-shrink-0">{{ item.Label }}.</div>
              <div class="w-full flex-grow" v-html="item.Content"></div>
            </div>
          </van-checkbox>
        </van-checkbox-group>
        <div v-else-if="info.type === '填空'">
          <van-field
            v-for="(item, index) in fillAnswer"
            :readonly="info.baseInfo.isTested"
            v-model="item.value"
            label-width="10px"
            :label="index + 1"
            class="border-b"
            :key="index"
          />
        </div>
      </van-cell-group>

      <!-- 提交按钮 -->
      <div class="text-center mb-4" v-if="!info.baseInfo.isTested">
        <van-button :disabled="submiting" class="w-[70%]" type="primary" @click="submit"
          >确定</van-button
        >
      </div>

      <!-- 正确答案及解析区 -->
      <div class="px-4 mb-4" v-if="info.baseInfo.isTrue != null">
        <van-tag v-if="info.baseInfo.isTrue" type="success" size="large">正确</van-tag>
        <van-tag v-else type="danger" size="large">错误</van-tag>
        <p class="mt-3">答案：<span v-html="rightAnswer"></span></p>
        <p class="my-1">解析：</p>
        <div class="my-1" v-html="info.analysis"></div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onBeforeMount, ref, watch } from 'vue'
import type { TopicDetail } from '@/types/TopicDetail'
import { showNotify } from 'vant'
import http from '@http'

const props = defineProps<{
  info: TopicDetail
  testId?: number
  testedNumber?: string
}>()

const emit = defineEmits<{
  (e: 'submit', val: boolean): void
  (e: 'submitTest'): void
  (e: 'showList'): void
}>()

const uncollectInfo = { text: '收藏', icon: 'star-o', color: '#1989fa' }
const collectedInfo = { text: '已收藏', icon: 'star', color: '#ff5000' }

const level = ref(0)
const content = ref('')
const exeAnswer = ref('')
const submiting = ref(false)
const options = ref<any[]>([])
const fillAnswer = ref<any[]>([])
const singleSelected = ref<string>()
const multiSelected = ref<string[]>([])
const collectInfo = ref(uncollectInfo)
let clickCollect = false

// 工具函数：安全解析 JSON
const safeParse = (jsonString: string, defaultValue: any = []) => {
  try {
    return JSON.parse(jsonString)
  } catch (error) {
    console.error('解析 JSON 失败:', jsonString, error)
    return defaultValue
  }
}

/** 正确答案 */
const rightAnswer = computed(() => {
  const data = safeParse(props.info.rightAnswer || '[]', [])
  return data.join('、')
})

/** 收藏或取消收藏 */
const collectTopic = () => {
  if (clickCollect) return
  clickCollect = true
  const info = props.info
  const url = `api/TopicCollecte/${info.collected ? 'UnCollecte' : 'Collecte'}`
  http
    .get(url, { params: { topicId: info.id, groupName: '默认' } })
    .right(() => {
      info.changeCollect(!info.collected)
      collectInfo.value = info.collected ? collectedInfo : uncollectInfo
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
    .finally(() => {
      clickCollect = false
    })
}

/** 提交答题 */
const submit = () => {
  let data: string[] = []
  const type = props.info.type

  if (type === '单选') {
    data = [singleSelected.value || '']
  } else if (type === '多选') {
    data = multiSelected.value
  } else if (type === '填空') {
    data = fillAnswer.value.map((item) => item.value)
  } else if (type === '编程') {
    data = [exeAnswer.value || '']
  }

  if (data.length > 0) {
    sureSubmit(data)
  }
}

/** 确认提交答题 */
const sureSubmit = (answers: string[]) => {
  submiting.value = true
  const info = props.info
  if (props.testId) {
    const number = info.baseInfo.number
    const payload = { TestId: props.testId, Number: number, Answer: answers }
    http
      .post('api/MockTest/Answer', payload)
      .right(() => {
        info.checkTest(JSON.stringify(answers))
        emit('submit', true)
      })
      .error((res) => {
        showNotify({
          message: res.Msg,
          duration: 2000,
          type: 'danger'
        })
      })
      .finally(() => (submiting.value = false))
  } else {
    const url = `api/Topic/Check?topicId=${info.id}`
    http
      .post(url, answers)
      .right((res) => {
        const { Analysis, RightAnswer, IsTrue } = res.Data
        info.answered(Analysis, RightAnswer, IsTrue)
        emit('submit', IsTrue)
      })
      .error((res) => {
        showNotify({
          message: res.Msg,
          duration: 2000,
          type: 'danger'
        })
      })
      .finally(() => (submiting.value = false))
  }
}

/** 处理题目信息 */
const dealTopicInfo = () => {
  // 记录原始数据
  console.log('当前题目数据:', props.info)

  // 题目难度
  level.value = Math.round((1 - props.info.level) * 5)

  // 收藏图标信息
  collectInfo.value = props.info.collected ? collectedInfo : uncollectInfo

  // 处理可选项集合
  options.value = safeParse(props.info.options || '[]')

  // 处理用户答案
  if (props.info.answer) {
    const answers = safeParse(props.info.answer, [])
    if (answers.length > 0) {
      switch (props.info.type) {
        case '单选':
          singleSelected.value = answers[0]
          break
        case '编程':
          exeAnswer.value = answers[0]
          break
        case '多选':
          multiSelected.value = answers
          break
        case '填空':
          fillAnswer.value = answers.map((value: string) => ({ value }))
          break
      }
    }
  } else {
    // 重置答案
    singleSelected.value = ''
    multiSelected.value = []
    fillAnswer.value = []
    exeAnswer.value = ''
  }

  // 处理题目内容
  if (props.info.type === '填空') {
    const isTested = props.info.baseInfo.isTested
    const replaceFunc = (match: string) => {
      if (!isTested) fillAnswer.value.push({ value: '' })
      return match.slice(1, -1)
    }
    let newContent = props.info.content.replace(/\[_+\]/g, replaceFunc)
    // 如果没有 [_] 占位符但又是填空类型，在末尾加一个输入框
    if (fillAnswer.value.length === 0 && !isTested) {
      fillAnswer.value.push({ value: '' })
    }
    content.value = newContent
  } else {
    content.value = props.info.content
  }
}

// 监听 props.info 变化
watch(
  () => props.info,
  () => dealTopicInfo(),
  { immediate: true }
)

onBeforeMount(() => {
  dealTopicInfo()
})
</script>

<style>
.header-row {
  @apply flex items-center p-2 border-b;
}

.bar-item-center-left {
  @apply flex-row items-center justify-center flex-shrink-0 min-w-0 text-[15px];
  --van-action-bar-icon-active-color: transparent;
}

.bar-icon-big {
  @apply my-0 ml-0 mr-1 text-[22px];
}
</style>