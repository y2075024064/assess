import axios from 'axios'
import router from '@/router'
import { useAppStore } from '@store/app'
import { showDialog, showNotify } from 'vant'

axios.defaults.baseURL = import.meta.env.VITE_BASE_URL
axios.defaults.timeout = 300000
axios.defaults.headers.post['Content-Type'] = 'application/json;charset=UTF-8'
// 允许跨域
axios.defaults.headers.post['Access-Control-Allow-Origin'] = '*'

// 添加请求拦截器
axios.interceptors.request.use(
  (config) => {
    const appStore = useAppStore()
    // 在发送请求之前添加 Header 参数
    config.headers['X-ClientType'] = appStore.clientType
    // 添加 访问凭证 参数
    const token = appStore.token
    if (token) {
      config.headers['X-Token'] = token
    }

    return config
  },
  (error) => {
    // 对请求错误 打印日志
    console.log(error)
    const appStore = useAppStore()
    if (!appStore.notidyShowed) {
      appStore.notidyShowed = true
      showNotify({
        message: '请求失败',
        type: 'danger',
        duration: 2000,
        onClose: () => {
          appStore.notidyShowed = false
        }
      })
    }

    return Promise.reject(error)
  }
)

axios.interceptors.response.use(
  (response) => {
    // 解析并处理响应数据
    const res = response.data

    // 如果是二进制流则直接返回
    if (ArrayBuffer.isView(res)) {
      return response
    }

    switch (res.Code) {
      // 请求失败
      case 0:
        break;
      // 请求成功
      case 200:
        break
      // 未登录
      case 401: {
        router.push('login')
        break
      }
      // 没有权限
      case 402: {
        const appStore = useAppStore()
        if (!appStore.notidyShowed) {
          appStore.notidyShowed = true
          showNotify({
            message: '您没有相应的访问权限！',
            type: 'warning',
            duration: 2000,
            onClose: () => {
              appStore.notidyShowed = false
            }
          })
        }

        break
      }
      // 拒绝请求
      case 406: {
        const appStore = useAppStore()
        if (!appStore.notidyShowed) {
          appStore.notidyShowed = true
          showNotify({
            message: '您的请求被拒绝！',
            type: 'danger',
            duration: 2000,
            onClose: () => {
              appStore.notidyShowed = false
            }
          })
        }

        break
      }
      // 参数解析失败
      case 412: {
        console.log('参数解析失败，' + res.Msg)
        const appStore = useAppStore()
        if (!appStore.dialogShowed) {
          appStore.dialogShowed = true
          showDialog({
            title: '错误',
            messageAlign: 'left',
            theme: 'round-button',
            message: '参数解析失败！'
          }).then(() => {
            appStore.dialogShowed = false
          })
        }

        break
      }
      // 服务器异常
      case 500: {
        console.log('服务器异常，' + res.Msg)
        const appStore = useAppStore()
        if (!appStore.dialogShowed) {
          appStore.dialogShowed = true
          showDialog({
            title: '错误',
            messageAlign: 'left',
            theme: 'round-button',
            message: '服务器异常！'
          }).then(() => {
            appStore.dialogShowed = false
          })
        }

        break
      }
    }

    return response
  },
  (error) => {
    // 对响应错误 打印日志
    console.error(error)
    const appStore = useAppStore()
    if (!appStore.notidyShowed) {
      appStore.notidyShowed = true
      showNotify({
        message: '网络异常',
        type: 'danger',
        duration: 2000,
        onClose: () => {
          appStore.notidyShowed = false
        }
      })
    }

    return Promise.reject(error)
  }
)

export default axios
