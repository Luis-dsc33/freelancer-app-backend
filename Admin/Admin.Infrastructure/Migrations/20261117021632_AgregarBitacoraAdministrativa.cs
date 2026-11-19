// ADMIN Marcos: HU-27 crea la tabla admin.AccionesAdministrativas (bitácora). Generada con dotnet ef.
﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarBitacoraAdministrativa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.CreateTable(
                name: "AccionesAdministrativas",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AdminId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdminNombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TipoAccion = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RecursoTipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RecursoId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecursoNombre = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccionesAdministrativas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccionesAdministrativas_AdminId_TipoAccion",
                schema: "admin",
                table: "AccionesAdministrativas",
                columns: new[] { "AdminId", "TipoAccion" });

            migrationBuilder.CreateIndex(
                name: "IX_AccionesAdministrativas_Fecha",
                schema: "admin",
                table: "AccionesAdministrativas",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_AccionesAdministrativas_RecursoId",
                schema: "admin",
                table: "AccionesAdministrativas",
                column: "RecursoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccionesAdministrativas",
                schema: "admin");
        }
    }
}
