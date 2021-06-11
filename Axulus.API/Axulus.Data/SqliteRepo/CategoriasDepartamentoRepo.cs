using Axulus.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Text;
using System.Threading.Tasks;

namespace Axulus.Data.Repositorio.Sqlite
{
    public class CategoriasDepartamentoRepo
    {
        string connectionString = @"Data Source=Application.db;Cache=Shared";

        public async Task<CategoriasDepartamentoModel> AdicionarCategoriaDepartamentoAsync(CategoriasDepartamentoModel categoriasDepartamento)
        {
            using SqliteConnection con = new SqliteConnection(connectionString);
            string comandoSQL = @"insert into categoria_departamento (id_categoria, id_departamento)
                values (@idCategoria, @idDepartamento select last_insert_rowid()";

            SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
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
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"delete from categoria_departamento
                    where id_categoria_departamento = @idCategoriaDepartamento"; //FIXME

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
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
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"select CD.id_categoria_departamento, CD.id_categoria,
                       CD.id_departamento, D.nome_departamento, C.nome_categoria
                        from categoria_departamento as CD
                        join departamento as D
                        on CD.id_departamento = D.id_departamento
                        join categorias as C
                        on CD.id_categoria = C.id_categoria"; // FIXME

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
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
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"select CD.id_categoria_departamento, CD.id_categoria,
                        CD.id_departamento, D.nome_departamento, C.nome_categoria
                        from categoria_departamento as CD
                        join departamento as D
                        on CD.id_departamento = D.id_departamento
                        join categorias as C
                        on CD.id_categoria = C.id_categoria
                        where CD.id_departamento = @idDepartamento"; //FIXME

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
