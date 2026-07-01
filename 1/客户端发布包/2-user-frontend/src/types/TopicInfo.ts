export class TopicInfo {
  /**当前项的索引 */
  index: number = 0

  /**编号 */
  number: string = ''

  /**用户答案 */
  answer: string = ''

  /**是否回答正确 */
  isTrue: boolean | null = null

  /**是否答题过 */
  isTested: boolean = false

  /**题目列表按钮样式类名 */
  class: string = 'default'

  /**
   * 题目基础信息
   * @param index 当前项的索引
   * @param number 编号
   * @param isTrue 是否正确
   * @param isTested 是否答题过
   * @param answer 用户答案
   */
  constructor(
    index: number,
    number: string = '',
    isTrue: boolean | null = null,
    isTested: boolean = false,
    answer: string = ''
  ) {
    this.index = index
    this.number = number
    this.isTrue = isTrue
    this.isTested = isTested
    this.answer = answer
    this.changeClass()
  }

  tested(isTrue: boolean | null = null) {
    this.isTested = true
    this.isTrue = isTrue
    this.changeClass()
  }

  setActive() {
    this.class = 'active'
  }

  removeActive() {
    this.changeClass()
  }

  changeClass() {
    if (this.isTrue === true) {
      this.class = 'right'
    } else if (this.isTrue === false) {
      this.class = 'error'
    } else if (this.isTested) {
      this.class = 'right'
    } else {
      this.class = 'default'
    }
  }
}
