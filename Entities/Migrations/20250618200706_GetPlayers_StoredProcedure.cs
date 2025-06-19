using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPlayers_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPlayers = @"
                CREATE PROCEDURE [dbo].[GetAllPlayers]
                AS BEGIN 
                    SELECT * FROM [dbo].[Players]
                END";
            migrationBuilder.Sql(sp_GetAllPlayers);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPlayers = @"
                DROP PROCEDURE [dbo].[GetAllPlayers]";
            migrationBuilder.Sql(sp_GetAllPlayers);
        }
    }
}
