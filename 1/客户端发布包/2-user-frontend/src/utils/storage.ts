/**
 * 通过键名从 本地储存 中获取值
 * @param key 键名
 * @returns   值
 */
export const getItem = (key: string) => {
  const data = window.localStorage.getItem(key)
  try {
    // 试着转换为对象
    if (data) {
      return JSON.parse(data)
    } else {
      return null
    }
  } catch (err) {
    // 转换失败则为普通字符串
    return data
  }
}

/**
 * 在 本地储存 中添加或设置键值
 * @param key   键名
 * @param value 值
 */
export const setItem = (key: string, value: any) => {
  let strVal = ''
  const type = typeof value

  if (type === 'string') {
    strVal = value
  } else if (type === 'object') {
    // 如果是对象则转换成 json 串
    strVal = JSON.stringify(value)
  } else {
    strVal = value?.toString()
  }

  window.localStorage.setItem(key, strVal)
}

/**
 * 通过键名从 本地储存 中移除
 * @param key 键名
 */
export const removeItem = (key: string) => {
  window.localStorage.removeItem(key)
}
