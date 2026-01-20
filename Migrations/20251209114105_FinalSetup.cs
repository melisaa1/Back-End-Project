using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RateNowApi.Migrations
{

    public partial class FinalSetup : Migration
    {
   
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "Text",
                value: "This movie was amazing!, 10/10.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "Text",
                value: "I dont like it very much.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "Text",
                value: "I like it.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Role" },
                values: new object[] { "$2a$11$iTJRPFsz/EzKH5axKZcJYep1Cue64kze6U9D8CDv8jR/0RQ/UcUvq", "User" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Role" },
                values: new object[] { "$2a$11$k7yvHz3c6FHRxoWt4ZfVROBQsdSm3FWoXjzfs.4uGPpSq4CkwwhWy", "User" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "Text",
                value: "Muhteşem bir filmdi, 10/10.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "Text",
                value: "Ortalama bir yapım, beklentiyi karşılamadı.");

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "Text",
                value: "Git öğrenme sürecimi özetliyor.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$F6deNmYFsWmp9g7bwg0UteQokiQZmAEkjDk4nHM4QnR/WDE.2FlrG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$SfhSi93nAlOJp41Oybn7m.YHrKrcD5fIWH4LSd7T6AwFvjRsC26K2");
        }
    }
}
