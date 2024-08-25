using WebApplication1.DTOs;
using WebApplication1.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace WebApplication1.Services
{

    public interface IUserServices
    {
        public Task<User> CreateCliente(Credenciales credenciales);
        public Task<User> Login(Credenciales credenciales);
    }

    public class UserServices : IUserServices
    {
        private readonly JwtContext dbContext;

        public UserServices(JwtContext dBContext)
        {
            this.dbContext = dBContext;
        }


        public async Task<User> CreateCliente(Credenciales credenciales)
        {
            ResultadoHash contraseña = HashPassword(credenciales.Contraseña);

            User user = new User() { UserTag = credenciales.Usuario, Password = contraseña.Contraseña, Salt = contraseña.Sal};

            await dbContext.Usuario.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> Login(Credenciales credenciales)
        {
            User user = await dbContext.Usuario
                .Where<User>(user => user.UserTag == credenciales.Usuario)
                .FirstOrDefaultAsync();

            ResultadoHash contraseñaHash = HashPassword(credenciales.Contraseña, user.Salt);

            if (user.Password != contraseñaHash.Contraseña) throw new Exception("Correo o contraseña incorrecta");
            
            return user;
        }
        
        
        protected ResultadoHash HashPassword(string password)
        {
            // 1. Generar una sal aleatoria
            byte[] salt = GenerateSalt();

            return HashPassword(password, salt);
        }

        protected ResultadoHash HashPassword(string password, byte[] sal)
        {
            var Key = KeyDerivation.Pbkdf2(password, sal, KeyDerivationPrf.HMACSHA1, 10000, 32);

            string hashString = Convert.ToBase64String(Key); // Tamaño del hash en bytes para HMAC-SHA1

            return new ResultadoHash()
            {
                Contraseña = hashString,
                Sal = sal
            };
        }
        protected byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
