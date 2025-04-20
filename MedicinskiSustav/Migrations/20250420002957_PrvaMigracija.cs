using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedicinskiSustav.Migrations
{
    /// <inheritdoc />
    public partial class PrvaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacijenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OIB = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Ime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DatumRodjenja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Spol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacijenti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dokumentacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PacijentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dokumentacije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dokumentacije_Pacijenti_PacijentId",
                        column: x => x.PacijentId,
                        principalTable: "Pacijenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pregledi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SifraPregleda = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    VrijemePregleda = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PacijentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pregledi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pregledi_Pacijenti_PacijentId",
                        column: x => x.PacijentId,
                        principalTable: "Pacijenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bolesti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Naziv = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Pocetak = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Zavrsetak = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DokumentacijaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bolesti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bolesti_Dokumentacije_DokumentacijaId",
                        column: x => x.DokumentacijaId,
                        principalTable: "Dokumentacije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recepti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Lijek = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PregledId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recepti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recepti_Pregledi_PregledId",
                        column: x => x.PregledId,
                        principalTable: "Pregledi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Slike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Putanja = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PregledId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slike_Pregledi_PregledId",
                        column: x => x.PregledId,
                        principalTable: "Pregledi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bolesti_DokumentacijaId",
                table: "Bolesti",
                column: "DokumentacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Dokumentacije_PacijentId",
                table: "Dokumentacije",
                column: "PacijentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacijenti_OIB",
                table: "Pacijenti",
                column: "OIB",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacijenti_Prezime",
                table: "Pacijenti",
                column: "Prezime");

            migrationBuilder.CreateIndex(
                name: "IX_Pregledi_PacijentId",
                table: "Pregledi",
                column: "PacijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Recepti_PregledId",
                table: "Recepti",
                column: "PregledId");

            migrationBuilder.CreateIndex(
                name: "IX_Slike_PregledId",
                table: "Slike",
                column: "PregledId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bolesti");

            migrationBuilder.DropTable(
                name: "Recepti");

            migrationBuilder.DropTable(
                name: "Slike");

            migrationBuilder.DropTable(
                name: "Dokumentacije");

            migrationBuilder.DropTable(
                name: "Pregledi");

            migrationBuilder.DropTable(
                name: "Pacijenti");
        }
    }
}
