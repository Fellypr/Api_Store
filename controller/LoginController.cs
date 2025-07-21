using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using MyStoreApi.services;

namespace MyStoreApi.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class Login : ControllerBase
    {
        private readonly IConfiguration _config;
        public Login(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpPost("LoginForAdministrator")]

        public async Task<IActionResult> LoginForAdmin([FromBody] LoginaAdmin Request)
        {
            try
            {
                var connectionString = _config.GetConnectionString("DefaultConnection");
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM admin
                    WHERE email = @email AND senha = @senha AND chave = @chave;";

                    var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@email",Request.Email);
                    command.Parameters.AddWithValue("@senha",Request.Password);
                    command.Parameters.AddWithValue("@chave",Request.KeyCode);

                    var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        return Ok("Login realizado com sucesso!");
                    }
                    else
                    {
                        return BadRequest("Email ou senha incorretos.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}