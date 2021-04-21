using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Axulus.Data.Model;
using Axulus.Model;
using System.Threading.Tasks;

namespace Axulus.Data.Repositorio
{
    public class UsuarioRepo
    {
        string connectionString = @"Data Source=DESKTOP-AN84SVU\SQLEXPRESS;Initial Catalog=axulus_poc; Integrated Security=True";

        public async Task<List<UsuarioModel>> ListarUsuarioAsync()
        {
            var retorno = new List<UsuarioModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"SELECT [U].[id_usuario]
                                                       ,[U].[id_empresa]
                                                       ,[U].[nome_usuario]
                                                       ,[U].[cpf]
                                                       ,[U].[email]
                                                       ,[U].[sexo]
                                                       ,[U].[data_nas]
                                                       ,[U].[id_departamento]
                                                       ,[U].[id_image]
	                                                   ,[E].[nome_empresa]
	                                                   ,[D].[nome_departamento]
                                                       ,[I].[image_base64]
                                                       ,[I].[descricao]
                                                   FROM [axulus_poc].[dbo].[Usuario] AS [U]
                                                   JOIN[axulus_poc].[dbo].[Empresa] AS [E] 
                                                   ON [U].id_empresa = [E].[id_empresa]
                                                   JOIN[axulus_poc].[dbo].[Departamento] AS [D] 
                                                   ON [U].id_departamento = [D].[id_departamento]
                                                   JOIN[axulus_poc].[dbo].[Image] AS[I]
                                                   ON [U].[id_image] = [I].[id_image]";

                SqlCommand cmd = new SqlCommand(comandoSQL, con);
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
            using SqlConnection con = new SqlConnection(connectionString);
                string comandoSQL = @"Insert into Usuario (
                                        id_empresa,
                                        id_departamento,
                                        nome_usuario, 
                                        cpf, 
                                        email, 
                                        sexo,
                                        data_nas,
                                        id_image,
                                        us_ativo) 
                                    Values(
                                        @idEmpresa,
                                        @idDepartamento,
                                        @nomeUsuario,
                                        @cpf,
                                        @email, 
                                        @sexo, 
                                        @dataNasc,
                                        @idImage,
                                        @usAtivo);
                                    Select SCOPE_IDENTITY();";
            await con.OpenAsync();
                var transaction = con.BeginTransaction();
                try
                {
                    var idImage = await imageRepo.AdicionarImageUsuarioAsync(usuario.image);

                    SqlCommand cmd = new SqlCommand(comandoSQL, con)
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                await con.OpenAsync();
                string comandoSQL = @"UPDATE Usuario 
                                            SET id_empresa = @idEmpresa,
                                            id_departamento = @idDepartamento,
                                            nome_usuario = @nomeUsuario,
                                            cpf = @cpf,
                                            email = @email,
                                            sexo = @sexo,
                                            data_nas = @dataNasc,
                                            id_image = @idImage,
                                            us_ativo = @usAtivo
                                            where id_usuario = @idUsuario
                                      UPDATE Image 
                                      SET descricao = @descricao,
                                          image_base64 = @imageData
                                      WHERE id_image = @idImage;";

                SqlCommand cmd = new SqlCommand(comandoSQL, con);
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
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"DELETE Usuario WHERE id_usuario = @idUsuario;
                                      DELETE Image WHERE id_image = @idImage;";

                SqlCommand cmd = new SqlCommand(comandoSQL, con);
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

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = $@"SELECT [U].[id_usuario]
                                                 ,[U].[id_empresa]
                                                 ,[U].[nome_usuario]
                                                 ,[U].[cpf]
                                                 ,[U].[email]
                                                 ,[U].[sexo]
                                                 ,[U].[data_nas]
                                                 ,[U].[id_departamento]
                                                 ,[U].[id_image]
                                                 ,[U].[us_ativo]
	                                             ,[E].[nome_empresa]
	                                             ,[D].[nome_departamento]
                                                 ,[I].[image_base64]
                                                 ,[I].[descricao]
                                                 ,[I].[id_image]
                                           FROM [axulus_poc].[dbo].[Usuario] AS [U]
                                           JOIN[axulus_poc].[dbo].[Empresa] AS [E] 
                                           ON [U].id_empresa = [E].[id_empresa]
                                           JOIN[axulus_poc].[dbo].[Departamento] AS [D] 
                                           ON [U].id_departamento = [D].[id_departamento]
                                           JOIN[axulus_poc].[dbo].[Image] AS[I]
                                           ON [U].[id_image] = [I].[id_image]
                                           WHERE [U].id_usuario = @idUsuario";

                SqlCommand cmd = new SqlCommand(comandoSQL, con)
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
