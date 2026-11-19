using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Usuarios.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEstudianteSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstudianteSkills",
                schema: "usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PerfilEstudianteId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    NombreSkill = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Categoria = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    NivelDominio = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudianteSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstudianteSkills_PerfilesEstudiante_PerfilEstudianteId",
                        column: x => x.PerfilEstudianteId,
                        principalSchema: "usuarios",
                        principalTable: "PerfilesEstudiante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstudianteSkills_PerfilEstudianteId_SkillId",
                schema: "usuarios",
                table: "EstudianteSkills",
                columns: new[] { "PerfilEstudianteId", "SkillId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstudianteSkills",
                schema: "usuarios");
        }
    }
}
