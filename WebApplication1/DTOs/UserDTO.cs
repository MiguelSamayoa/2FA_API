using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserTag { get; set; }
    }
    public class Credenciales
    {
        [Required]
        [EmailAddress]
        public string Usuario { get; set; }
        [Required]
        public string Contraseña { get; set; }
    }

    public class ResultadoHash
    {
        public string Contraseña { get; set; }
        public byte[] Sal { get; set; }
    }
}
