<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    :title="title"
    safe-area-inset-top
    class="van-nav-bar flex-shrink-0"
    @click-left="onClickLeft"
  />

  <div id="test-body">
    <!-- 头部内容部分 -->
    <div class="flex-shrink-0">
      <!-- 第一行内容 如果是模拟测评，则渲染倒计时 -->
      <div class="count-down" v-if="isMockTest">
        <van-count-down
          ref="countDownRef"
          :time="remainTime"
          @change="timerChange"
          @finish="timerFinish"
        />
      </div>
    </div>

    <!-- 轮播-当前渲染的题目列表 -->
    <van-swipe
      ref="swipe"
      :show-indicators="false"
      class="h-full flex-grow-0"
      @change="swipeChange"
    >
      <van-swipe-item v-for="(item, index) in swipeList" :key="index" class="overflow-auto">
        <TestBox
          :info="item"
          :test-id="testId"
          :tested-number="testedNumber"
          @submit="onTopicSubmited"
          @submit-test="submitTest"
          @show-list="showTopicList = true"
        />
      </van-swipe-item>
    </van-swipe>
  </div>

  <!-- 题目数量信息 -->
  <van-popup
    :overlay-style="{ 'z-index': 30, position: 'absolute' }"
    style="z-index: 40; padding: 12px; height: 60%"
    v-model:show="showTopicList"
    teleport="#test-body"
    position="bottom"
  >
    <van-space wrap fill :size="10">
      <span
        v-for="(item, index) in topicList"
        :class="'topic-' + item.class"
        class="topic-number-item"
        :key="index"
        @click="gotoItem(index)"
      >
        {{ index + 1 }}
      </span>
    </van-space>
  </van-popup>

  <!-- 模拟测评结果弹窗 -->
  <van-dialog
    v-model:show="showTestResult"
    @closed="testResultDialogClosed"
    @confirm="showTestResult = false"
  >
    <van-cell-group>
      <van-cell title="总分数" :value="testResult.TotalScore" />
      <van-cell title="得分数" :value="testResult.Score" />
      <van-cell title="正确数" :value="testResult.RightCount" />
      <van-cell title="错误数" :value="testResult.ErrorCount" />
    </van-cell-group>
  </van-dialog>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeMount, getCurrentInstance, computed } from 'vue'
import type { CountDownInstance, CountDownCurrentTime } from 'vant'
import { showNotify, showConfirmDialog } from 'vant'
import { TopicDetail } from '@/types/TopicDetail'
import { useRoute, useRouter } from 'vue-router'
import TestBox from './components/TestBox.vue'
import { TopicInfo } from '@/types/TopicInfo'
import http from '@http'

let _this: any | null = null

const countDownRef = ref<CountDownInstance | null>(null) //计时器
const type = ref<string | undefined>('') //当前页面类型
const swipeList = ref<TopicDetail[]>([]) //轮播列表
const topicList = ref<TopicInfo[]>([]) //题目信息列表
const showTestResult = ref(false) //是否显示模拟测评结果
const showTopicList = ref(false) //是否显示题目列表
const testResult = ref<any>({}) //模拟测评结果
const remainTime = ref(1200000) //倒计时
const totalTopicCount = ref(0) //题目总数
const isMockTest = ref(false) //是否是模拟测试
const testId = ref<number>() //模拟测评ID
const currIndex = ref(-1) //当前题目的索引
const title = ref('') //标题

const testedNumber = computed(() => `${currIndex.value + 1} / ${totalTopicCount.value}`) //已练习与总题目数
const router = useRouter()

let isLeva = false
let currentTime = 0
let getTopicsUrl = ''
let submitTesting = false
let currSwipeIndex = 0 //当前轮播索引

/** 返回 */
const onClickLeft = () => {
  // 标签练习返回时回到搜索引擎（替换当前历史，实现逐层返回）
  if (type.value === 'tag') {
    router.replace({ name: 'test', query: { openTagSearch: '1' } })
    return
  }
  history.back()
}

/** 加载标题并生成路由参数 */
const loadTitleAndCreateParams = (): any | null => {
  const route = useRoute()
  type.value = route.query.type?.toString()

  getTopicsUrl = 'api/'
  switch (type.value) {
    case 'info':
      title.value = '试卷回顾'
      getTopicsUrl += 'ExamApply/AnswerInfo'
      return { recordId: route.query.recordId }
    case 'order':
      getTopicsUrl += 'Topic/OrderList'
      title.value = '顺序练习'
      break
    case 'tag':
      getTopicsUrl += 'Topic/OrderList'
      title.value = '标签练习'
      break
    case 'random':
      getTopicsUrl += 'Topic/RandomList'
      title.value = '随机练习'
      break
    case 'part':
      getTopicsUrl += 'Topic/OrderList'
      title.value = '专项练习'
      break
    case 'error':
      getTopicsUrl += 'TopicError/TotalInfo'
      title.value = '错误题集'
      return { partId: route.query.partId || '' }
    case 'test':
      getTopicsUrl += 'MockTest/TopicList'
      isMockTest.value = true
      title.value = '模拟测评'
      break
    case 'collecte':
      getTopicsUrl += 'TopicCollecte/GetList'
      title.value = '收藏练习'
      return { groupName: route.query.groupName }
  }

  const { grade, partId, tag } = route.query
  return { grade, partId, tag }
}

