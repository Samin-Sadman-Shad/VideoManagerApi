using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoManagerApi.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string VideoName { get; set; } 
        
        public string? Description { get; set; } 
        [Required]
        public IFormFile VideoFile { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
