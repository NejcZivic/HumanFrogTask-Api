using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HumanFrogApi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dogodek",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    naziv = table.Column<string>(type: "text", nullable: false),
                    datum_in_ura = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lokacija = table.Column<string>(type: "text", nullable: false),
                    kapaciteta = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dogodek", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prijava",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dogodek_id = table.Column<int>(type: "integer", nullable: false),
                    ime_in_priimek = table.Column<string>(type: "text", nullable: false),
                    e_posta = table.Column<string>(type: "text", nullable: false),
                    cas_prijave = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prijava", x => x.id);
                    table.ForeignKey(
                        name: "FK_prijava_dogodek_dogodek_id",
                        column: x => x.dogodek_id,
                        principalTable: "dogodek",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "dogodek",
                columns: new[] { "id", "datum_in_ura", "kapaciteta", "lokacija", "naziv" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 12, 15, 19, 0, 0, 0, DateTimeKind.Utc), 100, "Ljubljana", "Koncert" },
                    { 2, new DateTime(2026, 12, 20, 9, 0, 0, 0, DateTimeKind.Utc), 30, "Maribor", "Delavnica" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_prijava_dogodek_id",
                table: "prijava",
                column: "dogodek_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prijava");

            migrationBuilder.DropTable(
                name: "dogodek");
        }
    }
}
