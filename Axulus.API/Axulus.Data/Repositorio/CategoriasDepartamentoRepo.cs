using Axulus.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Axulus.Data.Repositorio
{
    public class CategoriasDepartamentoRepo
    {
        string connectionString = @"Data Source=DESKTOP-AN84SVU\SQLEXPRESS;Initial Catalog=axulus_poc; Integrated Security=True";

        public async Task<CategoriasDepartamentoModel> AdicionarCategoriaDepartamentoAsync(CategoriasDepartamentoModel categoriasDepartamento)
        {
            using SqlConnection con = new SqlConnection(connectionString);
            string comandoSQL = @"Insert into CategoriaDepartamento (
                                        id_categoria,
                                        id_departamento
                                        ) 
                                    Values(
                                        @idCategoria,
                                        @idDepartamento
                                        );
                                    Select SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(comandoSQL, con)
            {
                CommandType = CommandType.Text
            };

                cmd.Parameters.AddWithValue("@idCategoria", categoriasDepartamento.idCategoria);
                cmd.Parameters.AddWithValue("@idDepartamento", categoriasDepartamento.idDepartamento);

                await con.OpenAsync();
                var transaction = con.BeginTransaction();
                cmd.Transaction = transaction;
                try
                {
                    var idCategoriaDepartamento = await cmd.ExecuteScalarAsync();

                    transaction.Commit();
                    categoriasDepartamento.idCategoriaDepartamento = Convert.ToInt32(idCategoriaDepartamento);
                    return categoriasDepartamento;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao editar cadastro.", ex);
                }
                finally
                {
                    await con.CloseAsync();
                }
        }
        public async Task<int> ExcluirCategoriaDepartamentoAsync(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"DELETE FROM CategoriaDepartamento 
                                      WHERE id_caterogia_departamento = @idCategoriaDepartamento;";

                SqlCommand cmd = new SqlCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idCategoriaDepartamento", id);

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<CategoriasDepartamentoModel>> ListarCategoriasDepartamentoAsync()
        {
            var retorno = new List<CategoriasDepartamentoModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"SELECT[CD].[id_categoria_departamento]
                                              ,[CD].[id_categoria]
                                              ,[CD].[id_departamento]
                                              ,[D].[nome_departamento]
                                              ,[C].[nome_categoria]
                                      FROM [axulus_poc].[dbo].[CategoriaDepartamento] AS [CD]
                                      JOIN [axulus_poc].[dbo].[Departamento] AS [D]
                                      ON [CD].[id_departamento] = [D].[id_departamento]
                                      JOIN [axulus_poc].[dbo].[Categorias] AS [C]
                                      ON [CD].[id_categoria] = [C].[id_categoria]";

                SqlCommand cmd = new SqlCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var catDep = new CategoriasDepartamentoModel();
                        catDep.idCategoriaDepartamento = Convert.ToInt32(reader["id_categoria_departamento"]);
                        catDep.idCategoria = Convert.ToInt32(reader["id_categoria"]);
                        catDep.idDepartamento = Convert.ToInt32(reader["id_departamento"]);
                        catDep.nomeDepartamento = reader.GetString("nome_departamento");
                        catDep.nomeCategoria = reader.GetString("nome_categoria");
                        retorno.Add(catDep);
                    }
                }
                await con.CloseAsync();
                return retorno;
            }
        }

        public async Task<List<CategoriasDepartamentoModel>> ListarCategoriasPorIdDepartamentoAsync(int id)
        {
            var retorno = new List<CategoriasDepartamentoModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"SELECT[CD].[id_categoria_departamento]
                                              ,[CD].[id_categoria]
                                              ,[CD].[id_departamento]
                                              ,[D].[nome_departamento]
                                              ,[C].[nome_categoria]
                                      FROM [axulus_poc].[dbo].[CategoriaDepartamento] AS [CD]
                                      JOIN [axulus_poc].[dbo].[Departamento] AS [D]
                                      ON [CD].[id_departamento] = [D].[id_departamento]
                                      JOIN [axulus_poc].[dbo].[Categorias] AS [C]
                                      ON [CD].[id_categoria] = [C].[id_categoria]
                                      WHERE[CD].id_departamento = @idDepartamento";

                SqlCommand cmd = new SqlCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idDepartamento", id);

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var catDep = new CategoriasDepartamentoModel();
                        catDep.idCategoriaDepartamento = Convert.ToInt32(reader["id_categoria_departamento"]);
                        catDep.idCategoria = Convert.ToInt32(reader["id_categoria"]);
                        catDep.idDepartamento = Convert.ToInt32(reader["id_departamento"]);
                        catDep.nomeDepartamento = reader.GetString("nome_departamento");
                        catDep.nomeCategoria = reader.GetString("nome_categoria");
                        retorno.Add(catDep);
                    }
                }
                await con.CloseAsync();
                return retorno;
            }
        }

    }
}
