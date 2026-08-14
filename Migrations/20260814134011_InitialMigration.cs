using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Padel.Sgbd.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Site",
                columns: table => new
                {
                    IdSite = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HOuverture = table.Column<TimeOnly>(type: "time", nullable: false),
                    HFermeture = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.IdSite);
                });

            migrationBuilder.CreateTable(
                name: "Fermeture",
                columns: table => new
                {
                    IdFermeture = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateFermeture = table.Column<DateOnly>(type: "date", nullable: false),
                    IdSite = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fermeture", x => x.IdFermeture);
                    table.ForeignKey(
                        name: "FK_Fermeture_Site_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Site",
                        principalColumn: "IdSite",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Membre",
                columns: table => new
                {
                    Matricule = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    IdSiteRatt = table.Column<int>(type: "int", nullable: true),
                    DateFinPenalite = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoldeDu = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membre", x => x.Matricule);
                    table.CheckConstraint("CK_Membre_Type", "[Type] IN ('G', 'S', 'L')");
                    table.ForeignKey(
                        name: "FK_Membre_Site_IdSiteRatt",
                        column: x => x.IdSiteRatt,
                        principalTable: "Site",
                        principalColumn: "IdSite");
                });

            migrationBuilder.CreateTable(
                name: "Terrain",
                columns: table => new
                {
                    IdTerrain = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom_Terrain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdSite = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terrain", x => x.IdTerrain);
                    table.ForeignKey(
                        name: "FK_Terrain_Site_IdSite",
                        column: x => x.IdSite,
                        principalTable: "Site",
                        principalColumn: "IdSite",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    IdMatch = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstPrive = table.Column<bool>(type: "bit", nullable: false),
                    IdTerrain = table.Column<int>(type: "int", nullable: false),
                    DateHeure = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarifTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.IdMatch);
                    table.ForeignKey(
                        name: "FK_Match_Terrain_IdTerrain",
                        column: x => x.IdTerrain,
                        principalTable: "Terrain",
                        principalColumn: "IdTerrain",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participation",
                columns: table => new
                {
                    Matricule = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IdMatch = table.Column<int>(type: "int", nullable: false),
                    EstOrganisateur = table.Column<bool>(type: "bit", nullable: false),
                    APaye = table.Column<bool>(type: "bit", nullable: false),
                    DatePaiement = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MontantPaye = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participation", x => new { x.Matricule, x.IdMatch });
                    table.ForeignKey(
                        name: "FK_Participation_Match_IdMatch",
                        column: x => x.IdMatch,
                        principalTable: "Match",
                        principalColumn: "IdMatch",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participation_Membre_Matricule",
                        column: x => x.Matricule,
                        principalTable: "Membre",
                        principalColumn: "Matricule",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fermeture_IdSite",
                table: "Fermeture",
                column: "IdSite");

            migrationBuilder.CreateIndex(
                name: "IX_Match_IdTerrain",
                table: "Match",
                column: "IdTerrain");

            migrationBuilder.CreateIndex(
                name: "IX_Membre_IdSiteRatt",
                table: "Membre",
                column: "IdSiteRatt");

            migrationBuilder.CreateIndex(
                name: "IX_Participation_IdMatch",
                table: "Participation",
                column: "IdMatch");

            migrationBuilder.CreateIndex(
                name: "IX_Terrain_IdSite",
                table: "Terrain",
                column: "IdSite");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fermeture");

            migrationBuilder.DropTable(
                name: "Participation");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Membre");

            migrationBuilder.DropTable(
                name: "Terrain");

            migrationBuilder.DropTable(
                name: "Site");
        }
    }
}
