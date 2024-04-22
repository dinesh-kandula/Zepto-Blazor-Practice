using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeptoApi.Migrations
{
    /// <inheritdoc />
    public partial class Phonedatatypetolong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "ZeptoUser",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<long>(
                name: "Phone",
                table: "ZeptoUser",
                type: "bigint",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ZeptoUser",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "ZeptoUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Phone",
                table: "ZeptoUser",
                type: "int",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "ZeptoUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
