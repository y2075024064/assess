using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Assess.Entity;

namespace Assess.Service.Services;

public interface ITopicService : IBusinessServiceBase<TopicInfo>
{
    /// <summary>
    /// 验证答案是否正确
    /// </summary>
    /// <param name="entity">题目实体</param>
    /// <param name="answer">答案集合</param>
    /// <returns>错误信息，为 null 表示正确，为 空字符串 为错误</returns>
    string? CheckAnswer(TopicInfo entity, string[] answer);
}

public class TopicService : BusinessServiceBase<TopicInfo>, ITopicService
{
    public TopicService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }

    public string? CheckAnswer(TopicInfo entity, string[] answer)
    {
        var answers = entity.RightAnswer?.ToModel<string[]>();
        if (answers == null) { return "该题目没有答案"; }

        bool isRight = false;
        switch (entity.Type)
        {
            case TopicType.单选:
            case TopicType.编程:
                isRight = answers[0] == answer[0];
                break;
            case TopicType.多选:
            case TopicType.填空:
                if (answers.Length == answer.Length)
                {
                    for (int i = 0; i < answers.Length; i++)
                    {
                        if (answer[i] != answers[i]) { break; }
                    }
                    isRight = true;
                }
                break;
            default: break;
        }

        return isRight ? null : "";
    }
}
