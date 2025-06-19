using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Player
    {
        [Key]
        public Guid PlayerID { get; set; }
        [StringLength(20)]
        public string? Nickname { get; set; }
        [StringLength(40)]
        public string? Team { get; set; }
        public Guid? CountryID { get; set; }
        [StringLength(40)]
        public string? Mouse { get; set; }
        [StringLength(40)]
        public string? Mousepad { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
