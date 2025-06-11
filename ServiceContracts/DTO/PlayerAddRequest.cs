using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PlayerAddRequest
    {
        [Required(ErrorMessage ="Player nickname is required")]
        public string? Nickname { get; set; }
        public string? Team { get; set; }
        public MouseEnum? Mouse { get; set; }
        public string? Mousepad { get; set; }
        [Required(ErrorMessage ="Player country id is required")]
        public Guid? CountryID { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public Player ToPlayer()
        {
            return new Player()
            { 
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

