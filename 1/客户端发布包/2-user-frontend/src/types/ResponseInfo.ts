export class ResponseInfo {
  /** 用户id */
  Id: number = 0

  /** 回复时间 */
  CreationTime: string = ''

  /** 点赞数 */
  LikeNum: number = 0

  /** 是否收藏 */
  IsLike: boolean = false

  MemberId: number = 0

  MemberType: string = ""

  /** 用户名 */
  Nickname: string = ""

  /** 用户头像 */
  Avatar: string = ""

  ReplyMemberId: number = 0

  ReplyNickname: string = ""

  /** 回复内容 */
  Content: string = ""

  Images: string[] = []

  ChildrenTotal: number = 0

  Children: ResponseInfo[] = []
}
