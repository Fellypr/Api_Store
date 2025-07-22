using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyStoreApi.services;
using Npgsql;
namespace MyStoreApi.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdicionarProduto : ControllerBase
    {
        private readonly IConfiguration _config;
        public AdicionarProduto(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpPost("Products")]

        public async Task<IActionResult> Adicionar([FromBody] Produto model)
        {
            try
            {
                var connectionString = _config.GetConnectionString("DefaultConnection");
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"INSERT INTO mystore(produto,categoria_produto,descricao_produto,preco,imagem,Link_da_venda,imagem_primaria,imagem_segundaria,imagem_terciaria)
                    VALUES (@produto,@categoria_produto,@descricao_produto,@preco,@imagem,@Link_da_venda,@imagem_primaria,@imagem_segundaria,@imagem_terciaria);";

                    var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@produto", model.NameProduct);
                    command.Parameters.AddWithValue("@categoria_produto", model.Category);
                    command.Parameters.AddWithValue("@descricao_produto", model.Description);
                    command.Parameters.AddWithValue("@preco", model.Price);
                    command.Parameters.AddWithValue("@imagem", model.Picture);
                    command.Parameters.AddWithValue("@Link_da_venda", model.LinkVenda);
                    command.Parameters.AddWithValue("@imagem_primaria", model.PictureFirst);
                    command.Parameters.AddWithValue("@imagem_segundaria", model.PictureSecond);
                    command.Parameters.AddWithValue("@imagem_terciaria", model.PictureThird);

                    await command.ExecuteNonQueryAsync();
                }
                return Ok("Produto adicionado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarProdutos")]
        public async Task<IActionResult> BuscarProdutos()
        {
            try
            {
                var connectionString = _config.GetConnectionString("DefaultConnection");
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM mystore;";

                    var command = new NpgsqlCommand(query, connection);
                    var reader = await command.ExecuteReaderAsync();

                    var produtos = new List<Produto>();

                    while (await reader.ReadAsync())
                    {
                        var produto = new Produto
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            NameProduct = reader["produto"].ToString(),
                            Category = reader["categoria_produto"].ToString(),
                            Description = reader["descricao_produto"].ToString(),
                            Price = Convert.ToDecimal(reader["preco"]),
                            Picture = reader["imagem"].ToString(),
                            PictureFirst = reader["imagem_primaria"].ToString(),
                            PictureSecond = reader["imagem_segundaria"].ToString(),
                            PictureThird = reader["imagem_terciaria"].ToString(),
                            LinkVenda = reader["Link_da_venda"].ToString()
                        };
                        produtos.Add(produto);
                    }
                    return Ok(produtos);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPut("AtualizarProduto/{id}")]
        public async Task<IActionResult> Atualizar([FromBody] Produto Product)
        {
            try
            {
                var connectionString = _config.GetConnectionString("DefaultConnection");
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"UPDATE mystore
                    SET produto = @produto, categoria_produto = @categoria_produto, descricao_produto = @descricao_produto, preco = @preco, imagem = @imagem, link_da_venda = @Link_da_venda, imagem_primaria = @primaria, imagem_segundaria = @segundaria, imagem_terciaria = @terciaria
                    WHERE id = @id;";

                    var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", Product.Id);
                    command.Parameters.AddWithValue("@produto", Product.NameProduct);
                    command.Parameters.AddWithValue("@categoria_produto", Product.Category);
                    command.Parameters.AddWithValue("@descricao_produto", Product.Description);
                    command.Parameters.AddWithValue("@preco", Product.Price);
                    command.Parameters.AddWithValue("@imagem", Product.Picture);
                    command.Parameters.AddWithValue("@Link_da_venda", Product.LinkVenda);
                    command.Parameters.AddWithValue("@primaria", Product.PictureFirst);
                    command.Parameters.AddWithValue("@segundaria", Product.PictureSecond);
                    command.Parameters.AddWithValue("@terciaria", Product.PictureThird);

                    await command.ExecuteNonQueryAsync();
                }
                return Ok("Produto atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeletarProduto/{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                var connectionString = _config.GetConnectionString("DefaultConnection");
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"DELETE FROM mystore
                    WHERE id = @id;";

                    var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
                return Ok("Produto deletado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("DetalheDoProduto/{id}")]
        public async Task<IActionResult> DetalheDoProduto(int id)
        {
            try
            {

                var connectionString = _config.GetConnectionString("DefaultConnection");
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM mystore
                    WHERE id = @id;";

                    var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        var produto = new Produto
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            NameProduct = reader["produto"].ToString(),
                            Category = reader["categoria_produto"].ToString(),
                            Description = reader["descricao_produto"].ToString(),
                            Price = Convert.ToDecimal(reader["preco"]),
                            Picture = reader["imagem"].ToString(),
                            PictureFirst = reader["imagem_primaria"].ToString(),
                            PictureSecond = reader["imagem_segundaria"].ToString(),
                            PictureThird = reader["imagem_terciaria"].ToString(),
                            LinkVenda = reader["Link_da_venda"].ToString()
                        };
                        return Ok(produto);
                    }
                    return NotFound("Produto não encontrado!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("BuscarProdutosPelaQuery")]
        public async Task<IActionResult> BuscarProdutosPorCategoria([FromQuery] string? produto)
        {
            try
            {
                var connectionString = _config.GetConnectionString("DefaultConnection");
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM mystore
                          WHERE (@produto IS NULL OR LOWER(produto) LIKE LOWER(@produto));";

                    var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@produto", (object?)($"%{produto}%") ?? DBNull.Value);

                    var reader = await command.ExecuteReaderAsync();
                    var produtos = new List<Produto>();

                    while (await reader.ReadAsync())
                    {
                        var p = new Produto
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            NameProduct = reader["produto"].ToString(),
                            Category = reader["categoria_produto"].ToString(),
                            Description = reader["descricao_produto"].ToString(),
                            Price = Convert.ToDecimal(reader["preco"]),
                            Picture = reader["imagem"].ToString(),
                            PictureFirst = reader["imagem_primaria"].ToString(),
                            PictureSecond = reader["imagem_segundaria"].ToString(),
                            PictureThird = reader["imagem_terciaria"].ToString(),
                            LinkVenda = reader["Link_da_venda"].ToString()
                        };
                        produtos.Add(p);
                    }

                    return Ok(produtos);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Buscar")]
        public async Task<IActionResult> Buscar([FromBody] ProdutoRequest bus)
        {
            try
            {
                var connectionString = _config.GetConnectionString("DefaultConnection");
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT * FROM mystore
                          WHERE produto LIKE @produto;";

                    var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@produto", "%" + bus.Product + "%");

                    var reader = await command.ExecuteReaderAsync();
                    var produtos = new List<Produto>();

                    while (await reader.ReadAsync())
                    {
                        var p = new Produto
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            NameProduct = reader["produto"].ToString(),
                            Category = reader["categoria_produto"].ToString(),
                            Description = reader["descricao_produto"].ToString(),
                            Price = Convert.ToDecimal(reader["preco"]),
                            Picture = reader["imagem"].ToString(),

                            PictureFirst = reader["imagem_primaria"].ToString(),
                            PictureSecond = reader["imagem_segundaria"].ToString(),
                            PictureThird = reader["imagem_terciaria"].ToString(),
                            LinkVenda = reader["Link_da_venda"].ToString()
                        };
                        produtos.Add(p);
                    }

                    return Ok(produtos);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}