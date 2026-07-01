using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Assess.Entity.Migrations
{
    /// <inheritdoc />
    public partial class Update7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ta_couser_video__record",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<uint>(type: "int unsigned", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    VideoId = table.Column<int>(type: "int", nullable: false),
                    LastPlayPosition = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsCompleted = table.Column<int>(type: "int", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta_couser_video__record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ta_couser_video__record_ta_course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "ta_course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ta_couser_video__record_ta_course_video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "ta_course_video",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ta_couser_video__record_ta_member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "ta_member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ta_couser_video__record_CourseId",
                table: "ta_couser_video__record",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_couser_video__record_MemberId",
                table: "ta_couser_video__record",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ta_couser_video__record_VideoId",
                table: "ta_couser_video__record",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ta_couser_video__record");
        }
    }
}
