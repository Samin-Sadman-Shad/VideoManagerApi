using System.ComponentModel.DataAnnotations;

namespace VideoManagerApi.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        public List<Video> videos { get; set; }
    }
}
