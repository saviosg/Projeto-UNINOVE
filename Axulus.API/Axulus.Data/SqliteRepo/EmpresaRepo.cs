using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Axulus.Data.Model;
using Axulus.Model;

namespace Axulus.Data.Repositorio.Sqlite
{
    public class EmpresaRepo
    {
        string connectionString = @"Data Source=Application.db;Cache=Shared";

        public async Task<EmpresaModel> AdicionarEmpresaAsync(EmpresaModel empresa)
        {
            var imageRepo = new imageRepo();
            using SqliteConnection con = new SqliteConnection(connectionString);
            string comandoSQL = @"insert into empresa (nome_empresa, email, cnpj, data_liberacao, data_cadastro, data_alteracao, id_image)
                values (
                  @nomeEmpresa, 
                  @email, 
                  @cnpj, 
                  @dataLiberacao, 
                  @dataCadastro, 
                  @dataAtualizacao,
                  @idImage
                ); select last_insert_rowid();";

            SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
            {
                CommandType = CommandType.Text
            };
            cmd.Parameters.AddWithValue("@nomeEmpresa", empresa.nomeEmpresa);
            cmd.Parameters.AddWithValue("@email", empresa.email);
            cmd.Parameters.AddWithValue("@cnpj", empresa.cnpj);
            cmd.Parameters.AddWithValue("@dataLiberacao", empresa.dataLiberacao);
            cmd.Parameters.AddWithValue("@dataCadastro", DateTime.Now);
            cmd.Parameters.AddWithValue("@dataAtualizacao", DateTime.Now);

            await con.OpenAsync();
            var transaction = con.BeginTransaction();
            try
            {
                var image = await imageRepo.AdicionarImageAsync(empresa.image, con, transaction);

                cmd.Parameters.AddWithValue("@idImage", image.idImage);
                cmd.Transaction = transaction;

                var idEmpresa = await cmd.ExecuteScalarAsync();

                transaction.Commit();

                empresa.idEmpresa = Convert.ToInt32(idEmpresa);
                return empresa;
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
            return empresa;
        }

        public async Task<int> EditarEmpresaAsync(EmpresaModel empresa, int id)
        {
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                await con.OpenAsync();
                string comandoSQL = @"
                    update empresa
                    set
                        nome_empresa = @nomeEmpresa,
                        email = @email,
                        cnpj = @cnpj,
                        data_liberacao = @dataLiberacao,
                        data_alteracao = @dataAtualizacao
                    where id_empresa = @idEmpresa;
                    update image
                    set
                        descricao = @descricao,
                        image_base64 = @imageData
                    where id_image = (
                        select id_image from empresa
                        where id_empresa = @idEmpresa
                    );";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idEmpresa", id);
                cmd.Parameters.AddWithValue("@nomeEmpresa", empresa.nomeEmpresa);
                cmd.Parameters.AddWithValue("@email", empresa.email);
                cmd.Parameters.AddWithValue("@cnpj", empresa.cnpj);
                cmd.Parameters.AddWithValue("@dataLiberacao", empresa.dataLiberacao);
                cmd.Parameters.AddWithValue("@dataAtualizacao", DateTime.Now);
                cmd.Parameters.AddWithValue("@descricao", empresa.image.descricao);
                cmd.Parameters.AddWithValue("@imageData", empresa.image.imageData);
                
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> ExcluirEmpresaAsync(int id)
        {

            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"
                    delete from image
                        where id_image = (
                            select id_image from empresa
                            where id_empresa = @idEmpresa
                        );
                    delete from empresa
                        where id_empresa = @idEmpresa;";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idEmpresa", id);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return id;
            }

        }
        public async Task<List<EmpresaModel>> ListarEmpresaAsync()
        {
            var retorno = new List<EmpresaModel>();
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"select empresa.id_empresa, nome_empresa, email, cnpj, date(data_liberacao) as data_liberacao,
                        date(empresa.data_cadastro) as data_cadastro, date(empresa.data_alteracao) as data_alteracao,
                        I.id_image, I.image_base64, I.descricao
                        from empresa
                        join image as I
                        on empresa.id_image = i.id_image";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var empresa = new EmpresaModel();
                        var image = new ImageModel();
                        empresa.idEmpresa = Convert.ToInt32(reader["id_empresa"]);
                        empresa.nomeEmpresa = reader.GetString("nome_empresa");
                        empresa.email = reader.GetString("email");
                        empresa.cnpj = reader.GetString("cnpj");
                        empresa.dataLiberacao = Convert.ToDateTime(reader["data_liberacao"]);
                        empresa.dataCadastro = Convert.ToDateTime(reader["data_cadastro"]);
                        empresa.dataAtualizacao = Convert.ToDateTime(reader["data_cadastro"]);
                        image.idImage = Convert.ToInt32(reader["id_image"]);
                        image.descricao = reader.GetString("descricao");
                        image.imageData = reader.GetString("image_base64");
                        empresa.image = image;
                        retorno.Add(empresa);
                    }
                }
                await con.CloseAsync();
                return retorno;
            }
        }

        public async Task<EmpresaModel> ObterEmpresaPorIdAsync(int id)
        {
            var empresa = new EmpresaModel();
            var image = new ImageModel();
            var departamento = new DepartamentoModel();
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = $@"
                    select E.id_empresa, E.nome_empresa,
                       E.email, E.cnpj, E.data_liberacao, E.data_cadastro,
                       E.data_alteracao, I.id_image, I.image_base64, I.descricao
                    from empresa as E
                    join image as I
                        on E.id_image = I.id_image
                    where E.id_empresa = @idEmpresa";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idEmpresa", id);

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        empresa.idEmpresa = Convert.ToInt32(reader["id_empresa"]);
                        empresa.nomeEmpresa = reader.GetString("nome_empresa");
                        empresa.email = reader.GetString("email");
                        empresa.cnpj = reader.GetString("cnpj");
                        empresa.dataLiberacao = Convert.ToDateTime(reader["data_liberacao"]);
                        empresa.dataCadastro = Convert.ToDateTime(reader["data_cadastro"]);
                        empresa.dataAtualizacao = Convert.ToDateTime(reader["data_cadastro"]);
                        image.idImage = Convert.ToInt32(reader["id_image"]);
                        image.descricao = reader.GetString("descricao");
                        image.imageData = reader.GetString("image_base64");
                        empresa.image = image;
                        //departamento.idDepartamento = Convert.ToInt32(reader["id_departamento"]);
                        //departamento.nomeDepartamento = reader.GetString("nome_departamento");
                    }
                }
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return empresa;
            }
        }
    }
}
