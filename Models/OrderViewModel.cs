using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class OrderViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        
    }
}
