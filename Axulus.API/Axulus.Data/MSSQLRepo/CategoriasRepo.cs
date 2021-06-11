using Axulus.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Axulus.Data.Repositorio
{
    public class CategoriasRepo
    {
        string connectionString = @"Data Source=DESKTOP-AN84SVU\SQLEXPRESS;Initial Catalog=axulus_poc; Integrated Security=True";

        public async Task<List<CategoriaModel>> ListarCategoriasAsync()
        {
            var retorno = new List<CategoriaModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"SELECT [C].[id_categoria]
                                              ,[C].[nome_categoria]
                                      FROM [axulus_poc].[dbo].[Categorias] AS [C]";
                 
                SqlCommand cmd = new SqlCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var categorias = new CategoriaModel();
                        categorias.idCategoria = Convert.ToInt32(reader["id_categoria"]);
                        categorias.nomeCategoria = reader.GetString("nome_categoria");
                        retorno.Add(categorias);
                    }
                }
                await con.CloseAsync();
                return retorno;
            }
        }

        public async Task<CategoriaModel> ObterCategoriaPorIdAsync(int id)
        {
            var categoria = new CategoriaModel();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = $@"SELECT [C].[id_categoria]
                                              ,[C].[nome_categoria]
                                      FROM [axulus_poc].[dbo].[Categorias] AS [C]
                                      WHERE [D].id_departamento = @idCategoria";

                SqlCommand cmd = new SqlCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idCategoria", id);

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        categoria.idCategoria = Convert.ToInt32(reader["id_categoria"]);
                        categoria.nomeCategoria = reader.GetString("nome_categoria");
                    }
                }
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return categoria;
            }
        }
    }
}
