using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application_books.Migrations
{
    /// <inheritdoc />
    public partial class AddComentariosRespuestas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "id_comentario_padre",
                schema: "dbo",
                table: "comentario",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_comentario_id_comentario_padre",
                schema: "dbo",
                table: "comentario",
                column: "id_comentario_padre");

            migrationBuilder.AddForeignKey(
                name: "FK_comentario_comentario_id_comentario_padre",
                schema: "dbo",
                table: "comentario",
                column: "id_comentario_padre",
                principalSchema: "dbo",
                principalTable: "comentario",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comentario_comentario_id_comentario_padre",
                schema: "dbo",
                table: "comentario");

            migrationBuilder.DropIndex(
                name: "IX_comentario_id_comentario_padre",
                schema: "dbo",
                table: "comentario");

            migrationBuilder.DropColumn(
                name: "id_comentario_padre",
                schema: "dbo",
                table: "comentario");
        }
    }
}