/** 答案提交后 */
const onTopicSubmited = (val: boolean) => {
  if (val) setTimeout(() => swipeNextPage(), 2000)
}

/** 模拟测评结果弹窗关闭时 */
const testResultDialogClosed = () => {
  router.push('/')
}

/** 提交测评到服务器 */
const submitTestToServer = () => {
  http
    .get('api/MockTest/SubmitTest', { params: { testId: testId.value || '' } })
    .right((res) => {
      testResult.value = res.Data
      showTestResult.value = true
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
    .finally(() => {
      submitTesting = false
    })
}

/** 提交测评 */
const submitTest = () => {
  if (submitTesting) return

  showConfirmDialog({
    title: '操作确认',
    message: '确认提交本次测评吗？'
  })
    .then(() => {
      submitTestToServer()
    })
    .catch(() => {
      submitTesting = false
    })
}

/** 轮播页改变事件 */
const swipeChange = (index: number) => {
  // 将上一个题目活动的状态移除掉
  swipeList.value[currSwipeIndex].baseInfo.removeActive()

  // 更新当前轮播索引
  currSwipeIndex = index
  const info = swipeList.value[index]

  // 设置当前题目的活动状态
  const diff = info.baseInfo.index - currIndex.value
  currIndex.value = info.baseInfo.index
  info.baseInfo.setActive()

  if (diff == 1 || diff == 1 - totalTopicCount.value) {
    // 如果用户是往右划的，则向索引递增的方向获取题目详情
    let index1 = currIndex.value + 1
    index1 = index1 == totalTopicCount.value ? 0 : index1
    getTopicInfo(index1, false)
  } else {
    // 反之，，则向索引递减的方向获取题目详情
    let index2 = currIndex.value - 1
    index2 = index2 == -1 ? totalTopicCount.value - 1 : index2
    getTopicInfo(index2, true)
  }
}

/** 轮播跳转到下一页 */
const swipeNextPage = () => {
  if (_this && _this.$refs.swipe) {
    const swipe = _this.$refs.swipe as any
    swipe.next()
  }
}

/** 跳转到指定题目 */
const gotoItem = (index: number) => {
  if (index < totalTopicCount.value && index > -1 && index !== currIndex.value) {
    if (currIndex.value > -1) {
      topicList.value[currIndex.value].removeActive()
    }
    topicList.value[index].setActive()
    showTopicList.value = false
    getTopicInfo(index)

    const total = totalTopicCount.value
    if (total > 1) {
      if (index > 0 || total > 2) {
        const index1 = (index > 0 ? index : total) - 1
        getTopicInfo(index1, true)
      }

      if (index < total - 1 || total > 2) {
        const index2 = index < total - 1 ? index + 1 : 0
        getTopicInfo(index2, false)
      }
    }
  }
}

/** 改变当前渲染题目集合 */
const changeSwipeList = (detail: TopicDetail, isLeft: boolean | null = null) => {
  // 如果是属于直接跳转类型，则直接覆盖当前界面
  if (isLeft === null) {
    swipeList.value[currSwipeIndex] = detail
    currIndex.value = detail.baseInfo.index
  }
  // 如果是预加载 左边
  else if (isLeft) {
    let index1 = currSwipeIndex - 1
    index1 = index1 < 0 ? swipeList.value.length - 1 : index1
    swipeList.value[index1] = detail
  }
  // 否则为预加载 右边
  else {
    let index2 = currSwipeIndex + 1
    index2 = index2 >= swipeList.value.length ? 0 : index2
    swipeList.value[index2] = detail
  }
}

/** 获取题目信息 */
const getTopicInfo = (index: number, isLeft: boolean | null = null) => {
  // 判断索引是否正确
  if (index < 0 || index >= totalTopicCount.value) return

  // 生成请求参数
  const topicInfo = topicList.value[index]
  const isTested = isMockTest.value ? null : topicInfo.isTested
  const number = topicInfo.number
  const params = { number, isTested }

  http
    .get('api/Topic/TestInfo', { params })
    .right((res) => {
      const data = res.Data
      const detail = new TopicDetail(topicInfo)
      detail.collectionNum = data.CollectionNum
      detail.rightAnswer = data.RightAnswer
      detail.commentNum = data.CommentNum
      detail.collected = data.IsCollected
      detail.isOriginal = data.IsOriginal
      detail.analysis = data.Analysis
      detail.content = data.Content
      detail.options = data.Options
      detail.answer = data.Answer
      detail.origin = data.Origin
      detail.level = data.Level
      detail.type = data.Type
      detail.id = data.Id

      if (isMockTest.value) {
        detail.answer = topicInfo.answer
      }

      changeSwipeList(detail, isLeft)
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
}

/** 初始化 轮播列表 */
const initSwipeList = () => {
  for (let i = 0; i < 3 && i < totalTopicCount.value; i++) {
    const detail = new TopicDetail(topicList.value[i])
    swipeList.value.push(detail)
  }
}

const dealTestList = (data: any): number => {
  testId.value = data.TestId
  remainTime.value = (data.RemainTime || 0) * 1000
  const items = data.Items as any[]
  for (let i = 0; i < items.length; i++) {
    const item = items[i]
    topicList.value.push(new TopicInfo(i, item.Number, null, item.IsTested, item.Answer))
  }
  return data.CurrPostion
}

const dealOrderList = (data: any): number => {
  let currPostion = -1
  const array2 = data as any[]
  for (let i = 0; i < array2.length; i++) {
    const item = array2[i]
    topicList.value.push(new TopicInfo(i, item.Number, item.IsTrue, item.IsTested))
    if (item.IsTested) currPostion = i
  }

  if (currPostion < 0) {
    currPostion = 0
  }

  return currPostion
}

const dealInfoList = (data: any): number => {
  console.log(data)
  return 0
}

const dealOtherList = (data: any): number => {
  let currPostion = 0
  const array1 = data as string[]
  if (array1.length <= 0) {
    let message = ''
    if (type.value === 'random') {
      message = '该类别的题目已练习完毕'
    } else if (type.value === 'collecte') {
      message = '该分组暂无收藏的题目'
    } else {
      message = '您当前暂无错误题目'
    }
    showNotify({
      type: 'primary',
      duration: 3000,
      message
    })
    onClickLeft()
  }
  for (let i = 0; i < array1.length; i++) {
    topicList.value.push(new TopicInfo(i, array1[i]))
  }

  return currPostion
}

/** 获取题目列表 */
const getTopicList = () => {
  let currPostion = -1
  const params = loadTitleAndCreateParams()
  http
    .get(getTopicsUrl, { params })
    .right((res) => {
      // 如果是 模拟测评 模式
      if (type.value === 'test') {
        currPostion = dealTestList(res.Data)
      }
      // 如果试卷回顾
      else if (type.value === 'info') {
        currPostion = dealInfoList(res.Data)
      }
      // 如果是 随机练习、错题练习、收藏练习 模式
      else if (type.value === 'random' || type.value === 'error' || type.value === 'collecte') {
        currPostion = dealOtherList(res.Data)
      }
      // 如果是 顺序练习 模式
      else {
        currPostion = dealOrderList(res.Data)
      }

      totalTopicCount.value = topicList.value.length
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        duration: 2000,
        type: 'danger'
      })
    })
    .finally(() => {
      if (totalTopicCount.value > 0) {
        initSwipeList()
        gotoItem(currPostion)
      }
    })
}

/** 模拟测评倒计时结束 */
const timerFinish = () => {
  if (testId.value) submitTestToServer()
}

/** 倒计时变化事件 */
const timerChange = (time: CountDownCurrentTime) => {
  currentTime = time.total
}

/** 更新剩余时间 */
const updateRemainTime = () => {
  const remainTime = (currentTime / 1000).toFixed(0)
  const params = { testId, remainTime }
  http.get('api/MockTest/UpdateRemainTime', { params })
}

//界面加载之前
onBeforeMount(() => {
  _this = getCurrentInstance()
  if (_this && _this.proxy) {
    _this = _this.proxy
  }

  getTopicList()
})

onMounted(() => {
  if (isMockTest.value) {
    document.onvisibilitychange = (event: Event) => {
      console.log(event)
      if (document.visibilityState === 'hidden') {
        if (isLeva) return
        else isLeva = true
        countDownRef.value?.pause()
        updateRemainTime()
      } else {
        if (!isLeva) return
        else isLeva = false
        countDownRef.value?.start()
      }
    }
    window.onblur = (event: FocusEvent) => {
      console.log(event)
      if (isLeva) return
      else isLeva = true
      countDownRef.value?.pause()
      updateRemainTime()
    }
    window.onfocus = (event: FocusEvent) => {
      console.log(event)
      if (!isLeva) return
      else isLeva = false
      countDownRef.value?.start()
    }
  }
})
</script>

<style lang="scss" scoped>
#test-body {
  @apply h-full flex flex-col flex-grow relative overflow-hidden bg-white;

  .count-down {
    @apply text-xl flex-shrink-0 text-center py-1 border-b;
  }

  .topic-number-item {
    width: 40px;
    height: 40px;
    cursor: pointer;
    font-size: 13px;
    line-height: 40px;
    text-align: center;
    border-radius: 50%;
    border: 1px solid #dcdee0;
  }

  .topic-default {
    color: #323233;
    background-color: transparent;
  }

  .topic-active {
    color: white;
    background-color: #1989fa;
  }

  .topic-right {
    color: white;
    background-color: #07c160;
  }

  .topic-error {
    color: white;
    background-color: #ee0a24;
  }
}
</style>
