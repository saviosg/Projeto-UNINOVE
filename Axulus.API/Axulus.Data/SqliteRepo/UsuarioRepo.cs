using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using Axulus.Data.Model;
using Axulus.Model;
using System.Threading.Tasks;

namespace Axulus.Data.Repositorio.Sqlite
{
    public class UsuarioRepo
    {
        string connectionString = @"Data Source=Application.db;Cache=Shared";

        public async Task<List<UsuarioModel>> ListarUsuarioAsync()
        {
            var retorno = new List<UsuarioModel>();
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"select U.id_usuario, U.id_empresa, U.nome_usuario, U.cpf,
                        U.email, U.sexo, U.data_nas, U.id_departamento, U.id_image, E.nome_empresa,
                        D.nome_departamento, I.image_base64, I.descricao
                        from usuario as U
                          join empresa as E
                            on U.id_empresa = E.id_empresa
                          join departamento as D
                            on U.id_departamento = D.id_departamento
                          join image as I
                            on U.id_image = I.id_image";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con);
                cmd.CommandType = CommandType.Text;

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var usuario = new UsuarioModel();
                        var empresa = new EmpresaModel();
                        var departamento = new DepartamentoModel();
                        var image = new ImageModel();
                        usuario.idUsuario = Convert.ToInt32(reader["id_usuario"]);
                        usuario.idEmpresa = Convert.ToInt32(reader["id_empresa"]);
                        usuario.idDepartamento = Convert.ToInt32(reader["id_departamento"]);
                        usuario.idImagem = Convert.ToInt32(reader["id_image"]);
                        usuario.nome_usuario = reader.GetString("nome_usuario");
                        usuario.cpf = reader.GetString("cpf");
                        usuario.email = reader.GetString("email");
                        usuario.sexo = reader.GetString("sexo");
                        usuario.dataNasc = Convert.ToDateTime(reader["data_nas"]);
                        usuario.nomeEmpresa = reader.GetString("nome_empresa");
                        usuario.nomeDepartamento = reader.GetString("nome_departamento");
                        image.descricao = reader.GetString("descricao");
                        image.imageData = reader.GetString("image_base64");
                        usuario.image = image;
                        retorno.Add(usuario);

                    }
                }
                await con.CloseAsync();
                return retorno;
            }
        }
        public async Task<UsuarioModel> AdicionarUsuarioAsync(UsuarioModel usuario)
        {
            var imageRepo = new imageRepo();
            var catDepartamentoRepo = new CategoriasDepartamentoRepo();
            using SqliteConnection con = new SqliteConnection(connectionString);
                string comandoSQL = @"insert into usuario (id_empresa, id_departamento, nome_usuario, cpf, email, sexo, data_nas, id_image, us_ativo)
                    values (
                      @idEmpresa, 
                      @idDepartamento, 
                      @nomeUsuario, 
                      @cpf, 
                      @email, 
                      @sexo, 
                      @dataNasc, 
                      @idImage, 
                      @usAtivo
                    );
                    select last_insert_rowid();";
            await con.OpenAsync();
                var transaction = con.BeginTransaction();
                try
                {
                    var idImage = await imageRepo.AdicionarImageUsuarioAsync(usuario.image);

                    SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@idEmpresa", usuario.idEmpresa);
                    cmd.Parameters.AddWithValue("@idDepartamento", usuario.idDepartamento);
                    cmd.Parameters.AddWithValue("@nomeUsuario", usuario.nome_usuario);
                    cmd.Parameters.AddWithValue("@cpf", usuario.cpf);
                    cmd.Parameters.AddWithValue("@email", usuario.email);
                    cmd.Parameters.AddWithValue("@sexo", usuario.sexo);
                    cmd.Parameters.AddWithValue("@dataNasc", usuario.dataNasc);
                    cmd.Parameters.AddWithValue("@usAtivo", Convert.ToBoolean(usuario.usuarioAtivo));
                    cmd.Parameters.AddWithValue("@idImage", idImage);

                    cmd.Transaction = transaction;
                    var idUsuario = await cmd.ExecuteScalarAsync();
                    //var usuarioAtivo = await cmd.ExecuteScalarAsync();

                    transaction.Commit();
                    usuario.idUsuario = Convert.ToInt32(idUsuario);
                    //usuario.usuarioAtivo = Convert.ToBoolean(usuarioAtivo);
                    usuario.idImagem = Convert.ToInt32(idImage);
                    return usuario;
                }
                catch (Exception ex)
                {
                transaction.Rollback();    
                throw;
                }
                finally
                {
                    await con.CloseAsync();
                }
            return usuario;
        }

        public async Task<int> EditarUsuarioAsync(UsuarioModel usuario, int id)
        {
            var image = new ImageModel();
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                await con.OpenAsync();
                string comandoSQL = @"update usuario
                    set
                      id_empresa = @idEmpresa,
                      id_departamento = @idDepartamento,
                      nome_usuario = @nomeUsuario,
                      cpf = @cpf,
                      email = @email,
                      sexo = @sexo,
                      data_nas = @dataNasc,
                      id_image = @idImage,
                      us_ativo = @usAtivo
                    where id_usuario = @idUsuario;
                    update image
                    set
                      descricao = @descricao,
                      image_base64 = @imageData
                    where id_image = @idImage;";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@idUsuario", id);
                cmd.Parameters.AddWithValue("@idEmpresa", usuario.idEmpresa);
                cmd.Parameters.AddWithValue("@idDepartamento", usuario.idDepartamento);
                cmd.Parameters.AddWithValue("@nomeUsuario", usuario.nome_usuario);
                cmd.Parameters.AddWithValue("@cpf", usuario.cpf);
                cmd.Parameters.AddWithValue("@email", usuario.email);
                cmd.Parameters.AddWithValue("@sexo", usuario.sexo);
                cmd.Parameters.AddWithValue("@dataNasc", usuario.dataNasc);
                cmd.Parameters.AddWithValue("@idImage", usuario.idImagem);
                cmd.Parameters.AddWithValue("@usAtivo", Convert.ToBoolean(usuario.usuarioAtivo));
                cmd.Parameters.AddWithValue("@descricao", usuario.image.descricao);
                cmd.Parameters.AddWithValue("@imageData", usuario.image.imageData);


                return await cmd.ExecuteNonQueryAsync();
            }

        }

        public async Task<int> ExcluirUsuarioAsync(int id)
        {
            var usuario = new UsuarioModel();
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"delete from usuario
                    where id_usuario = @idUsuario;
                    delete from image
                    where id_image = @idImage;";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@idUsuario", id);
                cmd.Parameters.AddWithValue("@idImage", usuario.idImagem);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync ();
                await con.CloseAsync();
                return id;
            }

        }

        public async Task<UsuarioModel> ObterUsuarioPorIdAsync(int id)
        {
            var usuario = new UsuarioModel();
            var image = new ImageModel();

            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = $@"select U.id_usuario, U.id_empresa, U.nome_usuario, U.cpf,
                    U.email, U.sexo, U.data_nas, U.id_departamento, U.id_image, U.us_ativo,
                    E.nome_empresa, D.nome_departamento, I.image_base64, I.descricao, I.id_image
                    from usuario as U
                      join empresa as E
                        on U.id_empresa = E.id_empresa
                      join departamento as D
                        on U.id_departamento = D.id_departamento
                      join image as I
                        on U.id_image = I.id_image
                    where U.id_usuario = @idUsuario";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@idUsuario", id);

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        usuario.idUsuario = Convert.ToInt32(reader["id_usuario"]);
                        usuario.idEmpresa = Convert.ToInt32(reader["id_empresa"]);
                        usuario.idDepartamento = Convert.ToInt32(reader["id_departamento"]);
                        usuario.idImagem = Convert.ToInt32(reader["id_image"]);
                        usuario.nome_usuario = reader.GetString("nome_usuario");
                        usuario.cpf = reader.GetString("cpf");
                        usuario.email = reader.GetString("email");
                        usuario.sexo = reader.GetString("sexo");
                        usuario.dataNasc = Convert.ToDateTime(reader["data_nas"]);
                        usuario.nomeEmpresa = reader.GetString("nome_empresa");
                        usuario.nomeDepartamento = reader.GetString("nome_departamento");
                        usuario.usuarioAtivo = Convert.ToInt32(reader["us_ativo"]);
                        image.idImage = Convert.ToInt32(reader["id_image"]);
                        image.descricao = reader.GetString("descricao");
                        image.imageData = reader.GetString("image_base64");
                        usuario.image = image;
                    }
                }
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return usuario;
            }
        }
    }
}
