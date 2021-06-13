using Axulus.Data.Model;
using Axulus.Model;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Text;
using System.Threading.Tasks;

namespace Axulus.Data.Repositorio.Sqlite
{
    public class DepartamentoRepo
    {
        string connectionString = @"Data Source=Application.db;Cache=Shared";

        public async Task<DepartamentoModel> AdicionarDepartamentoAsync(DepartamentoModel departamento)
        {
            var departamentoM = new DepartamentoModel();
            var catDepartamentoRepo = new CategoriasDepartamentoRepo();
            using SqliteConnection con = new SqliteConnection(connectionString);
            string comandoSQL = @"insert into departamento (id_empresa, nome_departamento)
                values (
                  @idEmpresa, 
                  @nomeDepartamento
                );
                select last_insert_rowid();";

            SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
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

                transaction.Commit(); //FIXME

                /*
                foreach (int categoria in departamento.categorias)
                {
                    var categoriaDepModel = new CategoriasDepartamentoModel() { idCategoria = categoria, idDepartamento = Convert.ToInt32(idDepartamento)};
                    await catDepartamentoRepo.AdicionarCategoriaDepartamentoAsync(categoriaDepModel);
                }

                departamentoM.idDepartamento = Convert.ToInt32(idDepartamento);
                */

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
            using (var con = new SqliteConnection(connectionString))
            {
                await con.OpenAsync();
                string comandoSQL = @"update departamento
                    set
                    nome_departamento = @nomeDepartamento,
                    id_empresa = @idEmpresa
                    where id_departamento = @idDepartamento";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idDepartamento", id);
                cmd.Parameters.AddWithValue("@idEmpresa", departamento.idEmpresa);
                cmd.Parameters.AddWithValue("@nomeDepartamento", departamento.nomeDepartamento);

                string comandoSQLEditar = @"select CD.id_categoria_departamento, CD.id_categoria,
                    CD.id_departamento, D.nome_departamento, C.nome_categoria
                    from categoria_departamento as CD
                    join departamento as D
                    on CD.id_departamento = D.id_departamento
                    join categorias as C
                    on CD.id_categoria = C.id_categoria
                    where CD.id_departamento = @idDepartamento";

                SqliteCommand cmdEditar = new SqliteCommand(comandoSQLEditar, con)
                {
                    CommandType = CommandType.Text
                };

                cmdEditar.Parameters.AddWithValue("@idDepartamento", id);

                // var resultadoQueryDep = await catDepartamentoRepo.ListarCategoriasPorIdDepartamentoAsync(id);

                SqliteDataReader readersEdit = cmdEditar.ExecuteReader();

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

            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"delete from departamento
                    where id_departamento = @idDepartamento;
                    delete from categoria_departamento
                    where id_departamento = @idDepartamento;";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
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
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"select D.id_departamento, D.id_empresa,
                    D.nome_departamento, E.nome_empresa
                    from departamento as D
                    join empresa as E
                    on D.id_empresa = E.id_empresa";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
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

            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = $@"select D.id_departamento, D.id_empresa,
                    D.nome_departamento, E.nome_empresa
                    from departamento as D
                    join empresa as E
                    on D.id_empresa = E.id_empresa
                    where D.id_departamento = @idDepartamento";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
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

            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"select D.id_departamento, D.id_empresa,
                    D.nome_departamento, E.nome_empresa
                    from departamento as D
                    join empresa as E
                    on D.id_empresa = E.id_empresa
                    where E.id_empresa = @idEmpresa";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
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
