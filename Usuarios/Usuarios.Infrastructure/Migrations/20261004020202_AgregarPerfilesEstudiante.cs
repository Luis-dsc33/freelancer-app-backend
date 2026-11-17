using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Usuarios.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPerfilesEstudiante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerfilesEstudiante",
                schema: "usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Carrera = table.Column<string>(type: "text", nullable: false),
                    Habilidades = table.Column<List<string>>(type: "text[]", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilesEstudiante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilesEstudiante_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "usuarios",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerfilesEstudiante_UsuarioId",
                schema: "usuarios",
                table: "PerfilesEstudiante",
                column: "UsuarioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerfilesEstudiante",
                schema: "usuarios");
        }
    }
}
