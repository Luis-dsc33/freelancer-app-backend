using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Marketplace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AmpliarOfertasYCatalogoSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                schema: "marketplace",
                table: "Proyectos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alcance",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CantidadVacantes",
                schema: "marketplace",
                table: "Proyectos",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "CerradoEn",
                schema: "marketplace",
                table: "Proyectos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DedicacionSemanalEstimada",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EsquemaPago",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Unico");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaEntregaEstimada",
                schema: "marketplace",
                table: "Proyectos",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlexibilidadHorario",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GastosAdicionales",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modalidad",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Remoto");

            migrationBuilder.AddColumn<string>(
                name: "Moneda",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "MXN");

            migrationBuilder.AddColumn<string>(
                name: "NivelAcademicoSugerido",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NivelAvance",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PermitePortafolioPublico",
                schema: "marketplace",
                table: "Proyectos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PresupuestoMaximo",
                schema: "marketplace",
                table: "Proyectos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PresupuestoMinimo",
                schema: "marketplace",
                table: "Proyectos",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropiedadIntelectual",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicadoEn",
                schema: "marketplace",
                table: "Proyectos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiereConfidencialidad",
                schema: "marketplace",
                table: "Proyectos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequierePortafolio",
                schema: "marketplace",
                table: "Proyectos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TipoPresupuesto",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Fijo");

            migrationBuilder.AddColumn<string>(
                name: "Ubicacion",
                schema: "marketplace",
                table: "Proyectos",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Entregables",
                schema: "marketplace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProyectoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entregables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entregables_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalSchema: "marketplace",
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HitosPago",
                schema: "marketplace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProyectoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Porcentaje = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Importe = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    Orden = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HitosPago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HitosPago_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalSchema: "marketplace",
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Postulaciones",
                schema: "marketplace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProyectoId = table.Column<Guid>(type: "uuid", nullable: false),
                    EstudianteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Mensaje = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreadaEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResueltaEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postulaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Postulaciones_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalSchema: "marketplace",
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillCategories",
                schema: "marketplace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contratos",
                schema: "marketplace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostulacionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProyectoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    EstudianteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImporteAcordado = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaCompromiso = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CerradoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contratos_Postulaciones_PostulacionId",
                        column: x => x.PostulacionId,
                        principalSchema: "marketplace",
                        principalTable: "Postulaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                schema: "marketplace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    NombreNormalizado = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_SkillCategories_CategoriaId",
                        column: x => x.CategoriaId,
                        principalSchema: "marketplace",
                        principalTable: "SkillCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProyectoSkills",
                schema: "marketplace",
                columns: table => new
                {
                    ProyectoId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoRequisito = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectoSkills", x => new { x.ProyectoId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_ProyectoSkills_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalSchema: "marketplace",
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProyectoSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "marketplace",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "marketplace",
                table: "Proyectos",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ActualizadoEn", "Alcance", "CantidadVacantes", "CerradoEn", "DedicacionSemanalEstimada", "EsquemaPago", "FechaEntregaEstimada", "FlexibilidadHorario", "GastosAdicionales", "HabilidadesRequeridas", "Modalidad", "Moneda", "NivelAcademicoSugerido", "NivelAvance", "PermitePortafolioPublico", "PresupuestoMaximo", "PresupuestoMinimo", "PropiedadIntelectual", "PublicadoEn", "RequiereConfidencialidad", "RequierePortafolio", "TipoPresupuesto", "Ubicacion" },
                values: new object[] { null, null, 1, null, null, "Unico", null, null, null, new List<string> { "Angular", "Ionic" }, "Remoto", "MXN", null, null, false, null, null, null, null, false, false, "Fijo", null });

            migrationBuilder.UpdateData(
                schema: "marketplace",
                table: "Proyectos",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "ActualizadoEn", "Alcance", "CantidadVacantes", "CerradoEn", "DedicacionSemanalEstimada", "EsquemaPago", "FechaEntregaEstimada", "FlexibilidadHorario", "GastosAdicionales", "HabilidadesRequeridas", "Modalidad", "Moneda", "NivelAcademicoSugerido", "NivelAvance", "PermitePortafolioPublico", "PresupuestoMaximo", "PresupuestoMinimo", "PropiedadIntelectual", "PublicadoEn", "RequiereConfidencialidad", "RequierePortafolio", "TipoPresupuesto", "Ubicacion" },
                values: new object[] { null, null, 1, null, null, "Unico", null, null, null, new List<string> { "HTML", "CSS", "SEO" }, "Remoto", "MXN", null, null, false, null, null, null, null, false, false, "Fijo", null });

            migrationBuilder.InsertData(
                schema: "marketplace",
                table: "SkillCategories",
                columns: new[] { "Id", "Activo", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), true, "Habilidades de lenguajes de programación", "Lenguajes de programación" },
                    { new Guid("70000000-0000-0000-0000-000000000002"), true, "Habilidades de desarrollo frontend", "Desarrollo frontend" },
                    { new Guid("70000000-0000-0000-0000-000000000003"), true, "Habilidades de desarrollo backend y apis", "Desarrollo backend y APIs" },
                    { new Guid("70000000-0000-0000-0000-000000000004"), true, "Habilidades de bases de datos", "Bases de datos" },
                    { new Guid("70000000-0000-0000-0000-000000000005"), true, "Habilidades de desarrollo móvil y multiplataforma", "Desarrollo móvil y multiplataforma" },
                    { new Guid("70000000-0000-0000-0000-000000000006"), true, "Habilidades de inteligencia artificial y ciencia de datos", "Inteligencia artificial y ciencia de datos" },
                    { new Guid("70000000-0000-0000-0000-000000000007"), true, "Habilidades de cloud, devops y despliegue", "Cloud, DevOps y despliegue" },
                    { new Guid("70000000-0000-0000-0000-000000000008"), true, "Habilidades de ciberseguridad y redes", "Ciberseguridad y redes" },
                    { new Guid("70000000-0000-0000-0000-000000000009"), true, "Habilidades de diseño ui/ux y diseño gráfico", "Diseño UI/UX y diseño gráfico" },
                    { new Guid("70000000-0000-0000-0000-000000000010"), true, "Habilidades de producción audiovisual y multimedia", "Producción audiovisual y multimedia" },
                    { new Guid("70000000-0000-0000-0000-000000000011"), true, "Habilidades de marketing digital y comunicación", "Marketing digital y comunicación" },
                    { new Guid("70000000-0000-0000-0000-000000000012"), true, "Habilidades de ofimática, gestión y análisis empresarial", "Ofimática, gestión y análisis empresarial" },
                    { new Guid("70000000-0000-0000-0000-000000000013"), true, "Habilidades de testing y calidad de software", "Testing y calidad de software" },
                    { new Guid("70000000-0000-0000-0000-000000000014"), true, "Habilidades de hardware, soporte e iot", "Hardware, soporte e IoT" },
                    { new Guid("70000000-0000-0000-0000-000000000015"), true, "Habilidades de habilidades profesionales y transversales", "Habilidades profesionales y transversales" }
                });

            migrationBuilder.InsertData(
                schema: "marketplace",
                table: "Skills",
                columns: new[] { "Id", "Activo", "CategoriaId", "Descripcion", "Nombre", "NombreNormalizado" },
                values: new object[,]
                {
                    { new Guid("00703c98-0658-1194-8e3a-0d8a4596303d"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Ethical Hacking", "Ethical Hacking", "ETHICAL HACKING" },
                    { new Guid("0106bf86-c0b4-79ec-4180-9f75d85c83be"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Aprendizaje autónomo", "Aprendizaje autónomo", "APRENDIZAJE AUTONOMO" },
                    { new Guid("02189777-5939-7ca3-2e7b-6b7782cb7573"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Comunicación digital", "Comunicación digital", "COMUNICACION DIGITAL" },
                    { new Guid("02beb738-540b-f2c9-80f4-daa3b1eba7a3"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Astro", "Astro", "ASTRO" },
                    { new Guid("04da1e10-e60e-8820-9b94-2c51ffc0012e"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Gestión de identidades y accesos", "Gestión de identidades y accesos", "GESTION DE IDENTIDADES Y ACCESOS" },
                    { new Guid("06252e8e-a538-7be9-e934-53f94eb221db"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Electrónica básica", "Electrónica básica", "ELECTRONICA BASICA" },
                    { new Guid("07418829-217b-225b-7902-6ce589a39529"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Pentesting", "Pentesting", "PENTESTING" },
                    { new Guid("0754b8c7-cdad-c715-92be-3b2dac53a7c7"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Configuración de routers", "Configuración de routers", "CONFIGURACION DE ROUTERS" },
                    { new Guid("0812d0e3-bb1d-f633-01a2-0f309e1b8cf0"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Computer Vision", "Computer Vision", "COMPUTER VISION" },
                    { new Guid("08bfb7f9-18ac-7760-164f-aa115911f19e"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Investigación de mercado", "Investigación de mercado", "INVESTIGACION DE MERCADO" },
                    { new Guid("09565fa0-ab1d-b92f-19cd-bfa9e55fceed"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Swagger / OpenAPI", "Swagger / OpenAPI", "SWAGGER / OPENAPI" },
                    { new Guid("09dfda18-f73b-3858-0deb-5c0f961e09e1"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Scikit-learn", "Scikit-learn", "SCIKIT-LEARN" },
                    { new Guid("0b857d4e-4850-c1d9-9f44-bb0e446d2295"), true, new Guid("70000000-0000-0000-0000-000000000005"), "Kotlin Multiplatform", "Kotlin Multiplatform", "KOTLIN MULTIPLATFORM" },
                    { new Guid("0bc74181-d1e0-5f34-c997-9b6355659ebb"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Vitest", "Vitest", "VITEST" },
                    { new Guid("0d533b41-f365-231a-75fd-82941868486a"), true, new Guid("70000000-0000-0000-0000-000000000002"), "jQuery", "jQuery", "JQUERY" },
                    { new Guid("0f0f5d45-9116-30b2-c14e-50d366f079a9"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Internet of Things (IoT)", "Internet of Things (IoT)", "INTERNET OF THINGS (IOT)" },
                    { new Guid("12172e27-c1b4-26d2-0164-4598ac41b757"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Angular", "Angular", "ANGULAR" },
                    { new Guid("131e3744-6f40-4d67-ba44-4e2280238af7"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Gestión del tiempo", "Gestión del tiempo", "GESTION DEL TIEMPO" },
                    { new Guid("13bdf715-cdb9-26ad-0659-dcd315b7b59e"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Fotografía", "Fotografía", "FOTOGRAFIA" },
                    { new Guid("13d9840c-b98a-de4b-7889-f2b383c6fe0c"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Animación 3D", "Animación 3D", "ANIMACION 3D" },
                    { new Guid("14304d82-4b4c-381c-486b-b2b3a9887120"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Responsive Design", "Responsive Design", "RESPONSIVE DESIGN" },
                    { new Guid("14a911ac-ab9c-57bb-231a-5774ec1eb365"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Minería de datos", "Minería de datos", "MINERIA DE DATOS" },
                    { new Guid("14d8b440-aed5-f7d8-1ad7-dcc2aea57b0c"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Marketing estratégico", "Marketing estratégico", "MARKETING ESTRATEGICO" },
                    { new Guid("1606563f-065b-1847-59f0-ba2427654568"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Diseño responsivo", "Diseño responsivo", "DISENO RESPONSIVO" },
                    { new Guid("17070b22-b285-9018-4716-3c15331da0a1"), true, new Guid("70000000-0000-0000-0000-000000000004"), "PostgreSQL", "PostgreSQL", "POSTGRESQL" },
                    { new Guid("17d4a9db-08a7-51a9-0b89-aa44d3844563"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Sass / SCSS", "Sass / SCSS", "SASS / SCSS" },
                    { new Guid("182f6a7f-c359-9943-5e43-6f3dc90ec618"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Seguridad de aplicaciones web", "Seguridad de aplicaciones web", "SEGURIDAD DE APLICACIONES WEB" },
                    { new Guid("188d81d8-6018-6153-d505-04ae074907c0"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Next.js", "Next.js", "NEXT.JS" },
                    { new Guid("196f2857-6646-fa2b-ca3c-00a929c613e7"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Linux", "Linux", "LINUX" },
                    { new Guid("1b77799a-701c-21bf-7064-160e315b61bf"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Ensamblaje de computadoras", "Ensamblaje de computadoras", "ENSAMBLAJE DE COMPUTADORAS" },
                    { new Guid("1b96d552-8f32-ca3d-974c-35164a5680ac"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Netlify", "Netlify", "NETLIFY" },
                    { new Guid("1d2eab5f-3ad7-f2be-1627-77718aca345d"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Diseño de presentaciones", "Diseño de presentaciones", "DISENO DE PRESENTACIONES" },
                    { new Guid("1e178873-349a-819f-0e5d-23510c055441"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Google Cloud Platform", "Google Cloud Platform", "GOOGLE CLOUD PLATFORM" },
                    { new Guid("1e55c1ad-7670-84ea-ba17-e1131de8ebb3"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Sensores y actuadores", "Sensores y actuadores", "SENSORES Y ACTUADORES" },
                    { new Guid("222e6171-2040-d1d2-4ab5-6952068b5bd8"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Pensamiento crítico", "Pensamiento crítico", "PENSAMIENTO CRITICO" },
                    { new Guid("22c790b4-52a6-d8b5-d0b4-85eeae32f4d2"), true, new Guid("70000000-0000-0000-0000-000000000005"), "Jetpack Compose", "Jetpack Compose", "JETPACK COMPOSE" },
                    { new Guid("235df400-c6a6-3c10-e7b5-4f135435874d"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Deep Learning", "Deep Learning", "DEEP LEARNING" },
                    { new Guid("24a6f1e7-51f9-70c6-e455-14ef988417af"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Adobe Audition", "Adobe Audition", "ADOBE AUDITION" },
                    { new Guid("24e24279-5f16-b080-286b-28fc8468ea33"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Zustand", "Zustand", "ZUSTAND" },
                    { new Guid("258725a7-817f-b7bc-1ce0-481de9e2bfdc"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Web Accessibility", "Web Accessibility", "WEB ACCESSIBILITY" },
                    { new Guid("25a47606-9c16-f7f5-d694-f01d6237b10e"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Raspberry Pi", "Raspberry Pi", "RASPBERRY PI" },
                    { new Guid("25d3fa89-a697-64db-511a-bbd66910a132"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Automatización", "Automatización", "AUTOMATIZACION" },
                    { new Guid("281e9a47-e1f3-77a9-bd01-a51ae4150be0"), true, new Guid("70000000-0000-0000-0000-000000000001"), "Kotlin", "Kotlin", "KOTLIN" },
                    { new Guid("2a35cd67-b7df-9554-c593-74aa23ee7ecb"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Marketing digital", "Marketing digital", "MARKETING DIGITAL" },
                    { new Guid("2a97c966-3932-38a3-e747-f506af89d68b"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Microsoft Access", "Microsoft Access", "MICROSOFT ACCESS" },
                    { new Guid("2b1b1028-f8a2-7544-29fc-038a2d575b13"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Análisis de datos", "Análisis de datos", "ANALISIS DE DATOS" },
                    { new Guid("2baabb4e-e6d7-487d-6472-e928bf15bd78"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Testing de integración", "Testing de integración", "TESTING DE INTEGRACION" },
                    { new Guid("2bb6b7da-f0a1-c035-b931-a19e6b64fb70"), true, new Guid("70000000-0000-0000-0000-000000000001"), "SQL", "SQL", "SQL" },
                    { new Guid("2db60286-071f-e8a0-669b-297a961bd405"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Autodesk Maya", "Autodesk Maya", "AUTODESK MAYA" },
                    { new Guid("2eb3f381-df66-cef2-a3c1-0db0407d4fbf"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Animación 2D", "Animación 2D", "ANIMACION 2D" },
                    { new Guid("2ec4f867-4db3-a987-e690-428380363359"), true, new Guid("70000000-0000-0000-0000-000000000007"), "GitLab", "GitLab", "GITLAB" },
                    { new Guid("2fae9539-39ef-9e0d-54c3-5ef2c7a55d2a"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Redes TCP/IP", "Redes TCP/IP", "REDES TCP/IP" },
                    { new Guid("30a1c486-5a23-b3e7-7dd6-8b866544ec15"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Análisis predictivo", "Análisis predictivo", "ANALISIS PREDICTIVO" },
                    { new Guid("3142fc5b-145d-eae0-9ac8-0d59553631c9"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Procesamiento de lenguaje natural (NLP)", "Procesamiento de lenguaje natural (NLP)", "PROCESAMIENTO DE LENGUAJE NATURAL (NLP)" },
                    { new Guid("33921b52-271b-83c6-a8f5-4760af7750c3"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Kanban", "Kanban", "KANBAN" },
                    { new Guid("34271f40-d721-3d8d-bcfb-07e47b5ce8e5"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Adobe Premiere Pro", "Adobe Premiere Pro", "ADOBE PREMIERE PRO" },
                    { new Guid("343c08f6-bab5-2dc4-902d-908ebc377e69"), true, new Guid("70000000-0000-0000-0000-000000000014"), "ESP32", "ESP32", "ESP32" },
                    { new Guid("3570b0a5-5fea-82ca-589c-32102dd0aad1"), true, new Guid("70000000-0000-0000-0000-000000000007"), "GitHub Actions", "GitHub Actions", "GITHUB ACTIONS" },
                    { new Guid("357a3fed-6ec6-8949-38ca-2b0edcc6bf8a"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Scrum", "Scrum", "SCRUM" },
                    { new Guid("35b28f83-066d-e257-615d-11d21c21f6d3"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Svelte", "Svelte", "SVELTE" },
                    { new Guid("35b8224f-19ef-84ea-c1e5-45e4c6da62f7"), true, new Guid("70000000-0000-0000-0000-000000000005"), "Ionic", "Ionic", "IONIC" },
                    { new Guid("362a5dba-c678-b307-9bae-cca7fd3c0b18"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Adaptabilidad", "Adaptabilidad", "ADAPTABILIDAD" },
                    { new Guid("3756feea-38cf-c545-273d-d5b6c6e24977"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Gestión de campañas publicitarias", "Gestión de campañas publicitarias", "GESTION DE CAMPANAS PUBLICITARIAS" },
                    { new Guid("37671610-90a9-ad08-27df-3e70b70af10e"), true, new Guid("70000000-0000-0000-0000-000000000012"), "BPMN", "BPMN", "BPMN" },
                    { new Guid("37a9bdae-660a-748d-fd60-37afc92a8077"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Gestión de proyectos", "Gestión de proyectos", "GESTION DE PROYECTOS" },
                    { new Guid("381401ec-1440-a093-7c15-0fee2a0ae502"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Comunicación oral", "Comunicación oral", "COMUNICACION ORAL" },
                    { new Guid("3b937a7e-b70c-080d-2d66-0566b26c4f82"), true, new Guid("70000000-0000-0000-0000-000000000001"), "Dart", "Dart", "DART" },
                    { new Guid("3d3ff95c-06e0-abf4-b394-65c7a7b6b97c"), true, new Guid("70000000-0000-0000-0000-000000000004"), "MariaDB", "MariaDB", "MARIADB" },
                    { new Guid("3dc3e212-0122-7fc1-f016-1b0d2b1d7a95"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Análisis estático de código", "Análisis estático de código", "ANALISIS ESTATICO DE CODIGO" },
                    { new Guid("3fac950a-5c3c-3d02-49ef-5264b3b9ce79"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Pandas", "Pandas", "PANDAS" },
                    { new Guid("403963e8-8469-2989-77e9-1f0a155d4f89"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Estrategia de marca", "Estrategia de marca", "ESTRATEGIA DE MARCA" },
                    { new Guid("4120923f-f024-f87c-a6f1-924cc0eda4c6"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Ruby on Rails", "Ruby on Rails", "RUBY ON RAILS" },
                    { new Guid("41ca0f84-6195-8cc4-f50e-c1f4c4d6a471"), true, new Guid("70000000-0000-0000-0000-000000000004"), "Normalización de bases de datos", "Normalización de bases de datos", "NORMALIZACION DE BASES DE DATOS" },
                    { new Guid("4208f319-f571-2791-480f-42b06db58ad9"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Guionismo", "Guionismo", "GUIONISMO" },
                    { new Guid("42fccaa2-582b-4f8d-a17b-8f7beeb16a66"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Ciencia de datos", "Ciencia de datos", "CIENCIA DE DATOS" },
                    { new Guid("43d82e85-4111-f64e-37a9-7be10ede8e24"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Diseño sonoro", "Diseño sonoro", "DISENO SONORO" },
                    { new Guid("4671e4e3-969c-3dae-80c5-055d342e4888"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Adobe InDesign", "Adobe InDesign", "ADOBE INDESIGN" },
                    { new Guid("47ad63e8-aa9c-4897-c459-ab406f70e28e"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Power Query", "Power Query", "POWER QUERY" },
                    { new Guid("496ed55f-5ef0-6204-3b0e-7968329bd8b8"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Trabajo en equipo", "Trabajo en equipo", "TRABAJO EN EQUIPO" },
                    { new Guid("4a9f351d-16de-33b7-80d6-ef730ede3fff"), true, new Guid("70000000-0000-0000-0000-000000000003"), "gRPC", "gRPC", "GRPC" },
                    { new Guid("4aefe94c-d55b-bd43-be69-9cf5eb844c72"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Big Data", "Big Data", "BIG DATA" },
                    { new Guid("4afb0f63-a60e-570b-91a0-7c8fb1693099"), true, new Guid("70000000-0000-0000-0000-000000000001"), "JavaScript", "JavaScript", "JAVASCRIPT" },
                    { new Guid("4b3e7544-b7c9-5089-60ad-7fa736390b73"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Producción musical", "Producción musical", "PRODUCCION MUSICAL" },
                    { new Guid("4c124661-c3d3-4cf9-b12b-66018a146efe"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Diseño de identidad visual", "Diseño de identidad visual", "DISENO DE IDENTIDAD VISUAL" },
                    { new Guid("4d462ab5-794d-9ddf-da09-825972aa1252"), true, new Guid("70000000-0000-0000-0000-000000000001"), "Swift", "Swift", "SWIFT" },
                    { new Guid("4de89485-3e10-873f-1b93-981ec549186d"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Clustering", "Clustering", "CLUSTERING" },
                    { new Guid("4dead1ba-63c0-4b69-aef5-40e4829b6ac6"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Microsoft Azure", "Microsoft Azure", "MICROSOFT AZURE" },
                    { new Guid("4e5886c6-93e3-addf-287b-230ad3fca41f"), true, new Guid("70000000-0000-0000-0000-000000000005"), "Android Development", "Android Development", "ANDROID DEVELOPMENT" },
                    { new Guid("4e9043cc-1703-ebd1-c95d-89c07574b362"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Apache", "Apache", "APACHE" },
                    { new Guid("4ea37da6-3387-663a-7365-e8f440938a9b"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Configuración de entornos", "Configuración de entornos", "CONFIGURACION DE ENTORNOS" },
                    { new Guid("4fd2d5b5-915a-2e7a-af83-790b3ba2db1e"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Bootstrap", "Bootstrap", "BOOTSTRAP" },
                    { new Guid("5028f333-03ee-8e26-bb2b-270abce17dfc"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Diseño editorial", "Diseño editorial", "DISENO EDITORIAL" },
                    { new Guid("5037f961-a3e5-1247-8d5d-b1f7e999a5ec"), true, new Guid("70000000-0000-0000-0000-000000000004"), "Supabase", "Supabase", "SUPABASE" },
                    { new Guid("5065878a-ab14-e388-04bf-71ce3001f385"), true, new Guid("70000000-0000-0000-0000-000000000003"), "GraphQL", "GraphQL", "GRAPHQL" },
                    { new Guid("50770187-5eac-26b1-a5d2-5b9f96fa9f43"), true, new Guid("70000000-0000-0000-0000-000000000007"), "AWS", "AWS", "AWS" },
                    { new Guid("51c04c8a-1185-21a5-c525-eece3be22eaa"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Administración de redes", "Administración de redes", "ADMINISTRACION DE REDES" },
                    { new Guid("53093606-b0ac-8ccb-9f42-07b5aeadd730"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Soporte técnico", "Soporte técnico", "SOPORTE TECNICO" },
                    { new Guid("532de441-fb17-0c30-a143-a645881a09e1"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Selenium", "Selenium", "SELENIUM" },
                    { new Guid("5839e485-3362-4324-2665-8c6e426824aa"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Diseño de material publicitario", "Diseño de material publicitario", "DISENO DE MATERIAL PUBLICITARIO" },
                    { new Guid("5a8d5c9b-ff3b-063a-0798-4e74ccf1c161"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Git", "Git", "GIT" },
                    { new Guid("5ab6fa36-6d25-480e-eaa8-ea88fed7ea04"), true, new Guid("70000000-0000-0000-0000-000000000001"), "PHP", "PHP", "PHP" },
                    { new Guid("5adf6e81-67ca-3342-153a-0cf8fb91ade2"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Jupyter Notebook", "Jupyter Notebook", "JUPYTER NOTEBOOK" },
                    { new Guid("5d75a96f-43a1-17fe-f7a7-4b140cf5700b"), true, new Guid("70000000-0000-0000-0000-000000000001"), "MATLAB", "MATLAB", "MATLAB" },
                    { new Guid("5e03ded2-82cc-3cb2-d179-8c1c11bee772"), true, new Guid("70000000-0000-0000-0000-000000000009"), "UX Design", "UX Design", "UX DESIGN" },
                    { new Guid("5e565146-0b61-950d-9890-4057683f910f"), true, new Guid("70000000-0000-0000-0000-000000000004"), "MongoDB", "MongoDB", "MONGODB" },
                    { new Guid("5e7fc1fa-1389-2713-e230-768095682007"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Playwright", "Playwright", "PLAYWRIGHT" },
                    { new Guid("5f22f417-6d33-3440-bab1-e88d52b28140"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Kali Linux", "Kali Linux", "KALI LINUX" },
                    { new Guid("5f42ac4e-d8c5-de0b-b865-7b1cb4e4063a"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Storytelling", "Storytelling", "STORYTELLING" },
                    { new Guid("5f7ed375-cb97-4152-cf25-95f5089c42ff"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Docker", "Docker", "DOCKER" },
                    { new Guid("5fd7e46e-1f40-aedd-1fc8-2e99856b0582"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Prompt Engineering", "Prompt Engineering", "PROMPT ENGINEERING" },
                    { new Guid("60df1d5e-8cce-bbe5-065d-2aad6c0a8a14"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Generative AI", "Generative AI", "GENERATIVE AI" },
                    { new Guid("613b3904-2931-65e0-ed48-d1ef81c40b44"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Orientación a resultados", "Orientación a resultados", "ORIENTACION A RESULTADOS" },
                    { new Guid("61a1ead3-76b8-a4d3-0cab-4c98bf1be85a"), true, new Guid("70000000-0000-0000-0000-000000000013"), "xUnit", "xUnit", "XUNIT" },
                    { new Guid("641a86fb-f72b-36a5-e0b5-cfa476f21489"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Configuración de switches", "Configuración de switches", "CONFIGURACION DE SWITCHES" },
                    { new Guid("670f07ca-c30e-29eb-2100-841588e24115"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Jira", "Jira", "JIRA" },
                    { new Guid("672628fc-33cc-ffed-0cca-790800933e49"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Wireshark", "Wireshark", "WIRESHARK" },
                    { new Guid("672f3bbc-622b-da2a-2ded-144bbfc699ee"), true, new Guid("70000000-0000-0000-0000-000000000005"), "SwiftUI", "SwiftUI", "SWIFTUI" },
                    { new Guid("680dec49-9ddb-a472-20b9-2595961fac59"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Kubernetes", "Kubernetes", "KUBERNETES" },
                    { new Guid("68482dd2-b049-043a-c8d0-398302dad3ca"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Adobe Photoshop", "Adobe Photoshop", "ADOBE PHOTOSHOP" },
                    { new Guid("6881cd04-7f9d-2e36-558e-0518cd3a33e2"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Levantamiento de requerimientos", "Levantamiento de requerimientos", "LEVANTAMIENTO DE REQUERIMIENTOS" },
                    { new Guid("68f7a5ee-c065-8513-8410-a383eb11794d"), true, new Guid("70000000-0000-0000-0000-000000000008"), "OWASP Top 10", "OWASP Top 10", "OWASP TOP 10" },
                    { new Guid("69d42b13-6c99-ebe1-3ee8-3fd13ed5f94e"), true, new Guid("70000000-0000-0000-0000-000000000004"), "Modelado entidad-relación", "Modelado entidad-relación", "MODELADO ENTIDAD-RELACION" },
                    { new Guid("6a30b4ca-0b76-f507-760f-08f7c79a5fa5"), true, new Guid("70000000-0000-0000-0000-000000000001"), "Rust", "Rust", "RUST" },
                    { new Guid("6a7d3dc1-e74f-cfa0-76de-80a5ed0671c2"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Depuración de código", "Depuración de código", "DEPURACION DE CODIGO" },
                    { new Guid("6c167dc8-cfc8-620d-fd6a-ad7ecd1b9f7e"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Sistemas embebidos", "Sistemas embebidos", "SISTEMAS EMBEBIDOS" },
                    { new Guid("6c2f0cef-ae98-be12-1929-910bd5e456a5"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Jest", "Jest", "JEST" },
                    { new Guid("6d3faffe-4654-a354-d954-8f00f8d4c658"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Negociación", "Negociación", "NEGOCIACION" },
                    { new Guid("6d53c9c4-be57-9f8a-5a00-d39c5c0c36e3"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Modelado 3D", "Modelado 3D", "MODELADO 3D" },
                    { new Guid("6df1deda-4b88-71cc-17f3-01bf1c3ba661"), true, new Guid("70000000-0000-0000-0000-000000000006"), "NumPy", "NumPy", "NUMPY" },
                    { new Guid("6e5b91b2-3f90-f25a-4238-bc83d6aa61f8"), true, new Guid("70000000-0000-0000-0000-000000000013"), "JUnit", "JUnit", "JUNIT" },
                    { new Guid("73a00cfd-4e54-32aa-f228-9add42829395"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Blender", "Blender", "BLENDER" },
                    { new Guid("74e0a8b7-4f49-d72a-5753-3219c131fd41"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Diseño de experiencia de usuario", "Diseño de experiencia de usuario", "DISENO DE EXPERIENCIA DE USUARIO" },
                    { new Guid("76a13760-3e17-5538-ef9e-07222008cd82"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Gestión de redes sociales", "Gestión de redes sociales", "GESTION DE REDES SOCIALES" },
                    { new Guid("76d59d23-21f2-278a-cdd4-12e565052c37"), true, new Guid("70000000-0000-0000-0000-000000000003"), "NestJS", "NestJS", "NESTJS" },
                    { new Guid("76e6049b-6c55-80b7-4ebf-b52e5df5bf9b"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Resolución de problemas", "Resolución de problemas", "RESOLUCION DE PROBLEMAS" },
                    { new Guid("7cd133c8-d5c8-ba10-99f1-ecec920ff211"), true, new Guid("70000000-0000-0000-0000-000000000011"), "SEO", "SEO", "SEO" },
                    { new Guid("7dad9aaf-292e-bce5-b2a8-c66d0f5583f6"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Programación de microcontroladores", "Programación de microcontroladores", "PROGRAMACION DE MICROCONTROLADORES" },
                    { new Guid("7e30502d-2a06-0fee-bed9-145c05b9924e"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Wireframing", "Wireframing", "WIREFRAMING" },
                    { new Guid("7ebb9978-6ee6-c5ba-0856-f80ccac475e6"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Copywriting", "Copywriting", "COPYWRITING" },
                    { new Guid("7ee49ab7-823c-645f-08d1-120ebf05a841"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Regresión", "Regresión", "REGRESION" },
                    { new Guid("7f52b2ee-3484-9530-658a-a35e67ebd3f5"), true, new Guid("70000000-0000-0000-0000-000000000010"), "DaVinci Resolve", "DaVinci Resolve", "DAVINCI RESOLVE" },
                    { new Guid("80109d52-6f26-225e-f892-07e571fc396b"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Tableau", "Tableau", "TABLEAU" },
                    { new Guid("80981feb-7b7f-5313-ccbd-3ba3391d3fca"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Mantenimiento correctivo", "Mantenimiento correctivo", "MANTENIMIENTO CORRECTIVO" },
                    { new Guid("84d5298d-9b45-412f-3f34-5e8fea8e1f4d"), true, new Guid("70000000-0000-0000-0000-000000000005"), "iOS Development", "iOS Development", "IOS DEVELOPMENT" },
                    { new Guid("856e48d9-9e19-77bd-9a64-278cdc95fdd1"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Seguridad de APIs", "Seguridad de APIs", "SEGURIDAD DE APIS" },
                    { new Guid("864d51a1-792f-bc53-1302-a5eb4e90c8aa"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Microservicios", "Microservicios", "MICROSERVICIOS" },
                    { new Guid("8760c6a2-e464-ae10-78e2-4d7e6ab16269"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Análisis de requerimientos", "Análisis de requerimientos", "ANALISIS DE REQUERIMIENTOS" },
                    { new Guid("8a575a77-4ef6-4385-0882-e67616d3ee2e"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Modelado de procesos", "Modelado de procesos", "MODELADO DE PROCESOS" },
                    { new Guid("8a5973ab-2f92-4eab-4ad1-fc8b2e986109"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Despliegue de aplicaciones", "Despliegue de aplicaciones", "DESPLIEGUE DE APLICACIONES" },
                    { new Guid("8a70a495-a5e5-82ba-fe99-e8aba33b15d8"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Criptografía", "Criptografía", "CRIPTOGRAFIA" },
                    { new Guid("8b609ac5-83a2-230a-3cbe-6e1f41a4cf59"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Active Directory", "Active Directory", "ACTIVE DIRECTORY" },
                    { new Guid("8c31a6b3-023c-5ca3-5d17-3ad10b438c4e"), true, new Guid("70000000-0000-0000-0000-000000000009"), "UX Research", "UX Research", "UX RESEARCH" },
                    { new Guid("8d9bf309-c362-9737-ba40-755c4b370ba4"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Adobe After Effects", "Adobe After Effects", "ADOBE AFTER EFFECTS" },
                    { new Guid("8ecdbd80-33b0-614e-62b9-0865e3bba3ab"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Instalación de sistemas operativos", "Instalación de sistemas operativos", "INSTALACION DE SISTEMAS OPERATIVOS" },
                    { new Guid("8ee00c33-efa6-11e2-baa4-b67aef2bcfcc"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Excel avanzado", "Excel avanzado", "EXCEL AVANZADO" },
                    { new Guid("8f93d50e-3813-70e5-ff2a-eafb4e415286"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Express.js", "Express.js", "EXPRESS.JS" },
                    { new Guid("8fd7ff43-ef46-0e61-b17d-f69e8b8dc1c4"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Redes neuronales", "Redes neuronales", "REDES NEURONALES" },
                    { new Guid("9006fd5c-aafd-f288-837a-5c83c17dcd97"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Prototipado", "Prototipado", "PROTOTIPADO" },
                    { new Guid("902ae280-5eea-501f-83f7-70d73991554d"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Producción audiovisual", "Producción audiovisual", "PRODUCCION AUDIOVISUAL" },
                    { new Guid("90d6217f-a70a-0728-62a0-bcf6b75df7cc"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Google Analytics", "Google Analytics", "GOOGLE ANALYTICS" },
                    { new Guid("9172d711-4d04-0cc7-7263-55ae61cb49dd"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Spring Boot", "Spring Boot", "SPRING BOOT" },
                    { new Guid("92d6ca50-5ac2-c954-1207-425397182170"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Firewalls", "Firewalls", "FIREWALLS" },
                    { new Guid("93f4dadb-7f07-b532-435b-5587e5687451"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Gestión de equipos", "Gestión de equipos", "GESTION DE EQUIPOS" },
                    { new Guid("9420e293-1155-e7af-c369-87189e2553c6"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Shopify", "Shopify", "SHOPIFY" },
                    { new Guid("94a41571-fb98-a453-849c-bda82760265a"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Autonomía", "Autonomía", "AUTONOMIA" },
                    { new Guid("9622f62e-e283-3297-c2b6-f14f480fd863"), true, new Guid("70000000-0000-0000-0000-000000000001"), "Ruby", "Ruby", "RUBY" },
                    { new Guid("967ee897-3fd9-111c-7f83-644b122388d4"), true, new Guid("70000000-0000-0000-0000-000000000005"), "React Native", "React Native", "REACT NATIVE" },
                    { new Guid("976d0555-78e8-3b78-b602-3cd0dd897deb"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Design Systems", "Design Systems", "DESIGN SYSTEMS" },
                    { new Guid("986da2a9-9cc1-ceed-7926-a48565825d98"), true, new Guid("70000000-0000-0000-0000-000000000003"), ".NET", ".NET", ".NET" },
                    { new Guid("99db5cb1-243b-429a-edad-c62225f3c019"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Tailwind CSS", "Tailwind CSS", "TAILWIND CSS" },
                    { new Guid("99fc1c55-b617-ef31-c68c-88c497b1692c"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Modelos de lenguaje (LLM)", "Modelos de lenguaje (LLM)", "MODELOS DE LENGUAJE (LLM)" },
                    { new Guid("9a091ad5-0ab0-1eba-5e01-4c0de4d25982"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Monitoreo de aplicaciones", "Monitoreo de aplicaciones", "MONITOREO DE APLICACIONES" },
                    { new Guid("9a95ca19-1e39-4ef4-7be3-16c17fc2b78d"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Google Ads", "Google Ads", "GOOGLE ADS" },
                    { new Guid("9aa60b24-c52e-1fdd-25ef-a8076b130bcc"), true, new Guid("70000000-0000-0000-0000-000000000004"), "Oracle Database", "Oracle Database", "ORACLE DATABASE" },
                    { new Guid("9ae072c6-8162-227e-e293-fec36548e141"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Pensamiento analítico", "Pensamiento analítico", "PENSAMIENTO ANALITICO" },
                    { new Guid("9cd1077e-31cd-2a96-9b8e-8da4ac6a5d21"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Elaboración de reportes", "Elaboración de reportes", "ELABORACION DE REPORTES" },
                    { new Guid("9dc77ca8-0dd0-c343-74bc-a4c6200848c0"), true, new Guid("70000000-0000-0000-0000-000000000001"), "Go", "Go", "GO" },
                    { new Guid("a0399141-6146-c9d6-6e96-e44ad5b5d3ee"), true, new Guid("70000000-0000-0000-0000-000000000001"), "R", "R", "R" },
                    { new Guid("a274c21e-2305-baf2-fedf-40fe86401bfb"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Progressive Web Apps (PWA)", "Progressive Web Apps (PWA)", "PROGRESSIVE WEB APPS (PWA)" },
                    { new Guid("a2e04243-badf-def0-5f50-5b8915c5fd10"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Vercel", "Vercel", "VERCEL" },
                    { new Guid("a310cbb6-793c-d967-4315-47b08ba7e96e"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Adobe XD", "Adobe XD", "ADOBE XD" },
                    { new Guid("a3155bd1-7208-1d1e-4590-3846344c400d"), true, new Guid("70000000-0000-0000-0000-000000000007"), "CI/CD", "CI/CD", "CI/CD" },
                    { new Guid("a3ddc019-ac8d-ad6c-4512-db7070d040ea"), true, new Guid("70000000-0000-0000-0000-000000000004"), "Diseño de bases de datos", "Diseño de bases de datos", "DISENO DE BASES DE DATOS" },
                    { new Guid("a5a88e59-e71e-d952-c439-3fc171f43d05"), true, new Guid("70000000-0000-0000-0000-000000000007"), "GitHub", "GitHub", "GITHUB" },
                    { new Guid("a7415abf-5696-3ea0-b13d-c85f691c6c26"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Flask", "Flask", "FLASK" },
                    { new Guid("a91d1958-3ad6-ff85-a341-33ccb2eef669"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Cypress", "Cypress", "CYPRESS" },
                    { new Guid("a933546b-9b75-6ddb-1a8f-22e39a0af93d"), true, new Guid("70000000-0000-0000-0000-000000000001"), "C", "C", "C" },
                    { new Guid("a9488a2c-c28d-2fd9-de60-3f045b732d32"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Seguridad de bases de datos", "Seguridad de bases de datos", "SEGURIDAD DE BASES DE DATOS" },
                    { new Guid("a99aebd7-4f1a-9f8e-93e3-0636b16aa694"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Web Components", "Web Components", "WEB COMPONENTS" },
                    { new Guid("a9c15c33-21ee-a55f-438f-98e1299bd72c"), true, new Guid("70000000-0000-0000-0000-000000000001"), "Java", "Java", "JAVA" },
                    { new Guid("aab27f18-cc71-c09f-c770-cb57106cb4cc"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Creatividad", "Creatividad", "CREATIVIDAD" },
                    { new Guid("accaa4c6-fa93-a1db-c913-adabd5eddc83"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Microsoft PowerPoint", "Microsoft PowerPoint", "MICROSOFT POWERPOINT" },
                    { new Guid("adc91a71-be32-08b6-e775-da5d5f8e28f1"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Diseño de interfaces", "Diseño de interfaces", "DISENO DE INTERFACES" },
                    { new Guid("add1b648-9511-c9bb-549a-9d027e8d38b4"), true, new Guid("70000000-0000-0000-0000-000000000003"), "OAuth 2.0", "OAuth 2.0", "OAUTH 2.0" },
                    { new Guid("add5f36f-d44f-ffca-752c-2e80fa1a86ec"), true, new Guid("70000000-0000-0000-0000-000000000004"), "SQL Server", "SQL Server", "SQL SERVER" },
                    { new Guid("ae0e6dfe-16bf-940f-76cc-6867696d9cee"), true, new Guid("70000000-0000-0000-0000-000000000005"), ".NET MAUI", ".NET MAUI", ".NET MAUI" },
                    { new Guid("af65995a-c43d-f2e9-c75d-40670d81a5bb"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Nginx", "Nginx", "NGINX" },
                    { new Guid("b04b594c-22ab-51fa-341a-47e7ce2831a6"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Email Marketing", "Email Marketing", "EMAIL MARKETING" },
                    { new Guid("b0bc07dc-d51b-8a86-0afa-c595297549d6"), true, new Guid("70000000-0000-0000-0000-000000000002"), "React", "React", "REACT" },
                    { new Guid("b0cd3b3c-4191-5bcc-02cb-0208ba042dee"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Meta Ads", "Meta Ads", "META ADS" },
                    { new Guid("b2459233-96f5-9f2b-9658-f79363124897"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Arduino", "Arduino", "ARDUINO" },
                    { new Guid("b4551c38-bfda-8548-866e-8b95a5bde431"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Organización", "Organización", "ORGANIZACION" },
                    { new Guid("b7911880-6d9b-9449-7855-00268f2130af"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Ilustración digital", "Ilustración digital", "ILUSTRACION DIGITAL" },
                    { new Guid("b792a1c6-e6b5-98ba-f4a4-cbc5159eb15e"), true, new Guid("70000000-0000-0000-0000-000000000001"), "Visual Basic .NET", "Visual Basic .NET", "VISUAL BASIC .NET" },
                    { new Guid("b7be25d1-753f-95ee-7e09-7a86599785c5"), true, new Guid("70000000-0000-0000-0000-000000000004"), "MySQL", "MySQL", "MYSQL" },
                    { new Guid("b7f985bb-989f-6446-301a-39a8b40c1e41"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Agile", "Agile", "AGILE" },
                    { new Guid("b98c3488-6ca7-5e21-36c2-06a7fcd3ad5b"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Pruebas manuales", "Pruebas manuales", "PRUEBAS MANUALES" },
                    { new Guid("bb6378d2-3c75-2fcb-821e-b4631711364b"), true, new Guid("70000000-0000-0000-0000-000000000012"), "UML", "UML", "UML" },
                    { new Guid("bb653780-8d2b-db4c-5653-234388f99af0"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Trello", "Trello", "TRELLO" },
                    { new Guid("bba7501a-a613-1104-aa09-2ab9635b3213"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Diagnóstico de hardware", "Diagnóstico de hardware", "DIAGNOSTICO DE HARDWARE" },
                    { new Guid("bbab448b-97b1-8e1e-55cc-1b6b1db3512b"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Clasificación", "Clasificación", "CLASIFICACION" },
                    { new Guid("bd60c671-8304-f4e6-786d-557bfad615cb"), true, new Guid("70000000-0000-0000-0000-000000000004"), "PL/SQL", "PL/SQL", "PL/SQL" },
                    { new Guid("bd870776-2cd3-1818-5608-30147a784e75"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Administración de servidores", "Administración de servidores", "ADMINISTRACION DE SERVIDORES" },
                    { new Guid("bdbd7d69-71db-729a-1cc1-c6bdc9d94f59"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Branding", "Branding", "BRANDING" },
                    { new Guid("be0c041e-1ef8-f15c-476f-564b12323c69"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Social Media Management", "Social Media Management", "SOCIAL MEDIA MANAGEMENT" },
                    { new Guid("c00b39c9-7f26-a49b-1849-ec035cb9c5cb"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Google Sheets", "Google Sheets", "GOOGLE SHEETS" },
                    { new Guid("c02f40a8-d6fa-f91e-bcf7-fc509a9f5dc6"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Redux", "Redux", "REDUX" },
                    { new Guid("c257a4dc-13df-ef30-ded8-2d89d1433acd"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Canva", "Canva", "CANVA" },
                    { new Guid("c2995bc0-4423-b941-5951-bc56e12675de"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Seguridad informática", "Seguridad informática", "SEGURIDAD INFORMATICA" },
                    { new Guid("c4631407-8263-181d-a892-2b786f6ed569"), true, new Guid("70000000-0000-0000-0000-000000000001"), "TypeScript", "TypeScript", "TYPESCRIPT" },
                    { new Guid("c4b389a7-dd05-69a6-b426-6520d83fd839"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Diseño de logotipos", "Diseño de logotipos", "DISENO DE LOGOTIPOS" },
                    { new Guid("c4efe86d-d788-d5ad-c206-6cba9c05930f"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Análisis de vulnerabilidades", "Análisis de vulnerabilidades", "ANALISIS DE VULNERABILIDADES" },
                    { new Guid("c53e0552-eff8-0850-536c-22888aeb6676"), true, new Guid("70000000-0000-0000-0000-000000000005"), "Xamarin", "Xamarin", "XAMARIN" },
                    { new Guid("c5596e2b-71b1-6634-cb54-5ffe5658a1d0"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Power Pivot", "Power Pivot", "POWER PIVOT" },
                    { new Guid("c56d0586-204f-c3bb-83cf-4fb0c1f03c9c"), true, new Guid("70000000-0000-0000-0000-000000000001"), "PowerShell", "PowerShell", "POWERSHELL" },
                    { new Guid("c66733c6-fa6f-625a-723c-1d7e9e3fcee3"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Creación de contenido digital", "Creación de contenido digital", "CREACION DE CONTENIDO DIGITAL" },
                    { new Guid("c739aba0-b230-4977-575e-67bb7f67ec9a"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Laravel", "Laravel", "LARAVEL" },
                    { new Guid("c7b6dcf1-35b6-1554-2293-c7eb63f9402b"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Mantenimiento preventivo", "Mantenimiento preventivo", "MANTENIMIENTO PREVENTIVO" },
                    { new Guid("c88fbcd2-5ac7-19b3-0313-97b7aa7b1f1c"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Django", "Django", "DJANGO" },
                    { new Guid("c8e476b7-e068-421b-0b41-9f9b062c7806"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Windows Server", "Windows Server", "WINDOWS SERVER" },
                    { new Guid("c8f5df10-ce36-94f7-f7fd-3e309e739e22"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Marketing de contenidos", "Marketing de contenidos", "MARKETING DE CONTENIDOS" },
                    { new Guid("c92a0216-45f2-f21d-d68c-4b27070b1a97"), true, new Guid("70000000-0000-0000-0000-000000000004"), "Optimización de consultas SQL", "Optimización de consultas SQL", "OPTIMIZACION DE CONSULTAS SQL" },
                    { new Guid("ca7a40b0-de0b-b5c6-7a3e-82ed5d601184"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Autenticación JWT", "Autenticación JWT", "AUTENTICACION JWT" },
                    { new Guid("cbee066c-f49b-f999-5ba9-c0700d22fc54"), true, new Guid("70000000-0000-0000-0000-000000000004"), "SQLite", "SQLite", "SQLITE" },
                    { new Guid("cc1fc55c-46ca-0851-c2e8-620f8d781ebb"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Pruebas automatizadas", "Pruebas automatizadas", "PRUEBAS AUTOMATIZADAS" },
                    { new Guid("cc5f2eb8-5270-f05e-c741-963ead7ca9e0"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Liderazgo", "Liderazgo", "LIDERAZGO" },
                    { new Guid("cca087cd-cf36-b7b8-f755-f8f5fb8403ba"), true, new Guid("70000000-0000-0000-0000-000000000003"), "FastAPI", "FastAPI", "FASTAPI" },
                    { new Guid("ccfe7d5e-fa58-02d0-0666-24c4291688f6"), true, new Guid("70000000-0000-0000-0000-000000000004"), "Firebase", "Firebase", "FIREBASE" },
                    { new Guid("cd6a860f-e6ec-1750-b16b-66bd72eb8a92"), true, new Guid("70000000-0000-0000-0000-000000000003"), "ASP.NET Core", "ASP.NET Core", "ASP.NET CORE" },
                    { new Guid("ce1cb32d-74b8-cc68-d7de-a104ff499c10"), true, new Guid("70000000-0000-0000-0000-000000000011"), "SEM", "SEM", "SEM" },
                    { new Guid("ceb0ba2b-f59f-21ed-c4c0-faa1c2717565"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Edición fotográfica", "Edición fotográfica", "EDICION FOTOGRAFICA" },
                    { new Guid("d0aa2639-fd8d-ca86-7130-fcc77d9864d3"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Motion Graphics", "Motion Graphics", "MOTION GRAPHICS" },
                    { new Guid("d0ea0976-24bb-df9b-9c94-1f409f5d2a90"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Power BI", "Power BI", "POWER BI" },
                    { new Guid("d0eadf93-9ce4-d2f3-bc34-553d04e78448"), true, new Guid("70000000-0000-0000-0000-000000000001"), "Bash", "Bash", "BASH" },
                    { new Guid("d166561d-91b8-3c60-0083-776678dd3cda"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Comunicación escrita", "Comunicación escrita", "COMUNICACION ESCRITA" },
                    { new Guid("d190b65b-3f00-e965-27b4-59c34a2a03cb"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Redacción de contenidos", "Redacción de contenidos", "REDACCION DE CONTENIDOS" },
                    { new Guid("d2f0aa24-cc41-8911-2dcc-115e5da5b421"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Control de calidad de software", "Control de calidad de software", "CONTROL DE CALIDAD DE SOFTWARE" },
                    { new Guid("d306ae34-d0e6-28a3-9f1c-0a8cab3e31d9"), true, new Guid("70000000-0000-0000-0000-000000000014"), "Configuración de dispositivos", "Configuración de dispositivos", "CONFIGURACION DE DISPOSITIVOS" },
                    { new Guid("d34b50ab-0f8f-972b-458f-fa172a5b1806"), true, new Guid("70000000-0000-0000-0000-000000000008"), "Fundamentos de ciberseguridad", "Fundamentos de ciberseguridad", "FUNDAMENTOS DE CIBERSEGURIDAD" },
                    { new Guid("d3c3dc02-85e7-4293-2b58-2b621016a048"), true, new Guid("70000000-0000-0000-0000-000000000001"), "C#", "C#", "C#" },
                    { new Guid("d4d80eda-9ce0-0b74-7a10-bb06ee480aff"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Comunicación efectiva", "Comunicación efectiva", "COMUNICACION EFECTIVA" },
                    { new Guid("d8d39e34-b3e0-ee62-171e-abec1e272ff2"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Testing funcional", "Testing funcional", "TESTING FUNCIONAL" },
                    { new Guid("d9da5055-ac1c-1054-5217-72a4847d3b95"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Notion", "Notion", "NOTION" },
                    { new Guid("da88fba9-8a7c-38ff-58d3-56e835427a0c"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Cinema 4D", "Cinema 4D", "CINEMA 4D" },
                    { new Guid("db97a9a5-35a3-5c59-5d87-8759dfafdd68"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Dapper", "Dapper", "DAPPER" },
                    { new Guid("dbdf032c-8938-4820-c978-b0dbabdba65b"), true, new Guid("70000000-0000-0000-0000-000000000002"), "HTML5", "HTML5", "HTML5" },
                    { new Guid("dca51d98-1993-b1ab-2154-a1d42100bd7d"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Sketch", "Sketch", "SKETCH" },
                    { new Guid("dccc15cc-1def-78eb-959e-716bbaed973d"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Edición de video", "Edición de video", "EDICION DE VIDEO" },
                    { new Guid("ddf46edd-2486-1e3a-6f52-52d9acf6ca32"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Machine Learning", "Machine Learning", "MACHINE LEARNING" },
                    { new Guid("dea87bfb-b95d-bee8-d957-2a6fafc7efae"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Testing de seguridad", "Testing de seguridad", "TESTING DE SEGURIDAD" },
                    { new Guid("decfdde1-8ae7-7edb-1b7e-45b36fe02a54"), true, new Guid("70000000-0000-0000-0000-000000000011"), "WordPress", "WordPress", "WORDPRESS" },
                    { new Guid("df575e6d-dece-b24f-941a-77d79767dbb2"), true, new Guid("70000000-0000-0000-0000-000000000006"), "TensorFlow", "TensorFlow", "TENSORFLOW" },
                    { new Guid("e02a2586-22e7-1715-5980-86e085e6f034"), true, new Guid("70000000-0000-0000-0000-000000000006"), "OpenCV", "OpenCV", "OPENCV" },
                    { new Guid("e06682b3-eb1d-bb3b-0a34-59d5d3c61931"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Nuxt.js", "Nuxt.js", "NUXT.JS" },
                    { new Guid("e0b71fed-5c8c-3cfd-4d8f-86fb2f641984"), true, new Guid("70000000-0000-0000-0000-000000000002"), "Vue.js", "Vue.js", "VUE.JS" },
                    { new Guid("e28f8f26-258c-b964-fa57-a92d42c5e00a"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Infraestructura como código (IaC)", "Infraestructura como código (IaC)", "INFRAESTRUCTURA COMO CODIGO (IAC)" },
                    { new Guid("e2cb055d-da15-2ce1-7d9f-d2b43e00dbb5"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Testing de rendimiento", "Testing de rendimiento", "TESTING DE RENDIMIENTO" },
                    { new Guid("e446efd5-07cf-37dc-fe91-256f67c0b255"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Documentación técnica", "Documentación técnica", "DOCUMENTACION TECNICA" },
                    { new Guid("e4b3561c-13e4-6d65-6dea-43b4ebe7b356"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Entity Framework Core", "Entity Framework Core", "ENTITY FRAMEWORK CORE" },
                    { new Guid("e4dfbd4d-e511-6d63-9040-79177cac933c"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Inteligencia artificial generativa", "Inteligencia artificial generativa", "INTELIGENCIA ARTIFICIAL GENERATIVA" },
                    { new Guid("e6ccbb0b-8dd0-e94d-c9c8-ba0d1aa2341c"), true, new Guid("70000000-0000-0000-0000-000000000005"), "Integración de APIs móviles", "Integración de APIs móviles", "INTEGRACION DE APIS MOVILES" },
                    { new Guid("e84ac6e6-71f6-d5a9-4709-38fe8f494d25"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Gestión de conflictos", "Gestión de conflictos", "GESTION DE CONFLICTOS" },
                    { new Guid("ea3d4376-5e37-621a-7ea7-9a9c0ee9b18a"), true, new Guid("70000000-0000-0000-0000-000000000005"), "Desarrollo de aplicaciones híbridas", "Desarrollo de aplicaciones híbridas", "DESARROLLO DE APLICACIONES HIBRIDAS" },
                    { new Guid("eb0476d4-e31d-4ff5-1891-669510f12bda"), true, new Guid("70000000-0000-0000-0000-000000000003"), "ASP.NET MVC", "ASP.NET MVC", "ASP.NET MVC" },
                    { new Guid("eb13769f-267d-a532-655c-ea2e29cff73e"), true, new Guid("70000000-0000-0000-0000-000000000011"), "Community Management", "Community Management", "COMMUNITY MANAGEMENT" },
                    { new Guid("ecf39307-d23f-c0a2-0786-398336c6fdea"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Microsoft Word", "Microsoft Word", "MICROSOFT WORD" },
                    { new Guid("ed0e6975-9083-811c-57dd-d92699c140f6"), true, new Guid("70000000-0000-0000-0000-000000000007"), "Railway", "Railway", "RAILWAY" },
                    { new Guid("ede3c861-d578-df91-77d9-cd3579bba951"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Google Workspace", "Google Workspace", "GOOGLE WORKSPACE" },
                    { new Guid("ee01f508-d52a-7729-dcd6-6985ccfe05c4"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Testing de APIs", "Testing de APIs", "TESTING DE APIS" },
                    { new Guid("ee55e417-15bf-5fcc-0b3a-dd0f2b10e6d2"), true, new Guid("70000000-0000-0000-0000-000000000002"), "CSS3", "CSS3", "CSS3" },
                    { new Guid("ee6015ae-80eb-6eac-8e61-e04f22fb79d0"), true, new Guid("70000000-0000-0000-0000-000000000005"), "Diseño de interfaces móviles", "Diseño de interfaces móviles", "DISENO DE INTERFACES MOVILES" },
                    { new Guid("eebffeb5-22e4-4700-3734-a5ae1cbce621"), true, new Guid("70000000-0000-0000-0000-000000000013"), "NUnit", "NUnit", "NUNIT" },
                    { new Guid("f0ac30e9-7e6d-add2-c6a8-dc814651406f"), true, new Guid("70000000-0000-0000-0000-000000000009"), "UI Design", "UI Design", "UI DESIGN" },
                    { new Guid("f1706857-ea71-49a0-8694-2336afee550a"), true, new Guid("70000000-0000-0000-0000-000000000003"), "Node.js", "Node.js", "NODE.JS" },
                    { new Guid("f1bc73fb-ca77-5afe-af73-c9b9c64cb2fd"), true, new Guid("70000000-0000-0000-0000-000000000013"), "Postman", "Postman", "POSTMAN" },
                    { new Guid("f24024b7-7d1a-14ff-9a63-5601b2d875e1"), true, new Guid("70000000-0000-0000-0000-000000000005"), "Flutter", "Flutter", "FLUTTER" },
                    { new Guid("f2ddcf69-ef7c-4510-8383-b7516eb64116"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Data Visualization", "Data Visualization", "DATA VISUALIZATION" },
                    { new Guid("f3414325-ad4d-f2f9-eb7c-e46ee2e978e9"), true, new Guid("70000000-0000-0000-0000-000000000011"), "E-commerce", "E-commerce", "E-COMMERCE" },
                    { new Guid("f3cd7300-83e1-3fd3-a9dc-5fa89749771a"), true, new Guid("70000000-0000-0000-0000-000000000003"), "REST API", "REST API", "REST API" },
                    { new Guid("f4096530-1a7f-2ba3-9401-c92323da8323"), true, new Guid("70000000-0000-0000-0000-000000000008"), "VPN", "VPN", "VPN" },
                    { new Guid("f5591340-8549-2627-3249-923f6755be5f"), true, new Guid("70000000-0000-0000-0000-000000000012"), "Microsoft Excel", "Microsoft Excel", "MICROSOFT EXCEL" },
                    { new Guid("f5988c33-11e5-4dad-d981-c7cd09f41b2f"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Figma", "Figma", "FIGMA" },
                    { new Guid("f626e887-d54d-cc81-bd41-d7c9eac5319f"), true, new Guid("70000000-0000-0000-0000-000000000010"), "Renderizado 3D", "Renderizado 3D", "RENDERIZADO 3D" },
                    { new Guid("f998c0cc-c509-d17e-25f3-6b7d9cab23a0"), true, new Guid("70000000-0000-0000-0000-000000000015"), "Atención al detalle", "Atención al detalle", "ATENCION AL DETALLE" },
                    { new Guid("fa07e22d-d978-d6d9-a03a-de8f21a3ee56"), true, new Guid("70000000-0000-0000-0000-000000000006"), "PyTorch", "PyTorch", "PYTORCH" },
                    { new Guid("fa549385-be1d-07b3-6bb4-ace97e2bf505"), true, new Guid("70000000-0000-0000-0000-000000000009"), "Adobe Illustrator", "Adobe Illustrator", "ADOBE ILLUSTRATOR" },
                    { new Guid("fb486024-75f5-a83f-9586-a8c3997c7013"), true, new Guid("70000000-0000-0000-0000-000000000003"), "WebSockets", "WebSockets", "WEBSOCKETS" },
                    { new Guid("fb63fded-929f-fdc3-3b54-1605a684f25a"), true, new Guid("70000000-0000-0000-0000-000000000006"), "Estadística", "Estadística", "ESTADISTICA" },
                    { new Guid("fe01b4b9-abc4-9985-79bd-d73d05a32458"), true, new Guid("70000000-0000-0000-0000-000000000001"), "C++", "C++", "C++" },
                    { new Guid("fe53461c-6469-c7b3-8188-e678d054bb60"), true, new Guid("70000000-0000-0000-0000-000000000004"), "Firebase Firestore", "Firebase Firestore", "FIREBASE FIRESTORE" },
                    { new Guid("ff6bf857-97e9-9fbe-5062-48c66c2ab63b"), true, new Guid("70000000-0000-0000-0000-000000000001"), "Python", "Python", "PYTHON" },
                    { new Guid("ffb04843-10dd-ba55-e835-8df081d7a2f6"), true, new Guid("70000000-0000-0000-0000-000000000004"), "Redis", "Redis", "REDIS" }
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Proyecto_CantidadVacantes",
                schema: "marketplace",
                table: "Proyectos",
                sql: "\"CantidadVacantes\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Proyecto_Presupuesto",
                schema: "marketplace",
                table: "Proyectos",
                sql: "\"Presupuesto\" >= 0 AND (\"PresupuestoMinimo\" IS NULL OR \"PresupuestoMinimo\" > 0) AND (\"PresupuestoMaximo\" IS NULL OR \"PresupuestoMaximo\" > 0)");

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_ClienteId_Estado",
                schema: "marketplace",
                table: "Contratos",
                columns: new[] { "ClienteId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_PostulacionId",
                schema: "marketplace",
                table: "Contratos",
                column: "PostulacionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entregables_ProyectoId_Orden",
                schema: "marketplace",
                table: "Entregables",
                columns: new[] { "ProyectoId", "Orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HitosPago_ProyectoId_Orden",
                schema: "marketplace",
                table: "HitosPago",
                columns: new[] { "ProyectoId", "Orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Postulaciones_ProyectoId_Estado",
                schema: "marketplace",
                table: "Postulaciones",
                columns: new[] { "ProyectoId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Postulaciones_ProyectoId_EstudianteId",
                schema: "marketplace",
                table: "Postulaciones",
                columns: new[] { "ProyectoId", "EstudianteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoSkills_SkillId_TipoRequisito",
                schema: "marketplace",
                table: "ProyectoSkills",
                columns: new[] { "SkillId", "TipoRequisito" });

            migrationBuilder.CreateIndex(
                name: "IX_SkillCategories_Nombre",
                schema: "marketplace",
                table: "SkillCategories",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CategoriaId",
                schema: "marketplace",
                table: "Skills",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_NombreNormalizado",
                schema: "marketplace",
                table: "Skills",
                column: "NombreNormalizado",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contratos",
                schema: "marketplace");

            migrationBuilder.DropTable(
                name: "Entregables",
                schema: "marketplace");

            migrationBuilder.DropTable(
                name: "HitosPago",
                schema: "marketplace");

            migrationBuilder.DropTable(
                name: "ProyectoSkills",
                schema: "marketplace");

            migrationBuilder.DropTable(
                name: "Postulaciones",
                schema: "marketplace");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "marketplace");

            migrationBuilder.DropTable(
                name: "SkillCategories",
                schema: "marketplace");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Proyecto_CantidadVacantes",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Proyecto_Presupuesto",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "Alcance",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "CantidadVacantes",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "CerradoEn",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "DedicacionSemanalEstimada",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "EsquemaPago",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "FechaEntregaEstimada",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "FlexibilidadHorario",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "GastosAdicionales",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "Modalidad",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "Moneda",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "NivelAcademicoSugerido",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "NivelAvance",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "PermitePortafolioPublico",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "PresupuestoMaximo",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "PresupuestoMinimo",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "PropiedadIntelectual",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "PublicadoEn",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "RequiereConfidencialidad",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "RequierePortafolio",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "TipoPresupuesto",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "Ubicacion",
                schema: "marketplace",
                table: "Proyectos");

            migrationBuilder.UpdateData(
                schema: "marketplace",
                table: "Proyectos",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "HabilidadesRequeridas",
                value: new List<string> { "Angular", "Ionic" });

            migrationBuilder.UpdateData(
                schema: "marketplace",
                table: "Proyectos",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "HabilidadesRequeridas",
                value: new List<string> { "HTML", "CSS", "SEO" });
        }
    }
}
