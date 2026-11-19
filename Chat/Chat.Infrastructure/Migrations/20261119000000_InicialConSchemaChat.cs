using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InicialConSchemaChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "chat");

            migrationBuilder.CreateTable(
                name: "Conversaciones",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProyectoId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostulacionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContratoId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    EstudianteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TituloProyecto = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    NombreCliente = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CreadaEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UltimaActividadEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mensajes",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversacionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RemitenteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Contenido = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    EnviadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensajes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensajes_Conversaciones_ConversacionId",
                        column: x => x.ConversacionId,
                        principalSchema: "chat",
                        principalTable: "Conversaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conversaciones_ClienteId_UltimaActividadEn",
                schema: "chat",
                table: "Conversaciones",
                columns: new[] { "ClienteId", "UltimaActividadEn" });

            migrationBuilder.CreateIndex(
                name: "IX_Conversaciones_EstudianteId_UltimaActividadEn",
                schema: "chat",
                table: "Conversaciones",
                columns: new[] { "EstudianteId", "UltimaActividadEn" });

            migrationBuilder.CreateIndex(
                name: "IX_Conversaciones_ProyectoId_ClienteId_EstudianteId",
                schema: "chat",
                table: "Conversaciones",
                columns: new[] { "ProyectoId", "ClienteId", "EstudianteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_ConversacionId_EnviadoEn",
                schema: "chat",
                table: "Mensajes",
                columns: new[] { "ConversacionId", "EnviadoEn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mensajes",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "Conversaciones",
                schema: "chat");
        }
    }
}
