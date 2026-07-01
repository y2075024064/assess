export function dragMoveX(element: Element | null, isMobile: boolean = false) {
  if (element == null) {
    return
  }

  var flag: boolean = false
  var startX: number = 0
  var scrollLeft: number = 0

  //如果是移动端
  if (isMobile) {
    //手指按下
    element.addEventListener('touchstart', (_event) => {
      var event = _event as TouchEvent
      startX = event.targetTouches[0].pageX
      scrollLeft = element.scrollLeft
    })

    //手指移动
    element.addEventListener('touchmove', (_event) => {
      var event = _event as TouchEvent
      var moveX = event.targetTouches[0].pageX - startX
      var scrollX = scrollLeft - moveX
      element.scrollLeft = scrollX < 0 ? 0 : scrollX
    })
  }
  //否则为 PC 端
  else {
    //鼠标按下
    element.addEventListener('mousedown', (_event) => {
      var event = _event as MouseEvent
      flag = true
      startX = event.clientX
      scrollLeft = element.scrollLeft
    })

    //鼠标移动
    element.addEventListener('mousemove', (_event) => {
      var event = _event as MouseEvent
      if (flag) {
        var moveX = event.clientX - startX
        var scrollX = scrollLeft - moveX
        element.scrollLeft = scrollX < 0 ? 0 : scrollX
      }
    })

    //鼠标抬起
    element.addEventListener('mouseup', (_event) => {
      flag = false
    })

    //鼠标移开
    element.addEventListener('mouseleave', (_event) => {
      flag = false
    })
  }
}
