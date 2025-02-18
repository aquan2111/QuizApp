using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestion_Questions_QuestionId",
                table: "QuizQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestion_Quizzes_QuizId",
                table: "QuizQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizQuestion",
                table: "QuizQuestion");

            migrationBuilder.RenameTable(
                name: "QuizQuestion",
                newName: "QuizQuestions");

            migrationBuilder.RenameIndex(
                name: "IX_QuizQuestion_QuizId",
                table: "QuizQuestions",
                newName: "IX_QuizQuestions_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizQuestion_QuestionId",
                table: "QuizQuestions",
                newName: "IX_QuizQuestions_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizQuestions",
                table: "QuizQuestions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_Questions_QuestionId",
                table: "QuizQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_Quizzes_QuizId",
                table: "QuizQuestions",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_Questions_QuestionId",
                table: "QuizQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_Quizzes_QuizId",
                table: "QuizQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizQuestions",
                table: "QuizQuestions");

            migrationBuilder.RenameTable(
                name: "QuizQuestions",
                newName: "QuizQuestion");

            migrationBuilder.RenameIndex(
                name: "IX_QuizQuestions_QuizId",
                table: "QuizQuestion",
                newName: "IX_QuizQuestion_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizQuestions_QuestionId",
                table: "QuizQuestion",
                newName: "IX_QuizQuestion_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizQuestion",
                table: "QuizQuestion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestion_Questions_QuestionId",
                table: "QuizQuestion",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestion_Quizzes_QuizId",
                table: "QuizQuestion",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
