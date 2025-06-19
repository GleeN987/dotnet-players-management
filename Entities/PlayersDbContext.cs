using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace Entities
{
    public class PlayersDbContext : DbContext
    {
        public PlayersDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Player> Players { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Player>().ToTable("Players");
            modelBuilder.Entity<Country>().ToTable("Countries");

            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            string playersJson = System.IO.File.ReadAllText("players.json");
            List<Player> players = System.Text.Json.JsonSerializer.Deserialize<List<Player>>(playersJson);
            foreach (Player player in players)
            {
                modelBuilder.Entity<Player>().HasData(player);
            }
        }

        public List<Player> sp_GetAllPlayers()
        {
            return Players.FromSqlRaw("EXECUTE [dbo].[GetAllPlayers]").ToList();
        }

        public int sp_AddPlayer(Player player)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@PlayerID", player.PlayerID),
                new SqlParameter("@Nickname", player.Nickname),
                new SqlParameter("@Team", player.Team),
                new SqlParameter("@CountryID", player.CountryID),
                new SqlParameter("@Mouse", player.Mouse),
                new SqlParameter("@Mousepad", player.Mousepad),
                new SqlParameter("@DateOfBirth", player.DateOfBirth),
            };
            return Database.ExecuteSqlRaw("EXECUTE [dbo].[AddPlayer] @PlayerID, @Nickname, @Team, @CountryID, @Mouse, @Mousepad, @DateOfBirth", parameters);
        }
    }
}
