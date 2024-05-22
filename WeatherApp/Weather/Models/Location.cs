using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Weather.ViewModels;

namespace Weather.Models
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? CityName { get; set; }
        public string? CustomName { get; set; }
        public string? Country { get; set; }
    }
}
