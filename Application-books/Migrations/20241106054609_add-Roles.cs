using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application_books.Migrations
{
    /// <inheritdoc />
    public partial class addRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "security");

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "security",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "security",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    refresh_token = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    refresh_token_expire = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles_claims",
                schema: "security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_roles_claims_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "security",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "autor",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    autor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    bibliografia = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    img_autor = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autor", x => x.id);
                    table.ForeignKey(
                        name: "FK_autor_users_created_by",
                        column: x => x.created_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_autor_users_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "membresia",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_usuario = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    activa_membresia = table.Column<bool>(type: "bit", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_cancelacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membresia", x => x.id);
                    table.ForeignKey(
                        name: "FK_membresia_users_created_by",
                        column: x => x.created_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_membresia_users_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_membresia_users_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users_claims",
                schema: "security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_claims_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users_logins",
                schema: "security",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_users_logins_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users_roles",
                schema: "security",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_users_roles_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "security",
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_roles_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users_tokens",
                schema: "security",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_users_tokens_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "libros_book",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    genero = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_time = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    img_libro = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    pdf_libro = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    id_autor = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libros_book", x => x.id);
                    table.ForeignKey(
                        name: "FK_libros_book_autor_id_autor",
                        column: x => x.id_autor,
                        principalSchema: "dbo",
                        principalTable: "autor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_libros_book_users_created_by",
                        column: x => x.created_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_libros_book_users_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "calificacion",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_libro = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_usuario = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    puntuacion = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calificacion", x => x.id);
                    table.ForeignKey(
                        name: "FK_calificacion_libros_book_id_libro",
                        column: x => x.id_libro,
                        principalSchema: "dbo",
                        principalTable: "libros_book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_calificacion_users_created_by",
                        column: x => x.created_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_calificacion_users_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_calificacion_users_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comentario",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_libro = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_usuario = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    comentario = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comentario", x => x.id);
                    table.ForeignKey(
                        name: "FK_comentario_libros_book_id_libro",
                        column: x => x.id_libro,
                        principalSchema: "dbo",
                        principalTable: "libros_book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comentario_users_created_by",
                        column: x => x.created_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comentario_users_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comentario_users_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lista_favorito",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_usuario = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_libro = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lista_favorito", x => x.id);
                    table.ForeignKey(
                        name: "FK_lista_favorito_libros_book_id_libro",
                        column: x => x.id_libro,
                        principalSchema: "dbo",
                        principalTable: "libros_book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lista_favorito_users_created_by",
                        column: x => x.created_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lista_favorito_users_id_usuario",
                        column: x => x.id_usuario,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_lista_favorito_users_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "security",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_autor_created_by",
                schema: "dbo",
                table: "autor",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_autor_updated_by",
                schema: "dbo",
                table: "autor",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_calificacion_created_by",
                schema: "dbo",
                table: "calificacion",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_calificacion_id_libro",
                schema: "dbo",
                table: "calificacion",
                column: "id_libro");

            migrationBuilder.CreateIndex(
                name: "IX_calificacion_id_usuario",
                schema: "dbo",
                table: "calificacion",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_calificacion_updated_by",
                schema: "dbo",
                table: "calificacion",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_comentario_created_by",
                schema: "dbo",
                table: "comentario",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_comentario_id_libro",
                schema: "dbo",
                table: "comentario",
                column: "id_libro");

            migrationBuilder.CreateIndex(
                name: "IX_comentario_id_usuario",
                schema: "dbo",
                table: "comentario",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_comentario_updated_by",
                schema: "dbo",
                table: "comentario",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_libros_book_created_by",
                schema: "dbo",
                table: "libros_book",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_libros_book_id_autor",
                schema: "dbo",
                table: "libros_book",
                column: "id_autor");

            migrationBuilder.CreateIndex(
                name: "IX_libros_book_updated_by",
                schema: "dbo",
                table: "libros_book",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_lista_favorito_created_by",
                schema: "dbo",
                table: "lista_favorito",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_lista_favorito_id_libro",
                schema: "dbo",
                table: "lista_favorito",
                column: "id_libro");

            migrationBuilder.CreateIndex(
                name: "IX_lista_favorito_id_usuario",
                schema: "dbo",
                table: "lista_favorito",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_lista_favorito_updated_by",
                schema: "dbo",
                table: "lista_favorito",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_membresia_created_by",
                schema: "dbo",
                table: "membresia",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_membresia_id_usuario",
                schema: "dbo",
                table: "membresia",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_membresia_updated_by",
                schema: "dbo",
                table: "membresia",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "security",
                table: "roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_roles_claims_RoleId",
                schema: "security",
                table: "roles_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "security",
                table: "users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "security",
                table: "users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_users_claims_UserId",
                schema: "security",
                table: "users_claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_logins_UserId",
                schema: "security",
                table: "users_logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_roles_RoleId",
                schema: "security",
                table: "users_roles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calificacion",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "comentario",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "lista_favorito",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "membresia",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "roles_claims",
                schema: "security");

            migrationBuilder.DropTable(
                name: "users_claims",
                schema: "security");

            migrationBuilder.DropTable(
                name: "users_logins",
                schema: "security");

            migrationBuilder.DropTable(
                name: "users_roles",
                schema: "security");

            migrationBuilder.DropTable(
                name: "users_tokens",
                schema: "security");

            migrationBuilder.DropTable(
                name: "libros_book",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "security");

            migrationBuilder.DropTable(
                name: "autor",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "users",
                schema: "security");
        }
    }
}
