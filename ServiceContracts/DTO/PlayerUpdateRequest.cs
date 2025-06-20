﻿using Entities;
using ServiceContracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PlayerUpdateRequest
    {
        [Required(ErrorMessage = "Provide ID of a player you want to update")]
        public Guid PlayerID { get; set; }
        [Required(ErrorMessage = "Player nickname is required")]
        public string? Nickname { get; set; }
        public string? Team { get; set; }
        public MouseEnum? Mouse { get; set; }
        public string? Mousepad { get; set; }
        [Required(ErrorMessage = "Player country id is required")]
        public Guid? CountryID { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public Player ToPlayer()
        {
            return new Player()
            {
                PlayerID = PlayerID,
                Nickname = Nickname,
                Team = Team,
                Mouse = Mouse.ToString(),
                Mousepad = Mousepad,
                CountryID = CountryID,
                DateOfBirth = DateOfBirth
            };
        }
    }
}
