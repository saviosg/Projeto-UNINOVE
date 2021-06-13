using Axulus.Data.Model;
using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace Axulus.Data.Repositorio.Sqlite
{
    public class imageRepo
    {
        #region Propriedades
        string connectionString = @"Data Source=Application.db;Cache=Shared";
        #endregion

        #region Metodos Publicos
        public async Task<ImageModel> AdicionarImageAsync(ImageModel image, SqliteConnection con, SqliteTransaction transaction)
        { 
            string comandoSQL = @"insert into image (descricao, image_base64, data_cadastro, data_alteracao)
                values (
                  @descricao, 
                  @imageBase64, 
                  @dataCadastro, 
                  @dataAtualizacao
                );
                select last_insert_rowid();";

            SqliteCommand cmd = new SqliteCommand(comandoSQL, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@descricao", image.descricao);
            cmd.Parameters.AddWithValue("@imageBase64", image.imageData);
            cmd.Parameters.AddWithValue("@dataCadastro", DateTime.Now);
            cmd.Parameters.AddWithValue("@dataAtualizacao", DateTime.Now);

            //await con.OpenAsync();
            cmd.Transaction = transaction;
            var idImage = await cmd.ExecuteScalarAsync();
            image.idImage = Convert.ToInt32(idImage);
            //await con.CloseAsync();
            return image;
            #endregion
        }

        public async Task<int> AdicionarImageUsuarioAsync(ImageModel image)
        {
            var usuario = new UsuarioModel();
            using (SqliteConnection con = new SqliteConnection(connectionString))
            {
                string comandoSQL = @"insert into Image (descricao, image_base64, data_cadastro, data_alteracao)
                    values (
                      @descricao, 
                      @imageBase64, 
                      @dataCadastro, 
                      @dataAtualizacao
                    );
                    select last_insert_rowid();";

                SqliteCommand cmd = new SqliteCommand(comandoSQL, con);
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
