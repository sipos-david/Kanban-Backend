using Microsoft.EntityFrameworkCore.Migrations;

namespace KanbanBoard.Migrations
{
    public partial class SeedingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Project 1" },
                    { 2, "Project 2" }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "Id", "Name", "ProjectId" },
                values: new object[] { 2, "Table 1", 1 });

            migrationBuilder.InsertData(
                table: "Columns",
                columns: new[] { "Id", "Name", "Number", "TableId" },
                values: new object[,]
                {
                    { 3, "Col 1", 0, 2 },
                    { 4, "Col 2", 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "ColumnId", "Description", "Name", "Number" },
                values: new object[,]
                {
                    { 5, 3, "Task 1: description...", "Task 1", 0 },
                    { 6, 3, "Task 2: description...", "Task 2", 1 },
                    { 7, 3, "Task 3: description...", "Task 3", 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Columns",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Columns",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
