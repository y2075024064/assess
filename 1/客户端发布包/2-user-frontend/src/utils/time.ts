export const formatTime = (val: string) => {
  const now = new Date()
  const old = new Date(val)
  const n_time = now.getTime()
  const o_time = old.getTime()
  const diff = Math.floor((n_time - o_time) / 60000) //分钟

  if (diff <= 0) {
    return '刚刚'
  } else {
    const n_d = Math.floor(n_time / 86400000)
    const o_d = Math.floor(o_time / 86400000)
    const d_diff = n_d - o_d
    switch (d_diff) {
      case 0:
        if (diff < 60) return diff.toFixed() + ' 分钟前'
        else return Math.floor(diff / 60) + ' 小时前'
      case 1:
        return '昨天'
      case 2:
        return '2 天前'
      case 3:
        return '3 天前'
      default: {
        const n_y = now.getFullYear()
        const o_t = {
          y: old.getFullYear(),
          m: old.getMonth() + 1,
          d: old.getDate()
        }
        if (n_y == o_t.y) return `${o_t.m}月${o_t.d}日`
        else {
          let y = o_t.y.toString()
          y = y.substring(y.length - 2)
          return `${y}年${o_t.m}月${o_t.d}日`
        }
      }
    }
  }
}
