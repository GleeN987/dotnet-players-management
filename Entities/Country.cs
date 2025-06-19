using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain model for storing country details
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }
        [StringLength(30)]
        public string? CountryName { get; set; }
    }
}
