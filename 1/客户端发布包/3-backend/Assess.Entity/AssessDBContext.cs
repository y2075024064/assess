using Microsoft.EntityFrameworkCore;
using Assess.Entity.Converter;

namespace Assess.Entity;

/// <summary>
/// 数据库上下文
/// </summary>
public class AssessDBContext : DbContext
{
    public AssessDBContext()
    {
    }

    public AssessDBContext(DbContextOptions<AssessDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>()
            .Property(e => e.Birthday)
            .HasConversion(new DateOnlyConverter());

        modelBuilder.Entity<Examination>()
            .Property(e => e.StartDate)
            .HasConversion(new DateOnlyConverter());

        modelBuilder.Entity<Examination>()
            .Property(e => e.EndDate)
            .HasConversion(new DateOnlyConverter());
    }

    public virtual DbSet<AdModule> AdModules { get; set; }

    public virtual DbSet<Advertising> Advertisings { get; set; }

    public virtual DbSet<BrowseRecord> BrowseRecords { get; set; }

    public virtual DbSet<CommentInfo> CommentInfos { get; set; }

    public virtual DbSet<ConsultLetter> ConsultLetters { get; set; }

    public virtual DbSet<DictionaryInfo> DictionaryInfos { get; set; }

    public virtual DbSet<DynamicInfo> DynamicInfos { get; set; }

    public virtual DbSet<ExamApplyInfo> ExamApplyInfos { get; set; }

    public virtual DbSet<Examination> Examinations { get; set; }

    public virtual DbSet<ExaminationTopic> ExaminationTopics { get; set; }

    public virtual DbSet<ExamineRecord> ExamineRecords { get; set; }

    public virtual DbSet<GradeInfo> GradeInfos { get; set; }

    public virtual DbSet<LikeRecord> LikeRecords { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberAccount> MemberAccounts { get; set; }

    public virtual DbSet<MemberDisabledRecord> MemberDisabledRecords { get; set; }

    public virtual DbSet<MemberErrorRecord> MemberErrorRecords { get; set; }

    public virtual DbSet<MemberLoginLog> MemberLoginLogs { get; set; }

    public virtual DbSet<MemberToken> MemberTokens { get; set; }

    public virtual DbSet<MemberTopicCollection> MemberTopicCollections { get; set; }

    public virtual DbSet<MockTestRecord> MockTestRecords { get; set; }

    public virtual DbSet<MyLikeRecord> MyLikeRecords { get; set; }

    public virtual DbSet<PracticeRecord> PracticeRecords { get; set; }

    public virtual DbSet<SchoolInfo> SchoolInfos { get; set; }

    public virtual DbSet<StudentInfo> StudentInfos { get; set; }

    public virtual DbSet<TagRecord> TagRecords { get; set; }

    public virtual DbSet<TestPlan> TestPlans { get; set; }

    public virtual DbSet<TestPlanItem> TestPlanItems { get; set; }

    public virtual DbSet<ThinkStar> ThinkStars { get; set; }

    public virtual DbSet<TopicInfo> TopicInfos { get; set; }

    public virtual DbSet<TopicLabel> TopicLabels { get; set; }

    public virtual DbSet<TopicOptions> TopicOptions { get; set; }

    public virtual DbSet<TopicPart> TopicParts { get; set; }

    public virtual DbSet<WechatCache> WechatCaches { get; set; }

    public virtual DbSet<WechatMemberCache> WechatMemberCaches { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseCategory> CourseCategorys { get; set; }

    public virtual DbSet<CourseVideo> CourseVideos { get; set; }
    public virtual DbSet<CourseOrder> CourseOrders { get; set; }

    public virtual DbSet<CouserVideoRecord> CouserVideoRecords { get; set; }
}
