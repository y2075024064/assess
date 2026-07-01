<template>
  <van-nav-bar fixed placeholder left-arrow title="课程详情" safe-area-inset-top class="flex-shrink-0"
    @click-left="onClickLeft" />
  <div class="w-full h-full flex-grow overflow-x-hidden overflow-y-auto">
    <div class="w-full">
      <van-image height="200" width="100%" fit="cover" :src="courseImg" />
      <div class="flex flex-col justify-center p-[10px] bg-white">
        <div class="h-[50px] w-[95%] font-bold text-lg">【计算机思维课堂-2023-06】中小学人工智能的思考与实践----慕课堂</div>
        <div class="flex h-[35px] text-sm items-center">
          <div class="text-gray-600">价格</div>
          <div class="text-xl text-red-500 ml-[10px]">免费</div>
        </div>
        <div class="flex h-[35px] text-sm items-center">
          <div class="text-gray-600">学习有效期</div>
          <div class="ml-[10px]">长期有效</div>
        </div>
      </div>
    </div>

    <div class="mt-[10px]">
      <van-tabs v-model="active" sticky @click-tab="navChange">
        <van-tab :title="item" v-for="(item, index) in navList" :key="index" />
        <div class="bg-white p-[10px]">
          <div class="p-[10px 0] font-bold" id="nav0">简介</div>
          <div class="text-sm mt-[10px] mb-[10px]">
            本周六（10月21日），元卓学堂邀请北京市十一学校人工智能课程负责人郑子杰老师，
            围绕人工智能课程在北京市十一学校的开展与人工智能生态建设进行分享。
          </div>
          <van-image height="100%" width="100%" fit="cover" :src="courseInfo" />
        </div>
        <div class="bg-white p-[10px] mt-[10px]" id="nav1">
          <div class="p-[10px 0] font-bold">教师风采</div>
          <div class="text-sm mt-[10px]">
            暂无
          </div>
        </div>
        <div class="bg-white p-[10px] mt-[10px]" id="nav2">
          <div class="p-[10px 0] font-bold">课程目录</div>
          <ul class="text-sm mt-[10px] cursor-pointer">
            <li v-for="(item, index) in Catalog" :key="index"
              class="flex justify-between bg-gray-100 rounded-md my-[8px] py-[6px] px-[4px] hover:bg-gray-200 transition duration-300"
              @click="goToVideoDetail(item.Id)">
              <div class="flex items-center">
                <van-icon name="play-circle-o" size="20" class="mr-2" /> 
                <div class="flex flex-col">
                  <span class="font-semibold">{{ item.Title }}</span>
                  <span :class="{ 'text-green-500': item.IsFree, 'text-red-500': !item.IsFree }" class="mt-1">{{ item.IsFree ? '免费' : '付费' }}</span>
                </div>
              </div>
              <div class="text-gray-500 self-center">{{ item.FormattedDuration }}</div>
            </li>
          </ul>
        </div>
      </van-tabs>
    </div>
  </div>
</template>
  
<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import http from '@/utils/http' // 确保路径正确
import courseImg from '@assets/images/login-bg2.png'
import courseInfo from '@assets/images/course.png'

const courseId = ref(0) // 当前课程的id
const active = ref(0) // 简易导航栏当前的id
const brief = ref('') // 课程内容的简介
const Catalog = ref([]) // 课程视频的目录
const navList = ['简介', '教师', '目录']
const route = useRoute()
const router = useRouter()

const onClickLeft = () => router.push('/Course') 

/** 获取课程id */ 
const loadIdParams = (): any | null => {
  const id = route.query.CourseId // 修正为 CourseId
  console.log('Loaded Course Id:', id); // 调试信息
  return id
}

/** 获取课程详细信息 */
const getCourseDetail = async () => {
  const id = loadIdParams();
  if (!id) {
    console.error('No valid Course Id found'); // 调试信息
    return;
  }

  try {
    console.log('Fetching course details for CourseId:', id); // 调试信息
    const response = await new Promise((resolve, reject) => {
      http.post('/api/CourseVideo/Page', {
        CourseId: parseInt(id),
        Page: 1,
        Take: 10
      }).right(resolve).catch(reject)
    })

    console.log('Response received:', response); // 调试信息

    if (response.IsSucc && response.Data) {
      Catalog.value = response.Data;
      console.log('Catalog updated with data:', Catalog.value); // 调试信息
    } else {
      console.error('Failed to fetch course details:', response.Msg); // 调试信息
    }
  } catch (error) {
    console.error('Error fetching course details:', error); // 调试信息
  }
}

/** 导航栏切换事件 */
const navChange = (info: any) => {
  active.value = info.name
  var component = document.getElementById("nav" + active.value);
  if (component) {
    component.scrollIntoView({ behavior: "smooth" });
  }
}

/** 跳转到视频详情页 */
const goToVideoDetail = (videoId: number) => {
  router.push({ path: '/VideoDetail', query: { videoId: videoId } })
}

console.log('Component mounted and calling getCourseDetail'); // 调试信息
onMounted(() => {
  console.log('Route query parameters:', route.query); // 调试信息
  getCourseDetail()
})
</script>

<style>
/* 自定义样式 */
.video-list-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: #f1f1f1;
  border-radius: 8px;
  margin-bottom: 8px;
  padding: 8px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.video-list-item:hover {
  background-color: #e0e0e0;
}

.video-title {
  font-weight: bold;
}

.video-duration {
  color: #888;
}

.video-price-free {
  color: green;
}

.video-price-paid {
  color: red;
}
</style>



