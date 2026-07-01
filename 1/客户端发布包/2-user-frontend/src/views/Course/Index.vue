<template>
    <van-nav-bar fixed placeholder safe-area-inset-top class="custom-navbar flex-shrink-0">
        <template #title>
            <div class="nav-title"><span>📚</span> 计算思维课程 <span class="cq-tag">重庆</span></div>
        </template>
    </van-nav-bar>
    <van-dropdown-menu class="custom-dropdown">
        <van-dropdown-item v-model="selectedCategoryId" :options="categoryOptions" @change="handleCategoryChange" />
        <van-dropdown-item v-if="subcategoryOptions.length > 1 || selectedCategoryId !== 0" v-model="selectedSubcategoryId" :options="subcategoryOptions" @change="handleSubcategoryChange" />
        <van-dropdown-item v-model="selectedSortOption" :options="sortOptions" @change="handleSortChange" />
    </van-dropdown-menu>
    <div class="course-page-bg">
        <div class="course-list">
            <div v-for="(item, index) in courseList" :key="index" class="course-card" @click="gotoCourse(item.Id)">
                <div class="course-thumb">
                    <van-image height="100%" width="100%" fit="cover" :src="item.CoverImageUrl" />
                    <div class="course-price" v-if="item.Price > 0">
                        <span class="price-symbol">¥</span>{{ item.Price }}
                    </div>
                    <div class="course-free" v-else>免费</div>
                </div>
                <div class="course-body">
                    <div class="course-title">{{ item.Title }}</div>
                    <div class="course-desc">{{ item.Description }}</div>
                    <div class="course-footer">
                        <van-tag plain type="danger" v-if="item.DiscountPrice" class="discount-tag">
                            🔥 折扣价: ¥{{ item.DiscountPrice }}
                        </van-tag>
                    </div>
                </div>
            </div>
            <van-empty v-if="courseList.length === 0" description="暂无课程" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { ref, onMounted,reactive} from 'vue'
import http from '@/utils/http' // 确保路径正确
import { type CourseItem } from '@/types/CourseItem' // 导入 CourseItem 接口
import axios from 'axios'

const router = useRouter()
const categoryTree = ref([]) // 存储所有分类数据
const selectedCategoryId = ref(0) // 当前选中的顶级分类ID或子分类ID
const selectedSubcategoryId = ref(0) // 当前选中的子分类ID
const selectedSortOption = ref(0) // 当前选中的排序选项
const categoryOptions = ref([{ text: '全部课程', value: 0 }]) // 一级分类选项
const subcategoryOptions = ref([{ text: '课程类型', value: 0 }] as { text: string; value: number }[]) // 二级分类选项
const sortOptions = ref([
    { text: '最新', value: 0 },
    { text: '价格升序', value: 1 },
    { text: '价格降序', value: 2 }
])
const courseList = ref<CourseItem[]>([]) // 课程列表

// 获取课程分类树
const getCategoryTree = () => {
    http.get('/api/CourseCategory/GetCategoryTree')
        .right((response) => {
            if (response.IsSucc) {
                // 处理数据：为每个分类添加或替换 ParentId 字段
                categoryTree.value = response.Data.map((category: any) => ({
                    ...category,
                    ParentId: category.ParentId === undefined ? null : category.ParentId // 如果 ParentId 不存在则设为 null
                }))
                
                console.log('原始数据:', response.Data);
                console.log('处理后的数据:', categoryTree.value);

                // 构建一级分类选项
                categoryOptions.value = [
                    ...categoryOptions.value,
                    ...categoryTree.value.filter((category: any) => category.ParentId === null).map((category: any) => ({
                        text: category.Name,
                        value: category.Id
                    }))
                ];
                
                console.log('打印成功:', categoryOptions.value);
            } else {
                console.error('Failed to fetch category tree:', response.Msg);
            }
        })
        .catch((error) => {
            console.error('Failed to fetch category tree:', error);
        })
}

// 分页获取子分类
const getSubcategories = (parentId: number) => {
    http.post('/api/CourseCategory/Page', { ParentId: parentId })
        .right((response) => {
            if (response.IsSucc) {
                subcategoryOptions.value = [
                    { text: '课程类型', value: 0 },
                    ...response.Data.map((subcat: any) => ({
                        text: subcat.Name,
                        value: subcat.Id
                    }))
                ]
                console.log('Subcategories:', subcategoryOptions.value);
            } else {
                subcategoryOptions.value = [{ text: '暂无子分类', value: 0 }]
                console.error('No subcategories found:', response.Msg);
            }
        })
        .catch((error) => {
            console.error('Failed to fetch subcategories:', error);
            subcategoryOptions.value = [{ text: '加载失败', value: 0 }]
        })
}

