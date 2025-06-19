    using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Team = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Mouse = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Mousepad = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerID);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryID", "CountryName" },
                values: new object[,]
                {
                    { new Guid("24ab38a0-bbfd-4e79-bc63-d62e405390ef"), "United States" },
                    { new Guid("5a893d04-594e-49b2-b7d6-54e0e9f56321"), "Brazil" },
                    { new Guid("72351c57-3321-4f12-802b-64de52a56449"), "South Korea" },
                    { new Guid("8b72c61d-9e4d-4d6b-85f9-e89a3a3ea8b1"), "Germany" },
                    { new Guid("e1c0f7b0-9a4b-4d7f-931c-108adf4c88d2"), "Sweden" }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerID", "CountryID", "DateOfBirth", "Mouse", "Mousepad", "Nickname", "Team" },
                values: new object[,]
                {
                    { new Guid("2c5d5973-d9e0-4699-a232-d0c7b23a0d7e"), new Guid("5a893d04-594e-49b2-b7d6-54e0e9f56321"), new DateTime(2002, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Razer", "Viper Control", "Voltage", "Electric Force" },
                    { new Guid("30b3b7d2-883b-4380-8bfe-7c9b326dd8ec"), new Guid("24ab38a0-bbfd-4e79-bc63-d62e405390ef"), new DateTime(1995, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Razer", "Razer Goliathus", "SharpShooter", "Snipers United" },
                    { new Guid("4f181349-5e43-468b-8732-91a0f3287fc4"), new Guid("72351c57-3321-4f12-802b-64de52a56449"), new DateTime(1999, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zowie", "BenQ G-SR", "ZKiller", "Zenith" },
                    { new Guid("5ae6de2a-2f32-4041-a49c-9dbfb31255ee"), new Guid("e1c0f7b0-9a4b-4d7f-931c-108adf4c88d2"), new DateTime(1997, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zowie", "Zowie GTF-X", "IceMan", "Polar Squad" },
                    { new Guid("8797de4c-c189-4c23-962b-4a3fdfec5b1a"), new Guid("5a893d04-594e-49b2-b7d6-54e0e9f56321"), new DateTime(2001, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pulsar", "Artisan FX", "Pulse", "Brazilian Storm" },
                    { new Guid("8a0b7e6b-d6d5-47b8-bd47-6a1a63bb81f6"), new Guid("72351c57-3321-4f12-802b-64de52a56449"), new DateTime(2003, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pulsar", "SkyPad 3.0", "Rain", "Cloud Nine" },
                    { new Guid("b687f5e1-7531-48d7-a5f2-18a496891ed7"), new Guid("8b72c61d-9e4d-4d6b-85f9-e89a3a3ea8b1"), new DateTime(1996, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Other", "Logitech G640", "Ghost", "Shadow Tactics" },
                    { new Guid("df26bb1a-69ed-4f2c-81c2-b342ff3d7cb5"), new Guid("8b72c61d-9e4d-4d6b-85f9-e89a3a3ea8b1"), new DateTime(1994, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Other", "Cooler Master MP750", "Vortex", "Twisters" },
                    { new Guid("e3fc8e69-b6cc-4f4d-89a4-cb79ac9a2e2e"), new Guid("e1c0f7b0-9a4b-4d7f-931c-108adf4c88d2"), new DateTime(1998, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Logitech", "SteelSeries QcK", "Fragger", "Team Alpha" },
                    { new Guid("f1c29017-8f8b-4663-9e1e-93d85796eaa6"), new Guid("24ab38a0-bbfd-4e79-bc63-d62e405390ef"), new DateTime(2000, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Logitech", "Glorious Helios", "ClutchKing", "Team Bravo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
