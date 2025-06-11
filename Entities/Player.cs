using System;
using System.Collections.Generic;

namespace Entities
{
    public class Player
    {
        public Guid PlayerID { get; set; }
        public string? Nickname { get; set; }
        public string? Team { get; set; }
        public Guid? CountryID { get; set; }
        public string? Mouse { get; set; }
        public string? Mousepad { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
