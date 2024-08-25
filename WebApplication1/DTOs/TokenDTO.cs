using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public DateTime Creacion { get; set; }
        public UserDTO Usuario { get; set; }
    }
}
