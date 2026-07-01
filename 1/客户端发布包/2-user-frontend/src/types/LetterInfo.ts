class BrowseRecord {
    IsCollection: boolean = false
    IsGiveLike: boolean = false
    falseLastBrowseOn: string = ""
}

export class LetterInfo {
    Avatar: string = ""
    Content: string = ""
    CreationTime: string = ""
    Id: number = 0
    Image: string = ""
    Images: string[] = []
    IsModel: boolean = false
    IsPublic: boolean = true
    Label: string = ""
    Nickname: string = ""
    State: string = ""
    Title: string = ""
    ViewNum: number = 0
    LikeNum: number = 0
    CommentNum: number = 0

    BrowseRecord = new BrowseRecord
}