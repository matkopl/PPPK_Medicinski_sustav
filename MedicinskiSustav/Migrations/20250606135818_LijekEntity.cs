using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MedicinskiSustav.Migrations
{
    /// <inheritdoc />
    public partial class LijekEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lijek",
                table: "Recepti");

            migrationBuilder.AddColumn<int>(
                name: "LijekId",
                table: "Recepti",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lijekovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Naziv = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lijekovi", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recepti_LijekId",
                table: "Recepti",
                column: "LijekId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recepti_Lijekovi_LijekId",
                table: "Recepti",
                column: "LijekId",
                principalTable: "Lijekovi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recepti_Lijekovi_LijekId",
                table: "Recepti");

            migrationBuilder.DropTable(
                name: "Lijekovi");

            migrationBuilder.DropIndex(
                name: "IX_Recepti_LijekId",
                table: "Recepti");

            migrationBuilder.DropColumn(
                name: "LijekId",
                table: "Recepti");

            migrationBuilder.AddColumn<string>(
                name: "Lijek",
                table: "Recepti",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
