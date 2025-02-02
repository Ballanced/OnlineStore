using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the product name.")]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter the product price.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        // Видалимо це поле, щоб уникнути проблем з валідацією
        // public Category Category { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Please enter the stock quantity.")]
        public int Stock { get; set; }
    }
}
