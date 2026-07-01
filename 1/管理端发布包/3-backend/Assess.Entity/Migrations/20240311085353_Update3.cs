using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assess.Entity.Migrations
{
    /// <inheritdoc />
    public partial class Update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassNum",
                table: "ta_student_info",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GradeNum",
                table: "ta_student_info",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "ta_student_info",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ta_student_info_SchoolId",
                table: "ta_student_info",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_ta_student_info_ta_school_info_SchoolId",
                table: "ta_student_info",
                column: "SchoolId",
                principalTable: "ta_school_info",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ta_student_info_ta_school_info_SchoolId",
                table: "ta_student_info");

            migrationBuilder.DropIndex(
                name: "IX_ta_student_info_SchoolId",
                table: "ta_student_info");

            migrationBuilder.DropColumn(
                name: "ClassNum",
                table: "ta_student_info");

            migrationBuilder.DropColumn(
                name: "GradeNum",
                table: "ta_student_info");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "ta_student_info");
        }
    }
}
