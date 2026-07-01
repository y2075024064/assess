import { TopicInfo } from './TopicInfo'

export class TopicDetail {
  /**ID */
  id: number = 0

  /**类型 */
  type: string = '单选'

  /**难度 */
  level: number = 1.0

  /**内容 */
  content: string = ''

  /**可选项 */
  options: string = '[]'

  /**评论数 */
  commentNum: number = 0

  /**正确答案 */
  rightAnswer: string = ''

  /**用户答案 */
  answer: string = ''

  /**答案解析 */
  analysis: string = ''

  /**收藏数 */
  collectionNum: number = 0

  /**是否已收藏 */
  collected: boolean = false

  /**是否原创 */
  isOriginal: boolean = false

  /**来源 */
  origin: string = ''

  /**题目基础信息对象 */
  baseInfo: TopicInfo

  constructor(info?: TopicInfo) {
    if (info) {
      this.baseInfo = info
      this.answer = info.answer
    } else {
      this.baseInfo = new TopicInfo(0)
    }
  }

  changeCollect(val: boolean) {
    this.collected = val
  }

  answered(analysis: string, rightAnswer: string, isTrue: boolean) {
    this.analysis = analysis
    this.rightAnswer = rightAnswer
    this.baseInfo.tested(isTrue)
  }

  checkTest(answer: string) {
    this.baseInfo.answer = answer
    this.baseInfo.tested()
  }
}
