using Axulus.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Text;
using System.Threading.Tasks;

namespace Axulus.Data.Repositorio.Sqlite
{
    public class CategoriasRepo
    {
        string connectionString = @"Data Source=Application.db;Cache=Shared";

        public async Task<List<CategoriaModel>> ListarCategoriasAsync()
        {
            var retorno = new List<CategoriaModel>();
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"select C.id_categoria, C.nome_categoria
                    from categorias as C";
                 
                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
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

            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = $@"select C.id_categoria, C.nome_categoria
                    from categorias as C
                    where D.id_departamento = @idCategoria";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
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
