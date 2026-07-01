using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Assess.Model.Result;
using Assess.Entity;

namespace Assess.Service.Services;

public interface IMockTestRecordService : IBusinessServiceBase<MockTestRecord>
{
    /// <summary>
    /// 获取用户测评成绩排行榜
    /// </summary>
    /// <param name="top">前几强</param>
    /// <param name="partId">类别ID</param>
    /// <param name="currentMemberId">当前登录用户ID</param>
    /// <returns></returns>
    Task<MockTestRankInfo[]> GetScoreRankAsync(uint top, int? partId, uint? currentMemberId);
}

public class MockTestRecordService : BusinessServiceBase<MockTestRecord>, IMockTestRecordService
{
    public MockTestRecordService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }

    public async Task<MockTestRankInfo[]> GetScoreRankAsync(uint top, int? partId, uint? currentMemberId)
    {
        string partIdSql = partId == null ? "" : $"AND `PartId` = {partId}";

        // 计算每个用户的总答题数、正确数和正确率
        // 综合分 = 答题数 × 0.6 + 正确率 × 0.4
        string subSql =
            "SELECT `MemberId`, " +
            "SUM(JSON_LENGTH(`AnsweredIndexs`)) AS `AnswerCount`, " +
            "SUM(JSON_LENGTH(`AnsweredIndexs`) - JSON_LENGTH(`ErrorIndexs`)) / SUM(JSON_LENGTH(`AnsweredIndexs`)) AS `Accuracy`, " +
            "SUM(JSON_LENGTH(`AnsweredIndexs`)) * 0.6 + " +
            "(SUM(JSON_LENGTH(`AnsweredIndexs`) - JSON_LENGTH(`ErrorIndexs`)) / SUM(JSON_LENGTH(`AnsweredIndexs`))) * 0.4 AS `RankScore` " +
            $"FROM `ta_mock_test_record` WHERE `IsOver` = 1 {partIdSql} " +
            "GROUP BY `MemberId` HAVING `AnswerCount` > 0";

        // 取前 top 名
        string topSql =
            $"SELECT temp.*, tm.`Avatar`, tm.`Nickname` FROM ({subSql}) AS temp " +
            "LEFT JOIN `ta_member` AS tm ON (temp.`MemberId` = tm.`Id`) " +
            "ORDER BY temp.`RankScore` DESC " +
            $"LIMIT {top};";

        var topList = await SqlQueryListAsync<MockTestRankInfo>(topSql).NotWaitBack();

        // 标记排名
        var result = topList.Select((item, index) =>
        {
            item.Rank = index + 1;
            item.IsCurrentUser = currentMemberId != null && item.MemberId == currentMemberId.Value;
            item.Accuracy = Math.Round(item.Accuracy * 100, 1); // 转为百分比
            item.RankScore = Math.Round(item.RankScore, 2);
            return item;
        }).ToList();

        // 如果当前用户不在前十，追加用户的排名信息
        if (currentMemberId != null && !result.Any(r => r.IsCurrentUser))
        {
            // 查询所有用户排名，找到当前用户的位置
            string allRankSql =
                $"SELECT temp.*, tm.`Avatar`, tm.`Nickname` " +
                $"FROM ({subSql}) AS temp " +
                "LEFT JOIN `ta_member` AS tm ON (temp.`MemberId` = tm.`Id`) " +
                "ORDER BY temp.`RankScore` DESC;";

            var allRank = await SqlQueryListAsync<MockTestRankInfo>(allRankSql).NotWaitBack();
            for (int i = 0; i < allRank.Count; i++)
            {
                if (allRank[i].MemberId == currentMemberId.Value)
                {
                    var userItem = allRank[i];
                    userItem.Rank = i + 1;
                    userItem.IsCurrentUser = true;
                    userItem.Accuracy = Math.Round(userItem.Accuracy * 100, 1);
                    userItem.RankScore = Math.Round(userItem.RankScore, 2);
                    result.Add(userItem);
                    break;
                }
            }
        }

        return result.ToArray();
    }
}
