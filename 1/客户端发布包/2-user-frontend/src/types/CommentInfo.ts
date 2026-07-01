export class CommentInfo {
    Id: number = 0
    MemberId: number = 0
    MemberType: string = '普通'
    Nickname: string = ''
    Avatar: string = ''
    ReplyMemberId: number = 0
    ReplyNickname: string = ''
    Content: string = ''
    LikeNum: number = 0
    CreationTime: string = '2021-01-01 12:00:00'
    Images: string[] = []
    IsLike: boolean = true
    ChildrenTotal: number = 0
    Children: string[] = []
}