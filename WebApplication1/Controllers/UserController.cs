using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;
using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly IUserServices userServices;
        private readonly ISessionTokenServices tokenServices;
        private readonly IEmailService emailService;
        private ILogger logger;

        public UserController(IConfiguration configuration, IMapper mapper, IUserServices userServices, ISessionTokenServices tokenServices, ILogger<UserController> logger, IEmailService emailService)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.userServices = userServices;
            this.tokenServices = tokenServices;
            this.logger = logger;
            this.emailService = emailService;
        }

        [HttpPost("/CrearUsuario")]
        public async Task<ActionResult<TokenDTO>> CreateUser(Credenciales credenciales)
        {
            
            User user =  await userServices.CreateCliente(credenciales);

            if (user.Id == 0) return BadRequest("No se pudo ingresar el usuario");

            //// MANDAR CORREO

            return Ok(user);
        }


        [HttpGet("ConfirmarRegistro")]
        public async Task<ActionResult<TokenDTO>> ConfirmarRegistro([FromQuery] string credenciales)
        {
            

            User usuario = await tokenServices.ViewToken( credenciales );

            if (usuario == null) return BadRequest();

            SessionToken token = BuildJwt(usuario);


            TokenDTO tokenDTO = new TokenDTO()
            {
                Token = token.Token,
                Creacion = token.Creacion,
                Usuario = new UserDTO()
                {
                    UserTag = token.User.UserTag,
                    Id = token.User.Id,
                }
            };

            return Ok(tokenDTO);
        }

        [HttpPost("/Login")]
        public async Task<ActionResult<TokenDTO>> Login(Credenciales credenciales)
        {

            User user = await userServices.Login(credenciales);

            if (user == null) return NotFound();



            SessionToken token = await tokenServices.Savetoken(BuildToken(user));

            if (token == null) return BadRequest();

            string htmlBody = @"
                <!DOCTYPE html>
                <html lang='es'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Correo de Ejemplo</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            margin: 0;
                            padding: 0;
                            background-color: #f4f4f4;
                        }
                        .container {
                            width: 100%;
                            max-width: 600px;
                            margin: 20px auto;
                            background-color: #ffffff;
                            padding: 20px;
                            border-radius: 5px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }
                        .header {
                            text-align: center;
                            padding: 10px 0;
                            background-color: #007bff;
                            color: white;
                            border-radius: 5px 5px 0 0;
                        }
                        .header h1 {
                            margin: 0;
                            font-size: 24px;
                        }
                        .content {
                            padding: 20px;
                            line-height: 1.6;
                            color: #333;
                        }
                        .content p {
                            margin: 10px 0;
                        }
                        .button-container {
                            text-align: center;
                            margin-top: 20px;
                        }
                        .button {
                            background-color: #28a745;
                            color: white;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 5px;
                            font-size: 16px;
                        }
                        .button:hover {
                            background-color: #218838;
                        }
                        .footer {
                            text-align: center;
                            padding: 10px;
                            font-size: 12px;
                            color: #777;
                            margin-top: 20px;
                            border-top: 1px solid #dddddd;
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Bienvenido a Nuestro Servicio</h1>
                        </div>
                        <div class='content'>
                            <p>Hola [Nombre],</p>
                            <p>Gracias por registrarte en nuestro servicio. Estamos emocionados de tenerte con nosotros.</p>
                            <p>Por favor, confirma tu dirección de correo electrónico haciendo clic en el botón de abajo:</p>
                            <div class='button-container'>
                                <a href='https://localhost:7068/User/ConfirmarRegistro?credenciales=" + token.Token + @"' class='button'>Confirmar Correo</a>
                            </div>
                            <p>Si no solicitaste este registro, puedes ignorar este mensaje.</p>
                            <p>¡Gracias y que tengas un excelente día!</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024 - Tu Empresa. Todos los derechos reservados.</p>
                        </div>
                    </div>
                </body>
                </html>";

            await this.emailService.SendEmailAsync(user.UserTag, "Confirmacion de registro", htmlBody);

            return Ok();
        }

        private SessionToken BuildToken(User user)
        {
            SessionToken sessionToken = new();

            sessionToken.Creacion = DateTime.Now;

            // --- --- Se generan los Claims que se almacenaran en el JWT
            List<Claim> claims = new()
            {
                new Claim("IdUser", user.Id.ToString() ),
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["LlaveConfirmacion"]));

            var cred = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);


            var SecurityToken = new JwtSecurityToken(issuer: null,
                                                    audience: null,
                                                    claims: claims,
                                                    expires: DateTime.Now.AddHours(1),
                                                    signingCredentials: cred);


            sessionToken.Token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            sessionToken.User = user;

            return sessionToken;
        }


        private SessionToken BuildJwt(User user)
        {
            // --- Se crea el SessionToken, que contiene el JWT, fecha de expiracion, fecha de creacion y el empleado a quien pertenece 
            SessionToken sessionToken = new();

            sessionToken.Creacion = DateTime.Now;

            // --- --- Se generan los Claims que se almacenaran en el JWT
            List<Claim> claims = new()
            {
                new Claim("IdUser", user.Id.ToString() ),
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["LlaveJwt"]));

            var cred = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);


            var SecurityToken = new JwtSecurityToken(issuer: null,
                                                    audience: null,
                                                    claims: claims,
                                                    expires: DateTime.Now.AddHours(1),
                                                    signingCredentials: cred);


            sessionToken.Token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            sessionToken.User = user;

            return sessionToken;
        }

    }
}
