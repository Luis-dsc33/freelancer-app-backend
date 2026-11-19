using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDatosPublicacionOferta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClienteId",
                schema: "marketplace",
                table: "Proyectos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Disciplina",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaLimite",
                schema: "marketplace",
                table: "Proyectos",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.UpdateData(
                schema: "marketplace",
                table: "Proyectos",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ClienteId", "Disciplina", "FechaLimite" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Desarrollo de software", new DateOnly(2026, 11, 30) });

            migrationBuilder.UpdateData(
                schema: "marketplace",
                table: "Proyectos",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ClienteId", "Disciplina", "FechaLimite" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Marketing digital", new DateOnly(2026, 11, 30) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteId",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "Disciplina",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "FechaLimite",
                schema: "marketplace",
                table: "Proyectos");

        }
    }
}
