using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KMwareWeb01.Data
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NameIdentifier = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Firstname = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Lastname = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Mobile = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Roles = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RunTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    V0 = table.Column<int>(type: "INTEGER", nullable: false),
                    V1 = table.Column<int>(type: "INTEGER", nullable: false),
                    V2 = table.Column<int>(type: "INTEGER", nullable: false),
                    V3 = table.Column<int>(type: "INTEGER", nullable: false),
                    V4 = table.Column<int>(type: "INTEGER", nullable: false),
                    V5 = table.Column<int>(type: "INTEGER", nullable: false),
                    V6 = table.Column<int>(type: "INTEGER", nullable: false),
                    V7 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Updates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsRainDelay = table.Column<byte>(type: "INTEGER", nullable: false),
                    RunTimesJson = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    TimeStamp = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Updates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "UserId", "Email", "Firstname", "Lastname", "Mobile", "NameIdentifier", "Password", "Provider", "Roles", "Username" },
                values: new object[] { 1, "k@m.com", "K", "M", "1234567890", "KM", "password", "Cookies", "Admin", "k@m.com" });

            migrationBuilder.InsertData(
                table: "RunTimes",
                columns: new[] { "Id", "V0", "V1", "V2", "V3", "V4", "V5", "V6", "V7" },
                values: new object[,]
                {
                    { 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 2, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 3, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 4, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 5, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 6, 0, 0, 0, 0, 0, 0, 0, 0 },
                    { 7, 0, 0, 0, 0, 0, 0, 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "Updates",
                columns: new[] { "Id", "IsRainDelay", "RunTimesJson", "TimeStamp" },
                values: new object[] { 1, (byte)0, "{\"V0\":0,\"V1\":0,\"V2\":0,\"V3\":0,\"V4\":0,\"V5\":0,\"V6\":0,\"V7\":0}", "2021-01-01 00:00:00" });

            migrationBuilder.CreateIndex(
                name: "IX_RunTimes_Id",
                table: "RunTimes",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "RunTimes");

            migrationBuilder.DropTable(
                name: "Updates");
        }
    }
}
