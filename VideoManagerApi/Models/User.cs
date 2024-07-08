using System.ComponentModel.DataAnnotations;

namespace VideoManagerApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        
        public string Password { get; set; } = null!;


    }
}
