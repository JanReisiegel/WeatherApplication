using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Weather.Models
{
    public class SavedLocation
    {
        public int LocationId { get; set; }
        [ForeignKey("LocationId")]
        public Location? Location { get; set; }
        public string UserId { get; set; } = "";
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public string CustomName { get; set; } = "";
    }
}