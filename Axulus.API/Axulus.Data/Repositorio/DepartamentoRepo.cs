    using Axulus.Data.Model;
using Axulus.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Axulus.Data.Repositorio
{
    public class DepartamentoRepo
    {
        string connectionString = @"Data Source=DESKTOP-AN84SVU\SQLEXPRESS;Initial Catalog=axulus_poc; Integrated Security=True";

        public async Task<DepartamentoModel> AdicionarDepartamentoAsync(DepartamentoModel departamento)
        {
            var departamentoM = new DepartamentoModel();
            var catDepartamentoRepo = new CategoriasDepartamentoRepo();
            using SqlConnection con = new SqlConnection(connectionString);
            string comandoSQL = @"Insert into Departamento (
                                        id_empresa,
                                        nome_departamento
                                        ) 
                                    Values(
                                        @idEmpresa,
                                        @nomeDepartamento
                                        );
                                    Select SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(comandoSQL, con)
            {
                CommandType = CommandType.Text
            };
            cmd.Parameters.AddWithValue("@idEmpresa", departamento.idEmpresa);
            cmd.Parameters.AddWithValue("@nomeDepartamento", departamento.nomeDepartamento);

            await con.OpenAsync();
            var transaction = con.BeginTransaction();
            try
            {

                cmd.Transaction = transaction;
                var idDepartamento = await cmd.ExecuteScalarAsync();

                //var categorias = departamento.categorias;

                foreach (int categoria in departamento.categorias)
                {
                    var categoriaDepModel = new CategoriasDepartamentoModel() { idCategoria = categoria, idDepartamento = Convert.ToInt32(idDepartamento)};
                    await catDepartamentoRepo.AdicionarCategoriaDepartamentoAsync(categoriaDepModel);
                }

                transaction.Commit();
                departamentoM.idDepartamento = Convert.ToInt32(idDepartamento);

                return departamento;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao realizar cadastro.", ex);
            }
            finally
            {
                await con.CloseAsync();
            }
        }

        public async Task<int> EditarDepartamentoAsync(DepartamentoModel departamento, int id)
        {
            var catDepartamentoRepo = new CategoriasDepartamentoRepo();
            using (var con = new SqlConnection(connectionString))
            {
                await con.OpenAsync();
                string comandoSQL = @"UPDATE Departamento 
                                      SET nome_departamento = @nomeDepartamento,
                                          id_empresa = @idEmpresa
                                      WHERE id_departamento = @idDepartamento;";

                SqlCommand cmd = new SqlCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idDepartamento", id);
                cmd.Parameters.AddWithValue("@idEmpresa", departamento.idEmpresa);
                cmd.Parameters.AddWithValue("@nomeDepartamento", departamento.nomeDepartamento);

                string comandoSQLEditar = @"SELECT[CD].[id_categoria_departamento]
                                              ,[CD].[id_categoria]
                                              ,[CD].[id_departamento]
                                              ,[D].[nome_departamento]
                                              ,[C].[nome_categoria]
                                      FROM [axulus_poc].[dbo].[CategoriaDepartamento] AS [CD]
                                      JOIN [axulus_poc].[dbo].[Departamento] AS [D]
                                      ON [CD].[id_departamento] = [D].[id_departamento]
                                      JOIN [axulus_poc].[dbo].[Categorias] AS [C]
                                      ON [CD].[id_categoria] = [C].[id_categoria]
                                      WHERE[CD].id_departamento = @idDepartamento;";

                SqlCommand cmdEditar = new SqlCommand(comandoSQLEditar, con)
                {
                    CommandType = CommandType.Text
                };

                cmdEditar.Parameters.AddWithValue("@idDepartamento", id);

                // var resultadoQueryDep = await catDepartamentoRepo.ListarCategoriasPorIdDepartamentoAsync(id);

                SqlDataReader readersEdit = cmdEditar.ExecuteReader();

                foreach (int categoria in departamento.categorias)
                {
                    using (var readers = cmdEditar.ExecuteReaderAsync())
                    {
                        while (readersEdit.Read())
                        {
                            var idCategoria = Convert.ToInt32(readersEdit["id_categoria"]);
                            if (idCategoria == categoria)
                            {

                            }
                            else
                            {
                                var categoriaDepModel = new CategoriasDepartamentoModel() { idCategoria = categoria, idDepartamento = Convert.ToInt32(id) };
                                await catDepartamentoRepo.AdicionarCategoriaDepartamentoAsync(categoriaDepModel);
                            }
                        }
                    }
                }
                await con.CloseAsync();
                return await cmdEditar.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> ExcluirDepartamentoAsync(int id)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"DELETE Departamento WHERE id_departamento = @idDepartamento
                                      DELETE CategoriaDepartamento where id_departamento = @idDepartamento";

                SqlCommand cmd = new SqlCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idDepartamento", id);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return id;
            }

        }
        public async Task<List<DepartamentoModel>> ListarDepartamentoAsync()
        {
            var retorno = new List<DepartamentoModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"SELECT[D].[id_departamento]
                                              ,[D].[id_empresa]
                                              ,[D].[nome_departamento]
	                                          ,[E].[nome_empresa]
                                      FROM [axulus_poc].[dbo].[Departamento] AS [D]
                                      JOIN [axulus_poc].[dbo].[Empresa] AS [E]
                                      ON [D].[id_empresa] = [E].[id_empresa]";

                SqlCommand cmd = new SqlCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var departamento = new DepartamentoModel();
                        var empresa = new EmpresaModel();
                        departamento.idDepartamento = Convert.ToInt32(reader["id_departamento"]);
                        departamento.idEmpresa = Convert.ToInt32(reader["id_empresa"]);
                        departamento.nomeDepartamento = reader.GetString("nome_departamento");
                        departamento.nomeEmpresa = reader.GetString("nome_empresa");
                        retorno.Add(departamento);
                    }
                }
                await con.CloseAsync();
                return retorno;
            }
        }

        public async Task<DepartamentoModel> ObterDepartamentoPorIdAsync(int id)
        {
            var departamento = new DepartamentoModel();
            var empresa = new EmpresaModel();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = $@"SELECT[D].[id_departamento]
                                            ,[D].[id_empresa]
                                            ,[D].[nome_departamento]
	                                        ,[E].[nome_empresa]
                                      FROM [axulus_poc].[dbo].[Departamento] AS [D]
                                      JOIN [axulus_poc].[dbo].[Empresa] AS [E]
                                      ON [D].[id_empresa] = [E].[id_empresa] 
                                      WHERE [D].id_departamento = @idDepartamento";

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
                        departamento.idDepartamento = Convert.ToInt32(reader["id_departamento"]);
                        departamento.idEmpresa = Convert.ToInt32(reader["id_empresa"]);
                        departamento.nomeDepartamento = reader.GetString("nome_departamento");
                        departamento.nomeEmpresa = reader.GetString("nome_empresa");
                    }
                }
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return departamento;
            }
        }

        public async Task<List<DepartamentoModel>> ObterDepartamentoEmpresaPorIdAsync(int id)
        {
            var departamento = new DepartamentoModel();
            var empresa = new EmpresaModel();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"SELECT [D].[id_departamento]
                                            ,[D].[id_empresa]
                                            ,[D].[nome_departamento]
	                                        ,[E].[nome_empresa]
                                      FROM [axulus_poc].[dbo].[Departamento] AS [D]
                                      JOIN [axulus_poc].[dbo].[Empresa] AS [E]
                                      ON [D].[id_empresa] = [E].[id_empresa] 
                                      WHERE [E].id_empresa = @idEmpresa";

                SqlCommand cmd = new SqlCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idEmpresa", id);

                await con.OpenAsync();
                var lista = new List<DepartamentoModel>();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        departamento = new DepartamentoModel();
                        departamento.idDepartamento = Convert.ToInt32(reader["id_departamento"]);
                        departamento.idEmpresa = Convert.ToInt32(reader["id_empresa"]);
                        departamento.nomeDepartamento = reader.GetString("nome_departamento");
                        departamento.nomeEmpresa = reader.GetString("nome_empresa");
                        lista.Add(departamento);
                    }
                }
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return lista;
            }
        }
    }
}
