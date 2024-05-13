using System.ComponentModel.DataAnnotations;

namespace VideoManagerApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        public List<Video> Videos { get; set; }
    }
}
