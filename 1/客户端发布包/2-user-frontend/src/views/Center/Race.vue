<template>
  <van-nav-bar
    fixed
    left-arrow
    placeholder
    title="全部比赛"
    safe-area-inset-top
    class="flex-shrink-0"
    @click-left="urlBack"
  />

  <!-- 菜单 -->
  <van-tabs v-model:active="active" swipeable class="h-full">
    <van-tab
      v-for="(item, index) in ['全部', '已报名', '去参赛']"
      class="h-full"
      :title="item"
      :key="index"
    >
      <!-- 比赛列表 -->
      <van-list
        v-model:loading="loading"
        :finished="finished"
        finished-text="没有更多了"
        @load="onLoad"
      >
        <!-- 已报名 -->
        <van-cell-group v-if="active === 0 || active === 1">
          <van-cell
            is-link
            center
            :title="item.Name"
            :label="item.ExaminationId"
            v-for="(item, index) in myExamList"
            @click="getApplyExamDetail(index)"
            :key="index"
          >
            <template #value>
              <van-button type="warning" size="small" v-if="!item.IsTested"> 未参加 </van-button>
              <van-button
                type="success"
                size="small"
                v-else
                @click.stop="getAnswerResult(item.ExamineRecordId)"
              >
                查看结果
              </van-button>
            </template>
          </van-cell>
        </van-cell-group>
        <!-- 未报名 -->
        <van-cell-group v-if="active === 0 || active === 2">
          <van-cell
            is-link
            center
            :title="item.Name"
            :label="item.Number"
            v-for="(item, index) in allExamList"
            @click="getExamDetail(item.Id)"
            :key="index"
          >
            <template #value>
              <van-button type="success" size="small" @click.stop="applyExam(item.Id)"
                >报名</van-button
              >
            </template>
          </van-cell>
        </van-cell-group>
      </van-list>
    </van-tab>
  </van-tabs>

  <!-- 详细信息窗口 -->
  <van-overlay :show="showBox" @click="showBox = false">
    <div class="flex h-full items-center justify-center">
      <div
        v-if="showType === 'exam'"
        class="flex flex-col justify-center gap-[0.5rem] w-[80vw] bg-white rounded-[0.5rem] p-[1rem] text-[14px]"
      >
        <div>考试名称：{{ examDetail?.Name || '未知' }}</div>
        <div>满分：{{ examDetail?.TotalScore }} 分</div>
        <div>考试时长：{{ examDetail?.Duration }} 分钟</div>
        <div>考试开始时间：{{ examDetail?.StartDate || '未知' }}</div>
        <div>考试结束时间：{{ examDetail?.EndDate || '未知' }}</div>
        <div>主办方：{{ examDetail?.Organizer || '未知' }}</div>
        <div>协办方：{{ examDetail?.CoOrganizer || '未知' }}</div>
      </div>
      <div
        v-else
        class="flex flex-col justify-center gap-[0.5rem] w-[80vw] bg-white rounded-[0.5rem] p-[1rem] text-[14px]"
      >
        <div>考试名称：{{ applyExamDetail?.Name || '未知' }}</div>
        <div>
          考试得分：{{ applyExamDetail?.Score || '暂无成绩'
          }}{{ applyExamDetail?.Score ? ' 分' : '' }}
        </div>
        <div>是否支付：{{ applyExamDetail?.IsPay ? '是' : '否' }}</div>
        <div>报名时间：{{ applyExamDetail?.ApplyTime || '未知' }}</div>
        <div>考试开始时间：{{ applyExamDetail?.StartDate || '未知' }}</div>
        <div>考试开考时间：{{ applyExamDetail?.StartTime || '未知' }}</div>
        <div>
          距离考试开始：
          {{
            applyExamDetail!.Days >= 0
              ? applyExamDetail!.Days
              : '已开始 ' + -applyExamDetail!.Days + ' 天'
          }}
        </div>
      </div>
    </div>
  </van-overlay>
</template>

<script setup lang="ts">
import http from '@http'
import { ref, onBeforeMount } from 'vue'
import type { ExamInfo } from '@/types/ExamInfo'
import type { ApplyExamInfo } from '@/types/ApplyExamInfo'
import type { ExamDetailInfo } from '@/types/ExamDetailInfo'
import { showNotify } from 'vant'
import { useRouter } from 'vue-router'

const allExamList = ref<ExamInfo[]>([]) // 全部比赛
const myExamList = ref<ApplyExamInfo[]>([]) // 个人参加的比赛
const examDetail = ref<ExamDetailInfo>()
const applyExamDetail = ref<ApplyExamInfo>()
const loading = ref(false)
const finished = ref(false)
const Page = ref(1)
const Take = ref(5)
const total = ref(1)
const showBox = ref(false)
const showType = ref('answer' || 'exam')
const active = ref(0)

const urlBack = () => history.back()
const router = useRouter()

const onLoad = () => {
  if (total.value > Page.value * Take.value) {
    loading.value = true
    finished.value = false
    Page.value++
    getMyExam()
  } else {
    finished.value = true
  }
}

/** 获取全部比赛 */
const getAllExam = () => {
  http
    .get('api/Exam/Latest')
    .right((res) => {
      allExamList.value = res.Data
      loading.value = false
      finished.value = true
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        type: 'danger',
        duration: 2000
      })
    })
}

/** 获取个人报名的比赛 */
const getMyExam = () => {
  http
    .post('api/ExamApply/MyExam', {
      Page: Page.value,
      Take: Take.value,
      OrderRule: 'Asc'
    })
    .right((res) => {
      myExamList.value = res.Data || []
      total.value = res.Total || 0
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        type: 'danger',
        duration: 2000
      })
    })
}

/** 展示比赛详细信息 */
const getApplyExamDetail = (id: number) => {
  showBox.value = true
  showType.value = 'answer'
  applyExamDetail.value = myExamList.value[id]
}

/** 获取比赛详细信息 */
const getExamDetail = (id: number) => {
  http
    .get('api/Exam/Detail', { params: { id } })
    .right((res) => {
      examDetail.value = res.Data
      showBox.value = true
      showType.value = 'exam'
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        type: 'danger',
        duration: 2000
      })
    })
}

/** 获取已参加的比赛的做题信息 */
const getAnswerResult = (recordId: number) => {
  http
    .get('api/ExamApply/AnswerInfo', { params: { recordId } })
    .right(() => {
      // 跳转习题页，但不可做题
      router.push({ name: 'practice', query: { type: 'check' } })
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        type: 'danger',
        duration: 2000
      })
    })
}

/** 报名比赛 */
const applyExam = (id: number) => {
  http
    .get('api/Exam/Apply', { params: { examId: id } })
    .right((res) => {
      showNotify({
        message: res.Msg,
        type: 'success',
        duration: 2000
      })
    })
    .error((res) => {
      showNotify({
        message: res.Msg,
        type: 'danger',
        duration: 2000
      })
    })
}

onBeforeMount(() => {
  getAllExam()
  getMyExam()
})
</script>

<style>
.van-tabs__content--animated {
  overflow: hidden;
  height: 100%;
}
</style>
