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
    /// <returns></returns>
    Task<MockTestRankInfo[]> GetScoreRankAsync(uint top, int? partId);
}

public class MockTestRecordService : BusinessServiceBase<MockTestRecord>, IMockTestRecordService
{
    public MockTestRecordService(DbContextOptions<AssessDBContext> options) : base(options)
    {
    }

    public async Task<MockTestRankInfo[]> GetScoreRankAsync(uint top, int? partId)
    {
        string partIdSql = partId == null ? "" : $"AND `PartId` = {partId}";
        string subSql =
            "SELECT `MemberId`, MAX(`CurrScore`) AS `CurrScore`, `OverTime` FROM " +
            $"`ta_mock_test_record` WHERE `IsOver` = 1 {partIdSql} " +
            "GROUP BY `MemberId`";
        string sql =
            $"SELECT temp.*, tm.`Avatar`, tm.`Nickname` FROM ({subSql}) AS temp " +
            "LEFT JOIN `ta_member` AS tm ON (temp.`MemberId` = tm.`Id`) " +
            $"ORDER BY temp.`CurrScore` DESC, temp.`OverTime` DESC LIMIT {top};";

        List<MockTestRankInfo> list = await SqlQueryListAsync<MockTestRankInfo>(sql).NotWaitBack();
        return list.ToArray();
    }
}