// 获取课程分页列表
const getCoursesPage = (params: any) => {
    http.post('/api/Course/Page', params)
        .right((response) => {
            if (response.IsSucc) {
                console.log('Raw response data:', response.Data); // 打印原始响应数据
                courseList.value = response.Data.map((item: any) => ({
                    Id: item.Id, // 确保这里使用正确的属性名
                    Price: item.Price,
                    DiscountPrice: item.DiscountPrice,
                    Description: item.Description,
                    Title: item.Title,
                    CoverImageUrl: item.CoverImageUrl
                }));
                console.log('Mapped Courses:', courseList.value); // 打印映射后的数据
            } else {
                courseList.value = []
                console.error('Failed to fetch courses:', response.Msg);
            }
        })
        .catch((error) => {
            console.error('Failed to fetch courses page:', error);
            courseList.value = []
        })
}

// 处理一级分类变化
const handleCategoryChange = (value: number) => {
    selectedCategoryId.value = value
    selectedSubcategoryId.value = 0
    subcategoryOptions.value = [{ text: '课程类型', value: 0 }]
    if (value !== 0) {
        getSubcategories(value)
    }
    // 获取课程列表
    const params = buildParams()
    console.log('Fetching courses with params:', params);
    getCoursesPage(params)
}

// 处理二级分类变化
const handleSubcategoryChange = (value: number) => {
    selectedSubcategoryId.value = value
    // 获取课程列表
    const params = buildParams()
    console.log('Fetching courses with params:', params);
    getCoursesPage(params)
}

// 处理排序选项变化
const handleSortChange = (value: number) => {
    selectedSortOption.value = value
    // 获取课程列表
    const params = buildParams()
    console.log('Fetching courses with params:', params);
    getCoursesPage(params)
}

// 构建请求参数
const buildParams = () => {
    let categoryId = selectedSubcategoryId.value !== 0 ? selectedSubcategoryId.value : selectedCategoryId.value
    let orderField: string | null = null
    let orderRule: string | null = null

    switch (selectedSortOption.value) {
        case 0:
            orderField = 'CreateTime'
            orderRule = 'Desc'
            break
        case 1:
            orderField = 'Price'
            orderRule = 'Asc'
            break
        case 2:
            orderField = 'Price'
            orderRule = 'Desc'
            break
        default:
            orderField = 'CreateTime'
            orderRule = 'Desc'
            break
    }

    return {
        categoryId: categoryId === 0 ? null : categoryId,
        orderField: orderField,
        orderRule: orderRule,
        page: 1,
        take: 10
    }
}

// 跳转到课程详细页面
const gotoCourse = (Id: number) => {
    console.log('Clicked item ID:', Id); // 添加调试信息
    if (Id === undefined || Id === null) {
        console.error('Item ID is undefined or null');
    } else {
        const query = { CourseId: Id }
        console.log(query)
        router.push({ path: '/CourseDetail', query })
    }
}

onMounted(() => {
    getCategoryTree() // 初始化时获取分类树
    // 初始化时获取一次课程列表
    getCoursesPage(buildParams())
})
</script>

<style scoped>
.custom-navbar {
  --van-nav-bar-background: linear-gradient(135deg, #667eea, #764ba2);
  --van-nav-bar-title-text-color: #fff;
}
.nav-title {
  display: flex; align-items: center; justify-content: center; gap: 6px;
}
.cq-tag {
  @apply text-xs px-1.5 py-0.5 rounded-full ml-1;
  background: rgba(255,255,255,0.25); color: #fff; font-weight: 400;
}
.custom-dropdown { --van-dropdown-menu-background: #fff; }
.course-page-bg {
  min-height: calc(100vh - 46px); margin-top: 46px;
  padding: 12px 16px;
  background: linear-gradient(180deg, #f8f9ff 0%, #f0f2f5 100%);
}
.course-list {
  display: flex; flex-direction: column; gap: 16px; padding-bottom: 20px;
}
.course-card {
  background: #fff; border-radius: 16px; overflow: hidden;
  box-shadow: 0 2px 16px rgba(0,0,0,0.06); transition: all 0.3s; cursor: pointer;
}
.course-card:active { transform: scale(0.98); }
.course-thumb {
  position: relative; width: 100%; height: 160px; overflow: hidden;
}
.course-price {
  position: absolute; top: 10px; right: 10px;
  background: linear-gradient(135deg, #ff6b6b, #ee5a24);
  color: #fff; padding: 4px 10px; border-radius: 20px;
  font-size: 14px; font-weight: bold;
}
.price-symbol { font-size: 11px; }
.course-free {
  position: absolute; top: 10px; right: 10px;
  background: linear-gradient(135deg, #43e97b, #38f9d7);
  color: #fff; padding: 4px 12px; border-radius: 20px;
  font-size: 13px; font-weight: bold;
}
.course-body { padding: 14px 16px 16px; }
.course-title {
  font-size: 16px; font-weight: 700; color: #333; margin-bottom: 6px;
  overflow: hidden; text-overflow: ellipsis; white-space: nowrap;
}
.course-desc {
  font-size: 13px; color: #999;
  overflow: hidden; text-overflow: ellipsis; white-space: nowrap; margin-bottom: 8px;
}
.course-footer { display: flex; align-items: center; }
.discount-tag { font-size: 12px; padding: 3px 8px; border-radius: 12px; }
</style>
