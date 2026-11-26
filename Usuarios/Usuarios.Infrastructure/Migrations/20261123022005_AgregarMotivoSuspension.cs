// ADMIN Marcos: agrega la columna opcional MotivoSuspension para mostrar el motivo en el login (HU-02-03).
﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Usuarios.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMotivoSuspension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotivoSuspension",
                schema: "usuarios",
                table: "Usuarios",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoSuspension",
                schema: "usuarios",
                table: "Usuarios");
        }
    }
}
