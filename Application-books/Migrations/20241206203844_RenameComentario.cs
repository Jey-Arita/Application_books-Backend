using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application_books.Migrations
{
    /// <inheritdoc />
    public partial class RenameComentario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nombre",
                schema: "dbo",
                table: "comentario",
                newName: "usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "usuario",
                schema: "dbo",
                table: "comentario",
                newName: "nombre");
        }
    }
}
