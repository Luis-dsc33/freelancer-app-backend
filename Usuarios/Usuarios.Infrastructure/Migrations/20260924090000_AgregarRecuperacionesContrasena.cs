using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Usuarios.Infrastructure.Persistence;

#nullable disable

namespace Usuarios.Infrastructure.Migrations;

[DbContext(typeof(UsuariosDbContext))]
[Migration("20260924090000_AgregarRecuperacionesContrasena")]
public partial class AgregarRecuperacionesContrasena : Migration
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("usuarios")
            .HasAnnotation("ProductVersion", "10.0.12")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("Usuarios.Domain.Entities.RecuperacionContrasena", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uuid");
            b.Property<DateTime?>("ConsumidoEn").HasColumnType("timestamp with time zone");
            b.Property<DateTime>("CreadoEn").HasColumnType("timestamp with time zone");
            b.Property<DateTime>("ExpiraEn").HasColumnType("timestamp with time zone");
            b.Property<DateTime?>("RevocadoEn").HasColumnType("timestamp with time zone");
            b.Property<string>("TokenHash").IsRequired().HasMaxLength(64).HasColumnType("character varying(64)");
            b.Property<Guid>("UsuarioId").HasColumnType("uuid");
            b.Property<Guid>("Version").IsConcurrencyToken().HasColumnType("uuid");
            b.HasKey("Id");
            b.HasIndex("TokenHash").IsUnique();
            b.HasIndex("UsuarioId", "ExpiraEn");
            b.ToTable("RecuperacionesContrasena", "usuarios");
        });

        modelBuilder.Entity("Usuarios.Domain.Entities.Rol", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uuid");
            b.Property<string>("Nombre").IsRequired().HasColumnType("text");
            b.HasKey("Id");
            b.ToTable("Roles", "usuarios");
            b.HasData(
                new { Id = new Guid("11111111-1111-1111-1111-111111111111"), Nombre = "Estudiante" },
                new { Id = new Guid("22222222-2222-2222-2222-222222222222"), Nombre = "Cliente" },
                new { Id = new Guid("33333333-3333-3333-3333-333333333333"), Nombre = "Administrador" });
        });

        modelBuilder.Entity("Usuarios.Domain.Entities.Usuario", b =>
        {
            b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uuid");
            b.Property<string>("Email").IsRequired().HasColumnType("text");
            b.Property<string>("Estado").IsRequired().HasColumnType("text");
            b.Property<DateTime>("FechaRegistro").HasColumnType("timestamp with time zone");
            b.Property<string>("Nombre").IsRequired().HasColumnType("text");
            b.Property<string>("PasswordHash").IsRequired().HasColumnType("text");
            b.Property<Guid>("RolId").HasColumnType("uuid");
            b.HasKey("Id");
            b.HasIndex("Email").IsUnique();
            b.HasIndex("RolId");
            b.ToTable("Usuarios", "usuarios");
        });

        modelBuilder.Entity("Usuarios.Domain.Entities.RecuperacionContrasena", b =>
        {
            b.HasOne("Usuarios.Domain.Entities.Usuario", "Usuario")
                .WithMany().HasForeignKey("UsuarioId").OnDelete(DeleteBehavior.Cascade).IsRequired();
            b.Navigation("Usuario");
        });

        modelBuilder.Entity("Usuarios.Domain.Entities.Usuario", b =>
        {
            b.HasOne("Usuarios.Domain.Entities.Rol", "Rol")
                .WithMany().HasForeignKey("RolId").OnDelete(DeleteBehavior.Cascade).IsRequired();
            b.Navigation("Rol");
        });
    }

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "RecuperacionesContrasena",
            schema: "usuarios",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                TokenHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                CreadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ExpiraEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ConsumidoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                RevocadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Version = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RecuperacionesContrasena", x => x.Id);
                table.ForeignKey(
                    name: "FK_RecuperacionesContrasena_Usuarios_UsuarioId",
                    column: x => x.UsuarioId,
                    principalSchema: "usuarios",
                    principalTable: "Usuarios",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_RecuperacionesContrasena_TokenHash",
            schema: "usuarios",
            table: "RecuperacionesContrasena",
            column: "TokenHash",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_RecuperacionesContrasena_UsuarioId_ExpiraEn",
            schema: "usuarios",
            table: "RecuperacionesContrasena",
            columns: new[] { "UsuarioId", "ExpiraEn" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RecuperacionesContrasena",
            schema: "usuarios");
    }
}
