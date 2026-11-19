using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resenas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InicialConSchemaResenas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "resenas");

            migrationBuilder.CreateTable(
                name: "Resenas",
                schema: "resenas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContratoId = table.Column<Guid>(type: "uuid", nullable: false),
                    AutorId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinatarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Puntuacion = table.Column<int>(type: "integer", nullable: false),
                    Comentario = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreadaEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resenas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resenas_AutorId",
                schema: "resenas",
                table: "Resenas",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Resenas_ContratoId_AutorId_DestinatarioId",
                schema: "resenas",
                table: "Resenas",
                columns: new[] { "ContratoId", "AutorId", "DestinatarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resenas_DestinatarioId",
                schema: "resenas",
                table: "Resenas",
                column: "DestinatarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resenas",
                schema: "resenas");
        }
    }
}
