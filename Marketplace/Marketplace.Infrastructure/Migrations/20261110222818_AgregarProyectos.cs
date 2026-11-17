using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Marketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProyectos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "marketplace");

            migrationBuilder.CreateTable(
                name: "Proyectos",
                schema: "marketplace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Presupuesto = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    EsUrgente = table.Column<bool>(type: "boolean", nullable: false),
                    HabilidadesRequeridas = table.Column<List<string>>(type: "text[]", nullable: false),
                    NombreCliente = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "marketplace",
                table: "Proyectos",
                columns: new[] { "Id", "CreadoEn", "Descripcion", "EsUrgente", "Estado", "HabilidadesRequeridas", "NombreCliente", "Presupuesto", "Titulo" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MVP para estudiantes", false, "Abierto", new List<string> { "Angular", "Ionic" }, "CodeBridge", 10000m, "App móvil universitaria" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 11, 5, 15, 30, 0, 0, DateTimeKind.Utc), "Sitio marketing con formularios", true, "Abierto", new List<string> { "HTML", "CSS", "SEO" }, "Club Alumni", 4500m, "Landing de recaudación" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_Estado_EsUrgente_CreadoEn",
                schema: "marketplace",
                table: "Proyectos",
                columns: new[] { "Estado", "EsUrgente", "CreadoEn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proyectos",
                schema: "marketplace");
        }
    }
}
