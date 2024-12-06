using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application_books.Migrations
{
    /// <inheritdoc />
    public partial class addmembresia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activa_membresia",
                schema: "dbo",
                table: "membresia");

            migrationBuilder.DropColumn(
                name: "fecha_cancelacion",
                schema: "dbo",
                table: "membresia");

            migrationBuilder.AddColumn<int>(
                name: "dias_restantes",
                schema: "dbo",
                table: "membresia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "tipo_membresia",
                schema: "dbo",
                table: "membresia",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dias_restantes",
                schema: "dbo",
                table: "membresia");

            migrationBuilder.DropColumn(
                name: "tipo_membresia",
                schema: "dbo",
                table: "membresia");

            migrationBuilder.AddColumn<bool>(
                name: "activa_membresia",
                schema: "dbo",
                table: "membresia",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_cancelacion",
                schema: "dbo",
                table: "membresia",
                type: "datetime2",
                nullable: true);
        }
    }
}
