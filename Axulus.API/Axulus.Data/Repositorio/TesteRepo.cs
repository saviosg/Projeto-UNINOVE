using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Axulus.Data.Model;
using Axulus.Model;

namespace Axulus.Data.Repositorio
{
    public class TesteRepo
    {
        string connectionString = @"Data Source=DESKTOP-AN84SVU\SQLEXPRESS;Initial Catalog=axulus_poc; Integrated Security=True";

        public void AdicionarTeste(TesteModel teste)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"Insert into Teste (
                                        id_empresa, 
                                        id_usuario) 
                                    Values(                             
                                        @idEmpresa, 
                                        @idUsuario)";

                SqlCommand cmd = new SqlCommand(comandoSQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@idEmpresa", teste.id_empresa);
                cmd.Parameters.AddWithValue("@idUsuario", teste.id_usuario);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }

        public int EditarTeste(TesteModel teste)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string comandoSQL = @"update Teste set id_empresa = @idEmpresa,
                                               id_usuario = @idUsuario
                                               where id_teste = @idTeste";

                SqlCommand cmd = new SqlCommand(comandoSQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@idTeste", teste.id_teste);
                cmd.Parameters.AddWithValue("@idEmpresa", teste.id_empresa);
                cmd.Parameters.AddWithValue("@idUsuario", teste.id_usuario);


                return cmd.ExecuteNonQuery();
            }

        }

        public TesteModel ObterTestePorId(int id)
        {
            var teste = new TesteModel();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = $@"SELECT [id_teste] 
                                              ,[id_empresa] 
                                              ,[id_usuario]
                                          FROM[axulus_poc].[dbo].[Teste] WHERE id_teste = {id}";

                SqlCommand cmd = new SqlCommand(comandoSQL, con);
                cmd.CommandType = CommandType.Text;

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        teste.id_teste = Convert.ToInt32(reader["id_teste"]);
                        teste.id_empresa = Convert.ToInt32(reader["id_empresa"]);
                        teste.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    }
                }
                cmd.ExecuteNonQuery();
                con.Close();
                return teste;
            }
        }

        public List<TesteModel> ListarTeste()
        {
            var retorno = new List<TesteModel>();
            using SqlConnection con = new SqlConnection(connectionString);
            string comandoSQL = @"SELECT e.nome_empresa, u.nome_usuario FROM Empresa e
                                            JOIN Teste t
                                            ON e.id_empresa = t.id_empresa
                                            JOIN Usuario u
                                            ON u.id_usuario = t.id_usuario";

            SqlCommand cmd = new SqlCommand(comandoSQL, con);
            cmd.CommandType = CommandType.Text;

            con.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    TesteModel testeModel = new TesteModel();
                    testeModel.empresaModel = new EmpresaModel();
                    testeModel.usuarioModel = new UsuarioModel();

                    testeModel.empresaModel.nomeEmpresa = reader.GetString("nome_empresa");
                    testeModel.usuarioModel.nome_usuario = reader.GetString("nome_usuario");
                    retorno.Add(testeModel);

                }
            }
            con.Close();
            return retorno;
        }

        public void ExcluirTeste(int id)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string comandoSQL = @"DELETE Teste WHERE id_teste = @idTeste";

                SqlCommand cmd = new SqlCommand(comandoSQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@idTeste", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }
    }
}
