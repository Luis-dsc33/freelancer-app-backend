using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIndiceContratosEstudiante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contratos_EstudianteId_Estado",
                schema: "marketplace",
                table: "Contratos",
                columns: new[] { "EstudianteId", "Estado" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contratos_EstudianteId_Estado",
                schema: "marketplace",
                table: "Contratos");
        }
    }
}
