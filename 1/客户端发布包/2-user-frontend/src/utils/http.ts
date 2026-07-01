import type { AxiosRequestConfig } from 'axios'
import axios from '@/utils/axios'

class MyPromise<T = any> {
  private rightFunc: (result: T) => void = () => {}
  private errorFunc: (result: T) => void = () => {}
  private catchFunc: (err: any) => void = () => {}
  private finallyFunc: () => void = () => {}

  constructor(
    executor: (
      right: (result: T) => void,
      error: (result: T) => void,
      exception: (err: any) => void,
      over: () => void
    ) => void
  ) {
    executor(
      (res) => this.rightFunc(res),
      (res) => this.errorFunc(res),
      (err) => this.catchFunc(err),
      () => this.finallyFunc()
    )
  }

  right(func: (result: T) => void) {
    this.rightFunc = func
    return this
  }

  error(func: (result: T) => void) {
    this.errorFunc = func
    return this
  }

  catch(func: (err: Error) => void) {
    this.catchFunc = func
    return this
  }

  finally(func: () => void) {
    this.finallyFunc = func
    return this
  }
}

interface HttpResult {
  IsSucc: boolean
  Code: number
  Msg: string
  Data: any
  Total?: number
}

type Method1 = 'get' | 'delete' | 'head' | 'options'
type Method2 = 'post' | 'put' | 'patch' | 'postForm' | 'putForm' | 'patchForm'

const request = function (
  method: Method1,
  url: string,
  config?: AxiosRequestConfig<any> | undefined
) {
  return new MyPromise<HttpResult>((right, error, exception, over) => {
    axios[method](url, config)
      .then((res) => {
        const result = res.data as HttpResult
        if (result.IsSucc) {
          right(result)
        } else {
          error(result)
        }
      })
      .catch((err) => exception(err))
      .finally(() => over())
  })
}

const requestWithData = function (
  method: Method2,
  url: string,
  data?: any | undefined,
  config?: AxiosRequestConfig<any> | undefined
) {
  return new MyPromise<HttpResult>((right, error, exception, over) => {
    axios[method](url, data, config)
      .then((res) => {
        const result = res.data as HttpResult
        if (result.IsSucc) {
          right(result)
        } else {
          error(result)
        }
      })
      .catch((err) => exception(err))
      .finally(() => over())
  })
}

const http = {
  get: function (url: string, config?: AxiosRequestConfig<any> | undefined) {
    return request('get', url, config)
  },
  getFile: function (url: string, config?: AxiosRequestConfig<any> | undefined) {
    if (config) config.responseType = 'arraybuffer'
    else config = { responseType: 'arraybuffer' }
    return new MyPromise<ArrayBuffer>((right, error, exception, over) => {
      axios
        .get(url, config)
        .then((res) => {
          right(res.data)
        })
        .catch((err) => exception(err))
        .finally(() => over())
    })
  },
  delete: function (url: string, config?: AxiosRequestConfig<any> | undefined) {
    return request('delete', url, config)
  },
  head: function (url: string, config?: AxiosRequestConfig<any> | undefined) {
    return request('head', url, config)
  },
  options: function (url: string, config?: AxiosRequestConfig<any> | undefined) {
    return request('options', url, config)
  },
  post: function (url: string, data?: any, config?: AxiosRequestConfig<any> | undefined) {
    return requestWithData('post', url, data, config)
  },
  put: function (url: string, data?: any, config?: AxiosRequestConfig<any> | undefined) {
    return requestWithData('put', url, data, config)
  },
  patch: function (url: string, data?: any, config?: AxiosRequestConfig<any> | undefined) {
    return requestWithData('patch', url, data, config)
  },
  postForm: function (url: string, data?: any, config?: AxiosRequestConfig<any> | undefined) {
    return requestWithData('postForm', url, data, config)
  },
  putForm: function (url: string, data?: any, config?: AxiosRequestConfig<any> | undefined) {
    return requestWithData('putForm', url, data, config)
  },
  patchForm: function (url: string, data?: any, config?: AxiosRequestConfig<any> | undefined) {
    return requestWithData('patchForm', url, data, config)
  }
}

export default http
