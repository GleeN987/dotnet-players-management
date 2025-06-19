using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayer_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_AddPlayer = @"
            CREATE PROCEDURE [dbo].[AddPlayer]
            (@PlayerID uniqueidentifier, @Nickname nvarchar(20), @Team nvarchar(40), @CountryID uniqueidentifier, @Mouse nvarchar(40), @Mousepad nvarchar(40), @DateOfBirth datetime2(7))
            AS BEGIN
                INSERT INTO [dbo].[Players](PlayerID, Nickname, Team, CountryID, Mouse, Mousepad, DateOfBirth)
                VALUES (@PlayerID, @Nickname, @Team, @CountryID, @Mouse, @Mousepad, @DateOfBirth)
            END";
            migrationBuilder.Sql(sp_AddPlayer); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_AddPlayer = @"
            DROP PROCEDURE [dbo].[AddPlayer]";
            migrationBuilder.Sql(sp_AddPlayer);
        }
    }
}
