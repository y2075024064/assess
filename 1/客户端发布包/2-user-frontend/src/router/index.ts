import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('@views/Layout/Index.vue'),
      children: [
        {
          path: '',
          name: 'layout',
          redirect: '/test',
        },
        {
          path: 'test',
          name: 'test',
          component: () => import('@views/Test/Index.vue')
        },
        {
          path: 'scoreList',
          name: 'scoreList',
          component: () => import('@views/Test/ScoreList.vue')
        },
        {
          path: 'scoreAnaly',
          name: 'scoreAnaly',
          component: () => import('@views/Test/ScoreAnaly.vue')
        },
        {
          path: '/activity',
          name: 'activity',
          component: () => import('@views/Activity/Index.vue')
        },
        {
          path: '/course',
          name: 'course',
          component: () => import('@views/Course/Index.vue'),
        },
        {
          path: '/email',
          name: 'email',
          component: () => import('@views/Letter/Index.vue'),
        },
        {
          path: '/user',
          name: 'user',
          component: () => import('@views/User/Index.vue')
        },
        {
          path: 'collectTopic',
          name: 'collectTopic',
          component: () => import('@views/User/CollectTopic.vue')
        },
        {
          path: 'scoreRecord',
          name: 'scoreRecord',
          component: () => import('@views/User/ScoreRecord.vue')
        },
        {
          path: 'apply',
          name: 'apply',
          component: () => import('@views/User/Apply.vue')
        },
        {
          path: 'testInfo',
          name: 'testInfo',
          component: () => import('@views/Test/TestInfo.vue')
        },
        {
          path: 'about',
          name: 'about',
          component: () => import('@views/Center/About.vue')
        },
        {
          path: 'editUser',
          name: 'editUser',
          component: () => import('@views/Center/EditUser.vue')
        },
        {
          path: 'contact',
          name: 'contact',
          component: () => import('@views/Center/Contact.vue')
        }
      ]
    },
    {
      path: '/practice',
      name: 'practice',
      component: () => import('@views/Test/Practice.vue')
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('@views/Auth/Login.vue')
    },
    {
      path: '/regist',
      name: 'regist',
      component: () => import('@views/Auth/Regist.vue')
    },
    {
      path: '/agreement',
      name: 'agreement',
      component: () => import('@views/Auth/Agreement.vue')
    },
    {
      path: '/letterDetail',
      name: 'letterDetail',
      component: () => import('@views/Letter/LetterDetail.vue')
    },
    {
      path: '/addLetter',
      name: 'addLetter',
      component: () => import('@views/Letter/AddLetter.vue')
    },
    {
      path: '/adContent',
      name: 'adContent',
      component: () => import('@views/Activity/AdContent.vue')
    },
    {
      path: '/thinkStar',
      name: 'thinkStar',
      component: () => import('@views/Activity/ThinkStar.vue')
    },
    {
      path: '/allRace',
      name: 'allRace',
      component: () => import('@views/Center/Race.vue')
    },
    {
      path: '/errorTopic',
      name: 'errorTopic',
      component: () => import('@views/User/ErrorTopic.vue')
    },
    {
      path: '/examineLetter',
      name: 'examineLetter',
      component: () => import('@views/Center/ExamineLetter.vue')
    },
    {
      path: '/courseDetail',
      name: 'courseDetail',
      component: () => import('@views/Course/CourseDetail.vue')
    },
    {
      path: '/dynamicList',
      name: 'dynamicList',
      component: () => import('@/views/Activity/DynamicList.vue')
    },
    {
      path: '/dynamicDetail',
      name: 'dynamicDetail',
      component: () => import('@/views/Activity/DynamicDetail.vue')
    },
    {
      path: '/videoDetail',
      name: 'videoDetail',
      component: () => import('@views/Course/VideoDetail.vue')
    },
    {
      path: '/minai',
      name: 'minai',
      component: () => import('@views/Center/MinaiChat.vue')
    }
  ]
})

if (window.self !== window.top) {
  router.beforeEach((to) => {
    parent.history.pushState(null, '', to.fullPath)
  })
}

export default router




