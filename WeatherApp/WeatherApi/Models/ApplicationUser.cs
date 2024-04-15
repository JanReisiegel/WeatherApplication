﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WeatherApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public bool PaidAccount { get; set; } = false;

        public ICollection<SavedLocation>? SavedLocations { get; set; }
    }
}
