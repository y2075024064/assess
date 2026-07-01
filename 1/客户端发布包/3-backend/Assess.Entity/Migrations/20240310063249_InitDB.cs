using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Assess.Entity.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_ad_module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    Location = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    ModuleName = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    IsEnable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsCarousel = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_ad_module", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_browse_record",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ObjectId = table.Column<long>(type: "bigint", nullable: false),
                    ObjType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    IsGiveLike = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsCollection = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastBrowseOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_browse_record", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_dictionary",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_dictionary", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_examination",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(type: "char(12)", nullable: false),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    Purpose = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    TotalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Duration = table.Column<uint>(type: "int unsigned", nullable: false),
                    ReferenceObj = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Organizer = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    CoOrganizer = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_examination", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_grade_info",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    SortLevel = table.Column<int>(type: "int", nullable: false),
                    IsUse = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsShow = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_grade_info", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_member",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nickname = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    RealName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    Gender = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Avatar = table.Column<string>(type: "tinytext", nullable: true),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    MobilePhone = table.Column<string>(type: "char(15)", nullable: true),
                    Email = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    MainMemberId = table.Column<uint>(type: "int unsigned", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IdCard = table.Column<string>(type: "char(18)", nullable: true),
                    RefereeId = table.Column<uint>(type: "int unsigned", nullable: true),
                    IsBindWechat = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    InvitationCode = table.Column<string>(type: "char(10)", nullable: true),
                    CollectGroupNames = table.Column<string>(type: "json", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDelete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_member", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_member_login_log",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Account = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    FailPwd = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CheckTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsPass = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ClientType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    ClientIp = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    PassTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Reason = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_member_login_log", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_my_like_record",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    LikeNum = table.Column<uint>(type: "int unsigned", nullable: false),
                    ObjectId = table.Column<uint>(type: "int unsigned", nullable: false),
                    ObjectType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    LastTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_my_like_record", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_tag_record",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    UseNum = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_tag_record", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_topic_label",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    OldName = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    ModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_topic_label", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_topic_part",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    OldName = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    RcmdGrade = table.Column<string>(type: "json", nullable: true),
                    ModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SortLevel = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_topic_part", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_topic_part_ta_topic_part_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ta_topic_part",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_wechat_cache",
                columns: table => new
                {
                    AppId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    Token = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: false),
                    TokenOverdueTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Jticket = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: true),
                    JticketOverdueTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_wechat_cache", x => x.AppId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_wechat_member_cache",
                columns: table => new
                {
                    OpenId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    Scope = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Atoken = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: true),
                    AtokenOverdueTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Rtoken = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: true),
                    RtokenOverdueTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_wechat_member_cache", x => x.OpenId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_advertising",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ModuleId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    PreviewImg = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Hyperlink = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    FolderUrl = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Company = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_advertising", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_advertising_ta_ad_module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "ta_ad_module",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_comment_info",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    ObjectId = table.Column<uint>(type: "int unsigned", nullable: false),
                    CommentId = table.Column<uint>(type: "int unsigned", nullable: true),
                    LikeNum = table.Column<uint>(type: "int unsigned", nullable: false),
                    IsTop = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsUseful = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    ObjectType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Content = table.Column<string>(type: "varchar(320)", maxLength: 320, nullable: false),
                    Images = table.Column<string>(type: "longtext", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDelete = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_comment_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_comment_info_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_consult_letter",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    State = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    IsModel = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    Label = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Content = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false),
                    Images = table.Column<string>(type: "longtext", nullable: true),
                    LikeNum = table.Column<uint>(type: "int unsigned", nullable: false),
                    ViewNum = table.Column<uint>(type: "int unsigned", nullable: false),
                    CommentNum = table.Column<uint>(type: "int unsigned", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDelete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_consult_letter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_consult_letter_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_dynamic_info",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Title = table.Column<string>(type: "varchar(125)", maxLength: 125, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    PreviewImg = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    OriginalHref = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    PublishTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDelete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_dynamic_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_dynamic_info_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_examine_record",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ExaminationId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    IsOver = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CurrPostion = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnswerRecord = table.Column<string>(type: "json", nullable: true),
                    ErrorIndexs = table.Column<string>(type: "json", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OverTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_examine_record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_examine_record_ta_examination_ExaminationId",
                        column: x => x.ExaminationId,
                        principalTable: "ta_examination",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ta_examine_record_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_member_account",
                columns: table => new
                {
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Account = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    UnionId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    OpenId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    MiniOpenId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Password = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: true),
                    Signature = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true),
                    EncodeType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DisableStartTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DisableEndTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CanModifyAccount = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_member_account", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_ta_member_account_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_member_disabled_record",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Reason = table.Column<string>(type: "tinytext", nullable: true),
                    IsAppeal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AppealDesc = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true),
                    AppealImgs = table.Column<string>(type: "json", nullable: true),
                    AppealResult = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    FailReason = table.Column<string>(type: "tinytext", nullable: true),
                    RealEndTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CompensationInfo = table.Column<string>(type: "tinytext", nullable: true),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_member_disabled_record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_member_disabled_record_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_member_token",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Token = table.Column<string>(type: "char(64)", nullable: false),
                    ClientType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    IsRemember = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_member_token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_member_token_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_student_info",
                columns: table => new
                {
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    CurrentGrade = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    RegistGrade = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_student_info", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_ta_student_info_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_think_star",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    PreviewImg = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CommentNum = table.Column<uint>(type: "int unsigned", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_think_star", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_think_star_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_like_record",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    MyLikeRecordId = table.Column<long>(type: "bigint", nullable: false),
                    ObjectId = table.Column<uint>(type: "int unsigned", nullable: false),
                    ObjectType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_like_record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_like_record_ta_my_like_record_MyLikeRecordId",
                        column: x => x.MyLikeRecordId,
                        principalTable: "ta_my_like_record",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_mock_test_record",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(type: "char(12)", nullable: false),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: true),
                    TopicInfos = table.Column<string>(type: "json", nullable: false),
                    AnsweredIndexs = table.Column<string>(type: "json", nullable: true),
                    ErrorIndexs = table.Column<string>(type: "json", nullable: true),
                    IsOver = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AvgDifficulty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrPostion = table.Column<int>(type: "int", nullable: false),
                    AnswerRecord = table.Column<string>(type: "json", nullable: true),
                    RemainTime = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OverTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_mock_test_record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_mock_test_record_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ta_mock_test_record_ta_topic_part_PartId",
                        column: x => x.PartId,
                        principalTable: "ta_topic_part",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_test_plan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TestTime = table.Column<int>(type: "int", nullable: true),
                    TopicNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_test_plan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_test_plan_ta_topic_part_PartId",
                        column: x => x.PartId,
                        principalTable: "ta_topic_part",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_topic_info",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(type: "char(12)", nullable: false),
                    Title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Level = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: true),
                    Label = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    RcmdGrade = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    Purpose = table.Column<string>(type: "text", nullable: true),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: true),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    UseType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    RightAnswer = table.Column<string>(type: "json", nullable: true),
                    Analysis = table.Column<string>(type: "longtext", nullable: true),
                    LikeNum = table.Column<uint>(type: "int unsigned", nullable: false),
                    CollectionNum = table.Column<int>(type: "int", nullable: false),
                    CommentNum = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    OriginNumber = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    OriginType = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    IsOriginal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDelete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Remark = table.Column<string>(type: "tinytext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_topic_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_topic_info_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ta_topic_info_ta_topic_part_PartId",
                        column: x => x.PartId,
                        principalTable: "ta_topic_part",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_exam_apply_info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    ExaminationId = table.Column<int>(type: "int", nullable: false),
                    ExamineRecordId = table.Column<long>(type: "bigint", nullable: true),
                    IsPay = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_exam_apply_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_exam_apply_info_ta_examination_ExaminationId",
                        column: x => x.ExaminationId,
                        principalTable: "ta_examination",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ta_exam_apply_info_ta_examine_record_ExamineRecordId",
                        column: x => x.ExamineRecordId,
                        principalTable: "ta_examine_record",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ta_exam_apply_info_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_test_plan_item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    AvgLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_test_plan_item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_test_plan_item_ta_test_plan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "ta_test_plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ta_test_plan_item_ta_topic_part_PartId",
                        column: x => x.PartId,
                        principalTable: "ta_topic_part",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_examination_topic",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ExaminationId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_examination_topic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_examination_topic_ta_examination_ExaminationId",
                        column: x => x.ExaminationId,
                        principalTable: "ta_examination",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ta_examination_topic_ta_topic_info_TopicId",
                        column: x => x.TopicId,
                        principalTable: "ta_topic_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_member_error_record",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    TopicId = table.Column<uint>(type: "int unsigned", nullable: false),
                    ErrorAnswer = table.Column<string>(type: "tinytext", nullable: false),
                    IsPass = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PassTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastErrorTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_member_error_record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_member_error_record_ta_topic_info_TopicId",
                        column: x => x.TopicId,
                        principalTable: "ta_topic_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_member_topic_collection",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    TopicId = table.Column<uint>(type: "int unsigned", nullable: false),
                    GroupName = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_member_topic_collection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_member_topic_collection_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ta_member_topic_collection_ta_topic_info_TopicId",
                        column: x => x.TopicId,
                        principalTable: "ta_topic_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_practice_record",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    TopicId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PracticeTime = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Answer = table.Column<string>(type: "tinytext", nullable: true),
                    IsTrue = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_practice_record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_practice_record_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ta_practice_record_ta_topic_info_TopicId",
                        column: x => x.TopicId,
                        principalTable: "ta_topic_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ta_topic_options",
                columns: table => new
                {
                    TopicId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Options = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_topic_options", x => x.TopicId);
                    table.ForeignKey(
                        name: "FK_ta_topic_options_ta_topic_info_TopicId",
                        column: x => x.TopicId,
                        principalTable: "ta_topic_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "UQ_AdModule_ModuleName",
                table: "ta_ad_module",
                column: "ModuleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ta_advertising_ModuleId",
                table: "ta_advertising",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "UQ_Advertising_Title",
                table: "ta_advertising",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ta_comment_info_MemberId",
                table: "ta_comment_info",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_consult_letter_MemberId",
                table: "ta_consult_letter",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_dynamic_info_MemberId",
                table: "ta_dynamic_info",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_exam_apply_info_ExaminationId",
                table: "ta_exam_apply_info",
                column: "ExaminationId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_exam_apply_info_ExamineRecordId",
                table: "ta_exam_apply_info",
                column: "ExamineRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_exam_apply_info_MemberId",
                table: "ta_exam_apply_info",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_examination_topic_ExaminationId",
                table: "ta_examination_topic",
                column: "ExaminationId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_examination_topic_TopicId",
                table: "ta_examination_topic",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_examine_record_ExaminationId",
                table: "ta_examine_record",
                column: "ExaminationId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_examine_record_MemberId",
                table: "ta_examine_record",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "UQ_GradeInfo_Name",
                table: "ta_grade_info",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ta_like_record_MyLikeRecordId",
                table: "ta_like_record",
                column: "MyLikeRecordId");

            migrationBuilder.CreateIndex(
                name: "UQ_Member_Nickname",
                table: "ta_member",
                column: "Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_MemberAccount_Account",
                table: "ta_member_account",
                column: "Account",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ta_member_disabled_record_MemberId",
                table: "ta_member_disabled_record",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_member_error_record_TopicId",
                table: "ta_member_error_record",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberLoginLog_MemberId",
                table: "ta_member_login_log",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_member_token_MemberId",
                table: "ta_member_token",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "UQ_MemberToken_Token",
                table: "ta_member_token",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ta_member_topic_collection_MemberId",
                table: "ta_member_topic_collection",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_member_topic_collection_TopicId",
                table: "ta_member_topic_collection",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_mock_test_record_MemberId",
                table: "ta_mock_test_record",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_mock_test_record_PartId",
                table: "ta_mock_test_record",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "UQ_MockTestRecord_Number",
                table: "ta_mock_test_record",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ta_practice_record_MemberId",
                table: "ta_practice_record",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_practice_record_TopicId",
                table: "ta_practice_record",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "UQ_TagRecord_Name",
                table: "ta_tag_record",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ta_test_plan_PartId",
                table: "ta_test_plan",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_test_plan_item_PartId",
                table: "ta_test_plan_item",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_test_plan_item_PlanId",
                table: "ta_test_plan_item",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_think_star_MemberId",
                table: "ta_think_star",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_topic_info_MemberId",
                table: "ta_topic_info",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_topic_info_PartId",
                table: "ta_topic_info",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "UQ_TopicInfo_Number",
                table: "ta_topic_info",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_TopicLabel_Name",
                table: "ta_topic_label",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ta_topic_part_ParentId",
                table: "ta_topic_part",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ta_advertising");

            migrationBuilder.DropTable(
                name: "ta_browse_record");

            migrationBuilder.DropTable(
                name: "ta_comment_info");

            migrationBuilder.DropTable(
                name: "ta_consult_letter");

            migrationBuilder.DropTable(
                name: "ta_dictionary");

            migrationBuilder.DropTable(
                name: "ta_dynamic_info");

            migrationBuilder.DropTable(
                name: "ta_exam_apply_info");

            migrationBuilder.DropTable(
                name: "ta_examination_topic");

            migrationBuilder.DropTable(
                name: "ta_grade_info");

            migrationBuilder.DropTable(
                name: "ta_like_record");

            migrationBuilder.DropTable(
                name: "ta_member_account");

            migrationBuilder.DropTable(
                name: "ta_member_disabled_record");

            migrationBuilder.DropTable(
                name: "ta_member_error_record");

            migrationBuilder.DropTable(
                name: "ta_member_login_log");

            migrationBuilder.DropTable(
                name: "ta_member_token");

            migrationBuilder.DropTable(
                name: "ta_member_topic_collection");

            migrationBuilder.DropTable(
                name: "ta_mock_test_record");

            migrationBuilder.DropTable(
                name: "ta_practice_record");

            migrationBuilder.DropTable(
                name: "ta_student_info");

            migrationBuilder.DropTable(
                name: "ta_tag_record");

            migrationBuilder.DropTable(
                name: "ta_test_plan_item");

            migrationBuilder.DropTable(
                name: "ta_think_star");

            migrationBuilder.DropTable(
                name: "ta_topic_label");

            migrationBuilder.DropTable(
                name: "ta_topic_options");

            migrationBuilder.DropTable(
                name: "ta_wechat_cache");

            migrationBuilder.DropTable(
                name: "ta_wechat_member_cache");

            migrationBuilder.DropTable(
                name: "ta_ad_module");

            migrationBuilder.DropTable(
                name: "ta_examine_record");

            migrationBuilder.DropTable(
                name: "ta_my_like_record");

            migrationBuilder.DropTable(
                name: "ta_test_plan");

            migrationBuilder.DropTable(
                name: "ta_topic_info");

            migrationBuilder.DropTable(
                name: "ta_examination");

            migrationBuilder.DropTable(
                name: "ta_member");

            migrationBuilder.DropTable(
                name: "ta_topic_part");
        }
    }
}
