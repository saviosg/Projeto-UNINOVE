using Axulus.Data.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Axulus.Data.Repositorio
{
    public class imageRepo
    {
        #region Propriedades
        string connectionString = @"Data Source=DESKTOP-AN84SVU\SQLEXPRESS;Initial Catalog=axulus_poc; Integrated Security=True";
        #endregion

        #region Metodos Publicos
        public async Task<ImageModel> AdicionarImageAsync(ImageModel image, int idEmpresa)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"Insert into Image (
                                        id_empresa,
                                        descricao, 
                                        image_base64, 
                                        data_cadastro, 
                                        data_alteracao)
                                    Values(
                                        @idEmpresa,
                                        @descricao, 
                                        @imageBase64,
                                        @dataCadastro, 
                                        @dataAtualizacao)";

                SqlCommand cmd = new SqlCommand(comandoSQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@idEmpresa", idEmpresa);
                cmd.Parameters.AddWithValue("@descricao", image.descricao);
                cmd.Parameters.AddWithValue("@imageBase64", image.imageData);
                cmd.Parameters.AddWithValue("@dataCadastro", DateTime.Now);
                cmd.Parameters.AddWithValue("@dataAtualizacao", DateTime.Now);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
                return image;
            }
            #endregion
        }

        public async Task<int> AdicionarImageUsuarioAsync(ImageModel image)
        {
            var usuario = new UsuarioModel();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"Insert into Image (
                                        descricao, 
                                        image_base64, 
                                        data_cadastro, 
                                        data_alteracao)
                                    Values(
                                        @descricao, 
                                        @imageBase64,
                                        @dataCadastro, 
                                        @dataAtualizacao);
                                    Select SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(comandoSQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@descricao", image.descricao);
                cmd.Parameters.AddWithValue("@imageBase64", image.imageData);
                cmd.Parameters.AddWithValue("@dataCadastro", DateTime.Now);
                cmd.Parameters.AddWithValue("@dataAtualizacao", DateTime.Now);

                await con.OpenAsync();
                var transaction = con.BeginTransaction();
                try
                {
                    cmd.Transaction = transaction;
                    var idImage = await cmd.ExecuteScalarAsync();

                    transaction.Commit();
                    image.idImage = Convert.ToInt32(idImage);
                    return image.idImage;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    await con.CloseAsync();
                }
                return 0;
            }
        }
    }
}
